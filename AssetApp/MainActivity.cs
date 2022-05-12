using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using AndroidX.Core.View;
using AssetApp.FlexControls;
using AssetApp.Adapters;
using AssetApp.Fragments;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
using System.Globalization;
using System.Net;
using AssetApp.Data;
using Android.Content;

namespace AssetApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        public NoSwipePager main_pager;
        public FlexPagerAdapter pagerAdapter;
        internal string _token;
        Toolbar toolbar;

        public Toolbar ToolBar { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
/*
            var context = (Activity)Xamarin.Forms.Forms.Context;
            var toolbar = context.FindViewById<Android.Support.V7.Widget.Toolbar>(Droid.Resource.Id.toolbar);

            toolbar.NavigationIcon = Android.Support.V4.Content.ContextCompat.GetDrawable(context, Resource.Drawable.Back);
*/

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            main_pager = FindViewById<NoSwipePager>(Resource.Id.assetPager);

            pagerAdapter = new FlexPagerAdapter(SupportFragmentManager);

            pagerAdapter.AddFragment(new AssetScanFragment(), "Asset scan");
            pagerAdapter.AddFragment(new AssetPlacementFragment(), "Fragments");
            pagerAdapter.AddFragment(new AssetIncidenceFragment(), "Incidence");

            main_pager.Adapter = pagerAdapter;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
           

            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_logout)
            {

                FlexAppDatabase.ClearTokens();
                Intent LoginActivity = new Intent(this, typeof(LoginActivity));
                StartActivity(LoginActivity);
               /* return true;*/
            }
            if (id == Resource.Id.action_update)
            {
                var uri = Android.Net.Uri.Parse(Defaults.ROOT + Defaults.DOWNLOAD);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            }

            if (id == Resource.Id.action_go_to_web)
            {
                var uri = Android.Net.Uri.Parse(Defaults.ROOT);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
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

        public bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
        {
            try
            {
                url ??= CultureInfo.InstalledUICulture switch
                {
                    { Name: var n } when n.StartsWith("fa") => // Iran
                        "http://www.aparat.com",
                    { Name: var n } when n.StartsWith("zh") => // China
                        "http://www.baidu.com",
                    _ =>
                        "http://www.gstatic.com/generate_204",
                };

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (var response = (HttpWebResponse)request.GetResponse())
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_asset_profile)
            {
                // Handle the camera action
                main_pager.CurrentItem = 0;
            }
            else if (id == Resource.Id.nav_incidence)
            {
              /*  Console.WriteLine("cool1");*/
                main_pager.CurrentItem = 2;
            }
            else if (id == Resource.Id.nav_placement)
            {
                main_pager.CurrentItem = 1;
            }


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}
