/*****************************************************************************\
*                                                                             *
* SecurityDataHelper.h - Security data functions, types, and definitions.     *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/30.                                    *
*                                                                             *
******************************************************************************/

#ifndef __SECURITYDATAHELPER_H__
#define __SECURITYDATAHELPER_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\..\\..\\includes\\Sock\\QuoteService.h"

class SecurityDataHelper
{
public:
	SecurityDataHelper();
	virtual ~SecurityDataHelper();
public:
	static CTable* CreateDataSource(ChartA *chart);
	static CIndicator* CreateIndicator(ChartA *chart, CTable *dataSource, const String& text, const String& parameters);
	static void BindHistoryDatas(ChartA *chart, CTable *dataSource, vector<CIndicator*> *indicators, int *fields, vector<SecurityData> *historyDatas);
	static int InsertData(ChartA *chart, CTable *dataSource, int *fields, SecurityData *securityData);
	static int InsertLatestData(ChartA *chart, CTable *dataSource, vector<CIndicator*> *indicators, int *fields, SecurityData *latestData);
};

#endif