using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{

    [SerializeField]
    private float margin = 0, lerpSpeed = 10;

    private Camera cam;

    private OrthographicCameraFitter fitter;

    private Vector3 targetPosition;
    private float targetSize;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        fitter = new OrthographicCameraFitter(cam);
        targetPosition = transform.position;
        targetSize = cam.orthographicSize;
        StartCoroutine(CameraZoomRoutine());
    }

    public void Focus(Collider2D coll)
    {
        this.Focus(coll.bounds);
    }

    public void Focus(Bounds bounds)
    {
        fitter.SetObject(bounds);
        targetSize = fitter.CalculateCameraSize();
        targetPosition = fitter.CalculateCameraPosition().ToVector3(transform.position.z);
    }

    private IEnumerator CameraZoomRoutine()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize + margin, lerpSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

}