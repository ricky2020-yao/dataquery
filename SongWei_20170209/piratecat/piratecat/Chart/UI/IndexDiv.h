/*****************************************************************************\
*                                                                             *
* IndexDiv.h - Index div functions, types, and definitions.                   *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/21.                                    *
*                                                                             *
******************************************************************************/

#ifndef __INDEXDIV_H__
#define __INDEXDIV_H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "..\\Service\\DataCenter.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"

class OwChart;

class IndexDiv:public ControlA
{
private:
	OwChart *m_chart;
	SecurityLatestData m_cyLatestData;
	int m_requestID;
	SecurityLatestData m_ssLatestData;
	SecurityLatestData m_szLatestData;
public:
	IndexDiv();
	virtual ~IndexDiv();
	OwChart* GetChart();
	void SetChart(OwChart* chart);
public:
	static void __stdcall LatestDataCallBack(CMessage *message, void *pInvoke);
	virtual void OnInvoke(void *args);
	virtual void OnLatestDataCallBack(CMessage* message);
	virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
	void Start();
};
#endif