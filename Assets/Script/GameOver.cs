using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI score, highScore;

    private void OnEnable()
    {
        Admanager.Instance.DestroyBanner();
    }
    void Start()
    {
        score.text = TheStackver2.Score.ToString();
       
        if (TheStackver2.Score > PlayerPrefs.GetInt("Highscore") || PlayerPrefs.GetInt("Highscore") == 0)
        {
            PlayerPrefs.SetInt("Highscore", TheStackver2.Score);
        }
        highScore.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    
    
    public void Menu()
    {       
        Admanager.Instance.ShowInterstitial();
        SceneManager.LoadScene("Menu");        
    }
    public void Replay()
    {
        TheStackver2.Score = 0;
        Admanager.Instance.ShowInterstitial();
        SceneManager.LoadScene("GamePlay");
    }
}
