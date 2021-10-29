using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{     
    public GameObject loadPanel,mainmenuPanel;

    private void OnEnable()
    {
        DisableLoadPanel();
    }
    public void ToGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void Exit()
    {
        Application.Quit();
    }
   
    public void DisableLoadPanel()
    {
        if (Admanager.Instance.appOpenAdFinished)
        {
            loadPanel.SetActive(false);
            mainmenuPanel.SetActive(true);
        }       
    }      
  
}