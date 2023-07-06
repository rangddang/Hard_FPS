using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float gravityScale = -9.81f;
	[SerializeField] private float slidingSpeed;
	[SerializeField] private float slidingTime;
	[SerializeField] private float dashCooldown;

	[SerializeField] private Vector3 moveVelo;
	[SerializeField] private Vector3 slidingVelo;

	private CharacterController character;
	private Transform head;

	public bool isSliding;

	private float lastSlidingTime;


    private void Awake()
    {
		character = GetComponent<CharacterController>();
		head = transform.GetChild(0);
	}

    private void Update()
    {
        UpdateGravity();
		character.Move(moveVelo * Time.deltaTime);
	}

    public void Move(Vector3 dir)
    {
		if (isSliding) return;

		moveVelo = dir + (moveVelo.y * Vector3.up);
    }

	public void Sliding(Vector3 dir)
	{
		if (isSliding) return;

		if (Time.time - lastSlidingTime < dashCooldown) return;

		slidingVelo = dir;
		//moveVelo.y = 0;
		StopCoroutine("OnSliding");
		StartCoroutine("OnSliding");
	}

	public void StopSliding(Vector3 dir)
	{
		isSliding = false;

		if(dir != Vector3.zero)
			slidingVelo = dir;

		//moveVelo.y = 0;
		StopCoroutine("OnSliding");
		StopCoroutine("JumpSliding");
		StartCoroutine("JumpSliding");
	}

	public void Jump(float jumpScale)
    {
		moveVelo.y = jumpScale;
	}

	public void Crouch(bool isCrouch)
	{
		float height = isCrouch ? 1.1f : 2f;
		float center = isCrouch ? -0.5f : 0f;
		float headPos = isCrouch ? -0.3f : 0.7f;

		head.localPosition = Vector3.Lerp(head.localPosition, Vector3.up * headPos, Time.deltaTime * 10f);
		character.center = Vector3.Lerp(character.center, Vector3.up * center, Time.deltaTime * 10f);
		character.height = Mathf.Lerp(character.height, height, Time.deltaTime * 10f);
	}

	private void UpdateGravity()
    {
		if (!character.isGrounded)
		{
			moveVelo.y += gravityScale * Time.deltaTime;
		}
	}

	private IEnumerator OnSliding()
	{
		isSliding = true;
		float currentTime = 0;
		float currentSlidingSpeed = slidingSpeed;
		float t = 0;

		while (true)
		{
			Crouch(true);
			character.Move(slidingVelo * currentSlidingSpeed * Time.deltaTime);

			currentTime += Time.deltaTime;
			t= currentTime / slidingTime;
			t = t * t;

			currentSlidingSpeed = Mathf.Lerp(slidingSpeed, 0, t);
			if (currentTime >= slidingTime)
			{
				lastSlidingTime = Time.time;
				isSliding = false;
				yield break;
			}
			yield return null;
		}
	}

	private IEnumerator JumpSliding()
	{
		float currentTime = 0;
		float slidM = 0.7f;
		float currentSlidingSpeed = slidingSpeed * slidM;
		float t = 0;

		while (true)
		{
			Crouch(true);
			character.Move(slidingVelo * currentSlidingSpeed * Time.deltaTime);

			currentTime += Time.deltaTime;
			t = currentTime / slidingTime;
			t = t * t;

			currentSlidingSpeed = Mathf.Lerp(slidingSpeed * slidM, 0, t);
			if (currentTime >= slidingTime * 0.8f)
			{
				lastSlidingTime = Time.time;
				yield break;
			}
			yield return null;
		}
	}
}
