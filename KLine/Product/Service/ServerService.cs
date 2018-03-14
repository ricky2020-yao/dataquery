/*******************************************************************************\
*                                                                               *
* ServerService.cs - Server service functions, types, and definitions.          *
*                                                                               *
*               Version 6.00 ★★★                                             *
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
    /// 服务器信息
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public String m_id = "";

        /// <summary>
        /// 服务器地址
        /// </summary>
        public String m_ip = "";

        /// <summary>
        /// 端口
        /// </summary>
        public int m_port;

        /// <summary>
        /// 类型
        /// </summary>
        public String m_type = "";
    }

    /// <summary>
    /// 服务器服务
    /// </summary>
    public class ServerService
    {
        /// <summary>
        /// 创建服务器服务
        /// </summary>
        public ServerService()
        {
            CreateTable();
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        private String m_connectStr = "";

        /// <summary>
        /// 建表SQL
        /// </summary>
        public const String CREATETABLESQL = "CREATE TABLE SERVERS(ID PRIMARY KEY, IP, PORT INTEGER, TYPE)";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public const String DATABASENAME = "servers.db";

        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="serverInfo">服务器信息</param>
        /// <returns>状态</returns>
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
        /// 获取或设置是否需要创建表
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
                //创建数据库文件
                SQLiteConnection.CreateFile(dataBasePath);
                //创建表
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
        /// 创建默认的服务器
        /// </summary>
        public void CreateDefaultServers()
        {
            ServerInfo server1 = new ServerInfo();
            server1.m_id = System.Guid.NewGuid().ToString();
            server1.m_ip = "192.168.88.101";
            server1.m_port = 9999;
            server1.m_type = "主服务器";
            AddServer(server1);
        }

        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>状态</returns>
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
        /// 获取服务器信息
        /// </summary>
        /// <param name="serverInfos">服务器信息列表</param>
        /// <returns>状态</returns>
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
        /// 更新服务器信息
        /// </summary>
        /// <param name="serverInfo">服务器信息</param>
        /// <returns>状态</returns>
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
