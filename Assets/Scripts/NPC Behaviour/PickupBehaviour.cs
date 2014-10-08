using UnityEngine;
using System.Collections;

public class PickupBehaviour : MonoBehaviour {
	[SerializeField]
	private float manaReward = 10f;
	[SerializeField]
	private float healthReward = 10f;
	[SerializeField]
	private float lifeTime = 2f;

	private bool _isHealthTaken = false;
	private bool _isManaTaken = false;


	void Start () {
		gameObject.tag =  Tags.pickup;
		gameObject.name = Tags.pickup;

		Mathf.Clamp (healthReward, 0, GameMaster.Pickups.MAX_HEALTH_REWARD);
		Mathf.Clamp (manaReward, 0, GameMaster.Pickups.MAX_MANA_REWARD); 

	}

	void Update () {
		if (_isHealthTaken && _isManaTaken)
			Destroy (this, lifeTime);  
	}

	public float GetHealthReward () {
		if (!_isHealthTaken) {
			_isHealthTaken = true;
			return healthReward;
		} else
			return 0.0f;
	}

	public float GetManaReward () {
		if (!_isManaTaken) {
			_isManaTaken =true;
			return manaReward;

		} else 
			return 0.0f;
	}

}