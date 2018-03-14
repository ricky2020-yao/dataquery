/*****************************************************************************\
*                                                                             *
* GdiPlusPaintEx.h - GdiPlus paint functions, types, and definitions.         *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/29.                                    *
*                                                                             *
******************************************************************************/

#ifndef __GDIPLUSPAINTEX_H__
#define __GDIPLUSPAINTEX_H__
#pragma once
#include "CDraw.h"

class GdiPlusPaintEx:public GdiPlusPaint
{
private:
	bool m_supportTransparent;
	HBITMAP m_memBM;
	HDC m_wndHDC;
public:
	GdiPlusPaintEx();
	virtual ~GdiPlusPaintEx();
public:
	virtual _int64 GetColor(_int64 dwPenColor);
	virtual void SetSupportTransparent(bool supportTransparent);
	virtual bool SupportTransparent();
};
#endif