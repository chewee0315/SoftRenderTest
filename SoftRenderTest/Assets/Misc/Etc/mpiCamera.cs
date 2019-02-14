using UnityEngine;
using System.Collections;

public class mpiCamera : MonoBehaviour {
	
	public float		wheel_sense = 2.0f;		// ホイールの感度.
	public float		move_sense  = 0.01f;	// マウス移動の感度.
	public float		dolly_sense = 0.02f;	// ドリーのときのマウス移動の感度.
	
	public struct Config {
		
		public bool	is_controlable;
		public bool	is_use_alt_key;				// Alt キーを押してる間だけコントロールできる.
		public bool	is_enable_dolly_limit;
		public bool	is_chase_x;					// 位置追従のとき、Y 座標も追従する？.
		public bool	is_chase_y;					// 位置追従のとき、Y 座標も追従する？.
	};
	protected Config config;

	public struct Status {
		
		public bool	is_now_control;
	};
	protected Status	status;

	public struct Mouse {

		public struct Position {

			public Vector2	current;
			public Vector2	previous;
			public Vector2	move;
		};
		public Position	position;

		public struct Wheel {
			
			public float	delta;
		};
		public Wheel	wheel;
	};
	protected Mouse	mouse;

	public struct Posture {
		
		public Vector3	eye;
		public Vector3	interest;
		public Vector3	up;
	};
	protected Posture		posture;
	protected Posture		initial;					// resetPosture() したときの姿勢.
	
	protected Matrix4x4	matrix;							// カメラの姿勢マトリックス（view_matrix の逆行列）.
	protected Matrix4x4	view_matrix;
	
	public float	dolly_limit_near;
	public float	dolly_limit_far;

	protected Transform		chase_target;				// 位置追従するターゲット.
	protected Vector3		previous_position;			// chase_target の１フレーム前の位置.

	public struct InputStatus {

		public bool		alt_key;
		public bool		control_key;

		public bool		mouse_button_down0;
		public bool		mouse_button_down1;
		public bool		mouse_button_down2;

		public bool		mouse_button0;
		public bool		mouse_button1;
		public bool		mouse_button2;

		public Vector2	mouse_position;
		public float	wheel_delta;

		public void		clear()
		{
			this.alt_key     = false;
			this.control_key = false;

			this.mouse_button_down0 = false;
			this.mouse_button_down1 = false;
			this.mouse_button_down2 = false;

			this.mouse_button0 = false;
			this.mouse_button1 = false;
			this.mouse_button2 = false;

			this.mouse_position = Vector2.zero;
			this.wheel_delta = 0.0f;
		}
	}

	public delegate InputStatus		getInputDelegate();

	public getInputDelegate		getInput;
	
	public static InputStatus		getInputDefault()
	{
		InputStatus		input_status = new InputStatus();

		input_status.alt_key     = Input.GetKey(KeyCode.LeftAlt);
		input_status.control_key = Input.GetKey(KeyCode.LeftControl);

		input_status.mouse_button_down0 = Input.GetMouseButtonDown(0);
		input_status.mouse_button_down1 = Input.GetMouseButtonDown(1);
		input_status.mouse_button_down2 = Input.GetMouseButtonDown(2);

		input_status.mouse_button0 = Input.GetMouseButton(0);
		input_status.mouse_button1 = Input.GetMouseButton(1);
		input_status.mouse_button2 = Input.GetMouseButton(2);

		input_status.mouse_position = Input.mousePosition;
		input_status.wheel_delta = Input.GetAxisRaw("Mouse ScrollWheel");

		return(input_status);
	}

	// ================================================================ //

	void	Awake()
	{
		this.config.is_controlable        = true;
		this.config.is_use_alt_key        = true;
		this.config.is_enable_dolly_limit = true;
		this.config.is_chase_x            = true;
		this.config.is_chase_y            = true;

		this.status.is_now_control = false;

		this.posture.eye      = this.transform.position;
		//this.posture.interest = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
		this.posture.interest = this.transform.TransformPoint(Vector3.forward*10.0f);
		this.posture.up       = this.transform.up;

		this.dolly_limit_near = 1.0f;
		this.dolly_limit_far  = 100.0f;

		this.initial = this.posture;

		this.getInput = mpiCamera.getInputDefault;
	}

	void	Start()
	{
	}

	protected class AutoPilot {

		public bool			is_active;
		public Posture		start;
		public Posture		goal;

		public float		timer;
	}

	protected AutoPilot		auto_pilot = new AutoPilot();

	public void		startInterpolate(Posture goal)
	{
		this.auto_pilot.is_active = true;
		this.auto_pilot.start = this.getPosture();
		this.auto_pilot.goal = goal;
		this.auto_pilot.timer = 0.0f;
	}
	void	Update()
	{
		if(this.auto_pilot.is_active) {

			if(this.auto_pilot.timer >= 1.0f) {
				this.auto_pilot.is_active = false;
			}
		}

		if(this.auto_pilot.is_active) {

			Posture		posture;

			float		rate = Mathf.Clamp01(this.auto_pilot.timer);

			posture.eye = Vector3.Lerp(this.auto_pilot.start.eye, this.auto_pilot.goal.eye, rate);
			posture.interest = Vector3.Lerp(this.auto_pilot.start.interest, this.auto_pilot.goal.interest, rate);
			posture.up = Vector3.Lerp(this.auto_pilot.start.up, this.auto_pilot.goal.up, rate);
			posture.up.Normalize();

			this.setPosture(posture);

			this.auto_pilot.timer += Time.deltaTime;

		} else {
			if(this.config.is_controlable) {
				this.update_entity();
			}
		}
	}

	void LateUpdate()
	{
		// 位置追従.
		if(this.chase_target != null) {

			// ターゲットが移動したぶんだけ、平行移動.
			Vector3	move = this.chase_target.position - this.previous_position;

			if(!this.config.is_chase_x) {
				move.x = 0.0f;
			}
			if(!this.config.is_chase_y) {
				move.y = 0.0f;
			}

			this.parallelInterestTo(this.getPosture().interest + move);
			this.previous_position = this.chase_target.position;
		}
	}

	// ================================================================ //

	// 操作できる/できないをセットする.
	public void		SetControlable(bool is_controlable)
	{
		this.config.is_controlable = is_controlable;
	}

	// 操作中？.
	public bool		IsNowControlling()
	{
		return(this.status.is_now_control);
	}

	// Alt キーを使う？（false のときは、alt キーを押さなくても操作できる）
	public void		setUseAltKey(bool is_use)
	{
		this.config.is_use_alt_key = is_use;
	}

	// 位置追従するターゲットを設定する.
	public void		setChaseTarget(Transform chase_target)
	{
		this.chase_target = chase_target;
		if(this.chase_target != null) {
			this.previous_position = this.chase_target.position;
		}
	}

	// X 座標も追従する/しないをセットする.
	public void		SetChaseX(bool is_chase)
	{
		this.config.is_chase_x = is_chase;
	}
	// Y 座標も追従する/しないをセットする.
	public void		SetChaseY(bool is_chase)
	{
		this.config.is_chase_y = is_chase;
	}

	// カメラの姿勢を取得する.
	public Posture	getPosture()
	{
		return(this.posture);
	}

	// カメラの姿勢をセットする.
	public void		setPosture(Posture posture)
	{
		this.posture = posture;

		this.update_transform();			
	}

	// 注視点が指定の場所になるよう、平行移動する.
	public void		parallelInterestTo(Vector3 interest)
	{
		this.posture.eye += interest - this.posture.interest;
		this.posture.interest = interest;

		this.update_transform();			
	}

	// 注視点からの距離が指定の値になるよう、視点を前後させる.
	public void		dolly(float distance)
	{
		Vector3		eye_vector = this.posture.eye - this.posture.interest;

		eye_vector.Normalize();

		if(eye_vector.magnitude == 0.0f) {

			eye_vector = -this.transform.forward;
		}

		this.posture.eye = this.posture.interest + eye_vector*distance;

		this.update_transform();			
	}

	public float	calcFocalDistance()
	{
		return(Vector3.Distance(this.posture.eye, this.posture.interest));
	}

	public void		resetPosture()
	{
		this.posture = this.initial;

		this.update_transform();			
	}

	// ================================================================ //

	protected void	update_entity() 
	{
		float			dx, dy;
		Vector3			move;
		float			length;
		bool			is_updated;
	
		InputStatus		inps = this.getInput();

		this.mouse.position.previous = this.mouse.position.current;
		this.mouse.position.current  = inps.mouse_position;
		
		this.mouse.wheel.delta = inps.wheel_delta;

		float	depth_revise = Vector3.Distance(this.posture.interest, this.posture.eye);

		depth_revise = Mathf.Max(0.0f, depth_revise);
		
		if(!this.status.is_now_control) {
			
			do {
				
				if(this.config.is_use_alt_key) {
					if(!inps.alt_key) {				
						break;
					}
				}
				
				//
				
				this.status.is_now_control = true;
				
				if(inps.mouse_button_down0) {
					break;
				}
				if(inps.mouse_button_down2) {
					break;
				}
				if(inps.mouse_button_down1) {
					break;
				}
				
				this.status.is_now_control = false;
				
			} while(false);

			if(this.status.is_now_control) {
				this.mouse.position.previous = this.mouse.position.current;
			}

		} else {
			
			do {
				
				if(inps.mouse_button0) {				
					break;
				}
				if(inps.mouse_button2) {				
					break;
				}
				if(inps.mouse_button1) {				
					break;
				}
				
				this.status.is_now_control = false;
				
			} while(false);
			
			if(this.config.is_use_alt_key) {
				if(!inps.alt_key) {
					this.status.is_now_control = false;
				}
			}
		}

		this.mouse.position.move = this.mouse.position.current - this.mouse.position.previous;
		
		//
		
		do {
			
			is_updated = false;
			
			//

			if(mouse.wheel.delta != 0) {
				
				// 注視点に近づく／遠ざかる.

				this.calc_dolly(-mouse.wheel.delta*this.wheel_sense*depth_revise);
				
				is_updated = true;
			}
			
			//
			
			if(!this.status.is_now_control) {
				break;
			}
			
			dx = this.mouse.position.move.x;
			dy = this.mouse.position.move.y;

			if(inps.mouse_button0 && !inps.control_key) {

				// 注視点を中心に回転.
				this.rotate_around_interest(dx, -dy);
				
			} else if(inps.mouse_button2 || (inps.mouse_button0 && inps.control_key)) {

				// 平行移動　上下左右.
				move.x = -dx*this.move_sense;
				move.y = -dy*this.move_sense;
				move.z = 0.0f;
				move = this.transform.TransformVector(move);
	
				this.posture.interest += move;
				this.posture.eye      += move;
			
			} else if(inps.mouse_button1 && inps.control_key) {

				// 平行移動　前後左右.
				move.x = -dx*this.move_sense;
				move.y = 0.0f;
				move.z = 0.0f;
				move = this.transform.TransformVector(move);
	
				this.posture.interest += move;
				this.posture.eye      += move;

				// 前後（上下には動かない）.
				move = Vector3.Cross(this.transform.right, Vector3.up).normalized;
				move *= -dy*this.move_sense;

				this.posture.interest += move;
				this.posture.eye      += move;

			} else if(inps.mouse_button1) {
				
				// 注視点に近づく／遠ざかる.
						
				length = 0.0f;
				
				length += -dx*this.dolly_sense*depth_revise;
				length += -dy*this.dolly_sense*depth_revise;
				
				this.calc_dolly(length);
			
			} else {
				
				break;
			}
			
			is_updated = true;
			
		} while(false);
		
		if(is_updated) {

			this.update_transform();			
		}
	}

	protected void	update_transform()
	{
		this.transform.localPosition = this.posture.eye;
		this.transform.localRotation = Quaternion.LookRotation(this.posture.interest - this.posture.eye, this.posture.up);
		//this.transform.position = this.posture.eye;
		//LookAt(this.posture.interest, this.posture.up);
	}

	// ドリー　注視点に近づいたり、遠ざかったり.
	protected void	calc_dolly(float dolly)
	{
		Vector3		v;
		float		length;
		
		v = this.posture.eye - this.posture.interest;
		
		length = v.magnitude;
		
		length += dolly;

		if(this.config.is_enable_dolly_limit) {
			
			length = Mathf.Clamp(length, this.dolly_limit_near, this.dolly_limit_far);
			
		} else {
			
			length = Mathf.Max(this.dolly_limit_near, length);
		}
	
		v.Normalize();
		if(v.magnitude == 0.0f) {

			v = Vector3.forward;
		}
		v *= length;
		
		this.posture.eye = this.posture.interest + v;
	}

	// 注視点周りの回転.
	protected void	rotate_around_interest(float dx, float dy)
	{
		Vector3		eye_vector = this.posture.eye - this.posture.interest;

		eye_vector = Quaternion.AngleAxis(dy, this.transform.right)*eye_vector;
		eye_vector = Quaternion.AngleAxis(dx, Vector3.up)*eye_vector;

		this.posture.eye = this.posture.interest + eye_vector;

		this.posture.up = Quaternion.AngleAxis(dy,  this.transform.right)*this.posture.up;
		this.posture.up = Quaternion.AngleAxis(dx, Vector3.up)*this.posture.up;
		this.posture.up.Normalize();
	}

	// ================================================================ //
}

