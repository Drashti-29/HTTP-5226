namespace DigitalArtShowcase.Models.ViewModels
{
    public class ArtworkDetails
    {
        // A Artworks page must have a artwork
        // FindArtwork(artworkid)
        public required ArtworkDto Artwork { get; set; }

        // A Artwork may have Exhibition associated to it
        // ListExhibitionsForArtwork(artworkid)
        public IEnumerable<ExhibitionDto>? Exhibitions { get; set; }

        // All ExhibitionDto
        // ListExhibitions()
        public IEnumerable<ExhibitionDto>? AllExhibitions { get; set; }

        //Artist details for this artwork
        public ArtistDto? ArtworkArtistDetail{ get; set; }
    }
}
