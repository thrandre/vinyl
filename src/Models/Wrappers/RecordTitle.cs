using Vinyl.Utils;

namespace Vinyl.Models.Wrappers
{
    public class RecordTitle : ValueWrapper<string>
    {
        public RecordTitle(string value) : base(value) { }
    }
}