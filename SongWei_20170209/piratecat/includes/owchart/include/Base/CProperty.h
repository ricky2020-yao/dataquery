/*****************************************************************************\
*                                                                             *
* CProperty.h -   Property functions                                          *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __CPROPERTY_H__
#define __CPROPERTY_H__
#pragma once
#include "..\\..\\stdafx.h"

namespace OwLib
{
	class CProperty
	{
	public:
		virtual void GetProperty(const String& name, String *value, String *type)
		{
		}
		virtual vector<String> GetPropertyNames()
		{
			vector<String> propertyNames;
			return propertyNames;
		}
		virtual void SetProperty(const String& name, const String& value)
		{
		}
	};
}
#endif