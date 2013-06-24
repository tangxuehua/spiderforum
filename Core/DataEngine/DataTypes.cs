using System.Collections.Generic;

namespace System.Web.Core
{
    public class IntType : Property<int>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new PositiveIntValidator();
        }
    }
    public class NIntType : Property<int?>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new PositiveNullableIntValidator();
        }
    }
    public class LongType : Property<long>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new PositiveLongValidator();
        }
    }
    public class NLongType : Property<long?>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new PositiveNullableLongValidator();
        }
    }
    public class DoubleType : Property<double>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new PositiveDoubleValidator();
        }
    }
    public class NDoubleType : Property<double?>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new PositiveNullableDoubleValidator();
        }
    }
    public class StringType : Property<string>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new StringValidator();
        }
    }
    public class DateTimeType : Property<DateTime>
    {
        public DateTimeType()
        {
            this.Value = new DateTime(1753, 1, 1);
        }
        public override IDataValidator GetDefaultValidator()
        {
            return new DateTimeValidator();
        }
    }
    public class NDateTimeType : Property<DateTime?>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new NullableDateTimeValidator();
        }
    }
    public class GuidType : Property<Guid>
    {
        public override IDataValidator GetDefaultValidator()
        {
            return new GuidValidator();
        }
    }
    public class ByteType : Property<byte[]>
    {
    }
}
