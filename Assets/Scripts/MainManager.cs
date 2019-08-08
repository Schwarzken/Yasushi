using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
    Sounds sounds;
    InitiateDialog initiateDialog;
    TutorialSkip tutorialSkip;
    bool preventRepeat = false;
    bool preventRepeatSounds = false;

    void Start()
    {
        sounds = GetComponent<Sounds>();
        initiateDialog = GetComponent<InitiateDialog>();
        tutorialSkip = GetComponent<TutorialSkip>();
        sounds.PlayBG();
    }

    private void Update()
    {
        if(tutorialSkip.skip == false)
        {
            if(preventRepeat == false)
            {
                initiateDialog.StartDialog();
                preventRepeat = true;
            }
        }

        if(initiateDialog.firstDone == true && initiateDialog.done == true)
        {
            if(preventRepeatSounds == false)
            {
                sounds.PlaySamurai();
                sounds.fading2 = true;
                StartCoroutine(AfterFade(sounds));
                preventRepeatSounds = true;
            }
        }
    }

    public static IEnumerator AfterFade(Sounds bgmM)
    {
        yield return new WaitForSeconds(1.2f);
        print("AfterFade");
        bgmM.fading2 = false;
        bgmM.fadeDone2 = true;
    }
}
