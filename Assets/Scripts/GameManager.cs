using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : SingletonMonobehaviour<GameManager>
{

    public TextMeshProUGUI text;
    private int starsGathered;
    private int totalStars;
    private int count;

    void start(){
        totalStars =  Resources.FindObjectsOfTypeAll(typeof(Star)).Length;
        Debug.Log(totalStars);
          var getCount = GameObject.FindGameObjectsWithTag ("Star");
         count = getCount.Length;
        Debug.Log(count);


    }

 
    public void IncrementStarsGathered()
    {

        starsGathered++;
        SetStarsText();
        if(starsGathered >= 1){
            HandleEndGame();
        }
    }

    private void SetStarsText()
    {
        text.text = "Stars: " + starsGathered;

    }

    private void HandleEndGame(){
Debug.Log("You did it!" + totalStars);
Debug.Log("You did it!" + count);
    }

    
}
