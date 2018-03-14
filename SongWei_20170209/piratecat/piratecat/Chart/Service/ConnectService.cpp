#include "..\\..\\stdafx.h"
#include "..\\..\\..\\includes\\Sock\\CStr.h"
#include "ConnectService.h"
#include "..\\..\\..\\third_party\\SQLite\sqlite3x.hpp"
using namespace sqlite3x;

ConnectInfo::ConnectInfo()
{
	m_id = L"";
	m_ip = L"";
	m_port = 0;
	m_type = L"";
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ConnectService::ConnectService()
{
	m_createTableSQL = "CREATE TABLE CONNECTS(ID PRIMARY KEY, IP, PORT INTEGER, TYPE)";
	string dataPath = DataCenter::GetUserPath() + "\\data";
	if(!CFileA::IsDirectoryExist(dataPath.c_str()))
	{
		CFileA::CreateDirectory(dataPath.c_str());
	}
	string path = dataPath + "\\connects.db";
	CStrA::stringTowstring(m_dataBasePath, path);
	bool createDataBase = !CFileA::IsFileExist(path.c_str());
	if(createDataBase)
	{
		CreateTable();
	}
}

ConnectService::~ConnectService()
{
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int ConnectService::AddConnect(ConnectInfo *connectInfo)
{
	int strLen = sizeof(int) + CStrA::GetWStringCapacity(connectInfo->m_type) + CStrA::GetWStringCapacity(connectInfo->m_id) + CStrA::GetWStringCapacity(connectInfo->m_ip) + 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"INSERT INTO CONNECTS(ID, IP, PORT, TYPE) values ('%s','%s', %d, '%s')"
		, CStrA::GetDBString(connectInfo->m_id).c_str(), CStrA::GetDBString(connectInfo->m_ip).c_str(), connectInfo->m_port, CStrA::GetDBString(connectInfo->m_type).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	return 1;
}

void ConnectService::CreateDefaultConnects()
{	
	String guid = L"";
	CStrA::stringTowstring(guid, CStrA::GetGuid());
	ConnectInfo connectInfo1;
	connectInfo1.m_id = guid;
	connectInfo1.m_ip = L"114.55.4.91";
	connectInfo1.m_port = 9961;
	connectInfo1.m_type = L"Ö÷·þÎñÆ÷";
	AddConnect(&connectInfo1);
}

void ConnectService::CreateTable()
{
	const wchar_t *strPath = m_dataBasePath.c_str();
	sqlite3_connection conn(strPath);
	conn.executenonquery(m_createTableSQL.c_str());
	conn.close();
	CreateDefaultConnects();
}

int ConnectService::GetConnects(vector<ConnectInfo> *connectInfos)
{
	int strLen = 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, 0, strLen * sizeof(wchar_t));
	_stprintf_s(sql, strLen - 1, L"SELECT * FROM CONNECTS");
	sqlite3_connection conn(m_dataBasePath.c_str());
	sqlite3_command cmd(conn, sql);
	sqlite3_reader reader = cmd.executereader();
	while(reader.read())
	{
		ConnectInfo connectInfo;
		connectInfo.m_id = reader.getstring16(0);
		connectInfo.m_ip = reader.getstring16(1);
		connectInfo.m_port = reader.getint(2);
		connectInfo.m_type = reader.getstring16(3);
		connectInfos->push_back(connectInfo);
	}
	reader.close();
	delete[] sql;
	sql = 0;
	return 1;
}

int ConnectService::UpdateConnect(ConnectInfo *connectInfo)
{
	int strLen = sizeof(int) + CStrA::GetWStringCapacity(connectInfo->m_type) + CStrA::GetWStringCapacity(connectInfo->m_id) + CStrA::GetWStringCapacity(connectInfo->m_ip) + 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"UPDATE CONNECTS SET IP = '%s', PORT = %d, TYPE = '%s' WHERE ID = '%s'"
		, CStrA::GetDBString(connectInfo->m_ip).c_str(), connectInfo->m_port, CStrA::GetDBString(connectInfo->m_type).c_str(), CStrA::GetDBString(connectInfo->m_id).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	return 1;
}