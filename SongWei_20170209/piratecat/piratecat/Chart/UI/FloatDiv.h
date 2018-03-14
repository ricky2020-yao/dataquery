/*****************************************************************************\
*                                                                             *
* FloatDiv.h - Float div functions, types, and definitions.                   *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/22.                                    *
*                                                                             *
******************************************************************************/

#ifndef __FLOATDIV_H__
#define __FLOATDIV_H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"

class OwChart;

class FloatDiv:public ControlA
{
private:
	OwChart *m_chart;
	int m_digit;
public:
	FloatDiv();
	virtual ~FloatDiv();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	int GetDigit();
	void SetDigit(int digit);
public:
	virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
	virtual void OnPaintBorder(CPaint *paint, const RECT& clipRect);
};
#endif