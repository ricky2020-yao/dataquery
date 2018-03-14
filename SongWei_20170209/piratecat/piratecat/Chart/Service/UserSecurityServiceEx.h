/*************************************************************************************\
*                                                                                     *
* UserSecurityServiceEx.h - User security service functions, types, and definitions.  *
*                                                                                     *
*               Version 1.00  ¡ï¡ï¡ï                                                  *
*                                                                                     *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.              *
*               Created by Lord 2016/1/12.                                            *
*                                                                                     *
**************************************************************************************/

#ifndef __USERSECURITYSERVICEEX_H__
#define __USERSECURITYSERVICEEX_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\UserSecurityService.h"
#include "DataCenter.h"

class UserSecurityServiceEx:public UserSecurityService
{
private:
	vector<UserSecurityCategory> m_categories;
	int m_getListRequestID;
	bool m_loaded;
	int m_operatorRequestID;
public:
	map<String, int> m_visitsCount;
	UserSecurityServiceEx();
	virtual ~UserSecurityServiceEx();
	int GetListRequestID();
	bool IsLoaded();
	int GetOperatorRequestID();
public:
	int AddVisitsCount(String code);
	bool GetCategory(const String& categoryID, UserSecurityCategory *category);
	bool GetCategories(vector<UserSecurityCategory> *categories);
	virtual void OnReceive(CMessage *message);
};
#endif