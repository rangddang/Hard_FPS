using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GunController gun;
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

}
