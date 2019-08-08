using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;

    float shakeAmount = 0;

    void Awake()
    {
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    public void ShakeRight(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShakeRight", 0, 0.01f);
        Invoke("StopShakeRight", length);
    }

    public void ShakeLeft(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShakeLeft", 0, 0.01f);
        Invoke("StopShakeLeft", length);
    }

    void DoShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x = offsetX;
            camPos.y = offsetY;

            mainCam.transform.localPosition = camPos;
        }
    }

    void DoShakeRight()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount;
            float offsetY = 0f;

            camPos.x = offsetX;
            camPos.y = offsetY;

            mainCam.transform.localPosition = camPos;
        }
    }

    void DoShakeLeft()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = -Random.value * shakeAmount;
            float offsetY = 0f;

            camPos.x = offsetX;
            camPos.y = offsetY;

            mainCam.transform.localPosition = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }

    void StopShakeRight()
    {
        CancelInvoke("DoShakeRight");
        mainCam.transform.localPosition = Vector3.zero;
    }

    void StopShakeLeft()
    {
        CancelInvoke("DoShakeLeft");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
