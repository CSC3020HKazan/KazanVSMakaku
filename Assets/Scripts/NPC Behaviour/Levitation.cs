using UnityEngine;
using System.Collections;

public class Levitation : MonoBehaviour {

	public int frames, distance ; // number of frames per ascent/descent
	private int frameCounter ;
	public float speed ;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Levitate ();
	}

	private void Levitate () {
		if( frameCounter >= frames ) {
			frameCounter = 0;
			distance *= -1;
		} else {
			frameCounter++;
			transform.Translate(0, ( Time.deltaTime * distance * speed ), 0, Space.Self);
		}

	}
}
