using UnityEngine;
using System.Collections;

public class LaserBlinking : MonoBehaviour
{
	public float onTime;
	public float offTime;

	private float timer;
	private Renderer renderer;
	private Light light;

	void Awake ()
	{
		renderer = GetComponent<Renderer> ();
		light = GetComponent<Light> ();
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (renderer.enabled && timer >= onTime) {
			SwitchBeam ();
		}

		if (!renderer.enabled && timer >= offTime) {
			SwitchBeam ();
		}
	}

	void SwitchBeam ()
	{
		timer = 0f;

		renderer.enabled = !renderer.enabled;
		light.enabled = !light.enabled;
	}
}
