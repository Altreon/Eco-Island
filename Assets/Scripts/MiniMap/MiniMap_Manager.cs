using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMap_Manager : MonoBehaviour {
	/*[SerializeField]
	LevelCamSettings[] camSettings;*/

	[SerializeField]
	private float heightOfBlit = 0;
	/*[SerializeField]
	Blit[] tagsToTrack;*/
	[SerializeField]
	private Blit playerBlit = null;
	[SerializeField]
	private Transform player = null;

	private Camera cam;
	private List<GameObject> activeBlits;

	

	void Awake () {
		cam = GetComponent<Camera> ();

		activeBlits = new List<GameObject> ();
	}

	/*public void createBlits () {
		foreach (Blit blit in tagsToTrack) {
			foreach (GameObject objWithTag in GameObject.FindGameObjectsWithTag(blit.tag))
			{
				MiniMap_Blip miniMapBlit = GameObject.Instantiate (blit.prefab).GetComponent<MiniMap_Blip>();
				miniMapBlit.followed = objWithTag.transform;
				miniMapBlit.height = heightOfBlit;

				miniMapBlit.gameObject.GetComponent<Renderer>().material.SetColor("_Color", blit.color);

				activeBlits.Add (miniMapBlit.gameObject);
			}
		}
	}*/

	public void Start () {
		CreateBlit(playerBlit, player);
	}

	public void CreateBlit (Blit blit, Transform target) {
		MiniMap_Blip miniMapBlit = GameObject.Instantiate(blit.prefab);
		miniMapBlit.followed = target;
		miniMapBlit.height = heightOfBlit;

		miniMapBlit.SetColor(blit.color);

		activeBlits.Add(miniMapBlit.gameObject);
	}

	public void DestroyBlits () {
		foreach (GameObject blit in activeBlits) {
			Destroy (blit);
		}

		activeBlits.Clear ();
	}

	/*public void FitCameraWithLevel () {
		LevelCamSettings settings = camSettings[SceneManager.GetActiveScene().buildIndex - 1];

		transform.position = settings.position;
		cam.orthographicSize = settings.size;
	}*/
}

[System.Serializable]
public class Blit {
	//public string tag = null;
	public Color color = Color.black;
	public MiniMap_Blip prefab = null;
}

/*[System.Serializable]
class LevelCamSettings {
	public Vector3 position = Vector3.zero;
	public float size = 0;
}*/