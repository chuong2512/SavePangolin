using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBG : MonoBehaviour
{
	public string bgName="Background";

	// Start is called before the first frame update
	void Start()
	{
		var find=transform.Find(bgName).GetComponent<SpriteRenderer>();

		if(find!=null)
			find.color=new Color(1f,1f,1f,1f);
	}

	// Update is called once per frame
	void Update() { }
}
