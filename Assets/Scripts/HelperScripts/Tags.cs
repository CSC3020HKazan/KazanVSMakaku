using UnityEngine;
using System.Collections;

public class Tags 
{
	// A list of tag strings
	public const string enemy = "Enemy";
	public const string environment = "Environment";
	public const string fader = "SceneFader";
	public const string finish = "Finish";
	public const string gameController = "GameController";
	public const string menuItem = "MenuItem";
	public const string player = "Player";
	public const string playerOne = "PlayerOne";
	public const string playerTwo = "PlayerTwo";
	public const string elementBall = "ElementBall";
	public const string fireBall = "FireBall";
	public const string iceBall = "IceBall";
	public const string fireballSpawnPoint = "FireBallSpawnPoint"; 
	public const string platform = "Platform";
	public const string stationaryPlatform = "StationaryPlatform";
	public const string movingPlatform = "MovingPlatform";
	public const string mainCamera = "MainCamera";
	public const string mainCameraPlayer1 = "MainCamera_Player1";
	public const string mainCameraPlayer2 = "MainCamera_Player2";



	public class UserInputs {
		// Default 
		public const string horizontal = "Horizontal";
		public const string vertical = "Vertical";
		public const string sprint = "Sprint";
		public const string jump = "Jump";
		public const string attack = "Attack";
		// Player 1
		public const string horizontalPlayer1 = "Horizontal_Player1";
		public const string verticalPlayer1 = "Vertical_Player1";
		public const string sprintPlayer1 = "Sprint_Player1";
		public const string jumpPlayer1 = "Jump_Player1";
		public const string attackPlayer1 = "Attack_Player1";
		// Player 2
		public const string horizontalPlayer2 = "Horizontal_Player2";
		public const string verticalPlayer2 = "Vertical_Player2";
		public const string sprintPlayer2 = "Sprint_Player2";
		public const string jumpPlayer2 = "Jump_Player2";
		public const string attackPlayer2 = "Attack_Player2";
	}

	public class CameraInputs {
		// Default 
		public const string cameraHorizontal = "CameraHorizontal";
		public const string cameraVertical = "CameraVertical";
		public const string cameraSnap = "CameraSnap";
		public const string leftTrigger = "LeftTrigger";
		// Player 1
		public const string cameraHorizontalPlayer1 = "CameraHorizontal_Player1";
		public const string cameraVerticalPlayer1 = "CameraVertical_Player1";
		public const string cameraSnapPlayer1 = "CameraSnap_Player1";
		public const string leftTriggerPlayer1 = "LeftTrigger_Player1";
		// Player 2
		public const string cameraHorizontalPlayer2 = "CameraHorizontal_Player2";
		public const string cameraVerticalPlayer2 = "CameraVertical_Player2";
		public const string cameraSnapPlayer2 = "CameraSnap_Player2";
		public const string leftTriggerPlayer2 = "LeftTrigger_Player2";
	}

	public class WeaponFiringPoint {
		public const string playerElementball = "PlayerElementBallSpawnPoint";

	}
}
