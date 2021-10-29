using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGrounds : MonoBehaviour
{

   public List<GameObject> backGround = new List<GameObject>();
    public static bool backGroundSelected;
    int index;
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            backGround.Add(transform.GetChild(i).gameObject);
            backGround[i].SetActive(false);
        }
        if (backGroundSelected == false)
        {
            index= Random.Range(0, transform.childCount - 1);
            PlayerPrefs.SetInt("backGroundIndex", index);
            backGroundSelected = true;
        }
        else
        {
            index = PlayerPrefs.GetInt("backGroundIndex");
        }
        Debug.Log(index);
        backGround[index].SetActive(true);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
