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
}
