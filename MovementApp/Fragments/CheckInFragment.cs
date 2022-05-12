using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Toptoche.Searchablespinnerlibrary;
using MovementApp.Adapters;
using MovementApp.Data;
using MovementApp.Models;
using Newtonsoft.Json;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = AndroidX.Fragment.App.Fragment;

namespace MovementApp.Fragments
{
    public class CheckInFragment : SupportFragment
    {
        private const int SCAN_CODE_RESULT = 1010;

        private FlexRecyclerViewAdapter<Models.MovementAsset, Holders.AssetMovementViewHolder> assetsAdapter;

        private RecyclerView assetRecyclerView;
        private RecyclerView.LayoutManager rvLayoutManager;

        private SearchableSpinner search_asset_box,search_user_box;
        private TextView submit_btn;

        public Movement Movement;
        public List<MovementAsset> Assets;

        MovementContainer data;
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var main_activity = (MainActivity)this.Activity;
             // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_check_in, container, false);

            search_asset_box = view.FindViewById<SearchableSpinner>(Resource.Id.et_check_in_asset);
            search_user_box = view.FindViewById<SearchableSpinner>(Resource.Id.et_check_in_user);

            assetRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvCheckInAssets);

            submit_btn = view.FindViewById<TextView>(Resource.Id.in_submit_btn);
            submit_btn.Click += Submit_btn_Click;

            List<string> assets = main_activity.master_data.Assets.Select(x => x.Tag + " " + x.Name).ToList();
            List<string> assets_array = new List<string>() { "___ Select Asset ___" };

            assets_array.AddRange(assets);

            ArrayAdapter assets_dapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, assets_array);
            search_asset_box.Adapter = assets_dapter;

            List<string> users = main_activity.master_data.Users.Select(x => x.Name).ToList();
            List<string> users_array = new List<string>() { "___ Select User ___" };

            users_array.AddRange(users);

            ArrayAdapter users_adapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, users_array);
            search_user_box.Adapter = users_adapter;

            search_asset_box.ItemSelected += Search_asset_box_ItemSelected;
            LoadMovementAssets();

            return view;

        }

        private async void Submit_btn_Click(object sender, EventArgs e)
        {

          
            try
            {

                var selected = search_user_box.SelectedItem.ToString();

                if (selected != "___ Select User ___" && data.Assets != null )
                {
                    var token = FlexAppDatabase.GetTokens().FirstOrDefault();

                    ProgressDialog progress = new ProgressDialog(this.Context);
                    progress.SetTitle("Loading");
                    progress.SetMessage("Please wait..");
                    progress.Show();

                    using (var conn = new Connect())
                    {
                        var response = await conn.PostAsync(Defaults.SUBMIT_MOVEMENT, new { id = Movement.Id, reason = Movement.Reason }, token.Token);
                        AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        alert.SetTitle("Succesfull");
                        alert.SetMessage("Movement is Succesfull");
                        alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        Dialog dialog = alert.Create();
                        dialog.Show();

                        search_asset_box.SetSelection(0);
                        search_user_box.SetSelection(0);
                    }
                    progress.Hide();

                    LoadMovementAssets();
                }

                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                    alert.SetTitle("Failed");
                    alert.SetMessage("Form incomplete");
                    alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                
            }

            catch (Exception err)
            {
                Toast.MakeText(Application.Context, ""+ data.Assets + "", ToastLength.Long).Show();
                AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                alert.SetTitle("Fail");
                alert.SetMessage("Movement Not Successful");
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            }

        }

        private async void Search_asset_box_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

            try
            {
                var selected = search_asset_box.SelectedItem.ToString();

                if (selected != "___ Select Asset ___")
                {

                    ProgressDialog progress = new ProgressDialog(this.Context);
                    progress.SetTitle("Adding Asset");
                    progress.SetMessage("Please wait..");
                    progress.SetCancelable(false);
                    progress.Show();
                    var tag = selected.Split(' ')[0];

                    using (var conn = new Connect())
                    {
                        var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                        object data_ = null;

                        if (Movement == null)
                        {
                            data_ = new { tag = tag, direction = "fin", reason = "check in", condition = "exellent", date = DateTime.UtcNow, site_id = 1 };
                        }
                        else
                        {
                            data_ = new { tag = tag, direction = "fin", reason = "check in", condition = "exellent", date = DateTime.UtcNow, movement_id = Movement.Id, site_id = 1 };
                        }

                        var res = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data_, token.Token);
                    }

                    LoadMovementAssets();
                    progress.Hide();
                    search_asset_box.SetSelection(0);

                }

            }


            catch (Exception err)
            {
                Toast.MakeText(Application.Context, ""+ err+"", ToastLength.Long).Show();

            }
        }

        private async void LoadMovementAssets()
        {
            List<Models.MovementAsset> myassets = new List<Models.MovementAsset>();



            var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            using (var conn = new Connect())
            {
                string json = await conn.GetAsync(Defaults.SAVED_IN_MOVEMENTS, token.Token);
                data = JsonConvert.DeserializeObject<MovementContainer>(json);

                if (data.Movement != null)
                {
                    Movement = data.Movement;
                    Assets = data.Assets;
                    foreach (var asset in data.Assets.OrderByDescending(x => x.Id))
                    {
                        myassets.Add(asset);
                    }
                }

            }



            assetsAdapter = new FlexRecyclerViewAdapter<Models.MovementAsset, Holders.AssetMovementViewHolder>(this.Context, myassets, (thing, holder, view) =>
            {
                if (thing != null && holder != null)
                {
                    holder.AssetName.Text = thing.Asset.Name;
                    string filename = "image.jpg";

                    if (thing.Asset.Image != null)
                    {
                        filename = System.IO.Path.GetFileName(thing.Asset.Image);
                    }

                    Picasso.Get().Load(Defaults.ROOT + "/public/storage/images/" + filename).Into(holder.Image);
                    holder.ConditionLabel.Text = "Select Condition";
                    var condition_adapter = ArrayAdapter.CreateFromResource(this.Context, Resource.Array.flex_asset_condition, Android.Resource.Layout.SimpleSpinnerDropDownItem);

                    holder.Condition.Adapter = condition_adapter;
                   
                      

                    holder.DeleteItem.Click += delegate (object sender, EventArgs e)
                    {
                        //var base_act = (MainActivity)this.Activity;

                        //Intent assetActivity = new Intent(Application.Context, typeof(AssetActivity));
                        //assetActivity.PutExtra("Token", base_act._token);
                        //assetActivity.PutExtra("asset_id", thing.Id);
                        //StartActivity(assetActivity);

                        DeleteAsset(thing.Asset.Id,thing.MovementId);

                    };
                    //holder.Condition
                    holder.Condition.SetSelection(condition_adapter.GetPosition(thing.Condition));
                    holder.Condition.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)

                    {
                        //var base_act = (MainActivity)this.Activity;

                        //Intent assetActivity = new Intent(Application.Context, typeof(AssetActivity));
                        //assetActivity.PutExtra("Token", base_act._token);
                        //assetActivity.PutExtra("asset_id", thing.Id);
                        //StartActivity(assetActivity);

                        SaveCondition(thing.Asset.Id, thing.MovementId, holder.Condition.SelectedItem.ToString());

                    };
                }

            }, Resource.Layout.flex_item_movement);

            rvLayoutManager = new LinearLayoutManager(this.Context);
            assetRecyclerView.SetLayoutManager(rvLayoutManager);
            assetRecyclerView.SetAdapter(assetsAdapter);
        }

        private void Condition_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_scan)
            {
                Intent activity1 = new Intent(this.Context, typeof(ScanCodeActivity));

                StartActivityForResult(activity1, SCAN_CODE_RESULT);
            }

            return base.OnOptionsItemSelected(item);    
        }

        public async void DeleteAsset(int asset_id, int movement_id)
        {

            try
            {
                ProgressDialog progress = new ProgressDialog(this.Context);
            progress.SetTitle("Remove Asset");
            progress.SetMessage("Please wait..");
            progress.SetCancelable(false);
            progress.Show();
            using (var conn = new Connect())
            {
                var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                object data_ = null;



                data_ = new { asset_id = asset_id, movement_id = movement_id };

                var res = await conn.PostAsync(Defaults.REMOVE_MOVEMENT_ASSET, data_, token.Token);
            }

            LoadMovementAssets();

            progress.Hide();

            }


            catch (Exception err)
            {
                //Toast.MakeText(Application.Context, "" + err + "", ToastLength.Long).Show();

            }
        }

        public async void SaveCondition(int asset_id, int movement_id,string condition)
        {

            try
            {
                //ProgressDialog progress = new ProgressDialog(this.Context);
                //progress.SetTitle("Set Condition " + condition);
                //progress.SetMessage("Please wait..");
                //progress.SetCancelable(false);
                //progress.Show();

                if (condition != "exellent") {
                    using (var conn = new Connect())
                    {
                        var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                        object data_ = null;



                        data_ = new { asset_id = asset_id, movement_id = movement_id, condition = condition };

                        var res = await conn.PostAsync(Defaults.CHANGE_CONDITION, data_, token.Token);

                        Console.Write("Hello");
                    }

                }

                //LoadMovementAssets();

                //progress.Hide();

            }


            catch (Exception err)
            {
                Toast.MakeText(Application.Context, "" + err + "", ToastLength.Long).Show();

            }
        }

        public async override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {

            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
            var main_activity = (MainActivity)this.Activity;

            if (requestCode == SCAN_CODE_RESULT && data != null)
            {
                ProgressDialog progress = new ProgressDialog(this.Context);
                progress.SetTitle("Adding Asset");
                progress.SetMessage("Please wait..");
                progress.Show();

                var codes = data.GetStringExtra("barcode");

                List<string> code_list = JsonConvert.DeserializeObject<List<string>>(codes);

                foreach(string code in code_list)
                {

                    var elected = main_activity.master_data.Assets.Where(i => i.Tag.ToString() == code).ToList();
                    if(elected.Count()>0){
                        using (var conn = new Connect())
                        {
                            var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                            object _data = null;

                            if (Movement == null)
                            {
                                _data = new { tag = code, direction = "fin", reason = "check in", condition = "exellent", date = DateTime.UtcNow, site_id = 1 };
                            }
                            else
                            {
                                _data = new { tag = code, direction = "fin", reason = "check in", condition = "exellent", date = DateTime.UtcNow, movement_id = Movement.Id, site_id = 1 };
                            }

                            var res = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, _data, token.Token);


                            LoadMovementAssets();
                        }
                    }

                }
                            progress.Hide();
            }

            }


            catch (Exception err)
            {
                //Toast.MakeText(Application.Context, ""+ err+"", ToastLength.Long).Show();

            }
        }

    }
}