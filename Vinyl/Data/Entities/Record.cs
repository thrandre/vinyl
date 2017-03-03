using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Vinyl.Models.Wrappers;

namespace Vinyl.Data.Entities
{
    public class Record
    {
        private int _id;

        [NotMapped]
        public RecordId Id
        {
            get { return new RecordId(_id); }
            set { _id = value.Value; }
        }
        public Artist Artist { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public List<RecordGenre> Genres { get; set; }
        public string Location { get; set; }
    }
}