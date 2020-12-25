using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class StormTrooper : MonoBehaviour
{
    public AudioSource damageSource;
	public AudioSource standDown, disarm;
    public AudioSource walking;
	NavMeshAgent agent;
	public Transform targetT;
	Animator anim;
	public Transform colliderT, spineT;
	Transform playerCamT;

	Vector2 smoothDeltaPosition = Vector2.zero;
	Vector2 velocity = Vector2.zero;
	public Transform gunPointT;
	public GameObject bulletPrefab;
	Transform bulletsT;
	public AudioSource blastSource;

	public int numHitsNeeded;
	int numHits;
	public bool isMoving;
	public bool isDead;

	public float rotationSpeed;

	public CombatPoint currentCP;

	Quaternion? spineRot;

	float velY, velX;
	Quaternion startSpineLocalRot;

	public bool isCantina;

	public Transform targetCT;
	GameObject trooperGO;


	private void Awake()
	{
		playerCamT = PlaySpaceRelativity.cameraT;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
		bulletsT = GameObject.Find("Bullets").transform;
		trooperGO = transform.GetChild(0).gameObject;
		trooperGO.SetActive(false);
	}

	void OnAnimatorMove()
	{
		// Update position based on animation movement using navigation surface height
		//Vector3 position = anim.rootPosition;
		//position.y = agent.nextPosition.y;
		//transform.position = position;

		//agent.velocity = anim.deltaPosition / Time.deltaTime;
		//transform.position = agent.nextPosition;
		//Vector3 position = anim.rootPosition;
		//position.y = agent.nextPosition.y;
		//transform.position = position;
		//agent.velocity = anim.deltaPosition / Time.deltaTime;

		transform.rotation = anim.rootRotation;
		transform.position = agent.nextPosition;

	}

	private void Start()
	{
		agent.updatePosition = false;
		//StartFireScene();
		startSpineLocalRot = spineT.localRotation;

		if (isCantina)
		{
			//StartCoroutine(CantinaRoutine());
		}

		StartCoroutine(WalkingRoutine());
	}

	private void Update()
	{
		if (isDead) return;

		if (Input.GetKeyDown(KeyCode.B)) anim.SetTrigger("Damage_Right");
		if (Input.GetKeyDown(KeyCode.V)) anim.SetTrigger("Damage_Left");
		if (Input.GetKeyDown(KeyCode.C)) Fire();

		agent.isStopped = !isMoving;

		if (isMoving)
		{
			//agent.destination = targetT.position;
			//agent.Move(targetT.position - transform.position);

			Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

			float dx = Vector3.Dot(transform.right, worldDeltaPosition);
			float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
			Vector2 deltaPosition = new Vector2(dx, dy);

			// Low-pass filter the deltaMove
			float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
			smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

			// Update velocity if time advances
			if (Time.deltaTime > 1e-5f)
				velocity = smoothDeltaPosition / Time.deltaTime;


			// Update animation parameters
			//anim.SetBool("move", shouldMove);
			//anim.SetFloat("velx", velocity.x);
			//anim.SetFloat("vely", velocity.y);
			float forwardLerp = Mathf.InverseLerp(0, 4, velocity.y) * Mathf.Sign(velocity.y);
			float sideLerp = Mathf.InverseLerp(0, 4, velocity.x) * Mathf.Sign(velocity.x);
			velX = Mathf.Lerp(velX, sideLerp, 0.1f);
			velY = Mathf.Lerp(velY, forwardLerp, 0.1f);
			anim.SetFloat("Forward", velY);
			anim.SetFloat("Side", velX);
		}
		else
		{
			anim.SetFloat("Forward", 0);
			anim.SetFloat("Side", 0);
		}

	}

	private void LateUpdate()
	{
		if (spineRot.HasValue)
		{
			spineT.rotation = spineRot.Value;

			float XClamp = 40, YClamp = 30;

			float x = spineT.localEulerAngles.x;
			bool xAdded = false;
			if (x > 180)
			{
				x -= 360;
				xAdded = true;
			}

			float y = spineT.localEulerAngles.y;
			bool yAdded = false;
			if (y > 180)
			{
				y -= 360;
				yAdded = true;
			}

			Vector3 clampedAngles;
			spineT.localEulerAngles = spineT.localEulerAngles.WithX(Mathf.Clamp(x, -XClamp, XClamp)).WithY(Mathf.Clamp(y, -YClamp, YClamp)).WithZ(0);

		}
	}

	public void StartCantinaRoutine()
	{
		StartCoroutine(CantinaRoutine());
		trooperGO.SetActive(true);
	}

	IEnumerator CantinaRoutine()
	{
		isMoving = true;
		agent.destination = targetCT.position;
		yield return new WaitUntil(() => Vector3.Distance(transform.position, targetCT.position) < 1);

		yield return TurnToPlayer();
		StartCoroutine(ContinueAiming());
		disarm.Play();
		yield return new WaitForSeconds(disarm.clip.length);
		yield return new WaitForSeconds(1);
		standDown.Play();
		yield return new WaitForSeconds(standDown.clip.length);

	}

	IEnumerator ContinueAiming()
	{
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);
		yield return AimAtPlayer(1);

	}




	public void StartFireScene()
	{
		StartCoroutine(MasterRoutine());
		isMoving = true;
		trooperGO.SetActive(true);
	}


	IEnumerator MasterRoutine()
	{
		yield return new WaitForSeconds(Random.Range(0, 5));
		while (!isDead)
		{
			yield return MoveToNextPoint();
			yield return new WaitForSeconds(Random.Range(0,2));
			yield return FireAtPlayerRoutine();
			//yield return TurnToPlayer();
			//yield return AimAtPlayer();
			//yield return FireAtPlayerRoutine();
			yield return ReturnSpine();
			yield return new WaitForSeconds(Random.Range(0, 3));

		}
	}

	IEnumerator MoveToNextPoint()
	{
		CombatPoint nextPoint = CombatPoints.instance.GetNearestPoint(transform.position);
		TakePoint(nextPoint);
		MoveToPoint(nextPoint);

		while (Vector3.Distance(transform.position, agent.destination) > 1)
		{
			yield return null;
		}
	}

	IEnumerator FireRoutine(int numFires)
	{
		float fireDelay = 0.5f;
		for (int i = 0; i < numFires; i++)
		{
			Fire();
			yield return AimAtPlayer(0.1f);
			yield return new WaitForSeconds(fireDelay);
		}
	}

	

	IEnumerator FireAtPlayerRoutine()
	{
		yield return TurnToPlayer();
		yield return AimAtPlayer();
		yield return FireRoutine(Random.Range(2,3));

	}

	IEnumerator TurnToPlayer()
	{
		anim.SetLayerWeight(3, 1);
		Vector3 forward = gunPointT.forward;
		Vector3 target = (playerCamT.position - transform.position).normalized;
		forward.y = target.y = 0;
		Quaternion turnRot = Quaternion.FromToRotation(forward, target);
		float dot = Vector3.Dot(gunPointT.right, target);
		anim.SetFloat("Turn", Mathf.Sign(dot));
		float angle = Vector3.Angle(forward, target);
		float turnTime = angle/rotationSpeed;

		Sequence s = DOTween.Sequence();
		s.Append(transform.DORotateQuaternion(transform.rotation * turnRot, turnTime).SetEase(Ease.InOutQuad));
		s.Join(DOTween.To(() => anim.GetFloat("Turn"), x => anim.SetFloat("Turn", x), Mathf.Sign(dot), 1));
		s.Insert(turnTime -1, DOTween.To(() => anim.GetFloat("Turn"), x => anim.SetFloat("Turn", x), 0, 1));

		yield return s.WaitForCompletion();
		anim.SetLayerWeight(3, 0);

	}

	IEnumerator AimAtPlayer(float aimTime = 0.25f)
	{
		Vector3 forward = gunPointT.forward;
		Vector3 target = (PlayerBody() - transform.position).normalized;
		Quaternion turnRot = Quaternion.FromToRotation(forward, target);

		float lerpTime = aimTime;
		float curTime = 0;
		Quaternion startRot = spineT.rotation;

		while (curTime < lerpTime)
		{
			yield return null;
			curTime += Time.deltaTime;

			spineRot = Quaternion.Slerp(startRot, startRot * turnRot, Mathf.SmoothStep(0, 1, curTime / lerpTime));
		}

		yield return null;
		yield return null;

	}

	IEnumerator ReturnSpine()
	{
		float lerpTime = 0.25f;
		float curTime = 0;
		Quaternion startRot = spineT.rotation;

		while (curTime < lerpTime)
		{
			yield return null;
			curTime += Time.deltaTime;

			spineRot = Quaternion.Slerp(startRot, spineT.parent.rotation * startSpineLocalRot, curTime / lerpTime);
		}
		yield return null;
		yield return null;

		spineRot = null;
	}


	void MoveToPoint(CombatPoint cp)
	{
		agent.destination = cp.Position();
		//isMoving = true;
	}

	void TakePoint(CombatPoint cp)
	{
		if (currentCP != null)
		{
			currentCP.ReleasePoint();
		}
		cp.TakePoint();
		currentCP = cp;
	}


	Vector3 PlayerBody()
	{
		return playerCamT.position + Vector3.down * 0.5f;
	}


	void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, bulletsT);
		bullet.transform.position = gunPointT.position;
		bullet.transform.rotation = gunPointT.rotation;

		bullet.GetComponent<BlasterBullet>().SetTrooper(this);

		blastSource.Play();

		anim.SetTrigger("Fire");


	}



	public void Damage(int damage, Vector3 hitPoint)
	{
		if (isDead) return;

		Vector3 projectedPoint = Vector3.ProjectOnPlane(hitPoint - transform.position, Vector3.up).normalized;
		Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
		float angle = Vector3.Angle(projectedPoint, projectedForward);
		float angleLimit = 45f;
		bool isRight = transform.InverseTransformPoint(hitPoint).x > 0;

		string triggerName;
		if (angle < angleLimit)
		{
			triggerName = "Damage_Front";
		}
		else if (isRight)
		{
			triggerName = "Damage_Right";
		}
		else
		{
			triggerName = "Damage_Left";
		}

		anim.SetTrigger(triggerName);

		numHits++;
		if (numHits >= numHitsNeeded)
		{
			Death();
		}


        damageSource.Play();
    }


	[ContextMenu("Death")]
	public void Death()
	{
		string triggerName = Random.value > 0.5f ? "Death01" : "Death02";
		anim.SetTrigger(triggerName);
		isDead = true;
		agent.isStopped = true;
		colliderT.localEulerAngles = Vector3.right * 90;
		colliderT.localPosition = Vector3.forward * 0.25f;
	}





	IEnumerator WalkingRoutine()
	{
		yield return new WaitForSeconds(Random.Range(1, 10));
		while(!isDead)
		{
			while (isMoving)
			{
				if (!walking.isPlaying)
				{
					walking.Play();
					float speedLerp = Mathf.InverseLerp(0, 4, velocity.y);
					float delay = Mathf.Lerp(2, 0, speedLerp);
					yield return new WaitForSeconds(walking.clip.length + delay + Random.Range(0.1f, 0.5f));
				}
				yield return null;
			}
			yield return null;
		}
	}



}
