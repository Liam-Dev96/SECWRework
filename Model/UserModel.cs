using SQLite;

namespace SECWRework.Model
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    [Table("User")]
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Column("First_Name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Column("Last_Name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Column("Email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        [Column("Phone_number")]
        public string? Phone_number { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        [Column("Role")]
        public string? Role { get; set; }
    }
}
