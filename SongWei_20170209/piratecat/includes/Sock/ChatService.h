/*****************************************************************************\
*                                                                             *
* ChatService.h -  Login service functions, types, and definitions            *
*                                                                             *
*               Version 1.00 §Ò§ñ                                             *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by QiChunyou.                                         *
*                                                                             *
*******************************************************************************/

#ifndef __CHATSERVICE__H__
#define __CHATSERVICE__H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_CHAT 7
#define FUNCTIONID_CHAT_ENTER 0
#define FUNCTIONID_CHAT_EXIT 1
#define FUNCTIONID_CHAT_SEND 2
#define FUNCTIONID_CHAT_RECV 3

namespace OwLibCT
{
	class ChatService : public BaseService
	{
	private:
		int m_socketID;
	public:
		ChatService();
		virtual ~ChatService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int Enter(int requestID, ChatData* chat);
		int Exit(int requestID, ChatData* chat);
		static int GetChatDatas(vector<ChatData> *datas, const char* body, int bodyLength);
		virtual void OnReceive(CMessage* message);
		int Send(int requestID, ChatData* chat);
		int SendToAllClients(int functionID, int requestID, vector<ChatData> *datas);
	};
}

#endif