/*****************************************************************************\
*                                                                             *
* CDraw.h -    Draw functions, types, and definitions.                        *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/21.                                    *
*                                                                             *
******************************************************************************/

#ifndef __CDRAW_H__
#define __CDRAW_H__
#pragma once

#define PCOLORS_USERCOLOR -200000000100
#define PCOLORS_BACKCOLOR PCOLORS_USERCOLOR - 1
#define PCOLORS_BACKCOLOR2 PCOLORS_USERCOLOR - 2
#define PCOLORS_BACKCOLOR3 PCOLORS_USERCOLOR - 3
#define PCOLORS_BACKCOLOR4 PCOLORS_USERCOLOR - 4
#define PCOLORS_BACKCOLOR5 PCOLORS_USERCOLOR - 5
#define PCOLORS_BACKCOLOR6 PCOLORS_USERCOLOR - 6
#define PCOLORS_BACKCOLOR7 PCOLORS_USERCOLOR - 7
#define PCOLORS_FORECOLOR PCOLORS_USERCOLOR - 100
#define PCOLORS_FORECOLOR2 PCOLORS_USERCOLOR - 101
#define PCOLORS_FORECOLOR3 PCOLORS_USERCOLOR - 102
#define PCOLORS_FORECOLOR4 PCOLORS_USERCOLOR - 103
#define PCOLORS_FORECOLOR5 PCOLORS_USERCOLOR - 104
#define PCOLORS_FORECOLOR6 PCOLORS_USERCOLOR - 105
#define PCOLORS_FORECOLOR7 PCOLORS_USERCOLOR - 106
#define PCOLORS_FORECOLOR8 PCOLORS_USERCOLOR - 107
#define PCOLORS_FORECOLOR9 PCOLORS_USERCOLOR - 108
#define PCOLORS_FORECOLOR10 PCOLORS_USERCOLOR - 109
#define PCOLORS_FORECOLOR11 PCOLORS_USERCOLOR - 110
#define PCOLORS_LINECOLOR PCOLORS_USERCOLOR - 200
#define PCOLORS_LINECOLOR2 PCOLORS_USERCOLOR - 201
#define PCOLORS_LINECOLOR3 PCOLORS_USERCOLOR - 202
#define PCOLORS_LINECOLOR4 PCOLORS_USERCOLOR - 203
#define PCOLORS_LINECOLOR5 PCOLORS_USERCOLOR - 204
#define PCOLORS_MIDCOLOR PCOLORS_USERCOLOR - 300
#define PCOLORS_UPCOLOR PCOLORS_USERCOLOR - 301
#define PCOLORS_DOWNCOLOR PCOLORS_USERCOLOR - 302
#define PCOLORS_DOWNCOLOR2 PCOLORS_USERCOLOR - 303
#define PCOLORS_DOWNCOLOR3 PCOLORS_USERCOLOR - 304
#define PCOLORS_SELECTEDROWCOLOR PCOLORS_USERCOLOR - 400
#define PCOLORS_HOVEREDROWCOLOR PCOLORS_USERCOLOR - 401
#define PCOLORS_WINDOWFORECOLOR PCOLORS_USERCOLOR - 500
#define PCOLORS_WINDOWBACKCOLOR PCOLORS_USERCOLOR - 501
#define PCOLORS_WINDOWBACKCOLOR2 PCOLORS_USERCOLOR - 502
#define PCOLORS_WINDOWCONTENTBACKCOLOR PCOLORS_USERCOLOR - 503

class CDraw
{
public:
	static _int64 GetBlackColor(_int64 color);
	static _int64 GetWhiteColor(_int64 color);
	static SIZE DrawText(CPaint *paint, LPCTSTR strText, _int64 dwPenColor, FONT *font, int x, int y);
	static int DrawUnderLineNum(CPaint *paint, double value, int digit, FONT *font, _int64 fontColor, bool zeroAsEmpty, int x, int y);
	static _int64 GetPriceColor(double price, double comparePrice);
};
#endif