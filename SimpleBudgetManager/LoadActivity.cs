using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace SimpleBudgetManager
{
    [Activity(Label = "SBM", Theme = "@style/AppTheme.Load", MainLauncher = true, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoadActivity : Activity
    {
        // Shows the user an image while the application sets itself (MainActivity and Utils)

        static readonly string TAG = "X:" + typeof(LoadActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }
        protected override void OnResume()
        {
            base.OnResume();

            Task tmpStartupWork = new Task(() =>
            {
                Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
                Thread.Sleep(1);
                Log.Debug(TAG, "Working in the background - important stuff.");
            });

            tmpStartupWork.ContinueWith(inT =>
            {
                Log.Debug(TAG, "Work is finished - start MainActivity.");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            tmpStartupWork.Start();
        }
    }
}