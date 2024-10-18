
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

        // A collection of artwork IDs associated with this artist
        public List<int> ArtworkIds { get; set; } = new List<int>();

        // Optionally, you can also include a collection of detailed ArtworkDto objects
        public List<ArtworkDto> Artworks { get; set; } = new List<ArtworkDto>();
    }
}
