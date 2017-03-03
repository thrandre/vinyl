using System;
using System.ComponentModel;
using System.Globalization;

namespace Vinyl.Utils
{
    public class BoxedTypeConverter : System.ComponentModel.TypeConverter
    {
        protected Type Type { get; }

        public BoxedTypeConverter(Type type)
        {
            Type = type;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return TypeConverterInstance.Current.CanConvert(Type) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return TypeConverterInstance.Current.ConvertFrom(Type, value);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return TypeConverterInstance.Current.ConvertTo(Type, value);
        }
    }
}