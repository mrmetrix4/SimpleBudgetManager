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
    [Table("LogTrans")]
    public class LogTrans : LogAction
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { set; get; }
        public int SecAccID { get; set; }

        public LogTrans() { }
        public LogTrans(int mainAccID, double value) : base(mainAccID, value)
        {
            SecAccID = -1;
        }        
        public LogTrans(int mainAccID, int secAccID, double value) : base(mainAccID, value)
        {
            SecAccID = secAccID;
        }
        public override void UndoAction()
        {
            Account accMain = Utils.FullAccList.Find(i => i.Id == MainAccID);
            Account accSec = SecAccID != -1 ? Utils.FullAccList.Find(i => i.Id == SecAccID) : null;
            if (!accMain.IsActive || (accSec != null && !accSec.IsActive))
                return;
            int mainPos = Utils.AccList.FindIndex(i => i.Id == accMain.Id);
            int secPos = accSec != null ? Utils.AccList.FindIndex(i => i.Id == accSec.Id) : -1;
            if (secPos == -1)
            {
                Account curr = Utils.AccList[mainPos];

                Utils.AccList[mainPos].Amount -= Value;

                Utils.sbmDB.Update(curr);
            }
            else
            {
                Account fromAcc = Utils.AccList[mainPos];
                Account toAcc = Utils.AccList[secPos];

                Utils.AccList[mainPos].Amount -= Value;
                Utils.AccList[secPos].Amount += Value;

                Utils.sbmDB.Update(fromAcc);
                Utils.sbmDB.Update(toAcc);
                Utils.sbmDB.Delete(this);
            }
            Utils.sbmDB.Delete(this);
            Utils.TransList.Remove(this);
        }

        public override string AlertToString()
        {
            Account accMain = Utils.FullAccList.Find(i => i.Id == MainAccID);
            string str = Math.Abs(Value) + " ₪" + " was";
            if (SecAccID != -1)
            {
                Account accSec = Utils.FullAccList.Find(i => i.Id == SecAccID);
                str += " transferd from " + accSec.Name + " to " + accMain.Name;
            }
            else if (Value < 0)
                str += " spent from " + accMain.Name;
            else
                str += " added to " + accMain.Name;
            str += " on " + Date.ToString("d");

            return str;
        }
    }
}