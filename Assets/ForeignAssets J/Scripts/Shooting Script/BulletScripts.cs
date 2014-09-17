using UnityEngine;
using System.Collections;

public class BulletScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject,3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//on collision
	void OnCollisionEnter(Collision col)
	{
		//if collided with enemy
		if(col.gameObject.tag == "Enemy")
		{
			//decrease health and destroy bullet
			col.gameObject.GetComponent<HealthScript>().decreaseHealth(3);
			Destroy(gameObject);
		}
	}
}
