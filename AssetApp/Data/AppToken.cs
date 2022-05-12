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
    [Table("AppTokens")]
    public class AppToken
    {
        [PrimaryKey,AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("assigned")]
        public DateTime Assigned { get; set; }
        [Column("expires")]
        public DateTime Expires { get; set; }
    }
}