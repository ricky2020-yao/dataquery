#include "..\\..\\stdafx.h"
#include "ParametersWindow.h"

void ParametersWindow::ClickButton(void *sender, const POINT& mp, MouseButtonsA button, int clicks, int delta, void *pInvoke)
{
	if (button == MouseButtonsA_Left && clicks == 1)
    {
		ControlA *control = (ControlA*)sender;
		String name = control->GetName();
		ParametersWindow *parametersWindow = (ParametersWindow*)pInvoke;
		if (name == L"btnSubmit" || name == L"btnCancel" || name == L"btnClose")
		{
			parametersWindow->Close();
		}
	}
}

void ParametersWindow::GetParameters()
{
    if (m_indicator)
    {
        m_window->SetText(L"²ÎÊýÉèÖÃ(" + m_indicator->GetName() + L")");
        String parameters = m_indicator->GetParameters();
		vector<String> strs = CStr::Split(parameters, L";");
		int strsSize = (int)strs.size();
        int addHeight = 0;
        for (int i = 0; i < strsSize; i++)
        {
            String str = strs[i];
			vector<String> strs2 = CStr::Split(str, L":");
            String name = strs2[0];
            String value = strs2[1];
            LabelA *label = new LabelA();
			POINT location = {50, addHeight + 40};
            label->SetLocation(location);
            label->SetText(name);
            m_window->AddControl(label);
            SpinA *spin = new SpinA;
            location.x = 130;
            location.y = addHeight + 40;
            spin->SetLocation(location);
            spin->SetMaximum(10000000);
			spin->SetValue(CStr::ConvertStrToDouble(value.c_str()));
            m_window->AddControl(spin);
            addHeight += 30;
        }
		strs.clear();
        m_window->SetHeight(m_window->GetHeight() + addHeight);
        ButtonA *cancelButton = GetButton(L"btnCancel");
        ButtonA *submitButton = GetButton(L"btnSubmit");
        cancelButton->SetTop(cancelButton->GetTop() + addHeight);
        submitButton->SetTop(submitButton->GetTop() + addHeight);
    }
}

void ParametersWindow::RegisterEvents(ControlA *control)
{
	ControlMouseEvent clickButtonEvent = &ClickButton;
	ControlEvent spinInputChangedEvent = &SpinTextChanged;
	vector<ControlA*> controls = control->m_controls;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ButtonA *button = dynamic_cast<ButtonA*>(*sIter);
		if(button)
		{
			button->RegisterEvent(clickButtonEvent, EVENTID::CLICK, this);
		}
		SpinA *spin = dynamic_cast<SpinA*>(*sIter);
		if(spin)
		{
			spin->RegisterEvent(spinInputChangedEvent, EVENTID::TEXTCHANGED, this);
		}
		RegisterEvents(*sIter);
	}
}

void ParametersWindow::SetParameters()
{
    vector<ControlA*> controls = m_window->GetControls();
    vector<LabelA*> labels;
    vector<SpinA*> spins;
	vector<ControlA*>::iterator sIter = controls.begin();
	for(; sIter != controls.end(); ++sIter)
	{
		ControlA *control = *sIter;
        LabelA *label = dynamic_cast<LabelA*>(control);
        SpinA *spin = dynamic_cast<SpinA*>(control);
        if (label)
        {
            labels.push_back(label);
        }
        else if (spin)
        {
            spins.push_back(spin);
        }
	}
    int labelsSize = (int)labels.size();
    String newParameters = L"";
    for (int i = 0; i < labelsSize; i++)
    {
		TCHAR strValue[10];
		_stprintf_s(strValue, 9, L"%d", (int)spins[i]->GetValue());
		newParameters += labels[i]->GetText() + L":" + strValue;
        if (i != labelsSize - 1)
        {
            newParameters += L";";
        }
    }
    labels.clear();
    spins.clear();
    m_indicator->Clear();
	m_indicator->SetSourceField(CLOSE, CLOSE_INDEX);
	m_indicator->SetSourceField(HIGH, HIGH_INDEX);
    m_indicator->SetSourceField(LOW, LOW_INDEX);
    m_indicator->SetSourceField(OPEN, OPEN_INDEX);
    m_indicator->SetSourceField(VOL, VOL_INDEX);
	m_indicator->SetSourceField(AMOUNT, AMOUNT_INDEX);
	m_indicator->SetSourceField(CLOSE.substr(0, 1), CLOSE_INDEX);
	m_indicator->SetSourceField(HIGH.substr(0, 1), HIGH_INDEX);
	m_indicator->SetSourceField(LOW.substr(0, 1), LOW_INDEX);
	m_indicator->SetSourceField(OPEN.substr(0, 1), OPEN_INDEX);
	m_indicator->SetSourceField(VOL.substr(0, 1), VOL_INDEX);
	m_indicator->SetSourceField(AMOUNT.substr(0, 1), AMOUNT_INDEX);
    m_indicator->SetParameters(newParameters);
    m_indicator->SetScript(m_indicator->GetScript());
    m_indicator->OnCalculate(0);
    if (m_indicator->GetAttachVScale() == AttachVScale_Left)
    {
        m_indicator->GetDiv()->GetTitleBar()->SetText(m_indicator->GetTitle());
    }
    m_chart->GetChart()->Update();
	m_chart->GetChart()->Invalidate();
}

void ParametersWindow::SpinTextChanged(void *sender, void *pInvoke)
{
	ParametersWindow *parametersWindow = (ParametersWindow*)pInvoke;
	SpinA *spin = (SpinA*)sender;
	parametersWindow->OnSpinTextChanged(spin);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ParametersWindow::ParametersWindow(NativeBase *native, CIndicator *indicator)
{
	m_indicator = indicator;
	m_native = native;
	string xmlPath = DataCenter::GetAppPath() + "\\config\\ParametersWindow.xml";
	String wXmlPath;
	CStr::stringTowstring(wXmlPath, xmlPath);
    SetNative(m_native);
    LoadFile(wXmlPath, 0);
    m_window = dynamic_cast<WindowEx*>(FindControl(L"windowParameters"));
	GetParameters();
    RegisterEvents(m_window);
}

ParametersWindow::~ParametersWindow()
{
	m_indicator = 0;
	if (m_window)
    {
		if(m_window->GetParent())
		{
			m_window->Close();
		}
		delete m_window;
		m_window = 0;
    }
}

OwChart* ParametersWindow::GetChart()
{
	return m_chart;
}

void ParametersWindow::SetChart(OwChart *chart)
{
	m_chart = chart;
}

CIndicator* ParametersWindow::GetIndicator()
{
	return m_indicator;
}

WindowEx* ParametersWindow::GetWindow()
{
	return m_window;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


void ParametersWindow::Close()
{
	ControlA *focusedControl = m_native->GetFocusedControl();
	if(focusedControl)
	{
		m_native->RemoveControl(focusedControl);
	}
	m_window->Close();
	m_native->Invalidate();
	delete this;
}

void ParametersWindow::OnSpinTextChanged(SpinA *spin)
{
	SetParameters();
}

void ParametersWindow::Show()
{
	POINT location ={-m_window->GetWidth(), -m_window->GetHeight()};
    m_window->SetLocation(location);
	m_window->AnimateShow(false);
    m_window->Invalidate();
}