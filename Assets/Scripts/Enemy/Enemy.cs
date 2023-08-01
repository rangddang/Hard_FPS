using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	protected enum AttackType
	{
		Physical,
		Magical
	}

	protected Rigidbody rigid;
	protected Animator anim;

	protected Transform target;

	[Header("EnemyStatus")]
	[SerializeField] protected AttackType attackType;
	[SerializeField] protected float hp;
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected float attackRange;
	[SerializeField] protected float hitTime;

	[Header("EnemyState")]
	[SerializeField] protected bool isDead = false;
	[SerializeField] protected bool isFaint = false;
	[SerializeField] protected bool isStun = false;
	protected bool faint = false;
	protected bool isHit = false;
	protected bool isAttack = false;


	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		target = FindObjectOfType<PlayerController>().transform;
		anim = GetComponent<Animator>();
		Wake();
	}

	public virtual void Knockback(Vector3 dir, float attackPower)
	{
		if (!isFaint) return;

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
		Faint();
	}

	public virtual void Move(Vector3 dir)
	{
		if (isDead) return;

		if (isFaint) return;

		if (isHit) return;

		if (isAttack) return;



		Quaternion targetRotate = Quaternion.LookRotation(new Vector3(target.position.x,transform.position.y, target.position.z) - transform.position);

		transform.rotation = targetRotate;

		rigid.MovePosition(transform.position + (dir.normalized * Time.deltaTime * moveSpeed));
	}

	public virtual void Attack()
	{
		if (isDead) return;

		if (isFaint) return;

		if (isHit) return;

		if (isAttack) return;

		anim.SetTrigger("Attack");
		StartCoroutine("AttackCoroutine");
	}

	public virtual void Wake()
	{
		isFaint = false;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		anim.enabled = true;
	}

	public virtual void Faint()
	{
		isFaint = true;
		faint = true;
		rigid.constraints = RigidbodyConstraints.None;
		StartCoroutine("StopAnim");
		if (!isDead)
		{
			StopCoroutine("Fainted");
			StartCoroutine("Fainted");
		}
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
		yield return new WaitForSeconds(1f);
		isAttack = false;
	}

	private IEnumerator StopAnim()
	{
		anim.Play("Enemy_Walk", -1, 0);
		yield return new WaitForSeconds(0.05f);
		anim.enabled = false;
	}

	private IEnumerator Fainted()
	{
		float faintedTime = 5;
		yield return new WaitForSeconds(faintedTime);
		if (isDead) yield break;
		faint = false;
		StartCoroutine("WakeAnimation");
	}

	private IEnumerator WakeAnimation()	
	{
		if (isDead) yield break;
		if (faint) yield break;
		rigid.constraints = RigidbodyConstraints.FreezeRotation;
		Quaternion targetRotate = Quaternion.LookRotation(new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position);
		while (true)
		{
			if (isDead) yield break;
			if (faint) yield break;
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, Time.deltaTime * 12);
			if (Quaternion.Angle(transform.rotation, targetRotate) < 5f)
			{
				transform.rotation = targetRotate;
				Wake();
				yield break;
			}
			yield return null;
		}
	}
}
