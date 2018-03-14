#include "..\\..\\stdafx.h"
#include "WinformControlHostEx.h"

WinformControlHostEx::WinformControlHostEx()
{
}

WinformControlHostEx::~WinformControlHostEx()
{
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ControlA* WinformControlHostEx::CreateInternalControl(ControlA *parent, const String& clsid)
{
	CalendarA *calendar = dynamic_cast<CalendarA*>(parent);
    if (calendar)
    {
        if (clsid == L"datetitle")
        {
            return new DateTitle(calendar);
        }
        else if (clsid == L"headdiv")
        {
            HeadDiv *headDiv = new HeadDiv(calendar);
            headDiv->SetWidth(parent->GetWidth());
            headDiv->SetDock(DockStyleA_Top);
            return headDiv;
        }
        else if (clsid == L"lastbutton")
        {
            return new ArrowButton(calendar);
        }
        else if (clsid == L"nextbutton")
        {
            ArrowButton *nextBtn = new ArrowButton(calendar);
            nextBtn->SetToLast(false);
            return nextBtn;
        }
    }
    SplitLayoutDivA *splitLayoutDiv = dynamic_cast<SplitLayoutDivA*>(parent);
    if (splitLayoutDiv)
    {
        if (clsid == L"splitter")
        {
            ButtonA *splitter = new ButtonA;
			splitter->SetBackColor(PCOLORS_BACKCOLOR);
            SIZE size = {5, 5};
            splitter->SetSize(size);
            return splitter;
        }
    }
    ScrollBarA *scrollBar = dynamic_cast<ScrollBarA*>(parent);
    if (scrollBar)
    {
		scrollBar->SetBackColor(COLOR_EMPTY);
        if (clsid == L"addbutton")
        {
            RibbonButton *addButton = new RibbonButton;
            SIZE size = {15, 15};
            addButton->SetSize(size);
			if (dynamic_cast<HScrollBarA*>(scrollBar))
            {
                addButton->SetArrowType(2);
            }
            else
            {
                addButton->SetArrowType(4);
            }
            return addButton;
        }
        else if (clsid == L"backbutton")
        {
            ButtonA *backButton = new ButtonA;
            return backButton;
        }
        else if (clsid == L"scrollbutton")
        {
            ButtonA *scrollButton = new ButtonA;
            scrollButton->SetAllowDrag(true);
			scrollButton->SetBackColor(PCOLORS_BACKCOLOR);
			scrollButton->SetBorderColor(PCOLORS_LINECOLOR3);
            return scrollButton;
        }
        else if (clsid == L"reducebutton")
        {
            RibbonButton *reduceButton = new RibbonButton;
            SIZE size = {15, 15};
            reduceButton->SetSize(size);
			if (dynamic_cast<HScrollBarA*>(scrollBar))
            {
                reduceButton->SetArrowType(1);
            }
            else
            {
                reduceButton->SetArrowType(3);
            }
            return reduceButton;
        }
    }
    TabPageA *tabPage = dynamic_cast<TabPageA*>(parent);
    if (tabPage)
    {
        if (clsid == L"headerbutton")
        {
            RibbonButton *button = new RibbonButton;
            button->SetAllowDrag(true);
            SIZE size = {100, 20};
            button->SetSize(size);
            return button;
        }
    }
    ComboBoxA *comboBox = dynamic_cast<ComboBoxA*>(parent);
    if (comboBox)
    {
        if (clsid == L"dropdownbutton")
        {
            RibbonButton *dropDownButton = new RibbonButton;
			dropDownButton->SetArrowType(4);
			dropDownButton->SetDisplayOffset(false);
            int width = comboBox->GetWidth();
            int height = comboBox->GetHeight();
            POINT location = {width - 16, 0};
            dropDownButton->SetLocation(location);
            SIZE size = {16, height};
            dropDownButton->SetSize(size);
            return dropDownButton;
        }
        else if (clsid == L"dropdownmenu")
        {
            ComboBoxMenu *comboBoxMenu = new ComboBoxMenu;
            comboBoxMenu->SetComboBox(comboBox);
            comboBoxMenu->SetPopup(true);
            SIZE size = {100, 200};
            comboBoxMenu->SetSize(size);
            return comboBoxMenu;
        }
    }
    DatePickerA *datePicker = dynamic_cast<DatePickerA*>(parent);
    if (datePicker)
    {
        if (clsid == L"dropdownbutton")
        {
            RibbonButton *dropDownButton = new RibbonButton;
			dropDownButton->SetArrowType(4);
            dropDownButton->SetDisplayOffset(false);
            int width = datePicker->GetWidth();
            int height = datePicker->GetHeight();
            POINT location = {width - 16, 0};
            dropDownButton->SetLocation(location);
            SIZE size = {16, height};
            dropDownButton->SetSize(size);
            return dropDownButton;
        }
        else if (clsid == L"dropdownmenu")
        {
            MenuA *dropDownMenu = new MenuA();
            PADDING padding(1);
            dropDownMenu->SetPadding(padding);
            dropDownMenu->SetPopup(true);
            SIZE size = {200, 200};
            dropDownMenu->SetSize(size);
            return dropDownMenu;
        }
    }
    SpinA *spin = dynamic_cast<SpinA*>(parent);
    if (spin)
    {
        if (clsid == L"downbutton")
        {
            RibbonButton *downButton = new RibbonButton;
			downButton->SetArrowType(4);
			downButton->SetDisplayOffset(false);
            SIZE size = {16, 16};
            downButton->SetSize(size);
            return downButton;
        }
        else if (clsid == L"upbutton")
        {
            RibbonButton *upButton = new RibbonButton;
			upButton->SetArrowType(3);
			upButton->SetDisplayOffset(false);
            SIZE size = {16, 16};
            upButton->SetSize(size);
            return upButton;
        }
    }
	DivA *div = dynamic_cast<DivA*>(parent);
    if (div)
    {
        if (clsid == L"hscrollbar")
        {
            HScrollBarA *hScrollBar = new HScrollBarA;
            hScrollBar->SetVisible(false);
            SIZE size = {15, 15};
            hScrollBar->SetSize(size);
            return hScrollBar;
        }
        else if (clsid == L"vscrollbar")
        {
            VScrollBarA *vScrollBar = new VScrollBarA;
            vScrollBar->SetVisible(false);
            SIZE size = {15, 15};
            vScrollBar->SetSize(size);
            return vScrollBar;
        }
    }
	GridA *grid = dynamic_cast<GridA*>(parent);
	if(grid)
	{
		if(clsid == L"edittextbox")
		{
			TextBoxA *textBox = new TextBoxA;
			textBox->SetBackColor(PCOLORS_BACKCOLOR);
			return textBox;
		}
	}
    return 0;
}

void WinformControlHostEx::Execute(String cmd)
{
	STARTUPINFO si;
	memset(&si, 0, sizeof(STARTUPINFO));  
	si.cb = sizeof(STARTUPINFO);  
	si.dwFlags = STARTF_USESHOWWINDOW;  
	si.wShowWindow = SW_SHOW;  
	PROCESS_INFORMATION pi;
	wchar_t szPath[255] = {0};
	_stprintf_s(szPath, 254, L"%s", cmd.c_str());
	CreateProcess(0, szPath, 0, 0, false, 0, 0, 0, &si, &pi);
}

String WinformControlHostEx::GetText()
{
	POINT mp = {0};
	GetCursorPos(&mp);
	HWND handle = WindowFromPoint(mp);
	if(handle)
	{
		char temp[10240] = {0};
		SendMessage(handle, 0xD, sizeof(temp) / sizeof(char), (LPARAM)temp);
		String result = L"";
		CStr::stringTowstring(result, temp);
		return result;
	}
	return L"";
}

void WinformControlHostEx::MouseEvent(String eventID, int dx, int dy, int data)
{
	DWORD flag = MOUSEEVENTF_MOVE;
	if(eventID == L"SETCURSOR")
	{
		SetCursorPos(dx, dy);
		return;
	}
    else if(eventID == L"MOVE")
    {
        flag = MOUSEEVENTF_MOVE;
    }
    else if(eventID == L"LEFTDOWN")
    {
        flag = MOUSEEVENTF_LEFTDOWN;
    }
    else if(eventID == L"LEFTUP")
    {
        flag = MOUSEEVENTF_LEFTUP;
    }
    else if(eventID == L"RIGHTDOWN")
    {
        flag = MOUSEEVENTF_RIGHTDOWN;
    }
	else if(eventID == L"RIGHTUP")
    {
        flag = MOUSEEVENTF_RIGHTUP;
    }
    else if(eventID == L"MIDDLEDOWN")
    {
        flag = MOUSEEVENTF_MIDDLEDOWN;
    }
    else if(eventID == L"MIDDLEUP")
    {
        flag = MOUSEEVENTF_MIDDLEUP;
    }
    else if(eventID == L"XDOWN")
    {
        flag = MOUSEEVENTF_XDOWN;
    }
    else if(eventID == L"XUP")
    {
        flag = MOUSEEVENTF_XUP;
    }
    else if(eventID == L"WHEEL")
    {
        flag = MOUSEEVENTF_WHEEL;
    }
    else if (eventID == L"VIRTUALDESK")
    {
        flag = MOUSEEVENTF_VIRTUALDESK;
    }
    else if (eventID == L"ABSOLUTE")
    {
        flag = MOUSEEVENTF_ABSOLUTE;
    }
	mouse_event(flag, dx, dy, data, 0);
}

void WinformControlHostEx::SendKey(String key)
{
}

void WinformControlHostEx::SetText(String text)
{
	POINT mp = {0};
}
