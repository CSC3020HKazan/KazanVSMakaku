using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player2Behaviour : PlayerBehaviour {

	protected override void InitialiseTag () {
		gameObject.tag = Tags.playerTwo;
		gameObject.name = Tags.playerTwo;
	}

	public override string GetAttackInputTag  () {
		return Tags.UserInputs.attackPlayer2;
	}

}