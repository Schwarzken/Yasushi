using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour {

	public static GameInputManager GIM;

	public KeyCode jump {get; set;}
	public KeyCode left {get; set;}
	public KeyCode right{ get; set;}
	public KeyCode attack {get; set;}
	public KeyCode consume {get;set;}
	public KeyCode charge { get; set;}
	public KeyCode dodge {get;set;}
	public KeyCode tornado {get;set;}


	void Awake()
	{
		if (GIM == null) {
			//DontDestroyOnLoad (gameObject);
			GIM = this;
		}
		else if(GIM != this){
			Destroy(gameObject);
		}

		jump = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("jumpKey", "Space"));
		left = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("leftKey", "LeftArrow"));
		right = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("rightKey", "RightArrow"));
		attack = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("attackKey", "R"));
		consume = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("consumeKey", "T"));
		charge = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("chargeKey", "Q"));
		dodge = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("dodgeKey", "W"));
		tornado = (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("tornadoKey", "E"));
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
