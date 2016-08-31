using System;
using System.Collections.Generic;
using Xamarin.Forms;
using BeaconDemoAndroid;
using RadiusNetworks.IBeaconAndroid;
using BeaconDemo;
using Android.Content;

[assembly: Dependency(typeof(BeaconLocaterAndroid))]

namespace BeaconDemoAndroid
{
	public class BeaconLocaterAndroid : Java.Lang.Object, IBeaconLocater, IBeaconConsumer
	{
		readonly string uuid = "B0C091FA-D303-11E3-94A7-1A514932AC01";
		readonly string beaconId = "iBeacon";

		IBeaconManager iBeaconManager;
		MonitorNotifier monitorNotifier;
		RangeNotifier rangeNotifier;
		Region monitoringRegion;
		Region rangingRegion;
		Context context;
		bool paused;
		List<BeaconItem> beacons;

		public BeaconLocaterAndroid()
		{
			beacons = new List<BeaconItem>();
			context = Forms.Context;

			iBeaconManager = IBeaconManager.GetInstanceForApplication(context);
			monitorNotifier = new MonitorNotifier();
			rangeNotifier = new RangeNotifier();

			monitoringRegion = new Region(beaconId, uuid, null, null);
			rangingRegion = new Region(beaconId, uuid, null, null);

			iBeaconManager.Bind(this);

			rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

		}

		public List<BeaconItem> GetAvailableBeacons()
		{
			return !paused ? beacons : null;
		}

		public void PauseTracking()
		{
			paused = true;
		}

		public void ResumeTracking()
		{
			paused = false;
		}

		void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			if (e.Beacons.Count > 0)
			{
				foreach (var b in e.Beacons)
				{
					if ((ProximityType)b.Proximity != ProximityType.Unknown)
					{

						var exists = false;
						for (int i = 0; i < beacons.Count; i++)
						{
							if (beacons[i].Minor.Equals(b.Minor.ToString()))
							{
								beacons[i].CurrentDistance = Math.Round(b.Accuracy, 2);
								SetProximity(b, beacons[i]);
								exists = true;
							}
						}

						if (!exists)
						{
							var newBeacon = new BeaconItem
							{
								Minor = b.Minor.ToString(),
								Name = "",
								CurrentDistance = Math.Round(b.Accuracy, 2)
							};
							SetProximity(b, newBeacon);
							beacons.Add(newBeacon);
						}
					}
				}
			}
		}

		void SetProximity(IBeacon source, BeaconItem dest)
		{

			Proximity p = Proximity.Unknown;

			switch ((ProximityType)source.Proximity)
			{
				case ProximityType.Immediate:
					p = Proximity.Immediate;
					break;
				case ProximityType.Near:
					p = Proximity.Near;
					break;
				case ProximityType.Far:
					p = Proximity.Far;
					break;
			}

			if (p > dest.Proximity || p < dest.Proximity)
			{
				dest.ProximityChangeTimestamp = DateTime.Now;
			}

			dest.Proximity = p;
		}

		public void OnIBeaconServiceConnect()
		{
			iBeaconManager.SetMonitorNotifier(monitorNotifier);
			iBeaconManager.SetRangeNotifier(rangeNotifier);

			iBeaconManager.StartMonitoringBeaconsInRegion(monitoringRegion);
			iBeaconManager.StartRangingBeaconsInRegion(rangingRegion);
		}

		public Context ApplicationContext
		{
			get { return this.context; }
		}

		public bool BindService(Intent intent, IServiceConnection connection, Bind bind)
		{
			return context.BindService(intent, connection, bind);
		}

		public void UnbindService(IServiceConnection connection)
		{
			context.UnbindService(connection);
		}
	}
}

