using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CloseAttack : MonoBehaviour
{
	[SerializeField] private float attackPower;

	public bool canHit;

	private Transform head;

	private void Awake()
	{
		head = transform.parent;
	}

	private void OnTriggerEnter(Collider other)
    {
		if (other.transform.CompareTag("Enemy") && canHit == true)
		{
			canHit = false;
			other.transform.GetComponent<Enemy>().Faint();
			other.transform.GetComponent<Enemy>().Knockback(head.forward, attackPower);
		}
		else if (other.transform.CompareTag("Object"))
		{
			other.transform.GetComponent<Object>().Knockback(head.forward, attackPower);
		}
	}
}
