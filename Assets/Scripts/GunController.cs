using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	[SerializeField] private float closeAttackLate;
	[SerializeField] private float reloadLate;

	[SerializeField] private GameObject fireEffect;

	[SerializeField] private AnimationClip reloadClip;
	[SerializeField] private AnimationClip InspectClip;

	public bool aiming;

	private GunAnimationController gunAnim;
	private CameraController camera;
	private GunSetting gunSetting;

	private float lastAttackTime = 0;
	private bool isReload = false;
	private bool isCloseAttacked = false;
	private bool isInspected = false;

	private void Awake()
	{
		gunAnim = GetComponent<GunAnimationController>();
		gunSetting = GetComponent<GunSetting>();
		camera = Camera.main.GetComponent<CameraController>();

		gunSetting.currentAmmo = gunSetting.maxAmmo;
	}

	private void Update()
	{
		if (isReload || isCloseAttacked || isInspected)
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

		OnFire();

	}

	public void Reload()
	{
		if (isReload) return;

		if (isCloseAttacked) return;

		if (gunSetting.currentAmmo >= gunSetting.maxAmmo) return;

		if (gunSetting.ammos <= 0) return;

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
		if (Time.time - lastAttackTime > gunSetting.attackRate)
		{
			lastAttackTime = Time.time;

			if (gunSetting.currentAmmo <= 0)
			{
				Reload();
				return;
			}

			gunSetting.currentAmmo--;

			camera.FireCamera();
			gunAnim.FireAnimation();

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

				if (gunSetting.ammos >= gunSetting.maxAmmo)
				{
					gunSetting.ammos -= gunSetting.maxAmmo - gunSetting.currentAmmo;
					gunSetting.currentAmmo = gunSetting.maxAmmo;
				}
				else
				{
					int ammo = gunSetting.ammos;
					gunSetting.ammos -= gunSetting.ammos - gunSetting.currentAmmo;
					gunSetting.currentAmmo = ammo;
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

		gunAnim.CloseAttackAnimation();

		float currentTime = 0;

		while (true)
		{
			currentTime += Time.deltaTime;

			if (currentTime >= closeAttackLate)
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
		fireEffect.SetActive(true);
		yield return new WaitForSeconds(gunSetting.attackRate * 0.5f);
		fireEffect.SetActive(false);
	}
}
