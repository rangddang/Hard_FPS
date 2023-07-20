using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GunController : MonoBehaviour
{
	[SerializeField] private float closeAttackLate;
	[SerializeField] private float reloadLate;


	[SerializeField] private GameObject fireEffect;
	[SerializeField] private BoxCollider closeAttackRange;

	[SerializeField] private AnimationClip reloadClip;
	[SerializeField] private AnimationClip InspectClip;
	[SerializeField] private AnimationClip closeAttackClip;

	[SerializeField] public BulletSetting bulletSetting;
	[SerializeField] private Status status;

	public bool aiming;

	private Transform head;
	private GunAnimationController gunAnim;
	private CameraController camera;
	private PlayerMovement movement;

	private float lastAttackTime = 0;
	private bool isReload = false;
	private bool isCloseAttacked = false;
	private bool isAnimCloseAttacked = false;
	private bool isInspected = false;

	private RaycastHit hit;

	private void Awake()
	{
		gunAnim = GetComponent<GunAnimationController>();
		camera = Camera.main.GetComponent<CameraController>();
		movement = transform.parent.parent.GetComponent<PlayerMovement>();
		head = transform.parent;

		bulletSetting.currentAmmo = bulletSetting.maxAmmo;

		fireEffect.SetActive(false);
		closeAttackRange.enabled = false;
	}

	private void Update()
	{
		if (movement.isSliding)
		{
			camera.ZoomCamera(ZoomCam.Dash);
		}
		else if (isReload || isCloseAttacked || isInspected)
		{
			camera.ZoomCamera(ZoomCam.Nomal);
		}
		else if (aiming)
		{
			camera.ZoomCamera(ZoomCam.Zoom);
		}
	}

	public void Fire()
	{
		if (isReload) return;

		if (isCloseAttacked) return;

		isInspected = false;
		OnFire();
	}

	public void Reload()
	{
		if (isReload) return;

		if (isCloseAttacked) return;
		//Áß¿ä
		if (bulletSetting.currentAmmo >= bulletSetting.maxAmmo) return;

		if (bulletSetting.hasAmmo <= 0) return;

		isInspected = false;
		StartCoroutine("OnReload");
	}

	public void Aiming(bool aim)
	{
		aiming = aim;
		gunAnim.AimAnimation(aiming);
	}
	public void CloseAttack()
	{
		if (isReload) return;

		if (isCloseAttacked) return;

		isInspected = false;
		StartCoroutine("OnCloseAttack");
	}

	public void InspectWeapon()
	{
		if (isReload) return;

		if (isCloseAttacked) return;

		if (isInspected) return;

		StartCoroutine("OnInspect");
	}

	private void OnFire()
	{
		if (Time.time - lastAttackTime > status.attackLate)
		{

			lastAttackTime = Time.time;

			if (bulletSetting.currentAmmo <= 0)
			{
				Reload();
				return;
			}

			bulletSetting.currentAmmo--;

			if(Physics.Raycast(head.position, Camera.main.transform.forward, out hit, 500))
			{
				if (hit.transform.CompareTag("Enemy"))
				{
					hit.transform.GetComponent<Enemy>().Hit(status.attackDamage);
					hit.transform.GetComponent<Enemy>().Knockback(head.forward, status.attackDamage);
				}
				else if (hit.transform.CompareTag("Object"))
				{
					hit.transform.GetComponent<Object>().Knockback(head.forward, status.attackDamage);
				}
			}

			camera.FireCamera();
			gunAnim.FireAnimation(status.attackLate);

			StartCoroutine("FireEffect");

		}
	}

	private IEnumerator OnReload()
	{
		bool reload = false;

		isReload = true;

		gunAnim.ReloadAnimation();

		float currentTime = 0;

		while (true)
		{
			currentTime += Time.deltaTime;

			if (currentTime >= reloadLate && !reload)
			{
				reload = true;

				if (bulletSetting.hasAmmo >= bulletSetting.maxAmmo)
				{
					bulletSetting.hasAmmo -= bulletSetting.maxAmmo - bulletSetting.currentAmmo;
					bulletSetting.currentAmmo = bulletSetting.maxAmmo;
				}
				else
				{
					int ammo = bulletSetting.hasAmmo;
					bulletSetting.hasAmmo -= bulletSetting.hasAmmo - bulletSetting.currentAmmo;
					bulletSetting.currentAmmo = ammo;
				}

			}

			if(reload && currentTime >= reloadClip.length)
			{
				isReload = false;

				yield break;
			}

			yield return null;
		}
	}

	private IEnumerator OnCloseAttack()
	{
		isCloseAttacked = true;

		CloseAttack closeAttack = closeAttackRange.GetComponent<CloseAttack>();
		int attackTrigger = 0;

		gunAnim.CloseAttackAnimation();

		float currentTime = 0;

		while (true)
		{
			currentTime += Time.deltaTime;

			if(currentTime >= 0.05f && attackTrigger == 0)
			{
				closeAttack.canHit = true;
				attackTrigger = 1;
				closeAttackRange.enabled = true;
			}
			if (currentTime >= 0.14f && attackTrigger == 1)
			{
				closeAttack.canHit = false;
				attackTrigger = 2;
				closeAttackRange.enabled = false;
			}


			if (currentTime >= closeAttackClip.length)
			{
				isCloseAttacked = false;
				
				yield break;
			}

			yield return null;
		}
	}

	private IEnumerator OnInspect()
	{
		isInspected = true;

		gunAnim.InspectAnimation();

		float currentTime = 0;

		while (true)
		{
			currentTime += Time.deltaTime;

			if (currentTime >= InspectClip.length)
			{
				isInspected = false;

				yield break;
			}

			yield return null;
		}
	}

	private IEnumerator FireEffect()
	{
		float rand = Random.Range(0f, 360f);

		fireEffect.SetActive(true);
		fireEffect.transform.localRotation = Quaternion.Euler(0, 0, rand);
		yield return new WaitForSeconds(status.attackLate * 0.3f);
		fireEffect.SetActive(false);
	}
}
