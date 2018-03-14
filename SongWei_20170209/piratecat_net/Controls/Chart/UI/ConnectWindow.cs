/*********************************************************************************\
*                                                                                 *
* ConnectWindow.cs - Connect window functions, types, and definitions.            *
*                                                                                 *
*               Version 6.00  ��                                                  *
*                                                                                 *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.          *
*               Created by QiChunyou.                                             *
*                                                                                 *
**********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;

namespace piratecat
{
    /// <summary>
    /// �������ô���
    /// </summary>
    public class ConnectWindow : WindowXmlEx
    {
        #region Lord 2016/6/26
        /// <summary>
        /// �����������ô���
        /// </summary>
        /// <param name="native">������</param>
        /// <param name="order">������</param>
        public ConnectWindow(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\ConnectWindow.xml";
            Native = native;
            LoadFile(xmlPath, null);
            m_window = GetWindow("windowConnect") as WindowEx;
            m_gridServers = GetGrid("gridServers");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_gridServers.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_connectServer = DataCenter.ConnectService;
            m_proxyService = DataCenter.ProxyService;
            m_serverService = DataCenter.ServerService;
            m_cbMainServerIP = GetComboBox("cbMainServerIP");
            m_cbType = GetComboBox("cbType");
            m_spinPort = GetSpin("spinPort");
            m_txtIP = GetTextBox("txtIP");
            m_txtMainServerPort = GetSpin("spinMainServerPort");
            //ע�����¼�
            RegisterEvents(m_window);
        }

        /// <summary>
        /// �������������б�
        /// </summary>
        private ComboBoxA m_cbMainServerIP;

        /// <summary>
        /// ���������������б�
        /// </summary>
        private ComboBoxA m_cbType;

        /// <summary>
        /// ���ӷ���
        /// </summary>
        private ConnectService m_connectServer;

        /// <summary>
        /// �������б���
        /// </summary>
        private GridA m_gridServers;

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// �������
        /// </summary>
        private ProxyService m_proxyService;

        /// <summary>
        /// ����������
        /// </summary>
        private ServerService m_serverService;

        /// <summary>
        /// �˿��ı���
        /// </summary>
        private SpinA m_spinPort;

        /// <summary>
        /// IP��ַ�ı���
        /// </summary>
        private TextBoxA m_txtIP;

        /// <summary>
        /// ���������˿��ı���
        /// </summary>
        private SpinA m_txtMainServerPort;

        private OwChart m_chart;

        /// <summary>
        /// ��ȡ������XML�ĵ�
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
            set { m_window = value; }
        }

        /// <summary>
        /// ��ӷ�����
        /// </summary>
        private void AddServer()
        {
            ServerInfo serverInfo = new ServerInfo();
            serverInfo.m_id = System.Guid.NewGuid().ToString();
            serverInfo.m_ip = m_txtIP.Text;
            serverInfo.m_port = (int)m_spinPort.Value;
            serverInfo.m_type = m_cbType.Text;
            m_serverService.AddServer(serverInfo);
            List<ServerInfo> serverInfos = new List<ServerInfo>();
            serverInfos.Add(serverInfo);
            AddServersToGrid(serverInfos);
            serverInfos.Clear();
            BindServersToComboBox();
            m_window.Invalidate();
        }

        /// <summary>
        /// ���ģ�嵽���ؼ���
        /// </summary>
        /// <param name="serverInfos">��������Ϣ</param>
        private void AddServersToGrid(List<ServerInfo> serverInfos)
        {
            m_gridServers.BeginUpdate();
            int serversInfosSize = serverInfos.Count;
            GridRow selectedRow = null;
            for (int i = 0; i < serversInfosSize; i++)
            {
                ServerInfo serverInfo = serverInfos[i];
                GridRow row = new GridRow();
                row.Grid = m_gridServers;
                GridStringCell idCell = new GridStringCell();
                idCell.Grid = m_gridServers;
                idCell.Column = m_gridServers.GetColumn(0);
                idCell.Text = serverInfo.m_id;
                row.AddCell(0, idCell);
                GridStringCell ipCell = new GridStringCell();
                ipCell.Grid = m_gridServers;
                ipCell.Column = m_gridServers.GetColumn(1);
                ipCell.Text = serverInfo.m_ip;
                row.AddCell(1, ipCell);
                GridStringCell portCell = new GridStringCell();
                portCell.Grid = m_gridServers;
                portCell.Column = m_gridServers.GetColumn(2);
                portCell.Text = serverInfo.m_port.ToString();
                row.AddCell(2, portCell);
                GridStringCell typeCell = new GridStringCell();
                typeCell.Grid = m_gridServers;
                typeCell.Column = m_gridServers.GetColumn(3);
                typeCell.Text = serverInfo.m_type;
                row.AddCell(3, typeCell);
                m_gridServers.AddRow(row);
                selectedRow = row;
            }
            //ѡ����
            if (selectedRow != null)
            {
                List<GridRow> selectedRows = new List<GridRow>();
                selectedRows.Add(selectedRow);
                m_gridServers.SelectedRows = selectedRows;
            }
            m_gridServers.EndUpdate();
            m_gridServers.VScrollBar.ScrollToEnd();
        }

        /// <summary>
        /// �󶨷������������б�
        /// </summary>
        private void BindServersToComboBox()
        {
            m_cbMainServerIP.ClearItems();
            List<ServerInfo> serverInfos = new List<ServerInfo>();
            m_serverService.GetServers(serverInfos);
            int serverInfosSize = serverInfos.Count;
            for (int i = 0; i < serverInfosSize; i++)
            {
                ServerInfo connectInfo = serverInfos[i];
                String type = connectInfo.m_type;
                MenuItemA item = new MenuItemA(connectInfo.m_ip);
                item.Value = connectInfo.m_port.ToString();
                if (type == "��������")
                {
                    m_cbMainServerIP.AddItem(item);
                }
            }
        }

        /// <summary>
        /// ��ť����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                //���
                if (name == "btnAdd")
                {
                    AddServer();
                }
                //ɾ��
                else if (name == "btnDelete")
                {
                    DeleteServer();
                }
                //�ر�
                else if (name == "btnClose" || name == "btnCancel")
                {
                    Close();
                }
                //����
                else if (name == "btnSave")
                {
                    SaveData();
                }
                //�޸�
                else if (name == "btnUpdate")
                {
                    UpdateServer();
                }
            }
        }

        /// <summary>
        /// �����б�ѡ�������ı��¼�
        /// </summary>
        /// <param name="sender">������</param>
        private void ComboBoxSelectedIndexChanged(object sender)
        {
            ComboBoxA comboBox = sender as ComboBoxA;
            OnComboBoxSelectedIndexChanged(comboBox);
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// ɾ��������
        /// </summary>
        private void DeleteServer()
        {
            List<GridRow> selectedRows = m_gridServers.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                GridRow row = selectedRows[0];
                m_serverService.DeleteServer(row.GetCell(0).Text);
                m_gridServers.RemoveRow(row);
                row.Dispose();
                List<GridRow> rows = m_gridServers.GetRows();
                int rowsSize = rows.Count;
                if (rowsSize > 0)
                {
                    selectedRows = new List<GridRow>();
                    selectedRows.Add(m_gridServers.GetRow(rowsSize - 1));
                    m_gridServers.SelectedRows = selectedRows;
                }
                m_gridServers.Update();
                m_gridServers.Update();
                BindServersToComboBox();
                m_window.Invalidate();
            }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_gridServers != null)
                {
                    m_gridServers.UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_gridServers = null;
                }
                if (m_window != null)
                {
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        /// <param name="sender">������</param>
        private void GridSelectedRowsChanged(object sender)
        {
            OnGridSelectedRowsChanged();
        }

        /// <summary>
        /// ���ѡ���иı䷽��
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            List<GridRow> selectedRows = m_gridServers.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                GridRow row = selectedRows[0];     
                m_txtIP.Text = row.GetCell(1).Text;
                m_spinPort.Value = CStr.ConvertStrToInt(row.GetCell(2).Text);         
                m_cbType.Text = row.GetCell(3).Text;
                m_window.Invalidate();
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public override void LoadData()
        {
            //����������Ϣ
            List<ConnectInfo> connectInfos = new List<ConnectInfo>();
            m_connectServer.GetConnects(connectInfos);
            int connectInfosSize = connectInfos.Count;
            for (int i = 0; i < connectInfosSize; i++)
            {
                ConnectInfo connectInfo = connectInfos[i];
                String type = connectInfo.m_type;
                if (type == "��������")
                {
                    m_cbMainServerIP.Text = connectInfo.m_ip;
                    m_txtMainServerPort.Value = connectInfo.m_port;
                }
            }
            ComboBoxA cbProxyType = GetComboBox("cbProxyType");
            List<ProxyInfo> proxyInfos = new List<ProxyInfo>();
            m_proxyService.GetProxyInfos(proxyInfos);
            int proxyInfosSize = proxyInfos.Count;
            for (int i = 0; i < proxyInfosSize; i++)
            {
                ProxyInfo proxyInfo = proxyInfos[i];
                if (proxyInfo.m_use)
                {
                    cbProxyType.SelectedIndex = proxyInfo.m_type;
                }
            }
        }

        /// <summary>
        /// ���ش�����Ϣ
        /// </summary>
        private void LoadProxyInfo()
        {
            ComboBoxA cbProxyType = GetComboBox("cbProxyType");
            TextBoxA txtProxyIP = GetTextBox("txtProxyIP");
            SpinA spinProxyPort = GetSpin("spinProxyPort");
            TextBoxA txtProxyUserName = GetTextBox("txtProxyUserName");
            TextBoxA txtProxyUserPwd = GetTextBox("txtProxyUserPwd");
            TextBoxA txtProxyDomain = GetTextBox("txtProxyDomain");
            List<ProxyInfo> proxyInfos = new List<ProxyInfo>();
            m_proxyService.GetProxyInfos(proxyInfos);
            int proxyInfosSize = proxyInfos.Count;
            for (int i = 0; i < proxyInfosSize; i++)
            {
                ProxyInfo proxyInfo = proxyInfos[i];
                if (cbProxyType.SelectedIndex == proxyInfo.m_type)
                {
                    txtProxyIP.Enabled = (proxyInfo.m_type == 1 || proxyInfo.m_type == 2);
                    spinProxyPort.Enabled = (proxyInfo.m_type == 1 || proxyInfo.m_type == 2);
                    txtProxyUserName.Enabled = (proxyInfo.m_type == 1 || proxyInfo.m_type == 2 || proxyInfo.m_type == 3);
                    txtProxyUserPwd.Enabled = (proxyInfo.m_type == 1 || proxyInfo.m_type == 2 || proxyInfo.m_type == 3);
                    txtProxyDomain.Enabled = proxyInfo.m_type == 2;
                    txtProxyIP.Text = proxyInfo.m_ip;
                    spinProxyPort.Value = proxyInfo.m_port;
                    txtProxyUserName.Text = proxyInfo.m_name;
                    txtProxyUserPwd.Text = proxyInfo.m_pwd;
                    txtProxyDomain.Text = proxyInfo.m_domain;
                    break;
                }
            }
            m_window.Invalidate();
            proxyInfos.Clear();
        }

        /// <summary>
        /// �����б�ѡ�������ı��¼�
        /// </summary>
        /// <param name="comboBox">�����б�</param>
        private void OnComboBoxSelectedIndexChanged(ComboBoxA comboBox)
        {
            String name = comboBox.Name;
            if (name == "cbMainServerIP")
            {
                String port = comboBox.SelectedValue;
                m_txtMainServerPort.Value = CStr.ConvertStrToInt(port);
            }
            else if (name == "cbProxyType")
            {
                LoadProxyInfo();
            }
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            ControlEvent selectedIndexChangedEvent = new ControlEvent(ComboBoxSelectedIndexChanged);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                GridColumn column = subControl as GridColumn;
                ButtonA button = subControl as ButtonA;
                GridA grid = subControl as GridA;
                ComboBoxA comboBox = subControl as ComboBoxA;
                if (column != null)
                {
                    column.AllowResize = true;
                    column.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    column.BorderColor = COLOR.CONTROLBORDER;
                    column.ForeColor = CDraw.PCOLORS_FORECOLOR;
                }
                else if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (comboBox != null)
                {
                    comboBox.RegisterEvent(selectedIndexChangedEvent, EVENTID.SELECTEDINDEXCHANGED);
                }
                else if (grid != null)
                {
                    grid.GridLineColor = COLOR.EMPTY;
                    GridRowStyle rowStyle = new GridRowStyle();
                    grid.RowStyle = rowStyle;
                    rowStyle.BackColor = COLOR.EMPTY;
                    rowStyle.SelectedBackColor = CDraw.PCOLORS_SELECTEDROWCOLOR;
                    rowStyle.HoveredBackColor = CDraw.PCOLORS_HOVEREDROWCOLOR;
                }
                RegisterEvents(subControl);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void SaveData()
        {
            List<ConnectInfo> connectInfos = new List<ConnectInfo>();
            m_connectServer.GetConnects(connectInfos);
            int connectInfosSize = connectInfos.Count;
            for (int i = 0; i < connectInfosSize; i++)
            {
                ConnectInfo connectInfo = connectInfos[i];
                String type = connectInfo.m_type;
                if (type == "��������")
                {
                    connectInfo.m_ip = m_cbMainServerIP.Text;
                    connectInfo.m_port = CStr.ConvertStrToInt(m_txtMainServerPort.Text);
                }
                m_connectServer.UpdateConnect(connectInfo);
            }
            connectInfos.Clear();
            //���������Ϣ
            ComboBoxA cbProxyType = GetComboBox("cbProxyType");
            TextBoxA txtProxyIP = GetTextBox("txtProxyIP");
            SpinA spinProxyPort = GetSpin("spinProxyPort");
            TextBoxA txtProxyUserName = GetTextBox("txtProxyUserName");
            TextBoxA txtProxyUserPwd = GetTextBox("txtProxyUserPwd");
            TextBoxA txtProxyDomain = GetTextBox("txtProxyDomain");
            ProxyInfo proxyInfo = new ProxyInfo();
            proxyInfo.m_domain = txtProxyDomain.Text;
            proxyInfo.m_ip = txtProxyIP.Text;
            proxyInfo.m_port = (int)spinProxyPort.Value;
            proxyInfo.m_name = txtProxyUserName.Text;
            proxyInfo.m_pwd = txtProxyUserPwd.Text;
            proxyInfo.m_type = cbProxyType.SelectedIndex;
            proxyInfo.m_use = true;
            List<ProxyInfo> proxyInfos = new List<ProxyInfo>();
	        m_proxyService.GetProxyInfos(proxyInfos);
            int proxyInfosSize = proxyInfos.Count;
            for (int i = 0; i < proxyInfosSize; i++)
            {
                ProxyInfo pInfo = proxyInfos[i];
                if (cbProxyType.SelectedIndex == pInfo.m_type)
                {
                    m_proxyService.UpdateConnect(proxyInfo);
                }
                else
                {
                    pInfo.m_use = false;
                    m_proxyService.UpdateConnect(pInfo);
                }
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public override void Show()
        {
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            //���ط������б�
            List<ServerInfo> serverInfos = new List<ServerInfo>();
            m_serverService.GetServers(serverInfos);
            AddServersToGrid(serverInfos);
            BindServersToComboBox();
            LoadData();
            m_window.Invalidate();
        }

        /// <summary>
        /// �޸ķ�����
        /// </summary>
        private void UpdateServer()
        {
            List<GridRow> selectedRows = m_gridServers.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                GridRow row = selectedRows[0];
                ServerInfo serverInfo = new ServerInfo();
                serverInfo.m_id = row.GetCell(0).Text;
                serverInfo.m_ip = m_txtIP.Text;
                row.GetCell(1).Text = serverInfo.m_ip;
                serverInfo.m_port = (int)m_spinPort.Value;
                row.GetCell(2).Text = serverInfo.m_port.ToString();
                serverInfo.m_type = m_cbType.Text;
                row.GetCell(3).Text = serverInfo.m_type;
                m_serverService.UpdateServer(serverInfo);
                m_window.Invalidate();
                BindServersToComboBox();
            }
        }
        #endregion
    }
}
