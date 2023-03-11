using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Collider _playerDetectionCollider;
    private Transform _playerTransform;
    [SerializeField]
    private Rigidbody _playerRigidBody;
    private bool _isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = this.transform;
    }

    public void MoveCharacter(Vector3 movement)
    {
        _playerTransform.Translate(movement);

    }

    public void JumpCharacter()
    {
        if (_isGrounded)
        {
            _playerRigidBody.AddForce(_playerTransform.transform.up * 0.0000002f);
        }
    }


    private void OnTriggerStay(Collider otherCollider)
    {
        if (otherCollider.tag == "Ground")
        {
            _isGrounded = true;
        }
        
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.tag == "Ground")
        {
            _isGrounded = false;
        }
    }
    
}
