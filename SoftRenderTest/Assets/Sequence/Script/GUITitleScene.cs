using UnityEngine;
using System.Collections;

public class GUITitleScene : SeqGUIBase {

	public Botan.uiButton		start_button;
	public Botan.uiButton		setumei_button;
	protected Botan.uiItemBase	sprite_setumei;

	// ================================================================ //

	void	Awake()
	{

	}

	void	Start()
	{
		Sprite2DRoot.get().insertLayer("setumei", "fader");

		this.start_button   = this.findButton("StartButton", "start");
		this.setumei_button = this.findButton("SetsumeiButton", "setsumei");
		this.sprite_setumei = BotanRoot.get().findItem("SetsumeiImage");
	}
	
	void	Update()
	{
	}

	// ================================================================ //

	// 説明文を表示/非表示する.
	public void		setSetumeiVisible(bool is_visible)
	{
		if(is_visible) {

			this.sprite_setumei.setVisible(true);
			this.sprite_setumei.setFront();

		} else {

			this.sprite_setumei.setVisible(false);
		}
	}

}
