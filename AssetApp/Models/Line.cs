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
    public class Line
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Key { get; set; }
        public string TextPrimary { get; set; }
        public string TextSecondary { get; set; }
        public string Image { get; set; }
        public string Label { get; set; }
    }
}