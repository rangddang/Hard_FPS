using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
	[SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
	[SerializeField] private KeyCode reloadKey = KeyCode.R;
	[SerializeField] private KeyCode closeAttackKey = KeyCode.Mouse2;
	[SerializeField] private KeyCode viewWeaponDetailsKey = KeyCode.Y;
	[SerializeField] private KeyCode runKey = KeyCode.LeftShift;
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

	private Vector3 dir;
	private float currentSpeed;

    private PlayerMovement movement;
	private PlayerRotate playerRotate;
	private Status status;
	private GunController gun;
	private CharacterController character;
	private CameraController camera;

	private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
		playerRotate = GetComponent<PlayerRotate>();
		status = GetComponent<Status>();
        gun = FindObjectOfType<GunController>();
		character = GetComponent<CharacterController>();
		camera = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
		RotateToMouse();
		Move();
		GunAct();
	}

	private void RotateToMouse()
	{
		float mouseY = Input.GetAxis("Mouse Y");
		float mouseX = Input.GetAxis("Mouse X");

		playerRotate.RotateHead(mouseY);
		playerRotate.RotateBody(mouseX);
	}

	private void Move()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		bool isCrouch = Input.GetKey(crouchKey);

		
		if (character.isGrounded)
		{
			if (horizontal != 0 || vertical != 0)
			{
				dir = (vertical * transform.forward) + (horizontal * transform.right);
				dir.Normalize();
				if (isCrouch)
				{
					if(!gun.aiming)
						camera.ZoomCamera(ZoomCam.Nomal);
					currentSpeed = status.CrouchSpeed;
				}
				else if (Input.GetKey(runKey) && !gun.aiming)
				{
					camera.ZoomCamera(ZoomCam.Run);
					currentSpeed = status.RunSpeed;
				}
				else
				{
					if (!gun.aiming)
						camera.ZoomCamera(ZoomCam.Nomal);
					currentSpeed = status.WalkSpeed;
				}
			}
			else
			{
				dir = Vector3.zero;
				currentSpeed = 0;
			}
		}

		if (Input.GetKeyDown(jumpKey) && character.isGrounded)
		{
			print("¶ì¿ë!");
			movement.Jump(status.JumpScale);
		}
		movement.Crouch(isCrouch);
		movement.Move(dir * currentSpeed);
	}

    private void GunAct()
    {
		if (Input.GetKeyDown(fireKey))
		{
			gun.Fire();
		}
		if (Input.GetKeyDown(aimKey))
		{
			camera.ZoomCamera(ZoomCam.Zoom);
			gun.Aiming();
		}
		else if (Input.GetKeyUp(aimKey))
		{
			camera.ZoomCamera(ZoomCam.Run);
			gun.NotAim();
		}
		if (Input.GetKeyDown(reloadKey))
		{
			gun.Reload();
		}
	}
}
