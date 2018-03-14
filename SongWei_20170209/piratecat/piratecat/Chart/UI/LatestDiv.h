/*****************************************************************************\
*                                                                             *
* LatestDiv.h - Latest div functions, types, and definitions.                 *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/21.                                    *
*                                                                             *
******************************************************************************/

#ifndef __LATESTDIV_H__
#define __LATESTDIV_H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "..\\Service\\DataCenter.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\..\\..\\includes\\Sock\\QuoteService.h"

class OwChart;

class LatestDiv : public ControlA
{
private:
	OwChart *m_chart;
	int m_digit;
	GridA *m_gridTransaction;
	bool m_lV2;
	int m_requestID;
	SecurityLatestData m_latestData;
	SecurityLatestDataLV2 m_latestDataLV2;
	String m_securityCode;
	String m_securityName;
	int m_type;
public:
	LatestDiv();
	virtual ~LatestDiv();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	int GetDigit();
	void SetDigit(int digit);
	SecurityLatestData GetLatestData();
	bool IsLV2();
	void SetLV2(bool lv2);
	int GetRequestID();
	String GetSecurityCode();
	void SetSecurityCode(const String& securityCode);
	String GetSecurityName();
	void SetSecurityName(const String& securityName);
	int GetType();
	void SetType(int type);
public:
	static void __stdcall LatestDataCallBack(CMessage *message, void *pInvoke);
	double Max(vector<double> *list);
	virtual void OnAdd();
	virtual void OnInvoke(void *args);
	virtual void OnLatestDataCallBack(CMessage *message);
	virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
	virtual void Update();
};

class TransactionDateCell : public GridStringCell
{
public:
	TransactionDateCell();
	virtual ~TransactionDateCell();
public:
	virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
};

class TransactionDoubleCell : public GridDoubleCell
{
private:
	int m_digit;
public:
	TransactionDoubleCell();
	~TransactionDoubleCell();
	int GetDigit();
	void SetDigit(int digit);
public:
	virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
};
#endif