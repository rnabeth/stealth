using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public float health = 100f;
	public float resetAfterDeathTime = 5f;
	public AudioClip deathClip;

	private Animator anim;
	private PlayerMovement playerMovement;
	private HashIDs hash;
	private SceneFadeInOut sceneFadeInOut;
	private LastPlayerSighting lastPlayerSighting;
	private float timer;
	private bool playerDead;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		playerMovement = GetComponent<PlayerMovement> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
		sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.canvas).GetComponent<SceneFadeInOut> ();
		lastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ();
	}

	void Update ()
	{
		if (health <= 0f) {
			if (!playerDead) {
				PlayerDying ();
			} else {
				PlayerDead ();
				LevelReset ();
			}
		}
	}

	void PlayerDying ()
	{
		playerDead = true;
		anim.SetBool (hash.deadBool, true);
		//The player's AudioSource is set to loop so we don't want to replace it with the dying audio clip. So we play clip at point.
		AudioSource.PlayClipAtPoint (deathClip, transform.position);
	}

	void PlayerDead ()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).fullPathHash == hash.dyingState) {
			anim.SetBool (hash.deadBool, false); //Death animation will play just once
		}

		//Stop the player from moving
		anim.SetFloat (hash.speedFloat, 0f);
		playerMovement.enabled = false;

		//Switch off the alarms
		lastPlayerSighting.position = lastPlayerSighting.resetPosition;

		//Stop footsteps sound
		GetComponent<AudioSource> ().Stop ();
	}

	void LevelReset ()
	{
		timer += Time.deltaTime;

		if (timer >= resetAfterDeathTime) {
			sceneFadeInOut.EndScene ();
		}
	}

	public void TakeDamage (float amount)
	{
		health -= amount;
	}
}
