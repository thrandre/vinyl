using System.Collections.Generic;
using System.Linq;
using Vinyl.Data.Contexts;
using Vinyl.Data.Entities;
using Artist = Vinyl.Data.Entities.Artist;
using Genre = Vinyl.Models.Genre;
using Record = Vinyl.Data.Entities.Record;

namespace Vinyl.Data
{
    public static class DbInitializer
    {
        public static void Initialize(VinylContext context)
        {
            context.Database.EnsureCreated();

            if (context.Records.Any())
            {
                return;
            }

            var bringMeTheHorizon = new Artist
            {
                Name = "Bring Me The Horizon"
            };

            var parkwayDrive = new Artist
            {
                Name = "Parkway Drive"
            };

            var architects = new Artist
            {
                Name = "Architects"
            };

            var volbeat = new Artist
            {
                Name = "Volbeat"
            };

            var thePogues = new Artist
            {
                Name = "The Pogues"
            };

            var kvelertak = new Artist
            {
                Name = "Kvelertak"
            };

            var emoCore = new Entities.Genre
            {
                Key = Genre.EmoCore
            };

            var metalCore = new Entities.Genre
            {
                Key = Genre.MetalCore
            };

            var melodicDeathMetal = new Entities.Genre
            {
                Key = Genre.MelodicDeathMetal
            };

            var celticPunk = new Entities.Genre
            {
                Key = Genre.CelticPunk
            };

            var pop = new Entities.Genre
            {
                Key = Genre.Pop
            };

            var singerSongwriter = new Entities.Genre
            {
                Key = Genre.SingerSongwriter
            };

            var melancholia = new Entities.Genre
            {
                Key = Genre.Melancholia
            };

            var heavyMetal = new Entities.Genre
            {
                Key = Genre.HeavyMetal
            };

            var blackNRoll = new Entities.Genre
            {
                Key = Genre.BlackNRoll
            };

            context.Genres.AddRange
            (
                emoCore, 
                metalCore, 
                melodicDeathMetal,
                celticPunk,
                pop,
                singerSongwriter,
                melancholia,
                heavyMetal
            );

            context.Artists.AddRange
            (
                bringMeTheHorizon,
                parkwayDrive,
                architects,
                volbeat,
                thePogues,
                kvelertak
            );

            context.SaveChanges();

            var thatsTheSpirit = new Record
            {
                Artist = bringMeTheHorizon,
                Title = "That's The Spirit",
                ReleaseYear = 2015
            };

            thatsTheSpirit.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = emoCore,
                    Record = thatsTheSpirit
                }
            };

            var ire = new Record
            {
                Artist = parkwayDrive,
                Title = "IRE",
                ReleaseYear = 2015
            };

            ire.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = metalCore,
                    Record = ire
                }
            };

            var allOurGodsHaveAbandonedUs = new Record
            {
                Artist = architects,
                Title = "All Our Gods Have Abandoned Us",
                ReleaseYear = 2016
            };

            allOurGodsHaveAbandonedUs.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = metalCore,
                    Record = allOurGodsHaveAbandonedUs
                }
            };

            var sealTheDealAndLetsBoogie = new Record
            {
                Artist = volbeat,
                Title = "Seal The Deal And Let's Boogie",
                ReleaseYear = 2016
            };

            sealTheDealAndLetsBoogie.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = heavyMetal,
                    Record = sealTheDealAndLetsBoogie
                }
            };

            var rumSodomyAndTheLash = new Record
            {
                Artist = thePogues,
                Title = "Rum, Sodomy And The Lash",
                ReleaseYear = 1985
            };

            rumSodomyAndTheLash.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = celticPunk,
                    Record = rumSodomyAndTheLash
                }
            };

            var meir = new Record
            {
                Artist = kvelertak,
                Title = "Meir",
                ReleaseYear = 2015
            };

            meir.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = blackNRoll,
                    Record = meir
                }
            };

            context.Records.AddRange
            (
                thatsTheSpirit,
                ire,
                allOurGodsHaveAbandonedUs,
                sealTheDealAndLetsBoogie,
                rumSodomyAndTheLash,
                meir
            );

            context.SaveChanges();
        }
    }
}