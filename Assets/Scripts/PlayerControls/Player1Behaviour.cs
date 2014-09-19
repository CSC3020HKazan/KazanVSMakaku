using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player1Behaviour : PlayerBehaviour {
	protected override void InitialiseTag () {
		gameObject.tag = Tags.playerOne;
		gameObject.name = Tags.playerOne;
		_playerIndex = 1;
	}

	public override string GetAttackInputTag  () {
		return Tags.UserInputs.attackPlayer1;
	}

}