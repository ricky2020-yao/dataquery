/****************************************************************************************\
*                                                                                        *
* ConnectService.h -  Connect service functions, types, and definitions                  *
*                                                                                        *
*               Version 1.00 ��                                                          *
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

//������Ϣ
class ConnectInfo
{
public:
	ConnectInfo();
	//ID
	String m_id; 
	//IP��ַ
	String m_ip;
	//�˿ں�
	int m_port;
	//����
	String m_type;
};

//���ӷ���
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
	//�������
	int AddConnect(ConnectInfo *connectInfo);
	//������
	void CreateTable();
	//����Ĭ������
	void CreateDefaultConnects();
	//��ȡ����
	int GetConnects(vector<ConnectInfo> *connectInfos);
	//��������
	int UpdateConnect(ConnectInfo *connectInfo);
};
#endif
