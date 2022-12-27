using Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core
{
    public static class ModMetadataReader
    {
        public static byte[] StrByteBuffer = new byte[4096];

        public static Metadata LoadMetadata(string filename)
        {
            Metadata header = null;
            FileStream fileStream;
            try
            {
                fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            BinaryReader file = new BinaryReader(fileStream);
            try
            {
                int fileVersion = file.ReadInt32();
                if (fileVersion > 15)
                    header = MountMetadata(file, fileVersion);
            }
            catch (EndOfStreamException)
            {
            }
            file.Close();
            fileStream.Close();
            return header;
        }

        public static Metadata MountMetadata(BinaryReader file, int fileVersion)
        {
            Metadata header = new Metadata();
            long headerEnd = 0;
            if (fileVersion > 16)
            {
                headerEnd = file.ReadUInt32();
                headerEnd += file.BaseStream.Position;
            }
            header.Version = file.ReadInt32();
            header.Author = Read(file);
            header.Description = Read(file);
            header.Dependencies = new List<string>(Read(file).Split(',').Where(m => !Constants.SkippableMods.Contains(m.ToLower())));
            header.Referenced = new List<string>(Read(file).Split(',').Where(m => !Constants.SkippableMods.Contains(m.ToLower())));
            if (header.Dependencies.Count == 1 && header.Dependencies[0] == "")
                header.Dependencies.Clear();
            if (header.Referenced.Count == 1 && header.Referenced[0] == "")
                header.Referenced.Clear();
            if (fileVersion > 16)
                file.BaseStream.Seek(headerEnd, SeekOrigin.Begin);
            return header;
        }

        public static string Read(BinaryReader file)
        {
            int count = file.ReadInt32();
            if (count <= 0)
                return string.Empty;
            if (count > StrByteBuffer.Length)
                Array.Resize<byte>(ref StrByteBuffer, StrByteBuffer.Length * 2);
            file.Read(StrByteBuffer, 0, count);
            return Encoding.UTF8.GetString(StrByteBuffer, 0, count);
        }
    }
}