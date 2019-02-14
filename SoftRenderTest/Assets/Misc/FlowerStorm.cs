using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FlowerStorm {

public class Root {


	//public const int	PARTICLE_NUM = 200;
	public const int	PARTICLE_NUM = 1;
	public const int	SHAPE_VERTEX_NUM = 4;

	public List<Particle>		particles;
	//public List<Vertex>	VertexArray;

	public List<Vector3>		positions;

	public struct ShapeVertex {

		public Vector2		uv;
		public Vector3		pos;

		public ShapeVertex(Vector2 uv, Vector3 pos)
		{
			this.uv = uv;
			this.pos = pos;
		}
	}

	public static ShapeVertex[]	s_shape;

	public const float		TEX_SIZE = 0.10f;
	public const float		MAX_SPEED = 0.61f;
	public const float		VERTICAL_SPACE = 0.1f;		// 断面積.

	// ================================================================ //

	public Root()
	{
		s_shape = new ShapeVertex[SHAPE_VERTEX_NUM];

		s_shape[0] = new ShapeVertex(new Vector2(0.0f, 1.0f), new Vector3( -TEX_SIZE/2.0f, 0.0f,  TEX_SIZE/2.0f));
		s_shape[1] = new ShapeVertex(new Vector2(1.0f, 1.0f), new Vector3(  TEX_SIZE/2.0f, 0.0f,  TEX_SIZE/2.0f));
		s_shape[2] = new ShapeVertex(new Vector2(0.0f, 0.0f), new Vector3( -TEX_SIZE/2.0f, 0.0f, -TEX_SIZE/2.0f));
		s_shape[3] = new ShapeVertex(new Vector2(1.0f, 0.0f), new Vector3(  TEX_SIZE/2.0f, 0.0f, -TEX_SIZE/2.0f));

		this.initialize();
	}
	
	public void		initialize()
	{
		this.particles = new List<Particle>();
		this.positions = new List<Vector3>();

		for(int i = 0;i < PARTICLE_NUM;i++) {

			Particle	particle = new Particle();

			this.particles.Add(particle);

			for(int j = 0;j < SHAPE_VERTEX_NUM;j++) {

				this.positions.Add(Vector3.zero);
			}
		}

		this.wind = Vector3.zero;

		this.view_volume = new ViewVolumeShape();
		this.clipper = new ViewVolumeClipper();
	}
	public void		create()
	{
		ViewVolumeShape		view_volume_shape = new ViewVolumeShape();

		for(int i = 0;i < this.particles.Count;i++) {

			Particle	particle = this.particles[i];

			particle.create(view_volume_shape, 0.1f, 10000.0f);

			particle.pos = new Vector3(0.0f, 1.0f, 0.0f);
		}
	}

	public void		debugDraw()
	{
		Vector3[]	positions = new Vector3[SHAPE_VERTEX_NUM];

		for(int i = 0;i < PARTICLE_NUM;i++) {

			positions[0] = this.positions[i*SHAPE_VERTEX_NUM + 0];
			positions[1] = this.positions[i*SHAPE_VERTEX_NUM + 1];
			positions[2] = this.positions[i*SHAPE_VERTEX_NUM + 3];
			positions[3] = this.positions[i*SHAPE_VERTEX_NUM + 2];

			dbDraw.get().drawLineStrip(positions, true, Color.red);
		}
	}
	public void		execute(float delta_time)
	{
/*
		Root			*root = this;
		Matrix4x4f		cam_mat_inv;
		Matrix4x4f		cam_mat_diff;
		int				i;
		Particle		*pt;
		int				fail_safe_count = 0;
		Vector3f		gravity;

		root->updateClipper();

		// ------------------------------------------------------------ //

		root->camera_matrix.previous = root->camera_matrix.current;
		root->camera_matrix.current  = root->camera_matrix.next;

		cam_mat_inv = Matrix4x4f::inverse(root->camera_matrix.current);

		// ------------------------------------------------------------ //
		// ビューボリュームによるクリップ

		cam_mat_diff = root->camera_matrix.previous;
		cam_mat_diff.inverse();
		cam_mat_diff.multiply(root->camera_matrix.current);
		cam_mat_diff.inverse();

		for(i = 0;i < root->particles.size();i++) {

			pt = &root->particles[i];

			pt->cam_local_prev = pt->cam_local;

			pt->cam_local= cam_mat_inv.transform(pt->pos);

			if(pt->clip(root->clipper, root->view_volume, root->z_near, root->z_far, cam_mat_diff)) {

				pt->restart();
			}

			if(pt->fail_safe_clipped) {

				fail_safe_count++;
			}
		}

		for(i = 0;i < root->particles.size();i++) {

			pt = &root->particles[i];

			pt->pos = root->camera_matrix.current.transform(pt->cam_local);
		}


		// ------------------------------------------------------------ //
*/
		for(int		i = 0;i < this.particles.Count;i++) {

			Particle	particle = this.particles[i];

			Vector3		gravity = (new Vector3(0.0f, -9.8f, 0.0f));

			particle.vel += gravity*delta_time;

			// ---------------------------------------------------- //
			// 空気抵抗（＆風の影響）
			//
			//		パーティクルの法線と速度（風ベクトルに対する相対速度）
			//		の向きが近いほど、大きな抵抗を受ける
/*
			Vector3		u, v;
			Vector3		v_resist;
			float		max_speed;
			float		resist;

			max_speed = MAX_SPEED;
			resist = 1.0f - (max_speed/(max_speed + 0.98f*delta_time));

			v_resist = particle.vel -  this.wind;

			v_resist *= resist;

			//Math::Geometry::decompose(&u, &v, NULL, v_resist, pt->normal);
			u = Vector3.Project(v_resist, particle.normal);
			v = v_resist - u;

			v *= VERTICAL_SPACE;

			v_resist = u + v;

			particle.vel -= v_resist;
*/
// v = v + k(v - va + g*dt)*dt + g*dt;
// vc = vc + k*(max + g*dt)*dt + g*dt;
// 0.0 = k(max + g*dt) + g;

			float		resist = -9.8f/(1.0f + 9.8f*delta_time);

			particle.vel += resist*particle.vel*delta_time;

			// ---------------------------------------------------- //
			// 位置や角度の更新
dbPrint.setLocate(5, 5);
dbPrint.print(particle.vel.magnitude);

			particle.pos += particle.vel*delta_time;

//			particle.spin.z_ang = MathUtility.snormDegree(particle.spin.z_ang + particle.spin.z_omega*delta_time);

if(particle.pos.y < 0.0f) {
	particle.pos.y += 1.0f;
}

		}

		//root->camera_matrix.next = root->camera_matrix.current;


		this.transformVertices();
	}

	public void		transformVertices()
	{
		for(int i = 0;i < PARTICLE_NUM;i++) {

			Particle	particle = this.particles[i];

			Quaternion	q = Quaternion.Euler(particle.rot.x, particle.rot.y, particle.rot.z);

			q *= Quaternion.Euler(0.0f, particle.spin.y_ofst, 0.0f);
			q *= Quaternion.Euler(0.0f, 0.0f, particle.spin.z_ang);
			q *= Quaternion.Euler(0.0f, -particle.spin.y_ofst, 0.0f);

			particle.normal = q*Vector3.up;

			for(int j = 0;j < SHAPE_VERTEX_NUM;j++) {

				Vector3		p = s_shape[j].pos;

				p = q*p;
				p = particle.pos + p;

				this.positions[i*SHAPE_VERTEX_NUM + j] = p;
			}
		}
	}

	public void		updateClipper()
	{
		this.calcViewvolumeShape();
		this.createClipper();
	}
	public void		calcViewvolumeShape()
	{
		Root.calcViewVolumeShape(this.view_volume, this.fovy, this.aspect);
	}
	public void	createClipper()
	{
		Root.createClipper(this.clipper, this.view_volume, this.z_near, this.z_far);
	}

	public static void	calcViewVolumeShape(ViewVolumeShape shape, float fovy, float aspect)
	{
		Vector3		v;

		v.z = 1.0f;
		v.y = Mathf.Tan(fovy*Mathf.Deg2Rad/2.0f);
		v.x = v.y*aspect;

		shape.rt = v;

		shape.rb = v;
		shape.rb.y *= -1.0f;

		shape.lt = v;
		shape.lt.x *= -1.0f;

		shape.lb = v;
		shape.lb.x *= -1.0f;
		shape.lb.y *= -1.0f;
	}

	// クリッパー（クリップ平面＝ビューボリュームの各面）を作る.
	public static void	createClipper(ViewVolumeClipper clipper, ViewVolumeShape view_volume, float z_min, float z_max)
	{
		// left

		clipper.plane[(int)VIEW_VOLUME_PLANE.LEFT].tri[0] = Vector3.zero;
		clipper.plane[(int)VIEW_VOLUME_PLANE.LEFT].tri[1] = view_volume.lb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.LEFT].tri[2] = view_volume.lt;
		clipper.plane[(int)VIEW_VOLUME_PLANE.LEFT].is_rect = false;

		// right

		clipper.plane[(int)VIEW_VOLUME_PLANE.RIGHT].tri[0] = Vector3.zero;
		clipper.plane[(int)VIEW_VOLUME_PLANE.RIGHT].tri[1] = view_volume.rt;
		clipper.plane[(int)VIEW_VOLUME_PLANE.RIGHT].tri[2] = view_volume.rb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.RIGHT].is_rect = false;

		// top

		clipper.plane[(int)VIEW_VOLUME_PLANE.TOP].tri[0] = Vector3.zero;
		clipper.plane[(int)VIEW_VOLUME_PLANE.TOP].tri[1] = view_volume.lt;
		clipper.plane[(int)VIEW_VOLUME_PLANE.TOP].tri[2] = view_volume.rt;
		clipper.plane[(int)VIEW_VOLUME_PLANE.TOP].is_rect = false;

		// bottom

		clipper.plane[(int)VIEW_VOLUME_PLANE.BOTTOM].tri[0] = Vector3.zero;
		clipper.plane[(int)VIEW_VOLUME_PLANE.BOTTOM].tri[1] = view_volume.rb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.BOTTOM].tri[2] = view_volume.lb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.BOTTOM].is_rect = false;

		// near

		clipper.plane[(int)VIEW_VOLUME_PLANE.NEAR].tri[0] = view_volume.rb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.NEAR].tri[1] = view_volume.lb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.NEAR].tri[2] = view_volume.rt;

		for(int i = 0;i < 3;i++) {

			clipper.plane[(int)VIEW_VOLUME_PLANE.NEAR].tri[i] = clipper.plane[(int)VIEW_VOLUME_PLANE.NEAR].tri[i]*z_min;
		}

		clipper.plane[(int)VIEW_VOLUME_PLANE.NEAR].is_rect = true;

		// far

		clipper.plane[(int)VIEW_VOLUME_PLANE.FAR].tri[0] = view_volume.lb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.FAR].tri[1] = view_volume.rb;
		clipper.plane[(int)VIEW_VOLUME_PLANE.FAR].tri[2] = view_volume.lt;

		for(int i = 0;i < 3;i++) {

			clipper.plane[(int)VIEW_VOLUME_PLANE.FAR].tri[i] = clipper.plane[(int)VIEW_VOLUME_PLANE.FAR].tri[i]*z_max;
		}

		clipper.plane[(int)VIEW_VOLUME_PLANE.FAR].is_rect = true;

		// ------------------------------------------------------------ //

		for(int i = 0;i < (int)VIEW_VOLUME_PLANE.NUM;i++) {

			ViewVolumeClipper.Plane	plane = clipper.plane[i];

			if(!MathUtility.calcPolygonNormal(ref plane.normal, plane.tri)) {
				plane.normal = Vector3.up;
			}

			plane.space = MathUtility.calcTriSpace(plane.tri, plane.is_rect);

			if(i == (int)VIEW_VOLUME_PLANE.NEAR || i == (int)VIEW_VOLUME_PLANE.FAR) {

			} else {

				// NEAR と FAR 以外は Z の範囲が 0.0 ～ 1.0 なので。
				// （本当は zmin ～ zmax の大きさ）
				plane.space *= MathUtility.square(z_max);
			}
		}
	}

#if false
	public:

		Particle	*appendParticle(void);
		int			eraseParticle(int erase_num);

		int		getParticleNumber(void) const;

		void	setCameraPostureMatrix(const Matrix4x4f& matrix);

	public:
#endif

#if false
		struct {

			Matrix4x4f			previous;		//!< 前フレームのカメラマトリックス
			Matrix4x4f			current;
			Matrix4x4f			next;

		} camera_matrix;
#endif
		//! far、near クリップするカメラからの距離（＞０）
		public float				z_near;
		public float				z_far;

		public float				fovy;				//!< [degree]
		public float				aspect;

		public ViewVolumeShape		view_volume;
		public ViewVolumeClipper	clipper;

		public Vector3			wind;

}

//! パーティクル
public class Particle {

	public Particle()
	{
		init();
	}

	public void		init()
	{
	}
	public void		create(ViewVolumeShape view_volume, float z_near, float z_far/*, const Matrix4x4f& matrix*/)
	{
		int			i;
		float		all;
		float[]		ratio = new float[3];
		float		param_min, param_max;

		param_min = Mathf.Abs(z_near);
		param_max = Mathf.Abs(z_far);

		// ・min <= (ratio[0] + ratio[1] + ratio[2]) <= max
		// ・ratio[0] ～ ratio[2] はランダム
		//
		all = Random.Range(param_min, param_max);

		ratio[0] = Random.value*all;
		ratio[1] = Random.value*(all - ratio[0]);
		ratio[2] = all - (ratio[0] + ratio[1]);

		this.cam_local = Vector3.zero;

		if(Random.value < 0.5f) {

			this.cam_local = this.cam_local + view_volume.rt*ratio[0];
			this.cam_local = this.cam_local + view_volume.rb*ratio[1];
			this.cam_local = this.cam_local + view_volume.lt*ratio[2];

		} else {

			this.cam_local = this.cam_local + view_volume.lt*ratio[0];
			this.cam_local = this.cam_local + view_volume.lb*ratio[1];
			this.cam_local = this.cam_local + view_volume.rb*ratio[2];
		}

		this.cam_local_prev = this.cam_local;

		//this.pos = matrix.transform(this.cam_local);

		// ------------------------------------------------------------ //

		this.rot.y = Mathf.Lerp(-180.0f, 180.0f, Random.value);
		this.rot.x = Mathf.Lerp(0.0f, 30.0f, Random.value);
		this.rot.z = 0.0f;

		this.spin.y_ofst  = Mathf.Lerp(-10.0f, 10.0f, Random.value);
		this.spin.z_omega = Mathf.Lerp(10.0f, 45.0f, Random.value);
		this.spin.z_ang   = 0.0f;
/*
		// スピードの初期値を求める（近似計算です）
		this.calc_init_speed();

		// ------------------------------------------------------------ //

		this.set_color(Color.white);

		// ------------------------------------------------------------ //

		for(i = 0;i < SHAPE_VERTEX_NUM;i++) {

			this.vertices[i].uv.u = shape[i].uv.u;
			this.vertices[i].uv.v = shape[i].uv.v;
		}
*/
	}

#if false


	public:
		void	create(const ViewVolumeShape& view_volume, float z_near, float z_far, const Matrix4x4f& matrix);
		void	restart(void);

		void	calc_init_speed(void);

		bool	clip(const ViewVolumeClipper& clipper, const ViewVolumeShape& view_volume, float z_near, float z_far, const Matrix4x4f& cam_mat_diff_inv);

		void	set_color(const Color4f& col);

	protected:
		static bool		is_param_within_tri(float param[2], float min, float max);
		static bool 	is_param_within_rect(float param[2]);

	public:
#endif

		public Vector3	pos;
		public Vector3	rot;
		public Vector3	normal;

		public Vector3	vel;

		Color		color;
#if false
		Vertex		*vertices;
#endif
		Vector3		cam_local;
		Vector3		cam_local_prev;

		public struct Spin {

			public float		y_ofst;
			public float		z_ang;
			public float		z_omega;

		}
		public Spin		spin;
#if false
		bool		fail_safe_clipped;
#endif
};

// ビューボリューム（四角錐）.
public class ViewVolumeShape {

	// ビューボリュームの各エッジ（z = -1.0f であること）.
	// 長方形じゃないとだめ.
	public Vector3		lt, lb, rt, rb;
}

public enum VIEW_VOLUME_PLANE {

	NONE = -1,

	LEFT = 0,
	RIGHT,
	TOP,
	BOTTOM,
	NEAR,
	FAR,

	NUM,
};

//! ビューボリュームのクリップ面
public class ViewVolumeClipper {

	public class Plane {

		public Vector3[]	tri = new Vector3[3];	// PLANE_NEAR と PLANE_FAR 以外は、z = 0.0f ～ 1.0f.
		public Vector3		normal;

		public bool			is_rect;			// 四角形？.

		public float		space;				// 面積.

		public Plane()
		{
			this.tri = new Vector3[3];
			this.normal = Vector3.up;
			this.is_rect = true;
			this.space = 1.0f;
		}
	}

	public Plane[]	plane = new Plane[(int)VIEW_VOLUME_PLANE.NUM];

	public ViewVolumeClipper()
	{
		this.plane = new Plane[(int)VIEW_VOLUME_PLANE.NUM];

		for(int i = 0;i < this.plane.Length;i++) {

			this.plane[i] = new Plane();
		}
	}
};

}


