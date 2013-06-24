using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace System.Web.Core
{
    public class AjaxManager
    {
        #region Public Properties

        public static string CallBackType
        {
            get
            {
                return HttpContext.Current.Request.Form["Ajax_CallBackType"];
            }
        }
        public static string CallBackId
        {
            get
            {
                return HttpContext.Current.Request.Form["Ajax_CallBackId"];
            }
        }
        public static string CallBackMethod
        {
            get
            {
                return HttpContext.Current.Request.Form["Ajax_CallBackMethod"];
            }
        }
        public static bool IsCallBack
        {
            get
            {
                return CallBackType != null;
            }
        }

        #endregion

        #region Public Methods

        public static void Register(Page page)
        {
            Register(page, page.GetType().FullName, true, AjaxDebug.None);
        }
        public static void Register(Page page, string prefix)
        {
            Register(page, prefix, true, AjaxDebug.None);
        }
        public static void Register(Page page, AjaxDebug debug)
        {
            Register(page, page.GetType().FullName, true, debug);
        }
        public static void Register(Page page, string prefix, AjaxDebug debug)
        {
            Register(page, prefix, true, debug);
        }
        public static void Register(Control control)
        {
            Register(control, control.GetType().FullName, true, AjaxDebug.None);
        }
        public static void Register(Control control, string prefix)
        {
            Register(control, prefix, true, AjaxDebug.None);
        }
        public static void Register(Control control, AjaxDebug debug)
        {
            Register(control, control.GetType().FullName, true, debug);
        }
        public static void Register(Control control, string prefix, bool requireId, AjaxDebug debug)
        {
            HttpContext context = HttpContext.Current;
            Type type = control.GetType();
            StringBuilder controlScript = new StringBuilder();
            controlScript.Append("<script language=\"javascript\" type=\"text/javascript\">\n");
            string[] prefixParts = prefix.Split('.', '+');
            controlScript.AppendFormat("var {0} = {{\n", prefixParts[0]);
            for (int i = 1; i < prefixParts.Length; ++i)
            {
                controlScript.AppendFormat("\"{0}\": {{\n", prefixParts[i]);
            }
            int methodCount = 0;
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                object[] attributes = methodInfo.GetCustomAttributes(typeof(AjaxMethodAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    AjaxMethodAttribute methodAttribute = attributes[0] as AjaxMethodAttribute;
                    string clientSideName = methodAttribute.ClientSideName != null ? methodAttribute.ClientSideName : methodInfo.Name;
                    ++methodCount;
                    controlScript.AppendFormat("\"{0}\":function(", clientSideName);
                    if (requireId)
                    {
                        controlScript.AppendFormat("id,");
                    }
                    int paramCount = methodInfo.GetParameters().Length;

                    for (int i = 0; i < paramCount; i++)
                    {
                        controlScript.Append("p");
                        controlScript.Append(i.ToString());
                        controlScript.Append(",");
                    }

                    controlScript.AppendFormat(
                        "cb){{return Ajax_CallBack('{0}',{1},'{2}',[",
                        type.FullName,
                        requireId ? "id" : "null",
                        methodInfo.Name);
                    
                    for (int i = 0; i < paramCount; i++)
                    {
                        if (i > 0)
                        {
                            controlScript.Append(",");
                        }
                        controlScript.Append("p");
                        controlScript.Append(i.ToString());
                    }

                    controlScript.AppendFormat("],cb,{0},{1},{2},{3});}}",
                        (debug & AjaxDebug.RequestText) == AjaxDebug.RequestText ? "1" : "0",
                        (debug & AjaxDebug.ResponseText) == AjaxDebug.ResponseText ? "1" : "0",
                        (debug & AjaxDebug.Errors) == AjaxDebug.Errors ? "1" : "0",
                        (methodAttribute.IncludeControlValuesWithCallBack ? "1" : "0"));

                    controlScript.Append(",\n");
                }
            }

            if (methodCount == 0)
            {
                return;
            }

            controlScript.Length -= 2;
            for (int i = 0; i < prefixParts.Length; ++i)
            {
                controlScript.Append("}");
            }
            controlScript.Append(";\n</script>");

            control.Page.ClientScript.RegisterClientScriptBlock(control.Page.GetType(), control.UniqueID, controlScript.ToString());
            control.PreRender += new EventHandler(OnPreRender);
        }
        public static void WriteResult(HttpResponse resp, object val, string error)
        {
            resp.ContentType = "application/x-javascript";
            resp.Cache.SetCacheability(HttpCacheability.NoCache);
            StringBuilder sb = new StringBuilder();
            try
            {
                WriteValueAndError(sb, val, error);
            }
            catch (Exception ex)
            {
                // If an exception was thrown while formatting the
                // result value, we need to discard whatever was
                // written and start over with nothing but the error
                // message.
                sb.Length = 0;
                WriteValueAndError(sb, null, ex.Message);
            }
            resp.Write(sb.ToString());
        }

        #endregion

        #region Private Methods

        private static void OnPreRender(object s, EventArgs e)
        {
            if (IsCallBack)
            {
                Control control = s as Control;

                MethodInfo methodInfo = null;
                if (control != null)
                {
                    if (control.GetType().FullName == CallBackType || control is Page)
                    {
                        if (control is Page || control.ClientID == CallBackId)
                        {
                            methodInfo = FindTargetMethod(control);
                        }
                    }
                }
                object val = null;
                string error = null;
                HttpResponse resp = HttpContext.Current.Response;
                if (methodInfo != null)
                {
                    try
                    {
                        object[] parameters = ConvertParameters(methodInfo, HttpContext.Current.Request);
                        val = InvokeMethod(control, methodInfo, parameters);
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                    WriteResult(resp, val, error);
                    resp.End();
                }
            }
        }
        private static MethodInfo FindTargetMethod(Control control)
        {
            Type type = control.GetType();
            string methodName = CallBackMethod;
            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo != null)
            {
                object[] methodAttributes = methodInfo.GetCustomAttributes(typeof(AjaxMethodAttribute), true);
                if (methodAttributes.Length > 0)
                {
                    return methodInfo;
                }
            }
            return null;
        }
        private static object[] ConvertParameters(MethodInfo methodInfo, HttpRequest req)
        {
            object[] parameters = new object[methodInfo.GetParameters().Length];
            int i = 0;
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
            {
                object param = null;
                string paramValue = req.Form["Ajax_CallBackArgument" + i];
                if (paramValue != null)
                {
                    if (paramInfo.ParameterType.IsArray)
                    {
                        Type type = paramInfo.ParameterType.GetElementType();
                        string[] splits = paramValue.Split(',');
                        Array array = Array.CreateInstance(type, splits.Length);
                        for (int index = 0; index < splits.Length; index++)
                        {
                            array.SetValue(Convert.ChangeType(splits[index], type), index);
                        }
                        param = array;
                    }
                    else
                    {
                        param = Convert.ChangeType(paramValue, paramInfo.ParameterType);
                    }
                }
                parameters[i] = param;
                ++i;
            }
            return parameters;
        }
        private static object InvokeMethod(Control control, MethodInfo methodInfo, object[] parameters)
        {
            object val = null;
            try
            {
                val = methodInfo.Invoke(control, parameters);
            }
            catch (TargetInvocationException ex)
            {
                // TargetInvocationExceptions should have the actual
                // exception the method threw in its InnerException
                // property.
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
            return val;
        }
        private static void WriteValueAndError(StringBuilder sb, object val, string error)
        {
            sb.Append("{\"value\":");
            WriteValue(sb, val);
            sb.Append(",\"error\":");
            WriteValue(sb, error);
            sb.Append("}");
        }
        private static void WriteValue(StringBuilder sb, object val)
        {
            if (val == null || val == System.DBNull.Value)
            {
                sb.Append("null");
            }
            else if (val is string || val is Guid)
            {
                WriteString(sb, val.ToString());
            }
            else if (val is bool)
            {
                sb.Append(val.ToString().ToLower());
            }
            else if (val is double ||
                val is float ||
                val is long ||
                val is int ||
                val is short ||
                val is byte ||
                val is decimal)
            {
                sb.Append(val);
            }
            else if (val is DateTime)
            {
                sb.Append("new Date(\"");
                sb.Append(((DateTime)val).ToString("MMMM, d yyyy HH:mm:ss", new CultureInfo("en-US", false).DateTimeFormat));
                sb.Append("\")");
            }
            else if (val is DataSet)
            {
                WriteDataSet(sb, val as DataSet);
            }
            else if (val is DataTable)
            {
                WriteDataTable(sb, val as DataTable);
            }                        
            else if (val is DataRow)
            {
                WriteDataRow(sb, val as DataRow);
            }
            else if (val is IEnumerable)
            {
                WriteEnumerable(sb, val as IEnumerable);
            }
            else
            {
                WriteSerializable(sb, val);
            }
        }
        private static void WriteString(StringBuilder sb, string s)
        {
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");
        }
        private static void WriteDataSet(StringBuilder sb, DataSet ds)
        {
            sb.Append("{\"Tables\":{");
            foreach (DataTable table in ds.Tables)
            {
                sb.AppendFormat("\"{0}\":", table.TableName);
                WriteDataTable(sb, table);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (ds.Tables.Count > 0) 
            {
                --sb.Length;
            }
            sb.Append("}}");
        }
        private static void WriteDataTable(StringBuilder sb, DataTable table)
        {
            sb.Append("{\"Rows\":[");
            foreach (DataRow row in table.Rows)
            {
                WriteDataRow(sb, row);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (table.Rows.Count > 0) 
            {
                --sb.Length;
            }
            sb.Append("]}");
        }
        private static void WriteDataRow(StringBuilder sb, DataRow row)
        {
            sb.Append("{");
            foreach (DataColumn column in row.Table.Columns)
            {
                sb.AppendFormat("\"{0}\":", column.ColumnName);
                WriteValue(sb, row[column]);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (row.Table.Columns.Count > 0) 
            {
                --sb.Length;
            }
            sb.Append("}");
        }
        private static void WriteEnumerable(StringBuilder sb, IEnumerable e)
        {
            bool hasItems = false;
            sb.Append("[");
            foreach (object val in e)
            {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("]");
        }
        private static void WriteSerializable(StringBuilder sb, object o)
        {
            System.Reflection.MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            sb.Append("{");
            bool hasMembers = false;
            foreach (System.Reflection.MemberInfo member in members)
            {
                bool hasValue = false;
                object val = null;
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    val = field.GetValue(o);
                    hasValue = true;
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        val = property.GetValue(o, null);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    sb.Append("\"");
                    sb.Append(member.Name);
                    sb.Append("\":");
                    WriteValue(sb, val);
                    sb.Append(",");
                    hasMembers = true;
                }
            }
            if (hasMembers)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxMethodAttribute : Attribute
    {
        public AjaxMethodAttribute()
        {
            _includeControlValuesWithCallBack = false;
        }

        private string _clientSideName;
        private bool _includeControlValuesWithCallBack = false;

        public string ClientSideName
        {
            get { return _clientSideName; }
            set { _clientSideName = value; }
        }
        public bool IncludeControlValuesWithCallBack
        {
            get { return _includeControlValuesWithCallBack; }
            set { _includeControlValuesWithCallBack = value; }
        }
    }

    [Flags]
    public enum AjaxDebug
    {
        None = 0,
        RequestText = 1,
        ResponseText = 2,
        Errors = 4,
        All = 7
    }
}