using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Collider _playerDetectionCollider;
        [SerializeField]
        private Rigidbody _playerRigidBody;

        [SerializeField] private GameObject _playerSpawner;
        [SerializeField] private Animator _playerAnimator;
        private Transform _playerTransform;
        
        private bool _isGrounded;
        private const float MaxSpeed = 0.01f;


        // Start is called before the first frame update
        void Start()
        {
            _playerTransform = this.transform;
        }

          
        /// <summary>
        /// Apply a force to the character to move it
        /// </summary>
        /// <param name="movement">
        /// The vector3 is translation we want to apply to the player
        /// </param>
        public void MoveCharacter(Vector3 movement)
        {

            //Clamping the movement using the MaxSpeed, so the character doesn't run too fast
            movement.x = Mathf.Clamp(movement.x, -MaxSpeed, MaxSpeed);
            movement.y = Mathf.Clamp(movement.y, -MaxSpeed, MaxSpeed);
            movement.z = Mathf.Clamp(movement.z, -MaxSpeed, MaxSpeed);
            //Check if the player is in the airs
            if (!_isGrounded)
                //Double the speed in the airs so the jmp feels more natural
                _playerTransform.Translate(movement*2);
        
            else
            {
                _playerTransform.Translate(movement);
            }

        }
    
    
        /// <summary>
        ///Make the model jump if the player is on the ground
        /// </summary>
        public void JumpCharacter()
        {
            if (_isGrounded)
            {
                //Adding a very small force to the model in the "up" direction
                //We use a rigidbody here so the jump feels more natural
                _playerRigidBody.AddForce(_playerTransform.transform.up * 0.0000002f );
                //Once again here, we clamp the force added to the character so it doesn't stack force
                _playerRigidBody.velocity = Vector3.ClampMagnitude(_playerRigidBody.velocity, 00.1f);
                _playerAnimator.SetBool("IsJumping", true);
            }   
        }

        /// <summary>
        /// Called when the player is walking on the ground
        /// </summary>
        /// <param name="otherCollider">
        /// The collider on witch the player is walking
        /// </param>
        private void OnTriggerStay(Collider otherCollider)
        {
            // We use here a OnTriggerStay, because the player may exit a ground and go on another
            //so if the _isGrounded is not correctly set at that moment, it is at the next frame


            switch (otherCollider.tag)
            {
                case "Ground" :
                    _isGrounded = true;
                    _playerAnimator.SetBool("IsJumping", false);
                    break;
                case "Objective":
                    GameManager.Instance.AddScore();
                    Destroy(otherCollider.gameObject);
                    break;
                case "DeathPlane" :
                    _playerTransform.position = _playerSpawner.transform.position;
                    Debug.Log("Dead");
                    break;
                default:
                    break;
            }
        
        }

        /// <summary>
        /// Called when the player is leaving on the ground
        /// </summary>
        /// <param name="otherCollider">
        /// The collider on witch the player was walking
        /// </param>
        private void OnTriggerExit(Collider otherCollider)
        {
            if (otherCollider.tag == "Ground")
            {
                _isGrounded = false;
            }
        }
    
    }
}
