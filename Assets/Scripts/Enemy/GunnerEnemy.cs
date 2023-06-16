using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerEnemy : Enemy
{
	private void Update()
	{
		if(Vector3.Distance(transform.position, target.position) < attackRange)
		{
			Attack();
		}
		if(!isAttack)
			Move();
	}
}
