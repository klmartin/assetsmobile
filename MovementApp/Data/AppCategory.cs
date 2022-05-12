using SQLite;
using System;

namespace MovementApp.Data
{
    [Table("AppCategories")]
    public class AppCategory
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime Created { get; set; }
        [Column("updated_at")]
        public DateTime Updated { get; set; }
    }
}