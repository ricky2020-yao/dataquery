/*****************************************************************************\
*                                                                             *
* QuoteService.h -  Quote service functions, types, and definitions           *
*                                                                             *
*               Version 1.00 бя                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by Todd.                                              *
*                                                                             *
*******************************************************************************/

#ifndef __QUOTESERVICE_H__
#define __QUOTESERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_QUOTE 0
#define FUNCTIONID_QUOTE_PUSHLATESTDATA 0
#define FUNCTIONID_QUOTE_PUSHHISTORYDATA 1
#define FUNCTIONID_QUOTE_STOPPUSHHISTORYDATA 3
#define FUNCTIONID_QUOTE_STOPPUSHLATESTDATA 4
#define FUNCTIONID_QUOTE_GETLATESTDATA 5
#define FUNCTIONID_QUOTE_GETHISTORYDATA 6
#define FUNCTIONID_QUOTE_PUSHTRANSACTIONDATA 7
#define FUNCTIONID_QUOTE_GETVOLUMEFORECAST 8
#define FUNCTIONID_QUOTE_PUSHLATESTDATALV2 9

namespace OwLibCT
{
	class QuoteService : public BaseService
	{
	private:
		int m_socketID;
	public:
		QuoteService();
		virtual ~QuoteService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int GetHistoryDatas(int requestID, HistoryDataInfo *dataInfo);
		static int GetHistoryDatas(HistoryDataInfo *dataInfo, vector<SecurityData> *datas, const char *body, int bodyLength);
		static int GetLatestDatas(LatestDataInfo *dataInfo, vector<SecurityLatestData> *datas, const char *body, int bodyLength);
		static int GetLatestDatasLV2(LatestDataInfoLV2 *dataInfo, vector<SecurityLatestDataLV2> *datas, const char *body, int bodyLength);
		int GetLatestDatas(int requestID, LatestDataInfo *dataInfo);
		static int GetTransactionDatas(String *securityCode, vector<TransactionData> *transactionDatas, const char *body, int bodyLength);
		int GetVolumeForecast(int requestID, String code);
		static int GetVolumeForecastData(vector<VolumeForecastData> *datas, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
		int PushHistoryDatas(int requestID, HistoryDataInfo *dataInfo);
		int PushLatestDatas(int requestID, LatestDataInfo *dataInfo);
		int Send(int functionID, int requestID, int socketID, HistoryDataInfo *dataInfo);
		int Send(int functionID, int requestID, int socketID, LatestDataInfo* dataInfo);
		int StopPushHistoryDatas(int requestID);
		int StopPushLatestDatas(int requestID);
	};
}

#endif