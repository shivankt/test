using System;

namespace Common.Util
{
    /*Author: Ankit*/
    public class UtilEnumStringValueAttribute : Attribute
    {
        public string LargeDescription { get; protected set; }

        public string SmallDescription { get; protected set; }

        public UtilEnumStringValueAttribute(string largeDescription)
        {
            LargeDescription = largeDescription;
        }

        public UtilEnumStringValueAttribute(string largeDescription, string smallDescription)
        {
            LargeDescription = largeDescription;
            SmallDescription = smallDescription;
        }
    }
}
