namespace Publisher.Domain.Entities
{
    public class CoverPersons
    {
        public Guid PersonId { get; set; }
        public Guid CoverId { get; set; }
        public Guid ArtistPersonId { get; set; }
        
        // Navigation properties
        public Cover Cover { get; set; } = null!;
        public Artist Artist { get; set; } = null!;
    }
} 