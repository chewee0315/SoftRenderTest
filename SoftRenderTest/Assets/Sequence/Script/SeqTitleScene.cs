using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeqTitleScene : SeqSceneBase {

	public GUITitleScene	gui_title;

	// ================================================================ //

	public override void	awake_entity()
	{
		this.pushLayer(new SeqTitleLayer());
	}

	public override void	start_entity()
	{
		this.gui_title = this.GetComponent<GUITitleScene>();
	}

	// ================================================================ //
}