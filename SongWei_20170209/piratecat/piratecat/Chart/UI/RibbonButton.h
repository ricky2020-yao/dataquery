/*******************************************************************************\
*                                                                               *
* RibbonButton.h - Ribbon button functions, types, and definitions.             *
*                                                                               *
*               Version 6.00 бя                                                 *
*                                                                               *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.        *
*               Created by Lord 2016/12/2.                                      *
*                                                                               *
********************************************************************************/

#ifndef __RIBBONBUTTON_H__
#define __RIBBONBUTTON_H__
#pragma once
#include "CDraw.h"
#include "GdiPlusPaintEx.h"

class RibbonButton : public ButtonA
{
private:
	int m_arrowType;
	bool m_isClose;
public:
	RibbonButton();
	virtual ~RibbonButton();
	int GetArrowType();
	void SetArrowType(int arrowType);
	bool IsClose();
	void SetClose(bool isClose);
	bool IsSelected();
public:
	virtual _int64 GetPaintingForeColor();
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
};
#endif