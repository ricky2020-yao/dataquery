/*****************************************************************************\
*                                                                             *
* SecurityServiceEx.h - Security service functions, types, and definitions.   *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by QiChunyou 2016/6/6.                                *
*                                                                             *
******************************************************************************/

#ifndef __SECURITYSERVICEEX_H__
#define __SECURITYSERVICEEX_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\SecurityService.h"
#include "..\\..\\..\\includes\\Sock\\Security.h"

class SecurityOrder
{
public:
	SecurityOrder();
	~SecurityOrder();
	int m_order;
	Security* m_security;
};

class SecurityServiceEx:public SecurityService
{
private:
	int m_getListRequestID;
	ControlInvokeEvent m_invokeEvent;
	bool m_loaded;
	map<String, Security*> m_securitiesMap;
	CList<CList<Security*>*> *m_securities;
	vector<Security> m_securitiesCache;
public:
	SecurityServiceEx();
	virtual ~SecurityServiceEx();
	int GetListRequestID();
	bool IsLoaded();
public:
	bool CompareMakeupSecutitycode(const String& code, const String& makeupCode);
	int Find(const String& key, int limitSize, CList<Security*>* securities);
	bool FilterSecurities(const String& key, CList<Security*>* securities);
	bool GetSecurityByCode(const String& code, Security *security);
	bool GetSecuritiesByCodes(const String& codes, vector<Security> *securities);
	int IsMatching(const String& securityVal, int securityType, const String& input, int type, int orderIndex);
	void Load();
	void LoadSecurities();
	bool MakeupSecurity(const String& code, const String& input, int type);
	virtual void OnReceive(CMessage *message);
	int StrStr(const String& str1, const String& str2);
};
#endif