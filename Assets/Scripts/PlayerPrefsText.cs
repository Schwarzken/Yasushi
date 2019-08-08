using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPrefsText : MonoBehaviour
{

    public string Name;

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt(Name) + "";
    }
}
