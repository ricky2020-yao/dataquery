/*****************************************************************************\
*                                                                             *
* GdiPaintEx.h - Gdi paint functions, types, and definitions.                 *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/29.                                    *
*                                                                             *
******************************************************************************/

#ifndef __GDIPAINTEX_H__
#define __GDIPAINTEX_H__
#pragma once
#include "CDraw.h"

class GdiPaintEx : public GdiPaint
{
public:
	GdiPaintEx();
	virtual ~GdiPaintEx();
public:
	virtual _int64 GetColor(_int64 dwPenColor);
};
#endif