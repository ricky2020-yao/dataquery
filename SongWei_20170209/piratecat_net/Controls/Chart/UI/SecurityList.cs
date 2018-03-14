/*****************************************************************************\
*                                                                             *
* SecurityList.cs - Security list functions, types, and definitions.          *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/23.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using piratecat;
using OwLibCT;

namespace OwLib
{
    /// <summary>
    /// ��Ʊ�б���
    /// </summary>
    public class SecurityList : WindowXmlEx
    {
        #region Lord 2016/1/23
        /// <summary>
        /// ������Ʊ�б���
        /// </summary>
        /// <param name="native">������</param>
        public SecurityList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\SecurityList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowSecurity") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����¼�
            RegisterEvents(m_window);
            m_gridSecurities = GetGrid("gridSecurities");
            m_gridSelectSecurities = GetGrid("gridSelectSecurities");
            m_tvBlock = GetTree("tvBlock");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_tvBlock.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            //ע�����
            m_securityService = DataCenter.SecurityService;
            m_securityDataCallBack = new ListenerMessageCallBack(SecurityDataCallBack);
            m_securityService.RegisterListener(m_securitiesRequestID, m_securityDataCallBack);
            m_userSecurityService = DataCenter.UserSecurityService;
        }

        /// <summary>
        /// ֤ȯ���
        /// </summary>
        private GridA m_gridSecurities;

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// ��ѡ֤ȯ���
        /// </summary>
        private GridA m_gridSelectSecurities;

        /// <summary>
        /// ���ÿؼ��̷߳����¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// ��Ʊ���ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_securityDataCallBack;

        /// <summary>
        /// ��Ʊ����ID
        /// </summary>
        private int m_securitiesRequestID = BaseService.GetRequestID();

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// ��ѡ�ɷ���
        /// </summary>
        private UserSecurityServiceEx m_userSecurityService;

        /// <summary>
        /// �����
        /// </summary>
        private TreeA m_tvBlock;

        private bool m_isSubmit;

        /// <summary>
        /// ��ȡ�������Ƿ�ȷ��
        /// </summary>
        public bool IsSubmit
        {
            get { return m_isSubmit; }
            set { m_isSubmit = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// ����֤ȯ�������
        /// </summary>
        /// <param name="securities">֤ȯ�б�</param>
        public void AddSecuritiesToSecuritiesGrid(List<Security> securities)
        {
            Dictionary<String, String> existsCodes = new Dictionary<String, String>();
            List<GridRow> rows = m_gridSecurities.GetRows();
            int rowSize = rows.Count;
            for (int i = 0; i < rowSize; i++)
            {
                GridRow row = rows[i];
                String code = row.GetCell(0).GetString();
                existsCodes[code] = "";
            }
            int securitiesSize = securities.Count;
            m_gridSecurities.BeginUpdate();
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = securities[i];
                if (!existsCodes.ContainsKey(security.m_code))
                {
                    GridRow row = new GridRow();
                    m_gridSecurities.AddRow(row);
                    row.AddCell(0, new GridStringCell(security.m_code));
                    row.AddCell(1, new GridStringCell(security.m_name));
                    existsCodes[security.m_code] = "";
                }
            }
            m_gridSecurities.EndUpdate();
            m_gridSecurities.Invalidate();
        }

        /// <summary>
        /// �󶨴�ѡ֤ȯ�����
        /// </summary>
        /// <param name="securities">֤ȯ�б�</param>
        public void BindSecuritiesToSelectSecuritiesGrid(List<Security> securities)
        {
            m_gridSelectSecurities.BeginUpdate();
            m_gridSelectSecurities.ClearRows();
            int securitiesSize = securities.Count;
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = securities[i];
                GridRow row = new GridRow();
                m_gridSelectSecurities.AddRow(row);
                row.AddCell(0, new GridStringCell(security.m_code));
                row.AddCell(1, new GridStringCell(security.m_name));
            }
            m_gridSelectSecurities.EndUpdate();
            m_gridSelectSecurities.Invalidate();
        }
        
        /// <summary>
        /// �����ť����
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
                //ȡ��
                if (name == "btnCancel" || name == "btnClose")
                {
                    Close();
                }
                //ѡ�й�Ʊ
                else if (name == "btnSelectSecurities")
                {
                    SelectSecurities();
                }
                //ȫѡ��Ʊ
                else if (name == "btnSelectAllSecurities")
                {
                    SelectAllSecurities();
                }
                //ȷ��
                else if (name == "btnSubmit")
                {
                    Submit();
                }
                //ȡ��ѡ��֤ȯ
                else if (name == "btnUnSelectSecurities")
                {
                    UnSelectSecurities();
                }
                //ȡ��ѡ��ȫ����Ʊ
                else if (name == "btnUnSelectAllSecurities")
                {
                    UnSelectAllSecurities();
                }
            }
        }

        /// <summary>
        /// �رմ���
        /// </summary>
        public override void Close()
        {
            m_window.Close();
            m_window.Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_securityService != null)
                {
                    m_securityService.UnRegisterListener(m_securitiesRequestID, m_securityDataCallBack);
                    m_securityDataCallBack = null;
                    m_securityService = null;
                }
                if (m_tvBlock != null)
                {
                    m_tvBlock.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_gridSelectedRowsChangedEvent = null;
                }
                if (m_window != null)
                {
                    m_window.UnRegisterEvent(m_invokeEvent, EVENTID.INVOKE);
                    m_invokeEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// ��ȡѡ�еĹ�Ʊ
        /// </summary>
        /// <param name="securities">��Ʊ�б�</param>
        public void GetSelectedSecurities(List<Security> securities)
        {
            List<GridRow> rows = m_gridSecurities.GetRows();
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                GridRow row = rows[i];
                Security security = new Security();
                if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                {
                    securities.Add(security);
                }
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
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="args">����</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// ������ѡ�����
        /// </summary>
        /// <param name="categories">��ѡ���б�</param>
        public void LoadUserSecurityCategories(List<UserSecurityCategory> categories)
        {
            int categoriesSize = categories.Count;
            m_tvBlock.BeginUpdate();
            List<TreeNodeA> childNodes = m_tvBlock.GetChildNodes();
            int childNodesSize = childNodes.Count;
            if (childNodesSize > 0)
            {
                TreeNodeA userSecurityNode = childNodes[childNodesSize - 1];
                for (int i = 0; i < categories.Count; i++)
                {
                    UserSecurityCategory category = categories[i];
                    TreeNodeA categoryNode = new TreeNodeA();
                    userSecurityNode.AppendNode(categoryNode);
                    categoryNode.Text = category.m_name;
                    categoryNode.Value = category.m_categoryID;
                }
            }
            m_tvBlock.EndUpdate();
        }

        /// <summary>
        /// ���ѡ���иı䷽��
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            List<GridRow> selectedRows = m_tvBlock.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                TreeNodeA node = selectedRows[0].GetCell(0) as TreeNodeA;
                TreeNodeA parentNode = node.Parent;
                if (parentNode != null && parentNode.Text == "��ѡ��")
                {
                    String categoryID = node.Value;
                    UserSecurityCategory category = new UserSecurityCategory();
                    m_userSecurityService.GetCategory(categoryID, ref category);
                    List<Security> securities = new List<Security>();
                    m_securityService.GetSecuritiesByCodes(category.m_codes, securities);
                    BindSecuritiesToSelectSecuritiesGrid(securities);
                }
                else
                {
                    if(node.GetChildNodes().Count == 0)
                    {
                        String blockID = node.Value;
                        m_securityService.GetSecuritiesByBlockID(m_securitiesRequestID, blockID);
                    }
                }
            }
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            List<Security> securities = new List<Security>();
            SecurityService.GetSecurities(securities, message.m_functionID, message.m_body, message.m_bodyLength);
            int securitiesSize = securities.Count;
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = new Security();
                m_securityService.GetSecurityByCode(securities[i].m_code, ref security);
                securities[i] = security;
            }
            BindSecuritiesToSelectSecuritiesGrid(securities);
            securities.Clear();
        }

        /// <summary>
        /// ֤ȯ���ݻص�����
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public void OnSecurityDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                GridColumn column = subControl as GridColumn;
                ButtonA button = subControl as ButtonA;
                GridA grid = subControl as GridA;
                if (column != null)
                {
                    column.AllowResize = true;
                    column.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    column.BorderColor = CDraw.PCOLORS_LINECOLOR3;
                    column.ForeColor = CDraw.PCOLORS_FORECOLOR;
                }
                else if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (grid != null)
                {
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
        /// �ӱ����ɾ��֤ȯ
        /// </summary>
        /// <param name="securities">֤ȯ�б�</param>
        public void RemoveSecuritiesFromSecuritiesGrid(List<Security> securities)
        {
            Dictionary<String, Security> existsCodes = new Dictionary<String, Security>();
            int securitiesSize = securities.Count;
            for (int i = 0; i < securitiesSize; i++)
            {
                Security security = securities[i];
                existsCodes[security.m_code] = security;
            }
            int rowSize = m_gridSecurities.GetRows().Count;
            m_gridSecurities.BeginUpdate();
            for (int i = 0; i < rowSize; i++)
            {
                GridRow row = m_gridSecurities.GetRow(i);
                Security security = new Security();
                if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                {
                    if (existsCodes.ContainsKey(security.m_code))
                    {
                        m_gridSecurities.RemoveRow(row);
                        i--;
                        rowSize--;
                    }
                }
            }
            m_gridSecurities.EndUpdate();
            m_gridSecurities.Invalidate();
        }

        /// <summary>
        /// ��Ʊ���ݻص��¼�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void SecurityDataCallBack(CMessage message)
        {
            OnSecurityDataCallBack(message);
        }

        /// <summary>
        /// ѡ�й�Ʊ
        /// </summary>
        public void SelectSecurities()
        {
            List<GridRow> selectedRows = m_gridSelectSecurities.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                List<Security> securities = new List<Security>();
                for (int i = 0; i < selectedRowsSize; i++)
                {
                    GridRow row = selectedRows[i];
                    Security security = new Security();
                    if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                    {
                        securities.Add(security);
                    }
                }
                AddSecuritiesToSecuritiesGrid(securities);
            }
        }

        /// <summary>
        /// ȫѡ��Ʊ
        /// </summary>
        public void SelectAllSecurities()
        {
            List<GridRow> rows = m_gridSelectSecurities.GetRows();
            int rowSize = rows.Count;
            if (rowSize > 0)
            {
                List<Security> securities = new List<Security>();
                for (int i = 0; i < rowSize; i++)
                {
                    GridRow row = rows[i];
                    Security security = new Security();
                    if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                    {
                        securities.Add(security);
                    }
                }
                AddSecuritiesToSecuritiesGrid(securities);
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public override void Show()
        {
            List<UserSecurityCategory> categories = new List<UserSecurityCategory>();
            m_userSecurityService.GetCategories(categories);
            LoadUserSecurityCategories(categories);
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            m_window.Invalidate();
        }

        /// <summary>
        /// ȷ���ύ
        /// </summary>
        public void Submit()
        {
            m_isSubmit = true;
            Close();
        }

        /// <summary>
        /// ȡ��ѡ�����й�Ʊ
        /// </summary>
        public void UnSelectAllSecurities()
        {
            List<GridRow> rows = m_gridSecurities.GetRows();
            int rowSize = rows.Count;
            if (rowSize > 0)
            {
                List<Security> securities = new List<Security>();
                for (int i = 0; i < rowSize; i++)
                {
                    GridRow row = rows[i];
                    Security security = new Security();
                    if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                    {
                        securities.Add(security);
                    }
                }
                RemoveSecuritiesFromSecuritiesGrid(securities);
            }
        }

        /// <summary>
        /// ȡ��ѡ�й�Ʊ
        /// </summary>
        public void UnSelectSecurities()
        {
            List<GridRow> selectedRows = m_gridSecurities.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                List<Security> securities = new List<Security>();
                for (int i = 0; i < selectedRowsSize; i++)
                {
                    GridRow row = selectedRows[i];
                    Security security = new Security();
                    if (m_securityService.GetSecurityByCode(row.GetCell(0).GetString(), ref security))
                    {
                        securities.Add(security);
                    }
                }
                RemoveSecuritiesFromSecuritiesGrid(securities);
            }
        }
        #endregion
    }
}
