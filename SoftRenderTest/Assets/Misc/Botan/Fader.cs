﻿using UnityEngine;
using System.Collections;

namespace Botan {

// フェードアウト/イン.
public class Fader : MonoBehaviour {

	//protected Sprite2DControl	sprite;

	//public Texture	texture;

	protected struct Colors {

		public Color	current;
		public Color	goal;
	}
	protected Colors	colors;
	protected bool		is_disp;
	protected bool		is_fading = false;

	protected float		fade_rate = 0.01f;

	protected UnityEngine.UI.Image	ui_image;

	// ================================================================ //

	void	Awake()
	{
		this.ui_image = this.GetComponentInChildren<UnityEngine.UI.Image>();

		this.colors.current = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		this.colors.goal    = this.colors.current;
		this.ui_image.enabled = false;
	}
	
	void	Start()
	{
	}

	void	Update()
	{
		// ---------------------------------------------------------------- //
		// 色の補間.

		Vector3	rgb_current = Fader.color_to_vector3(this.colors.current);
		Vector3	rgb_goal    = Fader.color_to_vector3(this.colors.goal);

		this.is_fading = false;

		float	min_distance = 0.01f;

		if(Vector3.Distance(rgb_current, rgb_goal) < min_distance) {

			rgb_current = rgb_goal;

		} else {

			Vector3		rgb_next = Vector3.Lerp(rgb_current, rgb_goal, this.fade_rate);

			Vector3		distance_vector = rgb_next - rgb_current;

			if(distance_vector.magnitude < min_distance) {

				distance_vector.Normalize();
				distance_vector *= min_distance;

				rgb_next = rgb_current + distance_vector;
			}
			rgb_current = rgb_next;

			this.is_fading = true;
		}


		// ---------------------------------------------------------------- //
		// アルファーの補間.

		float	alpha_current = this.colors.current.a;
		float	alpha_goal    = this.colors.goal.a;

		if(Mathf.Abs(alpha_goal - alpha_current) < min_distance) {

			alpha_current = alpha_goal;

		} else {

			float	alpha_next = Mathf.Lerp(alpha_current, alpha_goal, this.fade_rate);

			if(Mathf.Abs(alpha_next - alpha_current) < min_distance) {

				alpha_next = alpha_current + min_distance*Mathf.Sign(alpha_next - alpha_current);

			}
			alpha_current = alpha_next;

			this.is_fading = true;
		}

		// ---------------------------------------------------------------- //
		// UI.Image にセットする.

		this.colors.current = Fader.vector3_to_color(rgb_current, alpha_current);

		if(this.colors.current.a > 0.0f) {

			this.ui_image.color = this.colors.current;
			this.ui_image.enabled = true;

		} else {

			this.ui_image.enabled = false;
		}
	}

	// ================================================================ //

	//public Sprite2DControl	getSprite()
	//{
	//	return(this.sprite);
	//}

	// フェードを開始する.
	public void		setGoal(Color goal)
	{
		this.colors.goal = goal;
	}

	// 現在の色を指定する.
	public void		setCurrentAnon(Color current)
	{
		this.colors.current = current;
		this.colors.goal    = this.colors.current;
	}

	// フェード中？.
	public bool		isFading()
	{
		return(this.is_fading);
	}

	// ================================================================ //

	protected static Vector3	color_to_vector3(Color color)
	{
		return(new Vector3(color.r, color.g, color.b));
	}

	protected static Color		vector3_to_color(Vector3 v, float alpha)
	{
		return(new Color(v.x, v.y, v.z, alpha));
	}
}

} // namespace Botan
