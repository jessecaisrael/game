using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour {

	public int pid;
	public int trialNum = 1;
	public int scenario;
	public string attendedPuckColor;
	public string attendedTargetColor;
	public string unattendedUnexpectedTargetColor;
	public int unexpectedTime;
	public string unexpectedPuckChgOccurred;
    public string unexpectedSoundOccurred;
    public int observedTransfers;
	public int actualTransfers = 0;
	public int transferError;
	public string correctTransfers;
	public int transferConfidence;
	public string observedUnexpected;
    public string correctUnexpected;
	public int unexpectedConfidence;
	public string unexpectedDescription;
	public string trackedBaseColor;
	public string actualBaseColor;
	public string correctTracked;
	public string logDate;
	public string logTime;
    public int oddballPosition;
    public int oddballEar;
    public string oddballOccurred;
    public string observedOddball;
    public int unexpectedSoundPosition;
    public int unexpectedSoundEar;
    public int unexpectedSoundOption;
    public int oddballConfidence;

    // Use this for initialization
	void Start () {
        DontDestroyOnLoad (this);
	}

	public void LogTrial() {
		//Calculations
		transferError = actualTransfers - observedTransfers;
		if (transferError == 0)
						correctTransfers = "yes";
		else
						correctTransfers = "no";

        if (observedUnexpected == unexpectedPuckChgOccurred && observedUnexpected == unexpectedSoundOccurred)
            correctUnexpected = "all";
        else if (observedUnexpected == unexpectedPuckChgOccurred || observedUnexpected == unexpectedSoundOccurred)
            correctUnexpected = "partial";
        else correctUnexpected = "none";

        if (trackedBaseColor == actualBaseColor) //v3: added
						correctTracked = "yes";
				else
						correctTracked = "no";

		//Date and time string conversion
		System.DateTime now = System.DateTime.Now;
		logDate = now.ToShortDateString();

        logTime = "";
		if (now.Hour < 10)
						logTime += "0" + now.Hour + ":";
				else
						logTime += now.Hour + ":";
		if (now.Minute < 10)
						logTime += "0" + now.Minute + ":";
				else
						logTime += now.Minute + ":";
		if (now.Second < 10)
						logTime += "0" + now.Second;
				else
						logTime += now.Second;

		//Log
        string message = pid.ToString () + "," + trialNum++ + "," + scenario + "," + attendedPuckColor + "," + attendedTargetColor + "," + unattendedUnexpectedTargetColor + "," + unexpectedTime +
            "," + observedTransfers + "," + actualTransfers + "," + transferError + "," + correctTransfers + "," + transferConfidence + "," + oddballOccurred + 
            "," + oddballPosition + "," + oddballEar + "," + observedOddball + "," + oddballConfidence + "," + observedUnexpected + "," + correctUnexpected + "," + unexpectedConfidence + "," + unexpectedPuckChgOccurred + "," + unexpectedSoundOccurred + "," + unexpectedSoundPosition + ","
            + unexpectedSoundEar + "," + unexpectedSoundOption + "," + unexpectedDescription + "," + trackedBaseColor + "," + 
            actualBaseColor + "," + correctTracked + "," + logDate + "," + logTime;

        using ( System.IO.StreamWriter w = System.IO.File.AppendText(Application.dataPath + "/.." + "/Assets/IO/Experiment Data.csv")) {
            w.WriteLine(message);
            w.Flush();
        }
	}

    //Need to add method to log fullattention trial here

}
