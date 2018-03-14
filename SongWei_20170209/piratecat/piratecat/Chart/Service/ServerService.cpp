#include "..\\..\\stdafx.h"
#include "..\\..\\..\\includes\\Sock\\CStr.h"
#include "ServerService.h"
#include "..\\..\\..\\third_party\\SQLite\sqlite3x.hpp"
using namespace sqlite3x;
#include "DataCenter.h"

ServerInfo::ServerInfo()
{
	m_id = L"";
	m_ip= L"";
	m_port = 0;
	m_type = L"";
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

ServerService::ServerService()
{
	m_createTableSQL = "CREATE TABLE SERVERS(ID PRIMARY KEY, IP, PORT INTEGER, TYPE)";
	string dataPath = DataCenter::GetUserPath() + "\\data";
	if(!CFileA::IsDirectoryExist(dataPath.c_str()))
	{
		CFileA::CreateDirectory(dataPath.c_str());
	}
	string path = dataPath + "\\servers.db";
	CStrA::stringTowstring(m_dataBasePath, path);
	bool createDataBase = !CFileA::IsFileExist(path.c_str());
	if(createDataBase)
	{
		CreateTable();
	}
}

ServerService::~ServerService()
{
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int ServerService::AddServer(ServerInfo *serverInfo)
{
	int strLen = sizeof(int) + CStrA::GetWStringCapacity(serverInfo->m_type) + CStrA::GetWStringCapacity(serverInfo->m_id) + CStrA::GetWStringCapacity(serverInfo->m_ip) + 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"INSERT INTO SERVERS(ID, IP, PORT, TYPE) values ('%s','%s', %d, '%s')"
		, CStrA::GetDBString(serverInfo->m_id).c_str(), CStrA::GetDBString(serverInfo->m_ip).c_str(), serverInfo->m_port, CStrA::GetDBString(serverInfo->m_type).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	return 1;
}

void ServerService::CreateDefaultServers()
{
	String guid = L"";
	CStrA::stringTowstring(guid, CStrA::GetGuid());
	ServerInfo server1;
	server1.m_id = guid;
	server1.m_ip = L"114.55.4.91";
	server1.m_port = 9961;
	server1.m_type = L"Ö÷·þÎñÆ÷";
	AddServer(&server1);
}

void ServerService::CreateTable()
{
	const wchar_t *strPath = m_dataBasePath.c_str();
	sqlite3_connection conn(strPath);
	conn.executenonquery(m_createTableSQL.c_str());
	conn.close();
	CreateDefaultServers();
}

int ServerService::DeleteServer(const String& id)
{
	int strLen = CStrA::GetWStringCapacity(id) + 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"DELETE FROM SERVERS WHERE ID = '%s'", CStrA::GetDBString(id).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
	return 1;
}

int ServerService::GetServers(vector<ServerInfo> *serverInfos)
{
	int strLen = 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, 0, strLen * sizeof(wchar_t));
	_stprintf_s(sql, strLen - 1, L"SELECT * FROM SERVERS");
	sqlite3_connection conn(m_dataBasePath.c_str());
	sqlite3_command cmd(conn, sql);
	sqlite3_reader reader = cmd.executereader();
	while(reader.read())
	{
		ServerInfo serverInfo;
		serverInfo.m_id = reader.getstring16(0);
		serverInfo.m_ip = reader.getstring16(1);
		serverInfo.m_port = reader.getint(2);
		serverInfo.m_type = reader.getstring16(3);
		serverInfos->push_back(serverInfo);
	}
	reader.close();
	delete[] sql;
	return 1;
}

void ServerService::UpdateServer(ServerInfo *serverInfo)
{
	int strLen = sizeof(int) + CStrA::GetWStringCapacity(serverInfo->m_type) + CStrA::GetWStringCapacity(serverInfo->m_id) + CStrA::GetWStringCapacity(serverInfo->m_ip) + 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, '\0', strLen);
	_stprintf_s(sql, strLen - 1, L"UPDATE SERVERS SET IP = '%s', PORT = %d, TYPE = '%s' WHERE ID = '%s'"
		, CStrA::GetDBString(serverInfo->m_ip).c_str(), serverInfo->m_port, CStrA::GetDBString(serverInfo->m_type).c_str(), CStrA::GetDBString(serverInfo->m_id).c_str());
	sqlite3_connection conn(m_dataBasePath.c_str());
	conn.executenonquery(sql);
	conn.close();
	delete[] sql;
}