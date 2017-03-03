using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vinyl.Models.Wrappers;

namespace Vinyl.Models
{
    public class Record
    {
        public RecordId Id { get; set; }
        [Required]
        public Artist Artist { get; set; }
        [Required]
        public RecordTitle Title { get; set; }
        [Required]
        public Year ReleaseYear { get; set; }
        [Required]
        public List<Genre> Genres { get; set; }
        [Required]
        public string Location { get; set; }
        public DateTime PurchasedDate { get; set; }
    }
}