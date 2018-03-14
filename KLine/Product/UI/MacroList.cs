/***********************************************************************************\
*                                                                                   *
* MacroList.cs - Macro list functions, types, and definitions.                      *
*                                                                                   *
*               Version 1.00  ����                                                *
*                                                                                   *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.            *
*               Created by Lord 2016/10/7.                                          *
*                                                                                   *
************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace KLine
{
    /// <summary>
    /// ͼ�굥Ԫ��
    /// </summary>
    public class GridIconCell : GridStringCell
    {
        #region Lord 2016/10/7
        private String m_id;

        /// <summary>
        /// ��ȡ������ID
        /// </summary>
        public String ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private bool m_keyPress;

        /// <summary>
        /// ��ȡ�����ü����Ƿ���
        /// </summary>
        public bool KeyPress
        {
            get { return m_keyPress; }
            set { m_keyPress = value; }
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="rect">����</param>
        /// <param name="clipRect">�ü�����</param>
        /// <param name="isAlternate">�Ƿ���</param>
        public override void OnPaint(CPaint paint, RECT rect, RECT clipRect, bool isAlternate)
        {
            GridA grid = Grid;
            GridRow row = Row;
            float opacity = grid.Opacity;
            float oldOpacity = opacity;
            if (grid.HoveredRow == row)
            {
                POINT mp = grid.MousePoint;
                if (mp.x >= clipRect.left && mp.x <= clipRect.right
                    && mp.y >= clipRect.top && mp.y <= clipRect.bottom)
                {
                    if (m_id != null && m_id.Length > 0)
                    {
                        opacity = 1;
                    }
                }
            }
            else if (m_keyPress)
            {
                opacity = 1;
            }
            if (m_id != null && m_id.Length > 0)
            {
                grid.Opacity = opacity;
                paint.SetOpacity(opacity);
            }
            String image = GetString();
            if (image != null && image.Length > 0)
            {
                paint.DrawImage(GetString(), rect);
            }
            else
            {
                paint.DrawRect(CDraw.PCOLORS_LINECOLOR, 1, 0, rect);
            }
            if (m_id != null && m_id.Length > 0)
            {
                grid.Opacity = oldOpacity;
                paint.SetOpacity(oldOpacity);
            }
        }
        #endregion
    }

    /// <summary>
    /// ��������
    /// </summary>
    public class MacroList : WindowXmlEx
    {
        #region Lord 2016/10/7
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="native">������</param>
        public MacroList(INativeBase native)
        {
            m_native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\MacroList.xml";
            Native = m_native;
            LoadFile(xmlPath, null);
            m_window = FindControl("windowMacro") as WindowEx;
            m_divIcon = GetDiv("divIcon");
            m_divIcon.ResourcePath = DataCenter.GetAppPath() + "\\config\\icons\\";
            m_gridIcons = GetGrid("gridIcons");
            m_gridCellClickEvent = new GridCellMouseEvent(GridCellClick);
            m_gridIcons.RegisterEvent(m_gridCellClickEvent, EVENTID.GRIDCELLCLICK);
            m_gridMacros = GetGrid("gridMacros");
            m_gridSelectedRowsChangedEvent = new ControlEvent(GridSelectedRowsChanged);
            m_gridMacros.RegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //ע�����¼�
            RegisterEvents(m_window);
            m_macroService = DataCenter.MacroService;
            m_macroDataCallBack = new ListenerMessageCallBack(MacroDataCallBack);
            m_macroService.RegisterListener(m_macroService.OperatorRequestID, m_macroDataCallBack);
        }

        /// <summary>
        /// ͼ���
        /// </summary>
        private DivA m_divIcon;

        /// <summary>
        /// ͼ����
        /// </summary>
        private GridA m_gridIcons;

        /// <summary>
        /// ���б�
        /// </summary>
        private GridA m_gridMacros;

        /// <summary>
        /// ��Ԫ�����¼�
        /// </summary>
        private GridCellMouseEvent m_gridCellClickEvent;

        /// <summary>
        /// ���ѡ���иı��¼�
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// ���ÿؼ��̷߳����¼�
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// ��ǰ�༭�ĺ�
        /// </summary>
        private Macro m_macro = new Macro();

        /// <summary>
        /// �����
        /// </summary>
        private MacroServiceEx m_macroService;

        /// <summary>
        /// �����ݻص��¼�
        /// </summary>
        private ListenerMessageCallBack m_macroDataCallBack;

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
        /// ��Ӻ굽���ؼ���
        /// </summary>
        /// <param name="macros">�����б�</param>
        private void AddMacrosToGrid(List<Macro> macros)
        {
            m_gridMacros.BeginUpdate();
            int macrosSize = macros.Count;
            for (int i = 0; i < macrosSize; i++)
            {
                Macro macro = macros[i];
                GridRow row = new GridRow();
                row.Grid = m_gridMacros;
                GridStringCell idCell = new GridStringCell();
                idCell.Grid = m_gridMacros;
                idCell.Column = m_gridMacros.GetColumn(0);
                idCell.Text = macro.m_macroID;
                row.AddCell(0, idCell);
                GridStringCell textCell = new GridStringCell();
                textCell.Grid = m_gridMacros;
                textCell.Column = m_gridMacros.GetColumn(1);
                textCell.Text = macro.m_name;
                row.AddCell(1, textCell);
                m_gridMacros.AddRow(row);
            }
            m_gridMacros.EndUpdate();
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
                else if (name == "btnSave")
                {
                    Save();
                }
                //����
                else if (name == "btnTest")
                {
                    Test();
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
            Macro macro = new Macro();
            macro.m_name = "�µĺ�";
            macro.m_userID = DataCenter.UserID;
            m_macroService.AddMacro(m_macroService.OperatorRequestID, macro);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        public void Delete()
        {
            List<GridRow> selectedRows = m_gridMacros.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                String macroID = selectedRows[0].GetCell(0).Text;
                Macro macro = new Macro();
                m_macroService.GetMacroByID(macroID, ref macro);
                m_macroService.DeleteMacro(m_macroService.OperatorRequestID, macro);
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
                if (m_gridIcons != null)
                {
                    m_gridIcons.UnRegisterEvent(m_gridCellClickEvent, EVENTID.GRIDCELLCLICK);
                    m_gridCellClickEvent = null;
                    m_gridIcons = null;
                }
                if (m_gridMacros != null)
                {
                    m_gridMacros.UnRegisterEvent(m_gridSelectedRowsChangedEvent, EVENTID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_gridMacros = null;
                }
                if (m_macroService != null)
                {
                    m_macroService.UnRegisterListener(m_macroService.OperatorRequestID, m_macroDataCallBack);
                    m_macroDataCallBack = null;
                    m_macroService = null;
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
        /// ��ȡ����Ϣ
        /// </summary>
        /// <param name="macro">����Ϣ</param>
        public void GetMacro(ref Macro macro)
        {
            TextBoxA txtMacroName = GetTextBox("txtMacroName");
            TextBoxA txtMacroDescription = GetTextBox("txtMacroDescription");
            TextBoxA txtMacroScript = GetTextBox("txtMacroScript");
            SpinA spinInterval = GetSpin("spinInterval");
            SpinA spinCD = GetSpin("spinCD");
            if (m_macro.m_macroID != null && m_macro.m_macroID.Length > 0)
            {
                macro.m_macroID = m_macro.m_macroID;
            }
            ComboBoxA cbType = GetComboBox("cbType");
            macro.m_type = cbType.SelectedIndex;
            macro.m_name = txtMacroName.Text;
            macro.m_description = txtMacroDescription.Text;
            macro.m_script = txtMacroScript.Text;
            macro.m_interval = (int)spinInterval.Value;
            macro.m_cd = (int)spinCD.Value;
            macro.m_icon = m_divIcon.BackImage;
            macro.m_userID = DataCenter.UserID;
        }

        /// <summary>
        /// ��ȡ���еĹ�ϣ��
        /// </summary>
        /// <returns>��ϣ��</returns>
        private Dictionary<String, GridRow> GetMacroRows()
        {
            Dictionary<String, GridRow> mRowsMap = new Dictionary<String, GridRow>();
            List<GridRow> rows = m_gridMacros.GetRows();
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                GridRow row = rows[i];
                mRowsMap[row.GetCell(0).Text] = row;
            }
            return mRowsMap;
        }

        /// <summary>
        /// ��Ԫ�����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            OnGridCellClick(cell, mp, button, clicks, delta);
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
        /// ����ͼ��
        /// </summary>
        private void LoadIcons()
        {
            String dir = DataCenter.GetAppPath() + "\\config\\icons\\";
            m_gridIcons.ResourcePath = dir;
            List<String> files = new List<String>();
            CFileA.GetFiles(dir, files);
            int filesSize = files.Count;
            int columnsSize = m_gridIcons.GetColumns().Count;
            m_gridIcons.BeginUpdate();
            GridRow row = null;
            for (int i = 0; i < filesSize; i++)
            {
                int col = i;
                if (i >= columnsSize)
                {
                    col = i % columnsSize;
                }
                if (col == 0)
                {
                    row = new GridRow();
                    row.Height = 64;
                    m_gridIcons.AddRow(row);
                }
                String file = files[i];
                file = file.Substring(file.LastIndexOf('\\') + 1);
                GridIconCell iconCell = new GridIconCell();
                iconCell.SetString(file);
                row.AddCell(col, iconCell);
            }
            m_gridIcons.EndUpdate();
            m_gridIcons.Invalidate();
        }

        /// <summary>
        /// ���غ�
        /// </summary>
        /// <param name="macro">����Ϣ</param>
        private void LoadMacro(Macro macro)
        {
            TextBoxA txtMacroName = GetTextBox("txtMacroName");
            TextBoxA txtMacroDescription = GetTextBox("txtMacroDescription");
            TextBoxA txtMacroScript = GetTextBox("txtMacroScript");
            SpinA spinInterval = GetSpin("spinInterval");
            SpinA spinCD = GetSpin("spinCD");
            ButtonA btnDelete = GetButton("btnDelete");
            ButtonA btnApply = GetButton("btnApply");
            ButtonA btnCreate = GetButton("btnCreate");
            ButtonA btnSave = GetButton("btnSave");
            ComboBoxA cbType = GetComboBox("cbType");
            m_divIcon.BackImage = macro.m_icon;
            txtMacroName.Text = macro.m_name;
            txtMacroDescription.Text = macro.m_description;
            txtMacroScript.Text = macro.m_script;
            spinInterval.Value = macro.m_interval;
            spinCD.Value = macro.m_cd;
            cbType.SelectedIndex = macro.m_type;
            bool btnEnabled = (macro.m_macroID != null && macro.m_macroID.Length > 0);
            btnDelete.Enabled = btnEnabled;
            btnSave.Enabled = btnEnabled;
        }

        /// <summary>
        /// �����ݻص�
        /// </summary>
        /// <param name="message">��Ϣ</param>
        private void MacroDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
        }

        /// <summary>
        /// ��Ԫ��������
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void OnGridCellClick(GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_divIcon.BackImage = cell.GetString();
            m_divIcon.Invalidate();
        }

        /// <summary>
        /// ���ѡ���иı䷽��
        /// </summary>
        public void OnGridSelectedRowsChanged()
        {
            String macroID = "";
            List<GridRow> selectedRows = m_gridMacros.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                macroID = selectedRows[0].GetCell(0).Text;
            }
            Macro macro = new Macro();
            m_macroService.GetMacroByID(macroID, ref macro);
            m_macro = macro;
            LoadMacro(m_macro);
            m_window.Invalidate();
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public void OnInvoke(object args)
        {
            CMessage message = (CMessage)args;
            List<Macro> macros = new List<Macro>();
            MacroService.GetMacros(macros, message.m_body, message.m_bodyLength);
            int macrosSize = macros.Count;
            switch (message.m_functionID)
            {
                case MacroService.FUNCTIONID_MACRO_ADDMACROS:
                    AddMacrosToGrid(macros);
                    break;
                case MacroService.FUNCTIONID_MACRO_DELETEMACROS:
                    {
                        Dictionary<String, GridRow> mRowsMap = GetMacroRows();
                        for (int i = 0; i < macrosSize; i++)
                        {
                            Macro macro = macros[i];
                            if (mRowsMap.ContainsKey(macro.m_macroID))
                            {
                                m_gridMacros.RemoveRow(mRowsMap[macro.m_macroID]);
                            }
                        }
                        m_gridMacros.Update();
                        break;
                    }
                case MacroService.FUNCTIONID_MACRO_UPDATEMACROS:
                    {
                        Dictionary<String, GridRow> mRowsMap = GetMacroRows();
                        for (int i = 0; i < macrosSize; i++)
                        {
                            Macro macro = macros[i];
                            if (mRowsMap.ContainsKey(macro.m_macroID))
                            {
                                mRowsMap[macro.m_macroID].GetCell(1).Text = macro.m_name;
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
            Macro macro = new Macro();
            GetMacro(ref macro);
            if (macro.m_macroID != null && macro.m_macroID.Length > 0)
            {
                m_macroService.UpdateMacro(m_macroService.OperatorRequestID, macro);
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public override void Show()
        {
            List<Macro> macros = new List<Macro>();
            m_macroService.GetMacros(macros);
            AddMacrosToGrid(macros);
            LoadIcons();
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Test()
        {
            Macro macro = new Macro();
            GetMacro(ref macro);
            m_macroService.Run(macro);
        }
        #endregion
    }
}
