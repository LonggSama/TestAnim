using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _walkAcceleration;
    [SerializeField] float _groundDeceleration;
    [SerializeField] float _airAcceleration;
    [SerializeField] float _jumpHeight;

    private BoxCollider2D _playerCollider;

    private Vector2 velocity;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool _isGround;

    void Awake()
    {
        _playerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGround = false;
        CheckGround();
        PlayerJump();
        PlayerMove();
        transform.Translate(velocity * Time.deltaTime);
        velocity.y += Physics2D.gravity.y * Time.deltaTime;
    }

    void CheckGround()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _playerCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit == _playerCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(_playerCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    _isGround = true;
                }  
            }
        }
    }

    void PlayerMove()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        float acceleration = _isGround ? _walkAcceleration : _airAcceleration;

        float deceleration = _isGround ? _groundDeceleration : 0;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, _speed * moveInput, acceleration * Time.deltaTime);
        }
        else
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
    }

    void PlayerJump()
    {
        if (_isGround)
        {
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }
    }
}
