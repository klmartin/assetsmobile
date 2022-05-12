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

namespace AssetApp.Models
{
    public class PlacementLocation
    {
        [JsonProperty("sites")]
        public List<Location> Sites { get; set; }
        [JsonProperty("current_site")]
        public string CurrentSite { get; set; }
        [JsonProperty("current_site_name")]
        public string CurrrentSiteName { get; set; }
    }

}