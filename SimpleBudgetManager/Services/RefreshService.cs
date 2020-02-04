using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    [Service]
    class RefreshService : Service
    {
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            ThreadStart threadStart = new ThreadStart(RefreshLists);
            Thread thread = new Thread(threadStart);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        private void RefreshLists()
        {
            Utils.RefreshAccountsLog(true);
            Utils.RefreshDialog.Progress = 25;
            Utils.RefreshTrans(true);
            Utils.RefreshDialog.Progress = 75;
            StopSelf();
        }
        public override void OnDestroy()
        {
            Utils.RefreshDialog.Progress = 95;
            ((Activity)Utils.CurrentContext).Recreate();
            Utils.RefreshDialog.Dismiss();
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}