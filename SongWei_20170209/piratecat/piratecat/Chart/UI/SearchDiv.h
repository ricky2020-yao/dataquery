/*****************************************************************************\
*                                                                             *
* SearchDiv.h - Search div functions, types, and definitions.                 *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/12.                                    *
*                                                                             *
******************************************************************************/

#ifndef __SEARCHDIV_H__
#define __SEARCHDIV_H__
#pragma once
#include "OwChart.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\Service\\DataCenter.h"

class OwChart;

class SearchDiv:public MenuA
{
private:
	String m_categoryID;
	OwChart *m_chart;
	GridA *m_grid;
	GridCellMouseEvent m_gridCellClickEvent;
	ControlKeyEvent m_gridKeyDownEvent;
	vector<Security*> m_securities;
	ControlEvent m_textBoxInputChangedEvent;
	ControlKeyEvent m_textBoxKeyDownEvent;
	static void GridCellClick(void *sender, GridCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
	static void GridKeyDown(void *sender, char key, void *pInvoke);
	static void TextBoxInput(void *sender, void *pInvoke);
	static void TextBoxKeyDown(void *sender, char key, void *pInvoke);
public:
	SearchDiv();
	virtual ~SearchDiv();
	String GetCategoryID();
	void SetCategoryID(const String& categoryID);
	OwChart* GetChart();
	void SetChart(OwChart *chart);
	TextBoxA *m_searchTextBox;
public:
	void FilterSearch();
	virtual void OnLoad();
	virtual void OnKeyDown(char key);
	void OnSelectRow();
	virtual void OnVisibleChanged();
};

#endif