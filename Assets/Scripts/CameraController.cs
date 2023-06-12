using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoomCam
{
    Nomal,
    Zoom,
    Run
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private float nomalSize;
	[SerializeField] private float zoomSize;
    [SerializeField] private float runSize;


	private Camera camera;
    private float currentZoomSize;
    private float targetZoomSize;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        targetZoomSize = nomalSize;
    }

    private void Update()
    {
		currentZoomSize = Mathf.Lerp(currentZoomSize, targetZoomSize, Time.deltaTime * 5);
        camera.fieldOfView = currentZoomSize;
	}

    public void FireCamera()
    {
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
		else if (zoom == ZoomCam.Run)
		{
            targetZoomSize = runSize;
		}
	}

    private IEnumerator Fire()
    {

        while (true)
        {

            yield return null;
        }
    }
}
