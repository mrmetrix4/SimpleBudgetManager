using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SimpleBudgetManager
{
    [Table("LogAccounts")]
    public class LogAccount : LogAction
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { set; get; }
        public int Type { set; get; }
        // 0 for adding, 1 for deleting, 2 for changing name, 3 for changing isWallet, 5 (2 + 3) for both

        public LogAccount() { }
        public LogAccount(int mainAccID, double value) : base(mainAccID, value)
        {
            Type = 0;
        }
        public LogAccount(int mainAccID, double value, int type) : base(mainAccID, value)
        {
            Type = type;
        }
        public override void UndoAction()
        {
            Account accMain = Utils.FullAccList.Find(i => i.Id == MainAccID);
            if (Type == 1)
            {
                accMain.IsActive = true;
                Utils.AccList.Add(accMain);
            }
            else if (Type == 0)
            {
                accMain.IsActive = false;
                Utils.AccList.Remove(accMain);
            }
            else if (Type == 2)
                return;
            else if (Type == 3)
                accMain.IsWallet = !accMain.IsWallet;
            else if (Type == 5)
                accMain.IsWallet = !accMain.IsWallet;

            // In any case (except 2)
            Utils.sbmDB.Delete(this);
            Utils.LogAccList.Remove(this);
            Utils.sbmDB.Update(accMain);
        }

        public override string AlertToString()
        {
            Account accMain = Utils.FullAccList.Find(i => i.Id == MainAccID);

            string str = accMain.Name;
            switch (Type)
            {
                case 0:
                    str += " was added";
                    break;
                case 1:
                    str += " was deleted";
                    break;
                case 2:
                    str += " name was changed (Restoring name is not possible)";
                    break;
                case 3:
                    str += " is-in-wallet was changed";
                    break;
                case 5:
                    str += " is-in-wallet and name were changed (Undo will restore is-in-wallet only)";
                    break;
            }
            str += " on " + Date.ToString("d");

            return str;
        }
    }
}