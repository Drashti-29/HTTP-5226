namespace DigitalArtShowcase.Models.ViewModels
{
    public class ExhibitionDetails
    {
        // For a list of Artists to choose from
        public IEnumerable<ArtistDto> AllArtist { get; set; }

        // For a list of Artworks to choose from
        public IEnumerable<ArtworkDto> AllArtworks { get; set; }
    }
}
