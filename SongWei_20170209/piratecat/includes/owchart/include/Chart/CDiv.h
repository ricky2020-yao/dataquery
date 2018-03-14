/*****************************************************************************\
*                                                                             *
* CDiv.h -      Chart div functions, types, and definitions                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*               Modified by Wang Shaoxu.                                      *
*******************************************************************************/

#ifndef __CDIV_H__
#define __CDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CPaint.h"
#include "Enums.h"
#include "CrossLine.h"
#include "ScaleGrid.h"
#include "Chart.h"
#include "VScale.h"
#include "SelectArea.h"
#include "CToolTip.h"
#include "TitleBar.h"
#include "HScale.h"
#include "CPlot.h"
#include "BaseShape.h"

namespace OwLib
{
	class ChartA;
	class CrossLine;
	class CToolTip;
	class CPlot;
	class BaseShape;
	class HScale;
	class VScale;
	class NativeBase;
	class TitleBar;
	class SelectArea;
	class ScaleGrid;
	class CDiv : public CProperty
	{
	private:
		bool m_allowUserPaint;
		_int64 m_backColor;
		_int64 m_borderColor;
		ChartA *m_chart;
		CrossLine *m_crossLine;
		FONT *m_font;
		ScaleGrid *m_hGrid;
		HScale *m_hScale;
		VScale *m_leftVScale;
		POINT m_location;
		vector<CPlot*> m_plots;
		int m_px;
		int m_py;
		VScale *m_rightVScale;
		SelectArea *m_selectArea;
		bool m_selected;
		vector<BaseShape*> m_shapes;
		bool m_showSelect;
		SIZE m_size;
		TitleBar *m_title;
		CToolTip *m_toolTip;
		int m_verticalPercent;
		ScaleGrid *m_vGrid;
		int m_workingAreaHeight;
	public:
		static bool ShapesAsc(BaseShape *x, BaseShape *y);
		static bool ShapesDesc(BaseShape *x, BaseShape *y);
		static bool PlotsAsc(CPlot *x, CPlot *y);
		static bool PlotsDesc(CPlot *x, CPlot *y);
	public:
		CDiv();
		virtual ~CDiv();
		bool AllowUserPaint();
		void SetAllowUserPaint(bool allowUserPaint);
		_int64 GetBackColor();
		void SetBackColor(_int64 backColor);
		_int64 GetBorderColor();
		void SetBorderColor(_int64 borderColor);
		int GetBottom();
		RECT GetBounds();
		void SetBounds(RECT bounds);
		CrossLine* GetCrossLine();
		void SetCrossLine(CrossLine *crossLine);
		FONT* GetFont();
		void SetFont(FONT *font);
		int GetHeight();
		ScaleGrid* GetHGrid();
		void SetHGrid(ScaleGrid *hGrid);
		HScale* GetHScale();
		void SetHScale(HScale *hScale);
		ChartA* GetChart();
		void SetChart(ChartA *chart);
		int GetLeft();
		VScale* GetLeftVScale();
		void SetLeftVScale(VScale *leftVScale);
		POINT GetLocation();
		int GetPx();
		void SetPx(int px);
		int GetPy();
		void SetPy(int py);
		int GetRight();
		VScale* GetRightVScale();
		void SetRightVScale(VScale *rightVScale);
		SelectArea* GetSelectArea();
		void SetSelectArea(SelectArea *selectArea);
		bool IsSelected();
		void SetSelected(bool selected);
		bool IsShowSelect();
		void SetShowSelect(bool showSelect);
		TitleBar* GetTitleBar();
		void SetTitleBar(TitleBar *title);
		CToolTip* GetToolTip();
		void SetToolTip(CToolTip *toolTip);
		int GetTop();
		int GetVerticalPercent();
		void SetVerticalPercent(int verticalPercent);
		ScaleGrid* GetVGrid();
		void SetVGrid(ScaleGrid *vGrid);
		int GetWidth();
		int GetWorkingAreaHeight();
		void SetWorkingAreaHeight(int workingAreaHeight);
	public:
		void AddPlot(CPlot *plot);
		void AddShape(BaseShape *shape);
		bool ContainsShape(BaseShape *shape);
		vector<CPlot*> GetPlots(SORTTYPE sortType);
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		vector<BaseShape*> GetShapes(SORTTYPE sortType);
		VScale* GetVScale(AttachVScale attachVScale);
		virtual void OnPaint(CPaint *paint, const RECT& rect);
		void RemovePlot(CPlot *plot);
		void RemoveShape(BaseShape *shape);
		virtual void SetProperty(const String& name, const String& value);
	};
}
#endif