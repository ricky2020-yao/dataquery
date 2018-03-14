/*****************************************************************************\
*                                                                             *
* ChatServiceEx.h -  Chart service functions, types, and definitions.         *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/20.                                    *
*                                                                             *
******************************************************************************/

#ifndef __CHATSERVICEEX_H__
#define __CHATSERVICEEX_H__
#pragma once
#include "..\\..\\..\\includes\\Sock\\ChatService.h"

class ChatServiceEx : public ChatService
{
private:
	int m_requestID;
public:
	ChatServiceEx();
	virtual ~ChatServiceEx();
	int GetRequestID();
};
#endif