using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOpenClose : MonoBehaviour {

	public GameObject goHinge;
	public TextMesh textMesh;
	public UnityStandardAssets.Cameras.LookatTarget interactPromptLooker;
	public float degreesRotate = 80;
	public float secondsToOpen = 1.0f;
	public float secondsToClose;

	protected float degreesClosed;
	protected float degreesOpened;

	protected enum HingeState
	{
		closed,
		opening,
		opened,
		closing
	}
	protected HingeState hingeState;

	protected bool isPlayerNear;
	protected float timeStartedRotation;

	// Use this for initialization
	void Start () {
		textMesh.text = "[R] Open";
		textMesh.gameObject.SetActive(false);
		if (secondsToClose <= 0)
		{
			Debug.LogWarning("Seconds To Close must be > 0. Defaulting to 1 second.");
			secondsToClose = 1.0f;
		}
		if (secondsToOpen <= 0)
		{
			Debug.LogWarning("Seconds To Open must be > 0. Defaulting to 1 second.");
			secondsToOpen = 1.0f;
		}
		degreesClosed = goHinge.transform.eulerAngles.y;
		degreesOpened = degreesClosed + degreesRotate;

		if (interactPromptLooker)
		{
			interactPromptLooker.SetTarget(Camera.main.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (isPlayerNear)
		{
			if (hingeState == HingeState.closed || hingeState == HingeState.opened)
			{
				if (Input.GetKeyDown(KeyCode.R))
				{
					textMesh.text = "";
					hingeState = (hingeState == HingeState.closed) ? HingeState.opening : HingeState.closing;
					timeStartedRotation = Time.time;
				}
			}
		}

		// Update rotation if in closing or opening state
		if (hingeState == HingeState.closing)
		{
			if (InterpRotationY(goHinge.transform, timeStartedRotation, secondsToClose, degreesOpened, degreesClosed))
			{ // Done rotating
				hingeState = HingeState.closed;
				textMesh.text = "[R] Open";
			}
		}
		else if (hingeState == HingeState.opening)
		{
			if (InterpRotationY(goHinge.transform, timeStartedRotation, secondsToOpen, degreesClosed, degreesOpened))
			{ // Done rotating
				hingeState = HingeState.opened;
				textMesh.text = "[R] Close";
			}
		}
	}

	// Returns true when rotation is complete
	bool InterpRotationY(Transform trans, float timeStarted, float secondsDuration, float degreesStart, float degreesEnd)
	{

		float timeElapsed = Time.time - timeStarted;
		float interp = timeElapsed / secondsDuration;
		if (interp < 1.0f)
		{
			float degreesInterp = degreesStart + (degreesEnd - degreesStart) * interp;
			trans.eulerAngles = new Vector3(0, degreesInterp, 0);
		}
		else
		{
			trans.eulerAngles = new Vector3(0, degreesEnd, 0);
			return true;
		}
		return false;
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger Entered by: " + other.name);
		isPlayerNear = true;
		textMesh.gameObject.SetActive(true);
	}
	void OnTriggerExit(Collider other)
	{
		Debug.Log("Trigger Exited by: " + other.name);
		isPlayerNear = false;
		textMesh.gameObject.SetActive(false);
	}
}
