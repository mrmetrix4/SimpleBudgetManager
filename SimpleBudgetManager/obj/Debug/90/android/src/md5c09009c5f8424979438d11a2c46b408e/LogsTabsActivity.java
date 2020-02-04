package md5c09009c5f8424979438d11a2c46b408e;


public class LogsTabsActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("SimpleBudgetManager.LogsTabsActivity, SimpleBudgetManager", LogsTabsActivity.class, __md_methods);
	}


	public LogsTabsActivity ()
	{
		super ();
		if (getClass () == LogsTabsActivity.class)
			mono.android.TypeManager.Activate ("SimpleBudgetManager.LogsTabsActivity, SimpleBudgetManager", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
