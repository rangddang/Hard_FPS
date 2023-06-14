using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
	protected Rigidbody rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	public virtual void Knockback(Vector3 dir, float attackPower)
	{
		rigid.AddForce(dir * attackPower, ForceMode.Impulse);
	}
}
