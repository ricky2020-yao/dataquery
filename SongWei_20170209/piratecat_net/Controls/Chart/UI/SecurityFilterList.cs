/********************************************************************************\
*                                                                                *
* SecurityFilterList.cs - Security filter list functions, types, and definitions.*
*                                                                                *
*               Version 1.00  ����                                             *
*                                                                                *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.         *
*               Created by Lord 2016/1/22.                                       *
*                                                                                *
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    /// <summary>
    /// ����ѡ�ɽ���
    /// </summary>
    public class SecurityFilterList : WindowXmlEx
    {
        #region Lord 2016/1/22
        /// <summary>
        /// ��������ѡ�ɽ���
        /// </summary>
        /// <param name="native">������</param>
        public SecurityFilterList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\SecurityFilterList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowSF") as WindowEx;
            m_gridTemplate = GetGrid("gridTemplates");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_gridTemplate.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����
            m_timerEvent = new ControlTimerEvent(CallTimer);
            m_window.RegisterEvent(m_timerEvent, EVENTID.TIMER);
            //ע�����¼�
            RegisterEvents(m_window);
            m_securityService = DataCenter.SecurityService;
            m_securityFilterService = DataCenter.SecurityFilterService;
            m_securityFilterDataCallBackEvent = new ListenerMessageCallBack(SecurityFilterDataCallBack);
            m_securityFilterService.RegisterListener(m_securityFilterService.OperatorRequestID, m_securityFilterDataCallBackEvent);
        }

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        private String m_codes = "";

        /// <summary>
        /// ģ������
        /// </summary>
        private GridA m_gridTemplate;

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// ���ÿؼ��̷߳����¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        private SecurityServiceEx m_securityService;

        /// <summary>
        /// ����ѡ�����ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_securityFilterDataCallBackEvent;

        /// <summary>
        /// ����ѡ�ɷ���
        /// </summary>
        private SecurityFilterServiceEx m_securityFilterService;

        /// <summary>
        /// ��ǰ�༭��ģ��
        /// </summary>
        private SecurityFilterTemplate m_template = new SecurityFilterTemplate();

        /// <summary>
        /// ����¼�
        /// </summary>
        private ControlTimerEvent m_timerEvent;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private OwChart m_chart;

        /// <summary>
        /// ��Ʊ�ؼ�
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
        }

        /// <summary>
        /// ���ģ�嵽���ؼ���
        /// </summary>
        /// <param name="templates">ģ���б�</param>
        private void AddTemplatesToGrid(List<SecurityFilterTemplate> templates)
        {
            m_gridTemplate.BeginUpdate();
            int templatesSize = templates.Count;
            for (int i = 0; i < templatesSize; i++)
            {
                SecurityFilterTemplate template = templates[i];
                GridRow row = new GridRow();
                row.Grid = m_gridTemplate;
                GridStringCell idCell = new GridStringCell();
                idCell.Grid = m_gridTemplate;
                idCell.Column = m_gridTemplate.GetColumn(0);
                idCell.Text = template.m_templateID;
                row.AddCell(0, idCell);
                GridStringCell textCell = new GridStringCell();
                textCell.Grid = m_gridTemplate;
                textCell.Column = m_gridTemplate.GetColumn(1);
                textCell.Text = template.m_name;
                row.AddCell(1, textCell);
                m_gridTemplate.AddRow(row);
            }
            m_gridTemplate.EndUpdate();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="timerID">���ID</param>
        private void CallTimer(object sender, int timerID)
        {
            OnTimer(timerID);
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
                //�ر�
                if (name == "btnClose")
                {
                    Close();
                }
                //�½�
                else if (name == "btnCreate")
                {
                    Create();
                }
                //ɾ��
                else if (name == "btnDelete")
                {
                    Delete();
                }
                //����
                else if (name == "btnRun")
                {
                    Run();
                }
                //ѡ���Ʊ
                else if (name == "btnSelectSecurities")
                {
                    SelectSecurities();
                }
                //����
                else if (name == "btnSave")
                {
                    Save();
                }
                //ѡ��ָ��
                else if (name == "btnSelectIndicator")
                {
                    SelectIndicator();
                }
                //�鿴
                else if (name == "btnWatch")
                {
                    Watch();
                }
            }
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
        /// �½�ģ��
        /// </summary>
        public void Create()
        {
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            template.m_name = "�µ�ģ��";
            template.m_cycle = 1440;
            template.m_subscription = 1;
            template.m_userID = DataCenter.UserID;
            m_securityFilterService.AddTemplate(m_securityFilterService.OperatorRequestID, template);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        public void Delete()
        {
            List<GridRow> selectedRows = m_gridTemplate.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                String templateID = selectedRows[0].GetCell(0).Text;
                SecurityFilterTemplate template = new SecurityFilterTemplate();
                m_securityFilterService.GetTemplate(templateID, ref template);
                m_securityFilterService.DeleteTemplate(m_securityFilterService.OperatorRequestID, template);
            }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_chart = null;
                if (m_gridTemplate != null)
                {
                    m_gridTemplate.UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_gridTemplate = null;
                }
                if (m_securityFilterService != null)
                {
                    m_securityFilterService.UnRegisterListener(m_securityFilterService.OperatorRequestID, m_securityFilterDataCallBackEvent);
                    m_securityFilterDataCallBackEvent = null;
                    m_securityFilterService = null;
                }
                if (m_window != null)
                {
                    m_window.UnRegisterEvent(m_invokeEvent, EVENTID.INVOKE);
                    m_invokeEvent = null;
                    m_window.StopTimer(m_timerID);
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
        /// ��ȡģ����Ϣ
        /// </summary>
        /// <param name="template">ģ����Ϣ</param>
        public void GetTemplate(ref SecurityFilterTemplate template)
        {
            TextBoxA txtName = GetTextBox("txtName");
            TextBoxA txtIndicator = GetTextBox("txtIndicator");
            TextBoxA txtParameters = GetTextBox("txtParameters");
            ComboBoxA cbCycle = GetComboBox("cbCycle");
            ComboBoxA cbSubscription = GetComboBox("cbSubscription");
            TextBoxA txtFilter = GetTextBox("txtFilter");
            if (m_template.m_templateID != null && m_template.m_templateID.Length > 0)
            {
                template.m_templateID = m_template.m_templateID;
            }
            template.m_name = txtName.Text;
            template.m_indicator = txtIndicator.Text;
            template.m_parameters = txtParameters.Text;
            if (cbCycle.SelectedValue != null)
            {
                template.m_cycle = CStrA.ConvertStrToInt(cbCycle.SelectedValue);
            }
            else
            {
                template.m_cycle = 1440;
            }
            template.m_subscription = cbSubscription.SelectedIndex;
            template.m_filter = txtFilter.Text;
            template.m_codes = m_codes;
            template.m_userID = DataCenter.UserID;
        }

        /// <summary>
        /// ��ȡģ���еĹ�ϣ��
        /// </summary>
        /// <returns>��ϣ��</returns>
        private Dictionary<String, GridRow> GetTemplateRows()
        {
            Dictionary<String, GridRow> templateRowsMap = new Dictionary<String, GridRow>();
            List<GridRow> rows = m_gridTemplate.GetRows();
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                GridRow row = rows[i];
                templateRowsMap[row.GetCell(0).Text] = row;
            }
            return templateRowsMap;
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
        /// ָ���б���ر��¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cancel">�Ƿ�ȡ��</param>
        public void IndicatorListWindowClosed(object sender, ref bool cancel)
        {
            ControlA control = sender as ControlA;
            OnIndicatorListWindowClosed(control.Tag as IndicatorList, ref cancel);
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
        /// ����ģ��
        /// </summary>
        /// <param name="template">ģ����Ϣ</param>
        private void LoadTemplate(SecurityFilterTemplate template)
        {
            TextBoxA txtName = GetTextBox("txtName");
            TextBoxA txtIndicator = GetTextBox("txtIndicator");
            TextBoxA txtParameters = GetTextBox("txtParameters");
            ComboBoxA cbCycle = GetComboBox("cbCycle");
            ComboBoxA cbSubscription = GetComboBox("cbSubscription");
            TextBoxA txtFilter = GetTextBox("txtFilter");
            ButtonA btnDelete = GetButton("btnDelete");
            ButtonA btnRun = GetButton("btnRun");
            ButtonA btnWatch = GetButton("btnWatch");
            ButtonA btnSave = GetButton("btnSave");
            txtName.Text = template.m_name;
            txtIndicator.Text = template.m_indicator;
            txtParameters.Text = template.m_parameters;
            cbCycle.SelectedValue = template.m_cycle.ToString();
            cbSubscription.SelectedIndex = template.m_subscription;
            txtFilter.Text = template.m_filter;
            m_codes = template.m_codes;
            bool btnEnabled = (template.m_templateID != null && template.m_templateID.Length > 0);
            btnDelete.Enabled = btnEnabled;
            btnRun.Enabled = btnEnabled;
            btnWatch.Enabled = btnEnabled;
            btnSave.Enabled = btnEnabled;
        }

        /// <summary>
        /// ���ѡ���иı䷽��
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            String templateID = "";
            List<GridRow> selectedRows = m_gridTemplate.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                templateID = selectedRows[0].GetCell(0).Text;
            }
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            m_securityFilterService.GetTemplate(templateID, ref template);
            m_template = template;
            LoadTemplate(m_template);
            m_window.Invalidate();
        }

        /// <summary>
        /// ָ���б���رշ���
        /// </summary>
        /// <param name="indicatorList">ָ���б���</param>
        /// <param name="cancel">�Ƿ�ȡ��</param>
        public void OnIndicatorListWindowClosed(IndicatorList indicatorList, ref bool cancel)
        {
            if (indicatorList.IsSubmit)
            {
                Indicator indicator = new Indicator();
                indicatorList.GetIndicator(ref indicator);
                TextBoxA txtIndicator = GetTextBox("txtIndicator");
                TextBoxA txtParameters = GetTextBox("txtParameters");
                txtIndicator.Text = indicator.m_text;
                if (indicator.m_parameters != null && indicator.m_parameters.Length > 0)
                {
                    String[] strs = indicator.m_parameters.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int strsSize = strs.Length;
                    String parameters = "";
                    for (int i = 0; i < strsSize; i++)
                    {
                        if (strs[i].IndexOf(",") != -1)
                        {
                            String[] subStrs = strs[i].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            parameters += subStrs[0] + ":" + subStrs[3] + ";";
                        }
                    }
                    txtParameters.Text = parameters;
                }
                else
                {
                    txtParameters.Text = "";
                }
            }
            m_window.StartTimer(m_timerID, 100);
            m_window.Invalidate();
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            SecurityFilterService.GetTemplates(templates, message.m_body, message.m_bodyLength);
            int templatesSize = templates.Count;
            switch (message.m_functionID)
            {
                case SecurityFilterService.FUNCTIONID_SECURITYFILTER_ADDTEMPLATES:
                    AddTemplatesToGrid(templates);
                    break;
                case SecurityFilterService.FUNCTIONID_SECURITYFILTER_DELETETEMPLATES:
                    {
                        Dictionary<String, GridRow> templateRowsMap = GetTemplateRows();
                        for (int i = 0; i < templatesSize; i++)
                        {
                            SecurityFilterTemplate template = templates[i];
                            if (templateRowsMap.ContainsKey(template.m_templateID))
                            {
                                m_gridTemplate.RemoveRow(templateRowsMap[template.m_templateID]);
                            }
                        }
                        m_gridTemplate.Update();
                        break;
                    }
                case SecurityFilterService.FUNCTIONID_SECURITYFILTER_UPDATETEMPLATES:
                    {
                        Dictionary<String, GridRow> templateRowsMap = GetTemplateRows();
                        for (int i = 0; i < templatesSize; i++)
                        {
                            SecurityFilterTemplate template = templates[i];
                            if (templateRowsMap.ContainsKey(template.m_templateID))
                            {
                                templateRowsMap[template.m_templateID].GetCell(1).Text = template.m_name;
                            }
                        }
                        break;
                    }
            }
            m_window.Invalidate();
        }

        /// <summary>
        /// ֤ȯ�б���رշ���
        /// </summary>
        /// <param name="securityList">֤ȯ�б���</param>
        /// <param name="cancel">�Ƿ�ȡ��</param>
        public void OnSecurityListWindowClosed(SecurityList securityList, ref bool cancel)
        {
            if (securityList.IsSubmit)
            {
                List<Security> selectedSecurities = new List<Security>();
                securityList.GetSelectedSecurities(selectedSecurities);
                int selectedSecuritiesSize = selectedSecurities.Count;
                String codes = "";
                for (int i = 0; i < selectedSecuritiesSize; i++)
                {
                    codes += selectedSecurities[i].m_code;
                    if (i != selectedSecuritiesSize - 1)
                    {
                        codes += ",";
                    }
                }
                m_codes = codes;
            }
            m_window.StartTimer(m_timerID, 100);
            m_window.Invalidate();
        }

        /// <summary>
        /// ִ�������
        /// </summary>
        /// <param name="timerID">���ID</param>
        public void OnTimer(int timerID)
        {
            if (m_timerID == timerID)
            {
                SecurityFilterTemplate template = new SecurityFilterTemplate();
                GetTemplate(ref template);
                SecurityFilterTask task = m_securityFilterService.GetTask(template.m_templateID);
                ButtonA btnRun = GetButton("btnRun");
                if (task != null && btnRun!=null)
                {
                    if (task.RunningState >= 0)
                    {
                        if (btnRun.Text != "ֹͣ")
                        {
                            btnRun.Text = "ֹͣ";
                            m_window.Invalidate();
                        }
                    }
                    else
                    {
                        if (btnRun.Text != "����")
                        {
                            btnRun.Text = "����";
                            m_window.Invalidate();
                        }
                    }
                }
                else
                {
                    if (btnRun != null && btnRun.Text != "����")
                    {
                        btnRun.Text = "����";
                        m_window.Invalidate();
                    }
                }
            }
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
        /// ����
        /// </summary>
        public void Run()
        {
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            GetTemplate(ref template);
            SecurityFilterTask task = m_securityFilterService.GetTask(template.m_templateID);
            if (task == null)
            {
                m_securityFilterService.AddTask(template, m_native);
            }
            task = m_securityFilterService.GetTask(template.m_templateID);
            if (task != null)
            {
                if (task.RunningState >= 0)
                {
                    task.Stop();
                }
                else
                {
                    task.Template = template;
                    task.Run();
                }
                m_window.Invalidate();
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            GetTemplate(ref template);
            if (template.m_templateID != null && template.m_templateID.Length > 0)
            {
                m_securityFilterService.UpdateTemplate(m_securityFilterService.OperatorRequestID, template);
            }
        }

        /// <summary>
        /// ��ѡ�����ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void SecurityFilterDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
        }

        /// <summary>
        /// ֤ȯ�б���ر��¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cancel">�Ƿ�ȡ��</param>
        public void SecurityListWindowClosed(object sender, ref bool cancel)
        {
            ControlA control = sender as ControlA;
            OnSecurityListWindowClosed(control.Tag as SecurityList, ref cancel);
        }

        /// <summary>
        /// ѡ��ָ��
        /// </summary>
        public void SelectIndicator()
        {
            IndicatorList indicatorList = new IndicatorList(m_native);
            indicatorList.Show();
            WindowEx window = indicatorList.Window;
            window.IsChildWindow = true;
            window.Tag = indicatorList;
            WindowClosingEvent windowClosingEvent = new WindowClosingEvent(IndicatorListWindowClosed);
            window.RegisterEvent(windowClosingEvent, EVENTID.WINDOWCLOSING);
            m_native.Invalidate();
        }

        /// <summary>
        /// ѡ���Ʊ
        /// </summary>
        public void SelectSecurities()
        {
            SecurityList securityList = new SecurityList(m_native);
            securityList.Show();
            WindowEx window = securityList.Window;
            window.IsChildWindow = true;
            window.Tag = securityList;
            if (m_codes != null && m_codes.Length > 0)
            {
                List<Security> securities = new List<Security>();
                m_securityService.GetSecuritiesByCodes(m_codes, securities);
                securityList.AddSecuritiesToSecuritiesGrid(securities);
            }
            WindowClosingEvent windowClosingEvent = new WindowClosingEvent(SecurityListWindowClosed);
            window.RegisterEvent(windowClosingEvent, EVENTID.WINDOWCLOSING);
            m_native.Invalidate();
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public override void Show()
        {
            List<SecurityFilterTemplate> templates = new List<SecurityFilterTemplate>();
            m_securityFilterService.GetTemplates(templates);
            AddTemplatesToGrid(templates);
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            m_window.Invalidate();
            m_window.StartTimer(m_timerID, 100);
        }

        /// <summary>
        /// �鿴
        /// </summary>
        public void Watch()
        {
            SecurityFilterTemplate template = new SecurityFilterTemplate();
            GetTemplate(ref template);
            SecurityFilterTask task = m_securityFilterService.GetTask(template.m_templateID);
            if (task != null)
            {
                SecurityFilterResult securityFilterResult = new SecurityFilterResult(m_native);
                securityFilterResult.Chart = m_chart;
                securityFilterResult.Task = task;
                securityFilterResult.Show();
                WindowEx window = securityFilterResult.Window;
                window.IsChildWindow = true;
                m_native.Invalidate();
            }
        }
        #endregion
    }
}
