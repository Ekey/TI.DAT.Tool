using System;

namespace TI.Unpacker
{
    class DatCipher
    {
        class RC4_Ctx
        {
            public static Byte[] m_State = new Byte[256];
            public static Byte x, y = 0;
        }

        private static Byte[] m_Key = new Byte[] {
            0x00, 0x31, 0xBC, 0xBA, 0x31, 0x21, 0xAB, 0xCB, 0x00, 0x43, 0x6B, 0xBA, 0x31, 0x1F, 0x9B, 0xCC,
            0x13, 0x34, 0xCB, 0x7A, 0x00, 0x8F, 0xBB, 0xCC, 0x28, 0x37, 0xB6, 0xB5, 0x34, 0x23, 0xA2, 0xC1
        };

        public static void iInitKey()
        {
            Int32 k = 0;

            for (Int32 i = 0; i < 256; ++i)
            {
                RC4_Ctx.m_State[i] = (Byte)(-1 - i);
            }

            for (Int32 i = 0; i < 2; i++)
            {
                for (Int32 j = 0; j < 256; j++)
                {
                    k = (m_Key[j % m_Key.Length] + RC4_Ctx.m_State[j] + k) % 256;
                    RC4_Ctx.x = RC4_Ctx.m_State[j];
                    RC4_Ctx.m_State[j] = RC4_Ctx.m_State[k];
                    RC4_Ctx.m_State[k] = RC4_Ctx.x;
                }
            }

            RC4_Ctx.x = 0;
            RC4_Ctx.y = 0;
        }

        public static Byte[] iDecrypt(Byte[] lpBuffer)
        {
            Int32 x = RC4_Ctx.x;
            Int32 y = RC4_Ctx.y;

            for (Int32 i = 0; i < lpBuffer.Length; i++)
            {
                Int32 _x = 0;
                Int32 _y = 0;

                x = x + 1;
                _x = RC4_Ctx.m_State[x % 256];
                y = y + _x;
                _y = RC4_Ctx.m_State[y % 256];

                RC4_Ctx.m_State[x % 256] = (Byte)_y;
                RC4_Ctx.m_State[y % 256] = (Byte)_x;

                lpBuffer[i] ^= RC4_Ctx.m_State[(_y + _x) % 256];
            }

            RC4_Ctx.x = (Byte)x;
            RC4_Ctx.y = (Byte)y;

            return lpBuffer;
        }
    }
}
