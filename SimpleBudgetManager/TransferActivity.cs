using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SimpleBudgetManager
{
    [Activity(Label = "TransferActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class TransferActivity : Activity, View.IOnClickListener
    {
        List<string> accNames;

        Spinner spinner;
        Button saveBTN;
        EditText amount;

        int pos;
        public void OnClick(View v)
        {
            if (v == saveBTN)
                if (amount.Text == "" || amount.Text == "0")
                    Utils.ShowToast("Amount mustn't be zero!");
                else
                {
                    int toPos = spinner.SelectedItemPosition;
                    // Since 'this' account is being removed from the names list, must update 'toPos' to the real position in AccList
                    if (toPos >= pos)
                        toPos++;

                    Account fromAcc = Utils.AccList[pos];
                    Account toAcc = Utils.AccList[toPos];

                    double amo = double.Parse(amount.Text);

                    Utils.AccList[pos].Amount -= amo;
                    Utils.AccList[toPos].Amount += amo;

                    Utils.sbmDB.Update(fromAcc);
                    Utils.sbmDB.Update(toAcc);

                    Utils.AddTrans(toAcc.Id, fromAcc.Id, double.Parse(amount.Text));
                    Finish();
                }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_transfer);

            // Initializing
            Utils.CurrentContext = this;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            ActionBar.Title = "Transfer money";

            accNames = new List<string>();
            foreach (Account acc in Utils.AccList)
                accNames.Add(acc.Name);

            pos = Intent.GetIntExtra("pos", -1);
            accNames.RemoveAt(pos);

            TextView accountTV = FindViewById<TextView>(Resource.Id.accountTV);
            spinner = FindViewById<Spinner>(Resource.Id.spinner);
            saveBTN = FindViewById<Button>(Resource.Id.saveBTN);
            amount = FindViewById<EditText>(Resource.Id.amoUpdate);

            accountTV.Text = Utils.AccList[pos].ToString();
            spinner.Adapter = new SpinnerAdapter(this, accNames);

            // Actions
            saveBTN.SetOnClickListener(this);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerItemClick);
        }

        private void SpinnerItemClick(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ActionBar.Title = Utils.AccList[pos].Name + " to " + accNames[e.Position];
        }
    }
}