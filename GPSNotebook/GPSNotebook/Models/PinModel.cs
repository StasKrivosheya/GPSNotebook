using SQLite;

namespace GPSNotebook.Models
{
    [Table("Pins")]
    public class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public bool IsFavorite { get; set; }

        public string PinImagePath { get; set; }
    }
}
