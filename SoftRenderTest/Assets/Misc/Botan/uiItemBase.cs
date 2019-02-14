using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;		// Required when using Event data.
using System.Collections;

namespace Botan {

public class uiItemBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler {

	// -------------------------------------------------------- //
	// Botan/ItemBase.cs からの移植.

	protected bool		is_visible  = true;
	protected bool		is_active   = true;
	public bool			is_can_drag = false;

	public string		group_name;
	
	public struct StateBit {

		public bool	trigger_on;

		public bool	previous;
		public bool	current;

		public void	update_trigger()
		{
			this.trigger_on = (!this.previous && this.current);
		}
	};

	public StateBit		focused;
	public StateBit		dragging;

	protected Vector2	grab_offset = Vector2.zero;

	// -------------------------------------------------------- //

	protected class Signal {

		public bool		trigger_focus_in  = false;
		public bool		trigger_focus_out = false;
		public bool		trigger_press     = false;
		public bool		trigger_release   = false;

		public void		clear()
		{
			this.trigger_focus_in  = false;
			this.trigger_focus_out = false;
			this.trigger_press     = false;
			this.trigger_release   = false;
		}
	};
	protected Signal	signal = new Signal();

	// ================================================================ //

	public void		OnPointerEnter(PointerEventData eventData) 
	{
		this.signal.trigger_focus_in = true;
	}

	public void		OnPointerExit(PointerEventData eventData) 
	{
		this.signal.trigger_focus_out = true;
	}

	public void		OnPointerDown(PointerEventData eventData) 
	{
		this.signal.trigger_press = true;
	}

	public void		OnPointerUp(PointerEventData eventData) 
	{
		this.signal.trigger_release = true;
	}

	// ================================================================ //

	// ヒエラルキーの親をセットする.
	public void		setParent(GameObject parent)
	{
		this.GetComponent<RectTransform>().SetParent(parent.GetComponent<RectTransform>());
	}

	// 位置をセットする.
	public virtual void		setPosition(Vector2 position)
	{
		this.GetComponent<RectTransform>().localPosition = position;
	}
	public virtual void		setPosition(float x, float y)
	{
		this.setPosition(new Vector2(x, y));
	}
	// 位置を取得する.
	public virtual Vector2		getPosition()
	{
		return(this.GetComponent<RectTransform>().localPosition);
	}

	// アングルをセットする.
	public virtual void		setAngle(float angle)
	{
		this.GetComponent<RectTransform>().localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	// スケールをセットする.
	public void		setScale(Vector2 scale)
	{
		this.GetComponent<RectTransform>().localScale = scale;
	}
	public void		setScale(float scale)
	{
		this.setScale(Vector2.one*scale);
	}

	// 表示/非表示をセットする.
	public virtual void		setVisible(bool is_visible)
	{
		this.is_visible = is_visible;

		this.gameObject.SetActive(this.is_visible);

		if(!this.is_visible) {

			this.reset();
		}
	}

	// 一番手前に表示する.
	public void		setFront()
	{
		this.GetComponent<RectTransform>().SetAsLastSibling();
	}

	// ================================================================ //

	public virtual void		reset()
	{
		this.focused.trigger_on = false;
		this.focused.previous   = false;
		this.focused.current    = false;
	}

	public void		execute(float delta_time, bool is_focusable)
	{
		if(this.is_visible) {

			this.update_focuse_status(is_focusable);

			if(this.is_can_drag) {

				//this.execute_dragging();
			}

			this.execute_entity(delta_time);
		}

		this.signal.clear();
	}

	public virtual void		execute_entity(float delta_time)
	{
	}

	// フォーカス状態の更新.
	protected void	update_focuse_status(bool is_focusable)
	{
#if true
		this.focused.previous = this.focused.current;

		// ---------------------------------------------------------------- //
		// フォーカス（ロールオーバー）.

		this.focused.trigger_on = false;

		if(this.is_active) {
	
			if(is_focusable) {

				if(this.signal.trigger_focus_in) {
		
					this.focused.trigger_on = true;
					this.focused.current = true;
				}
			}

			if(this.signal.trigger_focus_out) {
	
				this.focused.current = false;
			}

		} else {

			// フォーカスが禁止にされる前からフォーカスしていた場合は、
			// フォーカスアウトするまでフォーカス状態を維持する.
			if(this.focused.previous) {
	
				if(is_focusable) {
	
					if(this.signal.trigger_focus_in) {
			
						this.focused.trigger_on = true;
						this.focused.current = true;
					}
				}
	
				if(this.signal.trigger_focus_out) {
		
					this.focused.current = false;
				}
			}
		}

#else
		this.focused.previous = this.focused.current;
		
		// ---------------------------------------------------------------- //
		// フォーカス（ロールオーバー）.

		Vector2		mouse_pos = this.root.input.mouse_position;

		mouse_pos = Sprite2DRoot.get().convertMousePosition(mouse_pos);

		this.focused.current = false;

		if(this.is_active) {

			if(is_focusable) {

				if(this.isContainPoint(mouse_pos)) {
	
					this.focused.current = true;	
				}
			}

		} else {

			// フォーカスが禁止にされる前からフォーカスしていた場合は、
			// フォーカスアウトするまでフォーカス状態を維持する.

			if(this.focused.previous) {

				if(is_focusable) {
	
					if(this.isContainPoint(mouse_pos)) {
		
						this.focused.current = true;	
					}
				}
			}
		}

		this.focused.update_trigger();
#endif
	}
}

} // namespace Botan

