using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SimpleBudgetManager
{
    public static class Utils
    {
        public static Context CurrentContext { get; set; } // Every activity sets 'Utils.CurrentContext = this' in order to create toasts and dialogs without declaring context
        public static List<Account> AccList { get; set; } // All the active users list
        public static List<Account> FullAccList { get; set; } // All the users list (whether they are active or not)
        public static List<LogTrans> TransList { get; set; } // Transactions list (last 30 days till 'show all' is clicked)
        public static List<LogAccount> LogAccList { get; set; } // AccountsLog list (last 30 days till 'show all' is clicked)

        public static SQLiteConnection sbmDB = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sbmDB"));

        public static ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
        public static ISharedPreferencesEditor sPeditor = prefs.Edit();
        public static ProgressDialog RefreshDialog { get; set; } // Progress dialog (public in order to change percentage from service)

        // Methods that creat visual content

        /// <summary>
        /// Shows a toast with <paramref name="body"/> based on current context
        /// </summary>
        /// <param name="body">the toast's text</param>
        public static void ShowToast(string body)
        {
            Toast.MakeText(CurrentContext, body, ToastLength.Short).Show();
        }

        /// <summary>
        /// Returns a dialog with <paramref name="layoutID"/> based on current context, Cancelable is true
        /// </summary>
        /// <param name="layoutID">The dialog's layout's ID</param>
        public static Dialog MakeDialog(int layoutID)
        {
            Dialog Dia = new Dialog(CurrentContext);
            Dia.SetContentView(layoutID);
            Dia.SetCanceledOnTouchOutside(true);
            return Dia;
        }

        /// <summary>
        /// Shows a dialog with <paramref name="layoutID"/> based on current context, Cancelable is true
        /// </summary>
        /// <param name="layoutID">The dialog's layout's ID</param>
        public static void ShowDialog(int layoutID)
        {
            Dialog Dia = MakeDialog(layoutID);
            Dia.Show();
            Dia.Dispose();
        }

        /// <summary>
        /// Returns an alert dialog based on current context, includes Cancel button
        /// </summary>
        /// <param name="title">Alert dialog's title</param>
        /// <param name="body">Alert dialog's text</param>
        /// <param name="posName">Alert dialog's positive button's text</param>
        /// <param name="posMeth">Alert dialog's positive button's action</param>
        public static Dialog MakeAlertDialog(string title, string body, string posName, EventHandler<DialogClickEventArgs> posMeth)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(CurrentContext);

            alertDialog.SetTitle(title);
            alertDialog.SetMessage(body);
            alertDialog.SetPositiveButton(posName, posMeth);
            alertDialog.SetNegativeButton("Cancel", (a, b) => { alertDialog.Dispose(); });

            Dialog showAlert = alertDialog.Create();
            alertDialog.Dispose();

            return showAlert;
        }
        /// <summary>
        /// Returns an alert dialog based on current context, includes Cancel button
        /// </summary>
        /// <param name="title">Alert dialog's title</param>
        /// <param name="body">Alert dialog's text</param>
        /// <param name="posName">Alert dialog's positive button's text</param>
        /// <param name="posMeth">Alert dialog's positive button's action</param>
        /// <param name="negName">Alert dialog's negative button's text</param>
        /// <param name="negMeth">Alert dialog's negative button's action</param>
        public static Dialog MakeAlertDialog(string title, string body, string posName, EventHandler<DialogClickEventArgs> posMeth, string negName, EventHandler<DialogClickEventArgs> negMeth)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(CurrentContext);

            alertDialog.SetTitle(title);
            alertDialog.SetMessage(body);
            alertDialog.SetPositiveButton(posName, posMeth);
            alertDialog.SetNegativeButton("Cancel", (a, b) => { alertDialog.Dispose(); });
            alertDialog.SetNeutralButton(negName, negMeth);

            Dialog showAlert = alertDialog.Create();
            alertDialog.Dispose();

            return showAlert;
        }

        /// <summary>
        /// Creates and shows the refresh dialog of 'Show all' (Transactions/accounts log)
        /// </summary>
        public static void ShowRefreshDialog()
        {
            RefreshDialog = new ProgressDialog(CurrentContext)
            {
                Indeterminate = false
            };
            RefreshDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            RefreshDialog.SetMessage("Getting all actions is in progress...");
            RefreshDialog.SetCancelable(false);
            RefreshDialog.Show();
        }
        // Methods that setup things

        /// <summary>
        /// Sets the TransList from the database
        /// </summary>
        /// <param name="all">Whether or not all the transactions should be inserted or only last 30 days transactions</param>
        public static void RefreshTrans(bool all)
        {
            DateTime minMon = DateTime.Now.AddMonths(-1);
            string strSQL = string.Format("SELECT * FROM LogTrans");
            List<LogTrans> trans = sbmDB.Query<LogTrans>(strSQL);
            TransList = new List<LogTrans>();
            foreach (LogTrans lT in trans)
                if ((!all && lT.Date.CompareTo(minMon) >= 0) || all)
                    TransList.Insert(0, lT);
        }
        /// <summary>
        /// Sets the LogAccList from the database
        /// </summary>
        /// <param name="all">Whether or not all the accounts actions should be inserted or only last 30 days accounts actions</param>
        public static void RefreshAccountsLog(bool all)
        {
            DateTime minMon = DateTime.Now.AddMonths(-1);
            string strSQL = string.Format("SELECT * FROM LogAccounts");
            List<LogAccount> accLogs = sbmDB.Query<LogAccount>(strSQL);
            LogAccList = new List<LogAccount>();
            foreach (LogAccount lA in accLogs)
                if ((!all && lA.Date.CompareTo(minMon) >= 0) || all)
                    LogAccList.Insert(0, lA);
        }
        /// <summary>
        /// Sets the AccList from the database
        /// </summary>
        public static void RefreshAccounts()
        {
            string strSQL = string.Format("SELECT * FROM Accounts");
            List<Account> accounts = sbmDB.Query<Account>(strSQL);
            AccList = new List<Account>();
            FullAccList = new List<Account>();
            if (accounts.Count == 0)
            {
                sbmDB.Insert(new Account("Demo1", 1024.48, true));
                sbmDB.Insert(new Account("Demo2", -10.3, false));
                sbmDB.Insert(new Account("Demo3", -205, true));
                accounts = sbmDB.Query<Account>(strSQL);
            }
            foreach (Account acc in accounts)
            {
                if (acc.IsActive)
                    AccList.Add(acc);
                FullAccList.Add(acc);
            }
        }

        /// <summary>
        /// Refreshes the <paramref name="TV"/> based on the active accounts
        /// </summary>
        /// <param name="TV">The wallet TextView</param>
        public static void RefreshWallet(TextView TV)
        {
            double wallet = 0;
            foreach (Account acc in AccList)
                wallet += acc.IsWallet ? acc.Amount : 0;
            TV.Text = string.Format("{0:n}", wallet) + " ₪";
        }

        /// <summary>
        /// Adds a transactions
        /// </summary>
        /// <param name="mainAccID">The transaction's account ID</param>
        /// <param name="value">The transaction's amount</param>
        public static void AddTrans(int mainAccID, double value)
        {
            LogTrans trans = new LogTrans(mainAccID, value);
            sbmDB.Insert(trans);
            TransList.Insert(0, trans);
        }
        /// <summary>
        /// Adds a transactions
        /// </summary>
        /// <param name="mainAccID">The transaction's main account ID</param>
        /// <param name="secAccID">The transaction's second account ID</param>
        /// <param name="value">The transaction's amount</param>
        public static void AddTrans(int mainAccID, int secAccID, double value)
        {
            LogTrans trans = new LogTrans(mainAccID, secAccID, value);
            sbmDB.Insert(trans);
            TransList.Insert(0, trans);
        }

        /// <summary>
        /// Adds an accounts log item with type set to 0 (adding an account)
        /// </summary>
        /// <param name="mainAccID">The accounts log item's account ID</param>
        /// <param name="value">The accounts log item's amount</param>
        public static void AddAccountLog(int mainAccID, double value)
        {
            LogAccount logA = new LogAccount(mainAccID, value);
            sbmDB.Insert(logA);
            LogAccList.Insert(0, logA);
        }
        /// <summary>
        /// Adds an accounts log item
        /// </summary>
        /// <param name="mainAccID">The accounts log item's account ID</param>
        /// <param name="value">The accounts log item's amount</param>
        /// <param name="type">The accounts log item's type: 0 for adding, 1 for deleting, 2 for name changing, 3 for isWallet changing, 5 for both</param>
        public static void AddAccountLog(int mainAccID, double value, int type)
        {
            LogAccount logA = new LogAccount(mainAccID, value, type);
            sbmDB.Insert(logA);
            LogAccList.Insert(0, logA);
        }

    }
}