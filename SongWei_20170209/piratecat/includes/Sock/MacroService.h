/****************************************************************************************\
*                                                                                        *
* MacroService.h -  Macro service functions, types, and definitions.                     *
*                                                                                        *
*               Version 1.00 бя                                                          *
*                                                                                        *
*               Copyright (c) 2016-2016, Client. All rights reserved.                    *
*               Created by Todd.                                                         *
*                                                                                        *
*****************************************************************************************/

#ifndef __MACROSERVICE_H__
#define __MACROSERVICE_H__
#pragma once
#include "BaseService.h"
#include "Security.h"

#define SERVICEID_MACRO 9
#define FUNCTIONID_MACRO_ADDMACROS 0
#define FUNCTIONID_MACRO_DELETEMACROS 1
#define FUNCTIONID_MACRO_EXECUTEMACROS 2
#define FUNCTIONID_MACRO_GETMACROS 3
#define FUNCTIONID_MACRO_UPDATEMACROS 4
#define FUNCTIONID_MACRO_UPDATEORDERNUM 5

namespace OwLibCT
{
	class MacroService : public BaseService
	{
	private:
		int m_socketID;
	public:
		MacroService();
		virtual ~MacroService();
		int GetSocketID();
		void SetSocketID(int socketID);
	public:
		int AddMacro(int requestID, Macro *macro);
		int DeleteMacro(int requestID, Macro *macro);
		int ExecuteMacro(int requestID, Macro *macro);
		int GetMacros(int requestID, Macro *macro);
		static int GetMacros(vector<Macro> *macros, const char *body, int bodyLength);
		virtual void OnReceive(CMessage *message);
		int Send(int function, int requestID, vector<Macro> *macros);
		int StartFilter(int requestID, Macro *macro);
		int StopFilter(int requestID, Macro *macro);
		int UpdateMacro(int requestID, Macro *macro);
		int UpdateOrderNum(int requestID, int userID, vector<String> *ids);
	};
}

#endif