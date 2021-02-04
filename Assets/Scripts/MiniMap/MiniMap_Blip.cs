using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_Blip : MonoBehaviour {
	[HideInInspector]
	public Transform followed;
	[HideInInspector]
	public float height;

	private Renderer rend;
	
	private void Update () {
		transform.position = followed.position + Vector3.up * height;
	}

	public void SetColor (Color color) {
		if(!rend) rend = GetComponent<Renderer>();
		
		rend.material.SetColor("_Color", color);
	}
}
