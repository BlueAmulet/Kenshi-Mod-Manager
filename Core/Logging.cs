using MMDHelpers.CSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class Logging
    {
        private static readonly object sync = new object();
        public static void Write(string filename, params string[] texts)
        {
            if (texts.Length == 0) return;
            lock (sync)
            {
                filename
                    .ToCurrentPath()
                    .WriteToFile(new List<string> { string.Concat(texts.Select(c => $"{DateTime.Now} - {c} {Environment.NewLine}")) });
            }
        }

        public static void Write(string filename, IEnumerable<string> texts)
        {
            if (!texts.Any()) return;
            lock (sync)
            {
                filename
                    .ToCurrentPath()
                    .WriteToFile(new List<string> { string.Concat(texts.Select(c => $"{DateTime.Now} - {c} {Environment.NewLine}")) });
            }
        }

        public static void Write(string filename, Exception ex)
        {
            lock (sync)
            {
                filename
                    .ToCurrentPath()
                    .WriteToFile(new List<string> { $"{DateTime.Now} -  {ex.Message}.{Environment.NewLine}", $"{ex.StackTrace} {Environment.NewLine}" });
            }
        }
    }
}
