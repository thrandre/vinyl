using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Vinyl.Models.Wrappers;

namespace Vinyl.Data.Entities
{
    public class Artist
    {
        private int _id;

        [NotMapped]
        public ArtistId Id
        {
            get { return new ArtistId(_id); }
            set { _id = value.Value; }
        }
        public string Name { get; set; }
        public List<Record> Records { get; set; }
    }
}
