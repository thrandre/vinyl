using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinyl.Data.Entities
{
    public class Record
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Artist Artist { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public List<RecordGenre> Genres { get; set; }
    }
}