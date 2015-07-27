/*
 * ClampsBGone by Marc "Technicalfool" Gale.
 * A last-resort mod for "fixing" the flying launch clamps issue,
 * by obliterating all non-necessary launch clamps.
 */

using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace ClampsBGone
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class ClampsBGone : MonoBehaviour
	{
		/*
		 * A list to store all clamp-only vessels in
		 * on each check.
		 */
		List<Vessel> dList;
		/*
		 * A timer for running checks with.
		 */
		System.Timers.Timer checkTimer;
		public void Start()
		{
			dList = new List<Vessel>(); //Initialize dList as a new list.
			/*
			 * Initialize a new timer that fires once per second.
			 */
			checkTimer = new System.Timers.Timer(1000);
			/*
			 * Add the check method to the timer.
			 */
			checkTimer.Elapsed += runCheck;
			/*
			 * Start the timer!
			 */
			checkTimer.Enabled = true;
			Debug.Log("[ClampsBGone] Started!");
		}

		/*
		 * Run this periodically.
		 */
		private void runCheck(object source, System.Timers.ElapsedEventArgs e)
		{
			/*
			 * Get all vessels in the universe.
			 */
			List<Vessel> vList = FlightGlobals.Vessels;
			/*
			 * Check through all vessels.
			 */
			foreach(Vessel v in vList)
			{
				//Only pass if vessel has a part count of 1.
				if (v.parts.Count == 1)
				{
					//Only pass if the vessel part is a launch clamp.
					if (v.parts[0].Modules.Contains("LaunchClamp"))
					{
						//Add a vessel reference to dList.
						dList.Add(v);
					}
				}
			}
			/*
			 * Some debug logging here to see if this mod really is
			 * catching and killing unidentified flying launch clamps.
			 */
			if (dList.Count > 0)
			{
				Debug.Log("[ClampsBGone] Found " + dList.Count + " launch clamps.");
			}
			/*
			 * Go through all vessel references in the dList.
			 */
			foreach(Vessel v in dList)
			{
				v.Die(); //TERMINATE WITH EXTREME PREJUDICE.
			}
			/*
			 * Now clear the dList, because memory leaks and NREs are bad.
			 */
			dList.Clear();
		}
	}
}

