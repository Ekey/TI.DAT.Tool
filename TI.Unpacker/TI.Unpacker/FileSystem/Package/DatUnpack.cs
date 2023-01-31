using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TI.Unpacker
{
    class DatUnpack
    {
        private static List<DatEntry> m_EntryTable = new List<DatEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            DatCipher.iInitKey();
            using (FileStream TDatStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TDatStream.ReadBytes(16);
                lpHeader = DatCipher.iDecrypt(lpHeader);

                var m_Header = new DatHeader();
                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwTableSize = THeaderReader.ReadInt32();
                    m_Header.dwArchiveSize = THeaderReader.ReadUInt32();
                    m_Header.dwReserved = THeaderReader.ReadInt32();

                    if (m_Header.dwMagic != 0x4B434150)
                    {
                        throw new Exception("[ERROR]: Invalid magic of DAT archive file!");
                    }

                    THeaderReader.Dispose();
                }

                var lpEntryTable = TDatStream.ReadBytes(m_Header.dwTableSize);
                lpEntryTable = DatCipher.iDecrypt(lpEntryTable);

                m_EntryTable.Clear();

                var m_TableHeader = new DatTableHeader();
                using (var TEntryReader = new MemoryStream(lpEntryTable))
                {
                    m_TableHeader.dwTotalFiles = TEntryReader.ReadInt32();
                    m_TableHeader.dwUnknown1 = TEntryReader.ReadInt32();
                    m_TableHeader.dwUnknown2 = TEntryReader.ReadInt32();
                    m_TableHeader.dwUnknown3 = TEntryReader.ReadInt32();

                    for (Int32 i = 0; i < m_TableHeader.dwTotalFiles; i++)
                    {
                        var m_Entry = new DatEntry();

                        m_Entry.m_FileName = Encoding.ASCII.GetString(TEntryReader.ReadBytes(104)).TrimEnd('\0');
                        m_Entry.dwUnknown = TEntryReader.ReadInt32();
                        m_Entry.dwCrc32 = TEntryReader.ReadUInt32();
                        m_Entry.dwOffset = TEntryReader.ReadUInt32();
                        m_Entry.dwSize = TEntryReader.ReadInt32();
                        m_Entry.dwReserved = TEntryReader.ReadInt64();

                        m_EntryTable.Add(m_Entry);
                    }

                    TEntryReader.Dispose();
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TDatStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    var lpBuffer = TDatStream.ReadBytes(m_Entry.dwSize);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TDatStream.Dispose();
            }
        }
    }
}
