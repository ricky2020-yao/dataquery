/****************************************************************************************\
*                                                                                        *
* SecurityFilterService.h -  Security filter service functions, types, and definitions   *
*                                                                                        *
*               Version 1.00 бя                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Lord.                                                         *
*                                                                                        *
*****************************************************************************************/

#ifndef __USERSECURITYSERVICE_H__
#define __USERSECURITYSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_USERSECURITY 1
#define FUNCTIONID_USERSECURITY_ADDCATEGORIES 0
#define FUNCTIONID_USERSECURITY_DELETECATEGORIES 1
#define FUNCTIONID_USERSECURITY_GETCATEGORIES 2
#define FUNCTIONID_USERSECURITY_UPDATECATEGORIES 3
#define FUNCTIONID_USERSECURITY_ADDSECURITIES 4
#define FUNCTIONID_USERSECURITY_DELETESECURITIES 5
#define FUNCTIONID_USERSECURITY_UPDTAEORDERNUM 6
#define FUNCTIONID_USERSECURITY_GETVISITSCOUNT 7
#define FUNCTIONID_USERSECURITY_ADDVISITSCOUNT 8
#define FUNCTIONID_USERSECURITY_REMOVEVISITSCOUNT 9

namespace OwLibCT
{
	class UserSecurityService : public BaseService
	{
	private:
		int m_socketID;
	public:
		UserSecurityService();
		virtual ~UserSecurityService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int AddCategory(int requestID, UserSecurityCategory *category);
		int AddSecurities(int requestID, UserSecurityCategory *category);
		int AddVisitsCount(int requestID, UserSecurityVisitsCount *visitsCount); 
		int DeleteCategory(int requestID, UserSecurityCategory *category);
		int DeleteSecurities(int requestID, UserSecurityCategory *category);
		int GetCategories(int requestID, UserSecurityCategory *category);
		static int GetCategories(vector<UserSecurityCategory> *categories, const char *body, int bodyLength);
		int GetVisitsCount(int requestID, UserSecurityVisitsCount *visitsCount); 
		virtual void OnReceive(CMessage *message);
		int RemoveVisitsCount(int requestID, UserSecurityVisitsCount *visitsCount); 
		int Send(int function, int requestID, vector<UserSecurityCategory> *categories);
		int SendVisitsCount(int function, int requestID, UserSecurityVisitsCount *visitsCount); 
		int UpdateCategory(int requestID, UserSecurityCategory *category);
		int UpdateOrderNum(int requestID, int userID, vector<String> *ids);
	};
}

#endif