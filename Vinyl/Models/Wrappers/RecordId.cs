using Vinyl.Utils;

namespace Vinyl.Models.Wrappers
{
    public class RecordId : ValueWrapper<int>
    {
        public RecordId(int value) : base(value) { }
    }
}