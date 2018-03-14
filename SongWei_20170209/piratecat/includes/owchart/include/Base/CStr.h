/*****************************************************************************\
*                                                                             *
* CStr.h -      String functions                                              *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __CSTR_H__
#define __CSTR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include <tchar.h>
#include "CMathLib.h"
#include "CPaint.h"
#include "Control.h"

namespace OwLib
{
	class CStr
	{
	private:
		static double round(double x); 
	public:
		static void Contact(wchar_t *str, const wchar_t *str1, const wchar_t *str2, const wchar_t *str3);
		static String ConvertAnchorToStr(const ANCHOR& anchor);
		static String ConvertBoolToStr(bool value);
		static String ConvertColorToStr(_int64 color);
		static String ConvertContentAlignmentToStr(ContentAlignmentA contentAlignment);
		static String ConvertCursorToStr(CursorsA cursor);
		static String ConvertDoubleToStr(double value);
		static String ConvertDockToStr(DockStyleA dock);
		static String ConvertFloatToStr(float value);
		static String ConvertFontToStr(FONT *font);
		static String ConvertHorizontalAlignToStr(HorizontalAlignA horizontalAlign);
		static String ConvertIntToStr(int value);
		static String ConvertLayoutStyleToStr(LayoutStyleA layoutStyle);
		static String ConvertPaddingToStr(const PADDING& padding);
		static String ConvertPointToStr(const POINT& mp);
		static String ConvertRectToStr(const RECT& rect);
		static String ConvertSizeToStr(const SIZE& size);
		static ANCHOR ConvertStrToAnchor(const String& str);
		static bool ConvertStrToBool(const String& str);
		static _int64 ConvertStrToColor(const String& str);
		static ContentAlignmentA ConvertStrToContentAlignment(const String& str);
		static CursorsA ConvertStrToCursor(const String& str);
		static DockStyleA ConvertStrToDock(const String& str);
		static double ConvertStrToDouble(const String& str);
		static double ConvertStrToDouble(const wchar_t *str);
		static float ConvertStrToFloat(const String& str);
		static FONT* ConvertStrToFont(const String& str);
		static HorizontalAlignA ConvertStrToHorizontalAlign(const String& str);
		static int ConvertStrToInt(const String& str);
		static int ConvertStrToInt(const wchar_t *str);
		static LayoutStyleA ConvertStrToLayoutStyle(const String& str);
		static PADDING ConvertStrToPadding(const String& str);
		static POINT ConvertStrToPoint(const String& str);
		static RECT ConvertStrToRect(const String& str);
		static SIZE ConvertStrToSize(const String& str);
		static void GetValueByDigit(double value, int digit, wchar_t *str);
		static void GetFormatDate(double date, wchar_t *str);
		static int HexToDec(const char *str);
		static String Replace(const String& str, const String& src, const String& dest);
		static vector<String> Split(const String& str, const String& pattern);
		static void stringTowstring(String &strDest, const string& strSrc);
		static String ToLower(const String& str);
		static String ToUpper(const String& str);
		static void wstringTostring(string &strDest, const String& strSrc);
	};
}
#endif