using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using SQLite;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuditApp.Data
{
    [Table("AppAttributes")]
    public class AppAttribute
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("global_id")]
        public int GlobalId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("value")]
        public string Value { get; set; }
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
        [Column("asset_id")]
        [JsonProperty("asset_id")]
        public int AssetId { get; set; }
    }
}