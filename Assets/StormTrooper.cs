using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class StormTrooper : MonoBehaviour
{
    AudioSource source;
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

	private void Awake()
	{
		playerCamT = PlaySpaceRelativity.cameraT;
		source = GetComponentInChildren<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
		bulletsT = GameObject.Find("Bullets").transform;
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
		StartCoroutine(MasterRoutine());
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
			anim.SetFloat("Forward", forwardLerp);
			anim.SetFloat("Side", sideLerp);
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
			spineT.localEulerAngles = spineT.localEulerAngles.WithX(Mathf.Clamp(x, -XClamp, XClamp)).WithY(Mathf.Clamp(y, -YClamp, YClamp));

		}
	}


	IEnumerator MasterRoutine()
	{
		while (true)
		{
			yield return MoveToNextPoint();
			yield return new WaitForSeconds(2);
			yield return FireAtPlayerRoutine();
			//yield return TurnToPlayer();
			//yield return AimAtPlayer();
			//yield return FireAtPlayerRoutine();
			yield return ReturnSpine();
			yield return new WaitForSeconds(2);

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

			spineRot = Quaternion.Slerp(startRot, startRot * turnRot, Mathf.SmoothStep(0,1,curTime / lerpTime));
		}

		yield return null;
		yield return null;

	}

	IEnumerator FireAtPlayerRoutine()
	{
		yield return TurnToPlayer();
		yield return AimAtPlayer();
		yield return FireRoutine(3);

	}

	IEnumerator TurnToPlayer()
	{
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

			spineRot = Quaternion.Slerp(startRot, Quaternion.identity, curTime / lerpTime);
		}
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
		return playerCamT.position + Vector3.down;
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


        source.Play();
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






}
