using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneRoot : MonoBehaviour {

	private Camera	pers_camera;

	public Camera	ortho_camera;

	public GameObject	model;

	private Mesh	mesh;

	public float	depth_offset = 0.0f;

	public Material	material;
	public Material	_3d_material;

	// ================================================================ //
	// MonoBehaviour からの継承.
	
	void	Awake()
	{
		this.pers_camera = Camera.main;

		this.mesh = this.model.GetComponent<MeshFilter>().sharedMesh;

this._3d_material = this.model.GetComponent<MeshRenderer>().material;
	}
	
	void	Start()
	{
	}
	
	void	Update()
	{
		//this.pers_camera.

		//dbDraw.get().setModelMatrixTRS(this.model.transform);

		Matrix4x4	c_view_mat_ortho = this.ortho_camera.worldToCameraMatrix;
		Matrix4x4	c_view_mat_pers  = this.pers_camera.worldToCameraMatrix;
		Matrix4x4	c_proj_mat_ortho = this.ortho_camera.projectionMatrix;
		Matrix4x4	c_proj_mat_pers  = this.pers_camera.projectionMatrix;

		c_proj_mat_ortho = TestSceneRoot.convert_projection_matrix(c_proj_mat_ortho);
		c_proj_mat_pers  = TestSceneRoot.convert_projection_matrix(c_proj_mat_pers);

		Matrix4x4	c_proj_mat_hybrid = c_proj_mat_ortho;

		List<Vector3>	position_2ds = new List<Vector3>();

//dbPrint.setLocate(5, 5);

		for(int i = 0;i < this.mesh.vertices.Length;i++) {

			Vector3		position = this.mesh.vertices[i];

			position = this.model.transform.TransformPoint(position);

		#if false
			position = this.pers_camera.WorldToScreenPoint(position);
			position -= new Vector3(Screen.width/2.0f, Screen.height/2.0f, 0.0f);
			position.z = 0.0f;
		#else

			float	depth = this.pers_camera.transform.InverseTransformPoint(position).z;

			position = this.pers_camera.WorldToViewportPoint(position);
			position.x *= this.ortho_camera.orthographicSize*2.0f*Screen.width/Screen.height;
			position.y *= this.ortho_camera.orthographicSize*2.0f;

			position -= new Vector3(this.ortho_camera.orthographicSize*Screen.width/Screen.height, this.ortho_camera.orthographicSize, 0.0f);

			position = this.ortho_camera.transform.TransformPoint(position);
			position.z = this.ortho_camera.nearClipPlane;
			//position.z = depth + this.depth_offset;

//position = c_view_mat_ortho.MultiplyPoint(position);
		#endif

//dbPrint.print(position.x + " " + position.y + " " + position.z);

			position_2ds.Add(position);
		}

	#if false
		{
			for(int i = 0;i < this.mesh.triangles.Length;i += 3) {

				int		index0 = this.mesh.triangles[i + 0];
				int		index1 = this.mesh.triangles[i + 1];
				int		index2 = this.mesh.triangles[i + 2];

				Vector3		position_2ds0 = position_2ds[index0];
				Vector3		position_2ds1 = position_2ds[index1];
				Vector3		position_2ds2 = position_2ds[index2];

				dbDraw.get().drawLine2D(position_2ds0, position_2ds1, Color.red);
				dbDraw.get().drawLine2D(position_2ds1, position_2ds2, Color.red);
				dbDraw.get().drawLine2D(position_2ds2, position_2ds0, Color.red);
			}
			dbDraw.get().setModelMatrixIdentity();
		}
	#endif

		dbDraw.get().drawGridXZ(10.0f, 10.0f, 1.0f);

		BuildMesh.Base	builder = new BuildMesh.Base();

		for(int i = 0;i < position_2ds.Count;i++) {
			builder.positions.Add(position_2ds[i]);
		}
		for(int i = 0;i < this.mesh.uv.Length;i++) {

			Vector3		uv = position_2ds[i];

			uv.x /= this.ortho_camera.orthographicSize*Screen.width/Screen.height;
			uv.y /= this.ortho_camera.orthographicSize;

			builder.uvs.Add(uv);
		}

		for(int i = 0;i < this.mesh.triangles.Length;i++) {
			builder.sub_meshes[0].indices.Add(this.mesh.triangles[i]);
		}

		builder.instantiate();

		//



		this.material.SetMatrix("_c_view_matrix", c_view_mat_ortho);
		this.material.SetMatrix("_c_proj_matrix", c_proj_mat_hybrid);

		//this._3d_material.SetMatrix("_c_view_matrix", c_view_mat_pers);
		//this._3d_material.SetMatrix("_c_proj_matrix", c_proj_mat_pers);

		Graphics.DrawMesh(builder.mesh, Matrix4x4.identity, this.material, LayerMask.NameToLayer("SoftRender 2D"), this.ortho_camera);
		//Graphics.DrawMesh(this.mesh, Matrix4x4.identity, this.material, LayerMask.NameToLayer("Default"), this.pers_camera);
	}
	protected static Matrix4x4		convert_projection_matrix(Matrix4x4 mat)
	{
		if(mat.m33 == 0.0f) {

			mat.m11 = -mat.m11;
			mat.m22 = -(mat.m22 + 1.0f)/2.0f;
			mat.m23 = -mat.m23/2.0f;

		} else {

			mat.m11 = -mat.m11;
			mat.m22 = -mat.m22/2.0f;
			mat.m23 = -(mat.m23 - 1.0f)/2.0f;
		}

		return(mat);
	}

	protected static Matrix4x4	CreateProjectionMatrixPerspective(float fovy, float width_over_height, float znear, float zfar)
	{
		Matrix4x4	matrix = Matrix4x4.identity;

		float		width, height;

		height = Mathf.Tan(Mathf.Deg2Rad*fovy/2.0f);

		width = height*width_over_height;

		matrix.m00 = 1.0f/width;
		matrix.m10 = 0.0f;
		matrix.m20 = 0.0f;
		matrix.m30 = 0.0f;

		matrix.m01 = 0.0f;
		matrix.m11 = 1.0f/height;
matrix.m11 = -1.0f/height;
		matrix.m21 = 0.0f;
		matrix.m31 = 0.0f;

		matrix.m02 = 0.0f;
		matrix.m12 = 0.0f;
		matrix.m22 = -zfar/(zfar - znear);
matrix.m22 = znear/(zfar - znear);
		matrix.m32 = -1.0f;
		//matrix.m22 = -2.0f/(zfar - znear);
		//matrix.m32 =  1.0f;

		matrix.m03 =  0.0f;
		matrix.m13 =  0.0f;
		matrix.m23 = -zfar*znear/(zfar - znear);
matrix.m23 = zfar*znear/(zfar - znear);
		matrix.m33 =  0.0f;
		//matrix.m23 = -(zfar + znear)/(zfar - znear);
		//matrix.m33 =  0.0f;

		return(matrix);
	}
}
