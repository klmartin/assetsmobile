using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovementApp.Models
{
    public class MovementAsset
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("asset_id")]
        public int AssetId { get; set; }
        [JsonProperty("movement_id")]
        public int MovementId { get; set; }
        [JsonProperty("asset")]
        public Asset Asset { get; set; }
        [JsonProperty("condition")]
        public string Condition { get; set; }

    }
}