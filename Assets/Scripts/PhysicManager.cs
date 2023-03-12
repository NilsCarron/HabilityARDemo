using UnityEngine;

namespace Assets.Scripts
{
    public class PhysicManager : MonoBehaviour
    {
        [SerializeField] private GameObject _player; //The player's model to get its position
        [SerializeField] private Rigidbody _playerRb; //The rigidbody on which we will apply the gravity

        [SerializeField] private GameObject _deathPlane;//the plane toward which the player will be attracted
        private bool _usingGravity; //If the function must apply gravity (is the plan in sight?)
        private Vector3 _gravityToApply;
        private Vector3 _localGravity;



        // Start is called before the first frame update
        void FixedUpdate()
        {
            _playerRb.rotation = _deathPlane.transform.rotation;
            //Since the image to detect isn't fixed we have to emulate gravity
            if (_usingGravity)
            {
                //Reset the forces on the gravity we will apply
                _gravityToApply = Vector3.zero;
                //We get the attraction the plane exerts on the player as a vector
                _gravityToApply = - ( _deathPlane.transform.up).normalized;
                //We get the player upper direction
                _localGravity = _player.transform.up;
                //The player will rotate to adjust the new gravity, he is facing "up" from the plan
                _player.transform.up = Vector3.Lerp(_localGravity, _gravityToApply, Time.deltaTime);
                //We finally apply the gravity to the player, with a variable to adjust the gravity force
                //Since everything is so small in the Unity simulator, we use a small value
                //we could determine this number with the scale and the weigh of the player model
                _playerRb.AddForce((_gravityToApply * 0.00000001f));
                //Clamping the velocity of the character so it don't accumulate speed
                _playerRb.velocity = Vector3.ClampMagnitude(_playerRb.velocity, 00.1f);


                //Updating the rotation of the player according to the plan's one
                _player.transform.rotation = _deathPlane.transform.rotation;
            

            }

        }

    
        /// <summary>
        /// Stops the gravity and every movements on the player movement
        /// This should be called when the ImageTarget is lost 
        /// </summary>
        public void StopGravity()
        {
            _playerRb.velocity = Vector3.zero;
            _usingGravity = false;
            _playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }
    
        /// <summary>
        /// activates the gravity and unfreeze its rigid body
        /// This should be called when the ImageTarget is in sight 
        /// </summary>
        public void ActivateGravity()
        {
            _usingGravity = true;
            _playerRb.constraints = RigidbodyConstraints.None;
            _playerRb.constraints = RigidbodyConstraints.FreezeRotation;

  
        }

    }
}

