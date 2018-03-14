#include "..\\..\\stdafx.h"
#include "UserCookieService.h"
#include "..\\..\\..\\third_party\\SQLite\sqlite3x.hpp"
using namespace sqlite3x;

UserCookie::UserCookie()
{
	m_key = L"";
	m_userID = 0;
	m_value = L"";
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

UserCookieService::UserCookieService()
{
	string dataPath = DataCenter::GetUserPath() + "\\data";
	if(!CFileA::IsDirectoryExist(dataPath.c_str()))
	{
		CFileA::CreateDirectory(dataPath.c_str());
	}
	m_createTableSQL = "CREATE TABLE USERCOOKIE(USERID INTEGER, KEY, VALUE, MODIFYTIME DATE, CREATETIME DATE)";
	string dataBasePath = DataCenter::GetUserPath() + "\\data\\usercookies.db";
	CStrA::stringTowstring(m_dataBasePath, dataBasePath);
	m_userID = 0;
	if(!CFileA::IsFileExist(dataBasePath.c_str()))
	{
		CreateTable();
	}
}

UserCookieService::~UserCookieService()
{
}

int UserCookieService::GetUserID()
{
	return m_userID;
}

void UserCookieService::SetUserID(int userID)
{
	m_userID = userID;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void UserCookieService::CreateTable()
{
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(m_createTableSQL.c_str());
	conn.close();
}

int UserCookieService::AddCookie(UserCookie *cookie)
{
	UserCookie oldCookie;
	if(GetCookie(cookie->m_key, &oldCookie))
	{
		UpdateCookie(cookie);
	}
	else
	{
		int iLen = sizeof(int) + CStrA::GetWStringCapacity(cookie->m_key) + 200;
		wchar_t *sql = new wchar_t[iLen];
		memset(sql, 0, iLen * sizeof(wchar_t));
		_stprintf_s(sql, iLen - 1, L"INSERT INTO USERCOOKIE(USERID, KEY, VALUE, MODIFYTIME, CREATETIME) values (%d, '%s', '%s', '1970-1-1','1970-1-1')",
			m_userID, CStrA::GetDBString(cookie->m_key).c_str(), CStrA::GetDBString(cookie->m_value).c_str());
		sqlite3_connection conn(m_dataBasePath.c_str());
		conn.executenonquery(sql);
		conn.close();
		delete[] sql;
		sql = 0;
	}
	return 1;
}

int UserCookieService::DeleteCookie(String key)
{
	int iLen = sizeof(int)  + CStrA::GetWStringCapacity(key) + 200;
	wchar_t *sql = new wchar_t[iLen];
	memset(sql, 0, iLen * sizeof(wchar_t));
	_stprintf_s(sql, iLen - 1, L"DELETE FROM USERCOOKIE WHERE USERID = %d AND KEY = '%s'", m_userID, CStrA::GetDBString(key).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	sql = 0;
	return 1;
}

int UserCookieService::GetCookie(String key, UserCookie *cookie)
{
	int state = 0;
	int iLen = sizeof(int) + CStrA::GetWStringCapacity(cookie->m_key) + 200;
	wchar_t *sql = new wchar_t[iLen];
	memset(sql, 0, iLen * sizeof(wchar_t));
	_stprintf_s(sql, iLen - 1, L"SELECT * FROM USERCOOKIE WHERE USERID = %d AND KEY = '%s'", m_userID, CStrA::GetDBString(key).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	sqlite3_command cmd(conn, sql);
	sqlite3_reader reader = cmd.executereader();
	while(reader.read())
	{
		cookie->m_userID = reader.getint(0);
		cookie->m_key = reader.getstring16(1);
		cookie->m_value = reader.getstring16(2);
		state = 1;
	}
	reader.close();
	delete[] sql;
	sql = 0;
	return state;
}

void UserCookieService::UpdateCookie(UserCookie *cookie)
{
	int iLen = sizeof(int) + CStrA::GetWStringCapacity(cookie->m_key) + CStrA::GetWStringCapacity(cookie->m_value) + 200;
	wchar_t *sql = new wchar_t[iLen];
	memset(sql, 0, iLen * sizeof(wchar_t));
	_stprintf_s(sql, iLen - 1, L"UPDATE USERCOOKIE SET VALUE = '%s' WHERE USERID = %d AND KEY = '%s'",
		CStrA::GetDBString(cookie->m_value).c_str(), m_userID, CStrA::GetDBString(cookie->m_key).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	sql = 0;
}