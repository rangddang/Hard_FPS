using UnityEngine;

public class GunController : MonoBehaviour
{
	public bool aiming;

	private GunAnimationController gunAnim;

	private void Awake()
	{
		gunAnim = GetComponent<GunAnimationController>();
	}

	public void Fire()
	{
		gunAnim.FireAnimation();
	}

	public void Reload()
	{
		gunAnim.ReloadAnimation();
	}

	public void Aiming()
	{
		aiming = true;
		gunAnim.AimAnimation(aiming);
	}

	public void NotAim()
	{
		aiming = false;
		gunAnim.AimAnimation(aiming);
	}

	
}
