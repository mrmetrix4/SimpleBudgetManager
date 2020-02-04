using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    [Activity(Label = "LogsTabsActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LogsTabsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_logs_tabs);

            // Initializing
            Utils.CurrentContext = this;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            ActionBar.Title = "Actions log";

            // Using basic fragmants to show the Tabs view
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            SlidingTabsFragment fragment = new SlidingTabsFragment();
            transaction.Replace(Resource.Id.sampleContentFrag, fragment);
            transaction.Commit();
        }
    }
}