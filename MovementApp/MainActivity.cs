using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Google.Android.Material.BottomNavigation;
using MovementApp.FlexControls;
using MovementApp.Adapters;
using MovementApp.Fragments;
using MovementApp.Data;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Content;

namespace MovementApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private NoSwipePager main_pager;
        public FlexPagerAdapter pagerAdapter;

        public MasterData master_data;

        internal string _token;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            master_data = JsonConvert.DeserializeObject<MasterData>(Intent.GetStringExtra("master"));

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            main_pager = FindViewById<NoSwipePager>(Resource.Id.movementPager);

            pagerAdapter = new FlexPagerAdapter(SupportFragmentManager);

            pagerAdapter.AddFragment(new CheckInFragment(), "Check In");
            pagerAdapter.AddFragment(new CheckOutFragment(), "Check Out");
            pagerAdapter.AddFragment(new MoveFragment(), "Move");

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

       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    main_pager.CurrentItem = 0;
                    return true;
                case Resource.Id.navigation_dashboard:
                    main_pager.CurrentItem = 1;
                    return true;
                case Resource.Id.navigation_notifications:
                    main_pager.CurrentItem = 2;
                    return true;
            }
            return false;
        }

       
    }
}
