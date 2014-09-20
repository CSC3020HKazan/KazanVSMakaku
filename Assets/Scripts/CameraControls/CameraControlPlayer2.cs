using UnityEngine;
using System.Collections;

public class CameraControlPlayer2 : CameraControl {
	protected override void InitialiseTarget  () {
		target_ = GameObject.FindGameObjectWithTag (Tags.playerTwo);
	}

	protected override void InitialiseTag () {
		gameObject.tag = Tags.mainCameraPlayer2;
		gameObject.name = Tags.mainCameraPlayer2;
	}
	protected override float GetCameraHorizontalAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.cameraHorizontalPlayer2);
	}

	protected override float GetCameraVerticalAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.cameraVerticalPlayer2);
	}

	protected override bool GetCameraSnapInput () {
		return Input.GetButtonDown (Tags.CameraInputs.cameraSnapPlayer2);
	}

	protected override float GetLeftTriggerAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.leftTriggerPlayer2);
	}
}