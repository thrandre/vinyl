using System.Collections.Generic;

namespace Vinyl.Data.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public List<RecordGenre> Records { get; set; }
    }
}
