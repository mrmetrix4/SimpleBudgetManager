using System;

namespace SimpleBudgetManager
{
    public abstract class LogAction
    {
        public int MainAccID { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }

        public LogAction() { }
        public LogAction(int mainAccID, double value)
        {
            MainAccID = mainAccID;
            Value = value;
            Date = DateTime.Now;
        }
        
        public abstract void UndoAction();
        public abstract string AlertToString();
    }
}