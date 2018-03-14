/*****************************************************************************\
*                                                                             *  
* CIndicator.h - Indicator functions, types, and definitions                  *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's owchart. All rights reserved. *
*                                                                             *
*******************************************************************************/
#ifndef __CINDICATOR_H__
#define __CINDICATOR_H__
#pragma once

#include "..\\..\\stdafx.h"
#include "..\\Base\\CStr.h"
#include "..\\Base\\CMathLib.h"
#include "Enums.h"
#include "CTable.h"
#include "CDiv.h"
#include "BaseShape.h"

namespace OwLib
{
	class CIndicator;
	class CMathElement;

	class CVariable
	{
	public:
		CVariable();
		virtual ~CVariable();
		BarShape *m_barShape;
		CandleShape *m_candleShape;
		String m_expression;
		int m_field;
		int m_fieldIndex;
		String m_fieldText;
		int m_functionID;
		CIndicator *m_indicator;
		PointShape *m_pointShape;
		PolylineShape *m_polylineShape;
		String m_name;
		CVariable **m_parameters;
		int m_parametersLength;
		CMathElement **m_splitExpression;
		int m_splitExpressionLength;
		int *m_tempFields;
		int m_tempFieldsLength;
		int *m_tempFieldsIndex;
		int m_tempFieldsIndexLength;
		TextShape *m_textShape;
		int m_type;
		double m_value;
		void CreateTempFields(int count);
	};

	class CMathElement
	{
	public:
		CMathElement();
		virtual ~CMathElement();
		String m_expr;
		int m_type;
		double m_value;
		CVariable *m_var;
	};

	class CFunction
	{
	public:
		CFunction();
		virtual ~CFunction();
		int m_id;
		String m_name;
		bool m_withParameters;
	public:
		virtual double OnCalculate(CVariable *var);
	};

	class CIndicator
	{
	private:
		map<String,double> m_defineParams;
		vector<CFunction*> m_functions;
		map<int, CFunction*> m_functionsMap;
		int m_index;
		vector<CVariable*> m_lines;
		map<String,String> m_replaceKeyWords;
		vector<_int64> m_systemColors;
		vector<CVariable*> m_variables;
		String VARIABLE;
		String FUNCTIONS;
		String FUNCTIONS_FIELD;
	private:
		AttachVScale m_attachVScale;
		CTable *m_dataSource;
		CDiv *m_div;
		String m_fullName;
		String m_name;
		String m_parameters;
		double m_result;
		String m_script;
	private:
		void AnalysisVariables(String *sentence);
		void AnalysisScriptLine(String& line);
		double Calculate(CMathElement **expr, int exprLength);
		double CallFunction(CVariable *var);
		_int64 GetColor(const String& strColor);
		LPDATA GetDatas(int fieldIndex, int mafieldIndex, int index, int n);
		int GetLineWidth(const String& strLine);
		int GetOperator(const String& op);
		double GetValue(CMathElement *exp);
		bool IsNumeric(const String& str);
		double Operate(double opnd1, double oper, double opnd2);
		int Precede(CMathElement *Q1, CMathElement *Q2);
		String Replace(const String& parameter);
		CMathElement** SplitExpression(const String& expression, int *sLength);
		String* SplitExpression2(const String& expression, int *sLength);
	public:
		CIndicator();
		virtual ~CIndicator();
		map<String ,int> m_mainVariables;
		AttachVScale GetAttachVScale();
		void SetAttachVScale(AttachVScale attachVScale);
		CTable* GetDataSource();
		void SetDataSource(CTable *dataSource);
		CDiv* GetDiv();
		void SetDiv(CDiv *div);
		String GetFullName();
		void SetFullName(const String& fullName);
		int GetIndex();
		String GetName();
		void SetName(const String& name);
		String GetParameters();
		void SetParameters(const String& parameters);
		double GetResult();
		void SetResult(double result);
		String GetScript();
		void SetScript(const String& script);
		vector<_int64> GetSystemColors();
		void SetSystemColors(vector<_int64> systemColors);
		String GetTitle();
	public:
		void AddFunction(CFunction *function);
		void Clear();
		vector<CFunction*> GetFunctions();
		vector<BaseShape*> GetShapes();
		double GetValue(CVariable *var);
		void OnCalculate(int index);
		void RemoveFunction(CFunction *function);
		void SetSourceField(const String& key, int value);
		void SetSourceValue(const String& key, double value);
	private:
		double ABS(CVariable *var);
		double AMA(CVariable *var);
		double ACOS(CVariable *var);
		double ASIN(CVariable *var);
		double ATAN(CVariable *var);
		double AVEDEV(CVariable *var);
		int BARSCOUNT(CVariable *var);
		int BARSLAST(CVariable *var);
		int BETWEEN(CVariable *var);
		double CEILING(CVariable *var);
		double COS(CVariable *var);
		int COUNT(CVariable *var);
		int CROSS(CVariable *var);
		int CURRBARSCOUNT(CVariable *var);
		int DATE(CVariable *var);
		int DAY(CVariable *var);
		double DMA(CVariable *var);
		int DOWNNDAY(CVariable *var);
		double DRAWICON(CVariable *var);
		double DRAWKLINE(CVariable *var);
		double DRAWNULL(CVariable *var);
		double DRAWTEXT(CVariable *var);
		int EXIST(CVariable *var);
		double EMA(CVariable *var);
		int EVERY(CVariable *var);
		double EXPMEMA(CVariable *var);
		double EXP(CVariable *var);
		double FLOOR(CVariable *var);
		double HHV(CVariable *var);
		double HHVBARS(CVariable *var);
		int HOUR(CVariable *var);
		double IF(CVariable *var);
		double IFN(CVariable *var);
		double INTPART(CVariable *var);
		int LAST(CVariable *var);
		double LLV(CVariable *var);
		double LLVBARS(CVariable *var);
		double LOG(CVariable *var);
		double MA(CVariable *var);
		double MAX(CVariable *var);
		double MEMA(CVariable *var);
		double MIN(CVariable *var);
		int MINUTE(CVariable *var);
		double MOD(CVariable *var);
		int MONTH(CVariable *var);
		int NDAY(CVariable *var);
		int NOT(CVariable *var);
		double POLYLINE(CVariable *var);
		double POW(CVariable *var);
		int RAND(CVariable *var);
		double REF(CVariable *var);
		double RETURN(CVariable *var);
		double REVERSE(CVariable *var);
		double ROUND(CVariable *var);
		double SAR(CVariable *var);
		int SIGN(CVariable *var);
		double SIN(CVariable *var);
		double SMA(CVariable *var);
		double SQRT(CVariable *var);
		double SQUARE(CVariable *var);
		double STD(CVariable *var);
		double STICKLINE(CVariable *var);
		double SUM(CVariable *var);
		double TAN(CVariable *var);
		int TIME(CVariable *var);
		int TIME2(CVariable *var);
		double TMA(CVariable *var);
		int UPNDAY(CVariable *var);
		double VALUEWHEN(CVariable *var);
		double WMA(CVariable *var);
		int YEAR(CVariable *var);
		double ZIG(CVariable *var);
	};
}
#endif