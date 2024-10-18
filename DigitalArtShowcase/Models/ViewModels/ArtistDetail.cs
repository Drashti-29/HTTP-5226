namespace DigitalArtShowcase.Models.ViewModels
{
    public class ArtistDetail
    {
        // A Artist page must have a artwork
        // FindArtist(artistid)
        public required ArtistDto Artist { get; set; }

        // A Artist may have Artwork associated to it
        // ListArtworksForArtist(artistid)
        public IEnumerable<ArtworkDto>? Artworks { get; set; }

        // All ArtworkDto
        // ListArtworks()
        public IEnumerable<ArtworkDto>? AllArtworks { get; set; }
    }
}
