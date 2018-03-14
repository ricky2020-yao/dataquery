/*******************************************************************************\
*                                                                               *
* NewsList.h - News list functions, types, and definitions.                     *
*                                                                               *
*               Version 1.00  ¡ï¡ï¡ï                                            *
*                                                                               *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.        *
*               Created by Lord 2016/5/12.                                      *
*                                                                               *
********************************************************************************/

#ifndef __NEWSLIST__H__
#define __NEWSLIST__H__
#pragma once
#include "CDraw.h"
#include "OwChart.h"
#include "UIXmlEx.h"
#include "WindowEx.h"
#include "..\\..\\..\\includes\\Sock\\NewsService.h"

class NewsList;

class NewsCell : public LinkLabelA
{
private:
	int m_companyType;
	int m_index;
	NewsList *m_newsList;
public:
	NewsCell();
	virtual ~NewsCell();
	int GetCompanyType();
	void SetCompanyType(int companyType);
	int GetIndex();
	void SetIndex(int index);
	NewsList* GetNewsList();
	void SetNewsList(NewsList *newsList);
public:
	virtual void OnAdd();
	virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaint(CPaint *paint, const RECT& clipRect);
};

class NewsList : public DivA
{
private:
	vector<NewsCell*> m_cells;
	int m_filterType;
	ListenerMessageCallBack m_newsDataCallBack;
	NewsService *m_newsService;
	int m_requestID;
	String m_securityCode;
private:
	static void __stdcall NewsDataCallBack(CMessage *message, void *pInvoke);
public:
	NewsList();
	virtual ~NewsList();
	int GetFilterType();
	void SetFilterType(int filterType);
	String GetSecurityCode();
	void SetSecurityCode(const String& securityCode);
public:
	void ClearNews();
	void FilterNews();
	virtual void OnAdd();
	void OnCellMouseWheel(NewsCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	void OnClickCell(NewsCell *cell, const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnInvoke(void *args);
	void OnNewsDataCallBack(CMessage *message);
	virtual void Update();
};
#endif