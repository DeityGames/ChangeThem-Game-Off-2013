using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectDifficulty : MonoBehaviour {

	void OnGUI() {
		if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 55, 200, 50), "Downtown"))
			SceneManager.LoadScene(4);	
		else if (GUI.Button (new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 50), "Parish City"))
			SceneManager.LoadScene(1);
		else if (GUI.Button (new Rect(Screen.width / 2 - 100, Screen.height / 2 + 55, 200, 50), "The Octagon"))
			SceneManager.LoadScene(3);
		else if (GUI.Button (new Rect(Screen.width / 2 - 100, Screen.height / 2 + 110, 200, 50), "Patient Zero"))
			SceneManager.LoadScene(2);
	}
}
