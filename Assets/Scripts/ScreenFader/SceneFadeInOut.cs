using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
	private bool startScene = true;
	private bool endScene = false;
	private Animator anim;
	private float timer = 0f;
	private float fadeInTimer = 2f;
	private float fadeOutTimer = 3f;
	private PlayerMovement playerMovement;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		playerMovement = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerMovement> ();
	}

	void Update ()
	{
		if (startScene) {
			StartScene ();
		}

		if (endScene) {
			timer += Time.deltaTime;
			if (timer > fadeOutTimer) {
				endScene = false;
				Application.LoadLevel (1);
			}
		}

        //Player can only move after fade in finishes
		if (!playerMovement.canMove) {
			timer += Time.deltaTime;
			if (timer > fadeInTimer) {
				playerMovement.canMove = true;
				timer = 0f;
			}
		}
	}

	void StartScene ()
	{
		startScene = false;
		anim.SetTrigger ("FadeIn");
	}

	public void EndScene ()
	{
		endScene = true;
		anim.SetTrigger ("FadeOut");
	}
}
