using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
	[SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
	[SerializeField] private KeyCode reloadKey = KeyCode.R;
	[SerializeField] private KeyCode closeAttackKey = KeyCode.Mouse2;
	[SerializeField] private KeyCode inspectKey = KeyCode.Y;
	[SerializeField] private KeyCode slidingKey = KeyCode.LeftShift;
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

	public bool isDead = false;
	[SerializeField] private GameObject dieMessage;

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
		if(dieMessage != null)
		{
			dieMessage.SetActive(isDead);
		}
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
		float speedTime = 70;

		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		bool isChouch = false;

		if (horizontal != 0 || vertical != 0)
		{
			dir = (vertical * transform.forward) + (horizontal * transform.right);
			if (Input.GetKeyDown(slidingKey) && character.isGrounded)
			{
				if (Mathf.Abs(dir.x) > 1 || Mathf.Abs(dir.z) > 1)
				{
					dir.Normalize();
				}
				movement.Sliding(dir);
			}
			else
			{
				currentSpeed += Time.deltaTime * speedTime;
				if(currentSpeed > status.moveSpeed)
					currentSpeed = status.moveSpeed;
			}
		}
		else
		{
			dir = Vector3.zero;
			currentSpeed = 0;
		}
		if (Input.GetKey(crouchKey) && character.isGrounded && !movement.isSliding)
		{
			isChouch = true;
			currentSpeed += Time.deltaTime * speedTime;
			if (currentSpeed > status.moveSpeed * 0.6f)
				currentSpeed = status.moveSpeed * 0.6f;
		}
		if (Input.GetKeyDown(jumpKey))
		{
			if (movement.isSliding)
			{
				movement.StopSliding(dir);
				movement.Jump(status.jumpScale);
			}
			else if (character.isGrounded)
			{
				movement.Jump(status.jumpScale);
			}
		}

		if(Mathf.Abs(dir.x) > 1 || Mathf.Abs(dir.z) > 1)
		{
			dir.Normalize();
		}

		if (!gun.aiming && !movement.isSliding)
		{
			camera.ZoomCamera(ZoomCam.Nomal);
		}

		if (!movement.isSliding)
		{
			movement.Crouch(isChouch);
		}
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
            gun.transform.position = new Vector3(0f, -0.23f, 0);
        }
		else if (Input.GetKeyUp(aimKey))
		{
			gun.Aiming(false);
            gun.transform.position = new Vector3(0.35f, -0.23f, 0);
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
		if (dieMessage != null)
		{
			dieMessage.SetActive(isDead);
		}
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
