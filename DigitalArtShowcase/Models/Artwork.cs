using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalArtShowcase.Models
{
    public class Artwork
    {
        //make sure the Artwork is registered with a PK
        [Key]
        public int ArtworkId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CreationYear { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; } // Foreign key property

        // Navigation properties
        public virtual Artist Artist { get; set; } // One artwork has one artist

        // Many-to-Many: Artwork can be in many exhibitions
        public virtual ICollection<Exhibition> Exhibitions { get; set; } // Navigation property
    }
    public class ArtworkDto
    {
        public int ArtworkId { get; set; } // PK from Artwork model

        public string Title { get; set; }

        public string Description { get; set; }

        public int CreationYear { get; set; }

        public decimal Price { get; set; }

        public int ArtistId { get; set; } // Foreign key for Artist

        // Optionally include basic artist info or other relevant properties if needed
        public string ArtistName { get; set; } // e.g., Include artist name for reference in the DTo 

        // New property to hold a list of associated exhibitions
        public List<ExhibitionDto> Exhibitions { get; set; } // List of exhibitions where the artwork is featured
    }
}
