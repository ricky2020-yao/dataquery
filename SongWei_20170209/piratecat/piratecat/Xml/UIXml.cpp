#include "..\\stdafx.h"
#include "UIXml.h"

namespace OwLib
{
	void UIXml::CreateMenuItem(IXMLDOMNode *node, MenuA *menu, MenuItemA *parentItem)
	{
		HRESULT hr;
		BSTR name;
		hr = node->get_nodeName(&name);
		String nodeName = name;
		if(name)
		{
			SysFreeString(name);
		}
		MenuItemA *item = new MenuItemA;
		long childCount = 0;
		item->SetNative(m_native);
		SetAttributesBefore(node, item);
		if (parentItem)
		{
			parentItem->AddItem(item);
		}
		else
		{
			menu->AddItem(item);
		}
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		int col = 0;
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			CreateMenuItem(spChildNode, menu, item);
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}
		SetAttributesAfter(node, item);
		OnAddControl(item, node);
	}

	void UIXml::CreateGridColumns(IXMLDOMNode *node, ControlA *control)
	{
		GridA *grid = dynamic_cast<GridA*>(control);
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String subNodeName = name;
			if(name)
			{
				SysFreeString(name);
			}
			ControlA *subControl = CreateControl(spChildNode, CStr::ToLower(subNodeName));
			GridColumn *column = dynamic_cast<GridColumn*>(subControl);
			if(column)
			{
				column->SetNative(m_native);
				grid->AddColumn(column);
			}
			SetAttributesBefore(spChildNode, column);
			ReadChildNodes(spChildNode, column);
			SetAttributesAfter(spChildNode, column);
			OnAddControl(column, spChildNode);
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}
	}

	void UIXml::CreateGridRows(IXMLDOMNode *node, ControlA *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String nodeName = name;
			if(name)
			{
				SysFreeString(name);
			}
			if(nodeName == L"row")
			{
				GridA *grid = dynamic_cast<GridA*>(control);
				GridRow *row = new GridRow;
				grid->AddRow(row);
				SetAttributesBefore(spChildNode, row);
				IXMLDOMNodeList *spChildNodeList2 = 0;
				hr = spChildNode->get_childNodes(&spChildNodeList2);
				hr = spChildNodeList->get_length(&childCount);
				int col = 0;
				for(long j = 0; j < childCount; j++)
				{
					IXMLDOMNode *spChildNode2 = 0;
					hr = spChildNodeList2->get_item(j, &spChildNode2);
					BSTR name2;
					hr = spChildNode2->get_nodeName(&name2);
					String subNodeName = name2;
					if(name2)
					{
						SysFreeString(name2);
					}
					BSTR value2;
					hr = spChildNode2->get_text(&value2);
					String subNodeValue = value2;
					if(value2)
					{
						SysFreeString(value2);
					}
					if(subNodeName == L"cell")
					{
						String cellType = L"string";
						map<String, String> attributes = GetAttributes(spChildNode2);
						if (attributes.find(L"type") != attributes.end())
						{
							cellType = attributes[L"type"];
						}
						GridCell *cell = 0;
						if (cellType == L"bool")
						{
							cell = new GridBoolCell;
						}
						else if (cellType == L"button")
						{
							cell = new GridButtonCell;
						}
						else if (cellType == L"checkbox")
						{
							cell = new GridCheckBoxCell;
						}
						else if (cellType == L"combobox")
						{
							cell = new GridComboBoxCell;
						}
						else if (cellType == L"double")
						{
							cell = new GridDoubleCell;
						}
						else if (cellType == L"float")
						{
							cell = new GridFloatCell;
						}
						else if (cellType == L"string")
						{
							cell = new GridStringCell;
						}
						else if (cellType == L"int")
						{
							cell = new GridIntCell;
						}
						else if (cellType == L"long")
						{
							cell = new GridLongCell;
						}
						else if (cellType == L"textbox")
						{
							cell = new GridTextBoxCell;
						}
						row->AddCell(col, cell);
						SetAttributesBefore(spChildNode2, cell);
						cell->SetString(subNodeValue);
						SetAttributesAfter(spChildNode2, cell);
						col++;
					}
					spChildNode2->Release();
				}
				spChildNodeList2->Release();
				SetAttributesAfter(spChildNode, row);
			}
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}
	}

	void UIXml::CreateSplitLayoutDiv(IXMLDOMNode *node, ControlA *control)
	{
		SplitLayoutDivA *splitLayoutDiv = new SplitLayoutDivA;
		splitLayoutDiv->SetNative(m_native);
        if (control)
        {
			control->AddControl(splitLayoutDiv);
        }
        else
        {
            m_native->AddControl(splitLayoutDiv);
        }
        SetAttributesBefore(node, splitLayoutDiv);
		int oldCount = (int)splitLayoutDiv->m_controls.size();
        ReadChildNodes(node, splitLayoutDiv);
		vector<ControlA*> newControls = splitLayoutDiv->m_controls;
		int newCount = (int)newControls.size();
        if (newCount - oldCount >= 2)
        {
            splitLayoutDiv->SetFirstControl(newControls[newCount - 2]);
            splitLayoutDiv->SetSecondControl(newControls[newCount - 1]);
        }
		SetAttributesAfter(node, splitLayoutDiv);
        splitLayoutDiv->Update();
		OnAddControl(splitLayoutDiv, node);
	}

	void UIXml::CreateTabPage(IXMLDOMNode *node, ControlA *control)
	{
		TabControlA *tabControl = dynamic_cast<TabControlA*>(control);
		if(tabControl)
		{
			TabPageA *tabPage = new TabPageA;
			tabPage->SetNative(m_native);
			tabControl->AddControl(tabPage);
			SetAttributesBefore(node, tabPage);
			ReadChildNodes(node, tabPage);
			SetAttributesAfter(node, tabPage);
			OnAddControl(tabPage, node);
		}
	}

	void UIXml::CreateTreeNode(IXMLDOMNode *node, ControlA *control, TreeNodeA *treeNode)
	{
	    TreeA *tree = dynamic_cast<TreeA*>(control);
        if (tree)
        {
			TreeNodeA *appendNode = new TreeNodeA;
            if (treeNode)
            {
				treeNode->AppendNode(appendNode);
            }
            else
            {
				tree->AppendNode(appendNode);
            }
            SetAttributesBefore(node, appendNode);
			HRESULT hr;
			long childCount = 0;
			IXMLDOMNodeList *spChildNodeList = 0;
			hr = node->get_childNodes(&spChildNodeList);
			hr = spChildNodeList->get_length(&childCount);
			for(long j = 0; j < childCount; j++)
			{
				IXMLDOMNode *spChildNode = 0;
				hr = spChildNodeList->get_item(j, &spChildNode);
				BSTR name;
				hr = spChildNode->get_nodeName(&name);
				String nodeName = name;
				if(name)
				{
					SysFreeString(name);
				}
				if(nodeName == L"node")
				{
					CreateTreeNode(spChildNode, control, appendNode);
				}
				if(spChildNode)
				{
					spChildNode->Release();
				}
			}
			if(spChildNodeList)
			{
				spChildNodeList->Release();
			}
			SetAttributesAfter(node, appendNode);
        }
	}

	void UIXml::CreateTreeNodes(IXMLDOMNode *node, ControlA *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			CreateTreeNode(spChildNode, control, 0);
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}
	}

	ControlA* UIXml::CreateUserControl(IXMLDOMNode *node)
	{
		ControlA *userControl = 0;
        map<String, String> attributes = GetAttributes(node);
		if (attributes.find(L"cid") != attributes.end())
        {
            userControl = CreateControl(node, attributes[L"cid"]);
        }
        if (userControl)
        {
			userControl->SetNative(m_native);
            return userControl;
        }
        else
        {
            return new ButtonA;
        }
	}

	void UIXml::dprintf( char * format, ...)
	{
		static char buf[1024];
		va_list args;
		va_start( args, format );
		vsprintf_s( buf, format, args );
		va_end( args);
		OutputDebugStringA( buf);
		printf("%s", buf);
	}

	IXMLDOMDocument* UIXml::DomFromCOM()
	{
	   HRESULT hr;
	   IXMLDOMDocument *pxmldoc = 0;
	 
	   HRCALL( CoCreateInstance(__uuidof(MSXML2::DOMDocument30),
					  NULL,
					  CLSCTX_INPROC_SERVER,
					  __uuidof(IXMLDOMDocument),
					  (void**)&pxmldoc),
					  "Create a new DOMDocument");
	 
		HRCALL( pxmldoc->put_async(VARIANT_FALSE),
				"should never fail");
		HRCALL( pxmldoc->put_validateOnParse(VARIANT_FALSE),
				"should never fail");
		HRCALL( pxmldoc->put_resolveExternals(VARIANT_FALSE),
				"should never fail");
	 
		return pxmldoc;
	clean:
		if (pxmldoc)
		{
			pxmldoc->Release();
		}
		return NULL;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////

	UIXml::UIXml()
	{
		m_event = new UIEvent(this);
		m_native = 0;
		m_script = new UIScript(this);
	}

	UIXml::~UIXml()
	{
		m_controls.clear();
		if(m_event)
		{
			delete m_event;
			m_event = 0;
		}
		m_native = 0;
		if(m_script)
		{
			delete m_script;
			m_script = 0;
		}
	}

	UIEvent* UIXml::GetEvent()
	{
		return m_event;
	}

	void UIXml::SetEvent(UIEvent *uiEvent)
	{
		m_event = uiEvent;
	}

	NativeBase* UIXml::GetNative()
	{
		return m_native;
	}

	void UIXml::SetNative(NativeBase *native)
	{
		m_native = native;
	}

	UIScript* UIXml::GetScript()
	{
		return m_script;
	}

	void UIXml::SetScript(UIScript *script)
	{
		m_script = script; 
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	ButtonA* UIXml::GetButton(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<ButtonA*>(control);
        }
        return 0;
	}

	ChartA* UIXml::GetChart(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<ChartA*>(control);
        }
        return 0;
	}

	CheckBoxA* UIXml::GetCheckBox(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<CheckBoxA*>(control);
        }
        return 0;
	}

	ComboBoxA* UIXml::GetComboBox(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<ComboBoxA*>(control);
        }
        return 0;
	}

	DatePickerA* UIXml::GetDatePicker(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<DatePickerA*>(control);
        }
        return 0;
	}

	DivA* UIXml::GetDiv(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<DivA*>(control);
        }
        return 0;
	}

	GridA* UIXml::GetGrid(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<GridA*>(control);
        }
        return 0;
	}

	GroupBoxA* UIXml::GetGroupBox(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<GroupBoxA*>(control);
        }
        return 0;
	}

	LabelA* UIXml::GetLabel(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<LabelA*>(control);
        }
        return 0;
	}

	LayoutDivA* UIXml::GetLayoutDiv(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<LayoutDivA*>(control);
        }
        return 0;
	}

	vector<ControlA*> UIXml::GetLikeControls(const String& name)
	{
		vector<ControlA*> controls;
		vector<ControlA*>::iterator sIter = m_controls.begin();
		for(; sIter != m_controls.end(); ++sIter)
		{
			ControlA *control = *sIter;
			if(control->GetName().find(name) != -1)
			{
				controls.push_back(control);
			}
		}
        return controls;
	}

	MenuA* UIXml::GetMenu(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<MenuA*>(control);
        }
        return 0;
	}

	MenuItemA* UIXml::GetMenuItem(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<MenuItemA*>(control);
        }
        return 0;
	}

	RadioButtonA* UIXml::GetRadioButton(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<RadioButtonA*>(control);
        }
        return 0;
	}

	SpinA* UIXml::GetSpin(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<SpinA*>(control);
        }
        return 0;
	}

	SplitLayoutDivA* UIXml::GetSplitLayoutDiv(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<SplitLayoutDivA*>(control);
        }
        return 0;
	}

	TabControlA* UIXml::GetTabControl(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<TabControlA*>(control);
        }
        return 0;
	}

	TableLayoutDivA* UIXml::GetTableLayoutDiv(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<TableLayoutDivA*>(control);
        }
        return 0;
	}

	TabPageA* UIXml::GetTabPage(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<TabPageA*>(control);
        }
        return 0;
	}

	TextBoxA* UIXml::GetTextBox(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<TextBoxA*>(control);
        }
        return 0;
	}

	TreeA* UIXml::GetTree(const String& name)
	{
		ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<TreeA*>(control);
        }
        return 0;
	}

	WindowA* UIXml::GetWindow(const String& name)
	{
	    ControlA *control = FindControl(name);
        if (control)
        {
            return dynamic_cast<WindowA*>(control);
        }
        return 0;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool UIXml::ContainsControl(ControlA *control)
	{
		vector<ControlA*>::iterator sIter = m_controls.begin();
		for(; sIter != m_controls.end(); ++sIter)
		{
			if(control == *sIter)
			{
				return true;
			}
		}
		return false;
	}

	ControlA* UIXml::CreateControl(IXMLDOMNode *node, const String& type)
	{
		if(type == L"bandedgrid")
		{
			return new BandedGridA;
		}
		else if (type == L"button")
        {
            return new ButtonA;
        }
        else if (type == L"calendar")
        {
            return new CalendarA;
        }
        else if (type == L"chart")
        {
            if (m_native)
            {
                return new ChartA;
            }
            else
            {
                return 0;
            }
        }
        else if (type == L"checkbox")
        {
            return new CheckBoxA;
        }
		else if (type == L"column")
        {
            return new GridColumn;
        }
        else if (type == L"combobox")
        {
            return new ComboBoxA;
        }
        else if (type == L"datepicker")
        {
            return new DatePickerA;
        }
		else if(type == L"div")
		{
			return new DivA;
		}
		if (type == L"grid")
        {
            return new GridA;
        }
		else if(type == L"groupbox")
		{
			return new GroupBoxA;
		}
        else if (type == L"label")
        {
            return new LabelA;
        }
		else if(type == L"layoutdiv")
		{
			return new LayoutDivA;
		}
		else if(type == L"linklabel")
		{
			return new LinkLabelA;
		}
		else if(type == L"menu")
		{
			return new MenuA;
		}
        else if (type == L"radiobutton")
        {
            return new RadioButtonA;
        }
        else if (type == L"spin")
        {
            return new SpinA;
        }
        else if (type == L"tabcontrol")
        {
            return new TabControlA;
        }
		else if(type == L"tablelayoutdiv")
		{
			return new TableLayoutDivA;
		}
        else if (type == L"textbox")
        {
            return new TextBoxA;
        }
        else if (type == L"tree")
        {
            return new TreeA;
        }
		else if(type == L"usercontrol")
		{
			return CreateUserControl(node);
		}
		else if (type == L"window")
        {
            return new WindowA;
        }
        else
        {
            return 0;
        }
	}

	void UIXml::CreateNative()
	{
		m_native = new NativeBase;
	}

	void UIXml::CreateSubProperty(IXMLDOMNode *node, ControlA *control)
	{
		BSTR name;
		HRESULT hr;
		hr = node->get_nodeName(&name);
		String strName = name;
		if(name)
		{
			SysFreeString(name);
		}
		String controlType = L"";
		if(control)
		{
			controlType = control->GetControlType();
		}
		if (strName == L"columns")
        {
            if (controlType == L"Grid" || controlType == L"Tree")
            {
				CreateGridColumns(node, control);
            }
        }
		else if(strName == L"item")
		{
			if(controlType == L"ComboBox")
			{
				ComboBoxA *comboBox = dynamic_cast<ComboBoxA*>(control);
				if(comboBox)
				{
					CreateMenuItem(node, comboBox->GetDropDownMenu(), 0);
				}
			}
			else if (controlType == L"Menu")
            {
                MenuA *menu = dynamic_cast<MenuA*>(control);
                if (menu)
                {
                    CreateMenuItem(node, menu, 0);
                }
            }
		}
		else if(strName == L"nodes")
		{
			if(controlType == L"Tree")
			{
				CreateTreeNodes(node, control);
			}
		}
		else if (strName == L"rows")
        {
            if (controlType == L"Grid")
            {
				CreateGridRows(node, control);
            }
        }
		else if(strName == L"splitlayoutdiv")
		{
			CreateSplitLayoutDiv(node, control);
		}
		else if(strName == L"tabpage")
		{
			if(controlType == L"TabControl")
			{
				CreateTabPage(node, control);
			}
		}
	}

	ControlA* UIXml::FindControl(const String& name)
	{
		vector<ControlA*>::iterator sIter = m_controls.begin();
		for(; sIter != m_controls.end(); ++sIter)
		{
			ControlA *control = *sIter;
			if(control->GetName() == name)
			{
				return control;
			}
		}
		return 0;
	}

	map<String, String> UIXml::GetAttributes(IXMLDOMNode *node)
	{
		map<String, String> matrs;
		IXMLDOMAttribute *attribute = 0;
		IXMLDOMNamedNodeMap *attributes = 0;
		HRESULT hr;
		hr = node->get_attributes(&attributes);
		long mCount = 0;
		hr = attributes->get_length(&mCount);
		for(long j = 0; j < mCount; ++j)
		{
			IXMLDOMNode *atrNode = 0;
			hr = attributes->get_item(j, &atrNode);
			BSTR name;
			hr = atrNode->get_nodeName(&name);
			String strName = name;
			if(name)
			{
				SysFreeString(name);
			}
			BSTR value;
			hr = atrNode->get_text(&value);
			String strValue = value;
			if(value)
			{
				SysFreeString(value);
			}
			matrs[strName] = strValue;
			if(atrNode)
			{
				atrNode->Release();
			}
		}
		if(attributes)
		{
			attributes->Release();
		}
		return matrs;
	}

	vector<ControlA*> UIXml::GetControls()
	{
		vector<ControlA*> controls;
		if(m_native)
		{
			controls = m_native->m_controls;
		}
		return controls;
	}

	bool UIXml::IsAfterSetingAttribute(const String& name)
	{
	    if (name == L"selectedindex"
        || name == L"selectedtext"
        || name == L"selectedvalue"
		|| name == L"value")
        {
            return true;
        }
        else
        {
            return false;
        }
	}

	void UIXml::LoadFile(const String& fileName, ControlA *control)
	{
		m_controls.clear();
		IXMLDOMDocument *pXMLDom = 0;
		IXMLDOMParseError *pXMLErr = 0;
		IXMLDOMElement *spElement = 0;
		IXMLDOMNodeList *spNodeList = 0;
		BSTR bstr = 0;
		VARIANT_BOOL status;
		VARIANT var;
		HRESULT hr;
		long lCount = 0;
		pXMLDom = DomFromCOM();
		if (!pXMLDom) goto clean;
		VariantInit(&var);
		V_BSTR(&var) = SysAllocString(fileName.c_str());
		V_VT(&var) = VT_BSTR;
		HRCALL(pXMLDom->load(var, &status), "");
		if (status != VARIANT_TRUE) {
			HRCALL(pXMLDom->get_parseError(&pXMLErr), "");
			HRCALL(pXMLErr->get_reason(&bstr), "");
			goto clean;
		}
		HRCALL(pXMLDom->get_xml(&bstr), "");
		hr = pXMLDom->get_documentElement(&spElement);
		hr = spElement->get_childNodes(&spNodeList); 
		hr = spNodeList->get_length(&lCount);
		for (long i = 0; i < lCount; ++i)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spNodeList->get_item(i, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String nodeName = name;
			if(name)
			{
				SysFreeString(name);
			}
			if(nodeName == L"body")
			{
				ReadBody(spChildNode, control);
			}
			else if(nodeName == L"head")
			{
				ReadHead(spChildNode, control);
			}
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		clean:
		if (bstr) SysFreeString(bstr);
		if (&var) VariantClear(&var);
		if (pXMLErr) pXMLErr->Release();
		if (pXMLDom) pXMLDom->Release();
		if (spElement) spElement->Release();
		if (spNodeList) spNodeList->Release();
	}

	void UIXml::ReadBody(IXMLDOMNode *node, ControlA *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			ReadNode(spChildNode, control);
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}		
	}

	void UIXml::OnAddControl(ControlA *control, IXMLDOMNode *node)
	{
		m_controls.push_back(control);
		SetEvents(node, control);
		m_event->CallFunction(control, EVENTID::LOAD, m_event);
	}

	void UIXml::ReadChildNodes(IXMLDOMNode *node, ControlA *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			ReadNode(spChildNode, control);
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}
	}

	void UIXml::ReadHead(IXMLDOMNode *node, ControlA *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++)
		{
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String nodeName = name;
			if(name)
			{
				SysFreeString(name);
			}
			if(nodeName == L"script")
			{
				BSTR value;
				hr = spChildNode->get_text(&value);
				String nodeValue = value;
				m_script->SetText(nodeValue);
				if(value)
				{
					SysFreeString(value);
				}
			}			
			if(spChildNode)
			{
				spChildNode->Release();
			}
		}
		if(spChildNodeList)
		{
			spChildNodeList->Release();
		}		
	}
	
	ControlA* UIXml::ReadNode(IXMLDOMNode *node, ControlA* parent)
	{
		HRESULT hr;
		BSTR name;
		hr = node->get_nodeName(&name);
		String nodeName = name;
		if(name)
		{
			SysFreeString(name);
		}
		ControlA *control = CreateControl(node, nodeName);
		if(control)
		{
			control->SetNative(m_native);
            if (parent)
            {
                parent->AddControl(control);
            }
            else
            {
                m_native->AddControl(control);
            }
			SetAttributesBefore(node, control);
			ReadChildNodes(node, control);
			SetAttributesAfter(node, control);
			control->Update();
			OnAddControl(control, node);
		}
		else
		{
			CreateSubProperty(node, parent);
		}
		return control;
	}

	void UIXml::SetAttributesAfter(IXMLDOMNode *node, CProperty *control)
	{
		map<String, String> attributes = GetAttributes(node);
		map<String, String>::iterator sIter = attributes.begin();
		for(; sIter != attributes.end(); ++sIter)
		{
			if(IsAfterSetingAttribute(sIter->first))
			{
				control->SetProperty(CStr::ToLower(sIter->first), sIter->second);
			}
		}
		attributes.clear();
	}

	void UIXml::SetAttributesBefore(IXMLDOMNode *node, CProperty *control)
	{
		map<String, String> attributes = GetAttributes(node);
		map<String, String>::iterator sIter = attributes.begin();
		for(; sIter != attributes.end(); ++sIter)
		{
			if(!IsAfterSetingAttribute(sIter->first))
			{
				control->SetProperty(CStr::ToLower(sIter->first), sIter->second);
			}
		}
		attributes.clear();
	}

	void UIXml::SetEvents(IXMLDOMNode *node, CProperty *control)
	{
		ControlA *baseControl = dynamic_cast<ControlA*>(control);
		if(baseControl)
		{
			map<String, String> attributes = GetAttributes(node);
			map<String, String>::iterator sIter = attributes.begin();
			for(; sIter != attributes.end(); ++sIter)
			{
				m_event->RegisterEvent(baseControl, sIter->first, sIter->second);
			}
		}
	}
}