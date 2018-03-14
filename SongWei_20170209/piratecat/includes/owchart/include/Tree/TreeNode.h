/*********************************************************************************\
*                                                                                 *
* TreeNode.h - Tree node functions, types, and definitions                        *
*                                                                                 *
*               Version 4.00 ¡ï¡ï¡ï¡ï¡ï                                           *
*                                                                                 *
*               Copyright (c) 2016-2016, Lord's tree. All rights reserved.        *
*               Check 2016/9/23 by Lord.                                          *
*                                                                                 *
***********************************************************************************/

#ifndef __TREENODE_H__
#define __TREENODE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Grid\\Grid.h"
#include "..\\Grid\\GridRow.h"
#include "..\\Grid\\GridCell.h"
#include "..\\Button\\CheckBox.h"
#include "Tree.h"

namespace OwLib
{
	class TreeA;

	class TreeNodeA : public GridControlCell
	{
	private:
		bool m_allowDragIn;
		bool m_allowDragOut;
		bool m_checked;
		vector<TreeNodeA*> m_nodes;
		bool m_expended;
		int m_indent;
		TreeNodeA *m_parent;
		GridColumn *m_targetColumn;
		String m_text;
		TreeA *m_tree;
		String m_value;
		void CheckChildNodes(vector<TreeNodeA*> nodes, bool isChecked);
		void CollapseExpendChildNodes(vector<TreeNodeA*> nodes, bool collapeseOrExpend);
		TreeNodeA* GetLastNode(vector<TreeNodeA*> nodes);
	public:
		TreeNodeA();
		virtual ~TreeNodeA();
		bool AllowDragIn();
		void SetAllowDragIn(bool allowDragIn);
		bool AllowDragOut();
		void SetAllowDragOut(bool allowDragOut);
		bool IsChecked();
		void SetChecked(bool checked);
		bool IsExpended();
		void SetExpended(bool expended);
		int GetIndent();
		TreeNodeA* GetParent();
		void SetParent(TreeNodeA *parent);
		GridColumn* GetTargetColumn();
		void SetTargetColumn(GridColumn *targetColumn);
		TreeA* GetTree();
		void SetTree(TreeA *tree);
		String GetValue();
		void SetValue(const String& value);
	public:
		void AppendNode(TreeNodeA *node);
		void ClearNodes();
		void Collapse();
		void Expend();
		vector<TreeNodeA*> GetChildNodes();
		int GetNodeIndex(TreeNodeA *node);
		virtual String GetPaintText();
		virtual void GetProperty(const String& name, String *value, String *type);
		virtual vector<String> GetPropertyNames();
		virtual String GetString();
		void InsertNode(int index, TreeNodeA *node);
		virtual void OnAddingNode(int index);
		virtual void OnPaintCheckBox(CPaint *paint, const RECT& rect);
		virtual void OnPaintNode(CPaint *paint, const RECT& rect);
		virtual void OnPaint(CPaint *paint, const RECT& rect, const RECT& clipRect, bool isAlternate);
		virtual void OnRemovingNode();
		void RemoveNode(TreeNodeA *node);
		virtual void SetProperty(const String& name, const String& value);
		virtual void SetString(const String& value);
	};
}

#endif