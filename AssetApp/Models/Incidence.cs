using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace AssetApp.Models
{
    public class Incidence
    {

        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public string occurence_date { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }
}