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
    //the text on which the end game will be displayed
    [SerializeField]  private TextMeshProUGUI _ending;
    private bool _end;
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

   /// <summary>
   /// When the game is over, to stop the game and display the score
   /// This function will also reset the game after 8 seconds
   /// </summary>
   public void EndOfTHeGame()
    {
        _end = true;
        _ending.text = "You finished the level in " + _timePlayed + " Seconds! The High Score is " ;
        
        //Loading the score of the player in the file PlayerPrefs.GetFloat, checking if this is a new high score
        if((PlayerPrefs.GetFloat("highScore") < _timePlayed) && PlayerPrefs.GetFloat("highScore") != 0f)
        {
            _ending.text = "You finished the level in " + _timePlayed + "Seconds! The high score is still at " + PlayerPrefs.GetFloat("highScore");

        }
        else
        {
            _ending.text = "You finished the level in " + _timePlayed + " Seconds! This is the new high score, congratulation!";
            PlayerPrefs.SetFloat("highScore", _timePlayed);
            //We save the new high score
            PlayerPrefs.Save();

        }
        //Starting the countdown to reset the game
        StartCoroutine(LoadEndOfGame());


    }

    IEnumerator LoadEndOfGame()
    {
        yield return new WaitForSeconds(8);
        //We load the main screen
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        //If the game is over, we don't add score
        if(!_end)
            _timePlayed += Time.deltaTime;
        


    }
    private void Awake()
   {
      _instance = this;
        _end = false;
   }
}
