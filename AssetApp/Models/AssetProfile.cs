using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetApp.Models
{
    public class AssetProfile
    {

        [JsonProperty("asset")]
        public Object Asset { get; set; }

        [JsonProperty("attributes")]
        public Object Attributes { get; set; }

        [JsonProperty("current_movement_site")]
        public Object CurrentMovementSite { get; set; }

        [JsonProperty("movements")]
        public Object Movements { get; set; }

        [JsonProperty("currenct_incidence_site")]
        public Object CurrenctIncidenceSite { get; set; }

        [JsonProperty("incidences")]
        public Object Incidences { get; set; }


        [JsonProperty("currenct_audit_site")]
        public Object CurrenctAuditSite { get; set; }

        [JsonProperty("purchase")]
        public Object Purchase { get; set; }

        [JsonProperty("donation")]
        public Object Donation { get; set; }


        [JsonProperty("audits")]
        public Object Audits { get; set; }



    }


    public class AssetProfileAsset
    {


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("controller_first_name")]
        public string ControllerFirstName { get; set; }

        [JsonProperty("controller_last_name")]
        public string ControllerLastName { get; set; }

        [JsonProperty("assignee_first_name")]
        public string AsigneeFirstName { get; set; }


        [JsonProperty("assignee_last_name")]
        public string AssigneeLastName { get; set; }

        [JsonProperty("book_value")]
        public string BookValue { get; set; }

        [JsonProperty("purchase_price")]
        public string PurchasePrice { get; set; }



    }


    public class CurrentMovementSite
    {

        [JsonProperty("date_time")]
        public string Date { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class AssetPurchase
    {

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("date_time")]
        public string Date { get; set; }
    }


    public class AssetProfileAttributes
    {


        [JsonProperty("name")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }



    }


    public class AssetProfileMovements
    {



        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("date_time")]
        public string Date { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("site_name")]
        public string Site { get; set; }


    }

    public class AssetProfileAudit
    {
    //            "audit_id": "7",
    //"id": "14",
    //"title": "this is audit 1",
    //"description": "This is the description for audit 1",
    //"date_time": "2022-02-25 08:04:16",
    //"site_id": "21",
    //"site_name": "ARUSHA#SERVER",
    //"image": null,
    //"condition": null,
    //"value": null,
    //"currency": "TZS"

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("date_time")]
        public string Date { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("site_name")]
        public string Site { get; set; }

    }


    public class AssetSearchTag
    {


        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


    }





}
