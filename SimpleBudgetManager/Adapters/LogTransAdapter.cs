using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    public class LogTransAdapter : BaseAdapter<LogTrans>
    {
        readonly Context context;
        readonly List<LogTrans> objs;

        public LogTransAdapter(Context context, List<LogTrans> objs)
        {
            this.context = context;
            this.objs = objs;
        }

        public List<LogTrans> GetList()
        {
            return this.objs;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return this.objs.Count; }
        }
        public override LogTrans this[int position]
        {
            get { return this.objs[position]; }
        }

        // Returns the view, mathces the LogTrans properties
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((LogsTabsActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.log_list_item, parent, false);
            TextView nameTV = view.FindViewById<TextView>(Resource.Id.nameTV);
            TextView amountTV = view.FindViewById<TextView>(Resource.Id.amountTV);
            TextView dateTV = view.FindViewById<TextView>(Resource.Id.exTV);

            LogTrans curr = objs[position];

            if (curr != null)
            {
                Account accMain = Utils.FullAccList.Find(i => i.Id == curr.MainAccID);
                Account accSec = curr.SecAccID != -1 ? Utils.FullAccList.Find(i => i.Id == curr.SecAccID) : null;

                nameTV.Text = accSec == null ? accMain.Name : accSec.Name + " to " + accMain.Name;

                nameTV.SetWidth((int)Math.Floor(Resources.System.DisplayMetrics.WidthPixels * 0.4));
                amountTV.Text = string.Format("{0:n}", curr.Value);
                amountTV.SetTextColor(curr.Value < 0 ? Color.Red : Color.Black);
                dateTV.Text = curr.Date.ToString("MMM d");
                if (!accMain.IsActive || (accSec != null && !accSec.IsActive))
                    nameTV.PaintFlags |= PaintFlags.StrikeThruText;
            }

            return view;
        }
    }
}