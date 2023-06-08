using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

	public void AimAnimation(bool aiming)
	{
		animator.SetBool("Aim", aiming);
	}

	public void ReloadAnimation()
	{
		animator.SetTrigger("Reload");
	}

	public void BangAnimation()
	{
		animator.SetTrigger("Bang");
	}
}
