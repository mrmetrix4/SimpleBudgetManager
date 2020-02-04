using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;
using System;

namespace SimpleBudgetManager
{
    [Activity(Label = "AccountAddActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AccountAddActivity : Activity, View.IOnClickListener
    {
        Button addBTN;
        EditText accName, inAmo;
        CheckBox cbWallet;
        public void OnClick(View v)
        {
            if (v == addBTN)
                if (accName.Text.Length < 2)
                    Utils.ShowToast("The name must contain at least 2 letters!");
                else
                {
                    double inAmoD = inAmo.Text == "" ? 0 : double.Parse(inAmo.Text);
                    Account newAcc = new Account(accName.Text, inAmoD, cbWallet.Checked);
                    Utils.sbmDB.Insert(newAcc);
                    Utils.AccList.Add(newAcc);
                    Utils.FullAccList.Add(newAcc);
                    Utils.AddAccountLog(newAcc.Id, inAmoD);
                    Finish();
                }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_account_add);

            // Initializing
            Utils.CurrentContext = this;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            ActionBar.Title = "Add an account";

            addBTN = FindViewById<Button>(Resource.Id.addBTN);
            accName = FindViewById<EditText>(Resource.Id.accName);
            inAmo = FindViewById<EditText>(Resource.Id.accAmo);
            cbWallet = FindViewById<CheckBox>(Resource.Id.cbWallet);

            // Actions
            addBTN.SetOnClickListener(this);
        }
    }
}