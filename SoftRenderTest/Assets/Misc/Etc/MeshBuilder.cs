using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// メッシュを作る.
namespace BuildMesh {

// 共通のベースクラス.
public class Base {

	public class SubMesh {

		public List<int>		indices  = new List<int>();
		public MeshTopology		topology = MeshTopology.Triangles;
	}

	public Mesh		mesh = null;

	public List<Vector3>	positions  = new List<Vector3>();
	public List<Vector3>	normals    = new List<Vector3>();
	public List<Vector2>	uvs        = new List<Vector2>();
	public List<Vector2>	uvs2       = new List<Vector2>();
	public List<Color>		colors     = new List<Color>();

	public List<SubMesh>	sub_meshes;

	// ================================================================ //

	public Base()
	{
		this.sub_meshes = new List<SubMesh>();
		this.sub_meshes.Add(new SubMesh());
	}

	// ================================================================ //

	// mesh を作る.
	public void		instantiate()
	{
		do {

			if(this.positions.Count == 0) {
				break;
			}
			if(this.sub_meshes.Count == 0) {
				break;
			}

			this.mesh = new Mesh();

			// ---------------------------------------------------------------- //
			

			if(this.positions.Count > 0) {
				mesh.SetVertices(this.positions);
			}
	
			if(this.normals.Count > 0) {
				mesh.SetNormals(this.normals);
			}
	
			if(this.uvs.Count > 0) {
				mesh.SetUVs(0, this.uvs);
			}
	
			if(this.uvs2.Count > 0) {	
				mesh.SetUVs(1, this.uvs2);
			}

			if(this.colors.Count > 0) {				
				mesh.SetColors(this.colors);
			}

			mesh.subMeshCount = this.sub_meshes.Count;

			for(int i = 0;i < this.sub_meshes.Count;i++) {

				SubMesh		sub_mesh = this.sub_meshes[i];

				mesh.SetIndices(sub_mesh.indices.ToArray(), sub_mesh.topology, i);
			}

			mesh.UploadMeshData(true);

		} while(false);
	}

	// ================================================================ //

	// カラーを全部の頂点にセットする.
	public void		setColor(Color color)
	{
		if(this.colors.Count < this.positions.Count) {
			for(int i = this.colors.Count;i < this.positions.Count;i++) {
				this.colors.Add(Color.white);
			}
		}

		for(int i = 0;i < this.colors.Count;i++) {
			this.colors[i] = color;
		}
	}

	public void		createUVS2()
	{
		this.uvs2.Clear();

		foreach(var uv in this.uvs) {

			this.uvs2.Add(uv);
		}
	}

	// インデックスを追加する.
	public void		addIndex(int submesh_index, int index)
	{
		this.sub_meshes[submesh_index].indices.Add(index);
	}
	public void		addIndex(int index)
	{
		this.addIndex(this.sub_meshes.Count - 1, index);
	}

	// インデックスを三つ（三角形）追加する.
	public void		addTriangle(int submesh_index, int index0, int index1, int index2)
	{
		this.sub_meshes[submesh_index].indices.Add(index0);
		this.sub_meshes[submesh_index].indices.Add(index1);
		this.sub_meshes[submesh_index].indices.Add(index2);
	}
	public void		addTriangle(int index0, int index1, int index2)
	{
		this.addTriangle(this.sub_meshes.Count - 1, index0, index1, index2);
	}

	// トランスフォームを適応する.
	public void		applyTransform(Matrix4x4 matrix)
	{
		for(int i = 0;i < this.positions.Count;i++) {
			this.positions[i] = matrix.MultiplyPoint(this.positions[i]);
		}

		for(int i = 0;i < this.normals.Count;i++) {
			this.normals[i] = matrix.MultiplyVector(this.normals[i]);
		}
	}

	public int		calcIndexCount()
	{
		int		index_count = 0;

		foreach(var sub_mesh in this.sub_meshes) {
			index_count += sub_mesh.indices.Count;
		}

		return(index_count);
	}

	// ソリッド（ポリポリした感じ）にする.
	public void		makeSolid()
	{
		int		index_count = this.calcIndexCount();

		List<Vector3>	new_positions = new List<Vector3>(index_count);
		List<Vector3>	new_normals   = new List<Vector3>(index_count);
		List<Color>		new_colors    = new List<Color>(index_count);

		for(int i = 0;i < this.sub_meshes.Count;i++) {

			SubMesh		sub_mesh = this.sub_meshes[i];

			List<int>		new_indices   = new List<int>(sub_mesh.indices.Count);

			int		index_base = new_positions.Count;

			if(sub_mesh.topology == MeshTopology.Triangles) {

				for(int j = 0;j < sub_mesh.indices.Count;j += 3) {

					Vector3		p0 = this.positions[sub_mesh.indices[j]];
					Vector3		p1 = this.positions[sub_mesh.indices[j + 1]];
					Vector3		p2 = this.positions[sub_mesh.indices[j + 2]];

					Vector3		e0 = (p1 - p0).normalized;
					Vector3		e1 = (p2 - p1).normalized;

					Vector3		n = Vector3.Cross(e0, e1).normalized;

					new_indices.Add(index_base + j);
					new_indices.Add(index_base + j + 1);
					new_indices.Add(index_base + j + 2);
					new_positions.Add(p0);
					new_positions.Add(p1);
					new_positions.Add(p2);
					new_normals.Add(n);
					new_normals.Add(n);
					new_normals.Add(n);
					if(this.colors.Count == 0) {
						new_colors.Add(Color.white);
						new_colors.Add(Color.white);
						new_colors.Add(Color.white);
					} else {
						new_colors.Add(this.colors[sub_mesh.indices[j]]);
						new_colors.Add(this.colors[sub_mesh.indices[j + 1]]);
						new_colors.Add(this.colors[sub_mesh.indices[j + 2]]);
					}
				}

			} else {

				for(int j = 0;j < sub_mesh.indices.Count;j++) {

					new_indices.Add(index_base + j);

					new_positions.Add(this.positions[sub_mesh.indices[j]]);

					if(this.colors.Count == 0) {
						new_normals.Add(Vector3.right);
					} else {
						new_normals.Add(this.normals[sub_mesh.indices[j]]);
					}

					if(this.colors.Count == 0) {
						new_colors.Add(Color.white);
					} else {
						new_colors.Add(this.colors[sub_mesh.indices[j]]);
					}
				}
			}

			sub_mesh.indices = new_indices;
		}

		this.positions = new_positions;
		this.normals   = new_normals;
		this.colors    = new_colors;
	}

	// ライティングの結果を頂点カラーに焼きこむ.
	public void		bakeLighitingToVertexColor(Vector3 light_dir, float intensity, float ambient)
	{
		do {

			if(this.normals.Count == 0) {
				break;
			}

			if(this.colors.Count == 0) {
				this.setColor(Color.white);
			}

			Vector3		l = -light_dir.normalized;

			for(int i = 0;i < this.colors.Count;i++) {

				float		dp = Vector3.Dot(this.normals[i], l);

				dp = Mathf.InverseLerp(-1.0f, 1.0f, dp);
				dp = dp*intensity + ambient;

				this.colors[i] = this.colors[i]*dp;
				this.colors[i] = new Color(this.colors[i].r, this.colors[i].g, this.colors[i].b, 1.0f);
			}

		} while(false);
	}

	public void		clearSubMeshes()
	{
		this.sub_meshes.Clear();
	}

	public void		appendSubMesh(Base source)
	{
		SubMesh		sub_mesh = new SubMesh();

		SubMesh		source_sub_mesh = source.sub_meshes[0];

		sub_mesh.topology = source_sub_mesh.topology;

		//

		int		index_base = this.positions.Count;

		this.positions.AddRange(source.positions);

		if(source.normals.Count > 0) {
			this.normals.AddRange(source.normals);
		} else {
			for(int j = 0;j < source.positions.Count;j++) {
				this.normals.Add(Vector3.right);
			}
		}

		if(source.colors.Count > 0) {
			this.colors.AddRange(source.colors);
		} else {
			for(int j = 0;j < source.positions.Count;j++) {
				this.colors.Add(Color.white);
			}
		}

		foreach(var index in source_sub_mesh.indices) {
			sub_mesh.indices.Add(index_base + index);
		}

		this.sub_meshes.Add(sub_mesh);
	}

	// ================================================================ //

	public static Base	merge(params Base[] sources)
	{
		Base	merged = new Base();

		for(int i = 0;i < sources.Length;i++) {

			Base	source = sources[i];

			int		index_base = merged.positions.Count;

			merged.positions.AddRange(source.positions);
			merged.normals.AddRange(source.normals);
			merged.colors.AddRange(source.colors);

			foreach(var sub_mesh in source.sub_meshes) {
				foreach(var index in sub_mesh.indices) {
					merged.sub_meshes[0].indices.Add(index_base + index);
				}
			}
		}

		return(merged);
	}

	// ================================================================ //

	public void		create_plane(int division_x, int division_z, Matrix4x4 matrix)
	{
		Vector3		position;
		Vector2		uv;
		int			index_start = this.positions.Count;

		position.y = 0.0f;

		for(int i = 0;i < division_z + 1;i++) {

			position.z = Mathf.Lerp(0.5f, -0.5f, (float)i/(float)division_z);

			for(int j = 0;j < division_x + 1;j++) {

				position.x = Mathf.Lerp(-0.5f, 0.5f, (float)j/(float)division_x);

				this.positions.Add(matrix.MultiplyPoint(position));
				this.normals.Add(matrix.MultiplyVector(Vector3.up));

				uv.x = Mathf.Lerp(0.0f, 1.0f, Mathf.InverseLerp(-0.5f,  0.5f, position.x));
				uv.y = Mathf.Lerp(0.0f, 1.0f, Mathf.InverseLerp( 0.5f, -0.5f, position.z));

				this.uvs.Add(uv);
			}
		}

		//

		for(int z = 0;z < division_z;z++) {

			for(int x = 0;x < division_x;x++) {

				int		index0 = z*(division_x + 1)       + x;
				int		index1 = z*(division_x + 1)       + (x + 1);
				int		index2 = (z + 1)*(division_x + 1) + (x + 1);
				int		index3 = (z + 1)*(division_x + 1) + x;

				this.sub_meshes[0].indices.Add(index_start + index0);
				this.sub_meshes[0].indices.Add(index_start + index1);
				this.sub_meshes[0].indices.Add(index_start + index2);

				this.sub_meshes[0].indices.Add(index_start + index2);
				this.sub_meshes[0].indices.Add(index_start + index3);
				this.sub_meshes[0].indices.Add(index_start + index0);
			}
		}
	}

	// ================================================================ //

	// デバッグ用　ワイヤーフレーム描画.
	public void		debugDrawAsWire(Color color)
	{
		foreach(SubMesh sub_mesh in this.sub_meshes) {

			for(int i = 0;i < sub_mesh.indices.Count;i += 3) {

				int		index0 = sub_mesh.indices[i + 0];
				int		index1 = sub_mesh.indices[i + 1];
				int		index2 = sub_mesh.indices[i + 2];

				Vector3		p0 = this.positions[index0];
				Vector3		p1 = this.positions[index1];
				Vector3		p2 = this.positions[index2];

				dbDraw.get().drawLine(p0, p1, color);
				dbDraw.get().drawLine(p1, p2, color);
				dbDraw.get().drawLine(p2, p0, color);
			}
		}
	}
};
// ======================================================================== //
// 四角形(XY).																//
// ======================================================================== //
public class Quad : Base {

	// 頂点のインデックス.
	public enum VERTEX {

		NONE = -1,

		RT = 0,		// 右上.
		LT,			// 左上.
		RB,			// 右下.
		LB,			// 左下.

		NUM
	};

	// ================================================================ //

	public Quad()
	{
		for(int i = 0;i < (int)VERTEX.NUM;i++) {

			this.positions.Add(Vector3.zero);
			this.normals.Add(Vector3.forward);
			this.uvs.Add(Vector2.one);
			this.colors.Add(Color.red);
		}

		// ---------------------------------------------------------------- //

		this.createVertices(1.0f, 1.0f);

		// ---------------------------------------------------------------- //
		// インデックス.

		// LT--RT
		// |   |
		// LB--RB
		//			
		this.sub_meshes[0].indices.Add((int)VERTEX.RT);
		this.sub_meshes[0].indices.Add((int)VERTEX.RB);
		this.sub_meshes[0].indices.Add((int)VERTEX.LT);

		this.sub_meshes[0].indices.Add((int)VERTEX.RB);
		this.sub_meshes[0].indices.Add((int)VERTEX.LB);
		this.sub_meshes[0].indices.Add((int)VERTEX.LT);
	}

	public void		createVertices(float width, float height)
	{
		this.positions[(int)VERTEX.RT] = new Vector3( width/2.0f,  height/2.0f, 0.0f);
		this.positions[(int)VERTEX.LT] = new Vector3(-width/2.0f,  height/2.0f, 0.0f);
		this.positions[(int)VERTEX.RB] = new Vector3( width/2.0f, -height/2.0f, 0.0f);
		this.positions[(int)VERTEX.LB] = new Vector3(-width/2.0f, -height/2.0f, 0.0f);

		this.uvs[(int)VERTEX.RT] = new Vector2(1.0f, 1.0f);
		this.uvs[(int)VERTEX.LT] = new Vector2(0.0f, 1.0f);
		this.uvs[(int)VERTEX.RB] = new Vector2(1.0f, 0.0f);
		this.uvs[(int)VERTEX.LB] = new Vector2(0.0f, 0.0f);
	}
}
// ======================================================================== //
// キューブ.																//
// ======================================================================== //
public class Cube : Base {

	public enum Face {

		None = -1,

		PX = 0,			// 右.
		NX,				// 左.
		PY,				// 上.
		NY,				// 下.
		PZ,				// 前.
		NZ,				// 奥.

		Num,
	};

	public int	division = 1;

	public void	createVertices(int division)
	{
		this.division = division;

		Vector3[]	rotations = new Vector3[(int)Face.Num] {

			new Vector3(0.0f, 0.0f,  -90.0f),		// PX = 0,			//!< 右
			new Vector3(0.0f, 0.0f,   90.0f),		// NX,				//!< 左

			new Vector3(  0.0f, 0.0f, 0.0f),		// PY,				//!< 上
			new Vector3(180.0f, 0.0f, 0.0f),		// NY,				//!< 下

			new Vector3( 90.0f, 0.0f, 0.0f),		// PZ,				//!< 前
			new Vector3(-90.0f, 0.0f, 0.0f),		// Z,				//!< 奥
		};

		Matrix4x4	m;

		for(int i = 0;i < (int)Face.Num;i++) {

			m = Matrix4x4.identity;
			m *= Matrix4x4.TRS(Vector3.zero,    Quaternion.Euler(rotations[i]), Vector3.one);
			m *= Matrix4x4.TRS(Vector3.up*0.5f, Quaternion.identity,            Vector3.one);

			this.create_plane(this.division, this.division, m);
		}

		for(int i = 0;i < this.positions.Count;i++) {

			this.colors.Add(Color.red);
		}
	}
	public void		setUVs(Face face, Vector2 uv_lt, Vector2 uv_rb, float angle)
	{
		Vector2		uv;
		Vector2		cnt = (uv_lt + uv_rb)/2.0f;

		float		cos = Mathf.Cos(angle*Mathf.Deg2Rad);
		float		sin = Mathf.Sin(angle*Mathf.Deg2Rad);

		int		st = (int)face*MathUtility.square(this.division + 1);

		for(int i = 0;i < this.division + 1;i++) {

			for(int j = 0;j < this.division + 1;j++) {

				uv.x = Mathf.Lerp(uv_lt.x, uv_rb.x, (float)i/(float)this.division);
				uv.y = Mathf.Lerp(uv_lt.y, uv_rb.y, (float)j/(float)this.division);

				uv -= cnt;
				uv = new Vector2(uv.x*cos - uv.y*sin, uv.x*sin + uv.y*cos);
				uv += cnt;

				this.uvs[st + j*(this.division + 1) + i] = uv;
			}
		}
	}

	public int		getVertexNumInFace()
	{
		return(MathUtility.square(this.division + 1));
	}

	public Face		getFaceFromVertexIndex(int vertex_index)
	{
		Face	face = (Face)(vertex_index/this.getVertexNumInFace());

		return(face);
	}

	Vector3		getFaceNormal(Face face)
	{
		Vector3		normal;

		switch(face) {

			case Face.PX:	normal = new Vector3( 1.0f,  0.0f,  0.0f);	break;
			case Face.NX:	normal = new Vector3(-1.0f,  0.0f,  0.0f);	break;
			case Face.PY:	normal = new Vector3( 0.0f,  1.0f,  0.0f);	break;
			case Face.NY:	normal = new Vector3( 0.0f, -1.0f,  0.0f);	break;
			case Face.PZ:	normal = new Vector3( 0.0f,  0.0f,  1.0f);	break;
			case Face.NZ:	normal = new Vector3( 0.0f,  0.0f, -1.0f);	break;

			default:		normal = new Vector3(1.0f, 0.0f, 0.0f);		break;
		}

		return(normal);
	}

	// 球面（Ｙ軸を中心とした９０度四方）の四隅に向かうベクトル.
	protected struct SphereizeRadius {

		public Vector3	ln;			// left near.
		public Vector3	rn;			// right near.
		public Vector3	lf;			// left far.
		public Vector3	rf;			// right far.

		public Vector3	n, f;
	};

	// 頂点の移動だけで、球に変形する.
	public void		spherize()
	{
		Vector3			ratio;
		Math.TRS		trs = Math.TRS.identity();
		Math.TRS		trs_inv = trs.inverse();

		// 球面（Ｙ軸を中心とした９０度四方）の四隅に向かうベクトル.
		SphereizeRadius		radius;

		radius.ln = new Vector3(-0.5f, 0.5f,  0.5f);
		radius.rn = new Vector3( 0.5f, 0.5f,  0.5f);
		radius.lf = new Vector3(-0.5f, 0.5f, -0.5f);
		radius.rf = new Vector3( 0.5f, 0.5f, -0.5f);

		radius.ln.Normalize();
		radius.rn.Normalize();
		radius.lf.Normalize();
		radius.rf.Normalize();

		Face	current_face = Face.None;
		Face	last_face = Face.None;

		for(int i = 0;i < this.positions.Count;i++) {

			// 頂点が属する立方体の面.
			//
			current_face = this.getFaceFromVertexIndex(i);

			// 面がかわったらマトリックスを作り直す.
			//
			if(current_face != last_face) {

				// face.current を FACE_PY と同じ向きに回転させるマトリックスを
				// 求める.

				Vector3		face_normal;
				Vector3		pivot;
				float		angle;

				face_normal = this.getFaceNormal(current_face);

				pivot = Vector3.Cross(face_normal, Vector3.up);
				angle = Vector3.Angle(face_normal, Vector3.up);

				pivot.Normalize();

				if(pivot.magnitude == 0.0f) {

					if(Vector3.Dot(face_normal, Vector3.up) > 0.0f) {

						// Ｙ軸と同じ向き
						pivot = Vector3.right;
						angle = 0.0f;

					} else {

						// Ｙ軸と１８０度反対向き（－Ｙ）
						pivot = Vector3.right;
						angle = 180.0f;
					}
				}

				trs = Math.TRS.identity();
				trs.rotation = Quaternion.AngleAxis(angle, pivot);
				trs_inv = trs.inverse();

				last_face = current_face;
			}

			//
			this.positions[i] = trs.transformPosition(this.positions[i]);
			this.normals[i]   = trs.transformVector(this.normals[i]);

			// キューブ上の XZ 座標の比率＝球面を分割する比率になるようにする

			ratio.x = this.positions[i].x + 0.5f;
			ratio.z = this.positions[i].z + 0.5f;

			// 球面（Ｙ軸を中心とした９０度四方）の四隅に向かうベクトルを、
			// 球面補間する

			radius.n = Cube.slerpVector3(radius.ln, radius.rn, ratio.x);
			radius.f = Cube.slerpVector3(radius.lf, radius.rf, ratio.x);

			this.positions[i] = Cube.slerpVector3(radius.f, radius.n, ratio.z);

			this.positions[i] = this.positions[i].normalized*0.5f;
			this.normals[i]   = this.positions[i].normalized;

			this.positions[i] = trs_inv.transformPosition(this.positions[i]);
			this.normals[i]   = trs_inv.transformVector(this.normals[i]);
		}
	}
	public static Vector3	slerpVector3(Vector3 v0, Vector3 v1, float ratio)
	{
		float		angle = Vector3.Angle(v0, v1)*Mathf.Deg2Rad;
		float		length = Mathf.Lerp(v0.magnitude, v1.magnitude, ratio);

		Vector3		dest = Vector3.RotateTowards(v0, v1, ratio*angle, length);

		return(dest);
	}

}

// ======================================================================== //
// N 角すい.																//
// ======================================================================== //
public class Cone : Base {

	public int	division = 4;

	public void	createVertices(float radius, float height, int division)
	{
		this.division = division;

		this.positions.Add(Vector3.zero);
		this.normals.Add(Vector3.down);
		this.positions.Add(Vector3.up*height);
		this.normals.Add(Vector3.up);

		for(int i = 0;i < this.division;i++) {

			float	angle = -360.0f*(float)i/(float)this.division;

			Vector3		p = Quaternion.Euler(0.0f, angle, 0.0f)*Vector3.right*radius;

			this.positions.Add(p);
			this.normals.Add(p.normalized);
		}

		for(int i = 0;i < this.division;i++) {

			this.sub_meshes[0].indices.Add(0);
			this.sub_meshes[0].indices.Add(2 + i);
			this.sub_meshes[0].indices.Add(2 + (i + 1)%this.division);
		}

		for(int i = 0;i < this.division;i++) {

			this.sub_meshes[0].indices.Add(1);
			this.sub_meshes[0].indices.Add(2 + (i + 1)%this.division);
			this.sub_meshes[0].indices.Add(2 + i);
		}
	}
}

// ======================================================================== //
// ラインいっぱい.															//
// ======================================================================== //
public class Lines : Base {

	public Lines()
	{
		this.sub_meshes[0].topology = MeshTopology.Lines;
	}

	public void	addLine(Vector3 position0, Vector3 position1, Color color)
	{
		this.sub_meshes[0].indices.Add(this.positions.Count);
		this.sub_meshes[0].indices.Add(this.positions.Count + 1);

		this.positions.Add(position0);
		this.positions.Add(position1);

		this.colors.Add(color);
		this.colors.Add(color);
	}
}
// ======================================================================== //
// ワイヤーフレームの円(XY).																//
// ======================================================================== //
public class WireCircle : Base {

	public int		division = 16;
	public float	radius = 0.5f;

	public float	start_angle = 0.0f;
	public float	end_angle = 360.0f;

	// ================================================================ //

	public WireCircle()
	{
		this.sub_meshes[0].topology = MeshTopology.LineStrip;
	}

	public void	setArcAngle(float start_angle, float end_angle)
	{
		this.start_angle = start_angle;
		this.end_angle   = end_angle;
	}

	public void	createVertices(float radius, int division = -1)
	{
		this.radius = radius;

		if(division >= 2) {
			this.division = division;
		}

		for(int i = 0;i <= this.division;i++) {

			float	angle = Mathf.Lerp(this.start_angle, this.end_angle, (float)i/(float)this.division);

			this.positions.Add(Quaternion.AngleAxis(angle, Vector3.forward)*Vector3.right*this.radius);
			this.colors.Add(Color.white);

		}

		// ---------------------------------------------------------------- //
		// インデックス.

		for(int i = 0;i <= this.division;i++) {
			this.sub_meshes[0].indices.Add(i);
		}
	}
}


// ======================================================================== //
//																			//
// ======================================================================== //

} // namespace BuildMesh