using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AssetApp.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetApp
{
    


    [Activity(Label = "LHRC Flex Asset", MainLauncher = true, Theme = "@style/FlexAppTheme.Splash", NoHistory = true)]
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
                using (var conn = new Data.Connect())
                {
                    try
                    {

                        AppToken token = tokens.FirstOrDefault();

                        if (token != null)
                        {

                            Intent mainActivity = new Intent(Application.Context, typeof(MainActivity));
                            mainActivity.PutExtra("Token", JsonConvert.SerializeObject(token));

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