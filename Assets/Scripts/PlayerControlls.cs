using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{

    public float speed = 6;
    public int playerNum = 0;
    public bool supportMouse = true;
    Plane mouseTurnPlane = new Plane(Vector3.up, Vector3.zero);
    [Tooltip("Distance (in world units) above which rotation vector will be considered")]
    public float rotationDeadZone = 0.1f;

    Vector3 mousePosition = Vector3.zero;
    Rigidbody rb;
    Vector3 lastTurn = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Movement()
    {
        Vector3 movement = Vector3.zero;
        float h = Input.GetAxis("Horizontal_" + playerNum.ToString());
        float v = Input.GetAxis("Vertical_" + playerNum.ToString());
        movement.Set(h, 0f, v);

        rb.velocity = movement.normalized * speed;
    }

    private void AxisTurn()
    {
        Vector3 turn = Vector3.zero;
        float h = Input.GetAxis("AimHorizontal_" + playerNum.ToString());
        float v = Input.GetAxis("AimVertical_" + playerNum.ToString());
        turn.Set(h, 0f, v);
        Turning(turn);
    }

    private void MouseTurn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (mouseTurnPlane.Raycast(camRay, out rayDistance))
        {
            Vector3 planePoint = camRay.GetPoint(rayDistance);
            Vector3 lookVector = planePoint - transform.position;
            Turning(lookVector);
        }
    }

    private void Turning(Vector3 lookVector)
    {
        lookVector.y = 0f;
        if (lookVector.magnitude > rotationDeadZone)
        {
            Quaternion newRotation = Quaternion.LookRotation(lookVector);
            transform.rotation = newRotation;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    // Handle rotation in Update since it's able to poll input faster (60Hz+ vs typically <30Hz for physics)
    // which means it's able to interpolate rotation smoothly
    void Update()
    {
        Vector3 mouseScreenMovement = Input.mousePosition - mousePosition;
        mousePosition = Input.mousePosition;

        if (supportMouse && !mouseScreenMovement.Equals(Vector3.zero))
        {
            MouseTurn();
        }
        else
        {
            AxisTurn();
        }
    }

    public int getPlayerNum()
    {
        return playerNum;
    }
}
