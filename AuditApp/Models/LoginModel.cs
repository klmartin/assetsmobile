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

namespace AuditApp.Models
{
    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}