using Vinyl.Utils;

namespace Vinyl.Models.Wrappers
{
    public class ArtistName : ValueWrapper<string>
    {
        public ArtistName(string value) : base(value) { }
    }
}