using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
    }
}
