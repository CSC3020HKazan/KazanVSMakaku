using UnityEngine;
using System.Collections;

//script for display HUD
public class HUDScript : MonoBehaviour {

	//length of health and mana bar
	float healthLength = 400;
	float manaLength = 400;

	//maximum length of health and mana bar
	float hBarMax;
	float mBarMax;

	//health and mana scripts
	HealthScript hScript;
	ManaScript mScript;

	// player script
	//PlayerScript pScript;

	//guistyle for health and mana bars
	public GUIStyle healthStyle;
	public GUIStyle manaStyle;

	//modifier to change health and mana bar length
	public int modifier;

	// Use this for initialization
	void Start () {

		//finds the scripts
		hScript = GameObject.Find("Player").GetComponent<HealthScript>();
		mScript = GameObject.Find("Player").GetComponent<ManaScript>();
		//pScript = GameObject.Find("player1").GetComponent<PlayerScript>();

		//sets the maximum values for the bars
		hBarMax = hScript.maxHealth*modifier;
		mBarMax = mScript.maxMana*modifier;
	}
	
	// Update is called once per frame
	void Update () {

		//health bar and mana bar length update
		healthLength = hScript.getHealth() *modifier;
		manaLength = mScript.getMana() *modifier;

	}
	
	void OnGUI ()
	{
		// draws the labels for the bars
		GUI.Label(new Rect(70,10,60,60),"<size=40>x"+5+"</size>");
		GUI.Label(new Rect(1000,10,60,60),"<size=40>x"+5+"</size>");

		//health bar 
		GUI.Box(new Rect(150 ,10,healthLength,20), "  Health :" +hScript.getHealth() , healthStyle);
		GUI.Box(new Rect(150 ,10,hBarMax,20),"");

		//mana bar
		GUI.Box(new Rect(150 ,40,manaLength,20), "  Mana :" +mScript.getMana() , manaStyle);
		GUI.Box(new Rect(150 ,40,mBarMax,20),"");

		//minimap
		GUI.Box(new Rect(1090 ,10,275,30),"<size=20>MiniMap</size>");
	}
}
