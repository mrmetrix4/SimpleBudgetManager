using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SimpleBudgetManager
{
    [Activity(Label = "AccountEditActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AccountEditActivity : Activity, View.IOnClickListener
    {
        Button saveBTN, delBTN;
        EditText accName;
        CheckBox cbWallet;

        int pos;

        public void OnClick(View v)
        {
            if (v == delBTN)
            {
                Dialog showDelAlert = Utils.MakeAlertDialog("Are you sure?", "Deleting an account is irreversible, though you get to keep the money", "Delete", AccountDelete);
                showDelAlert.Show();
            }
            else if (v == saveBTN)
            {
                Account curr = Utils.AccList[pos];
                if (accName.Text.Length < 2 && accName.Text != "")
                    Utils.ShowToast("The name must contain at least 2 letters!");
                else
                {
                    int changeType = 0; // Indicates the LogAccount type
                    if (Utils.AccList[pos].IsWallet != cbWallet.Checked)
                    {
                        Utils.AccList[pos].IsWallet = cbWallet.Checked;
                        changeType += 3;
                    }
                    if (accName.Text != "" && accName.Text != Utils.AccList[pos].Name)
                    {
                        Utils.AccList[pos].Name = accName.Text;
                        changeType += 2;
                    }
                    Utils.AddAccountLog(Utils.AccList[pos].Id, changeType);
                    Utils.sbmDB.Update(curr);
                    Finish();
                }
            }

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.activity_account_edit);

            // Initializing
            Utils.CurrentContext = this;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));

            saveBTN = FindViewById<Button>(Resource.Id.uptBTN);
            delBTN = FindViewById<Button>(Resource.Id.delBTN);
            accName = FindViewById<EditText>(Resource.Id.accName);
            cbWallet = FindViewById<CheckBox>(Resource.Id.cbWallet);

            pos = Intent.GetIntExtra("pos", -1);

            cbWallet.Checked = Utils.AccList[pos].IsWallet;
            accName.Hint = Utils.AccList[pos].Name;
            ActionBar.Title = "Edit " + Utils.AccList[pos].Name;
            delBTN.Visibility = Utils.AccList.Count == 1 ? ViewStates.Gone : ViewStates.Visible;
            
            // Actions
            saveBTN.SetOnClickListener(this);
            delBTN.SetOnClickListener(this);
        }

        private void AccountDelete(object sender, DialogClickEventArgs e)
        {
            Account curr = Utils.AccList[pos];
            Utils.AccList[pos].IsActive = false;
            Utils.AccList.RemoveAt(pos);
            Utils.sbmDB.Update(curr);
            Utils.AddAccountLog(curr.Id, curr.Amount ,1);
            Finish();
        }
    }
}