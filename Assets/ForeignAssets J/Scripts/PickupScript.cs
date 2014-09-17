using UnityEngine;
using System.Collections;

//script for pick-up objects
public class PickupScript : MonoBehaviour {

	//integer used to distinguish between different pickup
	public int type = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//when triggered
	void OnTriggerEnter(Collider col)
	{
		//if pickup object collides with player
		if(col.gameObject.tag == "Player")
		{
			//for health pickup
			if(type ==1)
			{
				//play sound
				SoundScript.instance.playHealthSound(transform.position);

				//display text
				TextHelper.Instance.instantiateText(col.transform.position,"+ 30 health");

				//increase health
				col.GetComponent<HealthScript>().increaseHealth(30);
			}
			//for mana pickup
			else if(type == 2)
			{
				//play sound
				SoundScript.instance.playManaSound(transform.position);

				//display text
				TextHelper.Instance.instantiateText(col.transform.position,"+ 50 mana");

				//increase mana
				col.GetComponent<ManaScript>().increaseMana(50);
			}

			//pickup object is consumed and destroed
			Destroy(gameObject);
		}
	}
}
