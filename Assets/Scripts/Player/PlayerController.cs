using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
	[SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
	[SerializeField] private KeyCode reloadKey = KeyCode.R;
	[SerializeField] private KeyCode closeAttackKey = KeyCode.Mouse2;
	[SerializeField] private KeyCode inspectKey = KeyCode.Y;
	[SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

	public bool isDead = false;
	[SerializeField] private GameObject dieMessage;

	private bool isDash;
	private bool isCrouch;

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

		dieMessage.SetActive(isDead);
	}

    private void Update()
    {
		if (isDead) return;

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

		isCrouch = Input.GetKey(crouchKey);


		if (horizontal != 0 || vertical != 0)
		{
			isDash = Input.GetKeyDown(dashKey);
			//dir += ((vertical * transform.forward) + (horizontal * transform.right)) * Time.deltaTime * 3;
			dir = (vertical * transform.forward) + (horizontal * transform.right);
			if (isCrouch && character.isGrounded)
			{
				currentSpeed = status.CrouchSpeed;
			}
			else if (isDash && !gun.aiming)
			{
				movement.Dash(dir);
			}
			else
			{
				currentSpeed = status.WalkSpeed;
			}
		}
		else if (character.isGrounded)
		{
			isDash = false;
			dir = Vector3.zero;
			currentSpeed = 0;
		}

		if(Mathf.Abs(dir.x) > 1 || Mathf.Abs(dir.z) > 1)
		{
			dir.Normalize();
		}

		if (!gun.aiming && !isDash)
		{
			camera.ZoomCamera(ZoomCam.Nomal);
		}

		if (Input.GetKeyDown(jumpKey) && character.isGrounded)
		{
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
			gun.Aiming(true);
		}
		else if (Input.GetKeyUp(aimKey))
		{
			gun.Aiming(false);
		}
		if (Input.GetKeyDown(reloadKey))
		{
			gun.Reload();
		}
		if (Input.GetKeyDown(closeAttackKey))
		{
			gun.CloseAttack();
		}
		if (Input.GetKeyDown(inspectKey))
		{
			gun.InspectWeapon();
		}
	}

	public void Dead()
	{
		isDead = true;
		dieMessage.SetActive(isDead);
		dir = Vector3.zero;
		movement.Move(dir);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("EnemyAttack"))
		{
			Dead();
		}
	}
}
