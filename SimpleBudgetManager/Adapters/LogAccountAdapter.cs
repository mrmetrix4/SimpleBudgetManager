using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    public class LogAccountAdapter : BaseAdapter<LogAccount>
    {
        readonly Context context;
        readonly List<LogAccount> objs;

        public LogAccountAdapter(Context context, List<LogAccount> objs)
        {
            this.context = context;
            this.objs = objs;
        }

        public List<LogAccount> GetList()
        {
            return objs;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return this.objs.Count; }
        }
        public override LogAccount this[int position]
        {
            get { return this.objs[position]; }
        }

        // Returns the view, mathces the LogAccount properties
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((LogsTabsActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.log_list_item, parent, false);
            TextView nameTV = view.FindViewById<TextView>(Resource.Id.nameTV);
            TextView amountTV = view.FindViewById<TextView>(Resource.Id.amountTV);
            TextView dateTV = view.FindViewById<TextView>(Resource.Id.exTV);

            LogAccount curr = objs[position];

            if (curr != null)
            {
                Account accMain = Utils.FullAccList.Find(i => i.Id == curr.MainAccID);
                nameTV.Text = accMain.Name + " was";
                nameTV.SetTextColor(Color.Black);
                switch (curr.Type)
                {
                    case 0:
                        nameTV.Text += " added";
                        break;
                    case 1:
                        nameTV.Text += " deleted";
                        nameTV.SetTextColor(Color.Red);
                        break;
                    case 2:
                    case 3:
                    case 5:
                        nameTV.Text += " changed";
                        break;
                }
                nameTV.SetWidth((int)Math.Floor(Resources.System.DisplayMetrics.WidthPixels * 0.4));
                amountTV.Text = string.Format("{0:n}", curr.Value);
                dateTV.Text = curr.Date.ToString("MMM d");
            }

            return view;
        }
    }
}