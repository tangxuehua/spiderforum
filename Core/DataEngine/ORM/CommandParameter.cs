using System.Collections.Generic;
using System.Data;

namespace System.Web.Core
{
    public class CommandParameter
    {
        #region Private Members

        private string propertyPath = null;
        private string propertyName = null;
        private string parameterName = null;
        private string dbTypeHint = null;
        private ParameterDirection parameterDirection = ParameterDirection.Input;
        private CommandParameter parent = null;
        private List<CommandParameter> childs = new List<CommandParameter>();

        #endregion

        #region Constructors

        public CommandParameter()
        {
        }
        public CommandParameter(string propertyName, string parameterName, string dbTypeHint, string parameterDirection)
        {
            this.propertyName = propertyName;
            this.parameterName = parameterName;
            this.dbTypeHint = dbTypeHint;
            this.ParamDirection = parameterDirection;
        }

        #endregion

        #region Public Properties

        public string PropertyPath
        {
            get
            {
                return propertyPath;
            }
            set
            {
                propertyPath = value;
            }
        }
        public string PropertyName
        {
            get
            {
                return propertyName;
            }
            set
            {
                propertyName = value;
            }
        }
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                parameterName = value;
            }
        }
        public string DbTypeHint
        {
            get
            {
                return dbTypeHint;
            }
            set
            {
                dbTypeHint = value;
            }
        }
        public string ParamDirection
        {
            get
            {
                return parameterDirection.ToString();
            }
            set
            {
                if (value == "Input")
                {
                    parameterDirection = ParameterDirection.Input;
                }
                else if (value == "InputOutput")
                {
                    parameterDirection = ParameterDirection.InputOutput;
                }
                else if (value == "Output")
                {
                    parameterDirection = ParameterDirection.Output;
                }
                else
                {
                    parameterDirection = ParameterDirection.ReturnValue;
                }
            }
        }
        public ParameterDirection RealParameterDirection
        {
            get
            {
                return parameterDirection;
            }
        }
        public CommandParameter Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }
        public List<CommandParameter> Childs
        {
            get
            {
                return childs;
            }
        }

        #endregion
    }
}
