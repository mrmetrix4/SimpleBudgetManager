using System;

using Android.App;
using Android.Content;
using Android.Widget;
using Plugin.LocalNotifications;

namespace SimpleBudgetManager
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class MinChangeReceiver : BroadcastReceiver
    {
        public MinChangeReceiver()
        {

        }
        public override void OnReceive(Context context, Intent intent)
        {
            if (DateTime.Now.ToShortTimeString() == "19:00")
                CrossLocalNotifications.Current.Show("SBM is missing you", "Spent some money? don't forget to update me!");
        }
    }
}