using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace AssetApp.Data
{

    public class MasterData
    {
        [JsonProperty("categories")]
        public List<AppCategory> Categories { get; set; }
        [JsonProperty("category_attributes")]
        public List<AppCategoryAttribute> CategoryAttributes { get; set; }
        [JsonProperty("assets")]
        public List<AppAsset> Assets { get; set; }
    }
}