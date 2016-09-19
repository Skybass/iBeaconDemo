using System;
using BeaconDemo;
using Xamarin.Forms;
using BeaconDemoiOS;
using CoreLocation;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Diagnostics;

[assembly: Dependency(typeof(BeaconLocateriOS))]

namespace BeaconDemoiOS
{
	public class BeaconLocateriOS : IBeaconLocater
	{
		CLLocationManager locationManager;
		readonly string roximityUuid = "E5075A5C-AF20-4496-9501-7F449091A4EC";
		readonly string roximityBeaconId = "iBeacon";
		CLBeaconRegion rBeaconRegion;
		List<BeaconItem> beacons;
		bool paused;

		public BeaconLocateriOS()
		{
			SetupBeaconRanging();
			locationManager.StartMonitoring(rBeaconRegion);
			locationManager.RequestState(rBeaconRegion);

			if (locationManager == null)
			{
				locationManager = new CLLocationManager();
				locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
				locationManager.HeadingFilter = 1;
				locationManager.UpdatedHeading += LocationManager_UpdatedHeading; ;
			}
			else {
				locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
				locationManager.HeadingFilter = 1;
				locationManager.UpdatedHeading += LocationManager_UpdatedHeading; ;
			}
			locationManager.StartUpdatingHeading();

		}

		void LocationManager_UpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
		{
			Debug.WriteLine(e.NewHeading.TrueHeading);
		}

		public void PauseTracking()
		{
			paused = true;
		}

		public void ResumeTracking()
		{
			paused = false;
		}

		public List<BeaconItem> GetAvailableBeacons()
		{
			return !paused ? beacons : null;
		}

		private void SetupBeaconRanging()
		{
			locationManager = new CLLocationManager();
			beacons = new List<BeaconItem>();
			locationManager.RequestAlwaysAuthorization();

			var rUuid = new NSUuid(roximityUuid);
			rBeaconRegion = new CLBeaconRegion(rUuid, roximityBeaconId);


			rBeaconRegion.NotifyEntryStateOnDisplay = true;
			rBeaconRegion.NotifyOnEntry = true;
			rBeaconRegion.NotifyOnExit = true;

			locationManager.RegionEntered += HandleRegionEntered;
			locationManager.RegionLeft += HandleRegionLeft;
			locationManager.DidDetermineState += HandleDidDetermineState;
			locationManager.DidRangeBeacons += HandleDidRangeBeacons;
		}

		void HandleRegionLeft(object sender, CLRegionEventArgs e)
		{
			if (e.Region.Identifier.Equals(roximityBeaconId))
			{
				locationManager.StopRangingBeacons(rBeaconRegion);
			}
		}

		void HandleRegionEntered(object sender, CLRegionEventArgs e)
		{
			Console.WriteLine("Region entered: " + e.Region.Identifier);
			if (e.Region.Identifier.Equals(roximityBeaconId))
			{
				locationManager.StartRangingBeacons(rBeaconRegion);
				var notification = new UILocalNotification { AlertBody = "Beacons are in range" };
				UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
			}
		}

		void HandleDidDetermineState(object sender, CLRegionStateDeterminedEventArgs e)
		{
			if (e.Region.Identifier.Equals(roximityBeaconId))
			{
				if (e.State == CLRegionState.Inside)
				{
					Console.WriteLine("Inside roximity beacon region [{0}]", e.Region.Identifier);
					locationManager.StartRangingBeacons(rBeaconRegion);

				}
				else if (e.State == CLRegionState.Outside)
				{
					Console.WriteLine("Outside roximity beacon region");
					locationManager.StopRangingBeacons(rBeaconRegion);
				}
			}
		}

		void HandleDidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
		{
			if (e.Beacons.Length > 0)
			{
				foreach (var b in e.Beacons)
				{

					if (b.Proximity != CLProximity.Unknown)
					{
						Console.WriteLine("UUID: {0} | Major: {1} | Minor: {2} | Accuracy: {3} | Proximity: {4} | RSSI: {5}", b.ProximityUuid, b.Major, b.Minor, b.Accuracy, b.Proximity, b.Rssi);
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

		void SetProximity(CLBeacon source, BeaconItem dest)
		{

			Proximity p = Proximity.Unknown;

			switch (source.Proximity)
			{
				case CLProximity.Immediate:
					p = Proximity.Immediate;
					break;
				case CLProximity.Near:
					p = Proximity.Near;
					break;
				case CLProximity.Far:
					p = Proximity.Far;
					break;
			}

			if (p > dest.Proximity || p < dest.Proximity)
			{
				dest.ProximityChangeTimestamp = DateTime.Now;
			}

			dest.Proximity = p;
		}
	}
}

