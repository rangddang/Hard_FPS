using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationController : MonoBehaviour
{
    private Animator animator;
	private GunController gun;

    private void Awake()
    {
        animator = GetComponent<Animator>();
		gun = GetComponent<GunController>();
    }

	public void AimAnimation(bool aiming)
	{
		animator.SetBool("Aim", aiming);
	}

	public void ReloadAnimation()
	{
		animator.SetTrigger("Reload");
	}

	public void FireAnimation()
	{
		if (gun.aiming)
		{
			animator.Play("AimFire", -1, 0);
		}
		else
		{
			animator.Play("Fire", -1, 0);
		}
	}

	public void CloseAttackAnimation()
	{
		int rand = Random.Range(1, 2 + 1);
		animator.Play("CloseAttack_"+ rand, -1, 0);
	}

	public void InspectAnimation()
	{
		animator.SetTrigger("Inspect");
	}
}
