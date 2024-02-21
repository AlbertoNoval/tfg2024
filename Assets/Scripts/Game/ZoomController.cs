using Photon.Pun;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    private float initialDistance;
    private float zoomSpeed = 0.5f;
    private float minZoom = 30;
    private float maxZoom = 70;
    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    void Update()
    {
        if(view.IsMine){
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved)
                {
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    Zoom(deltaMagnitudeDiff * zoomSpeed);
                }
            }
        }
    }

    void Zoom(float deltaMagnitudeDiff)
    {
        Camera.main.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);
    }
}
