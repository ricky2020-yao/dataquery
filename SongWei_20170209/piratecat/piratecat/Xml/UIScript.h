/*****************************************************************************\
*                                                                             *
* UIScript.h -  Script functions, types, and definitions                      *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's UI. All rights reserved.      *
*                                                                             *
*******************************************************************************/

#ifndef __UISCRIPT_H__
#define __UISCRIPT_H__
#pragma once
#include "UIEvent.h"
#include "UIScript.h"

namespace OwLib
{
	class UIEvent;
	class UIXml;

	class UIScript
	{
	private:

		UIXml* m_xml;
	public:
		UIScript(UIXml *xml);
		virtual ~UIScript();
		UIXml* GetXml();
		void SetXml(UIXml *xml);
	public:
		virtual String CallFunction(const String& function);
		virtual void SetText(const String& text);
	};
}
#endif