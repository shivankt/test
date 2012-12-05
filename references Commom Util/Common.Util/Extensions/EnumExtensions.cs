using System;
using System.Collections.Generic;

namespace Common.Util
{
    public static class ExtensionMethods
    {
        public static string UtilGetEnumAsString(this Enum obj, bool isGetFull = true)
        {
            try
            {
                var result = obj.GetType().GetField(obj.ToString()).GetCustomAttributes(typeof(UtilEnumStringValueAttribute), false);
                if (result != null && result.Length > 0)
                {
                    if (isGetFull)
                        return (result[0] as UtilEnumStringValueAttribute).LargeDescription;
                    else
                        return (result[0] as UtilEnumStringValueAttribute).SmallDescription;
                }
                else
                    return obj.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string UtilGetEnumShortString<T>(this Enum obj)
        {
            return (Enum.Parse(typeof(T), obj.ToString()) as Enum).UtilGetEnumAsString(false);
        }

        public static string UtilGetDDLOptions<T>(this Enum obj)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (Enum item in Enum.GetValues(obj.GetType()))
            {
                string val = Convert.ToInt32(item).ToString();
                string txt = item.UtilGetEnumAsString(true);

                dict.Add(val, txt);
            }

            return dict.UtilGetDDLOptions(obj.ToString(), false);
        }

        public static T ParseToEnum<T>(this object val)
        {
            return (T)Enum.Parse(typeof(T), val.ToString());
        }
    }
}
