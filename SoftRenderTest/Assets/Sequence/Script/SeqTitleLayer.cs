using UnityEngine;
using System.Collections;

public class SeqTitleLayer : SeqLayerBase {

	// -------------------------------------------------------- //
		
	public enum STEP {
		
		NONE = -1,
		
		WAIT_START = 0,		// ボタンクリック待ち.
		FADE_OUT,			// フェードアウト中.

		DISP_SETUMEI,		// 説明表示.
	};
	public Step<STEP>			step = new Step<STEP>(STEP.NONE);
	
	// ================================================================ //

	public override void	start()
	{
		base.start();

		this.step.set_next(STEP.WAIT_START);
	}
	
	public override void	execute()
	{
		base.execute();

		SeqTitleScene	seq_scene = this.seq_scene as SeqTitleScene;

		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		
		switch(this.step.do_transition()) {

			// ボタンクリック待ち.
			case STEP.WAIT_START:
			{
				if(seq_scene.gui_title.selected == "start") {

					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 1.0f));

					this.step.set_next(STEP.FADE_OUT);

				} else if(seq_scene.gui_title.selected == "setsumei") {

					this.step.set_next(STEP.DISP_SETUMEI);
				}

			}
			break;

			case STEP.FADE_OUT:
			{
				if(!seq_scene.fader.isFading()) {

					seq_scene.next_scene("GameScene");
				}
			}
			break;

			// 説明表示.
			case STEP.DISP_SETUMEI:
			{
				if(Input.GetMouseButtonUp(0)) {

					this.step.set_next(STEP.WAIT_START);
				}
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.step.get_next() != STEP.NONE) {
			
			switch(this.step.do_initialize()) {

				// ボタンクリック待ち.
				case STEP.WAIT_START:
				{
					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 0.0f));
					seq_scene.gui_title.setSetumeiVisible(false);
					seq_scene.gui_title.selected = "";
				}
				break;

				// 説明表示.
				case STEP.DISP_SETUMEI:
				{
					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 0.5f));
					seq_scene.gui_title.setSetumeiVisible(true);
					seq_scene.gui_title.selected = "";
				}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step.do_execution(Time.deltaTime)) {

			// ボタンクリック待ち.
			case STEP.WAIT_START:
			{
			}
			break;		
		}
		
		// ---------------------------------------------------------------- //
	}

}
