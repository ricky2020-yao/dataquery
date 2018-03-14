/****************************************************************************************\
*                                                                                        *
* ServerService.h -  Server service functions, types, and definitions                    *
*                                                                                        *
*               Version 1.00 ★                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Lord 2016/8/18                                                *
*                                                                                        *
*****************************************************************************************/

#ifndef __SERVERSERVICE_H__
#define __SERVERSERVICE_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\BaseService.h"

//服务器信息
class ServerInfo
{
public:
	ServerInfo();
	String m_id; //用户ID
	String m_ip; //IP地址
	int m_port; //端口号
	String m_type; //类型
};

//服务器服务
class ServerService
{
private:
	string m_createTableSQL;
	String m_dataBasePath;
private:
	CLockEx m_lock;
public:
	ServerService();
	virtual ~ServerService();
public:
	//添加服务器
	int AddServer(ServerInfo *serverInfo);
	//创建表
	void CreateTable();
	//创建默认的服务器
	void CreateDefaultServers();
	//删除服务器
	int DeleteServer(const String& id);
	//获取服务器列表
	int GetServers(vector<ServerInfo> *serverInfos);
	//更新服务器
	void UpdateServer(ServerInfo *serverInfo);
};
#endif