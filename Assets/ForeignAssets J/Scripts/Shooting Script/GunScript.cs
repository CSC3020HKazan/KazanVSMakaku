using UnityEngine;
using System.Collections;

//gun script
public class GunScript : MonoBehaviour {

	//bullet transform
	public Transform bullet;

	//range and damage of weapon
	public float firerange = 20;
	public float damage = 2;

	//cooldown of gun
	public float cooldown = 2;
	private float cd;

	//spawning position of bullet
	Vector3 spawnPos;

	//target destinatioin for bullet
	Vector3 target;

	//player transform
	private Transform playerT;

	//cross hair texture
	public GUITexture crshair;

	// Use this for initialization
	void Start () {
		playerT = GameObject.Find("player1").transform;
	}
	
	// Update is called once per frame
	void Update () {

		//spawnposition is player position + up + forward/
		spawnPos = playerT.position + playerT.up + playerT.forward;

		//center of the camera
		Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,40));

		//shoot ray from spawn pos in direction of (center - spawnPos)
		Ray ray = new Ray(spawnPos,(center - spawnPos).normalized);


		RaycastHit hit;

		//shoot ray for 150 distance
		if (Physics.Raycast(ray, out hit, 150))
		{
			//target position is the point of collision
			target = hit.point;
		}

		//is no collision
		else target = ray.origin + ray.direction* 40;

		//change the crosshair position to the target
		crshair.transform.position = Camera.main.WorldToViewportPoint(target);


		if(cd <= 0)
		{
			//left mouse button
			if(Input.GetButton("Fire1"))
			{
				Fire ();
				cd = cooldown;
			}
		}
		//decrease cooldown
		else cd -= 1 * Time.deltaTime;
	}

	//fires bullet
	public void Fire()
	{
		//play sound
		SoundScript.instance.playGunSound(transform.position);

		//create bullet object
		Transform bul = Instantiate(bullet,spawnPos ,playerT.rotation) as Transform;

		//sets the direction of bullet object to be the direction of the ray
		bul.GetComponent<MoveScript>().setDirection((target - spawnPos).normalized);
	}
}
