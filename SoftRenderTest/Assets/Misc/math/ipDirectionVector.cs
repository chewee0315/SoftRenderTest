using UnityEngine;
using System.Collections;

namespace ipModule {

public class DirectionVector {

	protected struct Direction {

		public Vector3	start;
		public Vector3	goal;
		public Vector3	target;

		public Vector3	current;
	}
	protected Direction	direction = new Direction();

	protected struct Pivot {

		public Vector3	axis;
		public float	angle;
	}
	protected Pivot		start_goal_pivot = new Pivot();

	public struct Omega {

		public float	max;
		public float	min;
		public float	current;
		public float	next;
	}
	public Omega	omega = new Omega();

	protected struct PassageTime {

		public float	current;
		public float	duration;
	}
	protected PassageTime		passage_time = new PassageTime();

	protected struct Status {

		public bool		is_moving_to_goal;
		public bool		is_target_following;
	}
	protected Status	status = new Status();

	// ================================================================ //

	public DirectionVector()
	{
		this.direction.start   = Vector3.right;
		this.direction.goal    = this.direction.start;
		this.direction.target  = this.direction.start;
		this.direction.current = this.direction.start;

		this.start_goal_pivot.axis  = Vector3.right;
		this.start_goal_pivot.angle = 0.0f;

		this.omega.max     = 360.0f;
		this.omega.min     = this.omega.max*0.2f;
		this.omega.current = this.omega.min;
		this.omega.next    = this.omega.current;

		this.passage_time.current = 0.0f;
		this.passage_time.duration = 1.0f;

		this.status.is_moving_to_goal = false;
		this.status.is_target_following = false;
	}

	public void		setCurrent(Vector3 current)
	{
		this.direction.current = current;

		this.status.is_moving_to_goal = true;
		this.status.is_target_following = true;
	}
	public Vector3	getCurrent()
	{
		return(this.direction.current);
	}

	public Vector3	getStart()
	{
		return(this.direction.start);
	}

	public Vector3	getGoal()
	{
		return(this.direction.goal);
	}

	public void		setTarget(Vector3 target)
	{
		this.direction.target = target;

		this.status.is_target_following = true;
	}
	public Vector3	getTarget()
	{
		return(this.direction.target);
	}

	public void		startWithDuration(Vector3 goal, float duration)
	{
		this.direction.start  = this.direction.current;
		this.direction.goal   = goal;
		this.direction.target = goal;

		this.calc_start_goal_pivot();

		this.omega.current = this.omega.min;
		this.omega.next    = this.omega.current;

		this.passage_time.current  = 0.0f;
		this.passage_time.duration = duration;

		this.status.is_moving_to_goal = true;
		this.status.is_target_following = true;
	}

	public void		startWithSpeed(Vector3 goal, float speed)
	{
		this.direction.start  = this.direction.current;
		this.direction.goal   = goal;
		this.direction.target = goal;

		this.calc_start_goal_pivot();

		this.omega.current = this.omega.min;
		this.omega.next    = this.omega.current;

		this.passage_time.current  = 0.0f;
		this.passage_time.duration = this.start_goal_pivot.angle/speed;

		this.status.is_moving_to_goal = true;
		this.status.is_target_following = true;
	}

	// ================================================================ //

	public void		execute(float delta_time)
	{
		this.execute_follow_target(delta_time);

		//

		if(this.passage_time.current >= this.passage_time.duration) {
			this.status.is_moving_to_goal = false;
		}

		if(!this.status.is_moving_to_goal) {

			this.direction.current = this.direction.goal;

		} else {

			float	ratio = this.passage_time.current/this.passage_time.duration;

			if(float.IsInfinity(ratio)) {
				ratio = 1.0f;
			}

			ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);
			ratio = Mathf.Sin(Mathf.PI/2.0f*ratio);

			this.direction.current = Quaternion.AngleAxis(this.start_goal_pivot.angle*ratio, this.start_goal_pivot.axis)*this.direction.start;

			//

			this.passage_time.current += delta_time;
		}
	}

	public void		execute_follow_target(float delta_time)
	{
		if(this.status.is_target_following) {

			Vector3		axis  = Vector3.Cross(this.direction.goal, this.direction.target);
			float		pinch = Vector3.Angle(this.direction.goal, this.direction.target);

			this.omega.next = pinch*4.0f;
			this.omega.next = Mathf.Clamp(this.omega.next, this.omega.min, this.omega.max);
			this.omega.next = pinch*4.0f;
			this.omega.next = Mathf.Clamp(this.omega.next, this.omega.min, this.omega.max);

			float	omega_acc = this.omega.next - this.omega.current;
			omega_acc = Mathf.Clamp(omega_acc, -this.omega.min*4.0f*delta_time, this.omega.min*4.0f*delta_time);

			this.omega.current = this.omega.current + omega_acc;

			axis.Normalize();

			float	angle = this.omega.current*delta_time;

			if(angle >= pinch) {
				this.direction.goal = this.direction.target;
				this.status.is_target_following = false;
			} else {
				this.direction.goal = Quaternion.AngleAxis(angle, axis)*this.direction.goal;
			}

			this.calc_start_goal_pivot();
		}
	}

	protected void	calc_start_goal_pivot()
	{
		this.start_goal_pivot.axis  = Vector3.Cross(this.direction.start, this.direction.goal);
		this.start_goal_pivot.angle = Vector3.Angle(this.direction.start, this.direction.goal);

		this.start_goal_pivot.axis.Normalize();

		if(this.start_goal_pivot.axis.sqrMagnitude == 0.0f) {

			this.start_goal_pivot.axis  = Vector3.right;
			this.start_goal_pivot.angle = 0.0f;
		}
	}
}

} // namespace ipModule

// テストコード.
#if false
	protected ipModule.DirectionVector		ip_dir_vector = new ipModule.DirectionVector();


		Vector3		p = Vector3.up;

		Ray		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Plane		plane = new Plane((Camera.main.transform.position - p).normalized, p);
		float		depth;
		Vector3		xp = Vector3.zero;

		if(plane.Raycast(ray, out depth)) {
			xp = ray.origin + ray.direction*depth;
		}

		Vector3		v = (xp - p).normalized;

		if(Input.GetMouseButtonDown(0)) {
			//this.ip_dir_vector.startWithDuration(v, 0.5f*4.0f);
			this.ip_dir_vector.startWithSpeed(v, 360.0f);
		}

		if(Input.GetMouseButton(1)) {
			this.ip_dir_vector.setTarget(v);
		}

		this.ip_dir_vector.execute(Time.deltaTime);

		//


		dbDraw.get().setModelMatrix(Matrix4x4.Translate(p));
		dbDraw.get().drawWireSphere(0.01f, Color.white, 2.0f);

		dbDraw.get().setModelMatrixIdentity();
		dbDraw.get().drawArrow(p, p + this.ip_dir_vector.getCurrent()*0.4f, ColorUtility.Konst.pink());
		dbDraw.get().drawArrow(p, p + this.ip_dir_vector.getStart()*0.4f, Color.cyan);
		dbDraw.get().drawArrow(p, p + this.ip_dir_vector.getTarget()*0.4f, Color.yellow);
		dbDraw.get().drawArrow(p, p + this.ip_dir_vector.getGoal()*0.4f, Color.blue);
#endif
