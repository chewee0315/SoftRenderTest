using UnityEngine;
using System.Collections;

public class Motu : MonoBehaviour {

	public enum STEP {
		
		NONE = -1,
		
		NORMAL = 0,			// ドラッグしてない中.
		GRABBED,			// ドラッグしてる中.
	};
	public Step<STEP>	step = new Step<STEP>(STEP.NONE);

	// -------------------------------------------------------- //

	// ドラッグ中？.
	protected bool			is_now_dragging = false;

	protected Camera		main_camera;

	protected Vector3		grab_offset = Vector3.zero;

	// ================================================================ //

	void	Awake()
	{
		this.main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void	Start()
	{
		this.step.set_next(STEP.NORMAL);
	}

	void	Update()
	{
		switch(this.step.get_current()) {

			case STEP.GRABBED:
			{
				dbPrint.print3d(this.transform.position, this.transform.position.ToString());
			}
			break;
		}
	}

	void	FixedUpdate()
	{
		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		
		switch(this.step.do_transition()) {
			
			case STEP.NORMAL:
			{
				if(this.is_now_dragging) {

					this.step.set_next(STEP.GRABBED);
				}
			}
			break;

			case STEP.GRABBED:
			{
				if(!this.is_now_dragging) {
					
					this.step.set_next(STEP.NORMAL);
				}
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.step.get_next() != STEP.NONE) {
			
			switch(this.step.do_initialize()) {
				
				case STEP.NORMAL:
				{
					if(this.GetComponent<Rigidbody>() != null) {

						this.GetComponent<Rigidbody>().isKinematic = false;
						this.GetComponent<Rigidbody>().velocity = Vector3.zero;
						this.GetComponent<Rigidbody>().constraints = 0;
					}
				}
				break;

				case STEP.GRABBED:
				{
					if(this.GetComponent<Rigidbody>() != null) {
	
						this.GetComponent<Rigidbody>().isKinematic = true;
					}
				}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step.do_execution(Time.deltaTime)) {
			
			case STEP.NORMAL:
			{
			}
			break;

			case STEP.GRABBED:
			{
				// カーソル座標を、3D空間のワールド座標に変換する
				Vector3		world_position = this.unproject_mouse_position(Input.mousePosition);
			
				if(this.GetComponent<Rigidbody>() != null) {
				
					this.GetComponent<Rigidbody>().MovePosition(world_position);

					if(!this.GetComponent<Rigidbody>().isKinematic) {

						this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
					}

				} else {

					this.transform.position = world_position;
				}
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
	}

	// マウスボタンが押されたとき.
	void 	OnMouseDown()
	{
		do {

			// ---------------------------------------------------------------- //
			// クリックされたのが本当に自分のコリジョンなのか、調べる.
			// （OnMouseDown は子供のコリジョンをクリックしたときも呼ばれちゃうから）.
			
			Ray			ray = this.main_camera.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit	hit;
			
			if(!this.GetComponent<Collider>().Raycast(ray, out hit, float.PositiveInfinity)) {
				
				break;
			}
		
			// ---------------------------------------------------------------- //

			this.is_now_dragging = true;
	
			this.grab_offset = Vector3.zero;
				
			Vector3		world_position = this.unproject_mouse_position(Input.mousePosition, false);
				
			this.grab_offset = this.transform.position - world_position;

		} while(false);
	}

	// マウスボタンが離されたとき.
	void 	OnMouseUp()
	{
		this.is_now_dragging = false;
	}

	// マウスの位置を、３D空間のワールド座標に変換する.
	//
	// ・マウスカーソルとカメラの位置を通る直線
	// ・カメラから視線方向に 距離 depth だけいったところ.
	//　↑を求めます.
	//
	public Vector3		unproject_mouse_position(Vector3 mouse_position, bool enable_bind = true)
	{
		Vector3		current_position = this.transform.position;
		Vector3		next_position    = current_position;
		Vector3		camera_position  = this.main_camera.transform.position;

		// カメラ位置とマウスカーソルの位置を通る直線.
		Ray			ray = this.main_camera.ScreenPointToRay(mouse_position);

		Vector3		normal = -this.main_camera.transform.forward;

		Plane	plane = new Plane(normal, this.transform.position);
		float	depth;

		if(plane.Raycast(ray, out depth)) {

			next_position = ray.origin + ray.direction*depth;
		}

		next_position += this.grab_offset;

		// ---------------------------------------------------------------- //
		// Z、X キーを押していたら XZ 平面、XY 平面に拘束する.

		if(enable_bind) {

			if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X)) {
	
				normal = -this.main_camera.transform.forward;
	
				if(Input.GetKey(KeyCode.Z)) {
		
					normal = Vector3.up;
		
				} else if(Input.GetKey(KeyCode.X)) {
		
					normal = Vector3.back;
				}
				
				plane = new Plane(normal, this.transform.position);
	
				Vector3		direction = next_position - camera_position;
				
				direction.Normalize();
				
				ray = new Ray(camera_position, direction);
				
				if(plane.Raycast(ray, out depth)) {
	
					next_position  = ray.origin + ray.direction*depth;
				}
			}
		}

		// ---------------------------------------------------------------- //

		return(next_position);
	}
}
