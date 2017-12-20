using UnityEngine;
using System.Collections;

public class ExperimentCompleteGUI : MonoBehaviour {

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
			GUILayout.Label ("Trials Complete", titleStyle);
			
			string message = "All trials complete!  Please exit and return to Qualtrics for a few wrap-up questions.";
			GUILayout.Label (message, paragraphStyle, GUILayout.ExpandWidth(true));
			
			GUILayout.Space(50);
			
			if( GUILayout.Button("Click to Exit")) {
				Application.Quit();
			}
			
		}
		
		GUILayout.EndArea ();
	}
	
	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}


}
