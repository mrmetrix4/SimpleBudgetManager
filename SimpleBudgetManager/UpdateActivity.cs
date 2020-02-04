using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SimpleBudgetManager
{
    [Activity(Label = "UpdateActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UpdateActivity : Activity, View.IOnClickListener
    {
        RadioGroup rg;
        RadioButton rInc, rExp;
        Button saveBTN;
        EditText amount;

        int pos;

        public void OnClick(View v)
        {
            if (v == rInc)
                ActionBar.Title = "Add an income";
            else if (v == rExp)
                ActionBar.Title = "Add an expense";
            else if (v == saveBTN)
                if (amount.Text == "" || amount.Text == "0")
                    Utils.ShowToast("Amount mustn't be zero!");
                else
                {
                    int mul = -1;
                    if (rg.CheckedRadioButtonId == rInc.Id)
                        mul = 1;

                    Account curr = Utils.AccList[pos];

                    double amo = double.Parse(amount.Text) * mul;

                    Utils.AccList[pos].Amount += amo;

                    Utils.sbmDB.Update(curr);
                    Utils.AddTrans(curr.Id, amo);
                    Finish();
                }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_update);

            // Initializing
            Utils.CurrentContext = this;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));

            TextView accountTV = FindViewById<TextView>(Resource.Id.accountTV);
            saveBTN = FindViewById<Button>(Resource.Id.saveBTN);
            rg = FindViewById<RadioGroup>(Resource.Id.rgUpdate);
            rInc = FindViewById<RadioButton>(Resource.Id.radioIncome);
            rExp = FindViewById<RadioButton>(Resource.Id.radioExpense);
            amount = FindViewById<EditText>(Resource.Id.amoUpdate);

            pos = Intent.GetIntExtra("pos", -1);

            accountTV.Text = Utils.AccList[pos].ToString();
            ActionBar.Title = "Add an expense";

            // Actions
            saveBTN.SetOnClickListener(this);
            rInc.SetOnClickListener(this);
            rExp.SetOnClickListener(this);
        }
    }
}