using System.Collections.Generic;
using System.Linq;
using Vinyl.Utils;

namespace Vinyl.Models
{
    public sealed class Genre : TypesafeEnum<string>
    {
        public static Genre MetalCore { get; } = new Genre("MetalCore", "Metal core");
        public static Genre EmoCore { get; } = new Genre("EmoCore", "Emo core");
        public static Genre MelodicDeathMetal { get; } = new Genre("MelodicDeathMetal", "Melodic death metal");

        private Genre(string key, string description) : base(key, description) { }

        public static IEnumerable<Genre> GetAll()
        {
            return new[] { MetalCore, EmoCore, MelodicDeathMetal };
        }

        public static Genre FromKey(string key)
        {
            return GetAll().First(i => i.Key.Equals(key));
        }
    }
}
