using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerActive : MonoBehaviour
{
    [SerializeField] private Transform head;
    private Camera camera;
    private CharacterController character;

    private Vector3 dir;


	private float currentSpeed;
    [SerializeField] private float walkSpeed = 5;
	[SerializeField] private float runSpeed = 8;

    [SerializeField] private float limitAngle = 89;
    [SerializeField] private float sensitivity = 1f;

    private float bangAim;

    private float gravity = 0;
    [SerializeField] private float gravityScale = -9.8f;
    [SerializeField] private float jumpPower = 15f;

    private float rotateX;
    private float rotateY;

	private float fwd = 0;
	private float rit = 0;
    private float groundCount;

	private void Awake()
    {
        camera = Camera.main;
        character = GetComponent<CharacterController>();
    }

    public void Move()
    {
        //움직이는 방향(버니합 만들려고 김)
		if (character.isGrounded)
        {
            groundCount += Time.deltaTime;
            if((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && groundCount > 0.05f)
            {
                fwd = Input.GetAxisRaw("Vertical");
                rit = Input.GetAxisRaw("Horizontal");
			}
            else if(groundCount > 0.05f)
            {
				fwd = Mathf.Lerp(fwd, 0, Time.deltaTime * 25);
				rit = Mathf.Lerp(rit, 0, Time.deltaTime * 25);
			}
		}
        else
        {
            groundCount = 0;
			fwd += Input.GetAxisRaw("Vertical") * Time.deltaTime * 2;
			rit += Input.GetAxisRaw("Horizontal") * Time.deltaTime * 1;
            if (Mathf.Sign(rit) != Input.GetAxisRaw("Horizontal"))
                rit = 0;
			rit = Mathf.Clamp(rit, -0.75f, 0.75f);
		}
        fwd = Mathf.Clamp(fwd, -1, 1);
		rit = Mathf.Clamp(rit, -1, 1);
		dir = transform.forward * fwd + transform.right * rit;
		//달리기 감지
		currentSpeed = IsRun();
        //움직임
        character.Move(dir * Time.deltaTime * currentSpeed + (Vector3.up * gravity));
    }

    private float IsRun()
    {
        bool isRun = Input.GetKey(KeyCode.LeftShift);
        //camera.fieldOfView = isRun ? 75f : 65f;
        return isRun ? runSpeed : walkSpeed;
	}

    public void FirstPerson()
    {
        RotateHead();
		RotateBody();
	}

	private void RotateHead()
    {
		float mouseY = Input.GetAxis("Mouse Y");

        rotateX -= mouseY * sensitivity;
		rotateX = Mathf.Clamp(rotateX, -limitAngle, limitAngle);

		head.transform.localRotation = Quaternion.Euler(rotateX + bangAim, 0, 0);
	}

   private void RotateBody()
    {
		float mouseX = Input.GetAxis("Mouse X");

		rotateY += mouseX * sensitivity;

		transform.localRotation = Quaternion.Euler(0, rotateY, 0);
	}

    public void Gravity()
    {
		if (!character.isGrounded)
		{
			gravity += gravityScale * Time.deltaTime;
		}
	}

    public void Jump()
    {
        gravity = jumpPower * 0.01f;
    }

    public void Crouch()
    {
        bool isCrouch = Input.GetKey(KeyCode.LeftControl);

		float height = isCrouch ? 1f : 2f;
        float center = isCrouch ? -0.5f : 0f;
        float headPos = isCrouch ? 0.2f : 0.6f;

        head.localPosition = Vector3.Lerp(head.localPosition, Vector3.up * headPos, Time.deltaTime * 10f);
		character.center = Vector3.Lerp(character.center, Vector3.up * center, Time.deltaTime * 10f);
		character.height = Mathf.Lerp(character.height, height, Time.deltaTime * 10f);
	}

    public void Bang()
    {
        StartCoroutine("BangAim");
    }

    private IEnumerator BangAim()
    {
		bangAim = -8f;
		while (Mathf.Abs(bangAim) > 0.05f)
        {
			bangAim = Mathf.Lerp(bangAim, 0, Time.deltaTime * 7);
			yield return null;
        }
        bangAim = 0;
    }
}
