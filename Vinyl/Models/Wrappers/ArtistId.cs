using Vinyl.Utils;

namespace Vinyl.Models.Wrappers
{
    public class ArtistId : ValueWrapper<int>
    {
        public ArtistId(int value) : base(value) { }
    }
}