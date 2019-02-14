using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeqGameScene : SeqSceneBase {

	public GUIGameScene	gui_game;

	// ================================================================ //

	public override void	awake_entity()
	{
		SeqGameLayer	layer = new SeqGameLayer();

		layer.use_fade = this.use_fade;

		this.pushLayer(layer);
	}

	public override void	start_entity()
	{
		this.gui_game = this.GetComponent<GUIGameScene>();
	}

	// ================================================================ //
	//																	//
	// ================================================================ //

#if false
	protected void		db_create_debug_window()
	{
		if(dbwin.root().getWindow("その他.") == null) {
			
			var		window = dbwin.root().createWindow("その他.");

			window.createButton("ゴール")
				.setOnPress(() => 
					{
						JimenRoot.get().db_create_goal();

						//this.onigiri.startGoalAct();
						//this.step.set_next(STEP.GOAL_ACT);
						this.step.set_next(STEP.GOAL);
					}
				);

			window.createCheckBox("BGM.", SoundManager.get().dbIsEnableBGM())
				.setOnChanged((is_checked) => 
					{
						SoundManager.get().dbSetEnableBGM(is_checked);
					}
				);
			window.createCheckBox("SE.", SoundManager.get().is_play_se)
				.setOnChanged((is_checked) => 
					{
						SoundManager.get().is_play_se = is_checked;
					}
				);

		}

	}
#endif
}