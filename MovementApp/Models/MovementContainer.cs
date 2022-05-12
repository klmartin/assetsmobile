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
    public class MovementContainer
    {
        public Movement Movement { get; set; }
        public List<MovementAsset> Assets { get; set; }
    }

    public class FullMovementContainer
    {
        [JsonProperty("movement_in")]
        public Movement MovementIn { get; set; }
        [JsonProperty("movement_out")]
        public Movement MovementOut { get; set; }
        [JsonProperty("assets")]
        public List<MovementAsset> Assets { get; set; }
    }



    public class Movement
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("date_time")]
        public string DateTime { get; set; }
        [JsonProperty("direction")]
        public string Direction { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("confirmed_by")]
        public int ConfirmedBy { get; set; }
        [JsonProperty("site_id")]
        public int SiteId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}