using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float gravityScale = -9.81f;
	[SerializeField] private Vector3 moveVelo;

	private CharacterController character;
	private Transform head;

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
        moveVelo = (dir) + (moveVelo.y * Vector3.up);
    }

    public void Jump(float jumpScale)
    {
		moveVelo.y = jumpScale;
	}

	public void Crouch(bool isCrouch)
	{
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
            print("Áß·ÂÀÌ ´À²¸Áøµå¾Æ¾Ñ!!!");
			moveVelo.y += gravityScale * Time.deltaTime;
		}
	}
}
