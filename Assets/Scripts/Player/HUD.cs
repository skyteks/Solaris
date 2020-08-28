using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Rigidbody shipRigid;
    public float maxSpeed = 1000f;

    [Space]

    public Image speedGauge;
    public Text speedText;

    public RectTransform directionCursor;

    void Update()
    {
        float speed = shipRigid.velocity.magnitude;
        speedGauge.fillAmount = speed.LinearRemap(0f, maxSpeed);
        speedText.text = string.Concat(Mathf.RoundToInt(speed), " m/s", "\nFA: ", shipRigid.drag == 0f ? "Off" : "On");

        Vector3 screenMiddle = new Vector3(Screen.width, Screen.height) * 0.5f;
        float lenght = Vector3.Distance(Input.mousePosition, screenMiddle);
        float angle = Vector3.SignedAngle(Vector3.right, (Input.mousePosition - screenMiddle).normalized, Vector3.forward);

        directionCursor.sizeDelta = new Vector2(lenght, directionCursor.sizeDelta.y);
        directionCursor.rotation = Quaternion.Euler(Vector3.forward * (angle));
    }
}
