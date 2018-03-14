/*******************************************************************************\
*                                                                               *
* ConnectService.cs - Connect service functions, types, and definitions.        *
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
using System.IO;
using System.Data.SQLite;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// 连接信息
    /// </summary>
    public class ConnectInfo
    {
        #region Lord 2016/6/26
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
        #endregion
    }

    /// <summary>
    /// 连接服务
    /// </summary>
    public class ConnectService : IDisposable
    {
        #region Lord 2016/6/24
        /// <summary>
        /// 创建服务器服务
        /// </summary>
        public ConnectService()
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
        public const String CREATETABLESQL = "CREATE TABLE CONNECTS(ID PRIMARY KEY, IP, PORT INTEGER, TYPE)";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public const String DATABASENAME = "connects.db";

        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="connectInfo">连接信息</param>
        /// <returns>状态</returns>
        public int AddConnect(ConnectInfo connectInfo)
        {
            String sql = String.Format("INSERT INTO CONNECTS(ID, IP, PORT, TYPE) values ('{0}','{1}', {2}, '{3}')",
                CStrA.GetDBString(connectInfo.m_id), CStrA.GetDBString(connectInfo.m_ip), connectInfo.m_port, CStrA.GetDBString(connectInfo.m_type));
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
                CreateDefaultConnects();
            }
        }

        /// <summary>
        /// 创建默认的连接
        /// </summary>
        public void CreateDefaultConnects()
        {
            ConnectInfo connectInfo1 = new ConnectInfo();
            connectInfo1.m_id = System.Guid.NewGuid().ToString();
            connectInfo1.m_ip = "114.55.4.91";
            connectInfo1.m_port = 9961;
            connectInfo1.m_type = "主服务器";
            AddConnect(connectInfo1);
        }

        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="connectInfos">连接信息列表</param>
        /// <returns>状态</returns>
        public int GetConnects(List<ConnectInfo> connectInfos)
        {
            String sql = "SELECT * FROM CONNECTS";
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ConnectInfo connectInfo = new ConnectInfo();
                connectInfo.m_id = reader.GetString(0);
                connectInfo.m_ip = reader.GetString(1);
                connectInfo.m_port = reader.GetInt32(2);
                connectInfo.m_type = reader.GetString(3);
                connectInfos.Add(connectInfo);
            }
            reader.Close();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// 更新连接信息
        /// </summary>
        /// <param name="connectInfo">连接信息</param>
        /// <returns>状态</returns>
        public int UpdateConnect(ConnectInfo connectInfo)
        {
            String sql = String.Format("UPDATE CONNECTS SET IP = '{0}', PORT = {1}, TYPE = '{2}' WHERE ID = '{3}'",
                CStrA.GetDBString(connectInfo.m_ip), connectInfo.m_port, CStrA.GetDBString(connectInfo.m_type), CStrA.GetDBString(connectInfo.m_id));
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }
        #endregion
    }
}
