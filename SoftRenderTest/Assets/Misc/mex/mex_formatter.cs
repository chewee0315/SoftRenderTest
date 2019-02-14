using System.Collections.Generic;


namespace mex {

#if false
public class Formatter {

	public class Item {

		public OPERATOR_TYPE	op = OPERATOR_TYPE.NONE;
		public MathExpression	value;

		public static Item		createValue(string str)
		{
			Item	item = new Item();

			item.value = new MathExpression(str);

			return(item);
		}
	}

	// ================================================================ //

	public Formatter()
	{
	}

	// ================================================================ //

	// sinf.
	public static List<Item>	desolve(MathExpression x)
	{
		List<Item>	exps = new List<Item>();

		int		s, e;

		s = 0;

		for(e = s + 1;e < x.str.Length;e++) {

			if(char.IsLetter(x.str[e])) {
				continue;
			}
			if(x.str[e] == '_') {
				continue;
			}

			break;
		}

		string		substr = x.str.Substring(s, e - s + 1);

		exps.Add(Item.createValue(substr));

			/*string	substr = x.str.Substring(i);

			for(int j = 0;j < (int)OPERATOR_TYPE.NUM;j++) {

				OPERATOR_TYPE op = (OPERATOR_TYPE)j;

				if(substr.StartsWith(Formatter.getOperaterString(op))) {

				}
			}*/

			//op_type = Formatter.toOperaterType(x.str[i]);
		//}

		return(exps);
	}

	public static string		getOperaterString(OPERATOR_TYPE op)
	{
		string	str = "";

		switch(op) {

			case OPERATOR_TYPE.SUBSTITUTE:	str = "=";	break;
			case OPERATOR_TYPE.PLUS:		str = "+";	break;
			case OPERATOR_TYPE.MINUS:		str = "-";	break;
			case OPERATOR_TYPE.MULTIPLY:	str = "*";	break;
			case OPERATOR_TYPE.DIVIDE:		str = "/";	break;

			case OPERATOR_TYPE.COMMA:		str = ",";	break;

			//case OPERATOR_TYPE.FUNCTION:	str = "";	break;

			case OPERATOR_TYPE.DOT:			str = ".";	break;
		}

		return(str);
	}

} // Formatter
#endif

// ---------------------------------------------------------------- //
//																	//
//																	//
//																	//
// ---------------------------------------------------------------- //

} // namespace mex



