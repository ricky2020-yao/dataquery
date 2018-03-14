/*****************************************************************************\
*                                                                             *
* Play.h -      Play functions, types, and definitions                        *
*                                                                             *
*               Version 4.00 ¡ï¡ï¡ï                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's snake. All rights reserved.   *
*                                                                             *
*******************************************************************************/

#ifndef __PLAY_H__
#define __PLAY_H__
#pragma once
#include "SEnums.h"
#include "Battle.h"

class Battle;

class Play:public ControlA
{
private:
	Battle *m_battle;
	GameState m_gameState;
public:
	Play(Battle* battle);
	virtual ~Play();
	GameState GetGameState();
	void SetGameState(GameState gameState);
public:
	virtual void OnKeyDown(char key);
	virtual void OnMouseDown(const POINT& mp, MouseButtonsA button, int clicks, int delta);
	virtual void OnPaintBackground(CPaint *paint, const RECT& clipRect);
};

#endif