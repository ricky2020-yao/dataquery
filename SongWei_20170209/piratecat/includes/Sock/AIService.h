/*****************************************************************************\
*                                                                             *
* AIService.h -  AI service functions, types, and definitions            *
*                                                                             *
*               Version 1.00 бя                                             *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by QiChunyou.                                         *
*                                                                             *
*******************************************************************************/

#ifndef __AISERVICE__H__
#define __AISERVICE__H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_AI 12
#define FUNCTIONID_AI_ASK 0

namespace OwLibCT
{
	class AIChatData
	{
	public:
		AIChatData();
		String m_text;
		_int64 m_time;
		int m_userID;
	};

	class AIService : public BaseService
	{
	private:
		int m_socketID;
	public:
		AIService();
		virtual ~AIService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int Ask(int requestID, vector<AIChatData> *datas);
		static int GetDatas(vector<AIChatData> *datas, const char* body, int bodyLength);
		virtual void OnReceive(CMessage* message);
		int Send(int functionID, int requestID, vector<AIChatData> *datas);

	};
}

#endif