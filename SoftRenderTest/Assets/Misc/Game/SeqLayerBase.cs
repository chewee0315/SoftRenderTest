using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeqLayerBase {

	// ---------------------------------------------------------------- //

	public SeqSceneBase		seq_scene;

	// ================================================================ //
	// MonoBehaviour からの継承.

	// スタート.
	public virtual void	start()
	{
	}

	// 毎フレームの実行.
	public virtual void	execute()
	{
	}

	// 終了した？.
	public virtual bool	isFinished()
	{
		return(false);
	}

	// 子レイヤーが終わって、処理を再開するとき？.
	public virtual void	resume(SeqLayerBase child)
	{
	}

}