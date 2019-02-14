using UnityEngine;
using System.Collections;

namespace Math {

public struct TRS {

	public Vector3		position;
	public Quaternion	rotation;

	public static TRS	identity()
	{
		return(new TRS(Vector3.zero, Quaternion.identity));
	}

	// -------------------------------------------------------- //

	public TRS(Vector3 position, Quaternion	rotation)
	{
		this.position = position;
		this.rotation = rotation;
	}

	public void		set(Vector3 position, Quaternion rotation)
	{
		this.position = position;
		this.rotation = rotation;
	}

	// -------------------------------------------------------- //

	public Vector3	transformPosition(Vector3 position)
	{
		Vector3	p = this.rotation*position;

		p += this.position;

		return(p);
	}
	public Vector3	transformVector(Vector3 vector)
	{
		Vector3	p = this.rotation*vector;

		return(p);
	}

	public TRS		inverse()
	{
		TRS		inv_trs = this;

		inv_trs.rotation = Quaternion.Inverse(this.rotation);
		inv_trs.position = inv_trs.rotation*(-this.position);

		return(inv_trs);
	}

	public void		identitySelf()
	{
		this = TRS.identity();
	}

	public void		multiplySelf(TRS other)
	{
		this.position += this.rotation*other.position;
		this.rotation *= other.rotation;
	}

	// -------------------------------------------------------- //

	public static TRS	operator*(TRS x0, TRS x1)
	{
		return(new TRS(x0.position + x0.rotation*x1.position, x0.rotation*x1.rotation));
	}

	// -------------------------------------------------------- //

	public override string ToString()
	{
		return(this.position.ToString() + " " + this.rotation.ToString());
	}

	public string ToStringPositionAxises()
	{
		string		str = this.position.ToString();
		str += " ";

		str += this.transformVector(Vector3.right) + " ";
		str += this.transformVector(Vector3.up) + " ";
		str += this.transformVector(Vector3.forward);

		return(str);
	}
}

}