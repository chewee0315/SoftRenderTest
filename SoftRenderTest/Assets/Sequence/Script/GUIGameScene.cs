using UnityEngine;
using System.Collections;

// ゲーム中画面の UI.
public class GUIGameScene : SeqGUIBase {

	public enum MODE {

		NONE = -1,
		IN_PLAY = 0,	// プレイ中.

		PAUSE,

		NUM,
	}
	protected MODE	mode = MODE.NONE;

	protected Botan.uiItemBase	end_confirm_text;
	protected Botan.uiButton	clear_button;			// ゲームをクリアーしたことにするボタン.
	protected Botan.uiButton	end_button;
	protected Botan.uiButton	end_yes_button;
	protected Botan.uiButton	end_no_button;

	// ================================================================ //

	void	Start()
	{
		Sprite2DRoot.get().insertLayer("setumei", "fader");

		this.clear_button   = this.findButton("ClearButton",	"clear");
		this.end_button     = this.findButton("EndButton",		"end");
		this.end_yes_button = this.findButton("EndYesButton",   "end-yes");
		this.end_no_button  = this.findButton("EndNoButton",    "end-no");

		this.end_confirm_text = BotanRoot.get().findItem("uiEndConfirmText");

		this.changeMode(MODE.IN_PLAY);
	}
	
	void	Update()
	{
	}

	// ================================================================ //

	public void		changeMode(MODE mode)
	{
		this.mode = mode;

		this.clear_button.setVisible(false);
		this.end_button.setVisible(false);
		this.end_yes_button.setVisible(false);
		this.end_no_button.setVisible(false);
		this.end_confirm_text.setVisible(false);

		switch(this.mode) {

			case MODE.IN_PLAY:
			{
				this.clear_button.setVisible(true);
				this.end_button.setVisible(true);
			}
			break;

			case MODE.PAUSE:
			{
				this.end_yes_button.setVisible(true);
				this.end_no_button.setVisible(true);

				this.end_confirm_text.setVisible(true);
			}
			break;
		}
	}

	// ボタンを押せる/押せないする.
	public new void		setButtonActive(bool is_active)
	{
		this.end_button.setActive(is_active);
	}
}
