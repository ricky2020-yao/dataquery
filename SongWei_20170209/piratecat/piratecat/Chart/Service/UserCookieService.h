/**************************************************************************************\
*                                                                                      *
* UserCookieService.h -  User coolie service functions, types, and definitions.        *
*                                                                                      *
*               Version 1.00 бя                                                        *
*                                                                                      *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.               *
*               Created by WangShaoxu.                                                 *
*                                                                                      *
***************************************************************************************/

#ifndef __USERCOOKIESERVICE_H__
#define __USERCOOKIESERVICE_H__
#pragma once
#include "DataCenter.h"

class UserCookie
{
public:
	UserCookie();
	String m_key; 
	int m_userID; 
	String m_value; 
};

class UserCookieService
{
private:
	string m_createTableSQL;
	String m_dataBasePath;
	int m_userID;
public:
	UserCookieService();
	virtual ~UserCookieService();
	int GetUserID();
	void SetUserID(int userID);
public:
	int AddCookie(UserCookie *cookie);
	void CreateTable();
	int DeleteCookie(String key);
	int GetCookie(String key, UserCookie *cookie);
	void UpdateCookie(UserCookie *cookie);
};

#endif