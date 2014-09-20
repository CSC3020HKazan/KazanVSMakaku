using UnityEngine;
using System.Collections;

public class PickupBehaviour : MonoBehaviour {
	[SerializeField]
	private float manaReward = 10f;
	[SerializeField]
	private float healthReward = 10f;
	[SerializeField]
	private float lifeTime = 2f;

	void Start () {
		if (lifeTime > 0)
			Destroy (this, 20+lifeTime);
	}

	void Update () {

	}

	public float GetHealthReward () {
		return healthReward;
	}

	public float GetManaReward () {
		return manaReward;
	}

}