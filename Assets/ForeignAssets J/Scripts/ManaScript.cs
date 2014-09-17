using UnityEngine;
using System.Collections;

public class ManaScript : MonoBehaviour {

	//Mana of object
	public int maxMana = 5;
	public int currentMana;
	// Use this for initialization
	void Start () {
		//currentMana = maxMana;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	//decreases Mana
	public void decreaseMana(int cost)
	{
		//if cost is greater than current mana
		if(cost > currentMana){}

		//else
		else currentMana -= cost;
		
	}

	//increase mana
	public void increaseMana(int heal)
	{
		//replenish mana
		currentMana += heal;

		//no more than max mana
		if(currentMana > maxMana)
		{
			currentMana = maxMana;
		}
	}

	//returns mana
	public int getMana(){return currentMana;}
}
