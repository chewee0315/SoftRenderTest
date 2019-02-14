using System.Collections.Generic;

namespace mex {

public enum OPERATOR_TYPE {

	NONE = -1,

	SUBSTITUTE = 0,		// ë„ì¸.
	PLUS,
	MINUS,
	MULTIPLY,
	DIVIDE,

	NEGATE,				// íPçÄââéZéqÇÃÉ}ÉCÉiÉX.
	
	COMMA,

	FUNCTION,

	//

	DOT,

	NUM,
};

// óDêÊìxÇ™ìØÇ∂Ç∆Ç´Ç…âEç∂Ç«ÇøÇÁÇóDêÊÇ∑ÇÈÇ©.
public enum COMBINATION_POLICY {

	NONE = -1,

	LEFT = 0,
	RIGHT,

	NUM,
};

public class mexObject {

	public string			str;

	public mexObject()
	{
		this.str = "";
	}

	public mexObject(string s)
	{
		this.str = s;
	}

	public override string ToString()
	{
		return(this.str);
	}
};

public class Operator : mexObject {

	public OPERATOR_TYPE	type;

	public Operator(OPERATOR_TYPE type)
	{
		this.type = type;

		switch(this.type) {

			case OPERATOR_TYPE.SUBSTITUTE:	this.str = "=";	break;
			case OPERATOR_TYPE.PLUS:		this.str = "+";	break;
			case OPERATOR_TYPE.MINUS:		this.str = "-";	break;
			case OPERATOR_TYPE.MULTIPLY:	this.str = "*";	break;
			case OPERATOR_TYPE.DIVIDE:		this.str = "/";	break;

			case OPERATOR_TYPE.NEGATE:		this.str = "-";	break;

			case OPERATOR_TYPE.COMMA:		this.str = ",";	break;

			//case OPERATOR_TYPE.FUNCTION:	str = "";	break;

			case OPERATOR_TYPE.DOT:			this.str = ".";	break;
		}
	}

	public override string ToString()
	{
		return(this.str);
	}
};

public class Parency : mexObject {

	public enum Place {

		None = -1,

		Left = 0,
		Right,

		Num,
	}

	public Place	place;

	public Parency(Place place)
	{
		this.place = place;

		switch(this.place) {

			case Place.Left:	this.str = "(";	break;
			case Place.Right:	this.str = ")";	break;
		}
	}

	public override string ToString()
	{
		return(this.str);
	}
};

public class MathExpression {

	public OPERATOR_TYPE	last_op;
	//public string			str;

	public List<mexObject>	objects = new List<mexObject>();

	// ================================================================ //

	public MathExpression()
	{
		this.objects.Clear();
	}

	public MathExpression(string s)
	{
		//this.str = s;
		this.last_op = OPERATOR_TYPE.NONE;
		this.objects.Clear();
		this.objects.Add(new mexObject(s));
	}

	public MathExpression(float v)
	{
		this.last_op = OPERATOR_TYPE.NONE;

		this.objects.Clear();

		string	s = "";

		if(v == 1.0f) {
			s = "1";
		} else if(v == 0.0f) {
			s = "0";
		} else {
			s = v.ToString();
		}

		this.objects.Add(new mexObject(s));
	}

	public MathExpression(string s, OPERATOR_TYPE last_op)
	{
		//this.str = s;
		//this.last_op = last_op;
	}

	// ================================================================ //

	public static readonly MathExpression	zero  = new MathExpression(0.0f);
	public static readonly MathExpression	one   = new MathExpression(1.0f);
	public static readonly MathExpression	empty = new MathExpression();

	// ================================================================ //
	// ìÒçÄââéZéq.

	// "+".
	public static MathExpression	operator+(MathExpression v0, MathExpression v1)
	{
		MathExpression	ans = MathExpression.empty;

		if(v0 == MathExpression.zero) {

			ans = v1;

		} else {

			if(v1 == new MathExpression(0.0f)) {

				ans = v0;

			} else {

				ans = v0.operator_entity(OPERATOR_TYPE.PLUS, "+", v1);
			}
		}

		return(ans);
	}

	// "-".
	public static MathExpression operator-(MathExpression v0, MathExpression v1)
	{ 
		MathExpression	ans = MathExpression.empty;

		if(v0 == MathExpression.zero) {

			ans = -v1;

		} else {

			if(v1 == MathExpression.zero) {

				ans = v0;

			} else {

				ans = v0.operator_entity(OPERATOR_TYPE.MINUS, "-", v1);
			}
		}

		return(ans);
	}

	// "*".
	public static MathExpression operator*(MathExpression v0, MathExpression v1)
	{ 
		MathExpression	ans = MathExpression.empty;

		if(v0 == MathExpression.zero || v1 == MathExpression.zero) {

			ans = MathExpression.zero;

		} else if(v0 == MathExpression.one) {

			ans = v1;

		} else if(v1 == MathExpression.one) {

			ans = v0;

		} else {

		#if true
		#else
			ans = v0.operator_entity(OPERATOR_TYPE.MULTIPLY, "*", v1);
		#endif
		}

		return(ans);
	}

	// "/".
	public static MathExpression operator/(MathExpression v0, MathExpression v1)
	{ 
		MathExpression	ans = MathExpression.empty;

		if(v1 == MathExpression.zero) {

			ans = new MathExpression("inf");

		} else if(v0 == MathExpression.zero) {

			ans = MathExpression.zero;

		} else if(v1 == MathExpression.one) {

			ans = v0;

		} else {

			ans = v0.operator_entity(OPERATOR_TYPE.DIVIDE, "/", v1);
		}

		return(ans);
	}

	public static MathExpression operator-(MathExpression v0)
	{
		MathExpression	t = new MathExpression();

		t.objects.Add(new Operator(OPERATOR_TYPE.NEGATE));

		if(MathExpression.operator_priority(OPERATOR_TYPE.NEGATE) > MathExpression.operator_priority(v0.last_op)) {
			t.objects.Add(new Parency(Parency.Place.Left));
			t.objects.AddRange(v0.objects);
			t.objects.Add(new Parency(Parency.Place.Right));
		} else {
			t.objects.AddRange(v0.objects);
		}

		t.last_op = OPERATOR_TYPE.NEGATE;

		return(t);
	}

	protected MathExpression operator_entity(OPERATOR_TYPE op, string op_str, MathExpression a)
	{ 
		MathExpression	t = new MathExpression();

		if(MathExpression.operator_priority(op) > MathExpression.operator_priority(this.last_op)) {
			t.objects.Add(new Parency(Parency.Place.Left));
			t.objects.AddRange(this.objects);
			t.objects.Add(new Parency(Parency.Place.Right));
		} else {
			t.objects.AddRange(this.objects);
		}

		t.objects.Add(new Operator(op));

		if(MathExpression.operator_priority(op) > MathExpression.operator_priority(a.last_op)) {
			t.objects.Add(new Parency(Parency.Place.Left));
			t.objects.AddRange(a.objects);
			t.objects.Add(new Parency(Parency.Place.Right));
		} else {
			t.objects.AddRange(a.objects);
		}

		t.last_op = op;

		return(t);
	}

	private static int	operator_priority(OPERATOR_TYPE op)
	{
		int		pri = 0;

		switch(op) {

			case OPERATOR_TYPE.PLUS:
			case OPERATOR_TYPE.MINUS:		pri = 0;	break;

			case OPERATOR_TYPE.MULTIPLY:
			case OPERATOR_TYPE.DIVIDE:		pri = 1;	break;

			case OPERATOR_TYPE.NEGATE:		pri = (int)OPERATOR_TYPE.NUM;	break;
			case OPERATOR_TYPE.FUNCTION:	pri = 2;	break;

			default:
			{
				pri = (int)OPERATOR_TYPE.NUM;
			}
			break;
		};

		return(pri);
	}

	// ================================================================ //
	// î‰ärââéZéq.

	public static bool	operator==(MathExpression v0, MathExpression v1) 
	{
		return(v0.Equals(v1));
	}

	public static bool	operator !=(MathExpression v0, MathExpression v1) 
	{
		return(!v0.Equals(v1));
	}

	public override bool Equals(object obj)
	{
		bool	ret = false;

		if(obj is MathExpression) {
			ret = this.Equals((MathExpression)obj);
		}

		return(ret);
	}
	public bool Equals(MathExpression other)
	{
		bool	is_equal = false;

		if(this.objects.Count == other.objects.Count) {

			is_equal = true;

			for(int i = 0;i < this.objects.Count;i++) {

				if(this.objects[i].str != other.objects[i].str) {
					is_equal = false;
					break;
				}
			}
		}

		return(is_equal);
	}
	/*public override int GetHashCode()
	{
		return(this.str.GetHashCode());
	}*/

	// ================================================================ //

	public override string ToString()
	{
		string	str = "";

		for(int i = 0;i < this.objects.Count;i++) {

			bool	is_fuction = false;

			if(i > 0) {
				if((this.objects[i - 1] as mex.Function) != null) {
					is_fuction = true;
				}
			}

			if(is_fuction) {
				str += "(";
			}

			str += this.objects[i].ToString();

			if(is_fuction) {
				str += ")";
			}
		}

		return(str);
	}

	// ================================================================ //

} // MathExpression

// êîäwä÷êî.
public class Function : mexObject {

	public Function(string s) : base(s)
	{
	}

	// ================================================================ //

	// sinf.
	public static MathExpression sinf(MathExpression a)
	{
		string func_str = "";

		func_str = "sinf";

		return(Function.function_entity(a, func_str));
	}

	// cosf.
	public static MathExpression cosf(MathExpression a)
	{
		string func_str = "";

		func_str = "cosf";

		return(Function.function_entity(a, func_str));
	}

	// ================================================================ //

	protected static MathExpression function_entity(MathExpression v, string func_str)
	{
		MathExpression		ans = new MathExpression();

		ans.objects.Add(new Function(func_str));
		//ans.objects.Add(new Parency(Parency.Place.Left));
		ans.objects.AddRange(v.objects);
		//ans.objects.Add(new Parency(Parency.Place.Right));

		ans.last_op = OPERATOR_TYPE.FUNCTION;
		return(ans);
	}

};

// ---------------------------------------------------------------- //
//																	//
//																	//
//																	//
// ---------------------------------------------------------------- //

} // namespace mex



