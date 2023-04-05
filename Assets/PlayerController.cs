using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 1f;

    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private int count;

    private bool grounded = false;
    private bool doubleJumped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();

        winTextObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("game quit.");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                grounded = false;
                Jump();
            }
            else if (!doubleJumped)
            {
                doubleJumped = true;
                Jump();
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpHeight);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            count = count + 1;

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("environment"))
        {
            grounded = true;
            doubleJumped = false;
        }
    }

    private void SetCountText()
    {
        countText.text = (12 - count).ToString() + " gems remaining";

        if (count >= 12)
        {
            winTextObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
