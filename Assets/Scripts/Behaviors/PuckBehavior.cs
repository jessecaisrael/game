using UnityEngine;
using System.Collections;

public class PuckBehavior : MonoBehaviour {

	public bool isTarget;
	public Color puckColor;

	private Vector3 initialDirection;
	private Vector3 initialPosition;

	private float speed = 10.1f;
	private Texture colliderTex = null;
	private int numPuckCollisions = 0;
	private int numWallCollisions = 0;
	private float transferRefractoryPeriod = 0.5f;
	private float timeOfLastTransfer;

	void Start () {
		timeOfLastTransfer =  Time.time;
	}
	
	void OnCollisionEnter(Collision col) {
		if( col.gameObject.tag == "Puck" ) {
			//Count collisions
			numPuckCollisions++;
			PuckBehavior colScript = (PuckBehavior) col.gameObject.GetComponent<PuckBehavior>();

			//Count transfers
			if( colScript.isTarget) {
				if( puckColor == colScript.puckColor ) {
					if( Time.time - timeOfLastTransfer > transferRefractoryPeriod && Time.time - colScript.timeOfLastTransfer > transferRefractoryPeriod) {
						if( (GameObject.FindGameObjectWithTag("TrialScripts")).GetComponent<TrialDriver>().GetTimeRemaining() > 1) { //v3: Don't have collision in the last second so have enough time to hit spacebar
							if( Random.value <= .75 ) {
								//First stage of texture transfer (see OnCollisionExit for completion)
								colliderTex = col.gameObject.GetComponent<Renderer>().material.mainTexture;
								col.gameObject.GetComponent<Renderer>().material.mainTexture = gameObject.GetComponent<Renderer>().material.mainTexture;
								if( colScript.puckColor == ActiveConditionSingleton.attendedPuckColor) {
									GameObject logParent = GameObject.FindGameObjectWithTag("ExperimentScripts");
									logParent.GetComponent<Logger>().actualTransfers++;
								}
							}
						}
					}
				}
			}
		}
		else if( col.gameObject.tag == "Wall" ) {
			numWallCollisions++;
		}
	}
	
	void OnCollisionExit(Collision col) {
		if( col.gameObject.tag == "Puck" ) {
			PuckBehavior colScript = (PuckBehavior) col.gameObject.GetComponent<PuckBehavior>();
			if( Time.time - timeOfLastTransfer > transferRefractoryPeriod && Time.time - colScript.timeOfLastTransfer > transferRefractoryPeriod) {
				//The following only occurs if the other puck is the target
				if( colliderTex != null ) {
					//Complete texture transfer
					gameObject.GetComponent<Renderer>().material.mainTexture = colliderTex;
					colliderTex = null;
					//Switch target
					colScript.isTarget = false;
					isTarget = true;
					//Record collision time
					timeOfLastTransfer = Time.time;
					colScript.timeOfLastTransfer = Time.time;
				}
			}
		}
		
		//Scale velocity to maintain constant speed
		GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * speed;
	}

	public Vector3 getInitialPosition () {
		return initialPosition;
	}

	public void setInitialPosition (Vector3 initialPosition) {
		this.initialPosition = initialPosition;
        Random.InitState((int)initialPosition.x);
        //Deprecated: Random.seed = (int)initialPosition.x;
	}

	public Vector3 getInitialDirection () {
		return initialDirection;
	}

	public void setInitialDirection (Vector3 initialDirection) {
			this.initialDirection = initialDirection;
	}

	public int getNumPuckCollisions () {
		return numPuckCollisions;
	}

	public int getNumWallCollisions () {
		return numWallCollisions;
	}

	public float getSpeed() {
		return speed;
	}

}
