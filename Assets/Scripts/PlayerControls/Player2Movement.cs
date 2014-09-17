using UnityEngine;
using System.Collections;

public class Player2Movement : PlayerMovement {

	void Start () {
		gameObject.tag = Tags.playerOne;
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
	public override string GetAttackInputTag  () {
		return Tags.UserInputs.attackPlayer2;
	}
}