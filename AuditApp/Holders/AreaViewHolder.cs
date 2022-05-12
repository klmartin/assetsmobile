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

namespace AuditApp.Holders  
{
    public class AreaViewHolder : RecyclerView.ViewHolder
    {
        public AreaViewHolder(View view) : base(view)
        {
            TextView arl = view.FindViewById<TextView>(Resource.Id.areaLocation);
            TextView ars = view.FindViewById<TextView>(Resource.Id.areaSite);

            TextPrimary = arl;
            TextSecondary = ars;

        }

        public TextView TextPrimary { get; set; }
        public TextView TextSecondary { get; set; }
       
    }
}