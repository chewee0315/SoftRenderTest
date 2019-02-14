using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeqSceneBase : MonoBehaviour {

	public bool		use_fade = true;

	// ---------------------------------------------------------------- //

	internal Botan.Fader		fader;

	protected List<SeqLayerBase>	layers = new List<SeqLayerBase>();

	// ================================================================ //
	// MonoBehaviour からの継承.

	void	Awake()
	{
		this.awake_entity();

		if(this.layers.Count == 0) {

			Debug.LogError("Can't find root layer.");
		}
	}

	void	Start()
	{
		this.fader = BotanRoot.get().createFader();
		this.fader.setCurrentAnon(new Color(0.0f, 0.0f, 0.0f, 1.0f));

		this.start_entity();
	}

	void	Update()
	{
		if(Input.GetKeyDown(KeyCode.End)) {

			Application.Quit();
		}

		if(this.layers.Count > 0) {

			// 子レイヤーの終了チェック.
			if(this.layers[this.layers.Count - 1].isFinished()) {

				this.popLayer();
			}

			this.layers[this.layers.Count - 1].execute();
		}
	}

	// ================================================================ //

	public virtual void	awake_entity()
	{
	}

	public virtual void	start_entity()
	{
	}

	// ================================================================ //

	// レイヤーをプッシュする　子レイヤーに制御をうつす.
	public void		pushLayer(SeqLayerBase layer)
	{
		layer.seq_scene = this;
		this.layers.Add(layer);

		if(this.layers.Count > 0) {

			this.layers[this.layers.Count - 1].start();
		}
	}

	// レイヤーをポップする　子レイヤーを終了して、親レイヤーを再開する.
	public void		popLayer()
	{
		if(this.layers.Count > 1) {

			SeqLayerBase	child = this.layers[this.layers.Count - 1];

			this.layers.RemoveAt(this.layers.Count - 1);

			this.layers[this.layers.Count - 1].resume(child);
		}
	}

	// ================================================================ //

	internal bool	next_scene(string scene)
	{
		bool	ret = false;

		if(Application.CanStreamedLevelBeLoaded(scene)) {

			UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
			ret = true;

		} else {

			Debug.LogError("Can't load Scene \"" + scene + "\".");
		}

		return(ret);
	}

}