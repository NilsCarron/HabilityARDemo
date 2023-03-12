using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IntroUpDisplay : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private TextMeshProUGUI _textToDisplay;

    private int _displayCount;

    void Start()
    {
        _displayCount = 0;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {       
        switch (_displayCount)
        {
            case 0:
                _textToDisplay.text = "Scan the logo of the company for which the monkey will have to code first! <br><br> Then, touch the screen to control your monkey, <br> double tap to make him jump! <br><br> Hurry up, the recruiter is waiting!<br><br> Tap to play";
                _displayCount += 1;
                break;
            case 1:
                SceneManager.LoadScene(1);
                
                break;
            default:
                break;
        }



    }
}
