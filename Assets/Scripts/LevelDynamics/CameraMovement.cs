using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	public float smooth = 1.5f;

	private Transform player;
	private Vector3 relCameraPos; //Relative Camera Position
	private float relCameraPosMag; //Relative Camera Position Magnitude
	private Vector3 newPos;

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		relCameraPos = transform.position - player.position;
		//Reduce by small amount because the player's position is at his feet,
		//thus when we want to ray cast to the player to see if anything is in the way we will hit the ground if we don't reduce it slightely
		relCameraPosMag = relCameraPos.magnitude - 0.5f;
	}

	//Although it is not a physics object it will be following a physics object,
	//so in order to get the movement as smooth as possible both updates have to happen at same time (player and camera)
	void FixedUpdate ()
	{
		Vector3 standardPos = player.position + relCameraPos;
		Vector3 abovePos = player.position + relCameraPosMag * Vector3.up;
		Vector3[] checkPoints = new Vector3[5]; //points which we check if the camera can see the player
		checkPoints [0] = standardPos;
		checkPoints [1] = Vector3.Lerp (standardPos, abovePos, 0.25f);
		checkPoints [2] = Vector3.Lerp (standardPos, abovePos, 0.5f);
		checkPoints [3] = Vector3.Lerp (standardPos, abovePos, 0.75f);
		checkPoints [4] = abovePos;

		for (int i = 0; i < checkPoints.Length; i++) {
			if (ViewingPosCheck (checkPoints [i])) {
				break;
			}
		}

		transform.position = Vector3.Lerp (transform.position, newPos, smooth * Time.deltaTime);
		SmoothLookAt ();
	}

	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;

		if (Physics.Raycast (checkPos, player.position - checkPos, out hit, relCameraPosMag)) {
			if (hit.transform != player) {
				return false;
			}
		}

		newPos = checkPos;
		return true;
	}

	void SmoothLookAt ()
	{
		Vector3 relPlayerPosition = player.position - transform.position;
		Quaternion lookAtRotation = Quaternion.LookRotation (relPlayerPosition, Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
}
