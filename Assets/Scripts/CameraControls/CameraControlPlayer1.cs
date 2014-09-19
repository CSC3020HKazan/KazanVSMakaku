using UnityEngine;
using System.Collections;

public class CameraControlPlayer1 : CameraControl {

	protected override void InitialiseTarget  () {
		target_ = GameObject.FindGameObjectWithTag (Tags.playerOne);
	}
	protected override float GetCameraHorizontalAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.cameraHorizontalPlayer1);
	}

	protected override float GetCameraVerticalAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.cameraVerticalPlayer1);
	}

	protected override bool GetCameraSnapInput () {
		return Input.GetButtonDown (Tags.CameraInputs.cameraSnapPlayer1);
	}

	protected override float GetLeftTriggerAxisRaw () {
		return Input.GetAxisRaw (Tags.CameraInputs.leftTriggerPlayer1);
	}
}