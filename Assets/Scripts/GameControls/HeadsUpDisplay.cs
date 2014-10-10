using UnityEngine;
using System.Collections;

public enum MessageType {
	WARNING = 1,
	INFORMATION = 2,
	STORY_BOARD = 3
};

//script for display HUD
public class HeadsUpDisplay : MonoBehaviour {

	//length of health and mana bar
	public float healthLength = 400;
	public float manaLength = 400;

	//maximum length of health and mana bar
	public float hBarMax = 12;
	public float mBarMax = 12;

	public float clearWarningDelay = 5f;
	public float clearInformationDelay = 4f;
	public float clearStoryDelay = 4f;

	//health and mana scripts
	private PlayerHealth _playerHealth;
	private PlayerMana _playerMana;

	// player script
	//PlayerScript pScript;

	//guistyle for health and mana bars
	public GUIStyle healthStyle;
	public GUIStyle manaStyle;
	public GUIStyle storyStyle;
	public GUIStyle warningStyle;
	public GUIStyle informationStyle;
	public GUIStyle livesStyle;
	public GUIStyle experienceStyle;



	private string _warningMessage = "";
	private string _informationMessage = "";
	private string _storyMessage = "";

	//modifier to change health and mana bar length
	public int modifier = 12;



	// Use this for initialization
	void Start () {

		//pScript = GameObject.Find("player1").GetComponent<PlayerScript>();
		DisplayMessage ("WARNING", MessageType.WARNING) ;
		DisplayMessage( "Press A to Jump", MessageType.INFORMATION);
		DisplayMessage ("Kazan Meet Makaku", MessageType.STORY_BOARD);

	}
	
	// Update is called once per frame
	void Update () {
		if (_playerMana == null || _playerHealth == null)
			return; 
		//sets the maximum values for the bars
		hBarMax = _playerHealth.GetInitialHealth()*modifier;
		mBarMax = _playerMana.GetInitialMana()*modifier;

		//sets the maximum values for the bars
		hBarMax = _playerHealth.GetInitialHealth()*modifier;
		mBarMax = _playerMana.GetInitialMana()*modifier;
		//health bar and mana bar length update
		healthLength = _playerHealth.GetCurrentHealth() *modifier;
		manaLength = _playerMana.GetCurrentMana() *modifier;

	}
	
	void OnGUI ()
	{
		if (_playerMana == null || _playerHealth == null)
			return;
		// draws the labels for the bars
		GUI.Label(new Rect(70,10,60,60),"<size=40>x"+_playerHealth.GetLives()+"</size>", livesStyle);
		GUI.Label(new Rect(1000,10,60,60),"<size=40>"+_playerMana.GetExperience()+"XP</size>", experienceStyle);
		//health bar 
		GUI.Box(new Rect(150 ,10,healthLength,20), "  Health :" +(int) (_playerHealth.GetCurrentHealth() ), healthStyle);
		GUI.Box(new Rect(150 ,10,hBarMax,20),"");

		//mana bar
		GUI.Box(new Rect(150 ,40,manaLength,20), "  Mana :" +(int) (_playerMana.GetCurrentMana()) , manaStyle);
		GUI.Box(new Rect(150 ,40,mBarMax,20),"");

        GUI.Box (new Rect (Screen.width / 2 + 100,Screen.height / 2 + 50,100,50), _informationMessage,informationStyle);
        GUI.Box (new Rect (Screen.width / 2 - 100,Screen.height / 2 - 50,100,50), _warningMessage,warningStyle);
        GUI.Box (new Rect ((Screen.width / 2) - 100,(Screen.height) - 50,100,50), _storyMessage, storyStyle);
	}

	public void SetPlayerManaComponent (PlayerMana playerMana) { 
		_playerMana = playerMana; 
	}

	public void SetPlayerHealthComponent ( PlayerHealth playerHealth) {
		_playerHealth = playerHealth;

	}

	public void DisplayMessage( string message, MessageType mtype) {
		switch (mtype) {
			case MessageType.WARNING: 
				_warningMessage = message;
				Invoke ("ClearWarningMessage", clearWarningDelay );

				break;
			case MessageType.INFORMATION:
				_informationMessage = message;
				Invoke ("ClearInformationMessage", clearInformationDelay ); 

				break;
			case MessageType.STORY_BOARD:
				_storyMessage = message;
				Invoke ("ClearStoryMessage", clearStoryDelay );
				break;
		}
	}

	void ClearStoryMessage () {
		_storyMessage = "";
	}

	void ClearInformationMessage () {
		_informationMessage =  "";
	}

	void ClearWarningMessage () {
		_warningMessage = "";
	}
}
