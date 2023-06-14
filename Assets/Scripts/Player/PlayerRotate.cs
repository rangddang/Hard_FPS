using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRotate : MonoBehaviour
{
	private Transform head;
	private float rotateX;
	private float rotateY;

	[SerializeField] private float limitAngle = 89;
	[SerializeField] private float sensitivity = 1f;

	private void Awake()
	{
		head = transform.GetChild(0);
	}

	public void RotateHead(float mouseY)
	{
		rotateX -= mouseY * sensitivity;
		rotateX = Mathf.Clamp(rotateX, -limitAngle, limitAngle);

		head.transform.localRotation = Quaternion.Euler(rotateX, 0, 0);
	}

	public void RotateBody(float mouseX)
	{
		rotateY += mouseX * sensitivity;

		transform.localRotation = Quaternion.Euler(0, rotateY, 0);
	}
}
