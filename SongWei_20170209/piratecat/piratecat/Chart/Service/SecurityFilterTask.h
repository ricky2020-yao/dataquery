/**********************************************************************************\
*                                                                                  *
* SecurityFilterTask.h - Security filter task functions, types, and definitions.   *
*                                                                                  *
*               Version 1.00  ¡ï¡ï¡ï                                               *
*                                                                                  *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.           *
*               Created by Lord 2016/2/28.                                         *
*                                                                                  *
***********************************************************************************/

#ifndef __SECURITYFILTERTASK_H__
#define __SECURITYFILTERTASK_H__
#pragma once
#include "DataCenter.h"
#include "SecurityDataHelper.h"
#include "SecurityFilterServiceEx.h"
#include "..\\..\\..\\includes\\Sock\\QuoteService.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"

class SecurityFilterServiceEx;

class SecurityFilterInfo
{
private:
	bool m_completed;
	CTable *m_dataSource;
	SecurityLatestData m_latestData;
	Security m_security;
	map<String, double> m_values;
public:
	SecurityFilterInfo();
	virtual ~SecurityFilterInfo();
	bool IsCompleted();
	void SetCompleted(bool completed);
	CTable* GetDataSource();
	void SetDataSource(CTable *dataSource);
	SecurityLatestData GetLatestData();
	void SetLatestData(SecurityLatestData latestData);
	Security GetSecurity();
	void SetSecurity(Security security);
public:
	double GetValue(const String& key);
	void SetValue(const String& key, double value);
};

class SecurityFilterTask
{
private:
	ChartA *m_chart;
	CTable *m_dataSource;
	CIndicator *m_indicator;
	ListenerMessageCallBack m_lastestDataCallBack;
	map<String, SecurityFilterInfo*> m_infos;
	int m_matchCount;
	NativeBase *m_native;
	QuoteService *m_quoteService;
	int m_qtRequestID;
	int m_runningState;
	int m_sfRequestID;
	ListenerMessageCallBack m_securityFilterDataCallBack;
	SecurityFilterServiceEx *m_securityFilterService;
	SecurityFilterTemplate m_template;
private:
	static void __stdcall LatestDataCallBack(CMessage *message, void *pInvoke);
	static void __stdcall SecurityFilterDataCallBack(CMessage *message, void *pInvoke);
public:
	SecurityFilterTask();
	virtual ~SecurityFilterTask();
	CTable* GetDataSource();
	void SetDataSource(CTable *dataSource);
	CIndicator* GetIndicator();
	void SetIndicator(CIndicator *indicator);
	int GetMatchCount();
	void SetMatchCount(int matchCount);
	NativeBase* GetNative();
	void SetNative(NativeBase *native);
	int GetRunningState();
	void SetRunningState(int runningState);
	SecurityFilterTemplate GetTemplate();
	void SetTemplate(SecurityFilterTemplate sfTemplate);
public:
	void GetSecurityInfos(vector<SecurityFilterInfo*> *infos);
	int GetSecurityInfosSize();
	void OnLatestDataCallBack(CMessage *message);
	void OnSecurityFilterDataCallBack(CMessage *message);
	void Run();
	void Stop();
};
#endif