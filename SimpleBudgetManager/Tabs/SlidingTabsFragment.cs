using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace SimpleBudgetManager
{
    [Obsolete]
    public class SlidingTabsFragment : Fragment
    {
        private SlidingTabScrollView mSlidingTabScrollView;
        private ViewPager mViewPager;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            mViewPager.Adapter = new SamplePagerAdapter();
            mSlidingTabScrollView.ViewPager = mViewPager;
        }

        public class SamplePagerAdapter : PagerAdapter
        {
            readonly List<string> items = new List<string>();
            LogTransAdapter LTAdapter;
            LogAccountAdapter LAAdapter;

            public SamplePagerAdapter() : base()
            {
                if (Utils.TransList.Count != 0)
                    items.Add("Transactions log");
                if (Utils.LogAccList.Count != 0)
                    items.Add("Accounts actions log");
            }

            public override int Count
            {
                get { return items.Count;  }
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object obj)
            {
                return view == obj;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item, container, false);
                container.AddView(view);
                ListView lv = view.FindViewById<ListView>(Resource.Id.listView);
                if (items[position] == "Transactions log")
                {
                    LTAdapter = new LogTransAdapter(lv.Context, Utils.TransList);
                    lv.Adapter = LTAdapter;

                    lv.ItemClick += LogTransClick;
                    lv.ItemLongClick += LogLongClick;
                }
                if (items[position] == "Accounts actions log")
                {
                    LAAdapter = new LogAccountAdapter(lv.Context, Utils.LogAccList);
                    lv.Adapter = LAAdapter;

                    lv.ItemClick += LogAccountClick;
                    lv.ItemLongClick += LogLongClick;

                }
                return view;
            }

            private void LogLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
            {
                if (Utils.RefreshDialog == null)
                {
                    Dialog logDia = Utils.MakeAlertDialog("Show all?", "Are you sure you want to show all the actions?", "Show All", delegate
                    {
                        Utils.ShowRefreshDialog();
                        Intent startService = new Intent(Utils.CurrentContext, typeof(RefreshService));
                        Utils.CurrentContext.StartService(startService);
                    });
                    logDia.Show();
                }
            }

            private void LogTransClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                Dialog logDia = Utils.MakeAlertDialog("Transaction", Utils.TransList[e.Position].AlertToString(), "Undo action", delegate {
                    Utils.TransList[e.Position].UndoAction();
                    LTAdapter.NotifyDataSetChanged();
                });
                logDia.Show();
            }

            private void LogAccountClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                Dialog logDia = Utils.MakeAlertDialog("Account action", Utils.LogAccList[e.Position].AlertToString(), "Undo action", delegate {
                    Utils.LogAccList[e.Position].UndoAction();
                    LAAdapter.NotifyDataSetChanged();
                });
                logDia.Show();
            }

            public string GetHeaderTitle (int position)
            {
                return items[position];
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }
        }
    }
}