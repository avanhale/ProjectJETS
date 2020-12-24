using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatPoints : MonoBehaviour
{
	public static CombatPoints instance;

	public CombatPoint[] combatPoints;

	private void Awake()
	{
		instance = this;
		combatPoints = GetComponentsInChildren<CombatPoint>();
	}




	public CombatPoint GetNearestPoint(Vector3 targetPoint)
	{
		CombatPoint nearestPoint = null;
		float nearest = Mathf.Infinity;

		foreach (var combatPoint in combatPoints)
		{
			NavMeshPath path = new NavMeshPath();
			NavMesh.CalculatePath(targetPoint, combatPoint.transform.position, NavMesh.AllAreas, path);
			float distance = GetPathLength(path);
			if (distance < nearest)
			{
				if (!combatPoint.isTaken)
				{
					nearestPoint = combatPoint;
					nearest = distance;
				}
			}
		}

		return nearestPoint;
	}


	float GetPathLength(NavMeshPath path)
	{
		float lng = 0.0f;

		if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
		{
			for (int i = 1; i < path.corners.Length; ++i)
			{
				lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
		}

		return lng;
	}





}
