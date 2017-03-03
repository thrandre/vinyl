using System.Collections.Generic;
using System.Linq;
using Vinyl.Utils;

namespace Vinyl.Models
{
    public sealed class Genre : TypesafeEnum<string>
    {
        public static Genre MetalCore { get; } = new Genre("Metalcore", "Metal core");
        public static Genre EmoCore { get; } = new Genre("Emocore", "Emo core");
        public static Genre MelodicDeathMetal { get; } = new Genre("MelodicDeathMetal", "Melodic death metal");
        public static Genre CelticPunk { get; } = new Genre("CelticPunk", "Celtic Punk");
        public static Genre Pop { get; } = new Genre("Pop", "Pop");
        public static Genre SingerSongwriter { get; } = new Genre("SingerSongwriter", "Singer/Songwriter");
        public static Genre Melancholia { get; } = new Genre("Melancholia", "Melancholia");
        public static Genre HeavyMetal { get; } = new Genre("HeavyMetal", "Heavy Metal");
        public static Genre BlackNRoll { get; } = new Genre("BlackNRoll", "Black 'n' roll");

        private Genre(string value, string description) : base(value, description) { }

        public static IEnumerable<Genre> GetAll()
        {
            return new[]
            {
                MetalCore,
                EmoCore,
                MelodicDeathMetal,
                CelticPunk,
                Pop,
                SingerSongwriter,
                Melancholia,
                BlackNRoll,
                HeavyMetal
            };
        }

        public static Genre FromKey(string key)
        {
            return GetAll().First(i => i.Value.Equals(key));
        }
    }
}
