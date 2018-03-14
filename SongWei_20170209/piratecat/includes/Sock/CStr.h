/*****************************************************************************\
*                                                                             *
* Str.h -      String functions                                              *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __CSTRA_H__
#define __CSTRA_H__
#pragma once
#include "stdafx.h"

namespace OwLibCT
{
	class CStrA
	{
	public:
		static void Contact(wchar_t *str, LPCTSTR str1, LPCTSTR str2 = L"", LPCTSTR str3 = L"");
		static string ConvertDBCodeToFileName(const string& code);
		static double ConvertStrToDouble(const wchar_t *str);
		static int ConvertStrToInt(const wchar_t *str);
		static String GetDBString(const String& strSrc);
		static void GetFormatDate(double date, wchar_t *str);
		static string GetGuid();
		static int GetStringCapacity(const string& str);
		static void GetValueByDigit(double value, int digit, wchar_t *str);
		static int GetWStringCapacity(const String& str);
		static double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec);
		static void M130(double num, int *tm_year, int *tm_mon, int *tm_mday, int *tm_hour, int *tm_min, int *tm_sec, int *tm_msec);
		static string Replace(const string& str, const string& src, const string& dest);
		static String Replace(const String& str, const String& src, const String& dest);
		static vector<string> Split(string str, string pattern);
		static vector<String> Split(String str, String pattern);
		static void stringTowstring(String &strDest, const string& strSrc);
		static String ToLower(const String& str);
		static String ToUpper(const String& str);
		static void wstringTostring(string &strDest, const String& strSrc);
	};
}
#endif