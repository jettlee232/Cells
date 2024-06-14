using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody의 회전을 고정합니다.
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.velocity = movement * speed;

        if (Input.GetKey(KeyCode.Q))
        {
            rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        }
    }
}
