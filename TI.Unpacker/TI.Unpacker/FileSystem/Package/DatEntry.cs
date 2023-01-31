using System;

namespace TI.Unpacker
{
    class DatEntry
    {
        public String m_FileName { get; set; } // (104 bytes???)
        public Int32 dwUnknown { get; set; }
        public UInt32 dwCrc32 { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int64 dwReserved { get; set; }
    }
}
