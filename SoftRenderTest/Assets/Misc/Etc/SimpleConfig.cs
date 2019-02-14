using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SimpleConfig {

	// ---------------------------------------------------------------- //

	public class Line {

		public int				line_number = -1;				// テキストファイル中での行番号.
		public string			command = "";
		public List<string>		parameters = new List<string>();

		public bool		checkParameterCount(int count)
		{
			bool	ret = true;

			if(this.parameters.Count < count) {

				ret = false;
				Debug.LogError("Line " + (this.line_number + 1).ToString() + ":Out of parameters.");
			}

			return(ret);
		}
	}
	public List<Line>	lines;

	// ================================================================ //
	
	public SimpleConfig()
	{
		this.lines = new List<Line>();
	}

	public void	load(TextAsset text_asset)
	{
		this.lines.Clear();

		// テキスト全体をひとつの文字列に.
		string		as_text = text_asset.text;

		// 改行コードで区切ることで、
		// テキスト全体を一行単位の配列にする.
		string[]	line_text_array = as_text.Split('\n');
		int			n = -1;

		foreach(var line_text in line_text_array) {

			n++;

			if(line_text == "") {

				continue;
			}

			// 空白で区切って、単語の配列にする.
			string[]	word_array = line_text.Split();

			Line	line = new Line();

			foreach(var word in word_array) {

				// "#" 以降はコメントなのでそれ以降はスキップ.
				if(word.StartsWith("#")) {

					break;
				}
				if(word == "") {

					continue;
				}

				if(line.command == "") {

					line.command = word;

				} else {

					line.parameters.Add(word);
				}
			}

			// 単語がひとつもない行は保存しない.
			if(line.command == "") {

				continue;
			}

			line.line_number = n;
			this.lines.Add(line);
		}
	}
}