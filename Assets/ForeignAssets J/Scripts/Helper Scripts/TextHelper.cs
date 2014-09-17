using UnityEngine;
using System.Collections;

public class TextHelper : MonoBehaviour {

	//instance
	public static TextHelper Instance;

	//3dtext to be displayed
	public Transform Text;
	public Transform Text2;
	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of TextHelper!");
		}
		Instance = this;
	}

	//spawn 3dtext
	public void instantiateText( Vector3 position, string msg)
	{
		//create text
		Transform newText = Instantiate(Text,position + new Vector3(0,2,0),
		                                GameObject.FindGameObjectWithTag("Player").transform.rotation) as Transform;

		//change text's msg
		newText.GetComponent<TextMesh>().text = msg;

		// Destroy it after it's lifetime
		Destroy(
			newText.gameObject,
			1
			);
	}
	public void instantiateText2( Vector3 position, string msg)
	{
		//create text
		Transform newText = Instantiate(Text2,position + new Vector3(0,2,0),
		                                GameObject.FindGameObjectWithTag("Player").transform.rotation) as Transform;
		
		//change text's msg
		newText.GetComponent<TextMesh>().text = msg;
		
		// Destroy it after it's lifetime
		Destroy(
			newText.gameObject,
			1
			);
	}

}
