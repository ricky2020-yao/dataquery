/************************************************************************************\
*                                                                                    *
* NewsService.cs -  News service functions, types, and definitions.                  *
*                                                                                    *
*               Version 1.00 бя                                                      *
*                                                                                    *
*               Copyright (c) 2016-2016, Server. All rights reserved.                *
*               Created by Todd.                                                     *
*                                                                                    *
*************************************************************************************/

#ifndef __NEWSSERVICE__H__
#define __NEWSSERVICE__H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_NEWS 13
#define FUNCTIONID_NEWS_GETNEWS 0

namespace OwLibCT
{
	class NewsRequestData
	{
	public:
		NewsRequestData();
		String m_code;
		int m_companyType;
		int m_maxSize;
		int m_type;
	};

	class NewsData
	{
	public:
		NewsData();
		String m_code;
		int m_companyType;
		String m_content;
		double m_time;
		String m_title;
		int m_type;
		String m_url;
	};

	class NewsService : public BaseService
	{
	private:
		int m_socketID;
	public:
		NewsService();
		virtual ~NewsService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int GetNews(int requestID, vector<NewsRequestData> *datas);
		static int GetNewsDatas(vector<NewsData> *datas, const char* body, int bodyLength);
		virtual void OnReceive(CMessage* message);
		int Send(int functionID, int requestID, vector<NewsRequestData> *datas);
	};
}

#endif