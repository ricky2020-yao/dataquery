/*****************************************************************************\
*                                                                             *
* BaseService.h -  Base service functions, types, and definitions             *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï¡ï¡ï                                      *
*                                                                             *
*               Copyright (c) 2016-2016, Client. All rights reserved.         *
*               Created by Lord.                                              *
*                                                                             *
*******************************************************************************/

#ifndef __BASESERVICE_H__
#define __BASESERVICE_H__
#pragma once
#include "Binary.h"
#include <string>
#include <map>
using namespace std;

#define  DOWNHELPER_AGENTNAME         "SECURITY"  
#define  LEN_OF_BUFFER_FOR_QUERYINFO  128  
#define  MAX_DOWNLOAD_REQUEST_TIME    10
#define  COMPRESSTYPE_NONE			  0
#define  COMPRESSTYPE_GZIP			  1

static int m_requestID = 10000;

namespace OwLibCT
{
	struct CMessage
	{
	public:
		CMessage();
		CMessage(int groupID, int serviceID, int functionID, int sessionID, int requestID, int socketID, int state, int compressType, int bodyLength, char *body);
		int m_groupID;   
		int m_serviceID;   
		int m_functionID;  
		int m_sessionID;   
		int m_requestID;   
		int m_socketID; 
		int m_state;
		int m_compressType;
		int m_bodyLength;  
		char *m_body; 
		void Copy(CMessage *message)
		{
			m_groupID = message->m_groupID;
			m_serviceID = message->m_serviceID;
			m_functionID = message->m_functionID;
			m_sessionID = message->m_sessionID;
			m_requestID= message->m_requestID;
			m_socketID = message->m_socketID;
			m_state = message->m_state;
			m_compressType = message->m_compressType;
			m_bodyLength = message->m_bodyLength;
			m_body = message->m_body;
		}
	};

	typedef void (__stdcall *ListenerMessageCallBack)(CMessage *message, void *pInvoke);

	typedef void (__stdcall *MessageCallBack)(int socketID, int localSID, const char *str, int len);

	typedef void (__stdcall *WriteLogCallBack)(int socketID, int localSID, int state, const char *log);

	typedef int (*funcCloseClient)(int);

	typedef int (*funcConnectToServer)(int, const char*, int, const char*, int, const char*, const char*, const char*, int);

	typedef int (*funcRegisterCallBack)(MessageCallBack);

	typedef int (*funcRegisterLog)(WriteLogCallBack);

	typedef int (*funcSendByClient)(int, const char*, int);

	class MessageListener
	{
	private:
		vector<ListenerMessageCallBack> m_callBacks;
		vector<void*> m_callBackInvokes;
	public:
		MessageListener();
		virtual ~MessageListener();
	public:
		void Add(ListenerMessageCallBack callBack, void *pInvoke);
		void CallBack(CMessage *message);
		void Clear();
		void Remove(ListenerMessageCallBack callBack);
	};

	class CLockEx
	{
	public:
		CLockEx()
		{ 
			InitializeCriticalSection(&m_cs); 
		}
		virtual ~CLockEx()
		{
			DeleteCriticalSection(&m_cs); 
		}
		void Lock()
		{ 
			EnterCriticalSection(&m_cs);
		}
		void UnLock()
		{ 
			LeaveCriticalSection(&m_cs); 
		}
	private:
		CRITICAL_SECTION m_cs;
	};

	class BaseService
	{
	private:
		int m_compressType;
		int m_groupID;
		map<int, MessageListener*> m_listeners;
		int m_serviceID;
		int m_sessionID;
		map<int, CMessage*> m_waitMessages;
	private:
		CLockEx m_lock;
	public:
		static int CloseClient(int socketID);
		static int ConnectToServer(int proxyType, const char *ip, int port, const char *proxyIp, int proxyPort, const char *proxyUserName, const char *proxyUserPwd, const char *proxyDomain, int timeout);
		static void RegisterCallBack(MessageCallBack callBack);
		static void RegisterLog(WriteLogCallBack callBack);
		static int SendByClient(int socketID, const char *str, int len);
	public:
		BaseService();
		virtual ~BaseService();
		int GetCompressType();
		void SetCompressType(int compressType);
		static _int64 GetDownFlow();
		static void SetDownFlow(_int64 downFlow);
		int GetGroupID();
		void SetGroupID(int groupID);
		int GetServiceID();
		void SetServiceID(int serviceID);
		int GetSessionID();
		void SetSessionID(int sessionID);
		static _int64 GetUpFlow();
		static void SetUpFlow(_int64 upFlow);
	public:
		static void AddService(BaseService *service);
		static void __stdcall CallBack(int socketID, int localSID, const char *str, int len);
		static string GetHttpWebRequest(string strUrl, int bufferSize);
		static string GetProgramDir();
		static int GetRequestID();
		static void GetServices(vector<BaseService*> *services);
		virtual int KeepAlive(int socketID);
		virtual void OnCallBack(Binary *binary, int socketID, int localSID, int len);
		virtual void OnReceive(CMessage *message);
		virtual void OnWaitMessageHandle(CMessage *message);
		virtual void RegisterListener(int requestID, ListenerMessageCallBack callBack, void *pInvoke);
		virtual void RegisterWait(int requestID, CMessage *message);
		virtual int Send(CMessage *message);
		virtual void SendToListener(CMessage *message);
		static BOOL _TryHttpSendRequest(LPVOID hRequest, int nMaxTryTimes); 
		virtual void UnRegisterListener(int requestID);
		virtual void UnRegisterListener(int requestID, ListenerMessageCallBack callBack);
		virtual void UnRegisterWait(int requestID);
		virtual int WaitMessage(int requestID, int timeout);
	};
}
#endif