using UnityEngine;
using System.Collections;

//master script
public class MasterScript : MonoBehaviour {

	//objects to be spawned
	public Transform capsuleObj;
	public Transform cubeObj;
	public Transform cylinderObj;
	public Transform sphereObj;

	//setup object array
	public int numberOfObjects = 2;
	private static int objCount = 0;
	private Transform[] objArray = new Transform[4];

	//spawn delay
	private float spawnDelay = 1;

	// Use this for initialization
	void Start () {

		//hide cursor
		Screen.lockCursor = true;

		//assign objects to array
		objArray[0] = capsuleObj;
		objArray[1] = cubeObj;
		objArray[2] = cylinderObj;
		objArray[3] = sphereObj;
	}
	
	// Update is called once per frame
	void Update () {

		//if theres no object in scene
		if(objCount == 0)
		{
			//and spawn timer expired
			if(spawnDelay <= 0)
			{
				//spawn object
				for(int i = 0; i < numberOfObjects; i++)
				{
					//random object with random position with random rotation
					var tempObj= Instantiate(objArray[Random.Range(0,4)],
									           new Vector3(Random.Range(-25.0f,25f), Random.Range(1f,10f), 
									           Random.Range(-25.0f,25f)),
					                           Random.rotation) as Transform;

					//assign random material to object
					tempObj.renderer.material = MaterialHelper.Instance.getObjectMaterial();
					objCount++;
				}

				//find ground object and assign random material
				GameObject.FindGameObjectWithTag("Ground").renderer.material = MaterialHelper.Instance.getGroundMaterial();


				spawnDelay = 3;
			}

			//decrease spawn timer
			if(spawnDelay > 0)
			{
				spawnDelay -= Time.deltaTime;
			}
		}
	}

	//decrease oject count
	public static void DecreaseObjCount()
	{
		objCount--;
	}

	//string for 3dtext
	public static string getObjCountText()
	{
		if(objCount == 1) return "1 object remaining";
		else if(objCount > 0) return objCount.ToString() +" objects remaining";
		else return "All objects destroyed";
	}
}
