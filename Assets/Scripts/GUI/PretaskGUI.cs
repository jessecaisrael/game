using UnityEngine;
using System.Collections;

public class PretaskGUI : MonoBehaviour {
	
	void OnGUI() {
		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.alignment = TextAnchor.MiddleLeft;
		labelStyle.fontSize = 14;
		labelStyle.normal.textColor = Color.black;
		labelStyle.richText = true; //v3: added rich text
		GUIStyle paragraphStyle = new GUIStyle (labelStyle);
		paragraphStyle.wordWrap = true;

		//determine puck color string
		string attentedPuckColorString = "";
		if (ActiveConditionSingleton.attendedPuckColor == Color.white) attentedPuckColorString = "<b><size=" + 2 * labelStyle.fontSize + ">white</size></b>";  //v3: added rich text
		else if (ActiveConditionSingleton.attendedPuckColor == Color.black) attentedPuckColorString = "<b><size=" + 2 * labelStyle.fontSize + ">black</size></b>";  //v3: added rich text
		else throw new UnityException("Invalid color in TrialBean:" + ActiveConditionSingleton.attendedPuckColor);

		//build the label string
		string labelText = "";
		switch (ExperimentSettings.responseType) {
				case ExperimentSettings.ResponseType.Recall:
						labelText = "Count the number of times the " + attentedPuckColorString + " target puck transfers in the following simulation.";
						break;
				case ExperimentSettings.ResponseType.Spacebar:
						labelText = "Press the Spacebar each time the " + attentedPuckColorString + " target puck transfers in the following simulation.";
						break;
		}

        labelText = labelText + "\n\n Don't forget to listen for " +
            "the presence of the target sound. You can press the buttons below to listen to the sounds again\n";
           
        //place the label GUI element
        GUILayout.BeginArea (CenteredRect (Screen.width / 2, Screen.height / 2));
		GUILayout.BeginVertical ();
		GUILayout.Label (labelText, paragraphStyle);
        if (GUILayout.Button("Click to Hear Target Sound"))
            AudioManager.Instance().PlayOddball();
        if (GUILayout.Button("Click to Hear Distractor Sound 1"))
            AudioManager.Instance().PlayStdSound1();
        if (GUILayout.Button("Click to Hear Distractor Sound 2"))
            AudioManager.Instance().PlayStdSound2();
        if (GUILayout.Button("Click to Hear Distractor Sound 3"))
            AudioManager.Instance().PlayStdSound3();

        GUILayout.Space(20);

        if ( GUILayout.Button("Click to Begin")) {
			Time.timeScale = 1;
			gameObject.GetComponent<TrialDriver>().enabled = true;
			Destroy(this);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea ();
	}

	Rect CenteredRect(int width, int height) {
		return new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
	}

}
