using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
	[SerializeField]
	private float walkSpeed;
	[SerializeField]
	private float runSpeed;
	[SerializeField]
	private float crouchSpeed;
	[SerializeField]
	private float jumpScale;

	public float WalkSpeed => walkSpeed;
	public float RunSpeed => runSpeed;
	public float CrouchSpeed => crouchSpeed;
	public float JumpScale => jumpScale;
}
