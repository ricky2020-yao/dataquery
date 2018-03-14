/*****************************************************************************\
*                                                                             *
* PlotBase.h -  Base Plot functions, types, and definitions                   *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __PLOTBASE_H__
#define __PLOTBASE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "..\\Base\\NativeBase.h"
#include "..\\Base\\CPaint.h"
#include "..\\Base\\CMathLib.h"
#include "CDiv.h"
#include "Chart.h"
#include "CTable.h"
#include "CPlot.h"

namespace OwLib
{
	class CDiv;
	class CTable;
	class ChartA;
	class NativeBase;
	class CMathLib;
	class CPlot;
	class PlotMark
	{
	public:
		PlotMark(int index, double key, double value)
		{
			Index = index;
			Key = key;
			Value = value;
		}
		int Index;
		double Key;
		double Value;
		PlotMark* Copy()
		{
			PlotMark *plotMark = new PlotMark(Index, Key, Value);
			return plotMark;
		}
	};

	class PlotBase : public CPlot
	{
	protected:
		ActionType m_action;
		AttachVScale m_attachVScale;
		_int64 m_color;
		CTable *m_dataSource;
		CDiv *m_div;
		bool m_drawGhost;
		bool m_enabled;
		FONT *m_font;
		bool m_isPaintingGhost;
		int m_lineStyle;
		int m_lineWidth;
		map<int,PlotMark*> m_marks;
		int m_moveTimes;
		String m_plotType;
		map<String,int> m_sourceFields;
		map<int,PlotMark*> m_startMarks;
		bool m_selected;
		_int64 m_selectedColor;
		SelectPoint m_selectedPoint;
		POINT m_startPoint;
		String m_text;
		bool m_visible;
		int m_zOrder;
	protected:
		void DrawEllipse(CPaint *paint, _int64 dwPenColor, int width, int style, const RECT& rect);
		void DrawEllipse(CPaint *paint, _int64 dwPenColor, int width, int style, int left, int top, int right, int bottom);
		void DrawEllipse(CPaint *paint, _int64 dwPenColor, int width, int style, float left, float top, float right, float bottom);
		void DrawLine(CPaint *paint, _int64 dwPenColor, int width, int style, const POINT& x, const POINT& y);
		void DrawLine(CPaint *paint, _int64 dwPenColor, int width, int style, int x1, int y1, int x2, int y2);
		void DrawLine(CPaint *paint, _int64 dwPenColor, int width, int style, float x1, float y1, float x2, float y2);
		void DrawPolygon(CPaint *paint, _int64 dwPenColor, int width, int style, POINT *apt, int cpt);
		void DrawPolyline(CPaint *paint, _int64 dwPenColor, int width, int style, POINT *apt, int cpt);
		void DrawRay(CPaint *paint, _int64 dwPenColor, int width, int style, float x1, float y1, float x2, float y2, float k, float b);
		void DrawRect(CPaint *paint, _int64 dwPenColor, int width, int style, int left, int top, int right, int bottom);
		void DrawRect(CPaint *paint, _int64 dwPenColor, int width, int style, const RECT& rect);
		void DrawSelect(CPaint *paint, _int64 dwPenColor, int x, int y);
		void DrawSelect(CPaint *paint, _int64 dwPenColor, float x, float y);
		void DrawText(CPaint *paint, LPCWSTR strText, _int64 dwPenColor, FONT *font, int left, int top);
		void FillEllipse(CPaint *paint, _int64 dwPenColor, const RECT& rect);
		void FillPolygon(CPaint *paint, _int64 dwPenColor, POINT *apt, int cpt);
		void FillRect(CPaint *paint, _int64 dwPenColor, const RECT& rect);
		SIZE TextSize(CPaint *paint, LPCWSTR strText, FONT *font);
	protected:
		void MovePlot(float mouseY, float startY, int startIndex, int mouseBeginIndex, int mouseEndIndex, float pureV,
		double max, double min, int dataCount, double *yAddValue, int *newIndex);
	protected:
		void ClearMarks(map<int,PlotMark*> *marks);
		NativeBase* GetNative();
		bool CreatePoint(const POINT& mp);
		bool Create2PointsA(const POINT& mp);
		bool Create2PointsB(const POINT& mp);
		bool Create2CandlePoints(const POINT& mp);
		bool Create3Points(const POINT& mp);
		void CreateCandlePoint(int pos, int index, int close);
		bool Create4CandlePoints(const POINT& mp);	
		double* GetCandleRange(map<int,PlotMark*> *pList);
		POINT GetMouseOverPoint();
		int GetIndex(const POINT& mp);
		float* GetLineParams(PlotMark *markA, PlotMark *markB);
		double* GetLRBandRange(map<int,PlotMark*> *marks, float *param);
		float* GetLRParams(map<int,PlotMark*> *marks);
		POINT GetMovingPoint();
		double GetNumberValue(const POINT& mp);
		int GetPx();
		int GetPy();
		RECT GetRectangle(PlotMark *markA, PlotMark *markB);
		float* GoldenRatioParams(double value1, double value2);
		bool HLinesSelect(float *param, int length);
		void Move(const POINT& mp);
		virtual void OnPaint(CPaint *paint);
		virtual void OnPaintGhost(CPaint *paint);
		virtual void Paint(CPaint *paint, map<int,PlotMark*> *pList, _int64 lineColor);
		float PX(int index);
		float PY(double value);
		float PX(float x);
		float PY(float y);
		void Resize(int index);
		void Resize(const POINT& mp, POINT oppositePoint);
		bool SelectPoint(const POINT& mp, float x, float y);
		bool SelectLine(const POINT& mp, float x, float k, float b);
		bool SelectLine(const POINT& mp, float x1, float y1, float x2, float y2);
		bool SelectRay(const POINT& mp, float x1, float y1, float x2, float y2, float *pk, float *pb);
		bool SelectRay(const POINT& mp, float x1, float y1, float x2, float y2);
		ActionType SelectRect(const POINT& mp, PlotMark *markA, PlotMark *markB);
		bool SelectSegment(const POINT& mp, float x1, float y1, float x2, float y2);
		bool SelectSine(const POINT& mp,float x1, float y1, float x2, float y2);
		bool SelectTriangle(const POINT& mp, float x1, float y1, float x2, float y2, float x3, float y3);
		void SetCursor(CursorsA cursor);
	public:
		PlotBase();
		virtual ~PlotBase();
		AttachVScale GetAttachVScale();
		void SetAttachVScale(AttachVScale attachVScale);
		_int64 GetColor();
		void SetColor(_int64 color);
		CDiv* GetDiv();
		void SetDiv(CDiv *div);
		bool DrawGhost();
		void SetDrawGhost(bool drawGhost);
		bool IsEnabled();
		void SetEnabled(bool enabled);
		FONT* GetFont();
		void SetFont(FONT *font);
		ChartA* GetChart();
		int GetLineStyle();
		void SetLineStyle(int lineStyle);
		int GetLineWidth();
		void SetLineWidth(int lineWidth);
		String GetPlotType();
		void SetPlotType(const String& plotType);
		bool IsSelected();
		void SetSelected(bool selected);
		_int64 GetSelectedColor();
		void SetSelectedColor(_int64 selectedColor);
		enum SelectPoint GetSelectedPoint();
		void SetSelectedPoint(enum SelectPoint selectedPoint);
		String GetText();
		void SetText(const String& text);
		bool IsVisible();
		void SetVisible(bool visible);
		int GetWorkingAreaWidth();
		int GetWorkingAreaHeight();
		int GetZOrder();
		void SetZOrder(int zOrder);
	public:
		virtual ActionType GetAction();
		virtual bool OnCreate(const POINT& mp);
		virtual void OnMoveBegin();
		virtual void OnMoveEnd();
		virtual void OnMoving();
		virtual bool OnSelect();
		virtual void Render(CPaint *paint);
	};
}
#endif