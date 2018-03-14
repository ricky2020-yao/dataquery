/*****************************************************************************\
*                                                                             *
* BandedGridColumn.h -  Banded grid column functions, types, and definitions  *
*                                                                             *
*               Version 4.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's grid. All rights reserved.    *
*               Check right 2016/12/31 by Lord.                               *
*                                                                             *
*******************************************************************************/

#ifndef __BANDEDGRIDCOLUMN_H__
#define __BANDEDGRIDCOLUMN_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "GridEnums.h"
#include "BandedGrid.h"
#include "GridColumn.h"
#include "GridBand.h"

namespace OwLib
{
	class BandedGridA;
	class GridColumn;
	class GridBand;

	class BandedGridColumn : public GridColumn
	{
	private:
		GridBand *m_band;
	public:
		BandedGridColumn();
		virtual ~BandedGridColumn();
		GridBand* GetBand();
		void SetBand(GridBand *band);
	public:
		virtual String GetControlType();
		virtual bool OnDragBegin();
		virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
		virtual void OnMouseMove(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	};
}

#endif