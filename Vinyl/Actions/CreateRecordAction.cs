using System;
using System.Linq;
using Vinyl.Data.Contexts;
using Vinyl.Data.Entities;
using Vinyl.Models.Wrappers;

namespace Vinyl.Actions
{
    public class CreateRecordAction
    {
        private VinylContext Context { get; }

        public CreateRecordAction(VinylContext context)
        {
            Context = context;
        }

        public RecordId Execute(Models.Record record)
        {
            var artist = Context.Artists
                .SingleOrDefault(a => a.Name.Equals(record.Artist.Name, StringComparison.CurrentCultureIgnoreCase)) ??
                         new Artist
                         {
                            Name = record.Artist.Name
                         };

            var genres = Context.Genres.ToList();

            var newRecord = new Record
            {
                Artist = artist,
                ReleaseYear = record.ReleaseYear,
                Title = record.Title,
                Location = record.Location
            };

            newRecord.Genres = record.Genres
                .Select(g =>
                    new RecordGenre
                    {
                        Genre = genres.Single(ge => ge.Key == g),
                        Record = newRecord
                    }
                )
                .ToList();

            Context.Records.Add(newRecord);

            Context.SaveChanges();

            return newRecord.Id;
        }
    }
}
