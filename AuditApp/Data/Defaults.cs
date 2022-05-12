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

namespace AuditApp.Data
{
    public static class Defaults
    {

        public static string ROOT = "https://citsapps.com/lhrc/";
        //public static string ROOT = "http://10.0.16.250/assetsnew/public/";

        public static string ACCESS_TOKEN = "api/token";

        public static string MASTER_DATA = "api/masterdata";
        public static string ASSETS = "api/assets";
        public static string TASKS = "api/tasks";
        public static string UPLOAD_FILE = "api/upload";

        public static string SAVE_ASSET = "api/assets/save";
        public static string SAVE_ATTRIBUTE = "api/attributes/save";
        public static string SEARCH_ASSET = "api/assets/search/";
        public static string GET_ASSETS = "api/assets/list";
        public static string ASSET_DETAILS = "api/assets/details";

        public static string SAVED_IN_MOVEMENTS = "api/movements/saved/in";
        public static string SAVED_OUT_MOVEMENTS = "api/movements/saved/out";
        public static string SAVED_FULL_MOVEMENTS = "api/movements/saved/full";
        public static string ADD_MOVEMENT_ASSET = "api/movements/addasset";
        public static string SUBMIT_MOVEMENT = "api/movements/submit";
        public static string SITES = "api/movements/locations";

        public static string SAVED_INCIDENCES = "api/incidences/saved";
        public static string ADD_INCIDENCE_ASSET = "api/incidences/addasset";
        public static string SUBMIT_INCIDENCE = "api/incidences/submit";

        public static string SAVED_AUDITS = "api/audits/saved";
        public static string ADD_AUDITS_ASSET = "api/audits/addasset";
        public static string SUBMIT_AUDIT = "api/audits/submit";
        public static string GET_AUDIT_SITES = "api/select/areas";
    }
}