using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Util
{
    public static class StreamExtension
    {
        public static byte[] GetByte(this System.IO.Stream stream)
        {
            return GetData(stream);
        }

        public static byte[] GetByte(this System.IO.MemoryStream stream)
        {
            return GetData(stream);
        }

        static byte[] GetData(System.IO.Stream st)
        {
            byte[] data = null;
            if (st.CanRead)
            {
                int len = (int)st.Length;
                data = new byte[len];
                st.Read(data, 0, len);
            }
            return data;
        }
    }
}
