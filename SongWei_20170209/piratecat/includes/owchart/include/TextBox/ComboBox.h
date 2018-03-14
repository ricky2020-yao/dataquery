/******************************************************************************\
*                                                                              *
* ComboBox.h -  Combo box functions, types, and definitions                    *
*                                                                              *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                        *
*                                                                              *
*               Copyright (c) 2016-2016, Lord's combo box. All rights reserved.*
*               Checked 2016/9/25 by Lord.                                     *
*                                                                              *
*******************************************************************************/

#ifndef __COMBOBOX_H__
#define __COMBOBOX_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "TextBox.h"
#include "..\\Button\\Button.h"
#include "..\\Menu\\Menu.h"
#include "..\\Menu\\MenuItem.h"

namespace OwLib
{
	class ComboBoxA;

	class ComboBoxMenu : public MenuA
	{
	private:
		ComboBoxA *m_comboBox;
	public:
		ComboBoxMenu();
		virtual ~ComboBoxMenu();
		ComboBoxA* GetComboBox();
		void SetComboBox(ComboBoxA *comboBox);
		virtual bool OnAutoHide();
	};

	class ComboBoxA : public TextBoxA
	{
	private:
		ButtonA* m_dropDownButton;
		ControlMouseEvent m_dropDownButtonMouseDownEvent;
		MenuItemMouseEvent m_menuItemClickEvent;
		ControlKeyEvent m_menuKeyDownEvent;
		ComboBoxMenu* m_dropDownMenu;
		static void DropDownButtonMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void MenuItemClick(void *sender, MenuItemA *item, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke);
		static void MenuKeyDown(void *sender, char key, void *pInvoke);
	public:
		ComboBoxA();
		virtual ~ComboBoxA();
		ButtonA* GetDropDownButton();
		ComboBoxMenu* GetDropDownMenu();
		int GetSelectedIndex();
		void SetSelectedIndex(int selectedIndex);
		String GetSelectedText();
		void SetSelectedText(const String& selectedText);
		String GetSelectedValue();
		void SetSelectedValue(const String& selectedValue);
	public:
		void AddItem(MenuItemA *item);
		void ClearItems();
		virtual String GetControlType();
		vector<MenuItemA*> GetItems();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		void InsertItem(int index, MenuItemA *item);
		virtual void OnKeyDown(char key);
		virtual void OnLoad();
		virtual void OnSelectedIndexChanged();
		virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		void RemoveItem(MenuItemA *item);
		virtual void SetProperty(const String& name, const String& value);
		virtual void Update();
	};
}

#endif