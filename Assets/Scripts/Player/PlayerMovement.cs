using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public AudioClip shoutingClip;
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	public bool canMove = false;

	private Rigidbody rigidbody;
	private AudioSource audio;
	private Animator anim;
	private HashIDs hash;

	void Awake ()
	{
		rigidbody = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
		anim.SetLayerWeight (1, 1f);
	}

	//Player is a physic object
	void FixedUpdate ()
	{
		if (canMove) {
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");
			bool sneak = Input.GetButton ("Sneak");

			MovementManagement (h, v, sneak);
		}
	}
	
	void Update ()
	{
		if (canMove) {
			bool shout = Input.GetButton ("Attract");
			anim.SetBool (hash.shoutingBool, shout);
			AudioManagement (shout);
		}
	}

	void MovementManagement (float horizontal, float vertical, bool sneaking)
	{
		anim.SetBool (hash.sneakingBool, sneaking);

		if (horizontal != 0f || vertical != 0f) {
			Rotating (horizontal, vertical);
			anim.SetFloat (hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
		} else {
			anim.SetFloat (hash.speedFloat, 0f);
		}
	}

	void Rotating (float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3 (horizontal, 0f, vertical);
		//Quaternion is a way of storing a rotation - we need this to assign rotation to the player
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
		//Smooth the rotation of the player as he turns
		Quaternion newRotation = Quaternion.Lerp (rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		rigidbody.MoveRotation (newRotation);
	}

	void AudioManagement (bool shout)
	{
		if (anim.GetCurrentAnimatorStateInfo (0).fullPathHash == hash.locomotionState) {
			if (!audio.isPlaying) {
				audio.Play ();
			}
		} else {
			audio.Stop ();
		}

		if (shout && !(anim.GetCurrentAnimatorStateInfo (1).fullPathHash == hash.shoutState)) {
			//We don't want shouting audioclip to replace the footsteps audioclip so we PlayClipAtPoint
			AudioSource.PlayClipAtPoint (shoutingClip, transform.position);
		}
	}
}
