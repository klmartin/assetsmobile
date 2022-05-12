using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetApp.Data;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using Square.Picasso;
using AssetApp.Adapters;
using SupportFragment = AndroidX.Fragment.App.Fragment;
using AssetApp.Models;
using Com.Toptoche.Searchablespinnerlibrary;




namespace AssetApp.Fragments
{
    [Activity(Label = "Fragments.AssetIncidenceFragment")]

    public class AssetPlacementFragment : SupportFragment
    {
        private FlexRecyclerViewAdapter<Data.AppAsset, Holders.LineViewHolder> assetsAdapter;
        private Spinner LocationSpinner;
        private Button _scanButton;
        private RecyclerView assetsRecyclerView;
        private RecyclerView assetRecyclerView;
        private EditText _assetSearchBar;
        private Button savePlacement;
        private RecyclerView.LayoutManager rvLayoutManager;
        public event EventHandler Clicked;
        public int AssetId = 0;
        public string current_site = "";
        public string current_site_name = "";
        public List<AppAsset> _allassets;
        public List<Models.Location> locations = new List<Models.Location>();

        private SearchableSpinner search_asset_box, search_sites_box;
        private Button _submit_btn;
        private ProgressDialog progress;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment


            progress = new ProgressDialog((MainActivity)this.Activity);
            var main_activity = (MainActivity)this.Activity;
            View view = inflater.Inflate(Resource.Layout.fragment_asset_placement, container, false);
            search_asset_box = view.FindViewById<SearchableSpinner>(Resource.Id.spinnerrr);
            search_sites_box = view.FindViewById<SearchableSpinner>(Resource.Id.location_spinner);
            _scanButton = view.FindViewById<Button>(Resource.Id.btn_scan_asset);
            _scanButton.Click += _scanButton_Click;

            _submit_btn = view.FindViewById<Button>(Resource.Id.savePlacement);
            _submit_btn.Click += SavePlacement_Click;

            search_asset_box.ItemSelected += Search_asset_box_ItemSelected;


            //_scanButton = view.FindViewById<Button>(Resource.Id.btn_scan_asset);
            //_scanButton.Click += _scanButton_Click;

            //_assetSearchBar = view.FindViewById<EditText>(Resource.Id.et_asset_scanbox);
            //_assetSearchBar.AfterTextChanged += _assetSearchBar_AfterTextChanged;

            //assetsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvSelectedAsset);
            //LocationSpinner = view.FindViewById<Spinner>(Resource.Id.spinnerrr);
            //savePlacement = view.FindViewById<Button>(Resource.Id.savePlacement);

            //savePlacement.Click += SavePlacement_Click;



            //assetRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvCheckInAssets);

            GetAssets();

            //submit_btn = view.FindViewById<Button>(Resource.Id.in_submit_btn);
            //submit_btn.Click += Submit_btn_Click;



            //LoadMovementAssets();


            return view;

        }


        private async void Search_asset_box_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            try
            {
                var selected = search_asset_box.SelectedItem.ToString();

            if (selected != "___ Select Asset ___" || selected !=null )
            {
                var tag = selected.Split(' ')[0];

                //var elected = _allassets.First();

                //var elected = from sel in _allassets where sel.Tag == tag select sel;

                var elected = _allassets.Where(i => i.Tag.ToString() == tag).ToList();

                
                AssetId = elected.First().Id;
                LoadLocations(AssetId);


                //assetsAdapter = new FlexRecyclerViewAdapter<Data.AppAsset, Holders.LineViewHolder>(this.Context, elected, (thing, holder, view) =>
                //{
                //    holder.TextPrimary.Text = thing.Description;
                //    holder.TextSecondary.Text = thing.Description;
                //    // holder.Image.SetImageURI(thing.Image);

                //    string filename = System.IO.Path.GetFileName(thing.Image);
                //    Picasso.Get().Load(Defaults.ROOT + "/storage/images/" + filename).Into(holder.Image);

                //    // holder.Label.Text = "TS";

                //}, Resource.Layout.flex_item_line);


                //using (var conn = new Connect())
                //{
                //    var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                //    object data = null;

                //    if (Movement == null)
                //    {
                //        data = new { tag = tag, direction = "fin", reason = "check in", condition = "exellent", date = DateTime.UtcNow, site_id = 1 };
                //    }
                //    else
                //    {
                //        data = new { tag = tag, direction = "fin", reason = "check in", condition = "exellent", date = DateTime.UtcNow, movement_id = Movement.Id, site_id = 1 };
                //    }

                //    var res = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data, token.Token);
                //}

                //LoadMovementAssets(tag);
            }

            }



            catch (Exception err)
            {

            }
        }


        private async void GetAssets()
        {
            //progress.SetTitle("Loading Assets");
            //progress.SetMessage("Please wait...");
            //progress.SetCancelable(false);
            //progress.Show();
            try
            {
                var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            using (var conn = new Connect())
            {
                var _assets = await conn.GetAsync(Defaults.ASSETS, token.Token);
                _allassets = JsonConvert.DeserializeObject<List<AppAsset>>(_assets);
               

                List<string> assets = _allassets.Select(x => x.Tag + " " + x.Name).ToList();
                List<string> assets_array = new List<string>() { "___ Select Asset ___" };

                assets_array.AddRange(assets);

                ArrayAdapter assets_dapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, assets_array);
                search_asset_box.Adapter = assets_dapter;
                //progress.Hide();
            }
            }



            catch (Exception err)
            {

            }

        }


        private  void LoadMovementAssets(string tag)
        {


            //var selected = _allassets.Where(x => x.Tag == tag);

            ////LoadMovementAssets(tag);

            //selected = selected;

            //List<Models.MovementAsset> myassets = new List<Models.MovementAsset>();



            //var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            //using (var conn = new Connect())
            //{
            //    string json = await conn.GetAsync(Defaults.SAVED_IN_MOVEMENTS, token.Token);
            //    MovementContainer data = JsonConvert.DeserializeObject<MovementContainer>(json);

            //    if (data.Movement != null)
            //    {
            //        Movement = data.Movement;
            //        Assets = data.Assets;

            //        foreach (var asset in data.Assets.OrderByDescending(x => x.Id))
            //        {

            //            myassets.Add(asset);

            //        }
            //    }

            //}



            //assetsAdapter = new FlexRecyclerViewAdapter<Models.MovementAsset, Holders.AssetMovementViewHolder>(this.Context, myassets, (thing, holder, view) =>
            //{
            //    if (thing != null && holder != null)
            //    {
            //        holder.AssetName.Text = thing.Asset.Name;
            //        string filename = "image.jpg";

            //        if (thing.Asset.Image != null)
            //        {
            //            filename = System.IO.Path.GetFileName(thing.Asset.Image);
            //        }

            //        Picasso.Get().Load(Defaults.ROOT + "/public/storage/images/" + filename).Into(holder.Image);
            //        holder.ConditionLabel.Text = "Select Condition";
            //        var condition_adapter = ArrayAdapter.CreateFromResource(this.Context, Resource.Array.flex_asset_condition, Android.Resource.Layout.SimpleSpinnerDropDownItem);

            //        holder.Condition.Adapter = condition_adapter;
            //    }

            //}, Resource.Layout.flex_item_movement);

            //rvLayoutManager = new LinearLayoutManager(this.Context);
            //assetRecyclerView.SetLayoutManager(rvLayoutManager);
            //assetRecyclerView.SetAdapter(assetsAdapter);
        }

        private async void SavePlacement_Click(object sender, EventArgs e)
        {
            var selected = search_sites_box.SelectedItem.ToString();
            var selectedLocation = locations.Where(x => x.Name.Replace('#', '-') == selected).FirstOrDefault();

            if (selected != "___ Select Site ___" || selected != null)
            {

                using (var conn = new Connect())
                {
                    try
                    {
                        PlacementObject placement_object = new PlacementObject()
                        {
                            LocationName = selected,
                            asset_id = AssetId,
                            location_id = selectedLocation.Id,
                            current_location_name = current_site_name,
                            current_location_id = current_site
                        };
                        var token = FlexAppDatabase.GetTokens().FirstOrDefault();

                        string json = await conn.PostAsync(Defaults.SAVE_PLACEMENT, placement_object, token.Token);

                        search_asset_box.SetSelection(0);
                        search_sites_box.SetSelection(0);

                        AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        alert.SetTitle("Succesfull");
                        alert.SetMessage("Asset has Been Replaced to " + selectedLocation.Name.Replace('#', '-'));
                        alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        Dialog dialog = alert.Create();
                        dialog.Show();


                    }

                    catch (Exception err)
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        alert.SetTitle("Placement Failed");
                        alert.SetMessage("Please check your internet connection");
                        alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }


                }

            }

            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                alert.SetTitle("Form Incomplete");
                alert.SetMessage("Make sure you select asset and site");
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            }



            //}

        }

        private async void LoadLocations(int site_id)
        {
            try
            {

                using (var conn = new Connect())
            {
               
                progress.SetTitle("Loading Sites");
                progress.SetMessage("Please wait...");
                progress.SetCancelable(false);
                progress.Show();
                var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                var search_result = await conn.GetAsync(Defaults.GET_LOCATION_SITES + site_id, token.Token);
                var placement_locations = JsonConvert.DeserializeObject<Models.PlacementLocation>(search_result);
                locations = placement_locations.Sites;
                current_site = placement_locations.CurrentSite;
                current_site_name = placement_locations.CurrrentSiteName;


                var _sites = await conn.GetAsync(Defaults.GET_LOCATION_SITES + site_id, token.Token);
                var _allsites = JsonConvert.DeserializeObject<Models.PlacementLocation>(_sites);


                List<string> sites = _allsites.Sites.Select(x =>  x.Name.Replace('#', '-')).ToList();
                List<string> sites_array = new List<string>() { "___ Select Site ___" };

                sites_array.AddRange(sites);

                ArrayAdapter sites_dapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, sites_array);
                search_sites_box.Adapter = sites_dapter;
                progress.Hide();

            }
                        //var branches = locations.Select(x => x.Name).ToArray();

                        //var adapter = new ArrayAdapter<string>(this.Context,
                        //Android.Resource.Layout.SimpleSpinnerItem, branches);

                        //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        //LocationSpinner.Adapter = adapter;

                    }

                    catch (Exception err)
                    {

                    }


                }

        //private void location_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    LocationSpinner = (Spinner)sender;

        //    SavePlacement(e.Position);
        //    string toast = string.Format("Asset(s) is successful replaced to ", LocationSpinner.GetItemAtPosition(e.Position), '.');
        //    Toast.MakeText(this.Context, toast, ToastLength.Long).Show();
        //}

        //private async void SavePlacement(int term)
        //{
        //    using (var conn = new Connect())
        //    {
        //        var token = FlexAppDatabase.GetTokens().FirstOrDefault();
        //        var savedPlacement = await conn.PostAsync(Defaults.GET_LOCATION_SITES + term, token.Token);
        //    }
        //}

        private void _scanButton_Click(object sender, EventArgs e)
        {
            Intent activity1 = new Intent(this.Context, typeof(ScanCodeActivity));
            StartActivityForResult(activity1, 0);
        }


        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {

            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (requestCode == 0 && data != null)
                {


                    //_assetSearchBar.Text = data.GetStringExtra("barcode");

                    var elected = _allassets.Where(i => i.Tag.ToString() == data.GetStringExtra("barcode")).ToList();

                    if (elected.Count() > 0)
                    {
                        //search_asset_box.SetSelection(elected.First());
                        //search_sites_box.SetSelection(elected.First().Tag + "  " + elected.First().Name);

                        //search_asset_box.Adapter.GetItemId(elected.First().Tag + "  " + elected.First().Name);

                        search_asset_box.SetSelection(elected.First().Id );

                        //AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        //alert.SetTitle("Not Found");
                        //alert.SetMessage("Asset with Tag " + data.GetStringExtra("barcode") + " has Been Replaced to" + selectedLocation.Name.Replace('#', '-'));
                        //alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        //Dialog dialog = alert.Create();
                        //dialog.Show();

                    }


                    else
                    {

                        AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        alert.SetTitle("Not Found");
                        alert.SetMessage("Asset with Tag " + data.GetStringExtra("barcode") + " Not Found");
                        alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        Dialog dialog = alert.Create();
                        dialog.Show();

                    }
                }

            }
            //SearchTag(data.GetStringExtra("barcode"));

            catch (Exception err)
            {

            }
        

        }





        //private void _assetSearchBar_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        //{
        //    if (_assetSearchBar.Text.Length > 0)
        //    {
        //        LoadItems(_assetSearchBar.Text);
        //    }
        //}

        //private async void LoadItems(string term)
        //{
        //    var token = FlexAppDatabase.GetTokens().FirstOrDefault();

        //    List<Models.Line> _assets = new List<Models.Line>();

        //    using (var conn = new Connect())
        //    {
        //        var search_result = await conn.GetAsync(Defaults.SEARCH_ASSET + term, token.Token);
        //        var assets = JsonConvert.DeserializeObject<List<AppAsset>>(search_result).Select(x => new { Id = x.Id, TextPrimary = x.Name, TextSecondary = x.Description, Image = x.Image }).ToList();
        //        var _assets_string = JsonConvert.SerializeObject(assets);
        //        _assets = JsonConvert.DeserializeObject<List<Models.Line>>(_assets_string);
        //    }

        //    assetsAdapter = new FlexRecyclerViewAdapter<Models.Line, Holders.LineViewHolder>(this.Context, _assets, (thing, holder, view) =>
        //    {
        //        holder.TextPrimary.Text = thing.TextPrimary;
        //        holder.TextSecondary.Text = thing.TextSecondary;
        //        // holder.Image.SetImageURI(thing.Image);

        //        string filename = System.IO.Path.GetFileName(thing.Image);
        //        Picasso.Get().Load(Defaults.ROOT + "/storage/images/" + filename).Into(holder.Image);

        //        view.Click += delegate (object sender, EventArgs e)
        //        {
        //            LoadLocations(thing.Id);
        //            AssetId = thing.Id;
        //        };

        //    }, Resource.Layout.flex_item_line);

        //    rvLayoutManager = new LinearLayoutManager(this.Context);

        //    assetsRecyclerView.SetAdapter(assetsAdapter);
        //    assetsRecyclerView.SetLayoutManager(rvLayoutManager);
        //}


    }





}

   