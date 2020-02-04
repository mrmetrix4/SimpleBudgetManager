package crc64342baeca447c435d;


public class SlidingTabStrip
	extends android.widget.LinearLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDrawForeground:(Landroid/graphics/Canvas;)V:GetOnDrawForeground_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("SimpleBudgetManager.SlidingTabStrip, SimpleBudgetManager", SlidingTabStrip.class, __md_methods);
	}


	public SlidingTabStrip (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SlidingTabStrip.class)
			mono.android.TypeManager.Activate ("SimpleBudgetManager.SlidingTabStrip, SimpleBudgetManager", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SlidingTabStrip (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SlidingTabStrip.class)
			mono.android.TypeManager.Activate ("SimpleBudgetManager.SlidingTabStrip, SimpleBudgetManager", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SlidingTabStrip (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SlidingTabStrip.class)
			mono.android.TypeManager.Activate ("SimpleBudgetManager.SlidingTabStrip, SimpleBudgetManager", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SlidingTabStrip (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == SlidingTabStrip.class)
			mono.android.TypeManager.Activate ("SimpleBudgetManager.SlidingTabStrip, SimpleBudgetManager", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDrawForeground (android.graphics.Canvas p0)
	{
		n_onDrawForeground (p0);
	}

	private native void n_onDrawForeground (android.graphics.Canvas p0);

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
