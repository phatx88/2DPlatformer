using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] int speed;

    [Range(1, 10)]
    [SerializeField] float acceleration; 

    float speedMultiplier;

    bool btnPressed;

    bool isWallTouch;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    Vector2 relativeTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateRelativeTransform();
    }

    private void FixedUpdate() //Fixed at 60FPS regardless of actual performance
    {
        UpdateSpeedMultiplier();
        
        float targetSpeed = speed * speedMultiplier*relativeTransform.x;

        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

        isWallTouch = Physics2D.OverlapBox(wallCheckPoint.position, new Vector2(0.1f, 1.5f), 0, wallLayer);

        if (isWallTouch)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        UpdateRelativeTransform();
    }

    public void UpdateRelativeTransform()
    {
        relativeTransform = transform.InverseTransformVector(Vector2.one);
    }

    public void Move(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            btnPressed = true;
        }
        else if (value.canceled)
        {
            btnPressed = false;
        }
    }

    void UpdateSpeedMultiplier()
    {
        if(btnPressed && speedMultiplier < 1)
        {
            speedMultiplier += Time.deltaTime*acceleration;
        }
        else if(!btnPressed && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime*acceleration;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }
}
