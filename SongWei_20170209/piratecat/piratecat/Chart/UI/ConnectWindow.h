/*****************************************************************************\
*                                                                             *
* ConnectWindow.h - Connect Setting window functions, types, and definitions  *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/8/18.                                    *
*                                                                             *
******************************************************************************/

#ifndef __CONNECTWINDOW_H__
#define __CONNECTWINDOW_H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "UIXmlEx.h"
#include "WindowEx.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\ConnectService.h"
#include "..\\Service\\ProxyService.h"
#include "..\\Service\\ServerService.h"
using namespace OwLibCT;

class ConnectService;
class ProxyService;

//Á¬½Ó´°Ìå
class ConnectWindow : public WindowXmlEx
{
private:
	ComboBoxA *m_cbMainServerIP;
	ComboBoxA *m_cbType;
	OwChart *m_chart;
	ConnectService *m_connectService;
	ControlEvent m_gridSelectedRowsChangedEvent;
	GridA *m_gridServers;
	NativeBase *m_native;
	ProxyService *m_proxyService;
	ServerService *m_serverService;
	SpinA *m_spinMainServerPort;
	SpinA *m_spinPort;
	TextBoxA *m_txtIP;
	WindowEx *m_window;
private:
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void ComboBoxSelectedIndexChanged(void *sender, void *pInvoke);
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	void RegisterEvents(ControlA *control);
public:
	ConnectWindow(NativeBase *native);
	virtual ~ConnectWindow();
	OwChart *GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	void AddServer();
	void AddServersToGrid(vector<ServerInfo> *serverInfos);
	void BindServersToComboBox();
	void BindServerTypeComboBox();
	virtual void Close();
	void DeleteServer();
	virtual void LoadData();
	void LoadProxyInfo();
	void OnComboBoxSelectedIndexChanged(ComboBoxA *comboBox);
	void OnGridSelectedRowsChanged();
	void OnInvoke(void *args);
	void SaveData();
	virtual void Show();
	void UpdateServer();
};
#endif