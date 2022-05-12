using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using AuditApp.FlexControls;
using AuditApp.Adapters;
using Button = Android.Widget.Button;
using AndroidX.RecyclerView.Widget;
using AuditApp.Models;
using AuditApp.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Content;
using System.Linq;

namespace AuditApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class MainActivity : AppCompatActivity,DatePickerDialog.IOnDateSetListener
    {
        private AreasRecyclerViewAdapter<Models.AreaModel, Holders.AreaViewHolder> areasRecyclerViewAdapter;
        private RecyclerView areasRecyclerView;
        private RecyclerView.LayoutManager rvLayoutManager;
        public NoSwipePager main_pager;
        public FlexPagerAdapter pagerAdapter;
        internal string _token;
        Button audit_start;
        Button audit_end;
        Button audit_next;
        private int year;
        private int date;
        private  int month;
        public string start_end = "";

        public Context Context { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            audit_start = FindViewById<Button>(Resource.Id.audit_start);
            String myDate = DateTime.Now.ToString();
            audit_start.Click += Audit_start_Click;

            audit_end = FindViewById<Button>(Resource.Id.audit_end);
            audit_next = FindViewById<Button>(Resource.Id.audit_next);  
            audit_end.Click += Audit_end_Click;
            audit_next.Click += NextAuditAction;
            areasRecyclerView = FindViewById<RecyclerView>(Resource.Id.areas_recycler_view);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            LoadAreas();
        }

        private void Audit_start_Click(object sender, EventArgs e)
        {
            start_end = "start";
            int DatePickerDialogId = 1; //user to indentify another dialog
            ShowDialog(DatePickerDialogId);
        }

        private void Audit_end_Click(object sender, EventArgs e)
        {
            start_end = "end";
            int DatePickerDialogId = 1; //user to indentify another dialog
            ShowDialog(DatePickerDialogId);
        }

        protected override Dialog OnCreateDialog(int id)
        {
            DateTime currently = DateTime.Now;
            if (id == 1)
            {
                return new DatePickerDialog(this, this, currently.Year, currently.Month - 1, currently.Day);
            }
            return null;
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnDateSet(Android.Widget.DatePicker view, int year, int month, int dayOfMonth)
        {
            this.year = year;
            this.month = month;
            this.date = dayOfMonth;
            if (start_end == "start")
            {
                audit_start.Text = "Date: " + date + "/" + month + "/" + year;
            }
            if (start_end == "end")
            {
                audit_end.Text = "Date: " + date + "/" + month + "/" + year;
            }
            
            
        }

        private async void LoadAreas( )
        {
            var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            List<Models.AreaModel> _areas = new List<Models.AreaModel>();

            using (var conn = new Connect())
            {
                var area_result = await conn.GetAsync(Defaults.GET_AUDIT_SITES, token.Token);

                var assets = JsonConvert.DeserializeObject<List<AreaSite>>(area_result).Select(x => new { Id = x.SiteId, TextPrimary = x.Branch, TextSecondary = x.Site }).ToList();

                var _assets_string = JsonConvert.SerializeObject(assets);
                _areas = JsonConvert.DeserializeObject<List<Models.AreaModel>>(_assets_string);
            }

            areasRecyclerViewAdapter = new AreasRecyclerViewAdapter<Models.AreaModel, Holders.AreaViewHolder>(this.Context, _areas, (thing, holder, view) =>
            {
                holder.TextPrimary.Text = thing.TextPrimary;
                holder.TextSecondary.Text = thing.TextSecondary;

                

            }, Resource.Layout.area_select_view);

            rvLayoutManager = new LinearLayoutManager(this.Context);


            areasRecyclerView.SetAdapter(areasRecyclerViewAdapter);
            areasRecyclerView.SetLayoutManager(rvLayoutManager);
        }

        private void NextAuditAction(object sender, EventArgs e)
        {

            var intent = new Intent(this, typeof(PerformAuditActivity));
            intent.PutExtra("MyData", "Data from Activity1");
            StartActivity(intent);

        }



    }
}
