using UnityEngine;
using System.Collections;

public class ElementBallBehaviour : MonoBehaviour {
	[SerializeField]
	private float potency = 3f;

	void Awake () {

	}

	void Start () {
		gameObject.tag = Tags.elementBall;
		gameObject.name = Tags.elementBall;
	}

	void Update () {

	}

	float GetPotency () {
		return potency;
	}
} 