/*****************************************************************************\
*                                                                             *
* WindowFrameA.h -   Window frame functions, types, and definitions           *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
******************************************************************************/

#ifndef __WINDOWFRAME_H__
#define __WINDOWFRAME_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "Window.h"

namespace OwLib
{
	class WindowFrameA : public ControlA
	{
	private:
	public:
		WindowFrameA();
		virtual ~WindowFrameA();
	public:
		virtual bool ContainsPoint(const POINT& point);
		virtual String GetControlType();
		virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
	};
}
#endif