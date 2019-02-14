#include	"stdafx.h"

#include	<stdio.h>

#include	"math_expression.h"
#include	"mex_symbol.h"
#include	"mex_itpr.h"

namespace mex {


// ---------------------------------------------------------------- //
//																	//
//		ƒeƒXƒg														//
//																	//
// ---------------------------------------------------------------- //

#if 0
	mathExpression	a;
	mathExpression	b;
	mathExpression	c;

	a = "a";
	b = "b";
	c = a + b;

	appBase::outputConsole("%s\n", a.c_str());
	appBase::outputConsole("%s\n", b.c_str());
	appBase::outputConsole("%s\n", (a + b).c_str());
	appBase::outputConsole("%s\n", (a - b).c_str());
	appBase::outputConsole("%s\n", (a * b).c_str());
	appBase::outputConsole("%s\n", (a / b).c_str());
	appBase::outputConsole("%s\n", (a + b*a).c_str());
	appBase::outputConsole("%s\n", ((a + b)*a).c_str());
	appBase::outputConsole("%s\n", sinf(a).c_str());
	appBase::outputConsole("%s\n", ((sinf(a) + b)*cosf(a + b)).c_str());

	mathExpression	zero, one;
	zero = 0.0f;
	one = 1.0f;

	appBase::outputConsole("\n");
	appBase::outputConsole("%s\n", (a + zero).c_str());
	appBase::outputConsole("%s\n", (zero + a).c_str());
	appBase::outputConsole("%s\n", (a - zero).c_str());
	appBase::outputConsole("%s\n", (zero - a).c_str());
	appBase::outputConsole("%s\n", (a*zero).c_str());
	appBase::outputConsole("%s\n", (zero*a).c_str());
	appBase::outputConsole("%s\n", (a*one).c_str());
	appBase::outputConsole("%s\n", (one*a).c_str());
	appBase::outputConsole("%s\n", (a/zero).c_str());
	appBase::outputConsole("%s\n", (zero/a).c_str());
	appBase::outputConsole("%s\n", (a/one).c_str());
	appBase::outputConsole("%s\n", (one/a).c_str());

	MatrixBase<mathExpression>	mat;

	mat.rotateY(mathExpression("y"));
	mat.rotateX(mathExpression("x"));
	mat.rotateZ(mathExpression("z"));

	int		i, j;

	for(i = 0;i < 4;i++) {

		for(j = 0;j < 4;j++) {

			appBase::outputConsole("[%s] ", mat.f[j][i].c_str());
		}

		appBase::outputConsole("\n");
	}
#endif
// ---------------------------------------------------------------- //
//																	//
//																	//
//																	//
// ---------------------------------------------------------------- //

} // namespace mex
