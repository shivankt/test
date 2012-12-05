using System;

namespace Common.Util.Helpers
{
    public static class AuthorizeHelper
    {
        /// <summary>
        /// The enum type names must match to the class type private field name in the following way:
        /// (enumName) => _Is(enumName)
        /// </summary>
        /// <typeparam name="T1">The enum type</typeparam>
        /// <typeparam name="T2">The class type</typeparam>
        public static void SetAuthorizePoints<T1, T2>(T2 retval,ulong authenticationVal)
        {
            Type enumType = typeof(T1);
            Type classType = typeof(T2);

            string[] enumNames = Enum.GetNames(enumType);

            foreach (string enumName in enumNames)
            {
                int enumVal = (int)Enum.Parse(enumType, enumName);
                bool bitVal = authenticationVal.UtilGetBit(enumVal);

                classType.GetProperty("Is" + enumName).SetValue(retval, bitVal, null);
            }
        }

        /// <summary>
        /// The enum type names must match to the class type private field name in the following way:
        /// (enumName) => _Is(enumName)
        /// </summary>
        /// <typeparam name="T1">The enum type</typeparam>
        /// <typeparam name="T2">The class type</typeparam>
        public static ulong GetAuthorizeValue<T1, T2>(T2 newAuth)
        {
            Type enumType = typeof(T1);
            Type classType = typeof(T2);

            string[] enumNames = Enum.GetNames(enumType);
            ulong retval = 0;

            foreach (string enumName in enumNames)
            {
                int enumVal = (int)Enum.Parse(enumType, enumName);
                bool bitVal = (bool)classType.GetProperty("Is" + enumName).GetValue(newAuth, null);

                retval = retval.UtilSetBit(bitVal, enumVal);
            }

            return retval;
        }
    }
}
