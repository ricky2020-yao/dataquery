/***********************************************************************************\
*                                                                                   *
* MacroList.cs - Macro list functions, types, and definitions.                      *
*                                                                                   *
*               Version 1.00  ★★★                                                *
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
    /// 图标单元格
    /// </summary>
    public class GridIconCell : GridStringCell
    {
        #region Lord 2016/10/7
        private String m_id;

        /// <summary>
        /// 获取或设置ID
        /// </summary>
        public String ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private bool m_keyPress;

        /// <summary>
        /// 获取或设置键盘是否按下
        /// </summary>
        public bool KeyPress
        {
            get { return m_keyPress; }
            set { m_keyPress = value; }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        /// <param name="clipRect">裁剪区域</param>
        /// <param name="isAlternate">是否交替</param>
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
    /// 宏管理界面
    /// </summary>
    public class MacroList : WindowXmlEx
    {
        #region Lord 2016/10/7
        /// <summary>
        /// 创建宏管理界面
        /// </summary>
        /// <param name="native">方法库</param>
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
            //注册点击事件
            RegisterEvents(m_window);
            m_macroService = DataCenter.MacroService;
            m_macroDataCallBack = new ListenerMessageCallBack(MacroDataCallBack);
            m_macroService.RegisterListener(m_macroService.OperatorRequestID, m_macroDataCallBack);
        }

        /// <summary>
        /// 图标层
        /// </summary>
        private DivA m_divIcon;

        /// <summary>
        /// 图标表格
        /// </summary>
        private GridA m_gridIcons;

        /// <summary>
        /// 宏列表
        /// </summary>
        private GridA m_gridMacros;

        /// <summary>
        /// 单元格点击事件
        /// </summary>
        private GridCellMouseEvent m_gridCellClickEvent;

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        private ControlEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// 调用控件线程方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        /// <summary>
        /// 当前编辑的宏
        /// </summary>
        private Macro m_macro = new Macro();

        /// <summary>
        /// 宏服务
        /// </summary>
        private MacroServiceEx m_macroService;

        /// <summary>
        /// 宏数据回调事件
        /// </summary>
        private ListenerMessageCallBack m_macroDataCallBack;

        /// <summary>
        /// 方法库
        /// </summary>
        private INativeBase m_native;

        private OwChart m_chart;

        /// <summary>
        /// 股票控件
        /// </summary>
        public OwChart Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
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
        /// 添加宏到表格控件中
        /// </summary>
        /// <param name="macros">布局列表</param>
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
                //关闭
                if (name == "btnClose")
                {
                    Close();
                }
                //新建
                else if (name == "btnCreate")
                {
                    Create();
                }
                //删除
                else if (name == "btnDelete")
                {
                    Delete();
                }
                //保存
                else if (name == "btnSave")
                {
                    Save();
                }
                //测试
                else if (name == "btnTest")
                {
                    Test();
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
        /// 新建
        /// </summary>
        public void Create()
        {
            Macro macro = new Macro();
            macro.m_name = "新的宏";
            macro.m_userID = DataCenter.UserID;
            m_macroService.AddMacro(m_macroService.OperatorRequestID, macro);
        }

        /// <summary>
        /// 删除
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
        /// 销毁方法
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
        /// 获取宏信息
        /// </summary>
        /// <param name="macro">宏信息</param>
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
        /// 获取宏行的哈希表
        /// </summary>
        /// <returns>哈希表</returns>
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
        /// 单元格点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            OnGridCellClick(cell, mp, button, clicks, delta);
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
        /// 加载图标
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
        /// 加载宏
        /// </summary>
        /// <param name="macro">宏信息</param>
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
        /// 宏数据回调
        /// </summary>
        /// <param name="message">消息</param>
        private void MacroDataCallBack(CMessage message)
        {
            m_window.BeginInvoke(message);
        }

        /// <summary>
        /// 单元格点击方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void OnGridCellClick(GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_divIcon.BackImage = cell.GetString();
            m_divIcon.Invalidate();
        }

        /// <summary>
        /// 表格选中行改变方法
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
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
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
        /// 保存布局
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
        /// 显示
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
        /// 测试
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
