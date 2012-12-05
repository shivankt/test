using System;

namespace Common.Util
{
    public static class Bitwise
    {
        /// <param name="bitNumber">Zero-based bit location, 0-63</param>
        public static ulong UtilSetBit(this ulong objVal, bool bitVal, int bitNumber)
        {
            ulong retval = objVal;
            ulong singleBit = Convert.ToUInt64(Math.Pow(2, bitNumber));

            if (bitVal)
            {
                retval = objVal | singleBit;
            }
            else
            {
                ulong maxUint = ulong.MaxValue;
                retval = retval & (singleBit ^ maxUint);
            }

            return retval;
        }

        public static bool UtilGetBit(this ulong objVal, int bitNumber)
        {
            bool retval = false;
            ulong singleBit = Convert.ToUInt32(Math.Pow(2, bitNumber));

            retval = (objVal & singleBit) > 0;

            return retval;
        }
    }
}
