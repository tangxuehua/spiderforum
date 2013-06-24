using System;

namespace System.Web.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatorAttribute : Attribute
    {
        private Type validatorType;

        public ValidatorAttribute()
        {
        }
        public ValidatorAttribute(Type validatorType)
        {
            this.validatorType = validatorType;
        }

        public Type ValidatorType
        {
            get
            {
               return validatorType;
            }
            set
            {
                validatorType = value;
            }
        }
    }
}
