/*******************************************************************************\
*                                                                               *
* ServerService.cs - Server service functions, types, and definitions.          *
*                                                                               *
*               Version 6.00 ����                                             *
*                                                                               *
*               Copyright (c) 2016-2016, Order. All rights reserved.            *
*               Created by Lord.                                                *
*                                                                               *
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Data.SQLite;

namespace KLine
{
    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// ���
        /// </summary>
        public String m_id = "";

        /// <summary>
        /// ��������ַ
        /// </summary>
        public String m_ip = "";

        /// <summary>
        /// �˿�
        /// </summary>
        public int m_port;

        /// <summary>
        /// ����
        /// </summary>
        public String m_type = "";
    }

    /// <summary>
    /// ����������
    /// </summary>
    public class ServerService
    {
        /// <summary>
        /// ��������������
        /// </summary>
        public ServerService()
        {
            CreateTable();
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        private String m_connectStr = "";

        /// <summary>
        /// ����SQL
        /// </summary>
        public const String CREATETABLESQL = "CREATE TABLE SERVERS(ID PRIMARY KEY, IP, PORT INTEGER, TYPE)";

        /// <summary>
        /// �����ַ���
        /// </summary>
        public const String DATABASENAME = "servers.db";

        /// <summary>
        /// ��ӷ�����
        /// </summary>
        /// <param name="serverInfo">��������Ϣ</param>
        /// <returns>״̬</returns>
        public int AddServer(ServerInfo serverInfo)
        {
            String sql = String.Format("INSERT INTO SERVERS(ID, IP, PORT, TYPE) values ('{0}','{1}', {2}, '{3}')",
                CStrA.GetDBString(serverInfo.m_id), CStrA.GetDBString(serverInfo.m_ip), serverInfo.m_port, CStrA.GetDBString(serverInfo.m_type));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// ��ȡ�������Ƿ���Ҫ������
        /// </summary>
        public void CreateTable()
        {
            String dataDir = DataCenter.GetUserPath() + "\\data";
            if (!CFileA.IsDirectoryExist(dataDir))
            {
                CFileA.CreateDirectory(dataDir);
            }
            String dataBasePath = dataDir + "\\" + DATABASENAME;
            m_connectStr = "Data Source = " + dataBasePath;
            if (!CFileA.IsFileExist(dataBasePath))
            {
                //�������ݿ��ļ�
                SQLiteConnection.CreateFile(dataBasePath);
                //������
                SQLiteConnection conn = new SQLiteConnection(m_connectStr);
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = CREATETABLESQL;
                cmd.ExecuteNonQuery();
                conn.Close();
                CreateDefaultServers();
            }
        }

        /// <summary>
        /// ����Ĭ�ϵķ�����
        /// </summary>
        public void CreateDefaultServers()
        {
            ServerInfo server1 = new ServerInfo();
            server1.m_id = System.Guid.NewGuid().ToString();
            server1.m_ip = "192.168.88.101";
            server1.m_port = 9999;
            server1.m_type = "��������";
            AddServer(server1);
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id">���</param>
        /// <returns>״̬</returns>
        public int DeleteServer(String id)
        {
            String sql = String.Format("DELETE FROM SERVERS WHERE ID = '{0}'", CStrA.GetDBString(id));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// ��ȡ��������Ϣ
        /// </summary>
        /// <param name="serverInfos">��������Ϣ�б�</param>
        /// <returns>״̬</returns>
        public int GetServers(List<ServerInfo> serverInfos)
        {
            String sql = "SELECT * FROM SERVERS";
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ServerInfo serverInfo = new ServerInfo();
                serverInfo.m_id = reader.GetString(0);
                serverInfo.m_ip = reader.GetString(1);
                serverInfo.m_port = reader.GetInt32(2);
                serverInfo.m_type = reader.GetString(3);
                serverInfos.Add(serverInfo);
            }
            reader.Close();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// ���·�������Ϣ
        /// </summary>
        /// <param name="serverInfo">��������Ϣ</param>
        /// <returns>״̬</returns>
        public void UpdateServer(ServerInfo serverInfo)
        {
            String sql = String.Format("UPDATE SERVERS SET IP = '{0}', PORT = {1}, TYPE = '{2}' WHERE ID = '{3}'",
                CStrA.GetDBString(serverInfo.m_ip), serverInfo.m_port, CStrA.GetDBString(serverInfo.m_type), CStrA.GetDBString(serverInfo.m_id));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
