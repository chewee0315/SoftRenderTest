using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	// Input をラップして、デバッグ操作とゲーム操作がかぶらないようにするためのクラス.
	public class InputManager : MonoBehaviour {

		public enum Layer {

			None = -1,

			Default = 0,
			Debug,

			Num,
		}

		/// <summary>
		/// Input.GetMouseButton() のかわり.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool		GetMouseButton(int button)
		{
#if DEBUG
			return(InputManager.get().get_mouse_button(button, Layer.Default));
#else
			return(Input.GetMouseButton(button));
#endif
		}

		/// <summary>
		/// Input.GetMouseButtonDown() のかわり.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool		GetMouseButtonDown(int button)
		{
#if DEBUG
			return(InputManager.get().get_mouse_button_down(button, Layer.Default));
#else
			return(Input.GetMouseButtonDown(button));
#endif
		}

		/// <summary>
		/// Input.GetMouseButtonUp() のかわり.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public static bool		GetMouseButtonUp(int button)
		{
#if DEBUG
			return(InputManager.get().get_mouse_button_up(button, Layer.Default));
#else
			return(Input.GetMouseButtonUp(button));
#endif
		}

		/// <summary>
		/// Input.mousePosition のかわり.
		/// </summary>
		public static Vector3	mousePosition
		{
#if DEBUG
			get { return(InputManager.get().get_mouse_position(Layer.Default)); }
#else
			get {  return(Input.mousePosition); }
#endif
		}

		/// <summary>
		/// Input.touchCount のかわり.
		/// </summary>
		public static int touchCount
		{
#if DEBUG
			get { return(InputManager.get().get_touch_count(Layer.Default)); }
#else
			get {  return(Input.touchCount); }
#endif
		}

		public static Touch[] touches
		{
#if DEBUG
			get { return(InputManager.get().get_touches(Layer.Default)); }
#else
			get {  return(Input.touches); }
#endif
		}

		// ================================================================ //

		public int		AddOcuppyRegion(Layer layer, Rect rect)
		{
			OcuppyRegion	region = new OcuppyRegion();

			region.rect = rect;

			this.occupy_regions[(int)layer].regions.Add(region);

			return(this.occupy_regions[(int)layer].regions.Count - 1);
		}

		public void		ClearOcuppyRegionAll(Layer layer)
		{
			this.occupy_regions[(int)layer].regions.Clear();
		}

		public void		SetOcuppyRegionEnable(Layer layer, int region_index, bool is_enable)
		{
			int		layer_as_int = (int)layer;

			if(0 <= layer_as_int && layer_as_int < this.occupy_regions.Length) {

				if(0 <= region_index && region_index < this.occupy_regions[layer_as_int].regions.Count) {
					this.occupy_regions[layer_as_int].regions[region_index].is_enable = is_enable;
				}
			}
		}

		// ================================================================ //

		protected Layer		current_layer = Layer.Default;

		protected struct MouseButton {

			public bool		current;
			public bool		down;
			public bool		up;

			public void		clear()
			{
				this.current = false;
				this.down    = false;
				this.up      = false;
			}
		}
		protected struct Status {

			public MouseButton[]	mouse_buttons;		// MouseButton[ボタン].

			public int				touch_count;
			public Touch[]			touches;
			public Vector3			mouse_position;

			public static Touch[]	touches_empty = null;

			public void		Clear()
			{
				for(int j = 0;j < this.mouse_buttons.Length;j++) {
					this.mouse_buttons[j].clear();
				}

				this.touch_count = 0;
				this.touches = Status.touches_empty;
				this.mouse_position = Vector3.zero;
			}
		}
		protected Status[]	status;						// Status[レイヤー].

		public int[]		repeat_count;				// int[ボタン]　自動連射用カウンター.

		// レイヤーの占有領域（いっこ）.
		protected class OcuppyRegion {

			public Rect		rect;
			public bool		is_on_cursor = false;
			public bool		is_enable = true;
		}

		// レイヤーの占有領域　１レイヤー分ぜんぶ.
		protected class OcuppyRegionLayer {

			public List<OcuppyRegion>		regions = new List<OcuppyRegion>();

			public bool		is_on_cursor = false;

			public void		check_on_cursor(Vector2 cursor_position)
			{
				this.is_on_cursor = false;

				for(int i = 0;i < this.regions.Count;i++) {

					OcuppyRegion	region = this.regions[i];

					if(!region.is_enable) {
						continue;
					}

					region.is_on_cursor = region.rect.Contains(cursor_position);

					if(region.is_on_cursor) {
						this.is_on_cursor = true;
					}
				}
			}
		}

		protected OcuppyRegionLayer[]	occupy_regions;		// occupy_regions[レイヤー].

		// ================================================================ //

		protected bool		get_mouse_button(int button, Layer layer)
		{
			return(this.status[(int)layer].mouse_buttons[button].current);
		}

		protected bool		get_mouse_button_down(int button, Layer layer)
		{
			return(this.status[(int)layer].mouse_buttons[button].down);
		}

		protected bool		get_mouse_button_up(int button, Layer layer)
		{
			return(this.status[(int)layer].mouse_buttons[button].up);
		}

		protected int		get_touch_count(Layer layer)
		{
			return(this.status[(int)layer].touch_count);
		}

		public Touch[] get_touches(Layer layer)
		{
			return(this.status[(int)layer].touches);
		}

		protected Vector3	get_mouse_position(Layer layer)
		{
			return(this.status[(int)layer].mouse_position);
		}

		// ================================================================ //

		void	Awake()
		{
			this.status = new Status[(int)Layer.Num];

			if(Status.touches_empty == null) {
				Status.touches_empty = new Touch[0];
			}

			for(int i = 0;i < this.status.Length;i++) {

				this.status[i].mouse_buttons = new MouseButton[3];
				for(int j = 0;j < this.status[i].mouse_buttons.Length;j++) {
					this.status[i].mouse_buttons[j].clear();
				}

				this.status[i].touch_count = 0;
				this.status[i].touches = Status.touches_empty;
				this.status[i].mouse_position = Vector3.zero;
			}

			//
			this.repeat_count = new int[3];

			for(int i = 0;i < this.repeat_count.Length;i++) {
				this.repeat_count[i] = -1;
			}

			//
			this.occupy_regions = new OcuppyRegionLayer[(int)Layer.Num];
			for(int i = 0;i < this.occupy_regions.Length;i++) {
				this.occupy_regions[i] = new OcuppyRegionLayer();
			}
		}

		void	Start()
		{
		}


        void	Update()
		{
			this.update_regions();

			// レイヤー切り替え.
			if(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.touches.Length > 0) {
				// ボタンが押されているときは、レイヤー切り替えはしない.
			} else {

				this.decide_current_layer();

				if(this.current_layer != Layer.Default) {
					this.status[(int)this.current_layer].Clear();
				}
			}

			// シフトキー押しながらマウスボタンおしっぱで、自動連射.
			for(int i = 0;i < this.repeat_count.Length;i++) {

				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {

					if(Input.GetMouseButton(i)) {

						if(this.repeat_count[i] < 0) {
							this.repeat_count[i] = 0;
						}

					} else {
						this.repeat_count[i] = -1;
					}

				} else {
					this.repeat_count[i] = -1;
				}
			}

			for(int i = 0;i < this.status[(int)this.current_layer].mouse_buttons.Length;i++) {

				if(this.repeat_count[i] >= 0) {
					if(this.repeat_count[i]%2 == 0) {
						this.status[(int)this.current_layer].mouse_buttons[i].current = true;
						this.status[(int)this.current_layer].mouse_buttons[i].down    = true;
						this.status[(int)this.current_layer].mouse_buttons[i].up      = false;
					} else {
						this.status[(int)this.current_layer].mouse_buttons[i].current = false;
						this.status[(int)this.current_layer].mouse_buttons[i].down    = true;
						this.status[(int)this.current_layer].mouse_buttons[i].up      = false;
					}
				} else {
					this.status[(int)this.current_layer].mouse_buttons[i].current = Input.GetMouseButton(i);
					this.status[(int)this.current_layer].mouse_buttons[i].down    = Input.GetMouseButtonDown(i);
					this.status[(int)this.current_layer].mouse_buttons[i].up      = Input.GetMouseButtonUp(i);
				}
			}

			this.status[(int)this.current_layer].touch_count = Input.touchCount;

			this.status[(int)this.current_layer].touches = Input.touches;

			this.status[(int)this.current_layer].mouse_position = Input.mousePosition;

			for(int i = 0;i < this.repeat_count.Length;i++) {

				if(this.repeat_count[i] >= 0) {
					this.repeat_count[i]++;
				}
			}

			//dbPrint.setLocate(20, 20);
			//dbPrint.print(this.current_layer);

			//this.debug_draw_regions();
			//Debug.Log(Input.touches.Length);
		}

		// ================================================================ //

		protected void		update_regions()
		{
			for(int i = 0;i < this.occupy_regions.Length;i++) {

				this.occupy_regions[i].check_on_cursor(Input.mousePosition);
				if(this.occupy_regions[i].is_on_cursor) {
					continue;
				}

				for(int j = 0;j < Input.touches.Length;j++) {

					this.occupy_regions[i].check_on_cursor(Input.touches[j].position);
					if(this.occupy_regions[i].is_on_cursor) {
						break;
					}
				}
			}
		}

		protected void		decide_current_layer()
		{
			for(int i = this.occupy_regions.Length - 1;i >= 0;i--) {

				if(i == (int)Layer.Default) {
					this.current_layer = (Layer)i;
					break;
				}

				if(this.occupy_regions[i].is_on_cursor) {
					this.current_layer = (Layer)i;
					break;
				}
			}
		}

	#if DEBUG
		protected void		debug_draw_regions()
		{
			for(int i = 0;i < this.occupy_regions.Length;i++) {

				OcuppyRegionLayer	region_layer = this.occupy_regions[i];

				for(int j = 0;j < region_layer.regions.Count;j++) {

					OcuppyRegion	region = region_layer.regions[j];

					if(!region.is_enable) {
						continue;
					}

					Color	color = Color.blue;

					if(region.is_on_cursor) {
						color = Color.red;
					}
					color.a = 0.2f;

					dbDraw.get().drawSquare2D(region.rect.center - new Vector2(Screen.width/2.0f, Screen.height/2.0f), region.rect.size.x, region.rect.size.y, color);
				}
			}
		}
	#endif

		// ================================================================ //
		//																	//
		// ================================================================ //

		// インスタンス.
		protected	static InputManager	instance = null;

		public static InputManager	get()
		{
			if(InputManager.instance == null) {

				GameObject		go = GameObject.Find("InputManager");

				if(go != null) {

					InputManager.instance = go.GetComponent<InputManager>();
					//Stage.instance.create();

				} else {

					Debug.LogError("Can't find game object \"InputManager\".");
				}
			}

			return(InputManager.instance);
		}
		// ================================================================ //

	} // class InputManager.



