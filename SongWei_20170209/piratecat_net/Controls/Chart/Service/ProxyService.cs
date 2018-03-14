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
    /// 代理信息
    /// </summary>
    public class ProxyInfo
    {
        #region Lord 2016/6/26
        /// <summary>
        /// 域
        /// </summary>
        public String m_domain = "";

        /// <summary>
        /// IP
        /// </summary>
        public String m_ip = "";

        /// <summary>
        /// 用户名
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// 端口
        /// </summary>
        public int m_port = 0;

        /// <summary>
        /// 密码
        /// </summary>
        public String m_pwd = "";

        /// <summary>
        /// 类型
        /// </summary>
        public int m_type = 0;

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool m_use;
        #endregion
    }

    /// <summary>
    /// 代理服务
    /// </summary>
    public class ProxyService : IDisposable
    {
        #region Lord 2016/6/24
        /// <summary>
        /// 创建代理服务
        /// </summary>
        public ProxyService()
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
        public const String CREATETABLESQL = "CREATE TABLE PROXYINFOS(TYPE INTEGER PRIMARY KEY, IP, PORT INTEGER, NAME, PASSWORD, DOMAIN, USE INTEGER)";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public const String DATABASENAME = "proxyinos.db";

        /// <summary>
        /// 添加代理信息
        /// </summary>
        /// <param name="proxyInfo">代理信息</param>
        /// <returns>状态</returns>
        public int AddProxyInfo(ProxyInfo proxyInfo)
        {
            String sql = String.Format("INSERT INTO PROXYINFOS(TYPE, IP, PORT, NAME, PASSWORD, DOMAIN, USE) values ({0},'{1}',{2},'{3}','{4}','{5}',{6})",
                proxyInfo.m_type, CStrA.GetDBString(proxyInfo.m_ip), proxyInfo.m_port, CStrA.GetDBString(proxyInfo.m_name),
                CStrA.GetDBString(proxyInfo.m_pwd), CStrA.GetDBString(proxyInfo.m_domain), proxyInfo.m_use ? 1 : 0);
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
                CreateDefaultProxyInfos();
            }
        }

        /// <summary>
        /// 创建默认的代理信息
        /// </summary>
        public void CreateDefaultProxyInfos()
        {
            ProxyInfo proxyInfo1 = new ProxyInfo();
            proxyInfo1.m_use = true;
            AddProxyInfo(proxyInfo1);
            ProxyInfo proxyInfo2 = new ProxyInfo();
            proxyInfo2.m_type = 1;
            AddProxyInfo(proxyInfo2);
            ProxyInfo proxyInfo3 = new ProxyInfo();
            proxyInfo3.m_type = 2;
            AddProxyInfo(proxyInfo3);
            ProxyInfo proxyInfo4 = new ProxyInfo();
            proxyInfo4.m_type = 3;
            AddProxyInfo(proxyInfo4);
        }

        /// <summary>
        /// 获取代理信息
        /// </summary>
        /// <param name="proxyInfos">代理信息列表</param>
        /// <returns>状态</returns>
        public int GetProxyInfos(List<ProxyInfo> proxyInfos)
        {
            String sql = "SELECT * FROM PROXYINFOS";
            SQLiteConnection conn = new SQLiteConnection(m_connectStr);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ProxyInfo proxyInfo = new ProxyInfo();
                proxyInfo.m_type = reader.GetInt32(0);
                proxyInfo.m_ip = reader.GetString(1);
                proxyInfo.m_port = reader.GetInt32(2);
                proxyInfo.m_name = reader.GetString(3);
                proxyInfo.m_pwd = reader.GetString(4);
                proxyInfo.m_domain = reader.GetString(5);
                proxyInfo.m_use = reader.GetInt32(6) == 1;
                proxyInfos.Add(proxyInfo);
            }
            reader.Close();
            conn.Close();
            return 1;
        }

        /// <summary>
        /// 更新代理信息
        /// </summary>
        /// <param name="proxyInfo">代理信息</param>
        /// <returns>状态</returns>
        public int UpdateConnect(ProxyInfo proxyInfo)
        {
            String sql = String.Format("UPDATE PROXYINFOS SET IP = '{0}', PORT = {1}, NAME = '{2}', PASSWORD = '{3}', DOMAIN='{4}', USE = {5} WHERE TYPE = {6}",
                CStrA.GetDBString(proxyInfo.m_ip), proxyInfo.m_port, CStrA.GetDBString(proxyInfo.m_name),
                CStrA.GetDBString(proxyInfo.m_pwd), CStrA.GetDBString(proxyInfo.m_domain), proxyInfo.m_use ? 1 : 0, proxyInfo.m_type);
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
