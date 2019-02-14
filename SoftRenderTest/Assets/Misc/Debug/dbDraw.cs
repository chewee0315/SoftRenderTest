using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// デバッグ用プリミティブ描画.
public class dbDraw : MonoBehaviour {


	// ================================================================ //

	// ワイヤーフレームの球を描く.
	public void		drawWireSphere(float radius, Color color, float width = 1.0f)
	{
		this.commands.Add(new WireSphere.Command(this, radius, color, width));
	}
	// ワイヤーフレームの球を描く.
	public void		drawWireBox(float x_length, float y_length, float z_length, Color color, float width = 1.0f)
	{
		this.commands.Add(new WireBox.Command(this, x_length, y_length, z_length, color, width));
	}
	// ラインを描く.
	public void		drawLine(Vector3 position0, Vector3 position1, Color color, float width = 1.0f)
	{
		this.commands.Add(new Line.Command(this, position0, position1, color, width));
	}
	// ラインをたくさん描く.
	public void		drawLines(Vector3[] positions, Color color, float width = 1.0f)
	{
		this.commands.Add(new Lines.Command(this, positions, color, width));
	}
	public void		drawLines(VertexP3fC4f[] vertices, float width = 1.0f)
	{
		this.commands.Add(new Lines.Command(this, vertices, width));
	}
	// ラインストリップを描く.
	public void		drawLineStrip(Vector3[] positions, bool is_looped, Color color, float width = 1.0f)
	{
		this.commands.Add(new LineStrip.Command(this, positions, is_looped, color, width));
	}
	// ポイントをたくさん描く.
	public void		drawPoints(Vector3[] positions, Color color, float size_in_screen)
	{
		this.commands.Add(new Points.Command(this, positions, color, size_in_screen));
	}
	// ワイヤーフレームのカプセルを描く.
	public void		drawWireCapsule(Vector3 position, float length, float radius0, float radius1, Color color, bool is_draw_bottom_canopy, bool is_draw_top_canopy, float width = 1.0f)
	{
		this.commands.Add(new WireCapsule.Command(this, position, length, radius0, radius1, color, is_draw_bottom_canopy, is_draw_top_canopy ,width));
	}
	// ワイヤーフレームの四角形を描く.
	public void		drawWireSquare(Color color, float width = 1.0f)
	{
		this.commands.Add(new WireSquare.Command(this, color, width));
	}

	// ワイヤーフレームのやじるし（XZ 平面、X 方向）.
	public void		drawWireArrow(float width, float length, Color color, float line_width = 1.0f)
	{
		this.commands.Add(new WireArrow.Command(this, width, length, color, line_width));
	}

	// XYZ 軸を描く.
	public void		drawXYZ(float width = 1.0f)
	{
		this.commands.Add(new XYZ.Command(this, width));
	}
	// やじるしを描く.
	public void		drawArrow(Vector3 from, Vector3 to, Color color, float width = 1.0f)
	{
		this.commands.Add(new Arrow.Command(this, from, to, color, width));
	}

	// グリッドを描く.
	public void		drawGridXZ(float x_size, float z_size, float grid_length)
	{
		this.commands.Add(new GridXZ.Command(this, x_size, z_size, grid_length));
	}
	// 2D ラインを描く.
	public void		drawLine2D(Vector3 position0, Vector3 position1, Color color, float width = 1.0f)
	{
		this.commands.Add(new Line2D.Command(this, position0, position1, color, width));
	}
	// 2D ラインストリップを描く.
	public void		drawLineStrip2D(Vector3[] positions, bool is_looped, Color color, float width = 1.0f)
	{
		this.commands.Add(new LineStrip2D.Command(this, positions, is_looped, color, width));
	}
	// 2D ラインをたくさん描く.
	public void		drawLines2D(Vector3[] positions, Color color, float width = 1.0f)
	{
		this.commands.Add(new Lines2D.Command(this, positions, color, width));
	}
	// 2D ポイントをたくさん描く.
	public void		drawPoints2D(Vector3[] positions, Color color, float size_in_screen)
	{
		this.commands.Add(new Points2D.Command(this, positions, color, size_in_screen));
	}

	// 2D ワイヤーフレームの四角形を描く.
	public void		drawWireSquare2D(Vector3[] positions, Color color, float line_width = 1.0f)
	{
		this.commands.Add(new WireSquare2D.Command(this, positions, color, line_width));
	}
	public void		drawWireSquare2D(Vector3 center, float width, float height, Color color, float line_width = 1.0f)
	{
		this.commands.Add(new WireSquare2D.Command(this, center, width, height, color, line_width));
	}

	// 2D の四角形を描く.
	public void		drawSquare2D(Vector3[] positions, Color color)
	{
		this.commands.Add(new Square2D.Command(this, positions, color));
	}
	public void		drawSquare2D(Vector3 center, float width, float height, Color color)
	{
		this.commands.Add(new Square2D.Command(this, center, width, height, color));
	}

	// 2D ワイヤーフレームのまるを描く.
	public void		drawWireCircle2D(Vector3 center, float radius, Color color, float line_width = 1.0f)
	{
		this.commands.Add(new WireCircle2D.Command(this, center, radius, color, line_width));
	}

	// 2D テクスチャーを描く.
	public void		drawTexture2D(Texture2D texture, Vector3 center,  float width, float height, Color color)
	{
		this.commands.Add(new DrawTexture2D.Command(this, texture, center, width, height, color));
	}

	// レンダーターゲットをセットする.
	public void		setRenderTarget(RenderTexture rt)
	{
		this.commands.Add(new SetRenderTarget.Command(this, rt));
	}
	// デフォルトのレンダーターゲットをセットする.
	public void		setDefaultRenderTarget()
	{
		this.commands.Add(new SetRenderTarget.Command(this, null));
	}
	// バッファーをクリアーする.
	public void		clearBuffer(Color color)
	{
		this.commands.Add(new ClearBuffer.Command(this, color));
	}

	// モデルマトリックスをセットする.
	public void	setModelMatrix(Matrix4x4 matrix)
	{
		this.model_matrix_base = matrix;
		GL.modelview = this.model_view_matrix_base*this.model_matrix_base;
	}
	public void	setModelMatrixTRS(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		this.setModelMatrix(Matrix4x4.TRS(position, rotation, scale));
	}
	public void	setModelMatrixTRS(Transform transform)
	{
		this.setModelMatrix(Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale));
	}
	public void	setModelMatrixIdentity()
	{
		this.setModelMatrix(Matrix4x4.identity);
	}

	public void	setZTestEnable(bool is_enable)
	{
		this.is_z_test_enable = is_enable;
	}


	// ================================================================ //

	public class CommandBase {

		public dbDraw	db_draw;
		
		protected Matrix4x4		matrix;
		protected bool			is_z_test_enable = true;
		protected Color			color = Color.white;

		public CommandBase(dbDraw db_draw)
		{
			this.db_draw = db_draw;
			this.is_z_test_enable = this.db_draw.is_z_test_enable;
			this.matrix           = this.db_draw.model_matrix_base;
		}

		public void		pre_draw()
		{
			if(this.db_draw.is_z_test_enable != this.is_z_test_enable) {
				if(this.is_z_test_enable) {
					this.db_draw.line_material.SetPass(0);
				} else {
					this.db_draw.line_material_no_z_test.SetPass(0);
				}
				this.db_draw.is_z_test_enable = this.is_z_test_enable;
			}

			this.db_draw.model_matrix_base = this.matrix;
		}

		public virtual void		draw() {}
	}

	// ================================================================ //

	public Material	line_material;
	public Material	surface_material;
	public Material	texture_material;
	public Material	line_material_no_z_test;

	protected List<CommandBase>		commands = new List<CommandBase>();

	protected bool			is_z_test_enable = true;

	protected Matrix4x4		model_matrix_base;
	protected Matrix4x4		model_view_matrix_base;

	internal class DynamicMaterials {

		internal List<Material>		materials = new List<Material>();
		internal int		next_use_index = 0;
	}
	internal DynamicMaterials		dynamic_materials = new DynamicMaterials();

	internal Vector2	buffer_size = new Vector2(1280.0f, 720.0f);

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

	public void		OnRenderObject()
	{
		if(Camera.current == Camera.main) {

			this.begin_scene();
		
			this.is_z_test_enable = true;
			this.line_material.SetPass(0);

			foreach(var command in this.commands) {
				command.pre_draw();
				command.draw();
			}

			this.commands.Clear();

			this.end_scene();

			this.dynamic_materials.next_use_index = 0;
		}
	}

	protected void	begin_scene()
	{
		this.buffer_size.x = Screen.width;
		this.buffer_size.y = Screen.height;

		this.model_matrix_base      = Matrix4x4.identity;
		this.model_view_matrix_base = GL.modelview;
		this.is_z_test_enable = true;

		GL.PushMatrix();
	}

	protected void	end_scene()
	{
		GL.PopMatrix();

		this.model_matrix_base      = Matrix4x4.identity;
		this.model_view_matrix_base = GL.modelview;
		this.is_z_test_enable = true;

		this.buffer_size.x = Screen.width;
		this.buffer_size.y = Screen.height;
	}

	// ================================================================ //

	public void	create()
	{
		// Unity has a built-in shader that is useful for drawing
		// simple colored things.
		Shader	shader = Shader.Find("Hidden/Internal-Colored");

		this.line_material = new Material(shader);
		this.line_material.hideFlags = HideFlags.HideAndDontSave;

		this.line_material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		this.line_material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

		this.line_material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

		this.line_material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual /*4*/);
		this.line_material.SetInt("_ZWrite", 0);

		//

		this.surface_material = new Material(shader);
		this.surface_material.hideFlags = HideFlags.HideAndDontSave;

		this.surface_material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		this.surface_material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

		this.surface_material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);

		this.surface_material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Less/*2*/);
		this.surface_material.SetInt("_ZWrite", 1);

		//

		this.texture_material = new Material(Shader.Find("Unlit/Transparent"));
		this.texture_material.hideFlags = HideFlags.HideAndDontSave;

		this.texture_material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		this.texture_material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

		this.texture_material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);

		this.texture_material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Less/*2*/);
		this.texture_material.SetInt("_ZWrite", 1);

		//

		this.line_material_no_z_test = new Material(shader);
		this.line_material_no_z_test.hideFlags = HideFlags.HideAndDontSave;

		this.line_material_no_z_test.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		this.line_material_no_z_test.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

		this.line_material_no_z_test.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

		this.line_material_no_z_test.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Disabled /*0*/);
		this.line_material_no_z_test.SetInt("_ZWrite", 0);

		WireSphere.createStaticData();
		WireCapsule.createStaticData();
		WireSquare.createStaticData();
		XYZ.createStaticData();
		Arrow.createStaticData();
		WireCircle2D.createStaticData();
	}

	// １フレーム使い捨てのマテリアルを作る.
	internal Material		allocate_dynamic_material(Material src)
	{
		if(this.dynamic_materials.next_use_index >= this.dynamic_materials.materials.Count) {

			this.dynamic_materials.materials.Add(new Material(src));
		}

		Material	material = this.dynamic_materials.materials[this.dynamic_materials.next_use_index];

		material.CopyPropertiesFromMaterial(src);

		this.dynamic_materials.next_use_index++;

		return(material);
	}

	public void		draw_line_loop(Vector3[] positions)
	{
		for(int i = 0;i < positions.Length;i++) {
			GL.Vertex(positions[i%positions.Length]);
			GL.Vertex(positions[(i + 1)%positions.Length]);
		}
	}

	public void		draw_line_strip(Vector3[] positions)
	{
		for(int i = 0;i < positions.Length - 1;i++) {
			GL.Vertex(positions[i]);
			GL.Vertex(positions[i + 1]);
		}
	}

	public void		draw_line_strip_2d(Vector3[] positions, Color color)
	{
		GL.PushMatrix();

			this.load_ortho();
			GL.Begin(GL.LINES);
			GL.Color(color);

			for(int i = 0;i < positions.Length - 1;i++) {
				GL.Vertex(positions[i]);
				GL.Vertex(positions[i + 1]);
			}
			GL.End();
		GL.PopMatrix();
	}

	public void		draw_lines(Vector3[] positions)
	{
		for(int i = 0;i < positions.Length;i += 2) {
			GL.Vertex(positions[i]);
			GL.Vertex(positions[i + 1]);
		}
	}

	public void		draw_lines_2d(Vector3[] positions, Color color)
	{
		GL.PushMatrix();
			this.load_ortho();
			GL.Begin(GL.LINES);
			GL.Color(color);

			for(int i = 0;i < positions.Length;i += 2) {
				GL.Vertex(positions[i]);
				GL.Vertex(positions[i + 1]);
			}

			GL.End();
		GL.PopMatrix();
	}


	protected void	draw_lines_with_width(Vector3[] positions, float width, Color color)
	{
		GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(GL.TRIANGLES);
			GL.Color(color);

			for(int i = 0;i < positions.Length;i += 2) {

				int		fi = i + 1;
				int		bi = i;

				Vector3		f = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(positions[fi]));
				Vector3		b = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(positions[bi]));

				f.z = 0.0f;
				b.z = 0.0f;

				Vector3		dir = (f - b).normalized;
				Vector3		side;

				side.x =  dir.y;
				side.y = -dir.x;
				side.z =  dir.z;

				Vector3		fr = f + side*width/2.0f;
				Vector3		fl = f - side*width/2.0f;
				Vector3		br = b + side*width/2.0f;
				Vector3		bl = b - side*width/2.0f;

				fr.x /= Screen.width;
				fr.y /= Screen.height;
				fl.x /= Screen.width;
				fl.y /= Screen.height;
				br.x /= Screen.width;
				br.y /= Screen.height;
				bl.x /= Screen.width;
				bl.y /= Screen.height;

				GL.Vertex(br);
				GL.Vertex(bl);
				GL.Vertex(fr);

				GL.Vertex(fl);
				GL.Vertex(fr);
				GL.Vertex(bl);
			}

			GL.End();
		GL.PopMatrix();
	}

	protected void	draw_lines_with_width(VertexP3fC4f[] vertices, float width)
	{
		GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(GL.TRIANGLES);

			for(int i = 0;i < vertices.Length;i += 2) {

				int		fi = i + 1;
				int		bi = i;

				Vector3		f = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(vertices[fi].position));
				Vector3		b = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(vertices[bi].position));

				f.z = 0.0f;
				b.z = 0.0f;

				Vector3		dir = (f - b).normalized;
				Vector3		side;

				side.x =  dir.y;
				side.y = -dir.x;
				side.z =  dir.z;

				Vector3		fr = f + side*width/2.0f;
				Vector3		fl = f - side*width/2.0f;
				Vector3		br = b + side*width/2.0f;
				Vector3		bl = b - side*width/2.0f;

				fr.x /= Screen.width;
				fr.y /= Screen.height;
				fl.x /= Screen.width;
				fl.y /= Screen.height;
				br.x /= Screen.width;
				br.y /= Screen.height;
				bl.x /= Screen.width;
				bl.y /= Screen.height;

				GL.Color(vertices[bi].color);
				GL.Vertex(br);
				GL.Vertex(bl);
				GL.Vertex(fr);

				GL.Color(vertices[fi].color);
				GL.Vertex(fl);
				GL.Vertex(fr);
				GL.Vertex(bl);
			}

			GL.End();
		GL.PopMatrix();
	}

	protected void	draw_line_strip_with_width(Vector3[] positions, bool is_looped, float width, Color color)
	{
		GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(GL.TRIANGLE_STRIP);
			GL.Color(color);

			int		loop_times = positions.Length;
			if(is_looped) {
				loop_times++;
			}

			for(int i = 0;i < loop_times;i++) {

				Vector3		c = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(positions[i%positions.Length]));

				c.z = 0.0f;

				int		fi, bi;

				if(is_looped) {
					fi = (i + 1)%positions.Length;
					bi = (i + positions.Length - 1)%positions.Length;
				} else {
					if(i < positions.Length - 1) {
						fi = i + 1;
					} else {
						fi = i;
					}
					if(i > 0) {
						bi = i - 1;
					} else {
						bi = i;
					}
				}

				Vector3		f = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(positions[fi]));
				Vector3		b = Camera.main.WorldToScreenPoint(this.model_matrix_base.MultiplyPoint(positions[bi]));

				f.z = 0.0f;
				b.z = 0.0f;

				Vector3		dir = (f - b).normalized;
				Vector3		side;

				side.x =  dir.y;
				side.y = -dir.x;
				side.z =  dir.z;

				Vector3		r = c + side*width/2.0f;
				Vector3		l = c - side*width/2.0f;

				r.x /= Screen.width;
				r.y /= Screen.height;
				l.x /= Screen.width;
				l.y /= Screen.height;

				GL.Vertex(r);
				GL.Vertex(l);
			}

			GL.End();
		GL.PopMatrix();
	}

	internal void	load_ortho()
	{
	#if true
		Matrix4x4	m = Matrix4x4.Ortho(-this.buffer_size.x/2.0f, this.buffer_size.x/2.0f - 1.0f, -this.buffer_size.y/2.0f, this.buffer_size.y/2.0f - 1.0f, 0.0f, 1000.0f);

		GL.LoadProjectionMatrix(m);
		GL.LoadIdentity();
	#else
		GL.LoadOrtho();
	#endif
	}

	// ================================================================ //

	protected static dbDraw	instance = null;

	public static dbDraw	get()
	{
		if(dbDraw.instance == null) {

			GameObject	go = new GameObject("dbDraw");

			dbDraw.instance = go.AddComponent<dbDraw>();
			dbDraw.instance.create();
		}

		return(dbDraw.instance);
	}

	// ================================================================ //
	// ライン１本.

	public class Line {

		public class Command : CommandBase {

			public Vector3[]	positions;
			public float		width = 1.0f;

			public Command(dbDraw db_draw, Vector3 position0, Vector3 position1, Color color, float width) : base(db_draw)
			{
				this.positions = new Vector3[] { position0, position1 };
				this.width     = width;
				this.color     = color;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					GL.PushMatrix();
						GL.MultMatrix(this.db_draw.model_matrix_base);

						GL.Begin(GL.LINES);

						GL.Color(this.color);

						GL.Vertex(this.positions[0]);
						GL.Vertex(this.positions[1]);

						GL.End();
					GL.PopMatrix();

				} else {
					this.db_draw.model_matrix_base = Matrix4x4.identity;
					this.db_draw.draw_lines_with_width(this.positions, width, color);
				}
			}
		}
	}

	// ================================================================ //
	// ライン　たくさん.

	public struct VertexP3fC4f {

		public Vector3	position;
		public Color	color;

		public VertexP3fC4f(Vector3 position, Color color)
		{
			this.position = position;
			this.color    = color;
		}
	}

	public class Lines {

		public class Command : CommandBase {

			public Vector3[]		positions = null;
			public VertexP3fC4f[]	vertices_p3fc4f = null;
			public float			width     = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, Color color, float width = 1.0f) : base(db_draw)
			{
				this.positions       = positions;
				this.vertices_p3fc4f = null;
				this.width           = width;
				this.color           = color;
			}
			public Command(dbDraw db_draw, VertexP3fC4f[] vertices, float width = 1.0f) : base(db_draw)
			{
				this.positions       = null;
				this.vertices_p3fc4f = vertices;
				this.width           = width;
			}

			public override void	draw()
			{
				if(this.vertices_p3fc4f != null) {
					if(this.width <= 1.0f) {

						// 未実装.

					} else {
						this.db_draw.model_matrix_base = Matrix4x4.identity;
						this.db_draw.draw_lines_with_width(vertices_p3fc4f, width);
					}
				} else {

					if(this.width <= 1.0f) {

						GL.PushMatrix();
							GL.MultMatrix(this.db_draw.model_matrix_base);
							GL.Begin(GL.LINES);
								GL.Color(color);
								this.db_draw.draw_lines(positions);
							GL.End();
						GL.PopMatrix();
					} else {
						this.db_draw.model_matrix_base = Matrix4x4.identity;
						this.db_draw.draw_lines_with_width(positions, width, color);
					}
				}
			}
		}
	}

	// ================================================================ //
	// ライン　ストリップ.

	public class LineStrip {

		public class Command : CommandBase {

			public Vector3[]	positions = null;
			public bool			is_looped = false;
			public float		width     = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, bool is_looped, Color color, float width) : base(db_draw)
			{
				this.positions = positions;
				this.is_looped = is_looped;
				this.width     = width;
				this.color     = color;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					GL.PushMatrix();
						GL.MultMatrix(this.db_draw.model_matrix_base);
						GL.Begin(GL.LINES);
							GL.Color(color);
							if(this.is_looped) {
								this.db_draw.draw_line_loop(positions);
							} else {
								this.db_draw.draw_line_strip(positions);
							}
						GL.End();
					GL.PopMatrix();

				} else {
					this.db_draw.model_matrix_base = Matrix4x4.identity;
					this.db_draw.draw_line_strip_with_width(this.positions, this.is_looped, this.width, this.color);
				}
			}
		}
	}

	// ================================================================ //
	// ポイント　たくさん.

	public class Points {

		public class Command : CommandBase {

			public Vector3[]	positions      = null;
			public float		size_in_screen = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, Color color, float size_in_screen) : base(db_draw)
			{
				this.positions      = positions;
				this.size_in_screen = size_in_screen;
				this.color          = color;
			}

			public override void	draw()
			{
				GL.PushMatrix();
					GL.LoadOrtho();
					GL.Begin(GL.QUADS);
					GL.Color(color);

					for(int i = 0;i < positions.Length;i++) {

						Vector3	c = Camera.main.WorldToViewportPoint(positions[i]);

						c.z = 0.0f;

						float	vx = size_in_screen/2.0f/Screen.width;
						float	vy = size_in_screen/2.0f/Screen.height;

						GL.Vertex(c + new Vector3( vx,  vy, 0.0f));
						GL.Vertex(c + new Vector3( vx, -vy, 0.0f));
						GL.Vertex(c + new Vector3(-vx, -vy, 0.0f));
						GL.Vertex(c + new Vector3(-vx,  vy, 0.0f));
					}

					GL.End();
				GL.PopMatrix();
			}
		}
	}

	// ================================================================ //
	// ワイヤーフレームの球.

	public class WireSphere {

		public const int	DIVISION = 16;		// 球の分割数.

		public static List<Vector3>		s_points_yz = new List<Vector3>();
		public static List<Vector3>		s_points_zx = new List<Vector3>();
		public static List<Vector3>		s_points_xy = new List<Vector3>();

		public class Command : CommandBase {

			public float	radius = 1.0f;
			public float	width  = 1.0f;

			public Command(dbDraw db_draw, float radius, Color color, float width) : base(db_draw)
			{
				this.radius   = radius;
				this.color    = color;
				this.width    = width;
			}

			public override void	draw()
			{
				GL.PushMatrix();

				Matrix4x4	mat = this.db_draw.model_matrix_base;

				mat = mat*Matrix4x4.Scale(Vector3.one*this.radius);

				if(width <= 1.0f) {

					GL.MultMatrix(mat);

					GL.Begin(GL.LINES);

					GL.Color(this.color);

					this.db_draw.draw_line_loop(s_points_yz.ToArray());			// Ｘまわり.
					this.db_draw.draw_line_loop(s_points_zx.ToArray());			// Ｙまわり.
					this.db_draw.draw_line_loop(s_points_xy.ToArray());			// Ｚまわり.

					GL.End();

				} else {

					this.db_draw.model_matrix_base = mat;
					this.db_draw.draw_line_strip_with_width(s_points_yz.ToArray(), true, width, color);
					this.db_draw.draw_line_strip_with_width(s_points_zx.ToArray(), true, width, color);
					this.db_draw.draw_line_strip_with_width(s_points_xy.ToArray(), true, width, color);
				}

				GL.PopMatrix();
			}
		}

		public static void	createStaticData()
		{
			Vector3		p;

			// --------------------------------------------------------------------- //
			// Ｘまわり

			for(int i = 0;i < DIVISION;i++) {

				float	angle = 2.0f*Mathf.PI*(float)i/(float)DIVISION;

				p.x = 0.0f;
				p.y = Mathf.Cos(angle);
				p.z = Mathf.Sin(angle);

				s_points_yz.Add(p);
			}

			// --------------------------------------------------------------------- //
			// Ｙまわり

			for(int i = 0;i < DIVISION;i++) {

				float	angle = 2.0f*Mathf.PI*(float)i/(float)DIVISION;

				p.x = Mathf.Sin(angle);
				p.y = 0.0f;
				p.z = Mathf.Cos(angle);

				s_points_zx.Add(p);
			}

			// --------------------------------------------------------------------- //
			// Ｚまわり

			for(int i = 0;i < DIVISION;i++) {

				float	angle = 2.0f*Mathf.PI*(float)i/(float)DIVISION;

				p.x = Mathf.Cos(angle);
				p.y = Mathf.Sin(angle);
				p.z = 0.0f;

				s_points_xy.Add(p);
			}
		}
	}

	// ================================================================ //
	// ワイヤーフレームのボックス.

	public class WireBox {

		public const int	DIVISION = 16;		// 球の分割数.

		public static List<Vector3>		s_points_yz = new List<Vector3>();
		public static List<Vector3>		s_points_zx = new List<Vector3>();
		public static List<Vector3>		s_points_xy = new List<Vector3>();

		public class Command : CommandBase {

			public float	x_length = 1.0f;
			public float	y_length = 1.0f;
			public float	z_length = 1.0f;
			public float	width  = 1.0f;

			public Command(dbDraw db_draw, float x_length, float y_length, float z_length, Color color, float width) : base(db_draw)
			{
				this.x_length   = x_length;
				this.y_length   = y_length;
				this.z_length   = z_length;
				this.color    = color;
				this.width    = width;
			}

			public override void	draw()
			{
				GL.PushMatrix();

				Matrix4x4	mat = this.db_draw.model_matrix_base;

				Vector3		rtf = new Vector3( this.x_length/2.0f,  this.y_length/2.0f,  this.z_length/2.0f);
				Vector3		ltf = new Vector3(-this.x_length/2.0f,  this.y_length/2.0f,  this.z_length/2.0f);
				Vector3		rbf = new Vector3( this.x_length/2.0f, -this.y_length/2.0f,  this.z_length/2.0f);
				Vector3		lbf = new Vector3(-this.x_length/2.0f, -this.y_length/2.0f,  this.z_length/2.0f);
				Vector3		rtn = new Vector3( this.x_length/2.0f,  this.y_length/2.0f, -this.z_length/2.0f);
				Vector3		ltn = new Vector3(-this.x_length/2.0f,  this.y_length/2.0f, -this.z_length/2.0f);
				Vector3		rbn = new Vector3( this.x_length/2.0f, -this.y_length/2.0f, -this.z_length/2.0f);
				Vector3		lbn = new Vector3(-this.x_length/2.0f, -this.y_length/2.0f, -this.z_length/2.0f);

				Vector3[]	pnts = new Vector3[] {

					// XZ 平面の四角　手前.
					rtf, ltf,
					ltf, lbf,
					lbf, rbf,
					rbf, rtf,

					// XZ 平面の四角　奥.
					rtn, ltn,
					ltn, lbn,
					lbn, rbn,
					rbn, rtn,

					// 手前 -> 奥.
					rtf, rtn,
					ltf, ltn,
					lbf, lbn,
					rbf, rbn,
				};

				if(width <= 1.0f) {

					GL.MultMatrix(mat);

					GL.Begin(GL.LINES);

					GL.Color(this.color);

					this.db_draw.draw_lines(pnts);
					GL.End();

				} else {

					this.db_draw.model_matrix_base = mat;
					this.db_draw.draw_lines_with_width(pnts, width, color);
				}

				GL.PopMatrix();
			}
		}
	}

	// ================================================================ //
	// ワイヤーフレームのカプセル.

	public class WireCapsule {

		protected  const int	division = 16;

		protected static Vector3[]	s_lid_yz = null;
		protected static Vector3[]	s_canopy_zx = null;
		protected static Vector3[]	s_canopy_xy = null;


		public class Command : CommandBase {

			public Vector3	position;
			public float	length  = 1.0f;
			public float	radius0 = 1.0f;
			public float	radius1 = 1.0f;
			public float	width   = 1.0f;
			public bool		is_draw_bottom_canopy = true;
			public bool		is_draw_top_canopy = true;

			public Command(dbDraw db_draw, Vector3 position, float length, float radius0, float radius1, Color color, bool is_draw_bottom_canopy, bool is_draw_top_canopy, float width = 1.0f) : base(db_draw)
			{
				this.position  = position;
				this.length    = length;
				this.radius0   = radius0;
				this.radius1   = radius1;
				this.color     = color;
				this.is_draw_bottom_canopy = is_draw_bottom_canopy;
				this.is_draw_top_canopy = is_draw_top_canopy;
				this.width    = width;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					Matrix4x4	base_matrix = this.db_draw.model_matrix_base;

					base_matrix *= Matrix4x4.TRS(this.position, Quaternion.identity, new Vector3(-this.radius0, this.radius0, this.radius0));

					GL.PushMatrix();
					GL.MultMatrix(base_matrix);

					GL.Begin(GL.LINES);
					GL.Color(color);

					// 円柱の上面.
					this.db_draw.draw_line_loop(s_lid_yz);

					// 側面のライン.
					for(int i = 0;i < s_lid_yz.Length;i++) {
						GL.Vertex(s_lid_yz[i]);
						GL.Vertex(s_lid_yz[i]*radius1/radius0 - Vector3.right*length/radius0);
					}

					// 下側の半球.
					if(this.is_draw_bottom_canopy) {
						this.db_draw.draw_line_strip(s_canopy_zx);
						this.db_draw.draw_line_strip(s_canopy_xy);
					}
					GL.End();
					GL.PopMatrix();

					base_matrix = this.db_draw.model_matrix_base;
					base_matrix *= Matrix4x4.TRS(this.position + Vector3.right*this.length, Quaternion.identity, new Vector3(this.radius1, this.radius1, this.radius1));

					GL.PushMatrix();
					GL.MultMatrix(base_matrix);

					GL.Begin(GL.LINES);
					GL.Color(color);

					// 円柱の下面.
					this.db_draw.draw_line_loop(s_lid_yz);

					// 下側の半球.
					if(this.is_draw_top_canopy) {
						this.db_draw.draw_line_strip(s_canopy_zx);
						this.db_draw.draw_line_strip(s_canopy_xy);
					}
					GL.End();
					GL.PopMatrix();

				} else {

					Matrix4x4	base_matrix = this.db_draw.model_matrix_base;

					this.db_draw.model_matrix_base = base_matrix*Matrix4x4.TRS(this.position, Quaternion.identity, new Vector3(-this.radius0, this.radius0, this.radius0));

					// 円柱の上面.
					this.db_draw.draw_line_strip_with_width(s_lid_yz, true, this.width, this.color);

					// 側面のライン.
					Vector3[]	lid_lines = new Vector3[s_lid_yz.Length*2];
					for(int i = 0;i < s_lid_yz.Length;i++) {
						lid_lines[i*2 + 0] = s_lid_yz[i];
						lid_lines[i*2 + 1] = s_lid_yz[i]*this.radius1/this.radius0 - Vector3.right*this.length/this.radius0;
					}
					this.db_draw.draw_lines_with_width(lid_lines, this.width, this.color);

					// 下側の半球.
					if(is_draw_bottom_canopy) {
						this.db_draw.draw_line_strip_with_width(s_canopy_zx, false, this.width, this.color);
						this.db_draw.draw_line_strip_with_width(s_canopy_xy, false, this.width, this.color);
					}

					this.db_draw.model_matrix_base = base_matrix*Matrix4x4.TRS(this.position + Vector3.right*this.length, Quaternion.identity, new Vector3(this.radius1, this.radius1, this.radius1));

					// 円柱の下面.
					this.db_draw.draw_line_strip_with_width(s_lid_yz, true, this.width, color);

					// 下側の半球.
					if(this.is_draw_top_canopy) {
						this.db_draw.draw_line_strip_with_width(s_canopy_zx, false, this.width, color);
						this.db_draw.draw_line_strip_with_width(s_canopy_xy, false, this.width, color);
					}
				}
			}
		}

		public static void	createStaticData()
		{
			s_lid_yz = new Vector3[WireCapsule.division];
			s_canopy_zx = new Vector3[WireCapsule.division/2 + 1];
			s_canopy_xy = new Vector3[WireCapsule.division/2 + 1];

			// --------------------------------------------------------------------- //

			// 円柱のふた.
			for(int i = 0;i< WireCapsule.division;i++) {

				float	t = (Mathf.PI*2.0f*(float)i)/((float)WireCapsule.division);

				s_lid_yz[i].x = 0.0f;
				s_lid_yz[i].y = Mathf.Cos(t);
				s_lid_yz[i].z = Mathf.Sin(t);
			}

			// 半球.
			for(int i = 0;i < WireCapsule.division/2 + 1;i++) {

				float	t = (Mathf.PI*2.0f*(float)i)/((float)WireCapsule.division);

				s_canopy_zx[i].x = Mathf.Sin(t);
				s_canopy_zx[i].y = 0.0f;
				s_canopy_zx[i].z = Mathf.Cos(t);
			}
			for(int i = 0;i < WireCapsule.division/2 + 1;i++) {

				float	t = (Mathf.PI*2.0f*(float)i)/((float)WireCapsule.division) - Mathf.PI/2.0f;

				s_canopy_xy[i].x = Mathf.Cos(t);
				s_canopy_xy[i].y = Mathf.Sin(t);
				s_canopy_xy[i].z = 0.0f;
			}
		}
	}
	// ================================================================ //
	// ただの四角（XY 平面）.

	public class WireSquare {

		protected static Vector3[]	s_square = null;
		
		public class Command : CommandBase {

			public float	width   = 1.0f;

			public Command(dbDraw db_draw, Color color, float width = 1.0f) : base(db_draw)
			{
				this.color = color;
				this.width = width;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					GL.PushMatrix();
					GL.MultMatrix(this.db_draw.model_matrix_base);

					GL.Begin(GL.LINES);
					GL.Color(color);
					this.db_draw.draw_line_loop(s_square);
					GL.End();

					GL.PopMatrix();

				} else {
					this.db_draw.draw_line_strip_with_width(s_square, true, this.width, color);
				}
			}
		}

		public static void	createStaticData()
		{
			s_square = new Vector3[4];
			s_square[0] = new Vector3( 1.0f,  1.0f, 0.0f);
			s_square[1] = new Vector3( 1.0f, -1.0f, 0.0f);
			s_square[2] = new Vector3(-1.0f, -1.0f, 0.0f);
			s_square[3] = new Vector3(-1.0f,  1.0f, 0.0f);
		}
	}

	// ================================================================ //
	// ワイヤーフレームのやじるし（XZ 平面、X 方向）.

	public class WireArrow {

		protected static Vector3[]	s_points = null;
		
		public class Command : CommandBase {

			public float	width      = 0.0f;
			public float	length     = 0.0f;
			public float	line_width = 0.0f;

			public Command(dbDraw db_draw, float width, float length, Color color, float line_width = 1.0f) : base(db_draw)
			{
				this.width  = width;
				this.length = length;
				this.color  = color;
				this.line_width = line_width;
			}

			public override void	draw()
			{
				if(s_points == null) {
					s_points = new Vector3[7];
				}

				float	w  = this.width/2.0f;
				float	hw = this.width;

				float	l  = this.length;
				float	hl = Mathf.Min(l, hw);

				s_points[0] = new Vector3(  0.0f, 0.0f,   w/2.0f);
				s_points[1] = new Vector3(l - hl, 0.0f,   w/2.0f);
				s_points[2] = new Vector3(l - hl, 0.0f,  hw/2.0f);
				s_points[3] = new Vector3(     l, 0.0f,     0.0f);
				s_points[4] = new Vector3(l - hl, 0.0f, -hw/2.0f);
				s_points[5] = new Vector3(l - hl, 0.0f,  -w/2.0f);
				s_points[6] = new Vector3(  0.0f, 0.0f,  -w/2.0f);

				if(this.line_width <= 1.0f) {

					GL.PushMatrix();
					GL.MultMatrix(this.db_draw.model_matrix_base);

					GL.Begin(GL.LINES);
					GL.Color(color);
					this.db_draw.draw_line_loop(s_points);
					GL.End();

					GL.PopMatrix();

				} else {
					this.db_draw.draw_line_strip_with_width(s_points, true, this.line_width, color);
				}
			}
		}
	}

	// ================================================================ //
	// ワイヤーフレームの XYZ 軸.

	public class XYZ {

		protected static Vector3[]		s_x_axis = null;
		protected static Vector3[]		s_y_axis = null;
		protected static Vector3[]		s_z_axis = null;
				
		public class Command : CommandBase {

			public float	width   = 1.0f;

			public Command(dbDraw db_draw, float width = 1.0f) : base(db_draw)
			{
				this.width = width;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					GL.PushMatrix();
					GL.MultMatrix(this.db_draw.model_matrix_base);

					GL.Begin(GL.LINES);
					GL.Color(Color.red);
					this.db_draw.draw_line_strip(s_x_axis);
					GL.Color(Color.green);
					this.db_draw.draw_line_strip(s_y_axis);
					GL.Color(Color.blue);
					this.db_draw.draw_line_strip(s_z_axis);
					GL.End();

					GL.PopMatrix();

				} else {
					this.db_draw.draw_lines_with_width(s_x_axis, this.width, Color.red);
					this.db_draw.draw_lines_with_width(s_y_axis, this.width, Color.green);
					this.db_draw.draw_lines_with_width(s_z_axis, this.width, Color.blue);
				}
			}
		}

		public static void	createStaticData()
		{
			s_x_axis = new Vector3[]{ Vector3.zero, Vector3.right };
			s_y_axis = new Vector3[]{ Vector3.zero, Vector3.up };
			s_z_axis = new Vector3[]{ Vector3.zero, Vector3.forward };
		}
	}

	// ================================================================ //
	// やじるし.

	public class Arrow {

		public class Command : CommandBase {

			protected float	width   = 1.0f;
			protected Vector3		from;
			protected Vector3		to;

			protected Material		material;
			public Command(dbDraw db_draw, Vector3 from, Vector3 to, Color color, float width = 1.0f) : base(db_draw)
			{
				this.from  = from;
				this.to    = to;
				this.color = color;
				this.width = width;

				this.material = this.db_draw.allocate_dynamic_material(this.db_draw.surface_material);
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					Vector3		p0 = this.db_draw.model_matrix_base.MultiplyPoint(this.from);
					Vector3		p1 = this.db_draw.model_matrix_base.MultiplyPoint(this.to);

					Quaternion	q = Quaternion.FromToRotation(Vector3.right, p1 - p0);

					GL.PushMatrix();
					GL.Begin(GL.LINES);
					GL.Color(this.color);
					this.db_draw.draw_line_strip(new Vector3[] {p0, p1});
					GL.End();
					GL.PopMatrix();

					float		base_scale = 0.2f;

					float	scale = base_scale;

					this.material.SetColor("_Color", this.color);
					this.material.SetPass(0);
					Graphics.DrawMeshNow(Arrow.s_builder_cone.mesh, this.db_draw.model_matrix_base*Matrix4x4.TRS(p1, q, Vector3.one*scale));

					this.db_draw.line_material.SetPass(0);

				} else {
					//this.db_draw.draw_lines_with_width(s_x_axis, this.width, Color.red);
					//this.db_draw.draw_lines_with_width(s_y_axis, this.width, Color.green);
					//this.db_draw.draw_lines_with_width(s_z_axis, this.width, Color.blue);
				}
			}
		}

		protected static BuildMesh.Cone		s_builder_cone;

		public static void	createStaticData()
		{
			Arrow.s_builder_cone  = new BuildMesh.Cone();

			float	radius = 0.1f;
			float	height = 0.5f;

			Arrow.s_builder_cone.createVertices(radius, height, 12);
			Arrow.s_builder_cone.applyTransform(Matrix4x4.TRS(Vector3.right*-height, Quaternion.Euler(0.0f, 0.0f, -90.0f), Vector3.one));
			Arrow.s_builder_cone.makeSolid();
			Arrow.s_builder_cone.bakeLighitingToVertexColor(new Vector3(-1.0f, -1.0f, 1.0f), 0.8f, 0.2f);
			Arrow.s_builder_cone.instantiate();
		}
	}

	// ================================================================ //
	// グリッド描画.

	public class GridXZ {
			
		public class Command : CommandBase {

			public float	x_size      = 1.0f;
			public float	z_size      = 1.0f;
			public float	grid_length = 1.0f;

			public Command(dbDraw db_draw, float x_size, float z_size, float grid_length) : base(db_draw)
			{
				this.x_size = x_size;
				this.z_size = z_size;
				this.grid_length = grid_length;
			}

			public override void	draw()
			{
				int			i;
				int			divs_x, divs_z;

				divs_x = (int)Mathf.Abs(((this.x_size/2.0f)/this.grid_length));
				divs_z = (int)Mathf.Abs(((this.z_size/2.0f)/this.grid_length));

				float	x0 = -this.x_size/2.0f;
				float	x1 =  this.x_size/2.0f;
				float	z0 = -this.z_size/2.0f;
				float	z1 =  this.z_size/2.0f;

				GL.Begin(GL.LINES);

				// ---------------------------------------------------------------- //

				GL.Color(Color.white);

				// Ｚ軸と平行.
				for(i = 1;i < divs_x + 1;i++) {
					GL.Vertex3( (float)i*this.grid_length, 0.0f, z0);
					GL.Vertex3( (float)i*this.grid_length, 0.0f, z1);
					GL.Vertex3(-(float)i*this.grid_length, 0.0f, z0);
					GL.Vertex3(-(float)i*this.grid_length, 0.0f, z1);
				}

				// Ｘ軸と平行.
				for(i = 1;i < divs_z + 1;i++) {
					GL.Vertex3(x0, 0.0f,  (float)i*this.grid_length);
					GL.Vertex3(x1, 0.0f,  (float)i*this.grid_length);
					GL.Vertex3(x0, 0.0f, -(float)i*this.grid_length);
					GL.Vertex3(x1, 0.0f, -(float)i*this.grid_length);
				}

				// あまり（Ｚ軸と平行）.
				if(Mathf.Abs(this.x_size/2.0f - divs_x*this.grid_length) > float.Epsilon) {
					GL.Vertex3( x1, 0.0f, z0);
					GL.Vertex3( x1, 0.0f, z1);
					GL.Vertex3(-x1, 0.0f, z0);
					GL.Vertex3(-x1, 0.0f, z1);
				}

				// あまり（Ｘ軸と平行）.
				if(Mathf.Abs(this.z_size/2.0f - divs_z*this.grid_length) > float.Epsilon) {
					GL.Vertex3(x0, 0.0f,  z1);
					GL.Vertex3(x1, 0.0f,  z1);
					GL.Vertex3(x0, 0.0f, -z1);
					GL.Vertex3(x1, 0.0f, -z1);
				}

				// ---------------------------------------------------------------- //
				// X/Z 軸.

				GL.Color(Color.red);
				GL.Vertex3(x0, 0.0f, 0.0f);
				GL.Vertex3(x1, 0.0f, 0.0f);

				GL.Color(Color.blue);
				GL.Vertex3(0.0f, 0.0f, z0);
				GL.Vertex3(0.0f, 0.0f, z1);

				// ---------------------------------------------------------------- //

				GL.End();
			}
		}
	}

	// ================================================================ //
	// 2D ライン１本.

	public class Line2D {

		public class Command : CommandBase {

			public Vector3[]	positions;
			public float		width = 1.0f;

			public Command(dbDraw db_draw, Vector3 position0, Vector3 position1, Color color, float width) : base(db_draw)
			{
				this.positions = new Vector3[2];

				this.positions[0] = this.db_draw.model_matrix_base.MultiplyPoint(position0);
				this.positions[1] = this.db_draw.model_matrix_base.MultiplyPoint(position1);
				//this.positions[0] = new Vector3(this.positions[0].x/Screen.width + 0.5f, this.positions[0].y/Screen.height + 0.5f, 0.0f);
				//this.positions[1] = new Vector3(this.positions[1].x/Screen.width + 0.5f, this.positions[1].y/Screen.height + 0.5f, 0.0f);

				this.width     = width;
				this.color     = color;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					this.db_draw.draw_lines_2d(this.positions, this.color);

				} else {
					// 未実装.
				}
			}
		}
	}

	// ================================================================ //
	// 2D ライン　たくさん.

	public class Lines2D {

		public class Command : CommandBase {

			public Vector3[]	positions;
			public float		width = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, Color color, float width) : base(db_draw)
			{
				this.positions = new Vector3[positions.Length];

				for(int i = 0;i < positions.Length;i++) {
					this.positions[i] = this.db_draw.model_matrix_base.MultiplyPoint(positions[i]);
					//this.positions[i] = new Vector3(this.positions[i].x/this.db_draw.buffer_size.x + 0.5f, this.positions[i].y/this.db_draw.buffer_size.y + 0.5f, 0.0f);
				}

				this.width     = width;
				this.color     = color;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					this.db_draw.draw_lines_2d(this.positions, this.color);

				} else {
					// 未実装.
				}
			}
		}
	}

	// ================================================================ //
	// 2D ライン　ストリップ.

	public class LineStrip2D {

		public class Command : CommandBase {

			public Vector3[]	positions;
			public bool			is_looped = false;
			public float		width = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, bool is_looped, Color color, float width) : base(db_draw)
			{
				this.positions = new Vector3[positions.Length];

				for(int i = 0;i < positions.Length;i++) {
					this.positions[i] = this.db_draw.model_matrix_base.MultiplyPoint(positions[i]);
					//this.positions[i] = new Vector3(this.positions[i].x/this.db_draw.buffer_size.x + 0.5f, this.positions[i].y/this.db_draw.buffer_size.y + 0.5f, 0.0f);
				}

				this.is_looped = is_looped;
				this.width     = width;
				this.color     = color;
			}

			public override void	draw()
			{
				if(this.width <= 1.0f) {

					this.db_draw.draw_line_strip_2d(this.positions, this.color);

				} else {
					// 未実装.
				}
			}
		}
	}

	// ================================================================ //
	// 2D のポイント　たくさん.

	public class Points2D {

		public class Command : CommandBase {

			public Vector3[]	positions      = null;
			public float		size_in_screen = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, Color color, float size_in_screen) : base(db_draw)
			{
				this.positions = new Vector3[positions.Length];

				System.Array.Copy(positions, this.positions, positions.Length);

				this.convert_position_all();

				this.size_in_screen = size_in_screen;
				this.color          = color;
			}


			protected void	convert_position_all()
			{
				for(int i = 0;i < positions.Length;i++) {
					this.positions[i] = this.db_draw.model_matrix_base.MultiplyPoint(this.positions[i]);
					//this.positions[i].x = this.positions[i].x/this.db_draw.buffer_size.x + 0.5f;
					//this.positions[i].y = this.positions[i].y/this.db_draw.buffer_size.y + 0.5f;
				}
			}

			public override void	draw()
			{
				//float	w = (this.size_in_screen/2.0f)/this.db_draw.buffer_size.x;
				//float	h = (this.size_in_screen/2.0f)/this.db_draw.buffer_size.y;
				float	w = this.size_in_screen/2.0f;
				float	h = this.size_in_screen/2.0f;

				GL.PushMatrix();
					//GL.LoadOrtho();
					this.db_draw.load_ortho();
					GL.Begin(GL.QUADS);
					GL.Color(color);

					for(int i = 0;i < this.positions.Length;i++) {

						GL.Vertex(this.positions[i] + new Vector3( w,  h, 0.0f));
						GL.Vertex(this.positions[i] + new Vector3( w, -h, 0.0f));
						GL.Vertex(this.positions[i] + new Vector3(-w, -h, 0.0f));
						GL.Vertex(this.positions[i] + new Vector3(-w,  h, 0.0f));
					}

					GL.End();
				GL.PopMatrix();
			}
		}

	} // class Square2D

	// ================================================================ //
	// 2D ワイヤーフレームの四角形を描く.

	public class WireSquare2D {

		public class Command : CommandBase {

			public Vector3[]	positions      = null;
			public float		line_width = 1.0f;

			public Command(dbDraw db_draw, Vector3[] positions, Color color, float line_width) : base(db_draw)
			{
				this.positions = new Vector3[5];

				for(int i = 0;i < 4;i++) {
					this.positions[i] = positions[i];
				}
				this.positions[4] = positions[0];

				this.color      = color;
				this.line_width = line_width;

				//this.convert_position_all();
			}

			public Command(dbDraw db_draw, Vector3 center, float width, float height, Color color, float line_width) : base(db_draw)
			{
				this.positions    = new Vector3[5];
				this.positions[0] = center + new Vector3( width/2.0f,  height/2.0f, 0.0f);
				this.positions[1] = center + new Vector3( width/2.0f, -height/2.0f, 0.0f);
				this.positions[2] = center + new Vector3(-width/2.0f, -height/2.0f, 0.0f);
				this.positions[3] = center + new Vector3(-width/2.0f,  height/2.0f, 0.0f);
				this.positions[4] = this.positions[0];
				this.color        = color;
				this.line_width   = line_width;

				//this.convert_position_all();
			}

			protected void	convert_position_all()
			{
				// 
				for(int i = 0;i < positions.Length;i++) {
					this.positions[i].x = this.positions[i].x/this.db_draw.buffer_size.x + 0.5f;
					this.positions[i].y = this.positions[i].y/this.db_draw.buffer_size.y + 0.5f;
				}
			}

			public override void	draw()
			{
				if(this.line_width <= 1.0f) {

					this.db_draw.draw_line_strip_2d(this.positions, this.color);

				} else {
					// 未実装.
				}
			}
		}

	} // class WireSquare2D

	// ================================================================ //
	// 2D の四角形を描く.

	public class Square2D {

		public class Command : CommandBase {

			public Vector3[]	positions      = null;

			public Command(dbDraw db_draw, Vector3[] positions, Color color) : base(db_draw)
			{
				this.positions = positions;
				this.color     = color;

				this.convert_position_all();
			}

			public Command(dbDraw db_draw, Vector3 center, float width, float height, Color color) : base(db_draw)
			{

				this.positions = new Vector3[4];
				this.positions[0] = center + new Vector3( width/2.0f,  height/2.0f, 0.0f);
				this.positions[1] = center + new Vector3( width/2.0f, -height/2.0f, 0.0f);
				this.positions[2] = center + new Vector3(-width/2.0f, -height/2.0f, 0.0f);
				this.positions[3] = center + new Vector3(-width/2.0f,  height/2.0f, 0.0f);
				this.color     = color;

				this.convert_position_all();
			}

			protected void	convert_position_all()
			{
				// 
				for(int i = 0;i < positions.Length;i++) {
					this.positions[i] = this.db_draw.model_matrix_base.MultiplyPoint(this.positions[i]);
					//this.positions[i].x = this.positions[i].x/this.db_draw.buffer_size.x + 0.5f;
					//this.positions[i].y = this.positions[i].y/this.db_draw.buffer_size.y + 0.5f;
				}
			}

			public override void	draw()
			{
				GL.PushMatrix();
					//GL.LoadOrtho();
					this.db_draw.load_ortho();
					GL.Begin(GL.QUADS);
					GL.Color(color);

					GL.Vertex(this.positions[0]);
					GL.Vertex(this.positions[1]);
					GL.Vertex(this.positions[2]);
					GL.Vertex(this.positions[3]);

					GL.End();
				GL.PopMatrix();
			}
		}

	} // class Square2D

	// ================================================================ //
	// 2D ワイヤーフレームのまるを描く.

	public class WireCircle2D {

		protected const int			s_division  = 8;
		protected static Vector3[]	s_positions = null;

		public class Command : CommandBase {

			public Vector3[]	positions      = null;
			public float		line_width = 1.0f;

			public Command(dbDraw db_draw, Vector3 center, float radius, Color color, float line_width) : base(db_draw)
			{
				this.positions = new Vector3[WireCircle2D.s_division + 1];

				for(int i = 0;i < s_division;i++) {

					this.positions[i] = WireCircle2D.s_positions[i];
					this.positions[i] *= radius;
					this.positions[i] += center;
				}

				this.positions[WireCircle2D.s_division] = this.positions[0];

				this.color        = color;
				this.line_width   = line_width;

				this.convert_position_all();
			}

			protected void	convert_position_all()
			{
				// 
				for(int i = 0;i < positions.Length;i++) {
					this.positions[i] = this.db_draw.model_matrix_base.MultiplyPoint(this.positions[i]);
					//this.positions[i].x = this.positions[i].x/this.db_draw.buffer_size.x + 0.5f;
					//this.positions[i].y = this.positions[i].y/this.db_draw.buffer_size.y + 0.5f;
				}
			}

			public override void	draw()
			{
				if(this.line_width <= 1.0f) {

					this.db_draw.draw_line_strip_2d(this.positions, this.color);

				} else {
					// 未実装.
				}
			}
		}

		public static void	createStaticData()
		{
			s_positions = new Vector3[s_division];

			for(int i = 0;i < s_division;i++) {

				s_positions[i] = Quaternion.Euler(0.0f, 0.0f, 360.0f*((float)i)/((float)s_division))*Vector3.right;
			}
		}

	} // class WireSquare2D

	// ================================================================ //
	// 2D テクスチャーを描く.

	public class DrawTexture2D {

		public class Command : CommandBase {

			public Vector3[]	positions = null;
			public Texture2D	texture   = Texture2D.whiteTexture;
			public Material		material;

			public Command(dbDraw db_draw, Texture2D texture, Vector3 center, float width, float height, Color color) : base(db_draw)
			{
				this.texture = texture;

				this.positions    = new Vector3[4];
				this.positions[0] = center + new Vector3( width/2.0f,  height/2.0f, 0.0f);
				this.positions[1] = center + new Vector3( width/2.0f, -height/2.0f, 0.0f);
				this.positions[2] = center + new Vector3(-width/2.0f, -height/2.0f, 0.0f);
				this.positions[3] = center + new Vector3(-width/2.0f,  height/2.0f, 0.0f);
				this.color        = color;

				this.material = this.db_draw.allocate_dynamic_material(this.db_draw.texture_material);

				//this.convert_position_all();
			}

			protected void	convert_position_all()
			{
				// 
				for(int i = 0;i < positions.Length;i++) {
					this.positions[i].x = this.positions[i].x/this.db_draw.buffer_size.x + 0.5f;
					this.positions[i].y = this.positions[i].y/this.db_draw.buffer_size.y + 0.5f;
				}
			}

			public override void	draw()
			{
				this.material.SetColor("_Color", this.color);
				this.material.SetTexture("_MainTex", this.texture);
				this.material.SetPass(0);


				GL.PushMatrix();
					//GL.LoadOrtho();
					this.db_draw.load_ortho();
					GL.Begin(GL.QUADS);
					GL.Color(color);

					GL.TexCoord(new Vector3(1.0f, 1.0f));GL.Vertex(this.positions[0]);
					GL.TexCoord(new Vector3(1.0f, 0.0f));GL.Vertex(this.positions[1]);
					GL.TexCoord(new Vector3(0.0f, 0.0f));GL.Vertex(this.positions[2]);
					GL.TexCoord(new Vector3(0.0f, 1.0f));GL.Vertex(this.positions[3]);

					GL.End();
				GL.PopMatrix();

				this.db_draw.line_material.SetPass(0);
			}
		}

	} // class DrawTexture2D

	// ================================================================ //
	// レンダーターゲットをセットする.

	public class SetRenderTarget {

		public class Command : CommandBase {

			public RenderTexture	rt = null;

			public Command(dbDraw db_draw, RenderTexture rt) : base(db_draw)
			{
				this.rt = rt;
			}

			public override void	draw()
			{
				Graphics.SetRenderTarget(this.rt);

				if(this.rt != null) {
					this.db_draw.buffer_size.x = this.rt.width;
					this.db_draw.buffer_size.y = this.rt.height;
				} else {
					this.db_draw.buffer_size.x = Screen.width;
					this.db_draw.buffer_size.y = Screen.height;
				}
			}
		}

	} // class SetRenderTarget

	// ================================================================ //
	// バッファーをクリアーする.

	public class ClearBuffer {

		public class Command : CommandBase {

			public Command(dbDraw db_draw, Color color) : base(db_draw)
			{
				this.color = color;
			}

			public override void	draw()
			{
				GL.Clear(false, true, this.color);
			}
		}

	} // class SetRenderTarget

	// ================================================================ //

} // class dbDraw

