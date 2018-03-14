/*****************************************************************************\
*                                                                             *
* MainFrame.cs -  MainFrame functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
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
    /// ����ϵͳ
    /// </summary>
    public class MainFrame : UIXmlEx, IDisposable
    {
        private OwChart m_owChart = null;
        /// <summary>
        /// ��������ϵͳ
        /// </summary>
        public MainFrame()
        {
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ/param>
        private void ClickEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
            }
        }

        /// <summary>
        /// ������Դ����
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// �˳�����
        /// </summary>
        public override void Exit()
        {
            DataCenter.DisConnect();
        }

        /// <summary>
        /// �Ƿ��д�����ʾ
        /// </summary>
        /// <returns>�Ƿ���ʾ</returns>
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
        /// ����XML
        /// </summary>
        /// <param name="xmlPath">XML·��</param>
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
        /// ��������
        /// </summary>
        public override void LoadData()
        {
        }

        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
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
        /// ��ʾ���촰��
        /// </summary>
        public void ShowChatWindow()
        {
            ChatWindow chatWindow = new ChatWindow(Native);
            chatWindow.MainFrame = this;
            chatWindow.Show();
        }

        /// <summary>
        /// ��ʾ��ʾ����
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <param name="caption">����</param>
        /// <param name="uType">��ʽ</param>
        /// <returns>���</returns>
        public int ShowMessageBox(String text, String caption, int uType)
        {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// ��ʾ��¼����
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
