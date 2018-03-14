/*****************************************************************************\
*                                                                             *
* DataCenter.h -  Data center functions, types, and definitions.              *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/10.                                    *
*                                                                             *
******************************************************************************/

#ifndef __DATACENTER_H__
#define __DATACENTER_H__
#pragma once
#include <atlstr.h>  
#import <msxml3.dll>
#include "ChatServiceEx.h"
#include "ConnectService.h"
#include "IndicatorServiceEx.h"
#include "IndicatorLayoutServiceEx.h"
#include "MacroServiceEx.h"
#include "ProxyService.h"
#include "SecurityFilterServiceEx.h"
#include "SecurityServiceEx.h"
#include "ServerService.h"
#include "UserCookieService.h"
#include "UserSecurityServiceEx.h"
#include "UserSessionServiceEx.h"
#include "..\\UI\\UIXmlEx.h"
#include "..\\..\\..\\includes\\Sock\\AIService.h"
#include "..\\..\\..\\includes\\Sock\\NewsService.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"
#include "..\\..\\..\\includes\\Sock\\BaseService.h"
#include "..\\..\\..\\includes\\Sock\\ChatService.h"
#include "..\\..\\..\\includes\\Sock\\LoginService.h"
#include "..\\..\\..\\includes\\Sock\\MacroService.h"
#include "..\\..\\..\\includes\\Sock\\QuoteService.h"

#define HRCALL(a, errmsg) \
do { \
    hr = (a); \
    if (FAILED(hr)) { \
        dprintf( "%s:%d  HRCALL Failed: %s\n  0x%.8x = %s\n", \
                __FILE__, __LINE__, errmsg, hr, #a ); \
        goto clean; \
    } \
} while (0)

class ConnectService;
class MacroServiceEx;
class ProxyService;
class SecurityFilterServiceEx;
class ServerService;
class UserCookieService;
class UserSessionServiceEx;

class DataCenter
{
private:
	static void dprintf(char *format, ...);
	static IXMLDOMDocument* DomFromCOM();
public:
	DataCenter();
	virtual ~DataCenter();
	static string GetAppPath();
	static string GetUserPath();
	static map<String, String> GetPlots();
	static AIService* GetAIService();
	static ChatServiceEx* GetChatService();
	static ConnectService* GetConnectService();
	static int GetConnectState();
	static IndicatorLayoutServiceEx* GetIndicatorLayoutService();
	static IndicatorServiceEx* GetIndicatorService();
	static LoginService* GetLoginService();
	static MacroServiceEx* GetMacroService();
	static NewsService* GetNewsService();
	static ProxyService* GetProxyService();
	static QuoteService* GetQuoteService();
	static SecurityFilterServiceEx* GetSecurityFilterService();
	static SecurityServiceEx* GetSecurityService();
	static ServerService* GetServerService();
	static int GetUserID();
	static UserCookieService* GetUserCookieService();
	static UserSecurityServiceEx* GetUserSecurityService();
	static UserSessionServiceEx* GetUserSessionService();
	static void SetMainUI(UIXmlEx *mainUI);
public:
	static int Connect();
	static int DisConnect();
	static int LoadData(int state);
	static void SaveLoginInfo(LoginInfo* loginInfo);
	static void StartService();
};
#endif