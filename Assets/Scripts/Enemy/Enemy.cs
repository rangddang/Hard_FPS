using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	protected Rigidbody rigid;

	[Header("EnemySettings")]
	[SerializeField] protected float hp;
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected bool isDead = false;


	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	public virtual void Knockback(Vector3 dir, float attackPower)
	{
		if (!isDead) return;

		rigid.AddForce(dir * attackPower, ForceMode.Impulse);
	}

	public virtual void Hit(float damage)
	{
		hp -= damage;
		if(hp <= 0)
		{
			Dead();
		}
	}

	public virtual void Dead()
	{
		hp = 0f;
		isDead = true;
	}
}
