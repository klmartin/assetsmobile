using SQLite;
using System;


namespace AuditApp.Data
{
    [Table("AppTokens")]
    public class AppToken
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("assigned")]
        public DateTime Assigned { get; set; }
        [Column("expires")]
        public DateTime Expires { get; set; }
    }
}