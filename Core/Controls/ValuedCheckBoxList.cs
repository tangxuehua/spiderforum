using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace System.Web.Core
{
    public class ValuedCheckBoxList : CheckBoxList, IValuedControl, IRepeatInfoUser
    {
        public string Value
        {
            get 
            {
                if (Page.IsPostBack || AjaxManager.IsCallBack)
                {
                    return Globals.GetControlValue(this);
                }
                else
                {
                    List<string> valueList = new List<string>();
                    foreach (ListItem item in Items)
                    {
                        if (item.Selected)
                        {
                            valueList.Add(item.Value);
                        }
                    }
                    if (valueList.Count > 0)
                    {
                        return string.Join(",", valueList.ToArray());
                    }
                    return null;
                }
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] subValueList = value.Split(',');
                    foreach (string subValue in subValueList)
                    {
                        foreach (ListItem item in Items)
                        {
                            if (item.Value == subValue)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        void IRepeatInfoUser.RenderItem(ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer)
        {
            string clientID = UniqueID + this.ClientIDSeparator + repeatIndex.ToString(NumberFormatInfo.InvariantInfo);

            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "checkbox");
            writer.WriteAttribute("name", UniqueID + this.IdSeparator + repeatIndex.ToString(NumberFormatInfo.InvariantInfo));
            writer.WriteAttribute("id", clientID);
            writer.WriteAttribute("value", Items[repeatIndex].Value);
            if (Items[repeatIndex].Selected)
            {
                writer.WriteAttribute("checked", "checked");
            }

            AttributeCollection attrs = Items[repeatIndex].Attributes;
            foreach (string key in attrs.Keys)
            {
                writer.WriteAttribute(key, attrs[key]);
            }

            writer.Write("/>");
            writer.Write("<label for='" + clientID + "'>");
            writer.Write(Items[repeatIndex].Text);
            writer.Write("</label>"); 
        }
    }
}
