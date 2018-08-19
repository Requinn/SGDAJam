using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitPrefs : MonoBehaviour {

	public void SavePrefs() {
		PlayerPrefs.Save();
	}
}
