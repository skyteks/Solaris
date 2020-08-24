using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform followTransform;
    public Transform cameraTransform;

    protected Vector3 localRot;
    protected float camDistance;

    public float mouseSensitivity = 4f;
    public float scrollSensitvity = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;

    public bool cameraLocked = true;


    // Use this for initialization
    void Start()
    {
        camDistance = Vector3.Distance(cameraTransform.position, followTransform.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            cameraLocked = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            cameraLocked = true;
        }
        //Cursor.visible = cameraLocked;
        Cursor.lockState = cameraLocked ? CursorLockMode.None : CursorLockMode.Locked;
    }


    void LateUpdate()
    {
        if (!cameraLocked)
        {
            //Rotation of the Camera based on Mouse Coordinates
            float deadzone = 0.001f;
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > deadzone || Mathf.Abs(Input.GetAxis("Mouse Y")) > deadzone)
            {
                localRot.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                localRot.y += -Input.GetAxis("Mouse Y") * mouseSensitivity;

                //Clamp the y rotation to not flipping over at the top or bottom
                localRot.y = Mathf.Clamp(localRot.y, -90f, 90f);
            }
            //Zooming Input from our Mouse Scroll Wheel
            /*
            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel") > deadzone)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                //Makes camera zoom faster the further it is from the target
                ScrollAmount *= (_CameraDistance * 0.3f);

                _CameraDistance += -ScrollAmount;

                _CameraDistance = Mathf.Clamp(_CameraDistance, 1.5f, 100f);
            }
            */

            //Actual Camera Rig Transformations
            Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0f);
            followTransform.localRotation = Quaternion.Lerp(followTransform.localRotation, QT, Time.deltaTime * orbitDampening);

            if (cameraTransform.localPosition.z != -camDistance)
            {
                cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(cameraTransform.localPosition.z, -camDistance, Time.deltaTime * scrollDampening));
            }
        }
    }
}