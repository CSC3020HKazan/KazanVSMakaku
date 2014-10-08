using UnityEngine;
using System.Collections;

public class ElementBallBehaviour : MonoBehaviour {
	[SerializeField]
	private float potency = 3f;
	[SerializeField]
	private float manaValue = 2f;

	void Awake () {

	}

	void Start () {
		gameObject.tag = Tags.elementBall;
		gameObject.name = Tags.elementBall;
	}

	void Update () {

	}

	public float GetPotency () {
		return potency;
	}

	public float GetManaValue () {
		return manaValue;
	}
} 