using UnityEngine;
using System.Collections;

// リザルト画面のシーケンス.
public class SeqResultLayer : SeqLayerBase {

	// -------------------------------------------------------- //
		
	public enum STEP {
		
		NONE = -1,

		FADE_IN = 0,		// フェードイン中.
		WAITING,			// ボタンクリック待ち.
		FADE_OUT,			// フェードアウト中.
	};
	public Step<STEP>			step = new Step<STEP>(STEP.NONE);
	
	// ================================================================ //

	public override void	start()
	{
		base.start();

		this.step.set_next(STEP.FADE_IN);
	}

	public override void	execute()
	{
		base.execute();

		SeqResultScene	seq_scene = this.seq_scene as SeqResultScene;

		// ---------------------------------------------------------------- //
		// 次の状態に移るかどうかを、チェックする.
		
		switch(this.step.do_transition()) {

			// フェードイン中.
			case STEP.FADE_IN:
			{
				if(!this.seq_scene.fader.isFading()) {

					this.step.set_next(STEP.WAITING);
				}
			}
			break;

			// ボタンクリック待ち.
			case STEP.WAITING:
			{
				if(seq_scene.gui_result.selected == "retry") {

					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 1.0f));
					this.step.set_next(STEP.FADE_OUT);

				} else if(seq_scene.gui_result.selected == "end") {

					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 1.0f));
					this.step.set_next(STEP.FADE_OUT);
				}
			}
			break;

			// フェードアウト中.
			case STEP.FADE_OUT:
			{
				if(!seq_scene.fader.isFading()) {

					if(seq_scene.gui_result.selected == "retry") {

						seq_scene.next_scene("GameScene");

					} else {

						seq_scene.next_scene("TitleScene");
					}
				}
			}
			break;
		}
		
		// ---------------------------------------------------------------- //
		// 状態が遷移したときの初期化.
		
		while(this.step.get_next() != STEP.NONE) {
			
			switch(this.step.do_initialize()) {

				// フェードイン中.
				case STEP.FADE_IN:
				{
					seq_scene.gui_result.setButtonVisible(false);
					seq_scene.fader.setGoal(new Color(0.0f, 0.0f, 0.0f, 0.0f));
				}
				break;

				// ボタンクリック待ち.
				case STEP.WAITING:
				{
					seq_scene.gui_result.setButtonVisible(true);
					seq_scene.gui_result.selected = "";
				}
				break;
			}
		}
		
		// ---------------------------------------------------------------- //
		// 各状態での実行処理.
		
		switch(this.step.do_execution(Time.deltaTime)) {

				// ボタンクリック待ち.
				case STEP.WAITING:
				{
				}
				break;
		}
		
		// ---------------------------------------------------------------- //

	}

	// ================================================================ //
}
