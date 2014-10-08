using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {
	public GameObject fireballPrefab;
	public bool canFire = false;
	public AudioClip collectedPickupClip;

	protected int _playerIndex = 0;
	[SerializeField]
	private float firingForce = 2000f;
	private List<GameObject>_platformTraversed;
	private List<GameObject> _buoyantSteps;
	private Transform _fireballTransform;
	private bool _hasFired = false;
	private PlayerHealth _playerHealth;
	private PlayerMana _playerMana;
	private CharacterController _playerController;
	private CheckPointManager _checkpointManager = new CheckPointManager();
	
	protected virtual void InitialiseTag () {
		gameObject.tag = Tags.player;
		gameObject.name = Tags.player;
	}

	void Start () {
		InitialiseTag();
		_playerHealth = gameObject.GetComponent<PlayerHealth> ();
		if (!_playerHealth) 
			Debug.Log ("No Player Health script attached"); 
		_playerMana = gameObject.GetComponent<PlayerMana> ();
		if (!_playerMana)
			Debug.Log ("No Player Mana script attached");
		_fireballTransform = GameObject.Find(""+gameObject.name +"/FireBallSpawnPoint").transform;
		_platformTraversed = new List<GameObject> ();
		_buoyantSteps = new List<GameObject> ();

		if (audio == null ) {
			gameObject.AddComponent<AudioSource> () ;
		}
		_checkpointManager.AddCheckPoint (new CheckPoint (transform.position, transform.rotation));

	}

	void Update () {
		if (canFire) {
			if (!_fireballTransform) {
				Debug.Log ("No FireBallSpawnPoint GameObject");
				return;
			}
			CheckFire (Input.GetButtonDown(GetAttackInputTag()), _fireballTransform);
			if (Input.GetButtonUp(GetAttackInputTag())) {
				FireProjectile(_fireballTransform);
			}

		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (!body)
			return;
		CheckPlatformCollision (hit.gameObject);
		CheckStepCollision(hit.gameObject);
		if (!_playerHealth)
			return;
		CheckProjectileCollision (hit.gameObject);
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.tag == Tags.pickup) {
			if (_playerMana == null || _playerHealth == null) 
				return;
			else {
				CheckPickupCollision (coll.gameObject) ;

			}
		}
	}

	private void CheckFire (bool attack, Transform _fireballTransform) {
		if (!_fireballTransform) {
			Debug.Log ("No FireBallSpawnPoint GameObject");
			return;
		}
		if(attack) {
			_hasFired = false;
			RaycastHit hit; // do the exploratory raycast first:
			if (Physics.Raycast (_fireballTransform.position, _fireballTransform.forward, out hit)){
				float delay = hit.distance / (2000); // calculate the flight time
				Vector3 hitPt = hit.point;
				hitPt.y -= delay * GameMaster.Physics.GRAVITY; // calculate the bullet drop at the target
				Vector3 dir = hitPt - _fireballTransform.position; // use this to modify the shot direction
				// then do the actual shooting:
				Debug.DrawLine (_fireballTransform.position, hit.point, Color.green, 5);
				if (Physics.Raycast (_fireballTransform.position, dir, out hit)){
					//Debug.Log ("Sending ray");
					// do here the usual job when something is hit: 
					// apply damage, instantiate particles etc.	
				}
			}
		}
	}

	private void FireProjectile (Transform _fireballTransform) {
		if (!_fireballTransform) {
			Debug.Log ("No FireBallSpawnPoint GameObject");
			return;
		}
		float elementBallManaValue  = 0f;
		
		ElementBallBehaviour ballBehaviour = fireballPrefab.GetComponent<ElementBallBehaviour>(); 
		if (ballBehaviour != null)
			elementBallManaValue = ballBehaviour.GetManaValue();
		//Debug.Log (elementBallManaValue); 
		if (_playerMana != null ) {
			if (canFire && !_hasFired && _playerMana.GetCurrentMana() > elementBallManaValue ) {
				GameObject fireballClone = (GameObject) Instantiate (fireballPrefab, _fireballTransform.position, _fireballTransform.rotation);
				fireballClone.transform.LookAt(_fireballTransform.forward);
				fireballClone.rigidbody.AddForce (_fireballTransform.forward * firingForce, ForceMode.Acceleration);
				_hasFired = true;
				_playerMana.ExhaustMana (elementBallManaValue);
				//fireTimeout = 0.0F;
			}
		}
	}

	private void CheckPlatformCollision (GameObject hitObject) {
		BasePlatform basePlatform = hitObject.GetComponent<BasePlatform> ();
		if(!basePlatform)
			return;
		// Debug.Log(_platformTraversed.Count);
		if (_platformTraversed.Count > 0) {
			if (_platformTraversed[_platformTraversed.Count - 1].Equals (hitObject))
				return;
			_platformTraversed[_platformTraversed.Count - 1].GetComponent<BasePlatform>().SetPlayerPresence(false,_playerIndex);
			Debug.Log(hitObject.ToString());
			_platformTraversed.Add(hitObject);
			if (_platformTraversed.Count % GameMaster.CHECKPOINT_FREQUENCY == 0) {
				AddCheckPoint ();
			}
		} else {
			_platformTraversed.Add(hitObject);
			AddCheckPoint () ;
		}
		basePlatform.SetPlayerPresence(true, _playerIndex);
	} 

	private void CheckStepCollision (GameObject hitObject) {
		Buoyancy buoyant = hitObject.GetComponent<Buoyancy> ();
		if(!buoyant)
			return;
		// Debug.Log(_platformTraversed.Count);
		if (_buoyantSteps.Count > 0) {
			if (_buoyantSteps[_buoyantSteps.Count - 1].Equals (hitObject))
				return;
			_buoyantSteps.Add(hitObject);
		} else
			_buoyantSteps.Add(hitObject);			
	} 

	private void CheckProjectileCollision (GameObject hitObject) {
		ElementBallBehaviour projectileBehaviour = hitObject.GetComponent<ElementBallBehaviour> ();
		if (!projectileBehaviour)
			return;
		if (!_playerHealth) {
			Debug.Log ("No Player Health script attached");
			return;
		}
		if (_playerHealth.IsAlive()) {
			_playerHealth.TakeDamage (projectileBehaviour.GetPotency()); 
		} 
	}

	private void AddCheckPoint  () {
		_checkpointManager.AddCheckPoint(new CheckPoint (transform.position, transform.rotation));
		PlayerMovement movement = gameObject.GetComponent<PlayerMovement> ();
		if (movement != null) {
			HeadsUpDisplay hud = GameObject.FindGameObjectWithTag(movement.GetAttachedCameraTag()).GetComponentInChildren<HeadsUpDisplay> ();
			if (hud != null) {
				hud.DisplayMessage ("CHECKPOINT REACHED!", MessageType.WARNING); 
			}
		}
	}

	private void CheckPickupCollision (GameObject hitObject) {
		PickupBehaviour pickupBehaviour = hitObject.GetComponent<PickupBehaviour> ();
		if (!pickupBehaviour)
			return;
		// Debug.Log("CheckPickupCollision");
		float currentMana = _playerMana.GetCurrentMana();
		float currentHealth = _playerHealth.GetCurrentHealth();
		_playerMana.ReplenishMana (pickupBehaviour.GetManaReward());
		_playerHealth.HealPlayer (pickupBehaviour.GetHealthReward());	
		if (currentHealth != _playerHealth.GetCurrentHealth() || currentMana != _playerMana.GetCurrentMana()) {
			audio.clip = collectedPickupClip;
			audio.Play();
			Destroy (hitObject, 0);
		}
	} 

	public virtual string GetAttackInputTag  () {
		return Tags.UserInputs.attack;
	}

	public CheckPoint RespawnPosition () {
		foreach (GameObject steps in _buoyantSteps ) {
			Buoyancy buoyant = steps.GetComponent<Buoyancy>();
			if (buoyant == null)
				continue;
			buoyant.Reset();
		} 
		return _checkpointManager.GetLastCheckPoint ();
	}
}