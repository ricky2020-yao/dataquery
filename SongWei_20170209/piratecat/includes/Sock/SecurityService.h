/******************************************************************************\
*                                                                              *
* SecurityService.h -  Security service functions, types, and definitions      *
*                                                                              *
*               Version 1.00 бя                                                *
*                                                                              *
*               Copyright (c) 2016-2016, Client. All rights reserved.          *
*               Created by Todd.                                               *
*                                                                              *
*******************************************************************************/

#ifndef __SECURITYSERVICE_H__
#define __SECURITYSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_SECURITY 3
#define FUNCTIONID_SECURITY_GETSECURITIES 0
#define FUNCTIONID_SECURITY_GETSECURITIESBYID 1

namespace OwLibCT
{
	class SecurityService : public BaseService
	{
	private:
		int m_socketID;
	public:
		SecurityService();
		virtual ~SecurityService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int GetSecurities(int requestID);
		int GetSecuritiesByBlockID(int requestID, String blockID);
		static int GetSecurities(vector<Security> *securities, int functionID, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
	};
}

#endif