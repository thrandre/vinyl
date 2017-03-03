using System.ComponentModel.DataAnnotations;
using Vinyl.Models.Wrappers;

namespace Vinyl.Models
{
    public class Artist
    {
        public ArtistId Id { get; set; }
        [Required]
        public ArtistName Name { get; set; }
    }
}
