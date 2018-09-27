using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display_Ending : MonoBehaviour
{

	//public GameObject Player;
	public CanvasRenderer[] UI_Ending;

	bool fadein;
	int stars = 0;

	public Image[] stars_sprites;
	public Sprite wonstar;

    float m_startTime;

    void OnTriggerEnter2D (Collider2D other)
	{
		//Player.SetActive (false);

		fadein = true;

		float YourTime = Time.realtimeSinceStartup - m_startTime;

		if (YourTime >= 290)
			stars = 1;
		if (YourTime <= 290)
			stars = 2;
		if (YourTime <= 250)
			stars = 3;
		if (YourTime <= 200)
			stars = 4;
		if (YourTime <= 150)
			stars = 5;

		for (int i = 0; i < stars; i++) {
			stars_sprites [i].sprite = wonstar;
		}
	}

	void Start ()
	{
        m_startTime = Time.realtimeSinceStartup;

		foreach (CanvasRenderer UI in UI_Ending) {
			UI.SetAlpha (0);
		}
	}

	void Update ()
	{
		if (fadein) {
			foreach (CanvasRenderer UI in UI_Ending) {
				UI.SetAlpha (Mathf.Lerp (UI.GetAlpha (), 1, 0.1f));
			}
		}
	}
}
