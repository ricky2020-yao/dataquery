/*****************************************************************************\
*                                                                             *
* MainFrame.cs -  MainFrame functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;

namespace KLine
{
    /// <summary>
    /// 行情系统
    /// </summary>
    public class MainFrame : UIXmlEx, IDisposable
    {
        private OwChart m_owChart = null;
        /// <summary>
        /// 创建行情系统
        /// </summary>
        public MainFrame()
        {
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值/param>
        private void ClickEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
            }
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public override void Exit()
        {
            DataCenter.DisConnect();
        }

        /// <summary>
        /// 是否有窗体显示
        /// </summary>
        /// <returns>是否显示</returns>
        public bool IsWindowShowing()
        {
            List<ControlA> controls = Native.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                WindowFrameA frame = controls[i] as WindowFrameA;
                if (frame != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML路径</param>
        public override void Load(String xmlPath)
        {
            LoadFile(xmlPath, null);
            m_owChart = new OwChart(this);
            DataCenter.MainUI = this;
            ControlA control = Native.GetControls()[0];
            control.BackColor = CDraw.PCOLORS_BACKCOLOR9;
            RegisterEvents(control);
            //CFunctionAjax.SetListener(control);
            //ShowLoginWindow();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void LoadData()
        {
        }

        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickEvent);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                ButtonA button = subControl as ButtonA;
                GridColumn column = subControl as GridColumn;
                GridA grid = subControl as GridA;
                CheckBoxA checkBox = subControl as CheckBoxA;
                if (column != null)
                {
                    column.AllowResize = true;
                    column.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    column.BorderColor = CDraw.PCOLORS_LINECOLOR2;
                    column.ForeColor = CDraw.PCOLORS_FORECOLOR;
                }
                else if (checkBox != null)
                {
                    checkBox.ButtonBackColor = CDraw.PCOLORS_BACKCOLOR;
                }
                else if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (grid != null)
                {
                    grid.BackColor = COLOR.EMPTY;
                    grid.GridLineColor = CDraw.PCOLORS_LINECOLOR2;
                    GridRowStyle rowStyle = new GridRowStyle();
                    grid.RowStyle = rowStyle;
                    rowStyle.BackColor = COLOR.EMPTY;
                    rowStyle.SelectedBackColor = CDraw.PCOLORS_SELECTEDROWCOLOR;
                    rowStyle.HoveredBackColor = CDraw.PCOLORS_HOVEREDROWCOLOR;
                    grid.HorizontalOffset = grid.Width;
                    grid.UseAnimation = true;
                }
                else
                {
                    if (subControl.GetControlType() == "Div" || subControl.GetControlType() == "TabControl"
                        || subControl.GetControlType() == "TabPage"
                        || subControl.GetControlType() == "SplitLayoutDiv")
                    {
                        subControl.BackColor = COLOR.EMPTY;
                    }
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示聊天窗体
        /// </summary>
        public void ShowChatWindow()
        {
            ChatWindow chatWindow = new ChatWindow(Native);
            chatWindow.MainFrame = this;
            chatWindow.Show();
        }

        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="caption">标题</param>
        /// <param name="uType">格式</param>
        /// <returns>结果</returns>
        public int ShowMessageBox(String text, String caption, int uType)
        {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// 显示登录窗体
        /// </summary>
        public void ShowLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow(Native);
            loginWindow.MainFrame = this;
            loginWindow.IsWinForm = true;
            loginWindow.Show();
        }
    }
}
