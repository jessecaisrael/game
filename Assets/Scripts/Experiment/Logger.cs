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
	public string unexpectedPuckOccurred;
	public int observedTransfers;
	public int actualTransfers = 0;
	public int transferError;
	public string correctTransfers;
	public int transferConfidence;
	public string observedUnexpected;
	public string correctPuckChgUnexpected;
    public string correctSoundUnexpected;
	public int unexpectedConfidence;
	public string unexpectedDescription;
	public int observedTrialTime;
	public string trackedBaseColor;
	public string actualBaseColor;
	public string correctTracked;
	public string logDate;
	public string logTime;
    public string unexpectedSoundOccurred;
    public int oddBallPosition = -1;
    public string oddballOccurred;
    public string observedOddball;

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

        if (observedUnexpected == unexpectedPuckOccurred) //v4: Add back if/else for yes/no.
		    correctPuckChgUnexpected = "yes";
		else
			correctPuckChgUnexpected = "no";

        if (observedUnexpected == unexpectedSoundOccurred)
            correctSoundUnexpected = "yes";
        else
            correctSoundUnexpected = "no";

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
            "," + oddBallPosition + "," + observedOddball + "," + unexpectedSoundOccurred + "," + unexpectedPuckOccurred + "," + observedUnexpected + "," + correctPuckChgUnexpected + "," + correctSoundUnexpected + "," + unexpectedConfidence + "," 
            + unexpectedDescription + "," + observedTrialTime + "," + trackedBaseColor + "," + 
            actualBaseColor + "," + correctTracked + "," + logDate + "," + logTime;

        Debug.Log("LOGGER.cs - DATAPATH is: " + Application.dataPath);
        using ( System.IO.StreamWriter w = System.IO.File.AppendText(Application.dataPath + "/.." + "/Assets/IO/Experiment Data.csv")) {
            w.WriteLine(message);
            w.Flush();
        }
	}


}
