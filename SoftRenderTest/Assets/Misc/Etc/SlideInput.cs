using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MathExtension;

public class SlideInput : MonoBehaviour {

	//protected const float		SPEED_THRESHOLD = 1000.0f;
	protected const float		SPEED_THRESHOLD = 500.0f;
	protected const float		SPEED_THRESHOLD_TO_STOP = 10.0f;

	protected const float		RESPIRE_TIME = 0.1f;

	protected const float		ANGLE_THRESHOLD = 10.0f;


	protected bool		is_trigger = false;

	protected Vector2	previous_position;

	protected float		recent_max = 0.0f;

	protected Vector3	slide_direction      = Vector3.zero;
	protected Vector3	slide_direction_last = Vector3.zero;
	protected float		slide_power          = 0.0f;
	protected float		slide_power_last     = 0.0f;

	protected float		slide_time;
	protected float		slide_distance;
	protected float		slide_distance_last = 0.0f;

	protected struct PositionHistory {

		public Vector2	position;
		public float	delta_time;
	};
	protected List<PositionHistory>		histories = new List<PositionHistory>();

	// -------------------------------------------------------- //
		
	public enum STEP {
		
		NONE = -1,

		IDLE = 0,
		SLIDING,

		STOP_RESPITE,		// スライドが終わった（失敗した）と判定するまでの猶予期間.

		WAIT_RELEASE,

		NUM,
	};
	public Step<STEP>			step = new Step<STEP>(STEP.NONE);


	// ================================================================ //

	public bool	isTriggerSliding()
	{
		return(this.is_trigger);
	}
	public bool	isNowSliding()
	{
		bool	is_sliding = false;
		
		switch(this.step.get_current()) {
			
			case STEP.SLIDING:
			case STEP.STOP_RESPITE:
			case STEP.WAIT_RELEASE:
			{
				is_sliding = true;
			}
			break;
		}
		
		return(is_sliding);
	}

	public Vector3	getSlideDirection()
	{
		return(this.slide_direction_last);
	}
	public Vector3	getSlideDirectionCurrent()
	{
		return(this.slide_direction);
	}
	public float	getSlidePower()
	{
		return(this.slide_power_last);
	}
	public float	getSlideDistance()
	{
		return(this.slide_distance_last);
	}

	// ================================================================ //

	void	Awake()
	{
	}
	
	void	Start()
	{
		this.previous_position = Input.mousePosition.xy();
		this.step.set_next(STEP.IDLE);
	}
	
	void	Update()
	{
		float	delta_time = Time.deltaTime;

		this.is_trigger = false;
	
		Vector2		current_position = Input.mousePosition.xy();

		Vector2		move_vector = current_position - this.previous_position;
		
		float	move_speed = move_vector.magnitude/delta_time;
		
		this.recent_max = Mathf.Max(this.recent_max, move_speed);

		// 

		PositionHistory		history = new PositionHistory();

		history.position   = current_position;
		history.delta_time = delta_time;

		this.histories.Add(history);

		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.

		switch(this.step.do_transition()) {

			case STEP.IDLE:
			{
				do {
	
					if(!Input.GetMouseButton(0)) {
						
						break;
					}
					if(move_speed < SPEED_THRESHOLD) {
							
						break;
					}
	
					this.step.set_next(STEP.SLIDING);
					
				} while(false);
			}
			break;

			case STEP.SLIDING:
			{
				do {
					
					if(!Input.GetMouseButton(0)) {
						
						this.on_slide_finish(delta_time, move_speed, move_vector);	
						this.step.set_next(STEP.IDLE);
						break;
					}
					if(move_speed < SPEED_THRESHOLD) {
						
						this.step.set_next(STEP.STOP_RESPITE);
						break;
					}
					
				} while(false);
			}
			break;

			case STEP.STOP_RESPITE:
			{
				do {
					
					// ボタンが離されたら成功.
					if(!Input.GetMouseButton(0)) {
						
						this.on_slide_finish(delta_time, move_speed, move_vector);	
						this.step.set_next(STEP.IDLE);
						break;
					}
					
					// スライド速度が超遅くなったらボタン離し待ちへ.
					if(move_speed < SPEED_THRESHOLD_TO_STOP) {

						this.step.set_next(STEP.WAIT_RELEASE);
						break;
					}
					
					// スライド速度が早くなったら、再度スライド中へ復帰.
					if(move_speed > SPEED_THRESHOLD) {
						
						this.step.set_next(STEP.SLIDING);
						break;
					}
					
					// ある程度時間がたったらボタン離し待ちへ.
					if(this.step.get_time() > RESPIRE_TIME) {

						this.step.set_next(STEP.WAIT_RELEASE);
						break;
					}
					
				} while(false);
			}
			break;
			
			case STEP.WAIT_RELEASE:
			{
				do {

					if(this.step.get_time() > 0.1f) {
	
						this.step.set_next(STEP.IDLE);
						break;
					}
					if(!Input.GetMouseButton(0)) {

						this.on_slide_finish(delta_time, move_speed, move_vector);	
						this.step.set_next(STEP.IDLE);
						break;
					}

				} while(false);
			}
			break;
		}

		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.

		while(this.step.get_next() != STEP.NONE) {

			switch(this.step.do_initialize()) {
	
				case STEP.SLIDING:
				{
					this.slide_time = 0.0f;
					this.slide_distance = 0.0f;

					this.histories.Clear();
				}
				break;
			}
		}

		// ---------------------------------------------------------------- //
		// 各状態での実行処理.

		switch(this.step.do_execution(delta_time)) {

			case STEP.SLIDING:
			{
				this.slide_time += delta_time;
				this.slide_distance += move_vector.magnitude;

				this.slide_direction = move_vector;
				this.slide_direction.Normalize();

				this.slide_power = move_speed;

			}
			break;
		}

		// ---------------------------------------------------------------- //

		//dbPrint.setLocate(10, 10);
		//dbPrint.print(this.recent_max);

		this.previous_position = current_position;

	}

	protected void	on_slide_finish(float delta_time, float move_speed, Vector3 move_vector)
	{
		this.is_trigger           = true;

	#if true
		if(this.histories.Count > 1) {

			int		rew_back = Mathf.Min(5, this.histories.Count - 1);

			this.slide_direction = this.histories[this.histories.Count - 1].position - this.histories[this.histories.Count - 1 - rew_back].position;
			this.slide_direction.Normalize();
		}
	#endif

		this.slide_direction_last = this.slide_direction;
		this.slide_power_last     = this.slide_power;
		this.slide_distance_last  = this.slide_distance;

		//Debug.Log(this.slide_power + " " + this.slide_distance);
	}
}
