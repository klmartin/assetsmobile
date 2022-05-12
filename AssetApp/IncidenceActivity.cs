using Android.App;
using Android.Content;
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
using System.Linq;
using System.Text;

namespace AssetApp
{

    [Activity(Label = "IncidenceActivity"), MetaData("android.support.PARENT_ACTIVITY", Value = "Fragments.AssetIncidenceFragment")]
   
    public class IncidenceActivity : AppCompatActivity, DatePickerDialog.IOnDateSetListener

    {
        public string _token;
        public int asset_id;
        public string name;
        public int year;
        public int month; 
        public int date;

        public string tag;
        Button occurence_input;
        private Toolbar toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _token = Intent.GetStringExtra("Token");
            asset_id = Intent.GetIntExtra("asset_id", 0);
            name = Intent.GetStringExtra("asset_name");
            tag = Intent.GetStringExtra("asset_tag");
            SetContentView(Resource.Layout.activity_incidence_reporting);


            toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar2);
            SetActionBar(toolbar);           
       /*ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

*/
            var spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += (s, e) =>
            {
                string firstItem = spinner.SelectedItem.ToString();

                if (firstItem.Equals(spinner.SelectedItem.ToString()))
                {

                }
                else
                {
                    Toast.MakeText(this, "selected" + e.Parent.GetItemIdAtPosition(e.Position), ToastLength.Long).Show(); ;
                }
            };


            occurence_input = FindViewById<Button>(Resource.Id.occurence_input);
            String myDate = DateTime.Now.ToString();
            occurence_input.Text = myDate;
            occurence_input.Click += Occurence_input_Click;

            EditText asset_tag_label = FindViewById<EditText>(Resource.Id.tag_input);
            asset_tag_label.Text = tag;

            EditText asset_name_label = FindViewById<EditText>(Resource.Id.name_input);
            asset_name_label.Text = name;
            Button submit_incidence = FindViewById<Button>(Resource.Id.submit);
            submit_incidence.Click += send_data;

            // Create your applicatioOnCreateDialogn here
        }

        private void Occurence_input_Click(object sender, EventArgs e)
        {
            int DatePickerDialogId = 1;//user to indentify another dialog
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

        private async void send_data(object sender, EventArgs e)
        {                
            EditText title = FindViewById<EditText>(Resource.Id.incidence_title_input);
            EditText description = FindViewById<EditText>(Resource.Id.description_input);

            ProgressDialog progress2 = new ProgressDialog(this);
            progress2.SetTitle("Saving");
            progress2.SetMessage("Please wait...");

            using (var conn = new Connect())
            {
                try
                {
                    Incidence incidenceModel = new Incidence
                    {
                        asset_id = asset_id,
                        asset_name = name,
                        title = title.Text,
                        occurence_date = occurence_input.Text,
                        description = description.Text
                    };
                    progress2.Show();

                    var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                    var response = await conn.PostAsync(Defaults.SUBMIT_INCIDENCE_WITH_ASSET, incidenceModel, token.Token);

                    Intent MainActivity = new Intent(Application.Context, typeof(MainActivity));
                    StartActivity(MainActivity);

                    Toast.MakeText(Application.Context, "You have successful report an incidence", ToastLength.Long).Show();

                    progress2.Hide();                   
                }
                catch (Exception err)
                {
                    progress2.Hide();
                    Toast.MakeText(Application.Context, "Fail to submit", ToastLength.Long).Show();

                }
            }
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            this.year = year;
            this.month = month;
            this.date = dayOfMonth;
            occurence_input.Text = date + "/" + month + "/" + year;

        }
    }
}