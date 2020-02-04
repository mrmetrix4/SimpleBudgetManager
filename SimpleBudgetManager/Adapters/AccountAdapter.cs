using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    public class AccountAdapter : BaseAdapter<Account>
    {
        readonly Context context;
        readonly List<Account> objs;

        public AccountAdapter(Context context, List<Account> objs)
        {
            this.context = context;
            this.objs = objs;
        }

        public List<Account> GetList()
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
        public override Account this[int position]
        {
            get { return this.objs[position]; }
        }

        // Returns the view, mathces the account properties
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((MainActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.list_item, parent, false);
            TextView nameTV = view.FindViewById<TextView>(Resource.Id.nameTV);
            TextView amountTV = view.FindViewById<TextView>(Resource.Id.amountTV);
            CheckBox cbWallet = view.FindViewById<CheckBox>(Resource.Id.cbWallet);

            Account curr = this.objs[position];

            if (curr != null)
            {
                nameTV.Text = curr.Name;
                amountTV.Text = string.Format("{0:n}", curr.Amount);
                if (curr.Amount < 0)
                    amountTV.SetTextColor(Android.Graphics.Color.Red);
                else
                    amountTV.SetTextColor(Android.Graphics.Color.Black);
                cbWallet.Checked = curr.IsWallet ? true : false;
            }

            return view;
        }
    }
}