using Microsoft.Security.Application;
namespace System.Web.Core
{
    public abstract class Property
    {
        protected object originalValue;
        protected object objectValue;
        protected bool isValueInitialized = false;
        private Condition condition = null;

        internal virtual object ObjectValue
        {
            get
            {
                return objectValue;
            }
            set
            {
                if (!isValueInitialized)
                {
                    originalValue = value;
                    isValueInitialized = true;
                }
                objectValue = value;
            }
        }
        public virtual bool IsDirty
        {
            get
            {
                return !Equals(originalValue, objectValue);
            }
        }
        public Condition Condition
        {
            get
            {
                return condition;
            }
            set
            {
                condition = value;
            }
        }

        public override string ToString()
        {
            if (objectValue != null)
            {
                return objectValue.ToString();
            }
            return null;
        }
        protected internal void SetDefaultValue(object defaultValue)
        {
            this.objectValue = defaultValue;
        }
        public virtual IDataValidator GetDefaultValidator()
        {
            return null;
        }
    }
    public class Property<T> : Property
    {
        public Property()
        {
            SetDefaultValue(Globals.ChangeType<T>(objectValue));
        }
        public T Value
        {
            get
            {
                return Globals.ChangeType<T>(objectValue);
            }
            set
            {
                object newValue = value;
                if (newValue != null && typeof(T) == typeof(string))
                {
                    //newValue = AntiXss.HtmlEncode(newValue.ToString());
                    newValue = newValue.ToString();
                }
                if (!isValueInitialized)
                {
                    originalValue = newValue;
                    objectValue = newValue;
                    isValueInitialized = true;
                }
                else
                {
                    T oldValue = Value;
                    objectValue = newValue;
                    if (!Equals(oldValue, newValue))
                    {
                        if (PropertyValueChanged != null)
                        {
                            PropertyValueChanged(oldValue, Globals.ChangeType<T>(newValue));
                        }
                    }
                }
            }
        }

        public event PropertyValueChangedEventHandler<T> PropertyValueChanged;
    }

    public delegate void PropertyValueChangedEventHandler<T>(T oldValue, T newValue);
}
