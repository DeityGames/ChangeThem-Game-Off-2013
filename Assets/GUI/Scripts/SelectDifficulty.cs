using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectDifficulty : MonoBehaviour {
	
	public void LoadDowntown()
	{
		SceneManager.LoadScene("DowntownScene");
	}

	public void LoadParishCity()
	{
		SceneManager.LoadScene("ParishCityScene");
	}

	public void LoadOctagon()
	{
		SceneManager.LoadScene("OctagonScene");
	}

	public void LoadPatientZero()
	{
		SceneManager.LoadScene("PatientZeroScene");
	}
}
