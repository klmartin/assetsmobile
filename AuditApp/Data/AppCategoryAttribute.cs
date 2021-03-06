using Newtonsoft.Json;
using SQLite;
using System;

namespace AuditApp.Data
{
    [Table("AppCategoryAttributes")]
    public class AppCategoryAttribute
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("data_type")]
        [JsonProperty("data_type")]
        public string Datatype { get; set; }

        [Column("units")]
        public string Units { get; set; }
        [Column("created_at")]
        [JsonProperty("created_at")]
        public DateTime Created { get; set; }
        [Column("updated_at")]
        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }
        [Indexed]
        [Column("category_id")]
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
    }
}