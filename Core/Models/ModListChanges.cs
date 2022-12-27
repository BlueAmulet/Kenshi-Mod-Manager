using System.Collections.Concurrent;

namespace Core.Models
{
    public class ModListChanges
    {
        public ConcurrentBag<string> Mod { get; set; }
        public ConcurrentQueue<GameChange> ChangeList { get; set; }
    }
}