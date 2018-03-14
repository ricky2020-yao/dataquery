/*****************************************************************************\
*                                                                             *
* ParametersWindow.h -  Parameters window functions, types, and definitions.  *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/6/10.                                    *
*                                                                             *
******************************************************************************/

#ifndef __PARAMETERSWINDOW__H__
#define __PARAMETERSWINDOW__H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "OwChart.h"
#include "WindowEx.h"

class OwChart;

class ParametersWindow : public WindowXmlEx
{
private:
	OwChart *m_chart;
	CIndicator *m_indicator;
	NativeBase *m_native;
	WindowEx *m_window;
private:
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	void GetParameters();
	void RegisterEvents(ControlA *control);
	void SetParameters();
	static void SpinTextChanged(void *sender, void *pInvoke);
public:
	ParametersWindow(NativeBase *native, CIndicator *indicator);
	virtual ~ParametersWindow();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	CIndicator* GetIndicator();
	WindowEx* GetWindow();
public:
	virtual void Close();
	void OnSpinTextChanged(SpinA *spin);
	virtual void Show();
};

#endif