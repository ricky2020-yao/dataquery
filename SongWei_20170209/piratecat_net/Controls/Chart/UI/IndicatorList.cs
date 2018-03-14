/*****************************************************************************\
*                                                                             *
* IndicatorList.cs - Indicator list functions, types, and definitions.        *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/20.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// 公式编辑界面
    /// </summary>
    public class IndicatorList : WindowXmlEx
    {
        #region Lord 2016/1/20
        /// <summary>
        /// 创建公式编辑界面
        /// </summary>
        /// <param name="native">方法库</param>
        public IndicatorList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\IndicatorList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowIE") as WindowEx;
            m_tvList = GetTree("tvList");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_tvList.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //注册点击事件
            RegisterEvents(m_window);
            //注册秒表
            m_timerEvent = new ControlTimerEvent(CallTimer);
            m_window.RegisterEvent(m_timerEvent, EVENTID.TIMER);
            m_indicatorService = DataCenter.IndicatorService;
            m_indicatorDataCallBackEvent = new ListenerMessageCallBack(IndicatorDataCallBack);
            m_indicatorService.RegisterListener(m_indicatorService.OperatorRequestID, m_indicatorDataCallBackEvent);
        }

        /// <summary>
        /// 表格行选中改变事件
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// 当前指标
        /// </summary>
        private Indicator m_indicator = new Indicator();

        /// <summary>
        /// 指标数据回调事件
        /// </summary>
        private ListenerMessageCallBack m_indicatorDataCallBackEvent;

        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 秒表事件
        /// </summary>
        private ControlTimerEvent m_timerEvent;

        /// <summary>
        /// 指标服务
        /// </summary>
        private IndicatorServiceEx m_indicatorService;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        /// <summary>
        /// 树控件
        /// </summary>
        private TreeA m_tvList;

        private OwChart m_chart;

        /// <summary>
        /// 获取或设置行情控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private CDiv m_div;

        /// <summary>
        /// 获取或设置要编辑的图层
        /// </summary>
        public CDiv Div
        {
            get { return m_div; }
            set { m_div = value; }
        }

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
        /// 添加指标到树控件中
        /// </summary>
        /// <param name="indicators">指标列表</param>
        public void AddIndicatorsToTree(List<Indicator> indicators)
        {
            Dictionary<String, TreeNodeA> nodesMap = GetIndicatorsCategoryNodes();
            int indicatorsSize = indicators.Count;
            int pos = 0;
            for (int i = 0; i < indicatorsSize; i++)
            {
                Indicator indicator = indicators[i];
                if (nodesMap.ContainsKey(indicator.m_category))
                {
                    TreeNodeA indicatorNode = new TreeNodeA();
                    indicatorNode.Grid = m_tvList;
                    indicatorNode.Name = indicator.m_indicatorID;
                    indicatorNode.Text = indicator.m_description;
                    TreeNodeA parentNode = nodesMap[indicator.m_category];
                    parentNode.AppendNode(indicatorNode);
                    if (pos == 0)
                    {
                        m_indicator = indicator;
                        LoadIndicator(indicator);
                    }
                    pos++;
                }
            }
            m_tvList.Update();
        }

        /// <summary>
        /// 调用秒表
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="timerID">秒表ID</param>
        private void CallTimer(object sender, int timerID)
        {
            OnTimer(timerID);
        }

        /// <summary>
        /// 是否使用密码
        /// </summary>
        public void ChangeUsePassword()
        {
            CheckBoxA cbUsePassword = GetCheckBox("cbUsePassword");
            TextBoxA txtPassword = GetTextBox("txtPassword");
            txtPassword.Enabled = !cbUsePassword.Checked;
            txtPassword.Invalidate();
        }

        /// <summary>
        /// 按钮点击事件
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
                //是否使用密码保护
                if (name == "cbUsePassword")
                {
                    ChangeUsePassword();
                }
                //关闭
                else if (name == "btnClose")
                {
                    control.Native.RemoveControl(control);
                    Close();
                }
                //删除
                else if (name == "btnDelete")
                {
                    Delete();
                }
                //保存
                else if (name == "btnSave")
                {
                    Save(false);
                }
                //另存为
                else if (name == "btnSaveAs")
                {
                    Save(true);
                }
                //使用
                else if (name == "btnSubmit")
                {
                    control.Native.RemoveControl(control);
                    Submit();
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public override void Close()
        {
            Dispose();
            m_native.Invalidate();
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        public void Delete()
        {
            List<GridRow> selectedRows = m_tvList.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                TreeNodeA node = selectedRows[0].GetCell(0) as TreeNodeA;
                if (node.GetChildNodes().Count == 0)
                {
                    String indicatorID = node.Name;
                    Indicator indicator = new Indicator();
                    m_indicatorService.GetIndicatorByID(indicatorID, ref indicator);
                    m_indicatorService.DeleteIndicator(m_indicatorService.OperatorRequestID, indicator);
                }
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_indicatorService != null)
                {
                    m_indicatorService.UnRegisterListener(m_indicatorService.OperatorRequestID, m_indicatorDataCallBackEvent);
                    m_indicatorDataCallBackEvent = null;
                    m_indicatorService = null;
                }
                if (m_tvList != null)
                {
                    m_tvList.UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_tvList = null;
                }
                if (m_window != null)
                {
                    m_window.UnRegisterEvent(m_invokeEvent, EVENTID.INVOKE);
                    m_invokeEvent = null;
                    m_window.UnRegisterEvent(m_timerEvent, EVENTID.TIMER);
                    m_timerEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// 获取树类别节点的哈希表
        /// </summary>
        /// <returns>哈希表</returns>
        public Dictionary<String, TreeNodeA> GetIndicatorsCategoryNodes()
        {
            Dictionary<String, TreeNodeA> nodesMap = new Dictionary<String, TreeNodeA>();
            List<TreeNodeA> nodes = m_tvList.GetChildNodes();
            int nodesSize = nodes.Count;
            for (int i = 0; i < nodesSize; i++)
            {
                TreeNodeA node = nodes[i];
                nodesMap[node.Text] = node;
            }
            return nodesMap;
        }

        /// <summary>
        /// 获取树节点的哈希表
        /// </summary>
        /// <returns>哈希表</returns>
        public Dictionary<String, TreeNodeA> GetIndicatorsNodes()
        {
            Dictionary<String, TreeNodeA> indicatorNodesMap = new Dictionary<String, TreeNodeA>();
            Dictionary<String, TreeNodeA> nodesMap = GetIndicatorsCategoryNodes();
            foreach (TreeNodeA node in nodesMap.Values)
            {
                List<TreeNodeA> childNodes = node.GetChildNodes();
                int childNodesSize = childNodes.Count;
                for (int i = 0; i < childNodesSize; i++)
                {
                    TreeNodeA childNode = childNodes[i];
                    indicatorNodesMap[childNode.Name] = childNode;
                }
            }
            return indicatorNodesMap;
        }

        /// <summary>
        /// 获取当前的指标
        /// </summary>
        /// <param name="indicator">指标</param>
        public void GetIndicator(ref Indicator indicator)
        {
            //获取控件
            TextBoxA txtName = GetTextBox("txtName");
            CheckBoxA cbUsePassword = GetCheckBox("cbUsePassword");
            TextBoxA txtPassword = GetTextBox("txtPassword");
            TextBoxA txtDescription = GetTextBox("txtDescription");
            ComboBoxA cbCategory = GetComboBox("cbCategory");
            ComboBoxA cbPaintType = GetComboBox("cbPaintType");
            ComboBoxA cbDigit = GetComboBox("cbDigit");
            TextBoxA txtVersion = GetTextBox("txtVersion");
            TextBoxA txtCoordinate = GetTextBox("txtCoordinate");
            TextBoxA txtSpecialCoordinate = GetTextBox("txtSpecialCoordinate");
            TextBoxA txtText = GetTextBox("txtText");
            String parameters = "";
            for (int i = 1; i <= 16; i++)
            {
                TextBoxA txtPN = GetTextBox("txtPN" + i.ToString());
                if (txtPN.Text != null && txtPN.Text.Length > 0)
                {
                    TextBoxA txtP = GetTextBox("txtP" + i.ToString());
                    TextBoxA txtPMin = GetTextBox("txtPMin" + i.ToString());
                    TextBoxA txtPMax = GetTextBox("txtPMax" + i.ToString());
                    parameters += txtPN.Text + "," + txtPMin.Text + "," + txtPMax.Text + "," + txtP.Text + ";";
                }
            }
            if (indicator.m_indicatorID == null || indicator.m_indicatorID.Length == 0)
            {
                indicator.m_indicatorID = m_indicator.m_indicatorID;
            }
            indicator.m_name = txtName.Text;
            indicator.m_usePassword = cbUsePassword.Checked ? 1 : 0;
            if (indicator.m_usePassword == 1)
            {
                indicator.m_password = txtPassword.Text;
            }
            else
            {
                indicator.m_password = "";
            }
            indicator.m_description = txtDescription.Text;
            indicator.m_category = cbCategory.SelectedText;
            indicator.m_paintType = cbPaintType.SelectedIndex;
            indicator.m_digit = cbDigit.SelectedIndex;
            int version = CStr.ConvertStrToInt(txtVersion.Text);
            indicator.m_version = version;
            indicator.m_coordinate = txtCoordinate.Text;
            indicator.m_specialCoordinate = txtSpecialCoordinate.Text;
            indicator.m_text = txtText.Text;
            indicator.m_parameters = parameters;
            indicator.m_userID = DataCenter.UserID;
            indicator.m_type = 1;
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
        /// 自选股数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void IndicatorDataCallBack(CMessage message)
        {
            OnIndicatorDataCallBack(message);
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
        /// 加载指标到界面
        /// </summary>
        public void LoadIndicator(Indicator indicator)
        {
            TextBoxA txtName = GetTextBox("txtName");
            CheckBoxA cbUsePassword = GetCheckBox("cbUsePassword");
            TextBoxA txtPassword = GetTextBox("txtPassword");
            TextBoxA txtDescription = GetTextBox("txtDescription");
            ComboBoxA cbCategory = GetComboBox("cbCategory");
            ComboBoxA cbPaintType = GetComboBox("cbPaintType");
            ComboBoxA cbDigit = GetComboBox("cbDigit");
            TextBoxA txtVersion = GetTextBox("txtVersion");
            TextBoxA txtCoordinate = GetTextBox("txtCoordinate");
            TextBoxA txtSpecialCoordinate = GetTextBox("txtSpecialCoordinate");
            TextBoxA txtText = GetTextBox("txtText");
            ButtonA btnDelete = GetButton("btnDelete");
            ButtonA btnSave = GetButton("btnSave");
            bool allowEdit = indicator.m_type == 1;
            int strSize = 0;
            String[] strs = null;
            if (indicator.m_parameters != null && indicator.m_parameters.Length > 0)
            {
                strs = indicator.m_parameters.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                strSize = strs.Length;
            }
            for (int i = 1; i <= 16; i++)
            {
                TextBoxA txtPN = GetTextBox("txtPN" + i.ToString());
                TextBoxA txtP = GetTextBox("txtP" + i.ToString());
                TextBoxA txtPMin = GetTextBox("txtPMin" + i.ToString());
                TextBoxA txtPMax = GetTextBox("txtPMax" + i.ToString());
                txtPN.Enabled = allowEdit;
                txtP.Enabled = allowEdit;
                txtPMin.Enabled = allowEdit;
                txtPMax.Enabled = allowEdit;
                if (i <= strSize)
                {
                    if (strs[i - 1].Length > 0)
                    {
                        String[] subStrs = strs[i - 1].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        txtPN.Text = subStrs[0];
                        txtPMin.Text = subStrs[1];
                        txtPMax.Text = subStrs[2];
                        txtP.Text = subStrs[3];
                    }
                }
                else
                {
                    txtPN.Text = "";
                    txtP.Text = "";
                    txtPMin.Text = "";
                    txtPMax.Text = "";
                }
            }
            txtName.Text = indicator.m_name;
            txtName.ReadOnly = !allowEdit;
            cbUsePassword.Checked = indicator.m_usePassword == 1;
            cbUsePassword.Enabled = allowEdit;
            txtPassword.Enabled = cbUsePassword.Checked;
            txtPassword.Text = indicator.m_password;
            txtPassword.ReadOnly = !allowEdit;
            txtDescription.Text = indicator.m_description;
            txtDescription.ReadOnly = !allowEdit;
            cbCategory.SelectedText = indicator.m_category;
            cbCategory.Enabled = allowEdit;
            cbPaintType.SelectedIndex = indicator.m_paintType;
            cbPaintType.Enabled = allowEdit;
            cbDigit.SelectedIndex = indicator.m_digit;
            cbDigit.Enabled = allowEdit;
            txtVersion.Text = indicator.m_version.ToString();
            txtVersion.ReadOnly = !allowEdit;
            txtCoordinate.Text = indicator.m_coordinate;
            txtCoordinate.ReadOnly = !allowEdit;
            txtSpecialCoordinate.Text = indicator.m_specialCoordinate;
            txtSpecialCoordinate.ReadOnly = !allowEdit;
            txtText.Text = indicator.m_text;
            txtText.ReadOnly = !allowEdit;
            btnDelete.Enabled = allowEdit;
            btnSave.Enabled = allowEdit;
        }

        /// <summary>
        /// 表格选中行改变方法
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            if (m_tvList != null)
            {
                String indicatorID = "";
                List<GridRow> selectedRows = m_tvList.SelectedRows;
                int selectedRowsSize = selectedRows.Count;
                if (selectedRowsSize > 0)
                {
                    TreeNodeA node = selectedRows[0].GetCell(0) as TreeNodeA;
                    indicatorID = node.Name;
                }
                Indicator indicator = new Indicator();
                m_indicatorService.GetIndicatorByID(indicatorID, ref indicator);
                m_indicator = indicator;
                LoadIndicator(m_indicator);
                m_window.Invalidate();
            }
        }

        /// <summary>
        /// 自选股数据回调
        /// </summary>
        /// <param name="message">消息</param>
        public void OnIndicatorDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            List<Indicator> indicators = new List<Indicator>();
            IndicatorService.GetIndicators(indicators, message.m_body, message.m_bodyLength);
            int indicatorsSize = indicators.Count;
            switch (message.m_functionID)
            {
                case IndicatorServiceEx.FUNCTIONID_INDICATOR_ADDINDICATORS:
                    AddIndicatorsToTree(indicators);
                    break;
                case IndicatorServiceEx.FUNCTIONID_INDICATOR_DELETEINDICATORS:
                    {
                        Dictionary<String, TreeNodeA> indicatorNodes = GetIndicatorsNodes();
                        m_indicator = new Indicator();
                        for (int i = 0; i < indicatorsSize; i++)
                        {
                            Indicator indicator = indicators[i];
                            if (indicatorNodes.ContainsKey(indicator.m_indicatorID))
                            {
                                m_tvList.RemoveNode(indicatorNodes[indicator.m_indicatorID]);
                            }
                        }
                        break;
                    }
                case IndicatorServiceEx.FUNCTIONID_INDICATOR_UPDATEINDICATORS:
                    {
                        Dictionary<String, TreeNodeA> indicatorNodes = GetIndicatorsNodes();
                        for (int i = 0; i < indicatorsSize; i++)
                        {
                            Indicator indicator = indicators[i];
                            if (indicatorNodes.ContainsKey(indicator.m_indicatorID))
                            {
                                indicatorNodes[indicator.m_indicatorID].Text = indicator.m_description;
                            }
                        }
                        break;
                    }
            }
            m_window.Invalidate();
        }

        /// <summary>
        /// 执行秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public void OnTimer(int timerID)
        {
            if (m_timerID == timerID)
            {
                POINT mp = m_window.MousePoint;
                int height = m_window.Height;
                if (mp.y >= 0 && mp.y <= height)
                {
                    bool paint = false;
                    int left = m_tvList.Left, right = m_tvList.Right;
                    if (mp.x < right + 5)
                    {
                        if (left < 0)
                        {
                            m_tvList.Left = left + Math.Abs(left) * 2 / 3;
                            paint = true;
                        }
                    }
                    else if (mp.x > right + 100)
                    {
                        if (left > -195)
                        {
                            m_tvList.Left = left - (195 - Math.Abs(left)) * 2 / 3;
                            paint = true;
                        }
                    }
                    if (paint)
                    {
                        m_window.Invalidate();
                    }
                }
            }
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
                ButtonA button = subControl as ButtonA;
                GridA grid = subControl as GridA;
                if (button != null)
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
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="saveAsNew">保存为新的指标</param>
        public void Save(bool saveAsNew)
        {
            Indicator indicator = new Indicator();
            GetIndicator(ref indicator);
            if ((indicator.m_indicatorID == null || indicator.m_indicatorID.Length == 0) || saveAsNew)
            {
                m_indicatorService.AddIndicator(m_indicatorService.OperatorRequestID, indicator);
            }
            else
            {
                m_indicatorService.UpdateIndicator(m_indicatorService.OperatorRequestID, indicator);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            List<Indicator> indicators = new List<Indicator>();
            m_indicatorService.GetIndicators(indicators);
            AddIndicatorsToTree(indicators);
            m_tvList.Update();
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            m_window.Invalidate();
            m_window.StartTimer(m_timerID, 10);
        }

        /// <summary>
        /// 确认方法
        /// </summary>
        public void Submit()
        {
            if (m_div != null)
            {
                Indicator indicator = new Indicator();
                GetIndicator(ref indicator);
                if (indicator.m_indicatorID != null && indicator.m_indicatorID.Length > 0)
                {
                    if (indicator.m_paintType == 0)
                    {
                        m_chart.AddMainIndicator(indicator.m_name, indicator.m_description, indicator.m_text, indicator.m_parameters, m_div, true);
                    }
                    else if (indicator.m_paintType == 1)
                    {
                        m_chart.AddViceIndicator(indicator.m_name, indicator.m_text, indicator.m_parameters, m_div, true);
                    }
                }
            }
            else
            {
                m_isSubmit = true;
                Close();
            }
        }
        #endregion
    }
}
