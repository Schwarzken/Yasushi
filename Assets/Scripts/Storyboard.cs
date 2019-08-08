using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storyboard : MonoBehaviour {

    public GameObject instructions;
    LoadSceneOnClick loadScene;

    [System.Serializable]
    public class Storyboards
    {
        public GameObject storyboard;
    }

    public Storyboards[] boards;
    int index = 0;

    void Start()
    {
        loadScene = GetComponent<LoadSceneOnClick>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(index < boards.Length-1)
            {
                index++;
                boards[index].storyboard.gameObject.SetActive(true);
                boards[index-1].storyboard.gameObject.SetActive(false);
            }
            else
            {
                loadScene.LoadByIndex(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index > 0)
            {
                index--;
                boards[index+1].storyboard.gameObject.SetActive(false);
                boards[index].storyboard.gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            loadScene.LoadByIndex(1);
        }
    }

    public void SetButtonsActive()
    {
        instructions.gameObject.SetActive(true);
    }
}
