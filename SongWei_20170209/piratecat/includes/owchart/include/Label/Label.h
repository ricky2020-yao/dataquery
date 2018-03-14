/*****************************************************************************\
*                                                                             *
* Label.h -  Label functions, types, and definitions                          *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's label. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __LABEL_H__
#define __LABEL_H__
#pragma once
#include "..\\..\\stdafx.h"

namespace OwLib
{
	class LabelA : public ControlA
	{
	private:
	public:
		LabelA();
		virtual ~LabelA();
	public:
		virtual String GetControlType();
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
	};
}
#endif