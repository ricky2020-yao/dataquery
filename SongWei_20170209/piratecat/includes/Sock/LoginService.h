/*****************************************************************************\
*                                                                             *
* LoginService.h -  Login service functions, types, and definitions           *
*                                                                             *
*               Version 1.00 бя                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by Todd.                                              *
*                                                                             *
*******************************************************************************/

#ifndef __LOGINSERVICE_H__
#define __LOGINSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"
#include "CFile.h"

#define SERVICEID_LOGIN 5
#define FUNCTIONID_LOGIN_LOGIN 0
#define FUNCTIONID_LOGIN_LOGOUT 1
#define FUNCTIONID_LOGIN_GETLOGININFOS 2

namespace OwLibCT
{
	class LoginService : public BaseService
	{
	private:
		int m_socketID;
	public:
		LoginService();
		virtual ~LoginService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int GetLoginInfos(int requestID);
		static int GetLoginInfos(vector<LoginInfo> *loginInfos, const char *body, int bodyLength);
		int Login(String userName, String passWord, int requestID);
		int Logout(int requestID);
		virtual void OnReceive(CMessage *message);
		int Send(int functionID, int requestID, vector<LoginInfo> *loginInfos);
	};
}

#endif