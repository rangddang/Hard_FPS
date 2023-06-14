using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CloseAttack : MonoBehaviour
{
	public bool canHit;

	[SerializeField] private GunSetting gunSetting;
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
			other.transform.GetComponent<Enemy>().Hit(gunSetting.closeAttackDamage);
			other.transform.GetComponent<Enemy>().Knockback(head.forward, gunSetting.closeAttackPower);
		}
		else if (other.transform.CompareTag("Object"))
		{
			other.transform.GetComponent<Object>().Knockback(head.forward, gunSetting.closeAttackPower);
		}
	}
}
