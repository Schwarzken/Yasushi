using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextTyper : MonoBehaviour
{
    public float letterPause = 0.02f;
    public Text dialog;
    string message;
    public bool messageFinished = true;
    //string finishedMessage;
    //public bool typing;

    // Use this for initialization
    public void PlayDialog(string content)
    {
       // finishedMessage = content;
        dialog.text = "";
        message = "";
        message = content;
        StartCoroutine(TypeText());
    }

    public void FinishDialog(string content)
    {
        dialog.text = "";
        dialog.text = content;
        print(dialog.text);
        messageFinished = true;
    }
    
    public void StopTypeTextCoroutine()
    {
        StopCoroutine(TypeText());

    }


    /*IEnumerator FinishText()
    {
        dialog.text = "";
        dialog.text = message;
        messageFinished = true;
        yield return new WaitForSeconds(0.01f);
    }*/

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            messageFinished = false;
            dialog.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
        messageFinished = true;
    }
}