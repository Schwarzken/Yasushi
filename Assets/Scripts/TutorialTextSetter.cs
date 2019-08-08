using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialTextSetter : MonoBehaviour {

	[SerializeField] TextMeshProUGUI movementTutorial;
	[SerializeField] TextMeshProUGUI jumpTutorial;
	[SerializeField] TextMeshProUGUI attackTutorial;
	[SerializeField] TextMeshProUGUI consumeTutorial;
	[SerializeField] TextMeshProUGUI chargeTutorial;
	[SerializeField] TextMeshProUGUI rollTutorial;
	[SerializeField] TextMeshProUGUI tornadoTutorial;
	[SerializeField] Text chargeIconText;
	[SerializeField] Text dodgeIconText;
	[SerializeField] Text tornadoIconText;


	void Start () {
		string leftControl = GameInputManager.GIM.left.ToString ();
		string rightControl = GameInputManager.GIM.right.ToString ();
		string jumpControl = GameInputManager.GIM.jump.ToString ();
		string attackControl = GameInputManager.GIM.attack.ToString ();
		string consumeControl = GameInputManager.GIM.consume.ToString ();
		string chargeControl = GameInputManager.GIM.charge.ToString ();
		string rollControl = GameInputManager.GIM.dodge.ToString ();
		string torControl = GameInputManager.GIM.tornado.ToString ();

		if (movementTutorial != null) {
			movementTutorial.text = "<color=red>" + leftControl + "</color>" + "\n Move Left \n" + "<color=red>" + rightControl + "</color>" + "\n Move Right";
		}
		if (jumpTutorial != null) {
			jumpTutorial.text = "<color=red>" + jumpControl + "</color> - Jump \n (Hold to Jump \n  Higher!)";
		}
		if (attackTutorial != null) {
			attackTutorial.text = "<color=red>" + attackControl + "</color> - Attack! \n <color=red>" + attackControl + "," + attackControl + "," + attackControl + "</color> - Combo! \n <color=red> Hold " + attackControl + "</color> - Uppercut!"; 
		}
		if (consumeTutorial != null) {
			consumeTutorial.text = "When enemies are \n <color=red>WEAK</color> enough, press \n<color=red>" + consumeControl + "</color> to EAT them!";
		}
		if (chargeTutorial != null) {
			chargeTutorial.text = "<color=red>" + chargeControl + "</color> - Charge! \n Now <color=red>EAT</color> this \n punk too!";
		}
		if (rollTutorial != null) {
			rollTutorial.text = "<color=red>" + rollControl + "</color> - Dodge Roll \n Now <color=red>DEFEAT</color> the \n waves of Sushi!";
		}
		if (tornadoTutorial != null) {
			tornadoTutorial.text = "<color=red>" + torControl + "</color> - Tornado!";
		}
	}
	
	// Update is called once per frame
	void Update () {
		string leftControl = GameInputManager.GIM.left.ToString ();
		string rightControl = GameInputManager.GIM.right.ToString ();
		string jumpControl = GameInputManager.GIM.jump.ToString ();
		string attackControl = GameInputManager.GIM.attack.ToString ();
		string consumeControl = GameInputManager.GIM.consume.ToString ();
		string chargeControl = GameInputManager.GIM.charge.ToString ();
		string rollControl = GameInputManager.GIM.dodge.ToString ();
		string torControl = GameInputManager.GIM.tornado.ToString ();

		if (chargeIconText != null) {
			chargeIconText.text = chargeControl;
		}
		if (dodgeIconText != null) {
			dodgeIconText.text = rollControl;
		}
		if (tornadoIconText != null) {
			tornadoIconText.text = torControl;
		}
	}
}
