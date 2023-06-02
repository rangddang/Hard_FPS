using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GunController gun;
    private PlayerActive active;
    private CharacterController character;

    private void Awake()
    {
        active = GetComponent<PlayerActive>();
        character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            gun.Bang();
        if (Input.GetMouseButton(1))
            gun.Aiming();
        else
            gun.NotAim();
		active.Move();
        active.FirstPerson();
        active.Gravity();
        active.Crouch();
		if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("Mouse ScrollWheel") > 0) && character.isGrounded)
			active.Jump();
    }
}
