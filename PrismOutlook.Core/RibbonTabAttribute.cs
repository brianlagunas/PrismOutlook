using System;

namespace PrismOutlook.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependentViewAttribute : Attribute
    {
        public string Region { get; set; }

        public Type Type { get; set; }

        public DependentViewAttribute(string region, Type type)
        {
            if (region is null)
                throw new ArgumentNullException(nameof(region));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Region = region;
            Type = type;
        }
    }
}
