using UnityEngine;
using System.Collections;

public class GUIResultScene : SeqGUIBase {

	protected Botan.uiButton	retry_button;
	protected Botan.uiButton	end_button;

	// ================================================================ //

	void	Start()
	{
		this.retry_button = this.findButton("RetryButton", "retry");
		this.end_button   = this.findButton("EndButton",   "end");
	}
	
	void	Update()
	{
	}

	// ================================================================ //

}
