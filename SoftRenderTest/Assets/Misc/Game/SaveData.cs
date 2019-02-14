using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// プレイヤーデーターのセーブ/ロード.
//
// UnityEditor から実行したとき　…　FileStream
// WebPlayer   から実行したとき　…　PlayerPrefs
//
// を使います。
//
// PlayerPrefs は UnityEditor から実行したときにレジストリー（！）に
// 保存してしまうので.
//
// 使用例は↓の方にあります.
//
public class SaveData {

	// セーブデーターの項目（名まえ/型/値のセット）.
	public List<Item>	items = new List<Item>();

	// ================================================================ //

	public SaveData() {}

	// ================================================================ //
	// シリアライズ.

	public enum SERIALIZE_MODE {

		NONE = -1,
		CREATE = 0,
		WRITE,
		READ,

		NUM,
	}
	public SERIALIZE_MODE	serialize_mode = SERIALIZE_MODE.NONE; 

	public int		serializeInt(string name, int default_value)
	{
		int		ret = default_value;

		switch(this.serialize_mode) {

			case SERIALIZE_MODE.CREATE:
			{
				this.addInt(name,  default_value);
			}
			break;

			case SERIALIZE_MODE.WRITE:
			{
				this.setInt(name,  default_value);
			}
			break;

			case SERIALIZE_MODE.READ:
			{
				ret = this.getInt(name,  default_value);
			}
			break;
		}

		return(ret);
	}

	// -------------------------------------------------------- //

	public bool		isHasItem(string name)
	{
		bool	ret = (this.items.Find(x => x.name == name) != null);

		return(ret);
	}

	// 文字アイテムを追加する.
	public void		addString(string name, string value)
	{
		if(!this.isHasItem(name)) {

			Item	item = new Item();
	
			item.name  = name;
			item.type  = TYPE.STRING;
			item.value = value;
	
			this.items.Add(item);
		}
	}

	// int の アイテムを追加する.
	public void		addInt(string name, int value)
	{
		if(!this.isHasItem(name)) {

			Item	item = new Item();
	
			item.name  = name;
			item.type  = TYPE.INT;
			item.value = value.ToString();
	
			this.items.Add(item);
		}
	}

	// 文字列アイテムに値をセットする.
	public bool		setString(string name, string value)
	{
		bool	ret = false;

		do {

			Item		item = this.items.Find(x => x.name == name);

			if(item == null) {

				break;
			}
			if(item.type != TYPE.STRING) {

				break;
			}

			item.value = value;

			ret = true;

		} while(false);

		return(ret);
	}

	// int アイテムに値をセットする.
	public bool		setInt(string name, int value)
	{
		bool	ret = false;

		do {

			Item		item = this.items.Find(x => x.name == name);

			if(item == null) {

				break;
			}
			if(item.type != TYPE.INT) {

				break;
			}

			item.value = value.ToString();

			ret = true;

		} while(false);

		return(ret);
	}
	
	// int アイテムから値をゲットする.
	public int		getInt(string name, int default_value = -1)
	{
		int		ret = default_value;

		do {

			Item		item = this.items.Find(x => x.name == name);

			if(item == null) {

				break;
			}
			if(item.type != TYPE.INT) {

				break;
			}

			if(!int.TryParse(item.value, out ret)) {

				ret = default_value;
			}

		} while(false);

		return(ret);
	}

	// ================================================================ //

	// ロード.
	public void		load()
	{
#if UNITY_EDITOR
		this.load_from_stream();
#else
		this.load_from_prefs();
#endif
	}

	// セーブ.
	public void		save()
	{
#if UNITY_EDITOR
		this.save_to_stream();
#else
		this.save_to_prefs();
#endif
	}

	// ================================================================ //
	// FileStream を使うばん（UnityEditor で使う）.

	// セーブ.
	public void		save_to_stream()
	{
		DebugWindow.get().console.print("[SaveData.save] unity editor");

		FileStream		stream = new FileStream("save_data.dat", FileMode.Create, FileAccess.Write);
		StreamWriter	writer = new StreamWriter(stream);

		foreach(var item in this.items) {
	
			writer.WriteLine(item.toString());
		}

		writer.Close();
	}

	// ロード.
	public void		load_from_stream()
	{
		DebugWindow.get().console.print("[SaveData.load] unity editor");

		FileStream		stream = new FileStream("save_data.dat", FileMode.Open, FileAccess.Read);
		StreamReader	reader = new StreamReader(stream);

		string	line_text;

		while((line_text = reader.ReadLine()) != null) {

			Item	item = Item.fromString(line_text);

			// 同じ名前のものがあったら、消しておく.
			this.items.RemoveAll(x => x.name == item.name);

			this.items.Add(item);
		}

		reader.Close();
	}

	// ================================================================ //
	// PlayerPrefs を使うばん（WebPlayer で使う）.

	// セーブ.
	public void		save_to_prefs()
	{
		DebugWindow.get().console.print("[SaveData.save] web player");

		foreach(var item in this.items) {

			switch(item.type) {

				case TYPE.STRING:
				{
					PlayerPrefs.SetString(item.name, item.value);	
				}
				break;

				case TYPE.INT:
				{
					int	int_value;

					if(int.TryParse(item.value, out int_value)) {

						PlayerPrefs.SetInt(item.name, int_value);
					}
				}
				break;

				case TYPE.FLOAT:
				{
					float	float_value;

					if(float.TryParse(item.value, out float_value)) {

						PlayerPrefs.SetFloat(item.name, float_value);
					}
				}
				break;
			}
		}

		PlayerPrefs.Save();
	}

	// ロード.
	public void		load_from_prefs()
	{
		DebugWindow.get().console.print("[SaveData.load] web player");

		foreach(var item in this.items) {

			switch(item.type) {

				case TYPE.STRING:
				{
					item.value = PlayerPrefs.GetString(item.name, item.value);	
				}
				break;

				case TYPE.INT:
				{
					int	int_value;

					if(!int.TryParse(item.value, out int_value)) {

						int_value = -1;
					}

					int_value = PlayerPrefs.GetInt(item.name, int_value);

					item.value = int_value.ToString();
				}
				break;

				case TYPE.FLOAT:
				{
					float	float_value;

					if(!float.TryParse(item.value, out float_value)) {

						float_value = -1.0f;
					}

					float_value = PlayerPrefs.GetFloat(item.name, float_value);

					item.value = float_value.ToString();
				}
				break;
			}
		}
	}

	// ================================================================ //

	// アイテムの値の型.
	public enum TYPE {

		NONE = -1,

		STRING = 0,
		INT,
		FLOAT,

		NUM,
	};

	// アイテム.
	public class Item {

		public	string	name  = "";
		public	TYPE	type  = TYPE.NONE;
		public	string	value = "";

		// 文字列に変換する.
		public	string	toString()
		{
			string str = "";

			str += this.name;
			str += "/";
			str += this.type.ToString().ToLower();
			str += "/";
			str += this.value.ToString();

			return(str);
		}

		// 文字列からつくる.
		public static Item		fromString(string str)
		{
			Item		item = new Item();

			do {

				char[]		separators = {'/'};
				string[]	words = str.Split(separators);
	
				if(words.Length < 3) {
	
					break;
				}
	
				item.name  = words[0];
				item.value = words[2];
	
				switch(words[1]) {
	
					case "string":	item.type = TYPE.STRING;	break;
					case "int":		item.type = TYPE.INT;		break;
					case "float":	item.type = TYPE.FLOAT;		break;
				}

			} while(false);

			return(item);
		}
	};

}
// 使用例
#if false

	// ================================================================ //
	// セーブデーターのテストコード.

	protected SaveData	save_data = null;

	public bool	save_data_loaded = false;				// セーブデーターをロードした？.

	protected int	hi_score  = 100;
	protected int	max_coins = 20;

	// セーブデーターをロードする（初回のみ）.
	public void		loadSaveData()
	{
		if(!this.save_data_loaded) {

			// 実際にロードするのは最初の一回だけ（遅いから）.
			this.save_data.load();
			this.save_data_loaded = true;

			this.save_data.serialize_mode = SaveData.SERIALIZE_MODE.READ;
			this.serialize_save_data();

			foreach(SaveData.Item item in this.save_data.items) {

				DebugWindow.get().console.print(item.name + " " + item.value);
			}
		}
	}

	// セーブデーターをセーブする.
	public void		saveSaveData()
	{
		this.save_data.serialize_mode = SaveData.SERIALIZE_MODE.WRITE;
		this.serialize_save_data();

		this.save_data.save();
	}

	// セーブデーターをゲットする.
	public SaveData		getSaveData()
	{
		return(this.save_data);
	}

	// セーブデーター（を、読み書きするためのオブジェクト）を作る.
	protected void		create_save_data()
	{
		this.save_data = new SaveData();
		this.save_data.serialize_mode = SaveData.SERIALIZE_MODE.CREATE;
		this.serialize_save_data();
	}

	protected void		serialize_save_data()
	{
		this.hi_score  = this.save_data.serializeInt("Hi-Score",   this.hi_score);
		this.max_coins = this.save_data.serializeInt("Max-Coins",  this.max_coins);
	}

	void	Start()
	{
		this.create_save_data();
		this.loadSaveData();
	}

#endif
