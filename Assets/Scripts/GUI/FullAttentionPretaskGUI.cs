using UnityEngine;
using System.Collections;

public class FullAttentionPretaskGUI : MonoBehaviour
{

    void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 14;
        labelStyle.normal.textColor = Color.black;
        labelStyle.richText = true;
        GUIStyle titleStyle = new GUIStyle(labelStyle);
        titleStyle.fontSize = 24;
        GUIStyle paragraphStyle = new GUIStyle(labelStyle);
        paragraphStyle.wordWrap = true;


        //Content
        GUILayout.BeginArea(CenteredRect((int)(Screen.width / 1.25), (int)(Screen.height / 1.25)));
        {//block for organization
            GUILayout.Label("Post Experiment Task", titleStyle);

            string instructionsText = "In the next trial, you will see the same moving pucks and hear the same sounds that were presented to you previously during this experiment.  For this task, the instructions are <b>different</b>. Instead of tracking pucks and listening for the high pitch sound, you will look and listen for " +
                        "things that are irrelevant to your previous tasks. Please take mental note of things that are <i>different or unexpected for this task</i>. You will answer questions about this task afterwards. ";               
            GUILayout.Label(instructionsText, paragraphStyle, GUILayout.ExpandWidth(true));

            GUILayout.Space(20);

            if (GUILayout.Button("Click to Continue"))
            {
                gameObject.GetComponent<FullAttentionDriver>().enabled = true;
                Destroy(this);
            }

        }

        GUILayout.EndArea();
    }


    Rect CenteredRect(int width, int height)
    {
        return new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height);
    }

}
