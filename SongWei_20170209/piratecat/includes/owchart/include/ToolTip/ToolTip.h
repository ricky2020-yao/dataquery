/*****************************************************************************\
*                                                                             *
* ToolTip.h -   Tool tip functions, types, and definitions                    *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's tool tip. All rights reserved.*
*                                                                             *
*******************************************************************************/

#ifndef __TOOLTIP_H__
#define __TOOLTIP_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Label\\Label.h"

namespace OwLib
{
	class ToolTipA : public LabelA
	{
	private:
		int m_autoPopDelay;
		int m_initialDelay;
		POINT m_lastMousePoint;
		bool m_useAnimation;
	protected:
		int m_timerID;
		int m_remainAutoPopDelay;
		int m_remainInitialDelay;
	public:
		ToolTipA();
		virtual ~ToolTipA();
		int GetAutoPopDelay();
		void SetAutoPopDelay(int autoPopDelay);
		int GetInitialDelay();
		void SetInitialDelay(int initialDelay);
		bool UseAnimation();
		void SetUseAnimation(bool useAnimation);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void Hide();
		virtual void OnLoad();
		virtual void OnTimer(int timerID);
		virtual void OnVisibleChanged();
		virtual void Show();
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif