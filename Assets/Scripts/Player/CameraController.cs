using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoomCam
{
    Nomal,
    Zoom,
    Dash
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private float nomalSize;
	[SerializeField] private float zoomSize;
    [SerializeField] private float dashSize;

    [SerializeField] private float recoil = 3;

	private Camera camera;
    private Camera gunCamera;
    private float currentZoomSize;
    private float targetZoomSize;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        gunCamera = transform.GetChild(0).GetComponent<Camera>();
        targetZoomSize = nomalSize;
    }

    private void Update()
    {
		currentZoomSize = Mathf.Lerp(currentZoomSize, targetZoomSize, Time.deltaTime * 10);
        camera.fieldOfView = currentZoomSize;
		gunCamera.fieldOfView = currentZoomSize;
	}

    public void FireCamera()
    {
		StopCoroutine("Fire");
		StartCoroutine("Fire");
    }

    public void ZoomCamera(ZoomCam zoom)
    {
        if(zoom == ZoomCam.Nomal)
        {
            targetZoomSize = nomalSize;
        }
		else if (zoom == ZoomCam.Zoom)
		{
            targetZoomSize = zoomSize;
		}
		else if (zoom == ZoomCam.Dash)
		{
            targetZoomSize = dashSize;
		}
	}

    private IEnumerator Fire()
    {
        float up;

		up = SetFireCam(0);
		while (up > -recoil)
        {
            up = SetFireCam(up - Time.deltaTime * (recoil * 35));
			yield return null;
        }
		up = SetFireCam(-recoil);
		yield return new WaitForSeconds(0.05f);
		while (up < 0)
		{
			up = SetFireCam(up + Time.deltaTime * (recoil * 10));
			yield return null;
		}
		up = SetFireCam(0);
	}

    private float SetFireCam(float up)
    {
		camera.transform.localRotation = Quaternion.Euler(up, 0, 0);
        return up;
	}
}
