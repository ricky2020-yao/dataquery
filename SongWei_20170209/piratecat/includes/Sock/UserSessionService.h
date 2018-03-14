/**************************************************************************************\
*                                                                                      *
* UserSessionService.h -  User session service functions, types, and definitions.      *
*                                                                                      *
*               Version 1.00 бя                                                        *
*                                                                                      *
*               Copyright (c) 2016-2016, Client. All rights reserved.                  *
*               Created by TangWen.                                                    *
*                                                                                      *
***************************************************************************************/

#ifndef __USERSESSIONSERVICE_H__
#define __USERSESSIONSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_USERSESSION 8
#define FUNCTIONID_USERSESSION_ADDSESSIONS 0
#define FUNCTIONID_USERSESSION_DELETESESSIONS 1
#define FUNCTIONID_USERSESSION_GETSESSIONS 2
#define FUNCTIONID_USERSESSION_UPDATESESSIONS 3

namespace OwLibCT
{
	class UserSessionService : public BaseService
	{
	private:
		int m_socketID;
	public:
		UserSessionService();
		virtual ~UserSessionService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int AddSession(int requestID, UserSession *session);
		int DeleteSession(int requestID, UserSession *session);
		int GetSessions(int requestID, UserSession *session);
		static int GetSessions(vector<UserSession> *sessions, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
		int Send(int functionID, int requestID, vector<UserSession> *sessions);
		int UpdateSession(int requestID, UserSession *session);
	};
}

#endif