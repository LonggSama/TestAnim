using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    [SerializeField] float _jumpForce;

    float _horizontalMove;

    public CharacterController Controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Controller.Move(_horizontalMove, false, false);
    }

}
