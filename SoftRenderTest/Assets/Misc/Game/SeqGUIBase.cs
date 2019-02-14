using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeqGUIBase : MonoBehaviour {

	public string	selected = "";

	// ---------------------------------------------------------------- //

	protected List<Botan.uiButton>	buttons = new List<Botan.uiButton>();

	// ================================================================ //
	// MonoBehaviour からの継承.

	void	Awake()
	{
	}

	void	Start()
	{
	}
	
	void	Update()
	{
	}

	// ================================================================ //

	// ボタンをつくる.
	public Botan.Button	createButton(string name, Texture texture)
	{
		Botan.Button	button = BotanRoot.get().createButton(name, texture);

		button.getSprite().setDepthLayer("ui.item");
		button.on_trigger_pressed = (buttn_name) => this.selected = buttn_name;

		//this.registerButton(button);

		return(button);
	}

	public Botan.uiButton	findButton(string go_name, string button_name)
	{
		Botan.uiButton	button = BotanRoot.get().findButton(go_name);

		if(button != null) {

			button.button_name = button_name;
			button.on_trigger_pressed = (_button_name) => this.selected = _button_name;

			this.registerButton(button);

		} else {

			Debug.LogError("Can't find button \"" + go_name + "\".");
		}

		return(button);
	}

	// ボタンを登録する.
	public void		registerButton(Botan.uiButton button)
	{
		this.buttons.Add(button);
	}

	// ボタンを表示/非表示する.
	public void		setButtonVisible(bool is_visible)
	{
		foreach(var button in this.buttons) {

			button.setVisible(is_visible);
		}
	}

	// ボタンを押せる/押せないする.
	public void		setButtonActive(bool is_active)
	{
		foreach(var button in this.buttons) {

			button.setActive(is_active);
		}
	}

	// ================================================================ //
/*
	public void	onButtonClicked(string button_name)
	{
		this.selected = button_name;
	}
*/
}