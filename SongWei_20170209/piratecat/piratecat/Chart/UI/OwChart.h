/*****************************************************************************\
*                                                                             *
* OwChart.h -  Chart functions, types, and definitions.                       *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

#ifndef __OWCHART_H__
#define __OWCHART_H__
#pragma once
#include "AIChat.h"
#include "BarrageDiv.h"
#include "ChatWindow.h"
#include "FloatDiv.h"
#include "GdiPlusPaintEx.h"
#include "GdiPaintEx.h"
#include "IndexDiv.h"
#include "IndicatorList.h"
#include "IndicatorLayoutList.h"
#include "LatestDiv.h"
#include "LoginWindow.h"
#include "MacroList.h"
#include "SearchDiv.h"
#include "SecurityFilterList.h"
#include "UserSecurityList.h"
#include "UserSecurityListT2.h"
#include "SandBoxWindow.h"
#include "ParametersWindow.h"
#include "NewsList.h"
#include "..\\..\\GannSquare\\GannSquare.h"
#include "..\\..\\GannSquare\\GSEnums.h"
#include "..\\..\\GannSquare\\SquareCell.h"
#include "..\\Service\\SecurityDataHelper.h"
#include "..\\Service\\UserSecurityServiceEx.h"
#include "..\\Service\\UserSessionServiceEx.h"
#include "..\\..\\EatSnake\\Battle.h"
#include "..\\..\\EatSnake\\Play.h"

using namespace OwLib;

class FloatDiv;
class IndexDiv;
class LatestDiv;
class SearchDiv;
class UserSecurityServiceEx;
class UserSessionServiceEx;

class OwChart : public UIXmlEx
{
private:
	String m_addingPlotType;
	BarShape *m_bar;
	BarShape *m_barForecast;
	BarrageDiv *m_barrageDiv;
	CandleShape *m_candle;
	double m_candleHScalePixel;
	ChartA *m_chart;
	CDiv *m_candleDiv;
	CDiv *m_currentDiv;
	int m_cycle;
	int m_digit;
	vector<CDiv*> m_divs;
	FloatDiv *m_float;
	int m_forecastTick;
	WinformControlHost *m_host;
	vector<double> m_hScaleSteps;
	IndexDiv *m_indexDiv;
	CDiv *m_indDiv;
	int m_index;
	IndicatorLayoutServiceEx *m_indicatorLayoutService;
	vector<CIndicator*> m_indicators;
	IndicatorServiceEx *m_indicatorService;
	MenuA *m_indRightMenu;
	LatestDiv *m_latestDiv;
	String m_layoutID;
	PolylineShape *m_minuteAvgLine;
	PolylineShape *m_minuteLine;
	NativeBase *m_native;
	MenuA *m_plotRightMenu;
	QuoteService *m_quoteService;
	bool m_reverseVScale;
	MenuA *m_rightMenu;
	double m_scaleFactor;
	SecurityServiceEx *m_securityService;
	bool m_showMinuteLine;
	int m_subscription;
	SearchDiv *m_searchDiv;
	UserSecurityServiceEx *m_userSecurityService;
	ListenerMessageCallBack m_userSessionDataCallBack;
	UserSessionServiceEx *m_userSessionService;
	int m_vfRequestID;
	CDiv *m_volumeDiv;
public:
	static void ChartInvoke(void *sender, void *args, void *pInvoke);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void CloseToolWindow(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);	
	static void __stdcall IndicatorDataCallBack(CMessage *message, void *pInvoke);
	static void __stdcall QuoteDataCallBack(CMessage *message, void *pInvoke);
	static void MenuItemClick(void *sender, MenuItemA *item, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	void RegisterEvents(ControlA *control);
	static void RightMenuVisibleChanged(void *sender, void *pInvoke);
	static void __stdcall UserSessionDataCallBack(CMessage *message, void *pInvoke);
public:
	OwChart();
	virtual ~OwChart();
	ChartA* GetChart();
	BarrageDiv* GetBarrageDiv();
	int GetCycle();
	void SetCycle(int cycle);
	int GetDigit();
	void SetDigit(int digit);
	int GetIndex();
	vector<CIndicator*> GetIndicators();
	SecurityLatestData GetLatestData();
	bool ShowMinuteLine();
	void SetShowMinuteLine(bool showMinuteLine);
	double GetScaleFactor();
	void SetScaleFactor(double scaleFactor);
	int GetSubscription();
	void SetSubscription(int subscription);
public:
	void AddBlankDiv();
	CIndicator* AddMainIndicator(const String& name, const String& title, const String& script, const String& parameters, CDiv *div, bool update);
	void AddUserSecurity(const String& categoryID, const String& code);
	CIndicator* AddViceIndicator(const String& name, const String& script, const String& parameters, CDiv *div, bool update);
	void BindHistoryData(HistoryDataInfo *dataInfo, vector<SecurityData> *historyDatas);
	void ChangeCycle(int cycle);
	void DeleteIndicators(bool update);
	void DeleteSelectedIndicator();
	void DeleteSelectedPlot();
	void Exit();
	String GetIndicatorLayout();
	CIndicator* GetSelectedIndicator();
	void InitInterface();
	bool IsWindowShowing();
	void Load(String xmlPath);
	virtual void LoadData();
	void OnChartInvoke(void *args);
	void OnIndicatorDataCallBack(CMessage *message);
	void OnQuoteDataCallBack(CMessage *message);
	void OnRightMenuVisibleChanged();
	void OnUserSessionDataCallBack(CMessage *message);
	void RefreshData();
	void RemoveBlankDivs(bool update);
	void ResetScaleSize(SIZE clientSize);
	void SearchSecurity(Security *security);
	void SetIndicatorLayout(const String& layoutID, const String& str);
	void SelectMenu(MenuItemA *item, const String& tag);
	void SetIndicatorParameters();
    void ShowChatWindow();
    void ShowIndicatorLayoutList();
    void ShowIndicatorList();
	void ShowLoginWindow();
	int ShowMessageBox(const String& text, const String& caption, UINT uType);
    void ShowSandBoxWindow();
	void ShowSearchDiv(char key);
    void ShowSecurityFilterList();
    void ShowUserSecurityList();
	int WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
};
#endif