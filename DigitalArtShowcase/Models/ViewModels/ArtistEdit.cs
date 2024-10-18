namespace DigitalArtShowcase.Models.ViewModels
{
    public class ArtistEdit
    {
        // A Artist page must have a artwork
        // FindArtist(artistid)
        public required ArtistDto Artist { get; set; }

        // A Artist may have Artwork associated to it
        // ListArtworksForArtist(artistid)
        public IEnumerable<ArtworkDto>? Artworks { get; set; }

        // List to hold selected artwork IDs
        public List<int> SelectedArtworkIds { get; set; } 

    }
}
