using UnityEngine;
using System.Collections;

public class SceneFadeInOutUnity4x : MonoBehaviour
{
	public float fadeSpeed = 1.5f;

	private bool sceneStarting = true;
	private GUITexture guiTexture;

	void Awake ()
	{
		guiTexture = GetComponent<GUITexture> ();
		guiTexture.pixelInset = new Rect (0f, 0f, Screen.width, Screen.height);
	}

	void Update ()
	{
		if (sceneStarting) {
			StartScene ();
		}
	}

	void FadeToClear ()
	{
		guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	void FadeToBlack ()
	{
		guiTexture.color = Color.Lerp (guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	void StartScene ()
	{
		FadeToClear ();

		//Color is very close to clear
		if (guiTexture.color.a <= 0.05f) {
			guiTexture.color = Color.clear;
			//Despite the fact that color is transparent Unity will need to check its alpha each frame to determine how things behind it are rendered
			guiTexture.enabled = false; //stop it from rendering
			sceneStarting = false;
		}
	}

	public void EndScene ()
	{
		guiTexture.enabled = true;
		FadeToBlack ();

		if (guiTexture.color.a >= 0.95f) {
			Application.LoadLevel (1);
		}
	}
}
