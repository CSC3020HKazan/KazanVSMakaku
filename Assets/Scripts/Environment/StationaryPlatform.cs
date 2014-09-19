using UnityEngine;
using System.Collections;

public class StationaryPlatform : BasePlatform {
	protected override void InitialiseTag () {
		gameObject.tag = Tags.stationaryPlatform;
		gameObject.name = Tags.stationaryPlatform;
	}
}	