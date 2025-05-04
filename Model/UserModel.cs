using SQLite;

namespace SECWRework.Model
{
    [Table("User")]
    public class UserModel
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("First_Name")]
        public string? F_Name { get; set; }

        [Column("Last_Name")]
        public string? L_Name { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("Phone_number")]
        public string? Phone_number { get; set; }

        [Column("Role")]
        public string? Role { get; set; }

    }
}
