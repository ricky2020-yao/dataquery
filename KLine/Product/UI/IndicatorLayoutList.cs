/***********************************************************************************\
*                                                                                   *
* IndicatorLayoutList.cs - Indicator layout list functions, types, and definitions. *
*                                                                                   *
*               Version 1.00  ����                                                *
*                                                                                   *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.            *
*               Created by Lord 2016/5/26.                                          *
*                                                                                   *
************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// ָ�겼�ֽ���
    /// </summary>
    public class IndicatorLayoutList : WindowXmlEx
    {
        #region Lord 2016/5/26
        /// <summary>
        /// ��������ѡ�ɽ���
        /// </summary>
        /// <param name="native">������</param>
        public IndicatorLayoutList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\IndicatorLayoutList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowILL") as WindowEx;
            m_gridLayouts = GetGrid("gridLayouts");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_gridLayouts.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����¼�
            RegisterEvents(m_window);
            m_indicatorLayoutService = DataCenter.IndicatorLayoutService;
            m_indicatorLayoutDataCallBack = new ListenerMessageCallBack(IndicatorLayoutDataCallBack);
            m_indicatorLayoutService.RegisterListener(m_indicatorLayoutService.OperatorRequestID, m_indicatorLayoutDataCallBack);
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private GridA m_gridLayouts;

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// ָ�겼�ַ���
        /// </summary>
        private IndicatorLayoutServiceEx m_indicatorLayoutService;

        /// <summary>
        /// ����ѡ�����ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_indicatorLayoutDataCallBack;

        /// <summary>
        /// ���ÿؼ��̷߳����¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// ��ǰ�༭�Ĳ���
        /// </summary>
        private IndicatorLayout m_layout = new IndicatorLayout();

        /// <summary>
        /// ������
        /// </summary>
        private INativeBase m_native;

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
        /// ��Ӳ��ֵ����ؼ���
        /// </summary>
        /// <param name="layouts">�����б�</param>
        private void AddLayoutsToGrid(List<IndicatorLayout> layouts)
        {
            m_gridLayouts.BeginUpdate();
            int layoutsSize = layouts.Count;
            for (int i = 0; i < layoutsSize; i++)
            {
                IndicatorLayout layout = layouts[i];
                GridRow row = new GridRow();
                row.Grid = m_gridLayouts;
                GridStringCell idCell = new GridStringCell();
                idCell.Grid = m_gridLayouts;
                idCell.Column = m_gridLayouts.GetColumn(0);
                idCell.Text = layout.m_layoutID;
                row.AddCell(0, idCell);
                GridStringCell textCell = new GridStringCell();
                textCell.Grid = m_gridLayouts;
                textCell.Column = m_gridLayouts.GetColumn(1);
                textCell.Text = layout.m_name;
                row.AddCell(1, textCell);
                m_gridLayouts.AddRow(row);
            }
            m_gridLayouts.EndUpdate();
        }

        /// <summary>
        /// Ӧ��
        /// </summary>
        private void Apply()
        {
            String layoutID = "";
            List<GridRow> selectedRows = m_gridLayouts.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                layoutID = selectedRows[0].GetCell(0).Text;
            }
            IndicatorLayout indicatorLayout = new IndicatorLayout();
            if (m_indicatorLayoutService.GetLayoutByID(layoutID, ref indicatorLayout))
            {
                m_chart.SetIndicatorLayout(indicatorLayout.m_layoutID, indicatorLayout.m_text);
            }
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
                //Ӧ��
                if (name == "btnApply")
                {
                    Apply();
                }
                //�ر�
                else if (name == "btnClose")
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
                else if (name == "btnSave")
                {
                    Save();
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
        /// �½�
        /// </summary>
        public void Create()
        {
            IndicatorLayout layout = new IndicatorLayout();
            layout.m_name = "�µĲ���";
            layout.m_text = m_chart.GetIndicatorLayout();
            layout.m_userID = DataCenter.UserID;
            m_indicatorLayoutService.AddLayout(m_indicatorLayoutService.OperatorRequestID, layout);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        public void Delete()
        {
            List<GridRow> selectedRows = m_gridLayouts.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                String layoutID = selectedRows[0].GetCell(0).Text;
                IndicatorLayout layout = new IndicatorLayout();
                m_indicatorLayoutService.GetLayoutByID(layoutID, ref layout);
                m_indicatorLayoutService.DeleteLayout(m_indicatorLayoutService.OperatorRequestID, layout);
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
                if (m_gridLayouts != null)
                {
                    m_gridLayouts.UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_gridLayouts = null;
                }
                if (m_indicatorLayoutService != null)
                {
                    m_indicatorLayoutService.UnRegisterListener(m_indicatorLayoutService.OperatorRequestID, m_indicatorLayoutDataCallBack);
                    m_indicatorLayoutDataCallBack = null;
                    m_indicatorLayoutService = null;
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
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="template">������Ϣ</param>
        public void GetLayout(ref IndicatorLayout layout)
        {
            TextBoxA txtLayoutName = GetTextBox("txtLayoutName");
            LayoutScaleDiv layoutScaleDiv = FindControl("divLayoutScale") as LayoutScaleDiv;
            if (m_layout.m_layoutID != null && m_layout.m_layoutID.Length > 0)
            {
                layout.m_layoutID = m_layout.m_layoutID;
            }
            layout.m_name = txtLayoutName.Text;
            layout.m_text = layoutScaleDiv.LayoutStr;
            layout.m_userID = DataCenter.UserID;
        }

        /// <summary>
        /// ��ȡ�����еĹ�ϣ��
        /// </summary>
        /// <returns>��ϣ��</returns>
        private Dictionary<String, GridRow> GetIndicatorLayoutsRows()
        {
            Dictionary<String, GridRow> ilRowsMap = new Dictionary<String, GridRow>();
            List<GridRow> rows = m_gridLayouts.GetRows();
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                GridRow row = rows[i];
                ilRowsMap[row.GetCell(0).Text] = row;
            }
            return ilRowsMap;
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
        /// ָ���������ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void IndicatorLayoutDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
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
        /// ���ز���
        /// </summary>
        /// <param name="layout">������Ϣ</param>
        private void LoadLayout(IndicatorLayout layout)
        {
            TextBoxA txtLayoutName = GetTextBox("txtLayoutName");
            LayoutScaleDiv layoutScaleDiv = FindControl("divLayoutScale") as LayoutScaleDiv;
            ButtonA btnDelete = GetButton("btnDelete");
            ButtonA btnApply = GetButton("btnApply");
            ButtonA btnCreate = GetButton("btnCreate");
            ButtonA btnSave = GetButton("btnSave");
            txtLayoutName.Text = layout.m_name;
            layoutScaleDiv.LayoutStr = layout.m_text;
            bool btnEnabled = (layout.m_layoutID != null && layout.m_layoutID.Length > 0);
            btnDelete.Enabled = btnEnabled;
            btnApply.Enabled = btnEnabled;
            btnSave.Enabled = btnEnabled;
        }

        /// <summary>
        /// ���ѡ���иı䷽��
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            String layoutID = "";
            List<GridRow> selectedRows = m_gridLayouts.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                layoutID = selectedRows[0].GetCell(0).Text;
            }
            IndicatorLayout layout = new IndicatorLayout();
            m_indicatorLayoutService.GetLayoutByID(layoutID, ref layout);
            m_layout = layout;
            LoadLayout(m_layout);
            m_window.Invalidate();
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            IndicatorLayoutService.GetLayouts(layouts, message.m_body, message.m_bodyLength);
            int layoutsSize = layouts.Count;
            switch (message.m_functionID)
            {
                case IndicatorLayoutService.FUNCTIONID_INDICATORLAYOUT_ADDLAYOUTS:
                    AddLayoutsToGrid(layouts);
                    break;
                case IndicatorLayoutService.FUNCTIONID_INDICATORLAYOUT_DELETELAYOUTS:
                    {
                        Dictionary<String, GridRow> ilRowsMap = GetIndicatorLayoutsRows();
                        for (int i = 0; i < layoutsSize; i++)
                        {
                            IndicatorLayout layout = layouts[i];
                            if (ilRowsMap.ContainsKey(layout.m_layoutID))
                            {
                                m_gridLayouts.RemoveRow(ilRowsMap[layout.m_layoutID]);
                            }
                        }
                        m_gridLayouts.Update();
                        break;
                    }
                case IndicatorLayoutService.FUNCTIONID_INDICATORLAYOUT_UPDATELAYOUTS:
                    {
                        Dictionary<String, GridRow> ilRowsMap = GetIndicatorLayoutsRows();
                        for (int i = 0; i < layoutsSize; i++)
                        {
                            IndicatorLayout layout = layouts[i];
                            if (ilRowsMap.ContainsKey(layout.m_layoutID))
                            {
                                ilRowsMap[layout.m_layoutID].GetCell(1).Text = layout.m_name;
                            }
                        }
                        break;
                    }
            }
            m_window.Invalidate();
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
        /// ���沼��
        /// </summary>
        public void Save()
        {
            IndicatorLayout layout = new IndicatorLayout();
            GetLayout(ref layout);
            if (layout.m_layoutID != null && layout.m_layoutID.Length > 0)
            {
                m_indicatorLayoutService.UpdateLayout(m_indicatorLayoutService.OperatorRequestID, layout);
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public override void Show()
        {
            List<IndicatorLayout> layouts = new List<IndicatorLayout>();
            m_indicatorLayoutService.GetLayouts(layouts);
            AddLayoutsToGrid(layouts);
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
        }
        #endregion
    }
}
