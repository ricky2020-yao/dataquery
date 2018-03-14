#include "..\\..\\stdafx.h"
#include "UIXmlEx.h"
#include "FloatDiv.h"
#include "IndexDiv.h"
#include "LatestDiv.h"
#include "WindowEx.h"
#include "LayoutScaleDiv.h"
#include "BarrageDiv.h"
#include "RibbonButton.h"
#include "UserSecurityListT2.h"
#include "AIChat.h"
#include "NewsList.h"

namespace OwLib
{
	UIXmlEx::UIXmlEx()
	{
	}

	UIXmlEx::~UIXmlEx()
	{
	}

	ControlA* UIXmlEx::CreateControl(IXMLDOMNode *node, const String& type)
	{
		if(type == L"aichat")
		{
			return new AIChat;
		}
        else if (type == L"barragediv")
        {
            return new BarrageDiv;
        }
        else if (type == L"floatdiv")
        {
            return new FloatDiv;
        }
        else if (type == L"indexdiv")
        {
            return new IndexDiv;
        }
        else if (type == L"klinediv")
        {
            return new ChartA;
        }
        else if (type == L"latestdiv")
        {
            return new LatestDiv;
        }
        else if (type == L"layoutscalediv")
        {
            return new LayoutScaleDiv;
        }
		else if(type == L"newslist")
		{
			return new NewsList;
		}
        else if (type == L"ribbonbutton")
        {
            return new RibbonButton;
        }
        else if (type == L"searchdiv")
        {
            return new SearchDiv;
        }
		else if(type == L"usersecuritylistt2")
		{
			return new UserSecurityListT2;
		}
        else if (type == L"windowex")
        {
            return new WindowEx;
        }
		else
		{
			return UIXml::CreateControl(node, type);
		}
	}

	void UIXmlEx::LoadData()
	{
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	WindowXmlEx::WindowXmlEx()
	{
	}

	WindowXmlEx::~WindowXmlEx()
	{
	}

	void WindowXmlEx::Close()
	{
	}

	void WindowXmlEx::Show()
	{
	}
}