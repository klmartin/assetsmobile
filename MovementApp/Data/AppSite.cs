using SQLite;
using System;

namespace MovementApp.Data
{
    [Table("AppSites")]
    public class AppSite
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("created_at")]
        public DateTime Created { get; set; }
        [Column("updated_at")]
        public DateTime Updated { get; set; }
    }
}