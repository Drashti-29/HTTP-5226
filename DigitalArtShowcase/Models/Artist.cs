
using System.ComponentModel.DataAnnotations;
namespace DigitalArtShowcase.Models
{
    public class Artist
    {
        //make sure the Artist is registered with a PK
        [Key]
        public int ArtistId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ArtistBio{ get; set; }

        public String Email { get; set; }

        //A artist has many artwork
        public ICollection<Artwork> Artworks { get; set; }

    }

    public class ArtistDto
    {
        public int ArtistId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ArtistBio { get; set; }

        public String Email { get; set; }
    }
}
