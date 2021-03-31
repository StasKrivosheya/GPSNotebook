using SQLite;

namespace GPSNotebook.Models
{
    [Table("Users")]
    public class UserModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Name { get; set; }

        [Unique]
        public string Mail { get; set; }

        [MaxLength(16)]
        public string Password { get; set; }
    }
}
