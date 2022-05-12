using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AssetApp.Adapters;
using AssetApp.Data;
using AssetApp.Models;
using Newtonsoft.Json;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using SupportFragment = AndroidX.Fragment.App.Fragment;

namespace AssetApp.Fragments
{
    public class AssetInfoFragment : SupportFragment
    {
        public AppAsset asset;
        public TextView _asset_name, _asset_tag, _asset_location;
        public ImageView _asset_image;
        public LinearLayout linear_layout;

        //private FlexRecyclerViewAdapter<Models.KeyValue, Holders.KeyValueViewHolder> assetsAdapter;

        private RecyclerView attributeRecyclerView;
        private RecyclerView.LayoutManager rvLayoutManager;

        private FlexRecyclerViewAdapter<Models.AssetProfileAttributes, Holders.AttributeViewHolder> AttributeAdapter;


        private RecyclerView movementsRecyclerView;

        private FlexRecyclerViewAdapter<Models.AssetProfileMovements, Holders.MovementViewHolder> MovementAdapter;

        private TextView _assetNameField;
        private TextView _assetTagField;
        private TextView _assetLocationField;
        private TextView _assetPriceField;
        private TextView _assetDescriptionField;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_asset_info, container, false);


            _assetNameField = view.FindViewById<TextView>(Resource.Id.tv_asset_name);
            _assetTagField = view.FindViewById<TextView>(Resource.Id.tv_asset_tag);
            _assetDescriptionField = view.FindViewById<TextView>(Resource.Id.tv_asset_description);
            attributeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvAssetAttributes);
            _assetLocationField = view.FindViewById<TextView>(Resource.Id.tv_asset_location);
            _assetPriceField = view.FindViewById<TextView>(Resource.Id.tv_asset_Price);

            /*            var asset_activity = (MainActivity)this.Activity;
                        asset = asset_activity._asset;

                        _asset_name = view.FindViewById<TextView>(Resource.Id.tv_asset_name);
                        _asset_name.Text = asset.Name;

                        _asset_tag = view.FindViewById<TextView>(Resource.Id.tv_asset_tag);
                        _asset_tag.Text = asset.Tag;

                        _asset_location = view.FindViewById<TextView>(Resource.Id.tv_asset_location);

                        _asset_image = view.FindViewById<ImageView>(Resource.Id.asset_info_image);
                        Picasso.Get().Load(asset.Image).Into(_asset_image);*/



            //var _assets_string = JsonConvert.SerializeObject(assets);
            //_assets = JsonConvert.DeserializeObject<List<Models.Line>>(_assets_string);


            //attributeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvAssetAttributes);



            LoadDetails("100");
            return view;

        }

        /*        private async void LoadAttributes()
                {
                    var cat_attrs = FlexAppDatabase.GetCategoryAttributes(asset.CategoryId).ToList();
                    var asset_attrs = FlexAppDatabase.GetAttributes(asset.Id).ToList();

                    List<KeyValue> attribute_values = new List<KeyValue>();

                    foreach (var cat_attr in cat_attrs)
                    {
                        var asset_attr = asset_attrs.Where(x => x.Name == cat_attr.Name).FirstOrDefault();
        repl
                        KeyValue attribute = new KeyValue()
                        {
                            KeyId = cat_attr.Id,
                            Key = cat_attr.Name,
                        };

                        if (asset_attr != null)
                        {
                            attribute.Id = asset_attr.Id;
                            attribute.Value = asset_attr.Value;
                        }

                        attribute_values.Add(attribute);
                    }


                    assetsAdapter = new FlexRecyclerViewAdapter<Models.KeyValue, Holders.KeyValueViewHolder>(this.Context, attribute_values, (thing, holder, view) =>
                    {
                        holder.Key.Text = thing.Key;
                        holder.Value.Text = thing.Value;


                        holder.Value.FocusChange += delegate (object sender, View.FocusChangeEventArgs e)
                        {

                            AppAttribute _attr = new AppAttribute();

                            if (!e.HasFocus && holder.Value.Text != string.Empty)
                            {
                                if (thing.Value == null)
                                {
                                    var _cat_attr = cat_attrs.Where(x => x.Id == thing.KeyId).FirstOrDefault();
                                    _attr.Name = _cat_attr.Name;
                                    _attr.Datatype = _cat_attr.Datatype;
                                    _attr.Description = _cat_attr.Description;
                                    _attr.Units = _cat_attr.Units;
                                    _attr.AssetId = asset.Id;
                                    _attr.Created = DateTime.Now;
                                    _attr.Updated = DateTime.Now;

                                }
                                else
                                {
                                    _attr = asset_attrs.Where(x => x.Name == thing.Key).FirstOrDefault();
                                }

                                _attr.Value = holder.Value.Text;

                                SubmitAttribute(_attr);

                                bool saved_attr = FlexAppDatabase.SaveOrUpdateAttribute(_attr);
                            }


                        };

                    }, Resource.Layout.flex_item_keyvalue);

                    rvLayoutManager = new LinearLayoutManager(this.Context);

                    attributeRecyclerView.SetAdapter(assetsAdapter);
                    attributeRecyclerView.SetLayoutManager(rvLayoutManager);
                }
        */
        private async void SubmitAttribute(AppAttribute attr)
        {
            using (Connect connect = new Connect())
            {
                var assetAct = (MainActivity)this.Activity;
                var token = JsonConvert.DeserializeObject<AppToken>(assetAct._token).Token;

                var _asset = new { name = attr.Name, data_type = attr.Datatype, value = attr.Value, units = attr.Units, created_at = attr.Created, updated_at = attr.Updated, asset_id = asset.GlobalId };
                var response = await connect.PostAsync(Defaults.SAVE_ATTRIBUTE, _asset, token);

            }
        }




        private async void LoadDetails(string term)
        {



            var token = FlexAppDatabase.GetTokens().FirstOrDefault();

            List<Models.Line> _assets = new List<Models.Line>();

            using (var conn = new Connect())
            {
                string search_result = await conn.GetAsync(Defaults.ASSET_DETAILS + "/"+ term, token.Token);

                //search_result = JsonConvert.SerializeObject(search_result);
                var response = JsonConvert.DeserializeObject<Models.AssetProfile>(search_result);




                //var attributes = response.Attributes;
                //var currentMovementSite = response.CurrentMovementSite;
                var movements = response.Movements;
                var currenctIncidenceSite = response.CurrenctIncidenceSite;
                var incidences = response.Incidences;
                var currenctAuditSite = response.CurrenctAuditSite;
                var purchase = response.Purchase;
                var donation = response.Donation;
                var audits = response.Audits;
                //var asset = response.Asset;



                var asset = JsonConvert.SerializeObject(response.Asset);
                //var assets_details = JsonConvert.DeserializeObject<List<AppAsset>>(data).Select(x => new { Id = x.Id, TextPrimary = x.Name, TextSecondary = x.Description, Image = x.Image }).ToList();
                var assets_details = JsonConvert.DeserializeObject<Models.AssetProfileAsset>(asset);

                //Console.Write(assets_details.Name);


                var AssetName = assets_details.Name;
                var AssetDescription = assets_details.Description;
                var AssetCode = assets_details.Code;
                var AssetTag = assets_details.Tag;
                var AssetCategory = assets_details.Category;
                var AssetImage = assets_details.Image;


                var currentMovementSite = JsonConvert.SerializeObject(response.CurrentMovementSite);
                //var assets_details = JsonConvert.DeserializeObject<List<AppAsset>>(data).Select(x => new { Id = x.Id, TextPrimary = x.Name, TextSecondary = x.Description, Image = x.Image }).ToList();
                var currentMovementSite_details = JsonConvert.DeserializeObject<Models.CurrentMovementSite>(currentMovementSite);




                var assetPurchase = JsonConvert.SerializeObject(response.Purchase);
                //var assets_details = JsonConvert.DeserializeObject<List<AppAsset>>(data).Select(x => new { Id = x.Id, TextPrimary = x.Name, TextSecondary = x.Description, Image = x.Image }).ToList();
                var assetPurchase_details = JsonConvert.DeserializeObject<Models.AssetPurchase>(assetPurchase);


                _assetNameField.Text = AssetName;


                _assetTagField.Text = AssetTag;

                _assetDescriptionField.Text = AssetDescription;

                _assetLocationField.Text = (currentMovementSite_details.Name).Replace('#', '-');

                _assetPriceField.Text = assetPurchase_details.Price + ' ' + assetPurchase_details.Currency;


                List<Models.AssetProfileAttributes> _attributes = new List<Models.AssetProfileAttributes>();

                string attributes = JsonConvert.SerializeObject(response.Attributes);


                _attributes = JsonConvert.DeserializeObject<List<Models.AssetProfileAttributes>>(attributes);



                //List<Models.AssetProfileMovements> _Movements = new List<Models.AssetProfileMovements>();

                //string Movements = JsonConvert.SerializeObject(response.Movements);


                //_Movements = JsonConvert.DeserializeObject<List<Models.AssetProfileMovements>>(Movements);




                //AttributeAdapter = new FlexRecyclerViewAdapter<Models.AssetProfileAttributes, Holders.AttributeViewHolder>(this.Context, _attributes, (thing, holder, view) =>
                //{
                //    holder.Label.Text = thing.Label;
                //    holder.Value.Text = thing.Value;


                //}, Resource.Layout.detail_item);

                //rvLayoutManager = new LinearLayoutManager(this.Context);

                //attributeRecyclerView.SetAdapter(AttributeAdapter);
                //attributeRecyclerView.SetLayoutManager(rvLayoutManager);


                string attribute="here";
                



                //MovementAdapter = new FlexRecyclerViewAdapter<Models.AssetProfileMovements, Holders.MovementViewHolder>(this.Context, _Movements, (thing, holder, view) =>
                //{
                //    holder.Reason.Text = thing.Reason;
                //    holder.Date.Text = thing.Date;
                //    holder.Direction.Text = thing.Direction;
                //    holder.Condition.Text = thing.Condition;
                //    holder.Site.Text = thing.Site;


                //}, Resource.Layout.detail_item);

                ////rvLayoutManager = new LinearLayoutManager(this.Context);

                //movementsRecyclerView.SetAdapter(AttributeAdapter);
                //movementsRecyclerView.SetLayoutManager(rvLayoutManager);


                //MovementAdapter = new FlexRecyclerViewAdapter<Models.AssetProfileMovements, Holders.MovementViewHolder>(this.Context, _Movements, (thing, holder, view) =>
                //{
                //    holder.Reason.Text = thing.Reason;
                //    holder.Date.Text = thing.Date;
                //    holder.Direction.Text = thing.Direction;
                //    holder.Condition.Text = thing.Condition;
                //    holder.Site.Text = thing.Site;


                //}, Resource.Layout.detail_item);

                //rvLayoutManager = new LinearLayoutManager(this.Context);

                //movementsRecyclerView.SetAdapter(AttributeAdapter);
                //movementsRecyclerView.SetLayoutManager(rvLayoutManager);




                /*         _asset_image = view.FindViewById<ImageView>(Resource.Id.asset_info_image);
                         Picasso.Get().Load(asset.Image).Into(_asset_image); */

            }
        }
    }
}


/*{
    "asset": {
        "id": "100",
        "name": "Dell Monitor",
        "description": "Dell Monitor",
        "code": "LHRC/AR/LAC/SC/03",
        "tag": "301000100",
        "category_id": "1",
        "category": "Computer Equipment",
        "image": null
    },
    "attributes": [
        {
        "id": 298,
            "name": "LABEL PRINTING",
            "data_type": "text",
            "value": "YES",
            "units": "units"
        },
        {
        "id": 299,
            "name": "PART_NUMBER",
            "data_type": "text",
            "value": "E1912Hf",
            "units": "units"
        },
        {
        "id": 300,
            "name": "SERIAL NUMBER",
            "data_type": "text",
            "value": "CN-OKCCP-72872-2CE-C54M",
            "units": "units"
        }
    ],
    "current_movement_site": {
        "date_time": "2022-02-08 14:44:18",
        "name": "ARUSHA#LAND UNIT",
        "id": "16"
    },
    "movements": [
        {
        "movement_id": "100",
            "id": "100",
            "reason": "Purchase",
            "date_time": "2022-02-08 14:44:18",
            "direction": "fin",
            "condition": "exellent",
            "site_name": "ARUSHA#LAND UNIT"
        }
    ],
    "currenct_incidence_site": null,
    "incidences": [],
    "currenct_audit_site": null,
    "purchase": {
        "code": "qqu625202w",
        "date_time": null,
        "price": "0.00",
        "quantity": "1",
        "currency": "TZS"
    },
    "donation": null,
    "audits": {
    [
     {
        "audit_id": "7",
    "id": "14",
    "title": "this is audit 1",
    "description": "This is the description for audit 1",
    "date_time": "2022-02-25 08:04:16",
    "site_id": "21",
    "site_name": "ARUSHA#SERVER",
    "image": null,
    "condition": null,
    "value": null,
    "currency": "TZS"
     }
]}
}*/