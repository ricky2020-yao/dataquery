/****************************************************************************************\
*                                                                                        *
* ProxyService.h -  Proxy service functions, types, and definitions                      *
*                                                                                        *
*               Version 1.00 бя                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Lord 2016/8/18                                                *
*                                                                                        *
*****************************************************************************************/

#ifndef __PROXYSERVICE_H__
#define __PROXYSERVICE_H__
#pragma once
#include "DataCenter.h"
#include "..\\..\\..\\includes\\Sock\\BaseService.h"

class ProxyInfo
{
public:
	ProxyInfo();
	String m_domain;
	String m_ip;
	String m_name;
	int m_port;
	String m_pwd;
	int m_type;
	bool m_use;
};

class ProxyService
{
private:
	string m_createTableSQL;
	String m_dataBasePath;
private:
	CLockEx m_lock;
public:
	ProxyService();
	virtual ~ProxyService();
public:
	int AddProxyInfo(ProxyInfo *proxyInfo);
	void CreateTable();
	void CreateDefaultProxyInfos();
	int GetProxyInfos(vector<ProxyInfo> *proxyInfos);
	int UpdateProxyInfo(ProxyInfo *proxyInfo);
};
#endif
