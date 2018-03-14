/*****************************************************************************\
*                                                                             *
* SandBoxWindow.h - Sandbox window functions, types, and definitions.         *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/6/4.                                     *
*                                                                             *
******************************************************************************/

#ifndef __SANDBOXWINDOW_H__
#define __SANDBOXWINDOW_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "OwChart.h"
#include "WindowEx.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\SecurityDataHelper.h"

class SandBoxCandleDiv : public ControlA
{
private:
	OwChart *m_chart;
	SecurityData m_data;
	float m_destHighY;
	POINT m_destPoint;
	int m_direction;
	bool m_moveStart;
	bool m_moving;
	POINT m_originLocation;
	float m_perDyncMoveStep;
	int m_perFixMoveStep;
	int m_perFixMoveTimes;
	float m_slope;
	POINT m_tempStops;
	int m_ticks;
	int m_totalDistance;
	float m_yIntercept;
private:
	int GetMovingLocation();
	void GetDestPos(POINT *location, float *destHighY);
	void GetDestSize(SIZE *size);
public:
	SandBoxCandleDiv();
	~SandBoxCandleDiv();
	OwChart* GetChart();
	void SetChart(OwChart* chart);
	SecurityData GetData();
	void SetData(SecurityData data);
	int GetDirection();
	void SetDirection(int direction);
	bool IsMoving();
	void SetMoving(bool moving);
public:
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
	bool OnTimer();
};

class SandBoxWindow : public WindowXmlEx
{
private:
	OwChart *m_chart;
	SIZE m_defaultCandleSize;
	POINT m_defaultCandleLocation;
	SIZE m_defaultWindowSize;
	vector<SandBoxCandleDiv*> m_divs;
	NativeBase *m_native;
	ControlTimerEvent m_timerEvent;
	int m_timerID;
	bool m_useAnimation;
	WindowEx *m_window;
private:
	static void CallTimer(void *sender, int timerID, void *pInvoke);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	void ConvertStrToDate(const String& str, int *year, int *month, int *day, int *hour, int *minute, int *second, int *ms);
	void CreateCandlePos(SIZE size, float candleSeparater, POINT *newPoint);
	void CreatePreView();
	void CreateCandleSize(SIZE *newSize, float *separator);
	void CreateNextData(bool addOrDelete);
	void Delete();
	void RegisterEvents(ControlA *control);
	void SetCandleStaticValue(SandBoxCandleDiv *div);
	void SetSpinValue(double date);
	static void SpinTextChanged(void *sender, void *pInvoke);
public:
	SandBoxWindow(NativeBase *native);
	~SandBoxWindow();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	bool UseAnimation();
	void SetUseAnimation(bool useAnimation);
	WindowA* GetWindow();
public:
	void Add();
	virtual void Close();
	void OnSpinTextChanged(SpinA *spin);
	void OnTimer(int timerID);
	virtual void Show();
};

#endif