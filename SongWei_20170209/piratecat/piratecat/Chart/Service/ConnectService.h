/****************************************************************************************\
*                                                                                        *
* ConnectService.h -  Connect service functions, types, and definitions                  *
*                                                                                        *
*               Version 1.00 ★                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Lord 2016/8/18                                                *
*                                                                                        *
*****************************************************************************************/

#ifndef __CONNECTSERVER_H__
#define __CONNECTSERVER_H__
#pragma once
#include "DataCenter.h"
#include "..\\..\\..\\includes\\Sock\\BaseService.h"

//连接信息
class ConnectInfo
{
public:
	ConnectInfo();
	//ID
	String m_id; 
	//IP地址
	String m_ip;
	//端口号
	int m_port;
	//类型
	String m_type;
};

//连接服务
class ConnectService
{
private:
	string m_createTableSQL;
	String m_dataBasePath;
private:
	CLockEx m_lock;
public:
	ConnectService();
	virtual ~ConnectService();
public:
	//添加连接
	int AddConnect(ConnectInfo *connectInfo);
	//创建表
	void CreateTable();
	//创建默认连接
	void CreateDefaultConnects();
	//获取连接
	int GetConnects(vector<ConnectInfo> *connectInfos);
	//更新连接
	int UpdateConnect(ConnectInfo *connectInfo);
};
#endif
