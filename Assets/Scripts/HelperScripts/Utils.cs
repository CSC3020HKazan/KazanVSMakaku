using UnityEngine;
using System.Collections;

public class Utils {

	public static Vector3 PolarToCartesian (Vector3 polar) {
		// supposes polar is of the form (r, phi, theta)
		// Asserts that phi and theta are in radians
		return new Vector3 (polar.x * Mathf.Cos (polar.y) * Mathf.Sin (polar.z), 
		                    polar.x * Mathf.Cos (polar.z), 
		                    polar.x * Mathf.Sin (polar.y) * Mathf.Sin (polar.z));

	}

	public static Vector3 CartesianToPolar (Vector3 cartesian) {
		float r = Vector3.Magnitude (cartesian);
		float theta = Mathf.Atan2 (cartesian.z, cartesian.x);
		float phi = Mathf.Acos(cartesian.y/r);
		return new Vector3 (r, theta, phi);
	}

}
