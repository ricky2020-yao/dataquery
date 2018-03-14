/**********************************************************************************\
*                                                                                  *
* UserSessionServiceEx.h - User session service functions, types, and definitions. *
*                                                                                  *
*               Version 1.00  ¡ï¡ï¡ï                                               *
*                                                                                  *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.           *
*               Created by Lord 2016/1/20.                                         *
*                                                                                  *
***********************************************************************************/

#ifndef __USERSESSIONSERVICEEX_H__
#define __USERSESSIONSERVICEEX_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\..\\..\\includes\\Sock\\UserSessionService.h"
#include "DataCenter.h"
using namespace OwLibCT;

class UserSessionServiceEx : public UserSessionService
{
private:
	int m_getListRequestID;
	bool m_loaded;
	int m_operatorRequestID;
	map<String, UserSession> m_sessions;
private:
	CLockEx m_lock;
public:
	UserSessionServiceEx();
	virtual ~UserSessionServiceEx();
	int GetListRequestID();
	bool IsLoaded();
	int GetOperatorRequestID();
public:
	int GetSession(String sessionID, UserSession *session);
	virtual void OnReceive(CMessage *message);
	virtual void OnSessionChanged(UserSession *session);
};
#endif