#include "..\\..\\stdafx.h"
#include "SecurityFilterServiceEx.h"

SecurityFilterServiceEx::SecurityFilterServiceEx()
{
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
	m_operatorRequestID = BaseService::GetRequestID();
}

SecurityFilterServiceEx::~SecurityFilterServiceEx()
{
	vector<SecurityFilterTask*>::iterator sIter = m_tasks.begin();
	for(; sIter != m_tasks.end(); ++sIter)
	{
		delete *sIter;
	}
	m_tasks.clear();
}

int SecurityFilterServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

bool SecurityFilterServiceEx::IsLoaded()
{
	return m_loaded;
}

int SecurityFilterServiceEx::GetOperatorRequestID()
{
	return m_operatorRequestID;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

SecurityFilterTask* SecurityFilterServiceEx::AddTask(SecurityFilterTemplate sfTemplate, NativeBase *native)
{
	vector<SecurityFilterTask*>::iterator sIter = m_tasks.begin();
	for(; sIter != m_tasks.end(); ++sIter)
	{
		SecurityFilterTask *task = *sIter;
		if(task->GetTemplate().m_templateID == sfTemplate.m_templateID)
		{
			return 0;
		}
	}
    SecurityFilterTask *task = new SecurityFilterTask;
    task->SetNative (native);
    task->SetTemplate(sfTemplate);
    m_tasks.push_back(task);
    return task;
}

SecurityFilterTask* SecurityFilterServiceEx::GetTask(const String&  templateID)
{
	vector<SecurityFilterTask*>::iterator sIter = m_tasks.begin();
	for(; sIter != m_tasks.end(); ++sIter)
	{
		SecurityFilterTask *task = *sIter;
		if(task->GetTemplate().m_templateID == templateID)
		{
			return task;
		}
	}
	return 0;
}

int SecurityFilterServiceEx::GetTemplate(const String& templateID, SecurityFilterTemplate *sfTemplate)
{
	vector<SecurityFilterTemplate>::iterator sIter = m_templates.begin();
	for(; sIter != m_templates.end(); ++sIter)
	{
		if((*sIter).m_templateID == templateID)
		{
			*sfTemplate = *sIter;
			return 1;
		}
	}
	return 0;
}

int SecurityFilterServiceEx::GetTemplates(vector<SecurityFilterTemplate> *templates)
{
	vector<SecurityFilterTemplate>::iterator sIter = m_templates.begin();
	for(; sIter != m_templates.end(); ++sIter)
	{
		templates->push_back(*sIter);
	}
	return 1;
}

void SecurityFilterServiceEx::OnReceive(CMessage *message)
{
    if (message->m_functionID != FUNCTIONID_SECURITYFILTER_STARTFILTER
    && message->m_functionID != FUNCTIONID_SECURITYFILTER_STOPFILTER)
    {
		if(message->m_functionID != FUNCTIONID_SECURITYFILTER_GETTEMPLATES)
		{
			message->m_requestID = m_operatorRequestID;
		}
		vector<SecurityFilterTemplate> templates;
		SecurityFilterService::GetTemplates(&templates, message->m_body, message->m_bodyLength);
		int templatesSize = (int)templates.size();
		switch (message->m_functionID)
		{
			case FUNCTIONID_SECURITYFILTER_GETTEMPLATES:
				{
					m_templates = templates;
					m_loaded = true;
					break;
				}
			case FUNCTIONID_SECURITYFILTER_ADDTEMPLATES:
				{
					bool add = false;
					for (int i = 0; i < templatesSize; i++)
					{
						bool has = false;
						vector<SecurityFilterTemplate>::iterator sIter = m_templates.begin();
						for(; sIter != m_templates.end(); ++sIter)
						{
							if((*sIter).m_templateID == templates[i].m_templateID)
							{
								has = true;
							}
							break;
						}
						if(has)
						{
							m_templates.push_back(templates[i]);
							add = true;
						}
					}
					if(!add)
					{
						return;
					}
					break;
				}
			case FUNCTIONID_SECURITYFILTER_DELETETEMPLATES:
				{
					for (int i = 0; i < templatesSize; i++)
					{
						vector<SecurityFilterTemplate>::iterator sIter = m_templates.begin();
						for(; sIter != m_templates.end(); ++sIter)
						{
							if((*sIter).m_templateID == templates[i].m_templateID)
							{
								m_templates.erase(sIter);
								break;
							}
						}
					}
					break;
				}
			case FUNCTIONID_SECURITYFILTER_UPDATETEMPLATES:
				{
					for (int i = 0; i < templatesSize; i++)
					{
						SecurityFilterTemplate updateTemplate = templates[i];
						int curTemplatesSize = (int)m_templates.size();
						for(int j = 0; j < curTemplatesSize; j++)
						{
							SecurityFilterTemplate sfTemplate = m_templates[j];
							if(sfTemplate.m_templateID == updateTemplate.m_templateID)
							{
								m_templates[j] =  updateTemplate;
								break;
							}
						}
					}
					break;
				}
		}
	}
	SecurityFilterService::OnReceive(message);
}

int SecurityFilterServiceEx::RemoveTask(const String&  templateID)
{
	vector<SecurityFilterTask*>::iterator sIter = m_tasks.begin();
	for(; sIter != m_tasks.end(); ++sIter)
	{
		SecurityFilterTask *task = *sIter;
		if(task->GetTemplate().m_templateID == templateID)
		{
			m_tasks.erase(sIter);
			delete task;
			return 1;
		}
	}
	return 0;
}
