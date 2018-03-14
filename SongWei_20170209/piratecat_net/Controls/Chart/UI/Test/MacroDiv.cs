using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using OwLibCT;

namespace piratecat
{
    public class MacroDiv:GridA
    {
        public MacroDiv()
        {
            AllowDrag = true;
            HeaderVisible = false;
            Opacity = 0.2F;
        }

        private MacroServiceEx m_macroService;

        public override void OnLoad()
        {
            base.OnLoad();
            for (int i = 0; i < 10; i++)
            {
                GridColumn column = new GridColumn();
                column.Width = 64;
                AddColumn(column);
            }
            Update();
            GridRow row = null;
            for (int i = 0; i < 20; i++)
            {
                if (i == 0 || i == 10)
                {
                    row = new GridRow();
                    row.Height = 64;
                    AddRow(row);
                }
                int col = i;
                if (i >= 10)
                {
                    col -= 10;
                }
                GridIconCell gridIconCell = new GridIconCell();
                row.AddCell(col, gridIconCell);
            }
            ResourcePath = DataCenter.GetAppPath() + "\\config\\icons\\";
            m_macroService = DataCenter.MacroService;
            m_macroService.RegisterListener(m_macroService.GetListRequestID, new ListenerMessageCallBack(MacroDatasCallBack));
            m_macroService.RegisterListener(m_macroService.OperatorRequestID, new ListenerMessageCallBack(MacroDatasCallBack));
            RegisterEvent(new ControlInvokeEvent(Invoke), EVENTID.INVOKE);      
        }

         /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void Invoke(object sender, object args)
        {
            CMessage message = (CMessage)args;
            List<Macro> macros = new List<Macro>();
            m_macroService.GetMacros(macros);
            GridRow row = null;
            int macrosSize = macros.Count;
            for (int i = 0; i < 20; i++)
            {
                if (i < 10)
                {
                    row = GetRow(1);
                }
                else
                {
                    row = GetRow(0);
                }
                int col = i;
                if (i >= 10)
                {
                    col -= 10;
                }
                GridIconCell iconCell = row.GetCell(col) as GridIconCell;
                if (i < macrosSize)
                {
                    Macro macro = macros[i];
                    iconCell.ID = macro.m_macroID;
                    iconCell.SetString(macro.m_icon);
                }
                else
                {
                    iconCell.ID = "";
                    iconCell.SetString("");
                }
            }
            Update();
            Invalidate();
        }

        public void MacroDatasCallBack(CMessage message)
        {
            BeginInvoke(message);
        }

        /// <summary>
        /// 单元格点击方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public override void OnCellClick(GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnCellClick(cell, mp, button, clicks, delta);
            String image = cell.GetString();
            if (image != null && image.Length > 0)
            {
                GridIconCell iconCell = cell as GridIconCell;
                String macroID = iconCell.ID;
                Macro macro = new Macro();
                m_macroService.GetMacroByID(macroID, ref macro);
                m_macroService.Run(macro);
            }
        }

        public override void OnLostFocus()
        {
            base.OnLostFocus();
        }

        public override void OnKeyDown(char key)
        {
            if (key >= 49 && key <= 57)
            {
                int index = CStrA.ConvertStrToInt(key.ToString()) - 1;
                GridIconCell iconCell = GetRow(1).GetCell(index) as GridIconCell;
                iconCell.KeyPress = true;
                String macroID = iconCell.ID;
                Macro macro = new Macro();
                if (m_macroService.GetMacroByID(macroID, ref macro))
                {
                    m_macroService.Run(macro);
                }
                Invalidate();
                iconCell.KeyPress = false;
            }
        }

        public override void OnMouseMove(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseMove(mp, button, clicks, delta);
            Invalidate();
        }

        public override void OnMouseEnter(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            Opacity = 0.4F;
            base.OnMouseEnter(mp, button, clicks, delta);
        }

        public override void OnMouseLeave(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            Opacity = 0.2F;
            base.OnMouseLeave(mp, button, clicks, delta);
        }
    }
}
