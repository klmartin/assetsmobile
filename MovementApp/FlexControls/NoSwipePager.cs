using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovementApp.FlexControls
{

    public class NoSwipePager : ViewPager
    {
        public NoSwipePager(Context _context) : base(_context)
        {

        }

        public NoSwipePager(Context _context, IAttributeSet _attrs) : base(_context, _attrs)
        {

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false;
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false;
        }
    }
}