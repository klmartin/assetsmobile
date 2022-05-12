using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovementApp.Holders
{
    public class LineViewHolder : RecyclerView.ViewHolder
    {
        public LineViewHolder(View view) : base(view)
        {
            TextView tvp = view.FindViewById<TextView>(Resource.Id.tvLinePrimaryText);
            TextView tvs = view.FindViewById<TextView>(Resource.Id.tvLineSecondaryText);
           // TextView tvl = view.FindViewById<TextView>(Resource.Id.tvLineLabel);
            ImageView iv = view.FindViewById<ImageView>(Resource.Id.ivLineImage);

            TextPrimary = tvp;
            TextSecondary = tvs;
           // Label = tvl;
            Image = iv;

        }


        public TextView TextPrimary { get; set; }
        public TextView TextSecondary { get; set; }
        public ImageView Image { get; set; }
        public TextView Label { get; set; }
    }
}