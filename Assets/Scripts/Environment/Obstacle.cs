using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	void Start () {
		gameObject.tag = Tags.obstacle;
		gameObject.name = Tags.obstacle;
	}

	void FixedUpdate () {

	}

	void OnCollisionEnter (Collision coll) {
		if (coll.gameObject.tag == Tags.player 		|| 
			coll.gameObject.tag == Tags.playerOne 	|| 
			coll.gameObject.tag == Tags.playerTwo 	|| 
			coll.gameObject.tag == Tags.enemy 		|| 
			coll.gameObject.tag == Tags.elementBall ||
			coll.gameObject.tag == Tags.fireBall 	||
			coll.gameObject.tag == Tags.iceBall) 
		{
			
		}
		
	} 


}