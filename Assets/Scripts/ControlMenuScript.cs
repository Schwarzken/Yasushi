using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMenuScript : MonoBehaviour {

	[SerializeField]Transform bindSetter;
	Event keyEvent;
	Text buttonText;
	KeyCode newKey;

	bool waitingForKey;

	void Start () {
		waitingForKey = false;

		for (int i = 0; i < 8; i++) {
			if (bindSetter.GetChild (i).name == "LeftButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.left.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "RightButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.right.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "JumpButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.jump.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "AttackButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.attack.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "ConsumeButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.consume.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "ChargeButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.charge.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "DodgeButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.dodge.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "TornadoButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.tornado.ToString ();
			}
		}
	}

	void Update () {
		
	}

	void OnGUI()
	{
		keyEvent = Event.current;

		if (keyEvent.isKey && waitingForKey) 
		{
			newKey = keyEvent.keyCode;
			waitingForKey = false;
		}
	}

	public void StartAssignment(string keyName)
	{
		if (!waitingForKey) {
			StartCoroutine (AssignKey (keyName));
		}
	}

	public void SendText (Text text)
	{
		buttonText = text;
	}

	IEnumerator WaitForKey()
	{
		while (!keyEvent.isKey)
			yield return null;
	}

	public IEnumerator AssignKey(string keyName)
	{
		waitingForKey = true;

		yield return WaitForKey ();

		switch (keyName) 
		{
		case "left":
			GameInputManager.GIM.left = newKey;
			buttonText.text = GameInputManager.GIM.left.ToString ();
			PlayerPrefs.SetString ("leftKey", GameInputManager.GIM.left.ToString ());
			break;
		case "right":
			GameInputManager.GIM.right = newKey;
			buttonText.text = GameInputManager.GIM.right.ToString ();
			PlayerPrefs.SetString ("rightKey", GameInputManager.GIM.right.ToString ());
			break;
		case "jump":
			GameInputManager.GIM.jump = newKey;
			buttonText.text = GameInputManager.GIM.jump.ToString ();
			PlayerPrefs.SetString ("jumpKey", GameInputManager.GIM.jump.ToString ());
			break;
		case "attack":
			GameInputManager.GIM.attack = newKey;
			buttonText.text = GameInputManager.GIM.attack.ToString ();
			PlayerPrefs.SetString ("attackKey", GameInputManager.GIM.attack.ToString ());
			break;
		case "consume":
			GameInputManager.GIM.consume = newKey;
			buttonText.text = GameInputManager.GIM.consume.ToString ();
			PlayerPrefs.SetString ("consumeKey", GameInputManager.GIM.consume.ToString ());
			break;
		case "charge":
			GameInputManager.GIM.charge = newKey;
			buttonText.text = GameInputManager.GIM.charge.ToString ();
			PlayerPrefs.SetString ("chargeKey", GameInputManager.GIM.charge.ToString ());
			break;
		case "dodge":
			GameInputManager.GIM.dodge = newKey;
			buttonText.text = GameInputManager.GIM.dodge.ToString ();
			PlayerPrefs.SetString ("dodgeKey", GameInputManager.GIM.dodge.ToString ());
			break;
		case "tornado":
			GameInputManager.GIM.tornado = newKey;
			buttonText.text = GameInputManager.GIM.tornado.ToString ();
			PlayerPrefs.SetString ("tornadoKey", GameInputManager.GIM.tornado.ToString ());
			break;
		}

		yield return null;
	}

	public void DefaultKey()
	{
		GameInputManager.GIM.left = KeyCode.LeftArrow;
		GameInputManager.GIM.right = KeyCode.RightArrow;
		GameInputManager.GIM.jump = KeyCode.Space;
		GameInputManager.GIM.attack = KeyCode.R;
		GameInputManager.GIM.consume = KeyCode.T;
		GameInputManager.GIM.charge = KeyCode.Q;
		GameInputManager.GIM.dodge = KeyCode.W;
		GameInputManager.GIM.tornado = KeyCode.E;

		PlayerPrefs.SetString ("leftKey", GameInputManager.GIM.left.ToString ());
		PlayerPrefs.SetString ("rightKey", GameInputManager.GIM.right.ToString ());
		PlayerPrefs.SetString ("jumpKey", GameInputManager.GIM.jump.ToString ());
		PlayerPrefs.SetString ("attackKey", GameInputManager.GIM.attack.ToString ());
		PlayerPrefs.SetString ("consumeKey", GameInputManager.GIM.consume.ToString ());
		PlayerPrefs.SetString ("chargeKey", GameInputManager.GIM.charge.ToString ());
		PlayerPrefs.SetString ("dodgeKey", GameInputManager.GIM.dodge.ToString ());
		PlayerPrefs.SetString ("tornadoKey", GameInputManager.GIM.tornado.ToString ());
		for (int i = 0; i < 8; i++) {
			if (bindSetter.GetChild (i).name == "LeftButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.left.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "RightButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.right.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "JumpButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.jump.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "AttackButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.attack.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "ConsumeButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.consume.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "ChargeButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.charge.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "DodgeButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.dodge.ToString ();
			}
			else if (bindSetter.GetChild (i).name == "TornadoButton") {
				bindSetter.GetChild (i).GetComponentInChildren<Text> ().text = GameInputManager.GIM.tornado.ToString ();
			}
		}
	}
}
