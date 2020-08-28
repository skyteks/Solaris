using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    private Rigidbody rigid;

    public float forwardSpeed = 25f, strafeSpeed = 5f, hoverSpeed = 7.5f;

    public float lookRotateSpeed = 90f;
    private Vector2 lookInput, mouseDistance;
    private Vector2 screenCenter { get { return new Vector2(Screen.width, Screen.height) * 0.5f; } }

    public float rollSpeed = 90f, rollAcceleration = 3.5f;
    private float rollInput;

    public float drag = 1f;
    private bool dampener = true;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            dampener = !dampener;
        }
        rigid.drag = dampener ? drag : 0f;

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.x;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRotateSpeed * Time.deltaTime, mouseDistance.x * lookRotateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        float activeForwardSpeed = Input.GetAxisRaw("Vertical") * forwardSpeed;
        float activeStrafeSpeed = Input.GetAxisRaw("Horizontal") * strafeSpeed;
        float activeHoverSpeed = Input.GetAxisRaw("Hover") * hoverSpeed;

        rigid.AddRelativeForce(Vector3.forward * activeForwardSpeed * Time.deltaTime * 1000f, ForceMode.Force);
        rigid.AddRelativeForce(Vector3.right * activeStrafeSpeed * Time.deltaTime * 1000f, ForceMode.Force);
        rigid.AddRelativeForce(Vector3.up * activeHoverSpeed * Time.deltaTime * 1000f, ForceMode.Force);
    }
}
