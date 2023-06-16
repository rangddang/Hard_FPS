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
	[SerializeField] protected float hitTime;
	[SerializeField] protected float attackRange;
	[SerializeField] protected bool isDead = false;
	protected bool isHit = false;
	protected bool isAttack = false;

	protected Transform target;
	protected Animator anim;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		target = FindObjectOfType<PlayerController>().transform;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		anim = GetComponent<Animator>();
	}

	public virtual void Knockback(Vector3 dir, float attackPower)
	{
		if (!isDead) return;

		rigid.AddForce(dir * attackPower, ForceMode.Impulse);
	}

	public virtual void Hit(float damage)
	{
		if (isDead) return;

		hp -= damage;
		StartCoroutine("HitCoroutine");
		if(hp <= 0)
		{
			Dead();
		}
	}

	public virtual void Dead()
	{
		hp = 0f;
		isDead = true;
		rigid.constraints = RigidbodyConstraints.None;
		anim.enabled = false;
	}

	public virtual void Move()
	{
		if (isDead) return;

		if (isHit) return;

		transform.rotation = Quaternion.LookRotation(target.position - transform.position);
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}

	public virtual void Attack()
	{
		if (isDead) return;

		if (isHit) return;

		if (isAttack) return;

		anim.SetTrigger("Attack");
		StartCoroutine("AttackCoroutine");
	}

	private IEnumerator HitCoroutine()
	{
		isHit = true;
		anim.SetBool("IsHit", isHit);
		yield return new WaitForSeconds(hitTime);
		isHit = false;
		anim.SetBool("IsHit", isHit);
	}

	private IEnumerator AttackCoroutine()
	{
		isAttack = true;
		yield return new WaitForSeconds(0.66f);
		//공격 콜라이더 활성화
		yield return new WaitForSeconds(0.04f);
		//공격 콜라이더 비활성화
		yield return new WaitForSeconds(0.3f);
		isAttack = false;
	}
}
