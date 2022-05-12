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

namespace AssetApp.Holders
{
    public class LineViewHolder : RecyclerView.ViewHolder
    {
        public LineViewHolder(View view) : base(view)
        {
            TextView tvp = view.FindViewById<TextView>(Resource.Id.tvLinePrimaryText);
            TextView tvs = view.FindViewById<TextView>(Resource.Id.tvLineSecondaryText);
            ImageView iv = view.FindViewById<ImageView>(Resource.Id.ivLineImage);

            TextPrimary = tvp;
            TextSecondary = tvs;
            Image = iv;

        }

        public TextView TextPrimary { get; set; }
        public TextView TextSecondary { get; set; }
        public ImageView Image { get; set; }
        public TextView Label { get; set; }
    }


    public class AttributeViewHolder : RecyclerView.ViewHolder
    {
        public AttributeViewHolder(View view) : base(view)
        {
            TextView lbl = view.FindViewById<TextView>(Resource.Id.tvLineLabelText);
            TextView val = view.FindViewById<TextView>(Resource.Id.tvLineValueText);

            Label = lbl;
            Value = val;

        }

        public TextView Label { get; set; }
        public TextView Value { get; set; }

    }


    public class MovementViewHolder : RecyclerView.ViewHolder
    {
        public MovementViewHolder(View view) : base(view)
        {

            TextView rsn = view.FindViewById<TextView>(Resource.Id.tvLineLabelText);
            TextView dt = view.FindViewById<TextView>(Resource.Id.tvLineValueText);
            //TextView directn = view.FindViewById<TextView>(Resource.Id.tvLineLabelText);
            //TextView cndtn = view.FindViewById<TextView>(Resource.Id.tvLineLabelText);
            //TextView site = view.FindViewById<TextView>(Resource.Id.tvLineValueText);

            Reason = rsn;
            Date = dt;
            //Direction = directn;
            //Condition = cndtn;
            //Site = site;

        }

        public TextView Reason { get; set; }
        public TextView Date { get; set; }
        //public TextView Direction { get; set; }
        //public TextView Site { get; set; }
        //public TextView Condition { get; set; }

    }
}