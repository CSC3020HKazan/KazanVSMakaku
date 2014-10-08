using UnityEngine;
using System.Collections;

public class Player1Movement : PlayerMovement {

	protected override void InitialiseTag () {
		gameObject.tag = Tags.playerOne;
		gameObject.name = Tags.playerOne;
	}
	
	public override string GetAttachedCameraTag () {
		return Tags.mainCameraPlayer1;
	}

	public override float GetHorizontalAxisRaw () {
		return  Input.GetAxisRaw (Tags.UserInputs.horizontalPlayer1);
	}
	public override float GetVerticalAxisRaw () {
		return  Input.GetAxisRaw (Tags.UserInputs.verticalPlayer1);
	}
	public override bool GetSprintInput () {
		return Input.GetButton (Tags.UserInputs.sprintPlayer1);
	}	
	public override bool GetJumpInput () {
		return  Input.GetButton (Tags.UserInputs.jumpPlayer1);
	}
}