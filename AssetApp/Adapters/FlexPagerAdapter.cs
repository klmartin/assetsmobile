using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetApp.Adapters
{
    public class FlexPagerAdapter : FragmentPagerAdapter
    {
        private IList<AndroidX.Fragment.App.Fragment> FlexFragmentList = new List<AndroidX.Fragment.App.Fragment>();
        private IList<string> FlexFragmentTitleList = new List<string>();


        public FlexPagerAdapter(AndroidX.Fragment.App.FragmentManager fm) : base(fm,0)
        {

        }
        public void AddFragment(AndroidX.Fragment.App.Fragment Fragment, string Title)
        {
            FlexFragmentList.Add(Fragment);
            FlexFragmentTitleList.Add(Title);
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(FlexFragmentTitleList[position]);
        }

        public override int Count
        {
            get
            {
                return FlexFragmentList.Count;
                //throw new NotImplementedException();
            }
        }

        public override AndroidX.Fragment.App.Fragment GetItem(int position)
        {
            return FlexFragmentList[position];
        }
    }
}