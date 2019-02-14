using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mpiObjectHandle : MonoBehaviour {

	public enum Mode {

		None = -1,
		Move = 0,
		Rotate,

		Character,			// XZ 平行移動と Y 軸回転.
		Num,
	}
	public Mode		mode = Mode.Move;

	public enum STEP {
		
		None = -1,
		
		Idle = 0,			// ドラッグしてない中.
		Grabbed,			// ドラッグしてる中.
	};
	public Step<STEP>	step = new Step<STEP>(STEP.None);

	public const float	rotate_ring_radious = 0.15f;

	// -------------------------------------------------------- //

	// ドラッグ中？.
	protected bool			is_now_dragging = false;

	protected Camera		main_camera;

	protected Vector3		grab_offset = Vector3.zero;

	protected Vector3		grab_mouse_pos  = Vector3.zero;

	public enum GrabbedAxis {

		None = -1,
		X = 0,
		Y,
		Z,

		Num,
	}
	protected GrabbedAxis	grabbed_axis = GrabbedAxis.None;
	protected Vector3		knob_pos_x;
	protected Vector3		knob_pos_y;
	protected Vector3		knob_pos_z;

	protected struct AxisMask {

		public bool		x;
		public bool		y;
		public bool		z;

		public void		set_all(bool v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}
	}
	protected AxisMask	move_mode_axis_mask   = new AxisMask();
	protected AxisMask	rotate_mode_axis_mask = new AxisMask();

	protected float		scale = 1.0f;

	protected static Mesh			s_mesh;
	protected static Material		s_material;

	protected static bool	s_initialized_statics = false;

	public enum SubMeshIndex {

		None = -1,

		X_Hat = 0,
		X_Line,
		Y_Hat,
		Y_Line,
		Z_Hat,
		Z_Line,

		X_Arc,
		Y_Arc,
		Z_Arc,

		Circle,

		Num,
	}

	// ================================================================ //

	void	Awake()
	{
		this.main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		if(!mpiObjectHandle.s_initialized_statics) {
			mpiObjectHandle.initializeStatics();
			mpiObjectHandle.s_initialized_statics = true;
		}

		this.move_mode_axis_mask.set_all(true);
		this.rotate_mode_axis_mask.set_all(true);
	}

	void	Start()
	{
		this.step.set_next(STEP.Idle);
	}

	void	LateUpdate()
	{

		// ---------------------------------------------------------------- //

		float		base_scale = mpiObjectHandle.rotate_ring_radious;

		Vector3		cam_local_pos = this.main_camera.transform.InverseTransformPoint(this.transform.position);

		this.scale = base_scale*Mathf.Abs(cam_local_pos.z)/1.0f;

		this.knob_pos_x = this.transform.TransformPoint(Vector3.right*this.scale);
		this.knob_pos_y = this.transform.TransformPoint(Vector3.up*this.scale);
		this.knob_pos_z = this.transform.TransformPoint(Vector3.forward*this.scale);

		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		
		switch(this.step.do_transition()) {
			
			case STEP.Idle:
			{
#if false
				this.mode = Mode.Character;
#else
				if(Input.GetKeyDown(KeyCode.M)) {

					int		mode_as_int = (int)this.mode;

					mode_as_int = (mode_as_int + 1)%(int)Mode.Num;

					this.mode = (Mode)mode_as_int; 
				}
				if(Input.GetKeyDown(KeyCode.R)) {

					this.transform.localRotation = Quaternion.identity;
				}
#endif
				switch(this.mode) {

					case Mode.Move:
					{
						 this.move_mode_axis_mask.set_all(true);
					}
					break;
					case Mode.Rotate:
					{
						 this.rotate_mode_axis_mask.set_all(true);
					}
					break;
					case Mode.Character:
					{
						 this.move_mode_axis_mask.x = true;
						 this.move_mode_axis_mask.y = false;
						 this.move_mode_axis_mask.z = true;

						 this.rotate_mode_axis_mask.x = false;
						 this.rotate_mode_axis_mask.y = true;
						 this.rotate_mode_axis_mask.z = false;
					}
					break;
				}

				do {

					if(!Input.GetMouseButtonDown(0)) {
						break;
					}

					switch(this.mode) {
						case Mode.Move:			this.do_transition_move();		break;
						case Mode.Rotate:		this.do_transition_rotate();	break;
						case Mode.Character:	this.do_transition_character();	break;
					}

					if(!step.is_has_next()) {
						this.grabbed_axis = GrabbedAxis.None;
					}

				} while(false);
			}
			break;

			case STEP.Grabbed:
			{
				if(!Input.GetMouseButton(0)) {
					this.step.set_next(STEP.Idle);
				}
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.step.get_next() != STEP.None) {
			
			switch(this.step.do_initialize()) {
				
				case STEP.Idle:
				{
				}
				break;

				case STEP.Grabbed:
				{

				}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step.do_execution(Time.deltaTime)) {
			
			case STEP.Idle:
			{
			}
			break;

			case STEP.Grabbed:
			{
				switch(this.mode) {
					case Mode.Move:			this.execute_move();		break;
					case Mode.Rotate:		this.execute_rotate();		break;
					case Mode.Character:	this.execute_character();	break;
				}
				//dbPrint.print3d(this.transform.position, this.transform.position.ToString());
			}
			break;
		}

		// ---------------------------------------------------------------- //

		this.draw();
	}

	// ================================================================ //

	protected bool	do_transition_move()
	{
		bool	is_transit = false;

		do {

			Ray		ray = this.main_camera.ScreenPointToRay(Input.mousePosition);

			Vector3[]	line0_x  = new Vector3[2];
			Vector3[]	line0_y  = new Vector3[2];
			Vector3[]	line0_z  = new Vector3[2];
			Vector3[]	line1    = new Vector3[2];

			Vector3[]	bridge_x = new Vector3[2];
			Vector3[]	bridge_y = new Vector3[2];
			Vector3[]	bridge_z = new Vector3[2];
			float[]		param    = new float[2];

			float		dist_x = float.PositiveInfinity;
			float		dist_y = float.PositiveInfinity;
			float		dist_z = float.PositiveInfinity;

			AxisMask	valid_axis = new AxisMask();

			valid_axis.set_all(false);

			line0_x[0] = this.transform.position;
			line0_x[1] = this.transform.position + this.transform.right;
			line0_y[0] = this.transform.position;
			line0_y[1] = this.transform.position + this.transform.up;
			line0_z[0] = this.transform.position;
			line0_z[1] = this.transform.position + this.transform.forward;
			line1[0] = ray.origin;
			line1[1] = ray.origin + ray.direction;

			do {

				if(!MathUtility.calcBridgeOfTwoLines(out bridge_x, out param, line0_x, line1)) {
					break;
				}
				if(param[0] < 0.0f || this.scale*1.5f <= param[0]) {
					break;
				}

				dist_x = Vector3.Distance(bridge_x[0], bridge_x[1]);
				valid_axis.x = true;

			} while(false);

			do {

				valid_axis.y = false;

				if(!MathUtility.calcBridgeOfTwoLines(out bridge_y, out param, line0_y, line1)) {
					break;
				}
				if(param[0] < 0.0f || this.scale*1.5f <= param[0]) {
					break;
				}

				dist_y = Vector3.Distance(bridge_y[0], bridge_y[1]);
				valid_axis.y = true;

			} while(false);

			do {

				valid_axis.z = false;

				if(!MathUtility.calcBridgeOfTwoLines(out bridge_z, out param, line0_z, line1)) {
					break;
				}
				if(param[0] < 0.0f || this.scale*1.5f <= param[0]) {
					break;
				}

				dist_z = Vector3.Distance(bridge_z[0], bridge_z[1]);
				valid_axis.z = true;

			} while(false);

			//

			GrabbedAxis		grabbed_axis = GrabbedAxis.None;
			float			dist_min = float.PositiveInfinity;

			mpiObjectHandle.select_axis(out grabbed_axis, out dist_min, dist_x, dist_y, dist_z, this.move_mode_axis_mask);

			if(dist_min/this.scale > 0.2f) {
				break;
			}

			this.grabbed_axis = grabbed_axis;
			switch(this.grabbed_axis) {
				case GrabbedAxis.X:	 this.grab_offset = bridge_x[0] - this.transform.position;	break;
				case GrabbedAxis.Y:	 this.grab_offset = bridge_y[0] - this.transform.position;	break;
				case GrabbedAxis.Z:	 this.grab_offset = bridge_z[0] - this.transform.position;	break;
			}

			//

			this.step.set_next(STEP.Grabbed);

			is_transit = true;

		} while(false);

		return(is_transit);
	}

	protected void	execute_move()
	{
		do {

			Ray		ray = this.main_camera.ScreenPointToRay(Input.mousePosition);

			Vector3[]	line0  = new Vector3[2];
			Vector3[]	line1  = new Vector3[2];
			Vector3[]	bridge = new Vector3[2];
			float[]		param  = new float[2];

			line0[0] = this.transform.position;
			line0[1] = this.transform.position;
			line1[0] = ray.origin;
			line1[1] = ray.origin + ray.direction;

			switch(this.grabbed_axis) {
				case GrabbedAxis.X:	 line0[1] += this.transform.right;		break;
				case GrabbedAxis.Y:	 line0[1] += this.transform.up;			break;
				case GrabbedAxis.Z:	 line0[1] += this.transform.forward;	break;
			}

			if(!MathUtility.calcBridgeOfTwoLines(out bridge, out param, line0, line1)) {
				break;
			}

			this.transform.position = bridge[0] - this.grab_offset;

			dbDraw.get().setModelMatrixTRS(bridge[0], Quaternion.identity, Vector3.one);
			dbDraw.get().drawWireSphere(0.01f, Color.white, 2.0f);

		} while(false);
	}

	// ---------------------------------------------------------------- //

	protected Vector3		grab_tangent = Vector3.right;
	protected Quaternion	grab_rotate  = Quaternion.identity;

	protected bool	do_transition_rotate()
	{
		bool	is_transit = false;

		do {

			Ray		ray = this.main_camera.ScreenPointToRay(Input.mousePosition);

			Vector3[]	xp = new Vector3[2];
			float[]		param = new float[2];

			if(!MathUtility.calcIntersectionLineAndSphere(xp, param, ray.origin, ray.direction, this.transform.position, this.scale*1.01f)) {
				break;
			}

			Vector3		xp_l = this.transform.InverseTransformPoint(xp[0]);

			xp_l.Normalize();

			float	dp_x = Vector3.Dot(xp_l, Vector3.right);
			float	dp_y = Vector3.Dot(xp_l, Vector3.up);
			float	dp_z = Vector3.Dot(xp_l, Vector3.forward);

			dp_x = Mathf.Abs(dp_x);
			dp_y = Mathf.Abs(dp_y);
			dp_z = Mathf.Abs(dp_z);

			GrabbedAxis		grabbed_axis = GrabbedAxis.None;
			float			dp_min = float.PositiveInfinity;

			if(dp_x <= dp_y) {
				if(dp_x <= dp_z) {
					grabbed_axis = GrabbedAxis.X;
					dp_min = dp_x;
				} else {
					grabbed_axis = GrabbedAxis.Z;
					dp_min = dp_z;
				}
			} else {
				if(dp_y <= dp_z) {
					grabbed_axis = GrabbedAxis.Y;
					dp_min = dp_y;
				} else {
					grabbed_axis = GrabbedAxis.Z;
					dp_min = dp_z;
				}
			}

			this.grabbed_axis = grabbed_axis;

			//

			this.grab_mouse_pos = Input.mousePosition;

			Vector3		radius = xp_l;
			Vector3		axis = Vector3.right;

			switch(this.grabbed_axis) {

				case GrabbedAxis.X:		radius.x = 0.0f;axis = Vector3.right;	break;
				case GrabbedAxis.Y:		radius.y = 0.0f;axis = Vector3.up;		break;
				case GrabbedAxis.Z:		radius.z = 0.0f;axis = Vector3.forward;	break;
			}
			radius.Normalize();

			this.grab_tangent = Vector3.Cross(axis, radius);
			this.grab_tangent.Normalize();

			this.grab_rotate = this.transform.rotation;

			this.step.set_next(STEP.Grabbed);

			is_transit = true;

		} while(false);

		return(is_transit);
	}

	protected void	execute_rotate()
	{

		do {

			Vector3		normal = (this.main_camera.transform.position - this.transform.position).normalized;
			Plane		plane = new Plane(normal, this.transform.position);

			Ray			ray = this.main_camera.ScreenPointToRay(Input.mousePosition);
			float		depth;

			if(!plane.Raycast(ray, out depth)) {
				break;
			}

			Vector3		xp_3d0 = ray.origin + ray.direction*depth;

			ray = this.main_camera.ScreenPointToRay(this.grab_mouse_pos);

			if(!plane.Raycast(ray, out depth)) {
				break;
			}

			Vector3		xp_3d1 = ray.origin + ray.direction*depth;
			this.transform.rotation = this.grab_rotate;

			Vector3		move_vector = this.transform.InverseTransformVector(xp_3d0 - xp_3d1);

			Vector3		axis = Vector3.right;

			switch(this.grabbed_axis) {

				case GrabbedAxis.X:	axis = Vector3.right;	break;
				case GrabbedAxis.Y:	axis = Vector3.up;		break;
				case GrabbedAxis.Z:	axis = Vector3.forward;	break;
			}

			move_vector = move_vector - axis*Vector3.Dot(move_vector, axis);

			float	angle = move_vector.magnitude/this.scale*Mathf.Rad2Deg;

			if(Vector3.Dot(move_vector, this.grab_tangent) < 0.0f) {
				angle = -angle;
			}

			this.transform.rotation = this.grab_rotate;
			this.transform.Rotate(axis, angle);

		} while(false);
	}


	// ---------------------------------------------------------------- //

	protected void	do_transition_character()
	{
		do {

			if(this.do_transition_move()) {
				break;
			}

			if(this.do_transition_rotate()) {
				break;
			}

		} while(false);
	}

	protected void	execute_character()
	{
		switch(this.grabbed_axis) {

			case GrabbedAxis.X:
			case GrabbedAxis.Z:	this.execute_move();	break;

			case GrabbedAxis.Y:	this.execute_rotate();	break;
		}
	}

	// ================================================================ //

	protected static void	select_axis(out GrabbedAxis axis, out float dist, float dist_x, float dist_y, float dist_z, AxisMask mask)
	{
		axis = GrabbedAxis.None;
		dist = float.PositiveInfinity;

		if(!mask.x) {
			dist_x = float.PositiveInfinity;
		}
		if(!mask.y) {
			dist_y = float.PositiveInfinity;
		}
		if(!mask.z) {
			dist_z = float.PositiveInfinity;
		}

		if(dist_x <= dist_y) {
			if(dist_x <= dist_z) {
				axis = GrabbedAxis.X;
				dist = dist_x;
			} else {
				axis = GrabbedAxis.Z;
				dist = dist_z;
			}
		} else {
			if(dist_y <= dist_z) {
				axis = GrabbedAxis.Y;
				dist = dist_y;
			} else {
				axis = GrabbedAxis.Z;
				dist = dist_z;
			}
		}
	}

	// ================================================================ //
	// 描画.

	protected void		draw()
	{
		Matrix4x4	matrix = Matrix4x4.TRS(this.transform.position, this.transform.rotation, Vector3.one*this.scale);

		MaterialPropertyBlock	prop = new MaterialPropertyBlock();

		Camera		camera = Camera.main;

		switch(this.mode) {

			case Mode.Move:			this.draw_move(matrix, prop, camera);		break;
			case Mode.Rotate:		this.draw_rotate(matrix, prop, camera);		break;
			case Mode.Character:	this.draw_character(matrix, prop, camera);	break;
		}
	}

	protected void	draw_move(Matrix4x4 matrix, MaterialPropertyBlock prop, Camera camera)
	{
		if(this.move_mode_axis_mask.x) {

			if(this.grabbed_axis == GrabbedAxis.X) {
				prop.SetColor("_Color", Color.yellow);
			} else {
				prop.SetColor("_Color", Color.red);
			}
			Graphics.DrawMesh(mpiObjectHandle.s_mesh, matrix, mpiObjectHandle.s_material, 0, camera, (int)SubMeshIndex.X_Line, prop);
			Graphics.DrawMesh(mpiObjectHandle.s_mesh, matrix, mpiObjectHandle.s_material, 0, camera, (int)SubMeshIndex.X_Hat,  prop);
		}

		if(this.move_mode_axis_mask.y) {

			if(this.grabbed_axis == GrabbedAxis.Y) {
				prop.SetColor("_Color", Color.yellow);
			} else {
				prop.SetColor("_Color", Color.green);
			}
			Graphics.DrawMesh(mpiObjectHandle.s_mesh, matrix, mpiObjectHandle.s_material, 0, this.main_camera, (int)SubMeshIndex.Y_Line, prop);
			Graphics.DrawMesh(mpiObjectHandle.s_mesh, matrix, mpiObjectHandle.s_material, 0, this.main_camera, (int)SubMeshIndex.Y_Hat,  prop);
		}

		if(this.move_mode_axis_mask.z) {

			if(this.grabbed_axis == GrabbedAxis.Z) {
				prop.SetColor("_Color", Color.yellow);
			} else {
				prop.SetColor("_Color", Color.blue);
			}
			Graphics.DrawMesh(mpiObjectHandle.s_mesh, matrix, mpiObjectHandle.s_material, 0, this.main_camera, (int)SubMeshIndex.Z_Line, prop);
			Graphics.DrawMesh(mpiObjectHandle.s_mesh, matrix, mpiObjectHandle.s_material, 0, this.main_camera, (int)SubMeshIndex.Z_Hat,  prop);
		}
	}

	// ---------------------------------------------------------------- //

	protected void	draw_rotate(Matrix4x4 matrix, MaterialPropertyBlock prop, Camera camera)
	{
		Vector3		camera_pos = camera.transform.position;
		Vector3		angle = Vector3.zero;

		Vector3		vx = this.transform.transform.InverseTransformVector(this.main_camera.transform.forward);
		Vector3		vy = this.transform.transform.InverseTransformVector(this.main_camera.transform.forward);
		Vector3		vz = this.transform.transform.InverseTransformVector(this.main_camera.transform.forward);

		angle.x = -Mathf.Atan2(vx.y, vx.z)*Mathf.Rad2Deg - 90.0f;
		angle.y =  Mathf.Atan2(vy.x, vy.z)*Mathf.Rad2Deg + 180.0f;
		angle.z =  Mathf.Atan2(vz.y, vz.x)*Mathf.Rad2Deg + 180.0f;

		//

		Mesh		mesh     = mpiObjectHandle.s_mesh;
		Material	material = mpiObjectHandle.s_material;

		prop.SetColor("_Color", new Color(0.8f, 0.8f, 0.8f));

		{
			Matrix4x4	billboard_matrix = Matrix4x4.TRS(this.transform.position, this.main_camera.transform.rotation, Vector3.one*this.scale);

			Graphics.DrawMesh(mesh, billboard_matrix, material, 0, this.main_camera, (int)SubMeshIndex.Circle, prop);
		}

		//

		if(this.rotate_mode_axis_mask.x) {

			if(this.grabbed_axis == GrabbedAxis.X) {
				prop.SetColor("_Color", Color.yellow);
			} else {
				prop.SetColor("_Color", Color.red);
			}
			Graphics.DrawMesh(mesh, matrix*Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(angle.x, 0.0f, 0.0f), Vector3.one), material, 0, this.main_camera, (int)SubMeshIndex.X_Arc, prop);
		}

		if(this.rotate_mode_axis_mask.y) {

			if(this.grabbed_axis == GrabbedAxis.Y) {
				prop.SetColor("_Color", Color.yellow);
			} else {
				prop.SetColor("_Color", Color.green);
			}
			Graphics.DrawMesh(mesh, matrix*Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, angle.y, 0.0f), Vector3.one), material, 0, this.main_camera, (int)SubMeshIndex.Y_Arc, prop);
		}

		if(this.rotate_mode_axis_mask.z) {

			if(this.grabbed_axis == GrabbedAxis.Z) {
				prop.SetColor("_Color", Color.yellow);
			} else {
				prop.SetColor("_Color", Color.blue);
			}
			Graphics.DrawMesh(mesh, matrix*Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, angle.z), Vector3.one), material, 0, this.main_camera, (int)SubMeshIndex.Z_Arc, prop);
		}
	}

	// ---------------------------------------------------------------- //

	protected void	draw_character(Matrix4x4 matrix, MaterialPropertyBlock prop, Camera camera)
	{
		this.draw_move(matrix, prop, camera);
		this.draw_rotate(matrix, prop, camera);
	}

	// ================================================================ //

	public static void	initializeStatics()
	{
		BuildMesh.Cone	builder_cone_x  = new BuildMesh.Cone();
		BuildMesh.Cone	builder_cone_y  = new BuildMesh.Cone();
		BuildMesh.Cone	builder_cone_z  = new BuildMesh.Cone();
		BuildMesh.Lines	builder_lines_x = new BuildMesh.Lines();
		BuildMesh.Lines	builder_lines_y = new BuildMesh.Lines();
		BuildMesh.Lines	builder_lines_z = new BuildMesh.Lines();

		builder_cone_x.createVertices(0.1f, 0.5f, 12);
		builder_cone_x.applyTransform(Matrix4x4.TRS(Vector3.right, Quaternion.Euler(0.0f, 0.0f, -90.0f), Vector3.one));

		builder_cone_y.createVertices(0.1f, 0.5f, 12);
		builder_cone_y.applyTransform(Matrix4x4.TRS(Vector3.up, Quaternion.Euler(0.0f, 0.0f, 0.0f), Vector3.one));

		builder_cone_z.createVertices(0.1f, 0.5f, 12);
		builder_cone_z.applyTransform(Matrix4x4.TRS(Vector3.forward, Quaternion.Euler(90.0f, 0.0f, 0.0f), Vector3.one));

		builder_lines_x.addLine(Vector3.zero, Vector3.right,   Color.white);
		builder_lines_y.addLine(Vector3.zero, Vector3.up,      Color.white);
		builder_lines_z.addLine(Vector3.zero, Vector3.forward, Color.white);

		//

		BuildMesh.WireCircle	builder_arc_x = new BuildMesh.WireCircle();
		BuildMesh.WireCircle	builder_arc_y = new BuildMesh.WireCircle();
		BuildMesh.WireCircle	builder_arc_z = new BuildMesh.WireCircle();
		BuildMesh.WireCircle	builder_circle = new BuildMesh.WireCircle();

		builder_arc_x.setArcAngle(-90.0f, 90.0f);
		builder_arc_x.createVertices(1.0f, 16);
		builder_arc_x.applyTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler( 0.0f, 90.0f, 90.0f), Vector3.one));

		builder_arc_y.setArcAngle(-90.0f, 90.0f);
		builder_arc_y.createVertices(1.0f, 16);
		builder_arc_y.applyTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler( 90.0f,  -90.0f,  0.0f), Vector3.one));

		builder_arc_z.setArcAngle(-90.0f, 90.0f);
		builder_arc_z.createVertices(1.0f, 16);
		builder_arc_z.applyTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f,   0.0f, 0.0f), Vector3.one));

		builder_circle.createVertices(1.0f, 32);

		//

		BuildMesh.Base	builder = new BuildMesh.Base();
		builder.clearSubMeshes();

			builder.appendSubMesh(builder_cone_x);
			builder.appendSubMesh(builder_lines_x);
			builder.appendSubMesh(builder_cone_y);
			builder.appendSubMesh(builder_lines_y);
			builder.appendSubMesh(builder_cone_z);
			builder.appendSubMesh(builder_lines_z);

			builder.appendSubMesh(builder_arc_x);
			builder.appendSubMesh(builder_arc_y);
			builder.appendSubMesh(builder_arc_z);

			builder.appendSubMesh(builder_circle);

		builder.makeSolid();
		builder.bakeLighitingToVertexColor(new Vector3(-1.0f, -1.0f, 1.0f), 0.8f, 0.2f);
		builder.instantiate();

		mpiObjectHandle.s_mesh = builder.mesh;

		//

		Shader	shader = Shader.Find("Hidden/Internal-Colored");

		mpiObjectHandle.s_material = new Material(shader);
		mpiObjectHandle.s_material.hideFlags = HideFlags.HideAndDontSave;

		mpiObjectHandle.s_material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mpiObjectHandle.s_material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

		mpiObjectHandle.s_material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);

		mpiObjectHandle.s_material.SetInt("_ZTest", 4);
		mpiObjectHandle.s_material.SetInt("_ZWrite", 1);

	}
}
