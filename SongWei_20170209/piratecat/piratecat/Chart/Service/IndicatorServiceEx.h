/*****************************************************************************\
*                                                                             *
* IndicatorServiceEx.h - Indicator service functions, types, and definitions. *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/20.                                    *
*                                                                             *
******************************************************************************/

#ifndef __INDICATORSERVICEEX_H__
#define __INDICATORSERVICEEX_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\IndicatorService.h"

class IndicatorServiceEx:public IndicatorService
{
private:
	int m_getListRequestID;
	vector<Indicator> m_indicators;
	bool m_loaded;
	int m_operatorRequestID;
public:
	IndicatorServiceEx();
	virtual ~IndicatorServiceEx();
	int GetListRequestID();
	bool IsLoaded();
	int GetOperatorRequestID();
public:
	bool GetIndicatorByID(const String& indicatorID, Indicator *indicator);
	bool GetIndicatorByName(const String& name, Indicator *indicator);
	bool GetIndicators(vector<Indicator> *indicators);
	virtual void OnReceive(CMessage *message);
};
#endif