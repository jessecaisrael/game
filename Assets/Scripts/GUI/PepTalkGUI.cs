using UnityEngine;
using System.Collections;

public class PepTalkGUI : MonoBehaviour {

	void OnGUI() {
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		labelStyle.richText = true;
		GUIStyle titleStyle = new GUIStyle (labelStyle);
		titleStyle.fontSize = 24;
		GUIStyle paragraphStyle = new GUIStyle (labelStyle);
		paragraphStyle.wordWrap = true;
		
		
		//Content
		GUILayout.BeginArea (CenteredRect ((int)(Screen.width / 1.25), (int)(Screen.height / 1.25)));
		{//block for organization
			GUILayout.Label ("Welcome!", titleStyle);
			
			string pepTalkText = "Please continue to keep all potential distractions, such as cell phones, watch alarms, etc. turned off during the remainder of the experimenter. Please read and follow the directions carefully as they have changed from the last part of the experiment. ";
			GUILayout.Label (pepTalkText, paragraphStyle, GUILayout.ExpandWidth(true));
			
			GUILayout.Space(20);
			
			if( GUILayout.Button("Click to Continue")) {
				gameObject.GetComponent<InstructionGUI>().enabled = true;
				Destroy(this);
			}
		
		}
		
		GUILayout.EndArea ();
	}

	
	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
