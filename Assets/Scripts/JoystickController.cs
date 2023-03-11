using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        public GameObject JoystickBack;
        public GameObject Joystick;
        [SerializeField] private GameObject _player;

        private bool _doubleTap;
        

        private RectTransform _transformBack;
        private RectTransform _transformJoystick;
        private float _radius;
    
        private Vector3 _vectorMove;
        private bool _isTouching = false;
    
        // Start is called before the first frame update
        void Start()
        {
            _doubleTap = false;
            _transformBack=JoystickBack.GetComponent<RectTransform>();
            _transformJoystick = Joystick.GetComponent<RectTransform>();
            _radius = _transformBack.rect.width * 0.5f;
        

        }
        IEnumerator ResetJumpCountDown()
        {
            yield return new WaitForSeconds(0.25f);
            _doubleTap = false;
        }
        void FixedUpdate()
        {
            if (_isTouching)
            {
                _player.GetComponent<PlayerController>().MoveCharacter(_vectorMove*0.1f  * Time.deltaTime);

            }

        }

        void OnTouch(Vector2 vecTouch)
        {
            if (_doubleTap)
            {
                _player.GetComponent<PlayerController>().JumpCharacter();
            }
            else
            {
                _doubleTap = true;
                StartCoroutine(ResetJumpCountDown());
            }
            Vector2 vec = new Vector2(vecTouch.x - _transformBack.position.x, vecTouch.y - _transformBack.position.y);

        
            vec = Vector2.ClampMagnitude(vec, _radius);
            _transformJoystick.localPosition = vec;

            float fSqr = (_transformBack.position - _transformJoystick.position).sqrMagnitude / (_radius * _radius);
        
            Vector2 vecNormal = vec;

            _vectorMove = new Vector3(vecNormal.x *  Time.deltaTime * fSqr, 0f,
                vecNormal.y *  Time.deltaTime * fSqr);
            


            
        }
        
        
        
 
        
           
 
        

        public void OnDrag(PointerEventData eventData)
        {
            OnTouch(eventData.position);
            _isTouching = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            JoystickBack.transform.position = eventData.position;
            
            
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