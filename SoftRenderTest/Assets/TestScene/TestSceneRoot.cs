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

	public SoftRender.Root	sf_root;

	// ================================================================ //
	// MonoBehaviour からの継承.
	
	void	Awake()
	{
		this.pers_camera = Camera.main;

		this.mesh = this.model.GetComponent<MeshFilter>().sharedMesh;

this._3d_material = this.model.GetComponent<MeshRenderer>().material;

		this.sf_root = new SoftRender.Root();
		this.sf_root.setCameras(this.pers_camera, this.ortho_camera);
	}
	
	void	Start()
	{
	}
	
	void	Update()
	{
		List<Vector3>	position_2ds = new List<Vector3>();
		List<float>		depthes = new List<float>();

dbPrint.setLocate(5, 5);

		for(int i = 0;i < this.mesh.vertices.Length;i++) {

			Vector3		position = this.mesh.vertices[i];

			position = this.model.transform.TransformPoint(position);

			float	depth = 0.0f;

			position = this.sf_root.view_mat_pers.MultiplyPoint(position);
			position = this.sf_root.proj_mat_pers.MultiplyPoint(position);

			depth = position.z;

		#if false

			position = this.pers_camera.WorldToViewportPoint(position);
			position.x *= this.ortho_camera.orthographicSize*2.0f*Screen.width/Screen.height;
			position.y *= this.ortho_camera.orthographicSize*2.0f;

			position -= new Vector3(this.ortho_camera.orthographicSize*Screen.width/Screen.height, this.ortho_camera.orthographicSize, 0.0f);

			position = this.ortho_camera.transform.TransformPoint(position);
		#endif

			depthes.Add(depth);

			position.z = this.ortho_camera.nearClipPlane;

dbPrint.print(position.x + " " + position.y + " " + position.z + " " + depth);

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
		for(int i = 0;i < depthes.Count;i++) {
			builder.uvs2.Add(new Vector2(depthes[i], 0.0f));
		}
		for(int i = 0;i < this.mesh.triangles.Length;i++) {
			builder.addIndex(this.mesh.triangles[i]);
		}

		builder.instantiate();

		//



		this.material.SetMatrix("_c_view_matrix", this.sf_root.view_mat_ortho);
		this.material.SetMatrix("_c_proj_matrix", this.sf_root.proj_mat_hybrid);

		//this._3d_material.SetMatrix("_c_view_matrix", c_view_mat_pers);
		//this._3d_material.SetMatrix("_c_proj_matrix", c_proj_mat_pers);

		Graphics.DrawMesh(builder.mesh, Matrix4x4.identity, this.material, LayerMask.NameToLayer("SoftRender 2D"), this.ortho_camera);
		//Graphics.DrawMesh(this.mesh, Matrix4x4.identity, this.material, LayerMask.NameToLayer("Default"), this.pers_camera);
	}
}
