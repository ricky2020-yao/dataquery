/*******************************************************************************************\
*                                                                                           *
* SecurityFilterServiceEx.h - Security filter service functions, types, and definitions.    *
*                                                                                           *
*               Version 1.00  ¡ï¡ï¡ï                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.                    *
*               Created by Lord 2016/1/22.                                                  *
*                                                                                           *
********************************************************************************************/

#ifndef __SECURITYFILTERSERVICEEX_H__
#define __SECURITYFILTERSERVICEEX_H__
#pragma once
#include "SecurityFilterTask.h"
#include "..\\..\\..\\includes\\Sock\\BaseService.h"
#include "..\\..\\..\\includes\\Sock\\SecurityFilterService.h"

class SecurityFilterTask;

class SecurityFilterServiceEx:public SecurityFilterService
{
private:
	int m_getListRequestID;
	bool m_loaded;
	int m_operatorRequestID;
	vector<SecurityFilterTask*> m_tasks;
	vector<SecurityFilterTemplate> m_templates;
public:
	SecurityFilterServiceEx();
	virtual ~SecurityFilterServiceEx();
	int GetListRequestID();
	bool IsLoaded();
	int GetOperatorRequestID();
public:
	SecurityFilterTask* AddTask(SecurityFilterTemplate sfTemplate, NativeBase *native);
	SecurityFilterTask* GetTask(const String& templateID);
	int GetTemplate(const String& templateID, SecurityFilterTemplate *sfTemplate);
	int GetTemplates(vector<SecurityFilterTemplate> *templates);
	virtual void OnReceive(CMessage *message);
	int RemoveTask(const String& templateID);
};
#endif