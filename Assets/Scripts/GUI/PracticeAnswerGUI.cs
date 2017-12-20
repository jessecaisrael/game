using UnityEngine;
using System.Collections;

public class PracticeAnswerGUI : MonoBehaviour {

	void OnGUI() {
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		GUIStyle titleStyle = new GUIStyle (labelStyle);
		titleStyle.fontSize = 24;
		GUIStyle paragraphStyle = new GUIStyle (labelStyle);
		paragraphStyle.wordWrap = true;
		
		
		//Content
		GUILayout.BeginArea (CenteredRect ((int)(Screen.width / 1.25), (int)(Screen.height / 1.25)));
		{//block for organization
			GUILayout.Label ("Practice Complete", titleStyle);

			string labelText = "Click Continue to begin the experiment.";
			GUILayout.Label (labelText, paragraphStyle, GUILayout.ExpandWidth(true));
			
			GUILayout.Space(50);
			
			if( GUILayout.Button("Click to Continue")) {
				gameObject.GetComponent<ExperimentDriver>().enabled = true;
				Destroy(this);
			}
			
		}
		
		GUILayout.EndArea ();
	}
	
	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
