package beacondemoandroid;


public class RangeNotifier
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.radiusnetworks.ibeacon.RangeNotifier
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_didRangeBeaconsInRegion:(Ljava/util/Collection;Lcom/radiusnetworks/ibeacon/Region;)V:GetDidRangeBeaconsInRegion_Ljava_util_Collection_Lcom_radiusnetworks_ibeacon_Region_Handler:RadiusNetworks.IBeaconAndroid.IRangeNotifierInvoker, Android-iBeacon-Service\n" +
			"";
		mono.android.Runtime.register ("BeaconDemoAndroid.RangeNotifier, BeaconDemo.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RangeNotifier.class, __md_methods);
	}


	public RangeNotifier () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RangeNotifier.class)
			mono.android.TypeManager.Activate ("BeaconDemoAndroid.RangeNotifier, BeaconDemo.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void didRangeBeaconsInRegion (java.util.Collection p0, com.radiusnetworks.ibeacon.Region p1)
	{
		n_didRangeBeaconsInRegion (p0, p1);
	}

	private native void n_didRangeBeaconsInRegion (java.util.Collection p0, com.radiusnetworks.ibeacon.Region p1);

	java.util.ArrayList refList;
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
