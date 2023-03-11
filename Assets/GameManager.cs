using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   private static GameManager _instance;

    private float _timePlayed;

    [SerializeField]  private TextMeshProUGUI _textMeshPro;
    [SerializeField] private TextMeshProUGUI _Ending;
    bool end;

    private static int _score;
   public static GameManager Instance
   {
      get
      {
         if(_instance == null)
            Debug.LogError("Null");
         return _instance;
      }
   }

    public void EndOfTHeGame()
    {
        end = true;
        _Ending.text = "You finished the level in " + _timePlayed + " Seconds! The High Score is " ;
        
        if((PlayerPrefs.GetFloat("highScore") < _timePlayed) && PlayerPrefs.GetFloat("highScore") != 0f)
        {
            _Ending.text = "You finished the level in " + _timePlayed + "Seconds! The high score is still at " + PlayerPrefs.GetFloat("highScore");

        }
        else
        {
            _Ending.text = "You finished the level in " + _timePlayed + " Seconds! This is the new high score, congratulation!";
            PlayerPrefs.SetFloat("highScore", _timePlayed);

        }
        PlayerPrefs.Save();
        StartCoroutine(LoadEndOfGame());


    }

    IEnumerator LoadEndOfGame()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if(!end)
            _timePlayed += Time.deltaTime;


    }
    private void Awake()
   {
      _instance = this;
        end = false;
   }
}
