using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public float ZoomSpeed = 5;
    public float PinchSensitivity = 0.1f;
    public float MoveSpeed = 5;
    public float LerpTime = 0.1f;
    public bool invertZoom = true;

    private float TargetZoom;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        TargetZoom = cam.orthographicSize;
    }

    bool panning = false;
    Vector3 lastPosition;
    private void Update()
    {
        float Horz = Input.GetAxis("Horizontal");
        float Vert = Input.GetAxis("Vertical");

        PerformZoomOperations();
        
        Vector2 moveVector = new Vector2(Horz, Vert) * MoveSpeed * GetCamSizeSpeedMultiplier();
        transform.position += new Vector3(moveVector.x, moveVector.y) * Time.deltaTime;

        bool startPanning = Input.GetMouseButtonDown(0);
        if (startPanning)
        {
            panning = true;
            lastPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            panning = false;
        }

        if (panning)
        {
            Vector3 nPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = nPos - lastPosition;
            delta *= -1;
            transform.Translate(delta.x, delta.y, 0);
        }
    }

    private void PerformZoomOperations() {
        float Zoom = Input.GetAxis("Zoom") * ZoomSpeed;

        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.orthographicSize += deltaMagnitudeDiff * PinchSensitivity;

        }

        #if UNITY_STANDALONE
        Zoom *= (invertZoom ? -1 : 1);
        TargetZoom += Zoom * Time.deltaTime;
        
        TargetZoom = Mathf.Clamp(TargetZoom, 1, 110);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, TargetZoom, LerpTime);
        #endif

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 110);
        
    }

    private float GetCamSizeSpeedMultiplier()
    {
        return cam.orthographicSize / 2;
    }
}