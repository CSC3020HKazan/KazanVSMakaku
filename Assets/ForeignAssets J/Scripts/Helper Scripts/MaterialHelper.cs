using UnityEngine;
using System.Collections;

//assign random material to objects
public class MaterialHelper : MonoBehaviour {

	//instance
	public static MaterialHelper Instance;

	//array of materials for ground and objects
	public Material[] groundMats;
	public Material[] objectMats;
	
	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of MaterialHelper!");
		}
		Instance = this;
	}

	//return randomly selected ground material
	public Material getGroundMaterial()
	{
		return groundMats[Random.Range(0,groundMats.Length)];
	}

	//return randomly selected object material
	public Material getObjectMaterial()
	{
		return objectMats[Random.Range(0,objectMats.Length)];
	}
}
