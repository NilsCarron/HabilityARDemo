using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        
        [SerializeField]
        private GameObject _joystickBack, _joystick, _player, _playerModel;

        [SerializeField] private Camera _cameraAr;
        // Both joysticks are Ui 
        // player is the character on which we'll apply the movement, while the model is purely for display
        
        //Declaring variables for the joystick inputs
        private RectTransform _transformBack;
        private RectTransform _transformJoystick;
        private float _radius;
    
        //Declaring variable for the controls
        private Vector3 _vectorMove, _forward, _right;
        private bool _isTouching;
        //DoubleTap is true if the player *might* be double-taping
        private bool _doubleTap;

        // Start is called before the first frame update
        //Initializing all values
        void Start()
        {
            _doubleTap = false;
            _transformBack=_joystickBack.GetComponent<RectTransform>();
            _transformJoystick = _joystick.GetComponent<RectTransform>();
            _radius = _transformBack.rect.width * 0.5f;
            
        }
        
          
        /// <summary>
        /// Resets the countdown of the double taping
        /// This is used as a Quaroutine
        /// the player must tap 2 times in less than 0.25 s to jump 
        /// </summary>
        IEnumerator ResetJumpCountDown()
        {
            yield return new WaitForSeconds(0.25f);
            _doubleTap = false;
        }
        
        void FixedUpdate()
        {
            if (_isTouching)
            {
                
     
                
                _player.GetComponent<PlayerController>().MoveCharacter(_vectorMove*0.05f  * Time.deltaTime);

            }
            
            //Updating the rotation of the model, it is facing the direction the joystick is moving toward
            if(_vectorMove != Vector3.zero)
                _playerModel.transform.localRotation = Quaternion.LookRotation(_vectorMove);


        }

          
        /// <summary>
        /// This function is called, as an Update when the player touches the screen
        /// </summary>
        /// <param name="vecTouch">
        /// The vector2 is the position were the finger is on the screen
        /// </param>
        void OnTouch(Vector2 vecTouch)
        {
            //Creating a vector between the player's finger and the center of the joystick
            Vector2 vec = new Vector2(vecTouch.x - _transformBack.position.x, vecTouch.y - _transformBack.position.y);
            //Clamping the vector so the player can put his finger far from the joystick without generating to big Vector
            vec = Vector2.ClampMagnitude(vec, _radius);
            //Implicit conversion here, working with Vectors2 since we are on UI
            
            
            
           
            _transformJoystick.localPosition = vec;
            
            //Make the movment relative to the camera angle
            _forward = _cameraAr.transform.forward;
            _right = _cameraAr.transform.right;
            
            
            //Creating the movement vector from the joystick direction, scaling it
            float fSqr = (_transformBack.position - _transformJoystick.position).sqrMagnitude / (_radius * _radius);
            Vector2 vecNormal = vec *( _forward+ _right);
            
           
            
            _vectorMove = new Vector3(-vecNormal.x *  Time.deltaTime * fSqr, 0f,
                vecNormal.y *  Time.deltaTime * fSqr);
            
            //Check if the finger is at the center of the joystick
            //This mean the player just tap on the screen
            if (_vectorMove == Vector3.zero)
            {
                if (_doubleTap)
                {
                    //This is the second tap, the player doubles tap
                    _player.GetComponent<PlayerController>().JumpCharacter();
                    _doubleTap = false;
                }
                else
                {
                    //This is the first tap, the player *might* be trying to double tap
                    _doubleTap = true;
                    StartCoroutine(ResetJumpCountDown());
                }
            }



        }

 
        
  
        //Here are the 3 functions of finger detection
        //OnDrag, which mean the player is sliding across the screen
        //OnPointerDown, which mean the player stated touch the screen
        //OnPointerUp, which mean the player just removed his finger

        public void OnDrag(PointerEventData eventData)
        {
            //Giving the position on which the finger touched
            OnTouch(eventData.position);
            _isTouching = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //creating a new joystick on the point were the player touched the screen
            _joystickBack.transform.position = eventData.position;
            OnTouch(eventData.position);
        
            _isTouching = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _transformJoystick.localPosition = Vector3.zero;
            _isTouching = false;
        }
    }
}