using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.ViewPager.Widget;
using AssetApp.Adapters;
using AssetApp.Data;
using AssetApp.Fragments;
using Google.Android.Material.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Square.Picasso;
using Android.Graphics;
using static Android.Views.ViewGroup;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;


namespace AssetApp
{
    [Activity(Label = "Asset")]
    public class AssetActivity : AppCompatActivity
    {
        //public Toolbar toolbar;
        public FlexPagerAdapter pagerAdapter;
        public ViewPager pager;

        public AppAsset _asset;

        public string _token;
        public int asset_id;
        public int row_count;


        public AppAsset asset;
        public TextView _asset_name, _asset_tag, _asset_location;
        public ImageView _asset_image;
        public LinearLayout linear_layout;



        private TextView _assetNameField;
        private TextView _assetTagField;
        private TextView _assetLocationField;
        private TextView _assetPriceField;
        private TextView _assetDescriptionField;

        public Android.Widget.Toolbar toolbar { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _token = Intent.GetStringExtra("Token");
            asset_id = Intent.GetIntExtra("asset_id", 0);

            _asset = FlexAppDatabase.GetAsset(asset_id);
            // Create your application here
            SetContentView(Resource.Layout.activity_asset);

            toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar3);
            SetActionBar(toolbar);

            _assetNameField = this.FindViewById<TextView>(Resource.Id.tv_asset_name);
            _assetTagField = this.FindViewById<TextView>(Resource.Id.tv_asset_tag);

            _assetDescriptionField = this.FindViewById<TextView>(Resource.Id.tv_asset_description);

            _assetLocationField = this.FindViewById<TextView>(Resource.Id.tv_asset_location);
            _assetPriceField = this.FindViewById<TextView>(Resource.Id.tv_asset_Price);

            //ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };

            LoadDetails(asset_id);

            //ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = false; }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }



        private async void LoadDetails(int term)
        {

            try
            {


                row_count = 0;

                var token = FlexAppDatabase.GetTokens().FirstOrDefault();

                List<Models.Line> _assets = new List<Models.Line>();

                using (var conn = new Connect())
                {
                    ProgressDialog progress = new ProgressDialog(this);
                    progress.SetTitle("Fetching Profile");
                    progress.SetMessage("Please wait...");
                    progress.SetCancelable(false);
                    progress.Show();
                    string search_result = await conn.GetAsync(Defaults.ASSET_DETAILS + "/" + term, token.Token);

                    //search_result = JsonConvert.SerializeObject(search_result);
                    var response = JsonConvert.DeserializeObject<Models.AssetProfile>(search_result);




                    //var attributes = response.Attributes;
                    //var currentMovementSite = response.CurrentMovementSite;
                    //var movements = response.Movements;
                    var currenctIncidenceSite = response.CurrenctIncidenceSite;
                    var incidences = response.Incidences;
                    var currenctAuditSite = response.CurrenctAuditSite;
                    var purchase = response.Purchase;
                    var donation = response.Donation;
                    //var audits = response.Audits;
                    //var asset = response.Asset;



                    var asset = JsonConvert.SerializeObject(response.Asset);
                    var assets_details = JsonConvert.DeserializeObject<Models.AssetProfileAsset>(asset);



                    var AssetName = assets_details.Name;
                    var AssetDescription = assets_details.Description;
                    var AssetCode = assets_details.Code;
                    var AssetTag = assets_details.Tag;
                    var AssetCategory = assets_details.Category;
                    var AssetImage = assets_details.Image;


                    var currentMovementSite = JsonConvert.SerializeObject(response.CurrentMovementSite);
                    var currentMovementSite_details = JsonConvert.DeserializeObject<Models.CurrentMovementSite>(currentMovementSite);




                    var assetPurchase = JsonConvert.SerializeObject(response.Purchase);
                    var assetPurchase_details = JsonConvert.DeserializeObject<Models.AssetPurchase>(assetPurchase);


                    _assetNameField.Text = AssetName;


                    _assetTagField.Text = AssetTag;

                    _assetDescriptionField.Text = AssetDescription;

                    _assetLocationField.Text = (currentMovementSite_details.Name).Replace('#', '-');

                    _assetPriceField.Text = assetPurchase_details.Price + ' ' + assetPurchase_details.Currency;

                    //addRow("DESCRIPTION", assets_details.Description);
                    addRow("CATEGORY", assets_details.Category);
                    addRow("CODE", assets_details.Code);
                    addRow("BRANCH", getBranchArea(currentMovementSite_details.Name)[0]);
                    addRow("AREA", getBranchArea(currentMovementSite_details.Name)[1]);
                    addRow("STATUS", "NEW");
                    addRow("PURCHASE COST", assets_details.PurchasePrice);
                    addRow("BOOK VALUE", assets_details.BookValue);
                    addRow("DEPRECIATION CLASS", "4 YEARS - STRAIGHT LINE");
                    addRow("PURCHASE DATE", assetPurchase_details.Date);
                    addRow("CONTROLLERS", assets_details.ControllerFirstName.ToUpper() + " " + assets_details.ControllerLastName.ToUpper());
                    addRow("ASSIGNEE", assets_details.AsigneeFirstName.ToUpper() + " " + assets_details.AssigneeLastName.ToUpper());

                    string attributes = JsonConvert.SerializeObject(response.Attributes);


                    var _attributes = JsonConvert.DeserializeObject<List<Models.AssetProfileAttributes>>(attributes);

                    foreach (var attribute in _attributes)
                    {

                        addRow(attribute.Label, attribute.Value);

                    }

                    string movements = JsonConvert.SerializeObject(response.Movements);


                    var _Movements = JsonConvert.DeserializeObject<List<Models.AssetProfileMovements>>(movements);


                    if (_Movements.Count() > 0)
                    {
                        addLine("Movements");
                        foreach (var movement in _Movements)
                        {
                            addLine2(movement.Date);
                            addRow("Branch", getBranchArea(movement.Site)[0]);
                            addRow("Area", getBranchArea(movement.Site)[1]);
                            addRow("Reason", movement.Reason.Replace('#', ' ').ToUpper());
                            addRow("Direction", getDirection(movement.Direction));

                        }

                    }



                    string audits = JsonConvert.SerializeObject(response.Audits);


                    var _Audits = JsonConvert.DeserializeObject<List<Models.AssetProfileAudit>>(audits);


                    if (_Audits.Count() > 0)
                    {
                        addLine("Audits");
                        foreach (var audit in _Audits)
                        {

                            addLine2(audit.Date);
                            addRow("Branch", getBranchArea(audit.Site)[0]);
                            addRow("Area", getBranchArea(audit.Site)[1]);
                            addRow("Description", audit.Description);

                        }

                    }

                    progress.Hide();

                }
            }

            catch (Exception err)
            {

            }
        }


            void addRow(string label, string details)
            {

                try
                {

                    var profileTable = (TableLayout)FindViewById(Resource.Id.rvAssetProfileTable);


                TableLayout.LayoutParams rowLayoutParams = new TableLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

                TableRow.LayoutParams columnLayoutParams2 = new TableRow.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

                TextView column1 = new TextView(this);

                //Use TableRow
                column1.LayoutParameters = columnLayoutParams2;
                column1.SetTextSize(Android.Util.ComplexUnitType.Px, (float)25.0);
                column1.SetTextColor(Color.Black);
                column1.SetPadding(5, 5, 5, 5);

                View line = new View(this);

                line.SetMinimumHeight(2);


                TextView column0 = new TextView(this);
                column0.LayoutParameters = columnLayoutParams2;

                column0.SetTextSize(Android.Util.ComplexUnitType.Px, (float)25.0);
                column0.SetTextColor(Color.Black);
                column0.SetPadding(5, 5, 5, 5);

                TextView column2 = new TextView(this);

                //Use TableRow
                column2.LayoutParameters = columnLayoutParams2;

                column2.SetTextColor(Color.Black);
                column2.SetTextSize(Android.Util.ComplexUnitType.Px, (float)25.0);
                column2.SetPadding(5, 5, 5, 5);
                TableRow tr = new TableRow(this);

                //Use TableLayout
                tr.LayoutParameters = rowLayoutParams;

                if (row_count % 2 == 0)
                {
                    tr.SetBackgroundColor(Android.Graphics.Color.ParseColor("#eaeaea"));
                }

                else
                {
                    tr.SetBackgroundColor(Android.Graphics.Color.ParseColor("#f7f7f7"));
                }

                column2.Text = details;
                column1.Text = label;
                column0.Text = " ";
                tr.AddView(column0);
                tr.AddView(column1);
                tr.AddView(column2);
                profileTable.AddView(tr);
                //profileTable.AddView(line);

                row_count++;
                    }

                      catch (Exception err)
                {

                }
            }


            void addLine(string title)
            {


                var profileTable = (TableLayout)FindViewById(Resource.Id.rvAssetProfileTable);


                TableLayout.LayoutParams rowLayoutParams = new TableLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

                TableRow.LayoutParams columnLayoutParams2 = new TableRow.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

                TextView column1 = new TextView(this);

                column1.LayoutParameters = columnLayoutParams2;

                column1.SetTextSize(Android.Util.ComplexUnitType.Px, (float)30.0);
                column1.SetTextColor(Color.Black);
                column1.SetPadding(30, 30, 30, 30);
                column1.SetBackgroundColor(Android.Graphics.Color.ParseColor("#75b4ef"));


                View line = new View(this);

                line.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FF909090"));
                TableRow tr = new TableRow(this);

                //Use TableLayout
                tr.LayoutParameters = rowLayoutParams;
                column1.Text = title;
                profileTable.AddView(column1);

            }

            void addLine2(string title)
            {
                var profileTable = (TableLayout)FindViewById(Resource.Id.rvAssetProfileTable);


                TableLayout.LayoutParams rowLayoutParams = new TableLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

                TableRow.LayoutParams columnLayoutParams2 = new TableRow.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

                TextView column1 = new TextView(this);

                column1.LayoutParameters = columnLayoutParams2;

                column1.SetTextSize(Android.Util.ComplexUnitType.Px, (float)30.0);
                column1.SetTextColor(Color.Black);
                column1.SetPadding(5, 5, 5, 5);
                column1.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FF909090"));

                column1.Text = title;
                profileTable.AddView(column1);

            }

            string[] getBranchArea(string site)
            {
                string[] BranchArea = site.Split('#');

                return BranchArea;
            }


            string getDirection(string direction)
            {

                if (direction == "fin")
                {
                    return "FULL IN";
                }

                else if (direction == "fout")
                {
                    return "FULL OUT";
                }

                else
                {
                    return " ";
                }

            }
        }
    }





//"{\"asset\":{\"id\":2,\"name\":\"HP ENVY 13-AH1025CL\",\"description\":\"HP ENVY 13-AH1025CL\",\"code\":\"LHRC\\/AR\\/LAC\\/LPT\\/2\\/003\",\"tag\":\"301000002\",\"created_at\":\"2022-02-08T14:44:14.000000Z\",\"updated_at\":\"2022-02-23T08:30:40.000000Z\",\"category_id\":\"1\",\"image\":null,\"Category\":\"Computer Equipment\",\"controller_name\":\"Frida Egidilius\",\"assignee_name\":\"Frida Egidilius\",\"asset_access_control\":{\"id\":2,\"asset_id\":\"2\",\"user_id\":\"5\",\"assignee_id\":\"7\",\"controller_id\":\"7\",\"created_at\":\"2022-02-08T14:44:14.000000Z\",\"updated_at\":\"2022-02-08T14:44:14.000000Z\",\"controller\":{\"id\":7,\"name\":\"Frida Egidilius\",\"email\":\"Frida Egidilius@flex.co.tz\",\"email_verified_at\":\"2022-02-08T14:44:14.000000Z\",\"current_team_id\":null,\"profile_photo_path\":null,\"created_at\":\"2022-02-08T14:44:14.000000Z\",\"updated_at\":\"2022-02-08T14:44:14.000000Z\",\"first_name\":\"Frida\",\"last_name\":\"Egidilius\",\"phone_number\":null,\"status\":\"Active\",\"last_seen\":null,\"site_id\":null,\"profile_photo_url\":\"https:\\/\\/ui-avatars.com\\/api\\/?name=Frida+Egidilius&color=7F9CF5&background=EBF4FF\"},\"assignee\":{\"id\":7,\"name\":\"Frida Egidilius\",\"email\":\"Frida Egidilius@flex.co.tz\",\"email_verified_at\":\"2022-02-08T14:44:14.000000Z\",\"current_team_id\":null,\"profile_photo_path\":null,\"created_at\":\"2022-02-08T14:44:14.000000Z\",\"updated_at\":\"2022-02-08T14:44:14.000000Z\",\"first_name\":\"Frida\",\"last_name\":\"Egidilius\",\"phone_number\":null,\"status\":\"Active\",\"last_seen\":null,\"site_id\":null,\"profile_photo_url\":\"https:\\/\\/ui-avatars.com\\/api\\/?name=Frida+Egidilius&color=7F9CF5&background=EBF4FF\"}},\"category\":{\"id\":1,\"code\":\"324r3frv\",\"name\":\"Computer Equipment\",\"depriciation_class_id\":\"2\",\"description\":\"Computer Equipment\",\"created_at\":\"2022-02-08T05:38:51.000000Z\",\"updated_at\":\"2022-02-08T05:38:51.000000Z\"}},\"missed_asset_details\":[{\"controller_name\":\"Frida Egidilius\",\"assign_name\":\"Frida Egidilius\"}],\"attributes\":[{\"id\":4,\"name\":\"LABEL PRINTING\",\"data_type\":\"text\",\"value\":\"YES\",\"units\":\"units\"},{\"id\":5,\"name\":\"PART_NUMBER\",\"data_type\":\"text\",\"value\":\"5HS18UAR#ABA\",\"units\":\"units\"},{\"id\":6,\"name\":\"SERIAL NUMBER\",\"data_type\":\"text\",\"value\":\"8CG842DG20\",\"units\":\"units\"}],\"current_movement_site\":{\"date_time\":\"2022-02-23 08:30:40\",\"name\":\"ARUSHA#ADMISSION UNIT\",\"id\":\"2\"},\"movements\":[{\"movement_id\":\"504\",\"id\":\"509\",\"reason\":\"#placement#\",\"date_time\":\"2022-02-23 08:30:40\",\"direction\":\"fout\",\"condition\":\"good\",\"site_name\":\"ARUSHA#CIVIL\"},{\"movement_id\":\"505\",\"id\":\"510\",\"reason\":\"#placement#\",\"date_time\":\"2022-02-23 08:30:40\",\"direction\":\"fin\",\"condition\":\"good\",\"site_name\":\"ARUSHA#ADMISSION UNIT\"},{\"movement_id\":\"501\",\"id\":\"504\",\"reason\":\"#placement#\",\"date_time\":\"2022-02-21 15:01:37\",\"direction\":\"fout\",\"condition\":\"good\",\"site_name\":\"ARUSHA#ADMISSION UNIT\"},{\"movement_id\":\"502\",\"id\":\"505\",\"reason\":\"#placement#\",\"date_time\":\"2022-02-21 15:01:37\",\"direction\":\"fin\",\"condition\":\"good\",\"site_name\":\"ARUSHA#CIVIL\"},{\"movement_id\":\"482\",\"id\":\"482\",\"reason\":\"#placement#\",\"date_time\":\"2022-02-21 02:26:30\",\"direction\":\"fin\",\"condition\":\"good\",\"site_name\":\"ARUSHA#ADMISSION UNIT\"},{\"movement_id\":\"481\",\"id\":\"481\",\"reason\":\"#placement#\",\"date_time\":\"2022-02-21 02:26:30\",\"direction\":\"fout\",\"condition\":\"good\",\"site_name\":\"ARUSHA#ARUSHA\"},{\"movement_id\":\"2\",\"id\":\"2\",\"reason\":\"Purchase\",\"date_time\":\"2022-02-08 14:44:14\",\"direction\":\"fin\",\"condition\":\"exellent\",\"site_name\":\"ARUSHA#ARUSHA\"}],\"currenct_incidence_site\":null,\"incidences\":[],\"currenct_audit_site\":null,\"purchase\":{\"code\":\"D8H35G92u4\",\"date_time\":\"2019-01-01 00:00:00\",\"price\":\"3245000.00\",\"quantity\":\"1\",\"currency\":\"TZS\"},\"donation\":null,\"audits\":[]}"