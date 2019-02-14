using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringUtility {

	// 文字列 → Vector3.
	public static bool	tryParse(out Vector3 v, string x_str, string y_str, string z_str)
	{
		bool		ret = false;
		Vector3		p = Vector3.zero;

		do {

			if(!float.TryParse(x_str, out p.x)) {

				break;
			}
			if(!float.TryParse(y_str, out p.y)) {

				break;
			}
			if(!float.TryParse(z_str, out p.z)) {

				break;
			}

			ret = true;

		} while(false);

		v = p;

		return(ret);
	}
}