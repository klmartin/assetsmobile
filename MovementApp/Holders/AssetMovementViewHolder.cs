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
    public class AssetMovementViewHolder : RecyclerView.ViewHolder
    {
        public AssetMovementViewHolder(View view) : base(view)
        {
            TextView tv = view.FindViewById<TextView>(Resource.Id.tvMovementItemName);
            TextView cl = view.FindViewById<TextView>(Resource.Id.tvMvtItemConditionLbl);
            Spinner sp = view.FindViewById<Spinner>(Resource.Id.spItemCondition);
            ImageView iv = view.FindViewById<ImageView>(Resource.Id.movementItemImage);
            TextView dl = view.FindViewById<TextView>(Resource.Id.deleteMovementItem);

            AssetName = tv;
            Image = iv;
            Condition = sp;
            ConditionLabel = cl;
            DeleteItem = dl;
            //view.Click += (sender, e) => listener(base.Position);
        }


        //Your adapter views to re-use
        public TextView AssetName { get; set; }
        public TextView ConditionLabel { get; set; }
        public TextView DeleteItem { get; set; }
        public Spinner Condition { get; set; }
        public ImageView Image { get; set; }
    }
}