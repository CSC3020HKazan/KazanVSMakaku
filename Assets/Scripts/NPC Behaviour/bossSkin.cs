using UnityEngine;
using System.Collections;

public class bossSkin : MonoBehaviour 
{
	public float scrollSpeed = 0.5F;
	void Update() {
		float offset = Time.time * scrollSpeed;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}