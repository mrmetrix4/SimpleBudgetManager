using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    class SpinnerAdapter : BaseAdapter
    {
        readonly Context context;
        readonly List<string> objs;
        public SpinnerAdapter(Context context, List<string> objs)
        {
            this.context = context;
            this.objs = objs;
        }

        public override int Count
        {
            get { return this.objs.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return objs[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        // Sets a spinner item (with a larger padding)
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((TransferActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.spinner_custom_layout, parent, false);
            TextView spinnerTV = view.FindViewById<TextView>(Resource.Id.spinnerTV);

            spinnerTV.Text = objs[position];

            return view;
        }
    }
}