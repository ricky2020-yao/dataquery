/*****************************************************************************\
*                                                                             *
* SecurityList.cs - Security list functions, types, and definitions.          *
*                                                                             *
*               Version 1.00  ★★★                                          *
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
    /// 股票列表窗体
    /// </summary>
    public class SecurityList : WindowXmlEx
    {
        #region Lord 2016/1/23
        /// <summary>
        /// 创建股票列表窗体
        /// </summary>
        /// <param name="native">方法库</param>
        public SecurityList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\SecurityList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowSecurity") as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //注册点击事件
            RegisterEvents(m_window);
            m_gridSecurities = GetGrid("gridSecurities");
            m_gridSelectSecurities = GetGrid("gridSelectSecurities");
            m_tvBlock = GetTree("tvBlock");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_tvBlock.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            //注册服务
            m_securityService = DataCenter.SecurityService;
            m_securityDataCallBack = new ListenerMessageCallBack(SecurityDataCallBack);
            m_securityService.RegisterListener(m_securitiesRequestID, m_securityDataCallBack);
            m_userSecurityService = DataCenter.UserSecurityService;
        }

        /// <summary>
        /// 证券表格
        /// </summary>
        private GridA m_gridSecurities;

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// 待选证券表格
        /// </summary>
        private GridA m_gridSelectSecurities;

        /// <summary>
        /// 调用控件线程方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 股票数据回调事件
        /// </summary>
        private ListenerMessageCallBack m_securityDataCallBack;

        /// <summary>
        /// 股票请求ID
        /// </summary>
        private int m_securitiesRequestID = BaseService.GetRequestID();

        /// <summary>
        /// 证券服务
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// 自选股服务
        /// </summary>
        private UserSecurityServiceEx m_userSecurityService;

        /// <summary>
        /// 板块树
        /// </summary>
        private TreeA m_tvBlock;

        private bool m_isSubmit;

        /// <summary>
        /// 获取或设置是否确认
        /// </summary>
        public bool IsSubmit
        {
            get { return m_isSubmit; }
            set { m_isSubmit = value; }
        }

        private WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// 增加证券到表格中
        /// </summary>
        /// <param name="securities">证券列表</param>
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
        /// 绑定待选证券到表格
        /// </summary>
        /// <param name="securities">证券列表</param>
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
        /// 点击按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                //取消
                if (name == "btnCancel" || name == "btnClose")
                {
                    Close();
                }
                //选中股票
                else if (name == "btnSelectSecurities")
                {
                    SelectSecurities();
                }
                //全选股票
                else if (name == "btnSelectAllSecurities")
                {
                    SelectAllSecurities();
                }
                //确定
                else if (name == "btnSubmit")
                {
                    Submit();
                }
                //取消选中证券
                else if (name == "btnUnSelectSecurities")
                {
                    UnSelectSecurities();
                }
                //取消选中全部股票
                else if (name == "btnUnSelectAllSecurities")
                {
                    UnSelectAllSecurities();
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public override void Close()
        {
            m_window.Close();
            m_window.Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// 销毁方法
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
        /// 获取选中的股票
        /// </summary>
        /// <param name="securities">股票列表</param>
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
        /// 表格选中行改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void GridSelectedRowsChanged(object sender)
        {
            OnGridSelectedRowsChanged();
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// 加载自选股类别
        /// </summary>
        /// <param name="categories">自选股列表</param>
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
        /// 表格选中行改变方法
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            List<GridRow> selectedRows = m_tvBlock.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                TreeNodeA node = selectedRows[0].GetCell(0) as TreeNodeA;
                TreeNodeA parentNode = node.Parent;
                if (parentNode != null && parentNode.Text == "自选股")
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
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
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
        /// 证券数据回调方法
        /// </summary>
        /// <param name="message">消息</param>
        public void OnSecurityDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
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
        /// 从表格中删除证券
        /// </summary>
        /// <param name="securities">证券列表</param>
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
        /// 股票数据回调事件
        /// </summary>
        /// <param name="message">消息</param>
        private void SecurityDataCallBack(CMessage message)
        {
            OnSecurityDataCallBack(message);
        }

        /// <summary>
        /// 选中股票
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
        /// 全选股票
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
        /// 显示
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
        /// 确认提交
        /// </summary>
        public void Submit()
        {
            m_isSubmit = true;
            Close();
        }

        /// <summary>
        /// 取消选中所有股票
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
        /// 取消选中股票
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
