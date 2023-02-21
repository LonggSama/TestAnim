using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] float _speed;
    [SerializeField] float _walkAcceleration;
    [SerializeField] float _groundDeceleration;
    [SerializeField] float _airAcceleration;
    [SerializeField] float _jumpHeight;

    private BoxCollider2D _playerCollider;

    private Animator _playerAC;

    private Vector2 velocity;

    private bool _facingRight = true;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool _isGround;

    void Awake()
    {
        _playerCollider = GetComponent<BoxCollider2D>();
        _playerAC = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //_isGround = false;
        //CheckGround();
        PlayerJump();
        PlayerMove();
        transform.Translate(velocity * Time.deltaTime);
        velocity.y += Physics2D.gravity.y * Time.deltaTime;
    }

    //void CheckGround()
    //{
    //    Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _playerCollider.size, 0);

    //    foreach (Collider2D hit in hits)
    //    {
    //        if (hit == _playerCollider)
    //            continue;

    //        ColliderDistance2D colliderDistance = hit.Distance(_playerCollider);

    //        if (colliderDistance.isOverlapped)
    //        {
    //            transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

    //            if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
    //            {
    //                _isGround = true;
    //            }  
    //        }
    //    }
    //}

    private bool IsGrounded()
    {
        float extraHeightText = 1f;

        RaycastHit2D raycastHit = Physics2D.Raycast(_playerCollider.bounds.center, Vector2.down, _playerCollider.bounds.extents.y + extraHeightText, platformLayerMask);

        Color rayColor;

        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
            rayColor = Color.red;

        Debug.DrawRay(_playerCollider.bounds.center, Vector2.down * (_playerCollider.bounds.extents.y + extraHeightText), rayColor);

        Debug.Log(raycastHit.collider);

        return raycastHit.collider != null;
    }

    void PlayerMove()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        float acceleration = IsGrounded() ? _walkAcceleration : _airAcceleration;

        float deceleration = IsGrounded() ? _groundDeceleration : 0;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, _speed * moveInput, acceleration * Time.deltaTime);

            _playerAC.SetFloat("Speed", 0.1f);

            if (moveInput > 0 && !_facingRight)
            {
                Flip();
            }

            if (moveInput < 0 && _facingRight)
            {
                Flip();
            }
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            _playerAC.SetFloat("Speed", 0f);
        }
    }

    void PlayerJump()
    {
        if (IsGrounded())
        {
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;

        currentScale.x *= -1;

        gameObject.transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }
}
