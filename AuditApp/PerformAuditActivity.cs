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
using Com.Toptoche.Searchablespinnerlibrary;

namespace AuditApp
{
    [Activity(Label = "PerformAudit")]

    
    public class PerformAuditActivity : Activity
    {
        private SearchableSpinner search_asset_box;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var _token = Intent.GetStringExtra("Token");
            SetContentView(Resource.Layout.activity_perform_audit);
       
            search_asset_box = FindViewById<SearchableSpinner>(Resource.Id.spinnerrr);
            // Create your application here
        }
    }
}