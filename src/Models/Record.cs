using System.Collections.Generic;
using Vinyl.Models.Wrappers;

namespace Vinyl.Models
{
    public class Record
    {
        public RecordId Id { get; set; }
        public Artist Artist { get; set; }
        public RecordTitle Title { get; set; }
        public Year ReleaseYear { get; set; }
        public List<Genre> Genres { get; set; }
    }
}