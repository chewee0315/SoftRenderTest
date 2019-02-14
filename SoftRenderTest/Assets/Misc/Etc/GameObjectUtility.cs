using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ---------------------------------------------------------------- //


// ================================================================ //
//																	//
// ================================================================ //
public class GameObjectUtility {

	public static GameObject	findChild(GameObject go, string name)
	{
		GameObject	target = null;

		if(go.name == name) {

			target = go;

		} else {

			for(int i = 0;i < go.transform.childCount;i++) {

				GameObject	child_go = go.transform.GetChild(i).gameObject;

				target = findChild(child_go, name);

				if(target != null) {
					break;
				}
			}
		}

		return(target);
	}
}



// ---------------------------------------------------------------- //




