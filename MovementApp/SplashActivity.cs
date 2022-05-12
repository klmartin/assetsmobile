using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using MovementApp.Data;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementApp
{
    [Activity(Label = "MovementApp", MainLauncher = true, Theme = "@style/AppTheme.Splash", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            // Create your application here
        }
       
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            await FlexAppDatabase.Initialize();

            await Task.Delay(8000); // Simulate a bit of startup work.
            var tokens = FlexAppDatabase.GetTokens();

            if (tokens.Count > 0)
            {
                using (var conn = new Connect())
                {
                    try
                    {

                        AppToken token = tokens.FirstOrDefault();

                        if (token.Token != null)
                        {

                            MasterData master_data = new MasterData();

                            var assets = await conn.GetAsync(Defaults.ASSETS, token.Token);
                            master_data.Assets = JsonConvert.DeserializeObject<List<AppAsset>>(assets);

                            var users = await conn.GetAsync(Defaults.USERS, token.Token);
                            master_data.Users = JsonConvert.DeserializeObject<List<Models.User>>(users);

                            var locations = await conn.GetAsync(Defaults.ROOT_LOCATIONS, token.Token);
                            master_data.Locations = JsonConvert.DeserializeObject<List<Models.Location>>(locations);


                            Intent mainActivity = new Intent(Application.Context, typeof(MainActivity));
                            mainActivity.PutExtra("master", JsonConvert.SerializeObject(master_data));

                            StartActivity(mainActivity);

                        }
                        else
                        {
                            FlexAppDatabase.ClearTokens();

                            Intent accountActivity = new Intent(this, typeof(LoginActivity));
                            StartActivity(accountActivity);
                        }

                    }
                    catch (Exception e)
                    {
                        Toast.MakeText(this, "Connecton Failed!...", ToastLength.Long).Show();
                        Finish();
                    }


                }

            }
            else
            {
                Intent accountActivity = new Intent(this, typeof(LoginActivity));

                //Intent accountActivity = new Intent(this, typeof(AddPlayerActivity));
                StartActivity(accountActivity);
            }

        }
    }
}