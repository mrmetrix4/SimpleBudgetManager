using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using System;
using Android.Content.PM;
using Android.Support.V7.App;
using Plugin.LocalNotifications;

namespace SimpleBudgetManager
{
    [Activity(Label = "Simple Budget Manager", Theme = "@style/AppTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, AdapterView.IOnItemClickListener, AdapterView.IOnItemLongClickListener
    {
        AccountAdapter adapter;

        bool isInEdit = false; // Indicates whether the user is in edit mode

        TextView walletTV, editTV;

        Dialog bankDia;
        EditText bankNumET;

        string bankNum;
        IMenuItem actions;

        Toolbar tb;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Initializing
            Utils.CurrentContext = this;

            tb = FindViewById<Toolbar>(Resource.Id.toolbar);

            SetActionBar(tb);
            ActionBar.Title = "Simple Budget Manager";

            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.Piggy; // Sets notifications' icon

            Utils.sbmDB.CreateTables<Account, LogTrans, LogAccount>();

            ListView lv = FindViewById<ListView>(Resource.Id.listView);
            walletTV = FindViewById<TextView>(Resource.Id.walletTV);
            editTV = FindViewById<TextView>(Resource.Id.editTV);

            bankNum = Utils.prefs.GetString("bankNum", "");
            bool firstOpened = Utils.prefs.GetBoolean("firstOpen", true);
            Utils.sPeditor.PutBoolean("firstOpen", false);
            Utils.sPeditor.Apply();

            Utils.RefreshAccounts();
            Utils.RefreshTrans(false);
            Utils.RefreshAccountsLog(false);

            adapter = new AccountAdapter(this, Utils.AccList);
            lv.Adapter = adapter;

            IntentFilter timeInt = new IntentFilter(Intent.ActionTimeTick);
            RegisterReceiver(new MinChangeReceiver(), timeInt); // Sets the broadcast reciever since it can't be set from manifest

            // Actions
            lv.OnItemClickListener = this;
            lv.OnItemLongClickListener = this;
            if (firstOpened)
                Utils.ShowDialog(Resource.Layout.dialog_help);

            tb.InflateMenu(Resource.Menu.main_menu);
            actions = tb.Menu.GetItem(1);
            actions.SetVisible(Utils.TransList.Count == 0 && Utils.LogAccList.Count == 0 ? false : true); // Leaves out "Action log" option when there are no actions
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (isInEdit)
            {
                Intent toEditAct = new Intent(this, typeof(AccountEditActivity));
                toEditAct.PutExtra("pos", position);
                StartActivity(toEditAct);
            }
            else
            {
                Intent toUpdateAct = new Intent(this, typeof(UpdateActivity));
                toUpdateAct.PutExtra("pos", position);
                StartActivity(toUpdateAct);
            }
        }
        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            if (isInEdit || Utils.AccList.Count == 1)
            {
                Intent toAddAct = new Intent(this, typeof(AccountAddActivity));
                StartActivity(toAddAct);
                return true;
            }
            else
            {
                Intent toTransferAct = new Intent(this, typeof(TransferActivity));
                toTransferAct.PutExtra("pos", position);
                StartActivity(toTransferAct);
                return true;
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.editMode)
            {
                isInEdit = !isInEdit;
                editTV.Visibility = isInEdit ? ViewStates.Visible : ViewStates.Gone;
            }
            else if (item.ItemId == Resource.Id.actionsLog)
            {
                Intent toActionsLog = new Intent(this, typeof(LogsTabsActivity));
                StartActivity(toActionsLog);
            }
            else if (item.ItemId == Resource.Id.help)
                Utils.ShowDialog(Resource.Layout.dialog_help);
            else if (item.ItemId == Resource.Id.call)
            {
                if (bankNum != "")
                    Utils.MakeAlertDialog("Call bank?", "About to call your bank at " + bankNum, "Call", CallBank, "Change num", ChangeBankNumDia).Show();
                else
                    ChangeBankNumDia(null, null);
            }

            return base.OnOptionsItemSelected(item);
        }
        // Shows a dialog that allows the user to change bank number
        private void ChangeBankNumDia(object sender, DialogClickEventArgs e)
        {
            bankDia = Utils.MakeDialog(Resource.Layout.dialog_bank);

            Button insBankBtn = bankDia.FindViewById<Button>(Resource.Id.insBTN);
            Button callBankBtn = bankDia.FindViewById<Button>(Resource.Id.callBTN);
            bankNumET = bankDia.FindViewById<EditText>(Resource.Id.bankNum);

            insBankBtn.Click += InsBankBtnClick;
            callBankBtn.Click += delegate
            {
                InsBankBtnClick(sender, e);
                if (bankNumET.Text.Length >= 7)
                    CallBank(sender, e);
            };

            bankDia.Show();
        }
        // Called when the insert or call button of the bank dialog is clicked
        private void InsBankBtnClick(object sender, EventArgs e)
        {
            
            //Programmer codes (Insert into bank number to activate):
            //'3386' - delets all user's data
            //'0988' - delets all logs
            if (bankNumET.Text.Length < 7 && bankNumET.Text != "3386" && bankNumET.Text != "0988")
                    Utils.ShowToast("Phone number must contain at least 7 digits");
            else if (bankNumET.Text == "3386")
            {
                Utils.sbmDB.DeleteAll<Account>();
                Utils.sbmDB.DeleteAll<LogTrans>();
                Utils.sbmDB.DeleteAll<LogAccount>();
                Utils.sPeditor.PutBoolean("firstOpen", true);
                Utils.sPeditor.PutString("bankNum", "");
                Utils.sPeditor.Apply();
                Finish();
            }
            else if (bankNumET.Text == "0988")
            {
                Utils.sbmDB.DeleteAll<LogTrans>();
                Utils.sbmDB.DeleteAll<LogAccount>();
                Finish();
            }
            else
            {
                bankNum = bankNumET.Text;
                Utils.sPeditor.PutString("bankNum", bankNum);
                Utils.sPeditor.Apply();
                bankDia.Dismiss();
            }
        }
        private void CallBank(object sender, DialogClickEventArgs e)
        {
            // Checks for permission and asks for it
            if (CheckSelfPermission("android.permission.CALL_PHONE") != Permission.Granted)
                RequestPermissions(new string[] { "android.permission.CALL_PHONE" }, 0);
            else
            {
                Intent callInt = new Intent();
                callInt.SetAction(Intent.ActionCall);
                Android.Net.Uri callUri = Android.Net.Uri.Parse("tel:" + bankNum);
                callInt.SetData(callUri);
                StartActivity(callInt);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();

            Utils.CurrentContext = this;
            adapter.NotifyDataSetChanged();
            Utils.RefreshWallet(walletTV);
            isInEdit = false;
            editTV.Visibility = ViewStates.Gone;
            if (actions != null)
                actions.SetVisible(Utils.TransList.Count != 0 || Utils.LogAccList.Count != 0 ? true : false);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            // requestCode == 0 is the phone permission request code
            if (requestCode == 0)
                if (grantResults[0] == Permission.Granted)
                    CallBank(null, null);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}