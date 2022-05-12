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
using System;
using SQLite;
using Newtonsoft.Json;

namespace AuditApp.Data
{   

    public class AreaSite
    { 
        
        [JsonProperty("branch")]
        public string Branch { get; set; }
      
        [JsonProperty("name")]
        public string Site { get; set; }
       
        [JsonProperty("id")]
        public int SiteId { get; set; }
        

    }
}