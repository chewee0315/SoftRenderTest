using UnityEngine;
using System.Collections;

public class uiBase : MonoBehaviour {

	// ================================================================ //
	// MonoBehaviour からの継承.

	void	Awake()
	{
	}

	void	Start()
	{
	}
	
	void	Update()
	{
	}

	// ================================================================ //

	// ポジションを取得する.
	public Vector2	getPosition()
	{
		return(this.GetComponent<RectTransform>().localPosition);
	}

	public void		setAngle(float angle)
	{
		this.GetComponent<RectTransform>().localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
