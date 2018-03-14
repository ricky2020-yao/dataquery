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
    public class ConnectInfo
    {
        #region Lord 2016/6/26
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
        #endregion
    }

    /// <summary>
    /// ���ӷ���
    /// </summary>
    public class ConnectService : IDisposable
    {
        #region Lord 2016/6/24
        /// <summary>
        /// ��������������
        /// </summary>
        public ConnectService()
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
        public const String CREATETABLESQL = "CREATE TABLE CONNECTS(ID PRIMARY KEY, IP, PORT INTEGER, TYPE)";

        /// <summary>
        /// �����ַ���
        /// </summary>
        public const String DATABASENAME = "connects.db";

        /// <summary>
        /// ��ӷ�����
        /// </summary>
        /// <param name="connectInfo">������Ϣ</param>
        /// <returns>״̬</returns>
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
                CreateDefaultConnects();
            }
        }

        /// <summary>
        /// ����Ĭ�ϵ�����
        /// </summary>
        public void CreateDefaultConnects()
        {
            ConnectInfo connectInfo1 = new ConnectInfo();
            connectInfo1.m_id = System.Guid.NewGuid().ToString();
            connectInfo1.m_ip = "114.55.4.91";
            connectInfo1.m_port = 9961;
            connectInfo1.m_type = "��������";
            AddConnect(connectInfo1);
        }

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="connectInfos">������Ϣ�б�</param>
        /// <returns>״̬</returns>
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
        /// ����������Ϣ
        /// </summary>
        /// <param name="connectInfo">������Ϣ</param>
        /// <returns>״̬</returns>
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
