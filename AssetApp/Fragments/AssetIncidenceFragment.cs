using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AssetApp.Adapters;
using AssetApp.Data;
using Newtonsoft.Json;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = AndroidX.Fragment.App.Fragment;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;


namespace AssetApp.Fragments
{
   // [Activity(Label = "@string/incidence_fragment_name")]
    public class AssetIncidenceFragment : SupportFragment
    {
        private FlexRecyclerViewAdapter<Models.Line, Holders.LineViewHolder> assetsAdapter;

        private RecyclerView assetsRecyclerView;
        private RecyclerView.LayoutManager rvLayoutManager;

        private Button _scanButton;
        private EditText _assetSearchBar;
        private ProgressDialog progress;

        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment

            View view = inflater.Inflate(Resource.Layout.fragment_asset_scan, container, false);

            
            _scanButton = view.FindViewById<Button>(Resource.Id.btn_scan_asset);
            progress = new ProgressDialog((MainActivity)this.Activity);

            _scanButton.Click += _scanButton_Click;

            _assetSearchBar = view.FindViewById<EditText>(Resource.Id.et_asset_scanbox);
            _assetSearchBar.AfterTextChanged += _assetSearchBar_AfterTextChanged;

            assetsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvSearchedAssets);

            return view;

        }

        private void _assetSearchBar_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {
            if (_assetSearchBar.Text.Length > 0)
            {
                LoadItems(_assetSearchBar.Text);
            }
        }

        private void _scanButton_Click(object sender, EventArgs e)
        {
            Intent activity1 = new Intent(this.Context, typeof(ScanCodeActivity));

            StartActivityForResult(activity1, 1);
        }

        private async void LoadItems(string term)
        {
            var token = FlexAppDatabase.GetTokens().FirstOrDefault();


            List<Models.Line> _assets = new List<Models.Line>();

            using (var conn = new Connect())
            {
                var search_result = await conn.GetAsync(Defaults.SEARCH_ASSET + term, token.Token);
                var assets = JsonConvert.DeserializeObject<List<AppAsset>>(search_result).Select(x => new { Id = x.Id, Tag = x.Tag, TextPrimary = x.Name, TextSecondary = x.Description, Image = x.Image }).ToList();
                var _assets_string = JsonConvert.SerializeObject(assets);
                _assets = JsonConvert.DeserializeObject<List<Models.Line>>(_assets_string);
            }




            assetsAdapter = new FlexRecyclerViewAdapter<Models.Line, Holders.LineViewHolder>(this.Context, _assets, (thing, holder, view) =>
            {
                holder.TextPrimary.Text = thing.TextPrimary;
                holder.TextSecondary.Text = thing.TextSecondary;
                // holder.Image.SetImageURI(thing.Image);

                //string filename = System.IO.Path.GetFileName(thing.Image);
                //Picasso.Get().Load(Defaults.ROOT + "/storage/images/" + filename).Into(holder.Image);
/*
                holder.Label.Text = "TS";*/

                view.Click += delegate (object sender, EventArgs e) {

                    var base_act = (MainActivity)this.Activity;

                    Intent IncidenceActivity = new Intent(Application.Context, typeof(IncidenceActivity));
                    IncidenceActivity.PutExtra("Token", base_act._token);
                    IncidenceActivity.PutExtra("asset_id", thing.Id);
                    IncidenceActivity.PutExtra("asset_name", thing.TextPrimary);
                    IncidenceActivity.PutExtra("asset_tag", thing.Tag);
                    StartActivity(IncidenceActivity);

                };

            }, Resource.Layout.flex_item_line);

            rvLayoutManager = new LinearLayoutManager(this.Context);

            assetsRecyclerView.SetAdapter(assetsAdapter);
            assetsRecyclerView.SetLayoutManager(rvLayoutManager);



        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1 && data != null)
            {


                //_assetSearchBar.Text = data.GetStringExtra("barcode");

                progress.SetTitle("Search Asset");
                progress.SetMessage("Please wait...");
                progress.SetCancelable(false);
                progress.Show();
                SearchTag(data.GetStringExtra("barcode"));
            }
        }

        private async void SearchTag(string tag)
        {
            var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            List<Models.Line> _assets = new List<Models.Line>();

            using (var conn = new Connect())
            {
                var search_result = await conn.GetAsync(Defaults.SEARCH_TAG + tag, token.Token);
                var response = JsonConvert.DeserializeObject<Models.AssetSearchTag>(search_result);
                if (response.Response != null)
                {
                    var base_act = (MainActivity)this.Activity;

                    Intent IncidenceActivity = new Intent(Application.Context, typeof(IncidenceActivity));
                    IncidenceActivity.PutExtra("Token", base_act._token);
                    IncidenceActivity.PutExtra("asset_id", response.Response);
                    IncidenceActivity.PutExtra("asset_name", response.Name);
                    IncidenceActivity.PutExtra("asset_tag", tag);
                    StartActivity(IncidenceActivity);

                    progress.Hide();
                }

                else
                {
                    progress.Hide();
                    _assetSearchBar.Text = tag;

                    Toast.MakeText(Application.Context, "Asset with Tag " + tag + " Not Found", ToastLength.Long).Show();
                }

            }
        }


    }
}
