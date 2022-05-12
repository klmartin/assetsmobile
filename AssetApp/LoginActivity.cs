using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AssetApp.Data;
using AssetApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace AssetApp
{
    [Activity(Label = "LoginActivity", Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class LoginActivity : AppCompatActivity
    {
        Button signIn;
        EditText username, password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_login);

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);


            signIn = FindViewById<Button>(Resource.Id.signinbtn);
            signIn.Click += SignIn_Click;
        }
        private async void SignIn_Click(object sender, EventArgs e)
        {
            string _username = username.Text;
            string _password = password.Text;

            if (_username == string.Empty || _password == string.Empty)
            {
                Toast.MakeText(Application.Context, "Please set correct credentials.", ToastLength.Long).Show();
            }
            else
            {
                MainActivity asset_activity = new MainActivity();
                if (asset_activity.CheckForInternetConnection())
                {
                    using (var conn = new Connect())
                    {

                        ProgressDialog progress = new ProgressDialog(this);
                        progress.SetTitle("Logging In");
                        progress.SetMessage("Please wait...");
                        progress.SetCancelable(false);

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

                                Intent mainActivity = new Intent(this, typeof(MainActivity));
                                mainActivity.PutExtra("Token", JsonConvert.SerializeObject(token));

                                progress.Hide();
                                StartActivity(mainActivity);
                            }
                            else
                            {
                                progress.Hide();

                                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                alert.SetTitle("Login Fail");
                                alert.SetMessage("Please check your credentials.");

                                alert.SetPositiveButton("Ok", (senderAlert, args) =>
                                {

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

                            alert.SetPositiveButton("Ok", (senderAlert, args) =>
                            {

                            });

                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }

                    }

                }
                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Sorry!");
                    alert.SetMessage("Please switch on your mobile network.");
                    alert.Show();
                }



            }

        }

        
    }
}