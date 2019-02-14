using UnityEngine;
using System.Collections;

public class SeqGamePauseLayer : SeqLayerBase {

	public string	selected = "";

	// -------------------------------------------------------- //
		
	public enum STEP {
		
		NONE = -1,

		YES_NO_ASK,			// 「はい」「いいえ」クリック待ち.
		FINISH,

		NUM,
	};
	public Step<STEP>			step = new Step<STEP>(STEP.NONE);

	// ================================================================ //

	// スタート.
	public override void	start()
	{
		base.start();

		this.step.set_next(STEP.YES_NO_ASK);
	}

	// 毎フレームの実行.
	public override void	execute()
	{
		base.execute();

		SeqGameScene	seq_scene = this.seq_scene as SeqGameScene;

		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		
		switch(this.step.do_transition()) {

			// プレイ中.
			case STEP.YES_NO_ASK:
			{
				if(seq_scene.gui_game.selected != "") {

					this.selected = seq_scene.gui_game.selected;
					seq_scene.gui_game.selected = "";
					this.step.set_next(STEP.FINISH);
				}
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.step.get_next() != STEP.NONE) {
			
			switch(this.step.do_initialize()) {

				// プレイ中.
				case STEP.YES_NO_ASK:
				{
					// ゲームをポーズする.
					Time.timeScale = 0.0f;

					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 0.5f));

					seq_scene.gui_game.changeMode(GUIGameScene.MODE.PAUSE);
					seq_scene.gui_game.selected = "";
				}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step.do_execution(Time.deltaTime)) {

			// プレイ中.		
			case STEP.YES_NO_ASK:
			{
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
	}

	// 終了した？.
	public override bool	isFinished()
	{
		return(this.step.get_current() == STEP.FINISH);
	}

	// ================================================================ //

}

