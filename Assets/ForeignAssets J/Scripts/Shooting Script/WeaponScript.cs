using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	//array of weapons
	Transform[] weapons;

	//current weapon
	int currentWeapon = 0;

	int count;
	// Use this for initialization
	void Start () {

		//add weapons to array
		weapons = new Transform[transform.childCount];
		count = 0;
		foreach (Transform child in transform)
		{
			weapons[count] = child;
			if(child.gameObject.activeSelf) currentWeapon = count;
			count++;
		}
	}
	
	// Update is called once per frame
	void Update () {

		//change weapon to torch
		if(Input.GetKeyDown("1"))
		{
			changeWeapon(0);
		}
		//change weapon to gun
		else if(Input.GetKeyDown("2"))
		{
			changeWeapon(1);
		}
	}

	//change weapon method
	public void changeWeapon(int newWeapon)
	{
		if(currentWeapon != newWeapon)
		{
			//change weapon by disabling current weapon and enabling the other weapon
			weapons[currentWeapon].gameObject.SetActive(false);
			weapons[newWeapon].gameObject.SetActive(true);
			currentWeapon = newWeapon;
		}
	}
}
