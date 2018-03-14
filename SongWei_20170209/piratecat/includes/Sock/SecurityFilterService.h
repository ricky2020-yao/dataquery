/****************************************************************************************\
*                                                                                        *
* SecurityFilterService.h -  Security filter service functions, types, and definitions   *
*                                                                                        *
*               Version 1.00 бя                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Todd.                                                         *
*                                                                                        *
*****************************************************************************************/

#ifndef __SECURITYFILTERSERVICE_H__
#define __SECURITYFILTERSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_SECURITYFILTER 2
#define FUNCTIONID_SECURITYFILTER_ADDTEMPLATES 0
#define FUNCTIONID_SECURITYFILTER_DELETETEMPLATES 1
#define FUNCTIONID_SECURITYFILTER_GETTEMPLATES 2
#define FUNCTIONID_SECURITYFILTER_UPDATETEMPLATES 3
#define FUNCTIONID_SECURITYFILTER_STARTFILTER 4
#define FUNCTIONID_SECURITYFILTER_STOPFILTER 5
#define FUNCTIONID_SECURITYFILTER_UPDTAEORDERNUM 6

namespace OwLibCT
{
	class SecurityFilterService : public BaseService
	{
	private:
		int m_socketID;
	public:
		SecurityFilterService();
		virtual ~SecurityFilterService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int AddTemplate(int requestID, SecurityFilterTemplate *sfTemplate);
		int DeleteTemplate(int requestID, SecurityFilterTemplate *sfTemplate);
		static int GetFilterResults(map<String, double*> *datas, int *currentBatch, int *totalBatch, const char *body, int bodyLength);
		int GetTemplates(int requestID, SecurityFilterTemplate *sfTemplate);
		static int GetTemplates(vector<SecurityFilterTemplate> *templates, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
		int Send(int function, int requestID, vector<SecurityFilterTemplate> *templates);
		int StartFilter(int requestID, SecurityFilterTemplate *sfTemplate);
		int StopFilter(int requestID, SecurityFilterTemplate *sfTemplate);
		int UpdateOrderNum(int requestID, int userID, vector<String> *ids);
		int UpdateTemplate(int requestID, SecurityFilterTemplate *sfTemplate);
	};
}

#endif