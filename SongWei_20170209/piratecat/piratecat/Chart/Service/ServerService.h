/****************************************************************************************\
*                                                                                        *
* ServerService.h -  Server service functions, types, and definitions                    *
*                                                                                        *
*               Version 1.00 ��                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Lord 2016/8/18                                                *
*                                                                                        *
*****************************************************************************************/

#ifndef __SERVERSERVICE_H__
#define __SERVERSERVICE_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\BaseService.h"

//��������Ϣ
class ServerInfo
{
public:
	ServerInfo();
	String m_id; //�û�ID
	String m_ip; //IP��ַ
	int m_port; //�˿ں�
	String m_type; //����
};

//����������
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
	//��ӷ�����
	int AddServer(ServerInfo *serverInfo);
	//������
	void CreateTable();
	//����Ĭ�ϵķ�����
	void CreateDefaultServers();
	//ɾ��������
	int DeleteServer(const String& id);
	//��ȡ�������б�
	int GetServers(vector<ServerInfo> *serverInfos);
	//���·�����
	void UpdateServer(ServerInfo *serverInfo);
};
#endif