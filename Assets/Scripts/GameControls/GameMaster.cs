using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// THIS CLASS MANAGES EACH AND EVERY DETAIL IN THE GAME
public class GameMaster : MonoBehaviour {

	public static System.String pauseMessage = "[ESC to pause]";
	public static string resumeMessage = "[ESC to resume game]";
	public float hSliderValue = ThirdPersonCamera.height;
	public float dSliderValue = ThirdPersonCamera.distance;
	public float sSliderValue = ThirdPersonCamera.orbitalAngleOffset;
	public Vector3 aSliderValue = ThirdPersonCamera.orbitalAxis;

	private static TextMesh messageBoard;
	private static TextMesh scoreBoard;
	private static int hits = 0;
	private static int misses= 0 ;
	private static int score = 0;
	private static int previousScore = 0;
	private static bool gamePaused = false, wasPaused;
	private bool pauseAllObjects = false;
	// Saves Previous message
	private static string lastMessageNotified;
	private string[] modeStrings = new string[] {"OrbitalController", "ThirdPersonController"};
	private string[] axisOptions = new string[] {"Vector3.up", "Vector3.forward", "Vector3.right"};
	private Vector3[] orbitalAxisOptions = new Vector3[] {Vector3.up,Vector3.forward, Vector3.right};



	public enum InGameCameraMode {
		FirstPersonController = 0,
		OrbitalController = 1,
		ThirdPersonController = 2
	};
	
	private static InGameCameraMode cameraMode = InGameCameraMode.ThirdPersonController; 

	// Use this for initialization
	void Start () {

	}


	void OnGUI() {
		if (gamePaused) {
			if (cameraMode == InGameCameraMode.ThirdPersonController) {
				GUILayout.Label("Set Camera Height");
				hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0.5F, 10.0F);
				GUILayout.Label("Set Camera Distance");
				dSliderValue = GUILayout.HorizontalSlider( dSliderValue, 1.0F, 10.0F);
				ThirdPersonCamera.AdjustCameraHeight (hSliderValue);
				ThirdPersonCamera.AdjustCameraDistance (dSliderValue);
			}
			if (cameraMode == InGameCameraMode.OrbitalController) {
				GUILayout.Label("Set Camera Speed");
				sSliderValue = GUILayout.HorizontalSlider( sSliderValue, 2.0F, 20.0F);
				GUILayout.Label("Set Camera Axis");
				aSliderValue  = orbitalAxisOptions[GUILayout.SelectionGrid (GetAxisIndex(), axisOptions, 1)];
				ThirdPersonCamera.AdjustOrbitalParameters (aSliderValue, sSliderValue);
			}
			GUILayout.Label ("Set the InGame Camera Mode");
			cameraMode = (InGameCameraMode)(GUILayout.SelectionGrid ((int)(cameraMode), modeStrings, 1));
			GUILayout.Label (resumeMessage);
		} else {
			GUILayout.Label (pauseMessage);
		}
	
	}

	void OnResumeGame () {
		if (cameraMode == InGameCameraMode.OrbitalController) {
			cameraMode = InGameCameraMode.ThirdPersonController;
			NotifyPlayer ("Switching to Third Person View");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			wasPaused = gamePaused;
			gamePaused = !gamePaused;
		}
		if (gamePaused) {
			ShowMenu();
		} else {
			ResumeGame();
		}
		if (wasPaused)
			NotifyPlayer (lastMessageNotified);
	}

	public static void NotifyPlayer (System.String message) {
		GameObject messageBoardgo = GameObject.FindGameObjectWithTag ("Message");
		if (!messageBoardgo)
			Debug.Log ("No message board");
		else
			messageBoard = messageBoardgo.GetComponent<TextMesh>();


        if (!messageBoard)
            Debug.Log("Message Board attached to the Main Camera cannot be found");
        else {
			lastMessageNotified = messageBoard.text;
			messageBoard.text = message;
		}
	}

	public static void NotifyPlayer (int score) {
		GameObject scoreBoardgo = GameObject.FindGameObjectWithTag ("ScoreBoard");
		if (!scoreBoardgo)
			Debug.Log ("NO scoreboard");
		else 
			scoreBoard = scoreBoardgo.GetComponent<TextMesh>();
		previousScore = score; 
		scoreBoard.text = "Score :: " + score;

	}

	public static bool IsPaused () {
		return gamePaused;
	}

	void ShowMenu() {
		Time.timeScale = 0;
		if (!pauseAllObjects) {
			Object[] objects = FindObjectsOfType (typeof(GameObject));
			foreach (GameObject go in objects) {
				go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
			}
			pauseAllObjects = true;
		}
	}

	public static bool DisplayScore () {
		score = hits - misses;
		if (score != previousScore) 
			NotifyPlayer (score);
		if (score == -200) 
			return false;
		return true;
	}

	void ResumeGame () {
		Time.timeScale = 1;
		NotifyPlayer ("");
		if (pauseAllObjects) {
			Object[] objects = FindObjectsOfType (typeof(GameObject));
			foreach (GameObject go in objects) {
				go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
			}
			pauseAllObjects = false;
		}
	}

	public static InGameCameraMode GetCameraMode () {
		return cameraMode;
	}

	private int GetAxisIndex () {
		for (int i = 0; i < orbitalAxisOptions.Length; i++) {
			if (ThirdPersonCamera.orbitalAxis == orbitalAxisOptions [i])
				return i;
		} return 0;
	}

	public static IEnumerator Wait (float seconds) {
		yield return new WaitForSeconds (seconds);
	}

	public static void RecordHit () {
		hits++;
		NotifyPlayer ("Recording a Hit");
	}

	public static void RecordMiss () {
		misses++;
		NotifyPlayer ("Recording a Miss");
	}
}
