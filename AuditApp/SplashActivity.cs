using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Linq;
using AuditApp.Data;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace AuditApp
{
    [Activity(Label = "AuditApp", MainLauncher = true, Theme = "@style/AppTheme.Splash", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState , PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState);
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
                            Intent mainActivity = new Intent(Application.Context, typeof(LoginActivity));
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