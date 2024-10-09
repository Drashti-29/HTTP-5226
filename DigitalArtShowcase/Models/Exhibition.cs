using System.ComponentModel.DataAnnotations;

namespace DigitalArtShowcase.Models
{
    public class Exhibition
    {

        //make sure the Artwork is registered with a PK
        [Key]
        public int ExhibitionId { get; set; }

        public string ExhibitionName { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        // Navigation property for many-to-many relationship with Artwork
        public virtual ICollection<Artwork> Artworks { get; set; }
    }
    public class ExhibitionDto
    {
        // Exhibition ID (PK)
        public int ExhibitionId { get; set; }

        // Name of the exhibition
        public string ExhibitionName { get; set; }

        // Location where the exhibition is held
        public string Location { get; set; }

        // Date of the exhibition
        public DateTime Date { get; set; }

        // A collection of Artwork IDs to show the relationship
        public List<ArtworkDto> Artworks { get; set; }
    }
}
