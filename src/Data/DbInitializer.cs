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

            context.Genres.AddRange(emoCore);

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

            context.Records.AddRange
            (
                thatsTheSpirit,
                new Record
                {
                    Artist = parkwayDrive,
                    Title = "IRE",
                    ReleaseYear = 2015
                },
                new Record
                {
                    Artist = architects,
                    Title = "All Our Gods Have Abandoned Us",
                    ReleaseYear = 2016
                },
                new Record
                {
                    Artist = volbeat,
                    Title = "Seal The Deal And Let's Boogie",
                    ReleaseYear = 2016
                },
                new Record
                {
                    Artist = thePogues,
                    Title = "Rum, Sodomy And The Lash",
                    ReleaseYear = 1980
                },
                new Record
                {
                    Artist = kvelertak,
                    Title = "Meir",
                    ReleaseYear = 2015
                }
            );

            thatsTheSpirit.Genres = new List<RecordGenre>
            {
                new RecordGenre
                {
                    Genre = emoCore,
                    Record = thatsTheSpirit
                }
            };

            context.SaveChanges();
        }
    }
}