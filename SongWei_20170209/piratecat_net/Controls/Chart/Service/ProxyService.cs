/*******************************************************************************\
*                                                                               *
* ConnectService.cs - Connect service functions, types, and definitions.        *
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
using System.IO;
using System.Data.SQLite;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class ProxyInfo
    {
        #region Lord 2016/6/26
        /// <summary>
        /// ��
        /// </summary>
        public String m_domain = "";

        /// <summary>
        /// IP
        /// </summary>
        public String m_ip = "";

        /// <summary>
        /// �û���
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �˿�
        /// </summary>
        public int m_port = 0;

        /// <summary>
        /// ����
        /// </summary>
        public String m_pwd = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_type = 0;

        /// <summary>
        /// �Ƿ�ʹ��
        /// </summary>
        public bool m_use;
        #endregion
    }

    /// <summary>
    /// �������
    /// </summary>
    public class ProxyService : IDisposable
    {
        #region Lord 2016/6/24
        /// <summary>
        /// �����������
        /// </summary>
        public ProxyService()
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
        public const String CREATETABLESQL = "CREATE TABLE PROXYINFOS(TYPE INTEGER PRIMARY KEY, IP, PORT INTEGER, NAME, PASSWORD, DOMAIN, USE INTEGER)";

        /// <summary>
        /// �����ַ���
        /// </summary>
        public const String DATABASENAME = "proxyinos.db";

        /// <summary>
        /// ��Ӵ�����Ϣ
        /// </summary>
        /// <param name="proxyInfo">������Ϣ</param>
        /// <returns>״̬</returns>
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
                CreateDefaultProxyInfos();
            }
        }

        /// <summary>
        /// ����Ĭ�ϵĴ�����Ϣ
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
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="proxyInfos">������Ϣ�б�</param>
        /// <returns>״̬</returns>
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
        /// ���´�����Ϣ
        /// </summary>
        /// <param name="proxyInfo">������Ϣ</param>
        /// <returns>״̬</returns>
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
