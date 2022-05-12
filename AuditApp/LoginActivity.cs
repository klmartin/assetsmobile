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
using AndroidX.AppCompat.App;
using AuditApp.Data;
using AuditApp.Models;
using Newtonsoft.Json;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace AuditApp
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

                    if (token != null)
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