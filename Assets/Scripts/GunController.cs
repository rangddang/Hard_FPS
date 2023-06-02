using UnityEngine;

public class GunController : MonoBehaviour
{
	[SerializeField] private PlayerActive active;
	private Animator anim;
	private Camera camera;

	public bool aiming;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		camera = Camera.main;
	}

	public void Bang()
	{
		anim.SetTrigger("Bang");
		active.Bang();
	}

	public void Aiming()
	{
		aiming = true;
		Aim();
	}

	public void NotAim()
	{
		aiming = false;
		Aim();
	}

	private void Aim()
	{
		anim.SetBool("Aim", aiming);
		camera.fieldOfView = aiming ? 45f : 65f;
	}
}
