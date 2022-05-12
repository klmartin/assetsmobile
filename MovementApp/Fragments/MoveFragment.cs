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
    public class MoveFragment : SupportFragment
    {
        private const int SCAN_CODE_RESULT = 1010;

        private FlexRecyclerViewAdapter<Models.MovementAsset, Holders.AssetMovementViewHolder> assetsAdapter;

        private RecyclerView assetRecyclerView;
        private RecyclerView.LayoutManager rvLayoutManager;

        private SearchableSpinner search_asset_box, search_user_box, source, destination;
        private TextView submit_btn;

        public Movement MovementIn;
        public Movement MovementOut;
        public List<MovementAsset> Assets;

        FullMovementContainer data;
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
            View view = inflater.Inflate(Resource.Layout.fragment_move, container, false);

            search_asset_box = view.FindViewById<SearchableSpinner>(Resource.Id.et_move_asset);
            search_user_box = view.FindViewById<SearchableSpinner>(Resource.Id.et_move_user);
            source = view.FindViewById<SearchableSpinner>(Resource.Id.movement_full_source);
            destination = view.FindViewById<SearchableSpinner>(Resource.Id.movement_full_destination);

            assetRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvMoveAssets);

            submit_btn = view.FindViewById<TextView>(Resource.Id.move_submit_btn);
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

            string[] locations = main_activity.master_data.Locations.Select(x => x.Name.Split("#")[0]).ToArray();
            List<string> locations_array = new List<string>() { "___ Select Branch ___" };

            locations_array.AddRange(locations);

            ArrayAdapter locations_adapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, locations_array);
            source.Adapter = locations_adapter;
            destination.Adapter = locations_adapter;


            search_asset_box.ItemSelected += Search_asset_box_ItemSelected;

            LoadMovementAssets();

            return view;

        }

        private async void Submit_btn_Click(object sender, EventArgs e)
        {
            ProgressDialog progress = new ProgressDialog(this.Context);

            try
            {

                var selected = search_user_box.SelectedItem.ToString();

                if (search_user_box.SelectedItem.ToString() != "___ Select User ___" && source.SelectedItem.ToString() != "___ Select Branch ___" && destination.SelectedItem.ToString() != "___ Select Branch ___" && data.Assets != null)
                {
                    var token = FlexAppDatabase.GetTokens().FirstOrDefault();

                    progress.SetTitle("Loading");
                    progress.SetMessage("Please wait..");
                    progress.Show();

                    using (var conn = new Connect())
                    {
                        var response1 = await conn.PostAsync(Defaults.SUBMIT_MOVEMENT, new { id = MovementOut.Id, reason = MovementOut.Reason }, token.Token);
                        var response2 = await conn.PostAsync(Defaults.SUBMIT_MOVEMENT, new { id = MovementIn.Id, reason = MovementIn.Reason }, token.Token);
                        AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        alert.SetTitle("Succesfull");
                        alert.SetMessage("Movement is Succesfull");
                        alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        search_asset_box.SetSelection(0);
                        search_user_box.SetSelection(0);
                        source.SetSelection(0);
                        destination.SetSelection(0);
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
                //Toast.MakeText(Application.Context, "" + err + "", ToastLength.Long).Show();
                progress.Hide();

                AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                alert.SetTitle("Fail");
                alert.SetMessage("Movement Not Successful");
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
            }
        }

        private async void LoadMovementAssets()
        {
            List<Models.MovementAsset> myassets = new List<Models.MovementAsset>();



            var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            using (var conn = new Connect())
            {
                string json = await conn.GetAsync(Defaults.SAVED_FULL_MOVEMENTS, token.Token);
                data = JsonConvert.DeserializeObject<FullMovementContainer>(json);

                if (data.MovementIn != null)
                {
                    MovementIn = data.MovementIn;
                    MovementOut = data.MovementOut;
                    Assets = data.Assets;

                    foreach (var asset in data.Assets)
                    {

                        myassets.Add(asset);

                    }
                }
                else
                {
                    MovementIn = new Movement();
                }

                if (data.MovementOut != null)
                {
                    MovementOut = data.MovementOut;

                }
                else
                {
                    MovementOut = new Movement();
                }

            }


            assetsAdapter = new FlexRecyclerViewAdapter<Models.MovementAsset, Holders.AssetMovementViewHolder>(this.Context, myassets, (thing, holder, view) =>
            {
                if (holder != null && thing != null)
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

                        DeleteAsset(thing.Asset.Id, thing.MovementId);

                    };

                    holder.Condition.SetSelection(condition_adapter.GetPosition(thing.Condition));
                    holder.Condition.ItemSelected += delegate (object sender, AdapterView.ItemSelectedEventArgs e)

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
        private async void Search_asset_box_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                var selected = search_asset_box.SelectedItem.ToString();

                if (selected != "___ Select Asset ___")
                {

                    if (source.SelectedItem.ToString() != "___ Select Branch ___" && destination.SelectedItem.ToString() != "___ Select Branch ___")
                    {
                        ProgressDialog progress = new ProgressDialog(this.Context);
                        progress.SetTitle("Adding Asset");
                        progress.SetMessage("Please wait..");
                        progress.Show();
                        var tag = selected.Split(' ')[0];


                        var _source = source.SelectedItem.ToString();
                        var _destination = destination.SelectedItem.ToString();



                        var main_activity = (MainActivity)this.Activity;

                        MovementIn.SiteId = main_activity.master_data.Locations.Where(x => x.Name.Split("#")[0] == _source).FirstOrDefault().Id;
                        MovementOut.SiteId = main_activity.master_data.Locations.Where(x => x.Name.Split("#")[0] == _destination).FirstOrDefault().Id;

                        using (var conn = new Connect())
                        {
                            var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                            object data1 = null;
                            object data2 = null;

                            if (MovementIn.Id == null)
                            {
                                data1 = new { tag = tag, direction = "fout", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, site_id = MovementIn.SiteId ,is_full=true };
                                data2 = new { tag = tag, direction = "fin", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, site_id = MovementOut.SiteId, is_full = true };
                            }
                            else
                            {
                                data1 = new { tag = tag, direction = "fout", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, movement_id = MovementIn.Id, site_id = MovementIn.SiteId };
                                data2 = new { tag = tag, direction = "fin", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, movement_id = MovementOut.Id, site_id = MovementOut.SiteId };
                            }

                            var res1 = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data1, token.Token);
                            var res2 = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data2, token.Token);

                            LoadMovementAssets();

                            progress.Hide();
                        }
                    }


                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                        alert.SetTitle("Note");
                        alert.SetMessage("You must select source and Destination First");
                        alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }

                    search_asset_box.SetSelection(0);

                }
            }


            catch (Exception err)
            {
                //Toast.MakeText(Application.Context, "" + err + "", ToastLength.Long).Show();

            }

        }

        public async void DeleteAsset(int asset_id, int movement_id)
        {

            try
            {
            var selected = search_asset_box.SelectedItem.ToString();
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

        public async void SaveCondition(int asset_id, int movement_id, string condition)
        {

            try
            {
                //ProgressDialog progress = new ProgressDialog(this.Context);
                //progress.SetTitle("Set Condition " + condition);
                //progress.SetMessage("Please wait..");
                //progress.SetCancelable(false);
                //progress.Show();

                if (condition != "exellent")
                {
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

        public async override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {

            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == SCAN_CODE_RESULT && data != null)
            {

                ProgressDialog progress = new ProgressDialog(this.Context);
                progress.SetTitle("Adding Asset");
                progress.SetMessage("Please wait..");
                progress.Show();
                var codes = data.GetStringExtra("barcode");

                var main_activity = (MainActivity)this.Activity;
                List<string> code_list = JsonConvert.DeserializeObject<List<string>>(codes);

                foreach (string code in code_list)
                {
                    var elected = main_activity.master_data.Assets.Where(i => i.Tag.ToString() == code).ToList();
                    if (elected.Count() > 0)
                    {
                        using (var conn = new Connect())
                    {
                       

                            var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                            object data1 = null;
                            object data2 = null;

                            if (MovementIn.Id == null)
                            {
                                data1 = new { tag = code, direction = "fout", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, site_id = MovementIn.SiteId };
                                data2 = new { tag = code, direction = "fin", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, site_id = MovementOut.SiteId };
                            }
                            else
                            {
                                data1 = new { tag = code, direction = "fout", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, movement_id = MovementIn.Id, site_id = MovementIn.SiteId };
                                data2 = new { tag = code, direction = "fin", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, movement_id = MovementOut.Id, site_id = MovementOut.SiteId };
                            }

                            var res1 = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data1, token.Token);
                            var res2 = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data2, token.Token);
                        }
                    }

                }


                LoadMovementAssets();
                progress.Hide();



                //    

                //    var elected = main_activity.master_data.Assets.Where(i => i.Tag.ToString() == code).ToList();


                //    if (elected.Count() > 0)
                //    {


                //        using (var conn = new Connect())
                //        {
                //            var token = FlexAppDatabase.GetTokens().FirstOrDefault();
                //            object data1 = null;
                //            object data2 = null;

                //            if (MovementIn.Id == null)
                //            {
                //                data1 = new { tag = code, direction = "fout", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, site_id = MovementIn.SiteId };
                //                data2 = new { tag = code, direction = "fin", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, site_id = MovementOut.SiteId };
                //            }
                //            else
                //            {
                //                data1 = new { tag = code, direction = "fout", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, movement_id = MovementIn.Id, site_id = MovementIn.SiteId };
                //                data2 = new { tag = code, direction = "fin", reason = "Moving", condition = "exellent", date = DateTime.UtcNow, movement_id = MovementOut.Id, site_id = MovementOut.SiteId };
                //            }

                //            var res1 = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data1, token.Token);
                //            var res2 = await conn.PostAsync(Defaults.ADD_MOVEMENT_ASSET, data2, token.Token);
                //        }

                //        LoadMovementAssets();
                //    }
                //}
            }
            }


            catch (Exception err)
            {
                //Toast.MakeText(Application.Context, "" + err + "", ToastLength.Long).Show();

            }
        }
    }
}