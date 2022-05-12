using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using MovementApp.Data;
using MovementApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace MovementApp
{
    [Activity(Label = "LoginActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class LoginActivity : AppCompatActivity
    {
        TextView signIn;
        EditText username, password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_login);

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);

            signIn = FindViewById<TextView>(Resource.Id.signinbtn);
            signIn.Click += SignIn_Click;
        }

        private async void SignIn_Click(object sender, EventArgs e)
        {
            string _username = username.Text;
            string _password = password.Text;


            using (var conn = new Connect())
            {
                
                ProgressDialog progress = new ProgressDialog(this);
                progress.SetTitle("Loging In");
                progress.SetMessage("Please wait.");
                progress.Show();

                try
                {
                    

                    FlexAppDatabase.ClearTokens();

                    LoginModel loginModel = new LoginModel
                    {
                        email = _username,
                        password = _password

                    };

                    string json = await conn.PostAsync(Defaults.ACCESS_TOKEN, loginModel);
                    AppToken token = JsonConvert.DeserializeObject<AppToken>(json);

                    if (token.Token != null)
                    {

                        FlexAppDatabase.InsertToken(token);

                        MasterData master_data = new MasterData();

                        var assets = await conn.GetAsync(Defaults.ASSETS, token.Token);
                        master_data.Assets = JsonConvert.DeserializeObject<List<AppAsset>>(assets);

                        var users = await conn.GetAsync(Defaults.USERS, token.Token);
                        master_data.Users = JsonConvert.DeserializeObject<List<Models.User>>(users);

                        var locations = await conn.GetAsync(Defaults.ROOT_LOCATIONS, token.Token);
                        master_data.Locations = JsonConvert.DeserializeObject<List<Models.Location>>(locations);


                        Intent mainActivity = new Intent(Application.Context, typeof(MainActivity));
                        mainActivity.PutExtra("master", JsonConvert.SerializeObject(master_data));

                        progress.Hide();

                        StartActivity(mainActivity);
                    }
                    else
                    {
                        progress.Hide();

                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Login Failed");
                        alert.SetMessage("Please check your credentials");

                        alert.SetPositiveButton("Ok", (senderAlert, args) => {

                        });

                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }

                }
                catch (Exception err)
                {
                    progress.Hide();

                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Login Failed");
                    alert.SetMessage("Please check your credentials");

                    alert.SetPositiveButton("Ok", (senderAlert, args) => {

                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }

            }

        }

        
    }
}