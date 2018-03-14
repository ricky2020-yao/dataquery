/***********************************************************************************\
*                                                                                   *
* MacroList.h - Macro list functions, types, and definitions.                       *
*                                                                                   *
*               Version 1.00  ¡ï¡ï¡ï                                                *
*                                                                                   *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.            *
*               Created by Lord 2016/10/7.                                          *
*                                                                                   *
************************************************************************************/

#ifndef __MACROLIST_H__
#define __MACROLIST_H__
#pragma once
#include "UIXmlEx.h"
#include "CDraw.h"
#include "OwChart.h"
#include "WindowEx.h"
#include "..\\Service\\DataCenter.h"
#include "..\\Service\\MacroServiceEx.h"

class OwChart;
class MacroServiceEx;

class GridIconCell : public GridStringCell
{
private:
	String m_id;
	bool m_keyPress;
public:
	GridIconCell();
	virtual ~GridIconCell();
	String GetID();
	void SetID(String id);
	bool IsKeyPress();
	void SetKeyPress(bool keyPress);
public:
	virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
};

class MacroList : public WindowXmlEx
{
private:
	OwChart *m_chart;
	DivA *m_divIcon;
	GridA *m_gridIcons;
	GridA *m_gridMacros;
	GridCellMouseEvent m_gridCellClickEvent;
	ControlEvent m_gridSelectedRowsChangedEvent;
	Macro m_macro;
	ListenerMessageCallBack m_macroDataCallBack;
	MacroServiceEx *m_macroService;
	ControlInvokeEvent m_invokeEvent;
	NativeBase *m_native;
	WindowEx *m_window;
private:
	void AddMacrosToGrid(vector<Macro> *macros);
	static void ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	map<String, GridRow*> GetMacroRows();
	static void GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void GridSelectedRowsChanged(void *sender, void *pInvoke);
	static void Invoke(void *sender, void *args, void *pInvoke);
	void LoadIcons();
	void LoadMacro(Macro *macro);
	static void __stdcall MacroDataCallBack(CMessage *message, void *pInvoke);
	void RegisterEvents(ControlA *control);
public:
	MacroList(NativeBase *native);
	virtual ~MacroList();
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	WindowEx* GetWindow();
public:
	virtual void Close();
	void Create();
	void Delete();
	void GetMacro(Macro *macro);
	void OnGridCellClick(GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	void OnGridSelectedRowsChanged();
	void OnInvoke(void *args);
	void OnMacroDataCallBack(CMessage *message);
	void Save();
	virtual void Show();
	void Test();
};

#endif