using UnityEngine;
using System.Collections;

public class Player2Movement : PlayerMovement {

	protected override void InitialiseTag () {
		gameObject.tag = Tags.playerTwo;
		gameObject.name = Tags.playerTwo;
	}

	public override string GetAttachedCameraTag () {
		return Tags.mainCameraPlayer2;
	}
	public override float GetHorizontalAxisRaw () {
		return  Input.GetAxisRaw (Tags.UserInputs.horizontalPlayer2);
	}
	public override float GetVerticalAxisRaw () {
		return  Input.GetAxisRaw (Tags.UserInputs.verticalPlayer2);
	}
	public override bool GetSprintInput () {
		return Input.GetButton (Tags.UserInputs.sprintPlayer2);
	}	
	public override bool GetJumpInput () {
		return  Input.GetButton (Tags.UserInputs.jumpPlayer2);
	}

}