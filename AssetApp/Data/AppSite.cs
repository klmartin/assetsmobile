using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetApp.Data
{
    [Table("AppSites")]
    public class AppSite
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("created_at")]
        public DateTime Created { get; set; }
        [Column("updated_at")]
        public DateTime Updated { get; set; }
    }
}