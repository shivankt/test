
namespace Common.Util
{
    public static class ReflectionExtensions
    {
        public static T UtilGetPropetyValue<T>(this object obj, string proprtyName)
        {
            var prop = obj.GetType().GetProperty(proprtyName);
            if (prop != null)
            {
                return (T)prop.GetValue(obj, null);
            }
            return default(T);
        }
    }
}
