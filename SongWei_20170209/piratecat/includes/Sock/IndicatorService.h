/******************************************************************************\
*                                                                              *
* IndicatorService.h -  Indicator service functions, types, and definitions    *
*                                                                              *
*               Version 1.00 бя                                                *
*                                                                              *
*               Copyright (c) 2016-2016, Client. All rights reserved.          *
*               Created by Todd.                                               *
*                                                                              *
*******************************************************************************/

#ifndef __INDICATORSERVICE_H__
#define __INDICATORSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_INDICATOR 4
#define FUNCTIONID_INDICATOR_ADDINDICATORS 0
#define FUNCTIONID_INDICATOR_DELETEINDICATORS 1
#define FUNCTIONID_INDICATOR_GETINDICATORS 2
#define FUNCTIONID_INDICATOR_UPDATEINDICATORS 3
#define FUNCTIONID_INDICATOR_UPDATEORDERNUM 4

namespace OwLibCT
{
	class IndicatorService : public BaseService
	{
	private:
		int m_socketID;
	public:
		IndicatorService();
		virtual ~IndicatorService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int AddIndicator(int requestID, Indicator *indicator);
		int DeleteIndicator(int requestID, Indicator *indicator);
		int GetIndicators(int requestID, Indicator *indicator);
		static int GetIndicators(vector<Indicator> *indicators, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
		int Send(int functionID, int requestID, vector<Indicator> *indicators);
		int UpdateIndicator(int requestID, Indicator *indicator);
		int UpdateOrderNum(int requestID, int userID, vector<String> *ids);
	};
}

#endif