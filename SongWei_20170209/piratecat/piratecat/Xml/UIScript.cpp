#include "..\\stdafx.h"
#include "UIScript.h"

namespace OwLib
{
	UIScript::UIScript(UIXml *xml)
	{
		m_xml = xml;
	}

	UIScript::~UIScript()
	{
		m_xml = 0;
	}

	UIXml* UIScript::GetXml()
	{
		return m_xml;
	}

	void UIScript::SetXml(UIXml *xml)
	{
		m_xml = xml;
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String UIScript::CallFunction(const String& function)
	{
		return L"";
	}

	void UIScript::SetText(const String& text)
	{
	}
}