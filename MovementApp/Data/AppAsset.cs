using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovementApp.Data
{
    [Table("AppAssets")]
    public class AppAsset
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("global_id")]
        public int GlobalId { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("tag")]
        public string Tag { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [Column("category_id")]
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
        [Column("created_at")]
        [JsonProperty("created_at")]
        public DateTime Created { get; set; }
        [Column("updated_at")]
        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }
    }
}