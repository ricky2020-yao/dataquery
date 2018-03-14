#include "..\\..\\stdafx.h"
#include "UserSessionServiceEx.h"

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

UserSessionServiceEx::UserSessionServiceEx()
{
	m_getListRequestID = BaseService::GetRequestID();
	m_loaded = false;
	m_operatorRequestID = BaseService::GetRequestID();
}

UserSessionServiceEx::~UserSessionServiceEx()
{
	m_lock.Lock();
	m_sessions.clear();
	m_lock.UnLock();
}

int UserSessionServiceEx::GetListRequestID()
{
	return m_getListRequestID;
}

bool UserSessionServiceEx::IsLoaded()
{
	return m_loaded;
}

int UserSessionServiceEx::GetOperatorRequestID()
{
	return m_operatorRequestID;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int UserSessionServiceEx::GetSession(String key, UserSession *session)
{
	int state = 0;
	m_lock.Lock();
	map<String, UserSession>::iterator sIter  = m_sessions.find(key);
	if(sIter != m_sessions.end())
	{
		*session = sIter->second;
		state = 1;
	}
	m_lock.UnLock();
	return state;
}

void UserSessionServiceEx::OnReceive(CMessage *message)
{
    vector<UserSession> sessions;
	UserSessionService::GetSessions(&sessions, message->m_body, message->m_bodyLength);
    int sessionsSize = (int)sessions.size();
	switch (message->m_functionID)
    {
		case FUNCTIONID_USERSESSION_GETSESSIONS:
			{
				m_lock.Lock();
				for (int i = 0; i < sessionsSize; i++)
				{
					UserSession session = sessions[i];
					m_sessions[session.m_key] = session;
					OnSessionChanged(&session);
				}
				m_lock.UnLock();
				m_loaded = true;
				break;
			}
        case FUNCTIONID_USERSESSION_ADDSESSIONS:
            {
				m_lock.Lock();
                for (int i = 0; i < sessionsSize; i++)
                {
					UserSession session = sessions[i];
					m_sessions[session.m_key] = session;
					OnSessionChanged(&session);
                }
				m_lock.UnLock();
                break;
            }
        case FUNCTIONID_USERSESSION_DELETESESSIONS:
            {
				m_lock.Lock();
                for (int i = 0; i < sessionsSize; i++)
                {
					UserSession session = sessions[i];
					map<String, UserSession>::iterator sIter  = m_sessions.find(session.m_key);
					if(sIter != m_sessions.end())
					{
						m_sessions.erase(sIter);
					}
					session.m_value = L"";
					OnSessionChanged(&session);
                }
				m_lock.UnLock();
                break;
            }
    }
	UserSessionService::OnReceive(message);
}

void UserSessionServiceEx::OnSessionChanged(UserSession *session)
{
}
