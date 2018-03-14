/*****************************************************************************\
*                                                                             *
* LinkLabel.h -  Link label functions, types, and definitions                 *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's label. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __LINKLABEL_H__
#define __LINKLABEL_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "Label.h"

namespace OwLib
{
	typedef enum LinkBehaviorA
	{
		LinkBehaviorA_AlwaysUnderLine,
		LinkBehaviorA_HoverUnderLine,
		LinkBehaviorA_NeverUnderLine
	};

	class LabelA;

	class LinkLabelA : public LabelA
	{
	private:
		_int64 m_activeLinkColor;
		_int64 m_disabledLinkColor;
		LinkBehaviorA m_linkBehavior;
		_int64 m_linkColor;
		bool m_linkVisited;
		bool m_visited;
		_int64 m_visitedLinkColor;
	protected:
		virtual _int64 GetPaintingLinkColor();
	public:
		LinkLabelA();
		virtual ~LinkLabelA();
		_int64 GetActiveLinkColor();
		void SetActiveLinkColor(_int64 activeLinkColor);
		_int64 GetDisabledLinkColor();
		void SetDisabledLinkColor(_int64 disabledLinkColor);
		LinkBehaviorA GetLinkBehavior();
		void SetLinkBehavior(LinkBehaviorA linkBehavior);
		_int64 GetLinkColor();
		void SetLinkColor(_int64 linkColor);
		bool IsLinkVisited();
		void SetLinkVisited(bool linkVisited);
		_int64 GetVisitedLinkColor();
		void SetVisitedLinkColor(_int64 visitedLinkColor);
	public:
		virtual String GetControlType();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseEnter(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseLeave(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void SetProperty(const String& name, const String& value);
	};
}
#endif