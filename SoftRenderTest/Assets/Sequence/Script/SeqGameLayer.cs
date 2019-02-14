using UnityEngine;
using System.Collections;

public class SeqGameLayer : SeqLayerBase {

	// -------------------------------------------------------- //
		
	public enum STEP {
		
		NONE = -1,

		FADE_IN = 0,		// フェードイン.	
		GAME,				// プレイ中.
		FADE_OUT,

		PAUSE,				// ポーズ.
	};
	public Step<STEP>			step = new Step<STEP>(STEP.NONE);

	protected bool	is_return_title = false;
	public bool		use_fade = true;

	// ================================================================ //

	// スタート.
	public override void	start()
	{
		base.start();

		this.step.set_next(STEP.FADE_IN);
	}

	// 毎フレームの実行.
	public override void	execute()
	{
		base.execute();

		SeqGameScene	seq_scene = this.seq_scene as SeqGameScene;

		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		
		switch(this.step.do_transition()) {

			// フェードイン.
			case STEP.FADE_IN:
			{
				this.step.set_next(STEP.GAME);
			}
			break;

			// プレイ中.
			case STEP.GAME:
			{
				if(seq_scene.gui_game.selected == "clear") {

					this.step.set_next(STEP.FADE_OUT);

				} else if(seq_scene.gui_game.selected == "end") {

					this.step.set_next(STEP.PAUSE);
				}
			}
			break;

			// フェードアウト.
			case STEP.FADE_OUT:
			{
				if(!seq_scene.fader.isFading()) {

					if(this.is_return_title) {

						// ポーズを解除する.
						Time.timeScale = 1.0f;
						seq_scene.next_scene("TitleScene");

					} else {

						seq_scene.next_scene("ResultScene");
					}
				}
			}
			break;

			// ポーズ.
			case STEP.PAUSE:
			{
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.step.get_next() != STEP.NONE) {
			
			switch(this.step.do_initialize()) {

				// フェードイン.
				case STEP.FADE_IN:
				{
					this.seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 0.0f));

					if(this.use_fade) {

					} else {

						this.seq_scene.fader.setCurrentAnon(new Color(0.0f, 0.0f, 0.0f, 0.0f));
						this.step.set_next(STEP.GAME);
					}
				}
				break;

				// プレイ中.
				case STEP.GAME:
				{
					seq_scene.gui_game.changeMode(GUIGameScene.MODE.IN_PLAY);
				}
				break;

				// フェードアウト.
				case STEP.FADE_OUT:
				{
					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 1.0f));
				}
				break;

				// 説明表示.
				case STEP.PAUSE:
				{
					seq_scene.pushLayer(new SeqGamePauseLayer());
				}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step.do_execution(Time.deltaTime)) {

			// プレイ中.		
			case STEP.GAME:
			{
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
	}

	// 子レイヤーが終わって、処理を再開するとき？.
	public override void	resume(SeqLayerBase child)
	{
		SeqGameScene		seq_scene = this.seq_scene as SeqGameScene;
		SeqGamePauseLayer	pause_layer = child as SeqGamePauseLayer;

		// ポーズを解除する.
		Time.timeScale = 1.0f;
	
		if(pause_layer.selected == "end-yes") {

			this.is_return_title = true;
			this.step.set_next(STEP.FADE_OUT);

		} else {

			seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 0.0f));
			seq_scene.gui_game.changeMode(GUIGameScene.MODE.IN_PLAY);
			this.step.set_next_delay(STEP.GAME, 1.0f);
		}
	}

	// ================================================================ //

}

