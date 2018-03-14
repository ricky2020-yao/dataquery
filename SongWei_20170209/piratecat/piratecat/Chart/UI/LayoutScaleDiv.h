/*****************************************************************************\
*                                                                             *
* LayoutScaleDiv.h - Layout scale div functions, types, and definitions.      *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/5/25.                                    *
*                                                                             *
******************************************************************************/

#ifndef __LAYOUTSCALEDIV_H__
#define __LAYOUTSCALEDIV_H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "..\\Service\\DataCenter.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"

class OwChart;

class LayoutScaleDiv:public ControlA
{
private:
	OwChart *m_chart;
	String m_layoutStr;
	_int64 m_sysColors[6];
public:
	LayoutScaleDiv();
	virtual ~LayoutScaleDiv();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	String GetLayoutStr();
	void SetLayoutStr(const String& layoutStr);
public:
	virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
};
#endif