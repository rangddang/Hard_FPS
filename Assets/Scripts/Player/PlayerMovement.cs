using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float gravityScale = -9.81f;
	[SerializeField] private float dashSpeed;
	[SerializeField] private float dashTime;
	[SerializeField] private float dashCooldown;

	[SerializeField] private Vector3 moveVelo;
	[SerializeField] private Vector3 dashVelo;

	public bool isDash;
	private float lastDashTime;

	private CharacterController character;
	private Transform head;

    private void Awake()
    {
		character = GetComponent<CharacterController>();
		head = transform.GetChild(0);
	}

    private void Update()
    {
		if(isDash) return;

        UpdateGravity();
		character.Move(moveVelo * Time.deltaTime);
	}

    public void Move(Vector3 dir)
    {
		if (isDash) return;

		moveVelo = (dir) + (moveVelo.y * Vector3.up);
    }

	public void Dash(Vector3 dir)
	{
		if (isDash) return;

		if (Time.time - lastDashTime < dashCooldown) return;

		dashVelo = dir;
		moveVelo.y = 0;
		StartCoroutine("OnDash");
	}

	public void Jump(float jumpScale)
    {
		if (isDash) return;

		moveVelo.y = jumpScale;
	}

	public void Crouch(bool isCrouch)
	{
		if (isDash) return;

		float height = isCrouch ? 1f : 2f;
		float center = isCrouch ? -0.5f : 0f;
		float headPos = isCrouch ? 0.2f : 0.6f;

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

	private IEnumerator OnDash()
	{
		isDash = true;
		float currentTime = 0;

		while (true)
		{
			character.Move(dashVelo * dashSpeed * Time.deltaTime);

			currentTime += Time.deltaTime;
			if (currentTime >= dashTime)
			{
				lastDashTime = Time.time;
				isDash = false;
				yield break;
			}
			yield return null;
		}
	}
}
