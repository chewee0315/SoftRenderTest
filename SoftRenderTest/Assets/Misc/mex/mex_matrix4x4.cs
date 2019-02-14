
namespace mex {

public class Matrix4x4 {

	public const int	s_column_count = 4;
	public const int	s_row_count = 4;

	public MathExpression[,]	m = new MathExpression[s_column_count, s_row_count];

	// ================================================================ //

	public Matrix4x4()
	{
		for(int x = 0;x < this.m.GetLength(0);x++) {

			for(int y = 0;y < this.m.GetLength(1);y++) {

				if(x == y) {
					this.m[x, y] = MathExpression.one;
				} else {
					this.m[x, y] = MathExpression.zero;
				}
			}
		}
	}

	public MathExpression	determinant()
	{
		MathExpression		det = MathExpression.zero;

		for(int x = 0;x < 3;x++) {
			det += this.m[x, 0] * this.m[(x + 1)%3, 1] * this.m[(x + 2)%3, 2];
		}

		for(int x = 0;x < 3;x++) {
			det -= this.m[x, 0] * this.m[(x - 1 + 3)%3, 1] * this.m[(x - 2 + 3)%3, 2];
		}

		return(det);
	}

	// ================================================================ //

	public static Matrix4x4		createRotationX(MathExpression angle)
	{
		Matrix4x4	m = new Matrix4x4();

		m.m[1, 1] =  mex.Function.cosf(angle);
		m.m[2, 1] = -mex.Function.sinf(angle);
		m.m[1, 2] =  mex.Function.sinf(angle);
		m.m[2, 2] =  mex.Function.cosf(angle);

		return(m);
	}
	public static Matrix4x4		createRotationY(MathExpression angle)
	{
		Matrix4x4	m = new Matrix4x4();

		m.m[2, 2] =  mex.Function.cosf(angle);
		m.m[0, 2] = -mex.Function.sinf(angle);
		m.m[2, 0] =  mex.Function.sinf(angle);
		m.m[0, 0] =  mex.Function.cosf(angle);

		return(m);
	}

	public static Matrix4x4	createRotationZ(MathExpression angle)
	{
		Matrix4x4	m = new Matrix4x4();

		m.m[0, 0] =  mex.Function.cosf(angle);
		m.m[1, 0] = -mex.Function.sinf(angle);
		m.m[0, 1] =  mex.Function.sinf(angle);
		m.m[1, 1] =  mex.Function.cosf(angle);

		return(m);
	}
	public static Matrix4x4	createTranslation(MathExpression x, MathExpression y, MathExpression z)
	{
		Matrix4x4	m = new Matrix4x4();

		m.m[3, 0] = x;
		m.m[3, 1] = y;
		m.m[3, 2] = z;

		return(m);
	}

	public static Matrix4x4	createPlaceHolder()
	{
		Matrix4x4	m = new Matrix4x4();

		for(int x = 0;x < m.m.GetLength(0);x++) {

			for(int y = 0;y < m.m.GetLength(1);y++) {

				m.m[x, y] = new MathExpression("m" + x + y);
			}
		}

		return(m);
	}

	// ================================================================ //
	// “ñ€‰‰ŽZŽq.

	// "-".
	public static Matrix4x4 operator-(Matrix4x4 m0, Matrix4x4 m1)
	{ 
		Matrix4x4	ans = new Matrix4x4();

		do {

			for(int x = 0;x < ans.m.GetLength(0);x++) {

				for(int y = 0;y < ans.m.GetLength(1);y++) {

					ans.m[x, y] = m0.m[x, y] - m1.m[x, y];
				}
			}

		} while(false);

		return(ans);
	}

	// "*".
	public static Matrix4x4 operator*(Matrix4x4 m0, Matrix4x4 m1)
	{ 
		Matrix4x4	ans = new Matrix4x4();

		for(int x = 0;x < ans.m.GetLength(0);x++) {

			for(int y = 0;y < ans.m.GetLength(1);y++) {

				ans.m[x, y] = m0.m[0, y]*m1.m[x, 0] + m0.m[1, y]*m1.m[x, 1] + m0.m[2, y]*m1.m[x, 2] + m0.m[3, y]*m1.m[x, 3];
			}
		}

		return(ans);
	}

	// ================================================================ //

	public override string ToString()
	{
		string	str = "";

		for(int y = 0;y < this.m.GetLength(1);y++) {

			string	line_str = "| ";

			for(int x = 0;x < this.m.GetLength(0);x++) {

				line_str += this.m[x, y];
				line_str += " ";
			}

			line_str += "|";

			str += line_str;

			if(y < this.m.GetLength(1) - 1) {
				str += "\n";
			}
		}

		return(str);
	}

	public string ToString3x3()
	{
		int		colmun_count = 3;
		int		row_count = 3;

		int[]	max_lengthes = new int[colmun_count];

		for(int x = 0;x < colmun_count;x++) {

			max_lengthes[x] = 0;

			for(int y = 0;y < row_count;y++) {

				for(int i = 0;i < this.m[x, y].objects.Count;i++) {

					max_lengthes[x] = System.Math.Max(max_lengthes[x], this.m[x, y].objects[i].str.Length);
				}
			}
		}

		string	format_str0 = "";

		for(int x = 0;x < max_lengthes.Length;x++) {

			format_str0 += "{" + x.ToString() + "," + max_lengthes[x] + "}";

			if(x < max_lengthes.Length - 1) {
				format_str0 += " ";
			}
		}

		//

		string	str = "";

		for(int y = 0;y < row_count;y++) {

			string	line_str;

			line_str = string.Format(format_str0, this.m[0, y], this.m[1, y], this.m[2, y]);

			str += "|" + line_str + "|";

			if(y < row_count - 1) {
				str += "\n";
			}
		}

		return(str);
	}

} // public class Matrix4x4


// ---------------------------------------------------------------- //
//																	//
//																	//
//																	//
// ---------------------------------------------------------------- //
#if false
template<class T> class MatrixBase {

	void	operator*=(MatrixBase& m) { 
				MatrixBase	t = *this;
				int		i, j;

				for(i = 0;i < 4;i++) for(j = 0;j < 4;j++) {

					f[i][j] = t.f[0][j]*m.f[i][0] + t.f[1][j]*m.f[i][1] + t.f[2][j]*m.f[i][2] + t.f[3][j]*m.f[i][3];
				}
			}
public:
;
#endif
} // namespace mex



