using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetApp.Models
{
    public class PlacementObject
    {

        public int asset_id { get; set; }
        public string LocationName { get; set; }
        public int location_id { get; set; }
        public string current_location_name { get; set; }
        public string current_location_id { get; set; }
    }
}