#include "..\\..\\stdafx.h"
#include "IndicatorLayoutServiceEx.h"

IndicatorLayoutServiceEx::IndicatorLayoutServiceEx()
{
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
	m_operatorRequestID = BaseService::GetRequestID();
}

IndicatorLayoutServiceEx::~IndicatorLayoutServiceEx()
{
}

int IndicatorLayoutServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

bool IndicatorLayoutServiceEx::IsLoaded()
{
	return m_loaded;
}

int IndicatorLayoutServiceEx::GetOperatorRequestID()
{
	return m_operatorRequestID;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

bool IndicatorLayoutServiceEx::GetLayoutByID(const String& layoutID, IndicatorLayout *layout)
{
	vector<IndicatorLayout>::iterator sIter = m_layouts.begin();
	for(; sIter != m_layouts.end(); ++sIter)
	{
		if((*sIter).m_layoutID == layoutID)
		{
			*layout = *sIter;
			return true;
		}
	}
	return false;
}

bool IndicatorLayoutServiceEx::GetLayoutByName(const String& layoutName, IndicatorLayout *layout)
{
	vector<IndicatorLayout>::iterator sIter = m_layouts.begin();
	for(; sIter != m_layouts.end(); ++sIter)
	{
		if((*sIter).m_name == layoutName)
		{
			*layout = *sIter;
			return true;
		}
	}
	return false;
}

bool IndicatorLayoutServiceEx::GetLayouts(vector<IndicatorLayout> *layouts)
{
	vector<IndicatorLayout>::iterator sIter = m_layouts.begin();
	for(; sIter != m_layouts.end(); ++sIter)
	{
		layouts->push_back(*sIter);
	}
	return true;
}

void IndicatorLayoutServiceEx::OnReceive(CMessage *message)
{
	if(message->m_functionID != FUNCTIONID_INDICATORLAYOUT_GETLAYOUTS)
	{
		message->m_requestID = m_operatorRequestID;
	}
    vector<IndicatorLayout> layouts;
	IndicatorLayoutService::GetLayouts(&layouts, message->m_body, message->m_bodyLength);
    int layoutsSize = (int)layouts.size();
	switch (message->m_functionID)
    {
		case FUNCTIONID_INDICATORLAYOUT_GETLAYOUTS:
			{
				m_layouts = layouts;
				m_loaded = true;
				break;
			}
        case FUNCTIONID_INDICATORLAYOUT_ADDLAYOUTS:
            {
				bool add = false;
                for (int i = 0; i < layoutsSize; i++)
                {
					bool has = false;
					vector<IndicatorLayout>::iterator sIter = m_layouts.begin();
					for(; sIter != m_layouts.end(); ++sIter)
					{
						if((*sIter).m_layoutID == layouts[i].m_layoutID)
						{
							has = true;
						}
					}
					if(!has)
					{
						m_layouts.push_back(layouts[i]);
						add = true;
					}
                }
				if(!add)
				{
					return;
				}
                break;
            }
        case FUNCTIONID_INDICATORLAYOUT_DELETELAYOUTS:
            {
                for (int i = 0; i < layoutsSize; i++)
                {
					vector<IndicatorLayout>::iterator sIter = m_layouts.begin();
					for(; sIter != m_layouts.end(); ++sIter)
					{
						if((*sIter).m_layoutID == layouts[i].m_layoutID)
						{
							m_layouts.erase(sIter);
							break;
						}
					}
                }
                break;
            }
        case FUNCTIONID_INDICATORLAYOUT_UPDATELAYOUTS:
            {
                for (int i = 0; i < layoutsSize; i++)
                {
                    IndicatorLayout updateLayout = layouts[i];
					int curLayoutsSize = (int)m_layouts.size();
                    for (int j = 0; j < curLayoutsSize; j++)
                    {
                        IndicatorLayout layout = m_layouts[j];
                        if (layout.m_layoutID == updateLayout.m_layoutID)
                        {
                            m_layouts[j] = updateLayout;
                            break;
                        }
                    }
                }
                break;
            }
    }
	IndicatorLayoutService::OnReceive(message);
}
