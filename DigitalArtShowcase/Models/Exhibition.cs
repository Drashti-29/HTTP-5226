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
}
