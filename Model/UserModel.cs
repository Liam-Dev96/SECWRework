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

        [Column("Password")]
        public string? Password { get; set; }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
