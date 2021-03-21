using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : SingletonMonobehaviour<GameManager>
{

    public TextMeshProUGUI text;
    private int starsGathered;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementStarsGathered()
    {
        starsGathered++;
        SetStarsText();
    }

    private void SetStarsText()
    {
        text.text = "Stars: " + starsGathered;

    }

    
}
