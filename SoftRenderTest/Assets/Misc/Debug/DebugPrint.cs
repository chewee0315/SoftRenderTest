using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// デバッグ文字を画面に表示するためのクラス.
// OnGUI() 以外でもデバッグプリントが使えます.
public class dbPrint : MonoBehaviour {

	private static dbPrint	instance = null;

	public struct TextItem {

		public int		x, y;
		public string	text;
		public float	lifetime;
	};

	private List<TextItem>	items;
	private int				locate_x, locate_y;

	//private static int		CHARA_W = 20;
	//private static int		CHARA_H = 20;
	private static int		CHARA_W = 12;
	private static int		CHARA_H = 24;

	public Camera	main_camera;

	protected	float		screen_scale;
	protected	GUISkin		skin;

	// ------------------------------------------------------------------------ //

	public static dbPrint	getInstance()
	{
		if(dbPrint.instance == null) {

			GameObject	go = new GameObject("DebugPrint");

			dbPrint.instance = go.AddComponent<dbPrint>();
			dbPrint.instance.create();

			//DontDestroyOnLoad(go);
		}

		return(dbPrint.instance);
	}

	// テキストを表示する.
	public static void	print(object text, float lifetime)
	{
		dbPrint	dp = dbPrint.getInstance();

		dp.add_text(text.ToString(), lifetime);
	}
	public static new void	print(object text)
	{
		// ↑MonoBehavior.print() を隠ぺいするので "new" をつける.
		dbPrint.print(text, 0.0f);
	}

	// 表示位置をセットする.
	public static void	setLocate(int x, int y)
	{
		dbPrint	dp = dbPrint.getInstance();

		dp.set_locate(x, y);
	}

	// ３Dのワールド座標を指定して、テキストを表示する.
	public static void	print3d(Vector3 position, object text)
	{
		dbPrint	dp = dbPrint.getInstance();

		if(dp.main_camera != null) {

			position = dp.main_camera.WorldToScreenPoint(position);

			position.y = Screen.height - position.y;

			dp.set_locate_in_pixels((int)position.x, (int)position.y);
			dp.add_text(text.ToString(), 0.0f);
		}
	}
	public static void	print3dWithOffset(Vector3 position, Vector2 offset_in_screen, object text)
	{
		dbPrint	dp = dbPrint.getInstance();

		if(dp.main_camera != null) {

			position = dp.main_camera.WorldToScreenPoint(position);

			position.y = Screen.height - position.y;

			position.x += offset_in_screen.x;
			position.y += offset_in_screen.y;

			dp.set_locate_in_pixels((int)position.x, (int)position.y);
			dp.add_text(text.ToString(), 0.0f);
		}
	}

	// カメラを明示的にセットする.
	// （print3D を使う、かつ "MainCamera" タグがカメラについていないとき用）.
	public static void	setCamera(Camera camera)
	{
		dbPrint	dp = dbPrint.getInstance();

		dp.main_camera = camera;
	}

	// ------------------------------------------------------------------------ //

	void Start ()
	{
		this.clear();

#if UNITY_EDITOR
		this.screen_scale = 1.0f;
#else
		this.screen_scale = Mathf.Min(Screen.width/640.0f, Screen.height/480.0f)*2.0f;
#endif
	}
	
	void Update ()
	{

	}

	void OnGUI()
	{
		if(this.skin == null) {

			this.skin = GUISkin.Instantiate<GUISkin>(GUI.skin);

			int		font_size = Mathf.FloorToInt(14.0f*this.screen_scale);

			this.skin.label.fontSize  = font_size;
			this.skin.button.fontSize = font_size;
			this.skin.box.fontSize    = font_size;
			this.skin.toggle.fontSize = font_size;
			//this.skin.window.fontSize = font_size;
		}
		GUI.skin = this.skin;

		// バッファーにたまっているテキストを表示する.

		int		x, y;

		foreach(var item in this.items) {

			x = item.x;
			y = item.y;

			GUI.Box(new Rect(x, y, item.text.Length*dbPrint.CHARA_W*this.screen_scale + 4, dbPrint.CHARA_H*this.screen_scale), item.text);

			y += (int)(dbPrint.CHARA_H*this.screen_scale);
		}

		// バッファーをクリアーする.

		if(UnityEngine.Event.current.type == UnityEngine.EventType.Repaint) {

			this.clear();
		}
	}

	public void	create()
	{
		this.items = new List<TextItem>();

		this.main_camera = Camera.main;
	}

	// バッファーをクリアーする.
	private void	clear()
	{
		this.locate_x = 10;
		this.locate_y = 10;

		for(int i = 0;i < this.items.Count;i++) {

			TextItem	item = this.items[i];

			if(item.lifetime >= 0.0) {

				item.lifetime -= Time.deltaTime;
	
				this.items[i] = item;
	
				if(item.lifetime <= 0.0f) {
	
					this.items.Remove(this.items[i]);
				}
			}
		}
	}

	// 表示位置をセットする.
	protected void	set_locate(int x, int y)
	{
		this.locate_x = x*dbPrint.CHARA_W;
		this.locate_y = y*dbPrint.CHARA_H;
	}

	// [pixel] 表示位置をセットする.
	protected void	set_locate_in_pixels(int x, int y)
	{
		this.locate_x = x;
		this.locate_y = y;
	}

	// テキストを追加する.
	protected void	add_text(string text, float lifetime)
	{
		TextItem	item;

		item.x        = this.locate_x;
		item.y        = this.locate_y;
		item.text     = text;
		item.lifetime = lifetime;

		this.items.Add(item);

		this.locate_y += dbPrint.CHARA_H;
	}
}

