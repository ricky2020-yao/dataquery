/************************************************************************************\
*                                                                                    *
* IndicatorLayoutService.h -  Kline layout service functions, types, and definitions     *
*                                                                                    *
*               Version 1.00 бя                                                      *
*                                                                                    *
*               Copyright (c) 2016-2016, Client. All rights reserved.                *
*               Created by Todd.                                                     *
*                                                                                    *
*************************************************************************************/

#ifndef __INDICATORLAYOUTSERVICE_H__
#define __INDICATORLAYOUTSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_INDICATORLAYOUT 6
#define FUNCTIONID_INDICATORLAYOUT_ADDLAYOUTS 0
#define FUNCTIONID_INDICATORLAYOUT_DELETELAYOUTS 1
#define FUNCTIONID_INDICATORLAYOUT_GETLAYOUTS 2
#define FUNCTIONID_INDICATORLAYOUT_UPDATELAYOUTS 3
#define FUNCTIONID_INDICATORLAYOUT_UPDATEORDERNUM 4

namespace OwLibCT
{
	class IndicatorLayoutService : public BaseService
	{
	private:
		int m_socketID;
	public:
		IndicatorLayoutService();
		virtual ~IndicatorLayoutService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int AddLayout(int requestID, IndicatorLayout *layout);
		int DeleteLayout(int requestID, IndicatorLayout *layout);
		int GetLayouts(int requestID, IndicatorLayout *layout);
		static int GetLayouts(vector<IndicatorLayout> *layouts, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
		int Send(int function, int requestID, vector<IndicatorLayout> *layouts);
		int UpdateLayout(int requestID, IndicatorLayout *layout);
		int UpdateOrderNum(int requestID, int userID, vector<String> *ids);
	};
}

#endif