using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityScale = -9.81f;
	private Vector3 moveVelo;
    private float gravity;
	private CharacterController character;

    private void Awake()
    {
		character = GetComponent<CharacterController>();
	}

    private void Update()
    {
        UpdateGravity();
		character.Move(moveVelo);
	}

    public void Move(Vector3 dir)
    {
        moveVelo = dir + (moveVelo.y * Vector3.up);

    }

    private void UpdateGravity()
    {
		if (!character.isGrounded)
		{
			moveVelo.y += gravity * Time.deltaTime;
		}
	}
}
