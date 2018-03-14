/*******************************************************************************************\
*                                                                                           *
* IndicatorLayoutServiceEx.h -  IndicatorLayout service functions, types, and definitions.  *
*                                                                                           *
*               Version 1.00  ¡ï¡ï¡ï                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/3/10.                                                  *
*                                                                                           *
********************************************************************************************/

#ifndef __INDICATORLAYOUTSERVICEEX_H__
#define __INDICATORLAYOUTSERVICEEX_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\IndicatorLayoutService.h"

class IndicatorLayoutServiceEx : public IndicatorLayoutService
{
private:
	int m_getListRequestID;
	vector<IndicatorLayout> m_layouts;
	bool m_loaded;
	int m_operatorRequestID;
public:
	IndicatorLayoutServiceEx();
	virtual ~IndicatorLayoutServiceEx();
	int GetListRequestID();
	bool IsLoaded();
	int GetOperatorRequestID();
public:
	bool GetLayoutByID(const String& layoutID, IndicatorLayout *layout);
	bool GetLayoutByName(const String& layoutName, IndicatorLayout *layout);
	bool GetLayouts(vector<IndicatorLayout> *layouts);
	virtual void OnReceive(CMessage *message);
};
#endif