using UnityEngine;
using System.Collections;

namespace Math {

	[System.Serializable]
	public struct Vector2i {

		public Vector2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public int		x;
		public int		y;

		public static Vector2i	roundToInt(Vector3 v3)
		{
			return(new Vector2i(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y)));
		}

		public override string ToString()
		{
			return(this.x.ToString() + "," + this.y.ToString());
		}

		public static bool	operator==(Vector2i v0, Vector2i v1)
		{
			return(v0.Equals(v1));
		}
		public static bool	operator!=(Vector2i v0, Vector2i v1)
		{
			return(!(v0 == v1));
		}

		public override bool Equals(object obj)
		{
			bool	ret = false;

			if(obj is Vector2i) {
				ret = this.Equals((Vector2i)obj);
			}

			return(ret);
		}
		public bool Equals(Vector2i other)
		{
			return((this.x == other.x) && (this.y == other.y));
		}
		public override int GetHashCode()
		{
			return(this.x^this.y);
		}
	}
}

public class MathUtility {

	public static void	swap<T>(ref T x0, ref T x1)
	{
		T	tmp = x0;
		x0 = x1;
		x1 = tmp;
	}

	// 二乗.
	public static int square(int x)
	{
		return(x*x);
	}
	public static float square(float x)
	{
		return(x*x);
	}

	public static Vector3	Vector2toVector3(Vector2 v2, float z = 0.0f)
	{
		return(new Vector3(v2.x, v2.y, z));
	}
	
	// ２点感の距離（XZ成分のみ）を求める.
	public static float		calcDistanceXZ(Vector3 from, Vector3 to)
	{
		Vector3		v = to - from;
		
		v.y = 0.0f;
		
		return(v.magnitude);
	}
	// from から to に向かうベクトルの Y アングルを求める.
	public static float		calcDirection(Vector3 from, Vector3 to)
	{
		Vector3		v = to - from;
		
		float	dir  = Mathf.Atan2(v.x, v.z)*Mathf.Rad2Deg;
		
		dir = MathUtility.snormDegree(dir);
		
		return(dir);
	}
	// v0 と v1 のなす角度を求める（up が回転軸となる方向がプラス）.
	public static float		calcPinch(Vector3 v0, Vector3 v1, Vector3 up)
	{
		float	angle = Vector3.Angle(v0, v1);

		Vector3		vp = Vector3.Cross(v0, v1);

		if(Vector3.Dot(vp, up) < 0.0f) {

			angle *= -1.0f;
		}

		return(angle);
	}

	// degree を -180.0f ～ 180.0f の範囲におさめる.
	public static float		snormDegree(float degree)
	{
		if(degree > 180.0f) {
			
			degree -= 360.0f;
			
		} else if(degree < -180.0f) {
			
			degree += 360.0f;
		}
		
		return(degree);
	}
	
	// degree を 0.0f ～ 360.0f の範囲におさめる.
	public static float		unormDegree(float degree)
	{
		if(degree > 360.0f) {
			
			degree -= 360.0f;
			
		} else if(degree < 0.0f) {
			
			degree += 360.0f;
		}
		
		return(degree);
	}
	
	public static float		remap(float a0, float a1, float x, float b0, float b1)
	{
		return(Mathf.Lerp(b0, b1, Mathf.InverseLerp(a0, a1, x)));
	}

	// 角度（ディグリー）を一定の比率で補間する.
	public static float		lerpDegreeAsymptotic(float cur_angle, float next_angle, float rate, float min_diff)
	{
		float		diff_angle = MathUtility.snormDegree(next_angle - cur_angle);

		if(Mathf.Abs(diff_angle) < min_diff) {

			next_angle = cur_angle;

		} else {

			diff_angle *= rate;

			if(Mathf.Abs(diff_angle) < min_diff) {
				diff_angle = Mathf.Sign(diff_angle)*min_diff;
			}
		
			next_angle = MathUtility.snormDegree(cur_angle + diff_angle);
		}

		return(next_angle);
	}

	// 点から三角形に下ろした垂線の足を求める.
	public static void	calcPerpendicularToTrigon(out Vector3 foot, out float[] param, Vector3 p, Vector3[] trigon)
	{
		Vector3		v0, v1;
		Vector3		p_p1;
		float		s, t;
		float		a, b, c, d, e;

		v0 = trigon[0] - trigon[1];
		v1 = trigon[2] - trigon[1];
		p_p1 =  p - trigon[1];

		a = Vector3.Dot(v0, v0);					// |V0|*|V0|.
		b = Vector3.Dot(v1, v1);					// |V1|*|V1|.
		c = Vector3.Dot(v0, v1);					// V0・V1.
		d = Vector3.Dot(v0, p_p1);					// (P - P0)・V0.
		e = Vector3.Dot(v1, p_p1);					// (P - P0)・V1.
		//

		s = (e*c - d*b)/(c*c - a*b);
		t = (d*c - e*a)/(c*c - a*b);

		// t を t = (d - s*a)/c を使って求めるのは誤差が大きいみたいなので.
		// やめた方がよさそう.

		if(float.IsNaN(s) || float.IsNaN(t)) {

			// V0 と V1 が同じ向きのとき（三角形が直線になっちゃってる）.

			if(Mathf.Abs(a) > Mathf.Abs(b)) {

				// V0 と V1 が平行、または V1 == 0 のとき.
				s = d/a;
				t = 0.0f;

			} else {

				// V0 == 0 のとき.
				s = 0.0f;
				t = e/b;
			}

			if(float.IsNaN(s) || float.IsNaN(t)) {

				// V0 == 0 かつ　V1 == 0 のとき（三角形が点になっちゃってる）.
				s = 0.0f;
				t = 0.0f;
			}
		}


		v0   = v0*s;						// sV0.
		v1   = v1*t;						// tV1.
		foot = v0 + v1 + trigon[1];			// sV0 + tV1 + P1.

		param = new float[2];
		param[0] = s;
		param[1] = t;
	}

	// 二つの直線上の、最短距離を結ぶ点をもとめる.
	public static bool calcBridgeOfTwoLines(out Vector3[] bridge, out float[] param, Vector3[] line0, Vector3[] line1)
	{
		float 		s,t;
		bool		ret;
		Vector3		v0, v1, p0_p1;
		float		a, b, c, d, e;
		
		bridge = new Vector3[2];
		param  = new float[2];
		
		v0    = line0[1] - line0[0];
		v1    = line1[1] - line1[0];
		p0_p1 = line0[0] - line1[0];
		
		a = Vector3.Dot(v0, v0);			// a = |v0|*|v0|
		b = Vector3.Dot(v1, v1);			// b = |v1|*|v1|
		c = Vector3.Dot(v1, v0);			// c = v0・v1
		d = Vector3.Dot(v0, p0_p1);			// d = v0・(p0 - p1)
		e = Vector3.Dot(v1, p0_p1);			// e = v1・(p0 - p1)
		
		
		ret = false;
		
		s = (b*d - c*e)/(c*c - a*b);
		t = (c*d - a*e)/(c*c - a*b);
		
		if(!float.IsInfinity(s) && !float.IsInfinity(t)) {
			
			// Lerp は 0 ～ 1 にクランプされちゃう.			
			//bridge[0] = Vector3.Lerp(line0[0], line0[1], s);
			//bridge[1] = Vector3.Lerp(line1[0], line1[1], t);
			bridge[0] = (1.0f - s)*line0[0] + s*line0[1];
			bridge[1] = (1.0f - t)*line1[0] + t*line1[1];
			
			ret = true;
		}
		
		param[0] = s;
		param[1] = t;
		
		return(ret);
	}

	public static bool	calcPerpendicularToVector(out Vector3 foot, out float param, Vector3 pnt, Vector3 p, Vector3 v)
	{
		bool		ret = false;
		Vector3		tv, f;
		float		s;
	
		foot  = Vector3.zero;
		param = 0.0f;

		do {

			tv = pnt - p;
	
			s = Vector3.Dot(tv, v);
			s /= v.sqrMagnitude;
	
			if(float.IsNaN(s)) {
	
				break;
			}
	
			f = p;
			f += v*s;
	
			foot  = f;
			param = s;
	
			ret = true;
	
		} while(false);
	
		return(ret);
	}

	/// <summary>
	/// ラインと球の交点を求める.
	/// </summary>
	/// <param name="dest_xp">[out] 交点.</param>
	/// <param name="dest_param">[out] パラメーター.</param>
	/// <param name="p">直線の始点.</param>
	/// <param name="v">直線の方向.</param>
	/// <param name="sphere_center">球の中心.</param>
	/// <param name="sphere_radius">球の半径.</param>
	/// <returns>直線と球が交わる？.</returns>
	public static bool	calcIntersectionLineAndSphere(Vector3[] dest_xp, float[] dest_param, Vector3 p, Vector3 v, Vector3 sphere_center, float sphere_radius)
	{
		bool		ret;
		Vector3[]	xp = new Vector3[2];
		float[]		t = new float[2];
		float		a, b, c, d;

		do {

			ret = false;

			//

			a = v.sqrMagnitude;
			b = 2.0f*Vector3.Dot(p - sphere_center, v);
			c = (p - sphere_center).sqrMagnitude - (sphere_radius*sphere_radius);

			d = (b*b) - 4.0f*a*c;

			if(d < 0.0f) {
				break;
			}

			t[0] = (-b - Mathf.Sqrt(d))/(2.0f*a);
			t[1] = (-b + Mathf.Sqrt(d))/(2.0f*a);

			if(float.IsInfinity(t[0]) || float.IsInfinity(t[1])) {
				break;
			}

			xp[0] = p + v*t[0];
			xp[1] = p + v*t[1];

			//

			dest_xp[0] = xp[0];
			dest_xp[1] = xp[1];

			dest_param[0] = t[0];
			dest_param[1] = t[1];

			ret = true;

		} while(false);

		return(ret);
	}

	/********************************************************************/
	/**
	 *
	 * @brief		三角形の面積を計算する
	 *
	 * @param[in]		tri				三角形
	 * @param[in]		is_rectangle	四角形？
	 *
	 * @return							面積
	 *
	 *			is_rectangle が true のときは、四角形（ひし形）の面積を
	 *			計算します。
	 *
	 *			s = |v0||v1|sinθ/2.0f より
	 *
	 *			sinθ = sqrt(1.0f - cosθ^2)
	 *			      = sqrt(1.0f - (v0・v1/|v0||v1|)^2)
	 *
	 *			|v0||v1|sinθ = sqrt(|v0||v1|^2 - (v0・v1)^2)
	 *
	 */
	/********************************************************************/
	public static float	calcTriSpace(Vector3[] tri, bool is_rectangle)
	{
		Vector3		v0, v1;
		float		s;

		v0 = tri[1] - tri[0];
		v1 = tri[2] - tri[1];

		s = v0.sqrMagnitude*v1.sqrMagnitude - Vector3.Dot(v0, v1)*Vector3.Dot(v0, v1);

		s = Mathf.Sqrt(s);

		if(!is_rectangle) {
			s /= 2.0f;
		}

		return(s);
	}

	/********************************************************************/
	/**
	 *
	 * @brief		多角形の法線を求める
	 *
	 * @param[out]		dest_normal		法線
	 * @param[in]		pnt				ポリゴンの頂点
	 * @param[in]		n				ポリゴンの頂点の数
	 *
	 * @return							求められた？
	 *
	 */
	/********************************************************************/
	public static bool	calcPolygonNormal(ref Vector3 dest_normal, Vector3[] pnt)
	{
		bool		ret = false;
		Vector3		candidate = Vector3.zero;
		Vector3		crnt = Vector3.zero;
		bool		has_candidate;

		do {

			if(pnt.Length < 2) {
				break;
			}

			has_candidate = false;

			for(int i = 0;i < pnt.Length - 2;i++) {

				Vector3		v0 = pnt[(i + 1)%pnt.Length] - pnt[(i + 0)%pnt.Length];
				Vector3		v1 = pnt[(i + 2)%pnt.Length] - pnt[(i + 1)%pnt.Length];

				crnt = Vector3.Cross(v0, v1);

				if(has_candidate) {
					if(crnt.sqrMagnitude > candidate.sqrMagnitude) {
						candidate = crnt;
					}
				} else {
					candidate = crnt;
					has_candidate = true;
				}
			}

			if(!has_candidate) {
				break;
			}

			candidate.Normalize();
			if(candidate.magnitude <= 0.0f) {
				break;
			}

			//

			dest_normal = candidate;

			ret = true;

		} while(false);

		return(ret);
	}
}
