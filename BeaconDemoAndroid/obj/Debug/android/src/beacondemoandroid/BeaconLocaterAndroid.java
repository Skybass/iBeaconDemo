package beacondemoandroid;


public class BeaconLocaterAndroid
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.radiusnetworks.ibeacon.IBeaconConsumer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_getApplicationContext:()Landroid/content/Context;:GetGetApplicationContextHandler:RadiusNetworks.IBeaconAndroid.IBeaconConsumerInvoker, Android-iBeacon-Service\n" +
			"n_bindService:(Landroid/content/Intent;Landroid/content/ServiceConnection;I)Z:GetBindService_Landroid_content_Intent_Landroid_content_ServiceConnection_IHandler:RadiusNetworks.IBeaconAndroid.IBeaconConsumerInvoker, Android-iBeacon-Service\n" +
			"n_onIBeaconServiceConnect:()V:GetOnIBeaconServiceConnectHandler:RadiusNetworks.IBeaconAndroid.IBeaconConsumerInvoker, Android-iBeacon-Service\n" +
			"n_unbindService:(Landroid/content/ServiceConnection;)V:GetUnbindService_Landroid_content_ServiceConnection_Handler:RadiusNetworks.IBeaconAndroid.IBeaconConsumerInvoker, Android-iBeacon-Service\n" +
			"";
		mono.android.Runtime.register ("BeaconDemoAndroid.BeaconLocaterAndroid, BeaconDemo.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BeaconLocaterAndroid.class, __md_methods);
	}


	public BeaconLocaterAndroid () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BeaconLocaterAndroid.class)
			mono.android.TypeManager.Activate ("BeaconDemoAndroid.BeaconLocaterAndroid, BeaconDemo.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.content.Context getApplicationContext ()
	{
		return n_getApplicationContext ();
	}

	private native android.content.Context n_getApplicationContext ();


	public boolean bindService (android.content.Intent p0, android.content.ServiceConnection p1, int p2)
	{
		return n_bindService (p0, p1, p2);
	}

	private native boolean n_bindService (android.content.Intent p0, android.content.ServiceConnection p1, int p2);


	public void onIBeaconServiceConnect ()
	{
		n_onIBeaconServiceConnect ();
	}

	private native void n_onIBeaconServiceConnect ();


	public void unbindService (android.content.ServiceConnection p0)
	{
		n_unbindService (p0);
	}

	private native void n_unbindService (android.content.ServiceConnection p0);

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
