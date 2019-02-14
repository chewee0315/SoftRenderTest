using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeqResultScene : SeqSceneBase {

	public GUIResultScene	gui_result;

	// ================================================================ //

	public override void	awake_entity()
	{
		this.pushLayer(new SeqResultLayer());
	}

	public override void	start_entity()
	{
		this.gui_result = this.GetComponent<GUIResultScene>();
	}

	// ================================================================ //
}