using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SimulationDriver : MonoBehaviour {

	private double startTime;
	private float wallLength;
	private float puckDiameter;
	private float trialLength;

	// Use this for initialization
	void Start () {
		//Instantiate vars
		wallLength = 30;
		puckDiameter = 1;
		trialLength = 20;

		//Freeze for loading
		Time.timeScale = 0;

		//Determine random initial positions
		int numPucks = GameObject.FindGameObjectsWithTag("Puck").Length;
		ArrayList initialPositions = new ArrayList (numPucks);
        int seedVal = this.GetHashCode() + unchecked((int)System.DateTime.UtcNow.Ticks);
        Random.InitState(seedVal);
        //Deprecated: Random.seed = this.GetHashCode () + unchecked((int)System.DateTime.UtcNow.Ticks);
		while(initialPositions.Count < initialPositions.Capacity) {
			//Generate random position
			Vector3 randomV = Random.insideUnitSphere;
			Vector3 randomPosition = new Vector3 (randomV.x, 0F, randomV.z).normalized;
			randomPosition *= Random.Range(0, wallLength/2 - puckDiameter);
			//Add to list if not overlapping
			bool overlapping = false;
			foreach( Vector3 existingPosition in initialPositions ) {
				if( Vector3.Distance(existingPosition, randomPosition) <= 1.5 * puckDiameter ) {
					overlapping = true;
					break;
				}
			}
			if( !overlapping ) {
				initialPositions.Add(randomPosition);
			}
		}

		//Determine random initial direction vectors
		ArrayList initialDirections = new ArrayList (numPucks);
		while (initialDirections.Count < initialDirections.Capacity) {
			//Generate random direction
			Vector3 randomV = Random.insideUnitSphere;
			initialDirections.Add(new Vector3 (randomV.x, 0F, randomV.z).normalized);
		}

		//Set initial position and velocity
		GameObject[] puckGOs = GameObject.FindGameObjectsWithTag("Puck");
		for (int i=0; i<puckGOs.Length; i++) {
			//position
			((PuckBehavior) puckGOs[i].GetComponent<PuckBehavior>()).setInitialPosition((Vector3)initialPositions[i]);
			puckGOs[i].transform.position = (Vector3)initialPositions[i];
			//velocity
			((PuckBehavior) puckGOs[i].GetComponent<PuckBehavior>()).setInitialDirection((Vector3)initialDirections[i]);
			puckGOs[i].GetComponent<Rigidbody>().velocity = (Vector3)initialDirections[i] * ((PuckBehavior) puckGOs[i].GetComponent<PuckBehavior>()).getSpeed();
		}

		//Start
		Time.timeScale = 1;
		startTime = Time.time;  //will be 0 because affected by timeScale
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - startTime >= trialLength) {
			// Compose log message
			int minPuckCollisions = 99999;
			int maxPuckCollisions = 0;
			int minTotalCollisions = 99999;
			int maxTotalCollisions = 0;
			string logMessage = "";
			GameObject[] puckGOs = GameObject.FindGameObjectsWithTag("Puck");
			foreach( GameObject puckGO in puckGOs) {
				PuckBehavior controller = (PuckBehavior) puckGO.GetComponent<PuckBehavior>();
				int puckCollisions = controller.getNumPuckCollisions();
				int wallCollisions = controller.getNumWallCollisions();
				int totalCollisions = puckCollisions + wallCollisions;
				if( puckCollisions < minPuckCollisions ) minPuckCollisions = puckCollisions;
				if( puckCollisions > maxPuckCollisions ) maxPuckCollisions = puckCollisions;
				if( totalCollisions < minTotalCollisions) minTotalCollisions = totalCollisions;
				if( totalCollisions > maxTotalCollisions) maxTotalCollisions = totalCollisions;
				logMessage += controller.getInitialPosition().x + "," + controller.getInitialPosition().z + "," +
					controller.getInitialDirection().x + "," + controller.getInitialDirection().z + "," +
					puckCollisions + "," + wallCollisions + ",";
			}
			logMessage += minPuckCollisions + "," + maxPuckCollisions + "," + (maxPuckCollisions-minPuckCollisions) + ",";
			if( (maxPuckCollisions-minPuckCollisions) <= 5 ) logMessage += "yes,";
			else logMessage += "no,";
			if( (maxPuckCollisions-minPuckCollisions) <= 10 ) logMessage += "yes,";
			else logMessage += "no,";
			if( (maxPuckCollisions-minPuckCollisions) <= 15 ) logMessage += "yes,";
			else logMessage += "no,";
			logMessage += minTotalCollisions + "," + maxTotalCollisions + "," + (maxTotalCollisions-minTotalCollisions) + ",";
			if( (maxTotalCollisions-minTotalCollisions) <= 5 ) logMessage += "yes,";
			else logMessage += "no,";
			if( (maxTotalCollisions-minTotalCollisions) <= 10 ) logMessage += "yes,";
			else logMessage += "no,";
			if( (maxTotalCollisions-minTotalCollisions) <= 15 ) logMessage += "yes";
			else logMessage += "no";

            Debug.Log(Application.dataPath);
			// Write to filef
			using( System.IO.StreamWriter w = System.IO.File.AppendText(Application.dataPath + "/.." + "/IO/Scenario Data.csv")) {
				w.WriteLine(logMessage);
                w.Flush();
			}

            SceneManager.LoadScene("Simulation");
			//Deprecated: Application.LoadLevel("Simulation");
		}
	}

}
