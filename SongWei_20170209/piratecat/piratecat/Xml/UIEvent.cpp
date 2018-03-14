#include "..\\stdafx.h"
#include "UIEvent.h"

namespace OwLib
{
	EventInfo::EventInfo()
	{
	}

	EventInfo::~EventInfo()
	{
		m_functions.clear();
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void EventInfo::AddEvent(int eventID, const String& function)
	{
		m_functions[eventID] = function;
	}

	String EventInfo::GetFunction(int eventID)
	{
		map<int, String>::iterator sIter = m_functions.find(eventID);
		if(sIter != m_functions.end())
		{
			return sIter->second;
		}
		return L"";
	}

	void EventInfo::RemoveEvent(int eventID)
	{
		map<int, String>::iterator sIter = m_functions.find(eventID);
		if(sIter != m_functions.end())
		{
			m_functions.erase(sIter);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	UIEvent::UIEvent(UIXml *xml)
	{
		m_script = 0;
		m_sender = L"";
		m_xml = xml;
	}

	UIEvent::~UIEvent()
	{
		map<ControlA*, EventInfo*>::iterator sIter = m_events.begin();
		for(; sIter != m_events.end(); ++sIter)
		{
			delete sIter->second;
		}
		m_events.clear();
		m_script = 0;
		m_xml = 0;
	}

	UIScript* UIEvent::GetScript()
	{
		return m_script;
	}

	void UIEvent::SetScript(UIScript *script)
	{
		m_script = script;
	}

	String UIEvent::GetSender()
	{
		return m_sender;
	}

	void UIEvent::SetSender(const String& sender)
	{
		m_sender = sender;
	}

	UIXml* UIEvent::GetXml()
	{
		return m_xml;
	}

	void UIEvent::SetXml(UIXml *xml)
	{
		m_xml = xml;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void UIEvent::CallAdd(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::ADD, pInvoke);
	}

    void UIEvent::CallCheckedChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::CHECKEDCHANGED, pInvoke);
	}

    void UIEvent::CallClick(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::CLICK, pInvoke);
	}

    void UIEvent::CallCopy(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::COPY, pInvoke);
	}

    void UIEvent::CallDragBegin(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::DRAGBEGIN, pInvoke);
	}

    void UIEvent::CallDragEnd(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::DRAGEND, pInvoke);
	}

    void UIEvent::CallDragging(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::DRAGGING, pInvoke);
	}

    void UIEvent::CallEnableChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::ENABLECHANGED, pInvoke);
	}

    String UIEvent::CallFunction(void *sender, int eventID, void *pInvoke)
	{
		UIEvent *uiEvent = (UIEvent*)pInvoke;
		if(uiEvent)
		{
			ControlA *control = (ControlA*)sender;
			map<ControlA*, EventInfo*>::iterator sIter = uiEvent->m_events.find(control);
			if(sIter != uiEvent->m_events.end())
			{
				EventInfo *eventInfo = sIter->second;
				String function = eventInfo->GetFunction(eventID);
				if (function.length() > 0)
				{
					UIScript *script = uiEvent->GetXml()->GetScript();
					if (script)
					{
						uiEvent->SetSender(control->GetName());
						String result = script->CallFunction(function);
						uiEvent->SetSender(L"");
						return result;
					}
				}
			}

		}
		return L"";
	}

    void UIEvent::CallGotFocus(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::GOTFOCUS, pInvoke);
	}

    void UIEvent::CallInvoke(void *sender, void *args, void *pInvoke)
	{
		CallFunction(sender, EVENTID::INVOKE, pInvoke);
	}

    void UIEvent::CallLoad(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::LOAD, pInvoke);
	}

    void UIEvent::CallLocationChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::LOCATIONCHANGED, pInvoke);
	}

    void UIEvent::CallLostFocus(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::LOSTFOCUS, pInvoke);
	}

    void UIEvent::CallKeyDown(void *sender, char key, void *pInvoke)
	{
		CallFunction(sender, EVENTID::KEYDOWN, pInvoke);
	}

    void UIEvent::CallKeyUp(void *sender, char key, void *pInvoke)
	{
		CallFunction(sender, EVENTID::KEYUP, pInvoke);
	}

    void UIEvent::CallMenuItemClick(void *sender, MenuItemA *item, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MENUITEMCLICK, pInvoke);
	}

    void UIEvent::CallMouseDown(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MOUSEDOWN, pInvoke);
	}

    void UIEvent::CallMouseEnter(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MOUSEENTER, pInvoke);
	}

    void UIEvent::CallMouseLeave(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MOUSELEAVE, pInvoke);
	}

    void UIEvent::CallMouseMove(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MOUSEMOVE, pInvoke);
	}

    void UIEvent::CallMouseUp(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MOUSEUP, pInvoke);
	}

    void UIEvent::CallMouseWheel(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
	{
		CallFunction(sender, EVENTID::MOUSEWHEEL, pInvoke);
	}

    void UIEvent::CallPaint(void *sender, CPaint *paint, const RECT& clipRect, void *pInvoke)
	{
		CallFunction(sender, EVENTID::PAINT, pInvoke);
	}

    void UIEvent::CallPaintBorder(void *sender, CPaint *paint, const RECT& clipRect, void *pInvoke)
	{
		CallFunction(sender, EVENTID::PAINTBORDER, pInvoke);
	}

    void UIEvent::CallPaste(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::PASTE, pInvoke);
	}

    void UIEvent::CallRemove(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::REMOVE, pInvoke);
	}

    void UIEvent::CallSelectedDayChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::SELECTEDDAYCHANGED, pInvoke);
	}

    void UIEvent::CallSelectedIndexChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::SELECTEDINDEXCHANGED, pInvoke);
	}

    void UIEvent::CallSelectedTabPageChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::SELECTEDTABPAGECHANGED, pInvoke);
	}

    void UIEvent::CallScrolled(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::SCROLLED, pInvoke);
	}

    void UIEvent::CallSizeChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::SIZECHANGED, pInvoke);
	}

    void UIEvent::CallTextChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::TEXTCHANGED, pInvoke);
	}

    void UIEvent::CallTimer(void *sender, int timerID, void *pInvoke)
	{
		CallFunction(sender, EVENTID::TIMER, pInvoke);
	}

    void UIEvent::CallVisibleChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::VISIBLECHANGED, pInvoke);
	}

    void UIEvent::CallValueChanged(void *sender, void *pInvoke)
	{
		CallFunction(sender, EVENTID::VALUECHANGED, pInvoke);
	}

	int UIEvent::GetEventID(const String& eventName)
	{
		String lowerName = CStr::ToLower(eventName);
        if (lowerName == L"onadd") return EVENTID::ADD;
        else if (lowerName == L"onclick") return EVENTID::CLICK;
        else if (lowerName == L"oncopy") return EVENTID::COPY;
        else if (lowerName == L"ondragbegin") return EVENTID::DRAGBEGIN;
        else if (lowerName == L"ondragend") return EVENTID::DRAGEND;
        else if (lowerName == L"ondragging") return EVENTID::DRAGGING;
        else if (lowerName == L"onenablechanged") return EVENTID::ENABLECHANGED;
        else if (lowerName == L"ongotfocus") return EVENTID::GOTFOCUS;
        else if (lowerName == L"oninvoke") return EVENTID::INVOKE;
        else if (lowerName == L"onkeydown") return EVENTID::KEYDOWN;
        else if (lowerName == L"onkeyup") return EVENTID::KEYUP;
        else if (lowerName == L"onload") return EVENTID::LOAD;
        else if (lowerName == L"onlocationchanged") return EVENTID::LOCATIONCHANGED;
        else if (lowerName == L"onlostfocus") return EVENTID::LOSTFOCUS;
        else if (lowerName == L"onmousedown") return EVENTID::MOUSEDOWN;
        else if (lowerName == L"onmouseenter") return EVENTID::MOUSEENTER;
        else if (lowerName == L"onmouseleave") return EVENTID::MOUSELEAVE;
        else if (lowerName == L"onmousemove") return EVENTID::MOUSEMOVE;
        else if (lowerName == L"onmouseup") return EVENTID::MOUSEUP;
        else if (lowerName == L"onmousewheel") return EVENTID::MOUSEWHEEL;
        else if (lowerName == L"onpaint") return EVENTID::PAINT;
        else if (lowerName == L"onpaintborder") return EVENTID::PAINTBORDER;
        else if (lowerName == L"onpaste") return EVENTID::PASTE;
        else if (lowerName == L"onremove") return EVENTID::REMOVE;
        else if (lowerName == L"onsizechanged") return EVENTID::SIZECHANGED;
        else if (lowerName == L"ontextchanged") return EVENTID::TEXTCHANGED;
        else if (lowerName == L"ontimer") return EVENTID::TIMER;
        else if (lowerName == L"onvisiblechanged") return EVENTID::VISIBLECHANGED;
        else if (lowerName == L"oncheckedchanged") return EVENTID::CHECKEDCHANGED;
        else if (lowerName == L"onselecteddaychanged") return EVENTID::SELECTEDDAYCHANGED;
        else if (lowerName == L"onmenuitemclick") return EVENTID::MENUITEMCLICK;
        else if (lowerName == L"onselectedindexchanged") return EVENTID::SELECTEDINDEXCHANGED;
        else if (lowerName == L"onselectedtabpagechanged") return EVENTID::SELECTEDTABPAGECHANGED;
        else if (lowerName == L"onscrolled") return EVENTID::SCROLLED;
        else if (lowerName == L"onvaluechanged") return EVENTID::VALUECHANGED;
        return -1;
	}

	void UIEvent::RegisterEvent(ControlA *control, const String& eventName, const String& function)
	{
	    int eventID = GetEventID(eventName);
        if (eventID != -1)
        {
            EventInfo *eventInfo = 0;
			map<ControlA*, EventInfo*>::iterator sIter = m_events.find(control);
			if(sIter != m_events.end())
			{
				eventInfo = sIter->second;
			}
			else
			{
				eventInfo = new EventInfo();
                m_events[control] = eventInfo;
			}
            eventInfo->AddEvent(eventID, function);
            switch (eventID)
            {
                case EVENTID::ADD:
                    {
                        ControlEvent cEvent = &CallAdd;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::CLICK:
                    {
                        ControlMouseEvent cEvent = &CallClick;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::COPY:
                    {
                        ControlEvent cEvent = &CallCopy;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::DRAGBEGIN:
                    {
                        ControlEvent cEvent = &CallDragBegin;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::DRAGEND:
                    {
                        ControlEvent cEvent = &CallDragEnd;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::DRAGGING:
                    {
                        ControlEvent cEvent = &CallDragging;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::ENABLECHANGED:
                    {
                        ControlEvent cEvent = &CallEnableChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::GOTFOCUS:
                    {
                        ControlEvent cEvent = &CallGotFocus;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::INVOKE:
                    {
                        ControlInvokeEvent cEvent = &CallInvoke;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::KEYDOWN:
                    {
                        ControlKeyEvent cEvent = &CallKeyDown;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::KEYUP:
                    {
                        ControlKeyEvent cEvent = &CallKeyUp;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::LOAD:
                    {
                        ControlEvent cEvent = &CallLoad;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::LOCATIONCHANGED:
                    {
                        ControlEvent cEvent = &CallLocationChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::LOSTFOCUS:
                    {
                        ControlEvent cEvent = &CallLostFocus;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MOUSEDOWN:
                    {
                        ControlMouseEvent cEvent = &CallMouseDown;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MOUSEENTER:
                    {
                        ControlMouseEvent cEvent = &CallMouseEnter;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MOUSELEAVE:
                    {
                        ControlMouseEvent cEvent = &CallMouseLeave;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MOUSEMOVE:
                    {
                        ControlMouseEvent cEvent = &CallMouseMove;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MOUSEUP:
                    {
                        ControlMouseEvent cEvent = &CallMouseUp;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MOUSEWHEEL:
                    {
                        ControlMouseEvent cEvent = &CallMouseWheel;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::PAINT:
                    {
                        ControlPaintEvent cEvent = &CallPaint;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::PAINTBORDER:
                    {
                        ControlPaintEvent cEvent = &CallPaintBorder;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::PASTE:
                    {
                        ControlEvent cEvent = &CallPaste;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::REMOVE:
                    {
                        ControlEvent cEvent = &CallRemove;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::SIZECHANGED:
                    {
                        ControlEvent cEvent = &CallSizeChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::TEXTCHANGED:
                    {
                        ControlEvent cEvent = &CallTextChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::TIMER:
                    {
                        ControlTimerEvent cEvent = &CallTimer;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::VISIBLECHANGED:
                    {
                        ControlEvent cEvent = &CallVisibleChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::CHECKEDCHANGED:
                    {
                        ControlEvent cEvent = &CallCheckedChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::SELECTEDDAYCHANGED:
                    {
                        ControlEvent cEvent = &CallSelectedDayChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::MENUITEMCLICK:
                    {
                        MenuItemMouseEvent cEvent = &CallMenuItemClick;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::SELECTEDINDEXCHANGED:
                    {
                        ControlEvent cEvent = &CallSelectedIndexChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::SELECTEDTABPAGECHANGED:
                    {
                        ControlEvent cEvent = &CallSelectedTabPageChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::SCROLLED:
                    {
                        ControlEvent cEvent = &CallScrolled;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }
                case EVENTID::VALUECHANGED:
                    {
                        ControlEvent cEvent = &CallVisibleChanged;
                        control->RegisterEvent(cEvent, eventID, this);
                        break;
                    }

            }
        }
	}
}