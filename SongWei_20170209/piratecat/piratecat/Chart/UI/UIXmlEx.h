/*****************************************************************************\
*                                                                             *
* UIXmlEx.h - Chart xml functions, types, and definitions.                 *
*                                                                             *
*               Version 1.00  ¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

#ifndef __CHARTUIXML_H__
#define __CHARTUIXML_H__
#pragma once
#include "..\\..\\Xml\\UIXml.h"
#include <atlstr.h>  
#import <msxml3.dll>

namespace OwLib
{
	#define HRCALL(a, errmsg) \
	do { \
		hr = (a); \
		if (FAILED(hr)) { \
			dprintf( "%s:%d  HRCALL Failed: %s\n  0x%.8x = %s\n", \
					__FILE__, __LINE__, errmsg, hr, #a ); \
			goto clean; \
		} \
	} while (0)

	class UIXmlEx : public UIXml
	{	
	public:
		UIXmlEx();
		virtual ~UIXmlEx();
	public:
		virtual ControlA* CreateControl(IXMLDOMNode *node, const String& type);
		virtual void LoadData();
	};

	class WindowXmlEx : public UIXmlEx
	{
	public:
		WindowXmlEx();
		virtual ~WindowXmlEx();
	public:
		virtual void Close();
		virtual void Show();
	};
}
#endif