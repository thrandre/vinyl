using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vinyl.Data.Contexts;
using Vinyl.Models;
using Vinyl.Models.Wrappers;

namespace Vinyl.Actions
{
    public class GetRecordsAction
    {
        private VinylContext Context { get; }

        public GetRecordsAction(VinylContext context)
        {
            Context = context;
        }

        public Record Execute(RecordId recordId)
        {
            return Context.Records
                .Include(r => r.Artist)
                .Include(r => r.Genres).ThenInclude(g => g.Genre)
                .Select(Map)
                .Single(r => r.Id.Equals(recordId));
        }

        public List<Record> Execute()
        {
            return Context.Records
                .Include(r => r.Artist)
                .Include(r => r.Genres).ThenInclude(g => g.Genre)
                .Select(Map)
                .ToList();
        }

        private static Record Map(Data.Entities.Record record)
        {
            return new Record
            {
                Id = new RecordId(record.Id),
                Artist = new Artist
                {
                    Name = new ArtistName(record.Artist.Name)
                },
                ReleaseYear = new Year(record.ReleaseYear),
                Title = new RecordTitle(record.Title),
                Genres = record.Genres.Select(g => Genre.FromKey(g.Genre.Key)).ToList()
            };
        }
    }
}