using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Botan {

public class uiButton : uiItemBase {

	public string		button_name = "";

	// -------------------------------------------------------- //
	// Botan/Button.cs からの移植.

	public StateBit		pressed;

	protected float		scale_timer = 0.0f;


	public delegate	void	Func(string name);
	public Func		on_trigger_pressed = (name)=>{};

	protected struct PressingAction {

		public bool		is_active;
		public bool		press_down;
	};
	protected PressingAction	press_act;

	// -------------------------------------------------------- //

	protected UnityEngine.UI.Button	ui_button;

	// ================================================================ //
	// MonoBehaviour からの継承.

	void	Awake()
	{
		if(this.button_name == "") {

			this.button_name = this.name;
		}
		this.ui_button = this.GetComponent<UnityEngine.UI.Button>();

		this.reset();
	}
	
	void	Start()
	{
	}
	
	void	Update()
	{
		float	delta_time = Mathf.Max(1.0f/60.0f, Time.deltaTime);

		this.execute(delta_time, true);
	}

	// -------------------------------------------------------- //
	// Botan/Button.cs からの移植.

	public override void		reset()
	{
		base.reset();

		this.pressed = this.focused;

		this.press_act.is_active = false;
		this.press_act.press_down = false;

		this.scale_timer = 0.0f;

		this.GetComponent<RectTransform>().localScale = Vector2.one;
	}

	public override void	execute_entity(float delta_time)
	{
		// ---------------------------------------------------------------- //
		// クリック.

		this.pressed.previous = this.pressed.current;
		//this.pressed.current  = false;

	#if true
		this.pressed.trigger_on = false;

		if(this.is_active) {

			if(this.signal.trigger_press) {

				this.pressed.current = true;
				this.pressed.trigger_on = true;
			}
		}
		if(this.signal.trigger_release) {

			this.pressed.current = false;
		}
	#else	
		if(this.root.input.button.trigger_on) {

			if(this.is_active) {

				if(this.focused.current) {
	
					this.pressed.current = true;
				}
			}
		}
	#endif

		// 押した演出中は、押されている状態にする.
		if(this.press_act.is_active) {

			this.pressed.current = true;
		}

		if(this.pressed.trigger_on) {

			this.press_act.is_active  = true;
			this.press_act.press_down = true;
		}

		// ---------------------------------------------------------------- //
		// 押された演出.

		if(this.press_act.is_active) {

			if(this.press_act.press_down) {

				// 最初の位置から押し込まれるまで.
				this.scale_timer -= delta_time;

				if(this.scale_timer <= 0.0f) {

					this.scale_timer = 0.0f;

					this.press_act.press_down = false;
				}

			} else {

				// 押し込まれたところから、元の位置にもどるまで.
				this.scale_timer += delta_time;

				if(this.scale_timer >= 0.1f) {

					this.scale_timer = 0.1f;
					this.press_act.is_active = false;

					this.on_trigger_pressed(this.button_name);
				}
			}

			this.setScale(Vector2.one*Mathf.Lerp(1.0f, 1.2f, this.scale_timer/0.1f));

		} else {

			if(this.focused.current) {
				
				this.scale_timer += delta_time;
				
			} else {
				
				this.scale_timer -= delta_time;
			}

			this.scale_timer = Mathf.Clamp(this.scale_timer, 0.0f, 0.1f);

			this.setScale(Vector2.one*Mathf.Lerp(1.0f, 1.2f, this.scale_timer/0.1f));
		}

		// ---------------------------------------------------------------- //
#if false
		// フォーカス中のボタンが一番手前にくるように.
		if(this.focused.current) {

			this.sprite.setDepthOffset(BotanRoot.FORCUSED_BUTTON_DEPTH);

		} else {

			this.sprite.setDepthOffset(0.0f);
		}
#endif
	}

	// ================================================================ //

	public void		setActive(bool is_active)
	{
		this.is_active = is_active;

		var		ui_image = this.GetComponent<UnityEngine.UI.Image>();

		if(ui_image != null) {

			Color	color = Color.white;

			if(this.is_active) {

				color = Color.white;

			} else {

				color = new Color(0.7f, 0.7f, 0.7f);
			}

			ui_image.color = color;
		}
	}

} // class uiButton

} // namespace Botan

