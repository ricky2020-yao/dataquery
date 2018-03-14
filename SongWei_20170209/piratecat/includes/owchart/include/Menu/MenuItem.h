/*********************************************************************************\
*                                                                                 *
* MenuItem.h - Menu item functions, types, and definitions                        *
*                                                                                 *
*               Version 4.00 ������                                           *
*                                                                                 *
*               Copyright (c) 2016-2016, Lord's menu. All rights reserved.        *
*               Checked in 2016/10/09 by Lord.                                    *
*                                                                                 *
***********************************************************************************/

#ifndef __MENUITEM_H__
#define __MENUITEM_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Button\\Button.h"
#include "Menu.h"

namespace OwLib
{
	class MenuA;

	class MenuItemA : public ButtonA
	{
	private:
		bool m_checked;
		MenuA *m_dropDownMenu;
		MenuItemA *m_parentItem;
		MenuA *m_parentMenu;
		String m_value;
	public:
		vector<MenuItemA*> m_items;
		MenuItemA();
		MenuItemA(const String& text);
		virtual ~MenuItemA();
		bool IsChecked();
		void SetChecked(bool checked);
		MenuA* GetDropDownMenu();
		void SetDropDownMenu(MenuA *dropDownMenu);
		MenuItemA* GetParentItem();
		void SetParentItem(MenuItemA *parentItem);
		MenuA* GetParentMenu();
		void SetParentMenu(MenuA *parentMenu);
		String GetValue();
		void SetValue(const String& value);
	public:
		void AddItem(MenuItemA *item);
		void ClearItems();
		virtual String GetControlType();
		vector<MenuItemA*> GetItems();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		void InsertItem(int index, MenuItemA *item);
		virtual void OnAddingItem(int index);
		virtual void OnClick(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void OnRemovingItem();
		void RemoveItem(MenuItemA *item);
		virtual void SetProperty(const String& name, const String& value);
	};
}

#endif