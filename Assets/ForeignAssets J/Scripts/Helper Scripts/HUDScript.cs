using UnityEngine;
using System.Collections;

//script for display HUD
public class HUDScript : MonoBehaviour {

	//length of health and mana bar
	public float healthLength = 400;
	public float manaLength = 400;

	//maximum length of health and mana bar
	public float hBarMax = 12;
	public float mBarMax = 12;

	//health and mana scripts
	private PlayerHealth hScript;
	private PlayerMana mScript;

	// player script
	//PlayerScript pScript;

	//guistyle for health and mana bars
	public GUIStyle healthStyle;
	public GUIStyle manaStyle;

	//modifier to change health and mana bar length
	public int modifier = 12;

	// Use this for initialization
	void Start () {

		//finds the scripts
		hScript = GameObject.FindWithTag(Tags.player).GetComponent<PlayerHealth>();
		mScript = GameObject.FindWithTag(Tags.player).GetComponent<PlayerMana>();
		//pScript = GameObject.Find("player1").GetComponent<PlayerScript>();

		//sets the maximum values for the bars
		hBarMax = hScript.GetInitialHealth()*modifier;
		mBarMax = mScript.GetInitialMana()*modifier;
	}
	
	// Update is called once per frame
	void Update () {

		//health bar and mana bar length update
		healthLength = hScript.GetCurrentHealth() *modifier;
		manaLength = mScript.GetCurrentMana() *modifier;

	}
	
	void OnGUI ()
	{
		// draws the labels for the bars
		GUI.Label(new Rect(70,10,60,60),"<size=40>x"+5+"</size>");
		GUI.Label(new Rect(1000,10,60,60),"<size=40>x"+5+"</size>");

		//health bar 
		GUI.Box(new Rect(150 ,10,healthLength,20), "  Health :" +hScript.GetCurrentHealth() , healthStyle);
		GUI.Box(new Rect(150 ,10,hBarMax,20),"");

		//mana bar
		GUI.Box(new Rect(150 ,40,manaLength,20), "  Mana :" +mScript.GetCurrentMana() , manaStyle);
		GUI.Box(new Rect(150 ,40,mBarMax,20),"");

		//minimap
		GUI.Box(new Rect(1090 ,10,275,30),"<size=20>MiniMap</size>");
	}
}
