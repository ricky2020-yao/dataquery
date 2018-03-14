/*****************************************************************************\
*                                                                             *  
* Chart.h -     Chart functions, types, and definitions                       *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __CHART_H__
#define __CHART_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "..\\Base\\CMathLib.h"
#include "CTable.h"
#include "CDiv.h"
#include "CPlot.h"
#include "BaseShape.h"
#include "VScale.h"

namespace OwLib
{
	class CDiv;
	class CPlot;
	class BaseShape;
	class BarShape;
	class PointShape;
	class CandleShape;
	class PolylineShape;
	class TextShape;
	class ControlA;
	class VScale;
	class ChartA:public ControlA
	{
	private:
		bool m_autoFillHScale;
		int m_blankSpace;
		bool m_canMoveShape;
		bool m_canResizeV;
		bool m_canResizeH;
		bool m_canScroll;
		bool m_canZoom;
		CrossLineMoveMode m_crossLineMoveMode;
		int m_crossStopIndex;
		int m_cross_y;
		CTable *m_dataSource;
		vector<CDiv*> m_divs;
		int m_firstVisibleIndex;
		int m_hResizeType;
		String m_hScaleFieldText;
		double m_hScalePixel;
		bool m_isMouseMove;
		bool m_isScrollCross;
		POINT m_lastMouseMovePoint;
		POINT m_lastMouseClickPoint;
		double m_lastMouseMoveTime;
		bool m_lastRecordIsVisible;
		int m_lastUnEmptyIndex;
		double m_lastVisibleKey;
		int m_lastVisibleIndex;
		int m_leftVScaleWidth;
		int m_maxVisibleRecord;
		CPlot *m_movingPlot;
		BaseShape *m_movingShape;
		bool m_reverseHScale;
		int m_rightVScaleWidth;
		bool m_scrollAddSpeed;
		int m_scrollStep;
		bool m_showCrossLine;
		bool m_showingSelectArea;
		bool m_showingToolTip;
		int m_timerID;
		int m_tooltip_dely;
		CDiv *m_userResizeDiv;
		int m_workingAreaWidth;
	public:
		virtual void DrawThinLine(CPaint *paint, _int64 dwPenColor, int width, int x1, int y1, int x2, int y2);
		virtual void DrawText(CPaint *paint, LPCWSTR strText, _int64 dwPenColor,FONT *font, int left, int top);
		virtual vector<double> GetVScaleStep(double max, double min, CDiv *div, VScale *vScale);
		virtual void OnPaintBar(CPaint *paint, CDiv *div, BarShape *bs);
		virtual void OnPaintCandle(CPaint *paint, CDiv *div, CandleShape *cs);
		virtual void OnPaintCandleEx(CPaint *paint, CDiv *div, CandleShape *cs, int visibleMaxIndex, double visibleMax, int visibleMinIndex, double visibleMin);
		virtual void OnPaintCrossLine(CPaint *paint, CDiv *div);
		virtual void OnPaintDivBackGround(CPaint *paint, CDiv *div);
		virtual void OnPaintDivBorder(CPaint *paint, CDiv *div);
		virtual void OnPaintHScale(CPaint *paint, CDiv *div);
		virtual void OnPaintIcon(CPaint *paint);
		virtual void OnPaintPlots(CPaint *paint, CDiv *div);
		virtual void OnPaintPolyline(CPaint *paint, CDiv *div, PolylineShape *ls);
		virtual void OnPaintPolyline(CPaint *paint, CDiv *div, _int64 lineColor, _int64 fillColor, int ciClr,
				int lineWidth, PolylineStyle lineStyle, double value, AttachVScale attachVScale,
				int scaleX, int lY, int i, vector<POINT> *points, int *x, int *y);
		virtual void OnPaintResizeLine(CPaint *paint);
		virtual void OnPaintSelectArea(CPaint *paint, CDiv *div);
		virtual void OnPaintShapes(CPaint *paint, CDiv *div);
		virtual void OnPaintText(CPaint *paint, CDiv *div, TextShape *ts);
		virtual void OnPaintTitle(CPaint *paint, CDiv *div);
		virtual void OnPaintToolTip(CPaint *paint);
		virtual void OnPaintVScale(CPaint *paint, CDiv *div);
	private:
		void CorrectVisibleRecord(int dataCount, int *first, int *last);
		void GetCandleMaxStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth,
		int leftVScaleWidth, int rightVScaleWidth, float *x, float *y);
		void GetCandleMinStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth,
		int leftVScaleWidth, int rightVScaleWidth, float *x, float *y);
		int GetChartIndex(int x, int leftScaleWidth, double hScalePixel, int firstVisibleIndex);
		float GetUpCandleHeight(double close, double open, double max, double min, float divPureV);
		float GetDownCandleHeight(double close, double open, double max, double min, float divPureV);
		void ScrollLeft(int step, int dateCount, double hScalePixel, int pureH, int *fIndex, int *lIndex);
		void ScrollRight(int step, int dataCount, double hScalePixel, int pureH, int *fIndex, int *lIndex);
		double GetVScaleValue(int y, double max, double min, float vHeight);
		int ResetCrossOverIndex(int dataCount, int maxVisibleRecord, int crossStopIndex, int firstL, int lastL);
		void ZoomIn(int pureH, int dataCount, int *findex, int *lindex, double *hScalePixel);
		void ZoomOut(int pureH,int dataCount, int *findex, int *lindex, double *hScalePixel);
	public:
		ChartA();
		virtual ~ChartA();
		bool AutoFillHScale();
		void SetAutoFillHScale(bool autoFillHScale);
		int GetBlankSpace();
		void SetBlankSpace(int blankSpace);
		bool CanMoveShape();
		void SetCanMoveShape(bool canMoveShape);
		bool CanResizeV();
		void SetCanResizeV(bool canResizeV);
		bool CanResizeH();
		void SetCanResizeH(bool canResizeH);
		bool CanScroll();
		void SetCanScroll(bool canScroll);
		bool CanZoom();
		void SetCanZoom(bool canZoom);
		CrossLineMoveMode GetCrossLineMoveMode();
		void SetCrossLineMoveMode(CrossLineMoveMode crossLineMoveMode);
		int GetCrossStopIndex();
		void SetCrossStopIndex(int crossStopIndex);
		CTable* GetDataSource();
		void SetDataSource(CTable *dataSource);
		int GetFirstVisibleIndex();
		void SetFirstVisibleIndex(int firstVisibleIndex);
		String GetHScaleFieldText();
		void SetHScaleFieldText(const String& hScaleFieldText);
		double GetHScalePixel();
		void SetHScalePixel(double hScalePixel);
		int GetLastVisibleIndex();
		void SetLastVisibleIndex(int lastVisibleIndex);
		int GetLeftVScaleWidth();
		void SetLeftVScaleWidth(int leftVScaleWidth);
		int GetMaxVisibleRecord();
		void SetMaxVisibleRecord(int maxVisibleRecord);
		CPlot* GetMovingPlot();
		BaseShape* GetMovingShape();
		bool IsReverseHScale();
		void SetReverseHScale(bool reverseHScale);
		int GetRightVScaleWidth();
		void SetRightVScaleWidth(int rightVScaleWidth);
		bool IsScrollAddSpeed();
		void SetScrollAddSpeed(bool scrollAddSpeed);
		BaseShape* GetSelectedShape();
		void SetSelectedShape(BaseShape *baseShape);
		CPlot* GetSelectedPlot();
		void SetSelectedPlot(CPlot *selectedPlot);
		CDiv* GetSelectedDiv();
		void SetSelectedDiv(CDiv *selectedDiv);
		bool IsShowCrossLine();
		void SetShowCrossLine(bool showCrossLine);
		int GetWorkingAreaWidth();
	public:
		virtual CDiv* AddDiv(int vPercent);
		virtual CDiv* AddDiv();
		virtual void Adjust();
		virtual void AddPlot(CPlot *bpl, const POINT& mp, CDiv *div);
		virtual void Clear();
		virtual void ChangeChart(ScrollType scrollType, int limitStep);
		virtual void CheckLastVisibleIndex();
		virtual void CheckToolTip();
		virtual void ClearSelectedShape();
		virtual void ClearSelectedPlot();
		virtual void ClearSelectedDiv();
		virtual void CloseSelectArea();
		virtual double DivMaxOrMin(int index, CDiv *div, int flag);
		virtual CDiv* FindDiv(const POINT& mp);
		virtual CDiv* FindDiv(BaseShape *shape);
		virtual String GetControlType();
		virtual int GetDateType(DateType dateType);
		virtual DateType GetDateType(int dateType);
		virtual vector<CDiv*> GetDivs();
		virtual void GetHScaleDateString(double date, double lDate, DateType *dateType, wchar_t *str);
		virtual int GetIndex(const POINT& mp);
		virtual int GetMaxVisibleCount(double hScalePixel, int pureH);
		virtual CDiv* GetMouseOverDiv();
		virtual int GetMouseOverIndex();
		double GetNumberValue(CDiv *div, const POINT& mp, AttachVScale attachVScale);
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual int GetShapesCount(int field);
		virtual float GetX(int index);
		virtual float GetY(CDiv *div,double value,AttachVScale attach);
		virtual int GetVScaleBaseField(CDiv *div, VScale *vScale);
		virtual double GetVScaleBaseValue(CDiv *div, VScale *vScale, int i);
		virtual bool IsOperating();
		virtual void LocateCrossLine();
		virtual void MoveShape(CDiv *div, BaseShape *shape);
		virtual void ResetCrossOverIndex();
		virtual void RemoveAll();
		virtual bool ResizeDiv();
		virtual void RemoveDiv(CDiv *div);
		virtual void Reset();
		virtual void ResetVisibleRecord();
		virtual void ScrollLeft(int step);
		virtual void ScrollLeftToBegin();
		virtual void ScrollRight(int step);
		virtual void ScrollRightToEnd();
		virtual void ScrollCrossLineLeft(int step);
		virtual void ScrollCrossLineRight(int step);
		virtual BaseShape* SelectShape(int curIndex, int state);
		virtual bool SelectBar(CDiv *div, float mpY, int fieldName, int fieldName2, int styleField, AttachVScale attachVScale, int curIndex);
		virtual bool SelectCandle(CDiv *div, float mpY, int highField, int lowField, int styleField, AttachVScale attachVScale, int curIndex);
		virtual bool SelectPolyline(CDiv *div, const POINT& mp, int fieldName, int lineWidth, AttachVScale attachVScale, int curIndex);
		virtual void SetProperty(const String& name, const String& value);
		virtual void SetVisibleIndex(int firstVisibleIndex, int lastVisibleIndex);
		virtual void Update();
		virtual void ZoomIn();
		virtual void ZoomOut();
	public:
		virtual void OnKeyDown(char key);
		virtual void OnLoad();
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseUp(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseWheel(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnKeyUp(char key);
		virtual void OnPaintForeground(CPaint *paint, const RECT& clipRect);
		virtual void OnTimer(int timerID);
	};
}
#endif