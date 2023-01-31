using System;

namespace TI.Unpacker
{
    class DatHeader
    {
        public UInt32 dwMagic { get; set; } // 0x4B434150 (PACK)
        public Int32 dwTableSize { get; set; }
        public UInt32 dwArchiveSize { get; set; }
        public Int32 dwReserved { get; set; }
    }

    class DatTableHeader
    {
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwUnknown1 { get; set; } // 64
        public Int32 dwUnknown2 { get; set; } // 16
        public Int32 dwUnknown3 { get; set; } // 128
    }
}
