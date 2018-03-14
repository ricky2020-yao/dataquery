/*****************************************************************************\
*                                                                             *
* UIXml.h -     Xml ui functions, types, and definitions                      *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's UI. All rights reserved.      *
*                                                                             *
*******************************************************************************/

#ifndef __UIXML_H__
#define __UIXML_H__
#pragma once
#include "UIEvent.h"
#include "UIScript.h"
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

	class UIEvent;
	class UIScript;

	class UIXml
	{
	protected:
		vector<ControlA*> m_controls;
		UIEvent *m_event;
		NativeBase *m_native;
		UIScript *m_script;
		void CreateMenuItem(IXMLDOMNode *node, MenuA *menu, MenuItemA *parentItem);
		void CreateGridColumns(IXMLDOMNode *node, ControlA *control);
		void CreateGridRows(IXMLDOMNode *node, ControlA *control);
		void CreateSplitLayoutDiv(IXMLDOMNode *node, ControlA *control);
		void CreateTabPage(IXMLDOMNode *node, ControlA *control);
		void CreateTreeNode(IXMLDOMNode *node, ControlA *control, TreeNodeA *treeNode);
		void CreateTreeNodes(IXMLDOMNode *node, ControlA *control);
		ControlA* CreateUserControl(IXMLDOMNode *node);
		static void dprintf(char *format, ...);
		static IXMLDOMDocument* DomFromCOM();	
	public:
		UIXml();
		virtual ~UIXml();
		UIEvent* GetEvent();
		void SetEvent(UIEvent *uiEvent);
		NativeBase* GetNative();
		void SetNative(NativeBase *native);
		UIScript* GetScript();
		void SetScript(UIScript *script);
		String GetXmlPath();
	public:
		ButtonA* GetButton(const String& name);
		ChartA* GetChart(const String& name);
		CheckBoxA* GetCheckBox(const String& name);
		ComboBoxA* GetComboBox(const String& name);
		DatePickerA* GetDatePicker(const String& name);
		DivA* GetDiv(const String& name);
		GridA* GetGrid(const String& name);
		GroupBoxA* GetGroupBox(const String& name);
		LabelA* GetLabel(const String& name);
		LayoutDivA* GetLayoutDiv(const String& name);
		vector<ControlA*> GetLikeControls(const String& name);
		MenuA* GetMenu(const String& name);
		MenuItemA* GetMenuItem(const String& name);
		RadioButtonA* GetRadioButton(const String& name);
		SpinA* GetSpin(const String& name);
		SplitLayoutDivA* GetSplitLayoutDiv(const String& name);
		TabControlA* GetTabControl(const String& name);
		TableLayoutDivA* GetTableLayoutDiv(const String& name);
		TabPageA* GetTabPage(const String& name);
		TextBoxA* GetTextBox(const String& name);
		TreeA* GetTree(const String& name);
		WindowA* GetWindow(const String& name);
	public:
		virtual bool ContainsControl(ControlA *control);
		virtual ControlA* CreateControl(IXMLDOMNode *node, const String& type);
		void CreateNative();
		virtual void CreateSubProperty(IXMLDOMNode *node, ControlA *control);
		virtual ControlA* FindControl(const String& name);
		map<String, String> GetAttributes(IXMLDOMNode *node);
		vector<ControlA*> GetControls();
		virtual bool IsAfterSetingAttribute(const String& name);
		virtual void LoadFile(const String& fileName, ControlA *control);
		virtual void OnAddControl(ControlA *control, IXMLDOMNode *node);
		virtual void ReadBody(IXMLDOMNode *node, ControlA *control);
		virtual void ReadChildNodes(IXMLDOMNode *node, ControlA *control);
		virtual void ReadHead(IXMLDOMNode *node, ControlA *control);
		virtual ControlA* ReadNode(IXMLDOMNode *node, ControlA* parent);
		virtual void SetAttributesAfter(IXMLDOMNode *node, CProperty *control);
		virtual void SetAttributesBefore(IXMLDOMNode *node, CProperty *control);
		virtual void SetEvents(IXMLDOMNode *node, CProperty *control);
	};
}
#endif