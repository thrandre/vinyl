using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinyl.Data.Entities
{
    public class Artist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Record> Records { get; set; }
    }
}
