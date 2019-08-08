using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindController : MonoBehaviour {

    public Sounds sounds;

	[HideInInspector]public KeyCode curAttackKey;
	[HideInInspector]public KeyCode curConsumeKey;

	void Start()
	{
		curAttackKey = KeyCode.R;
		curConsumeKey = KeyCode.T;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
            sounds.PlayYOO();
			curAttackKey = KeyCode.R;
			curConsumeKey = KeyCode.T;
			print ("Attack = R");
			print ("Consume = T");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
            sounds.PlayYOO();
            curAttackKey = KeyCode.A;
			curConsumeKey = KeyCode.S;
			print ("Attack = A");
			print ("Consume = S");
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
            sounds.PlayYOO();
            curAttackKey = KeyCode.F;
			curConsumeKey = KeyCode.R;
			print ("Attack = F");
			print ("Consume = R");
		}
	}
}
