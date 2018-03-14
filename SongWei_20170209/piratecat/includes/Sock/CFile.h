/*****************************************************************************\
*                                                                             *
* CFile.h -      File functions                                               *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __CFILE_H__
#define __CFILE_H__
#pragma once
#include "io.h"
#include <direct.h>
#include <fstream>
#include <sys/stat.h>

namespace OwLibCT
{
	class CFileA
	{
	public:
		static bool Append(const char *file, const char *content);
		static void CreateDirectory(const char *dir);
		static bool IsDirectoryExist(const char *dir);
		static bool IsFileExist(const char *file);
		static bool GetDirectories(const char *dir, vector<string> *dirs);
		static int GetFileLength(const char *file);
		static bool GetFiles(const char *dir, vector<string> *files);
		static int GetFileState(const char *file, struct stat *buf);
		static bool Read(const char *file, string *content);
		static void RemoveFile(const char *file);
		static bool Write(const char *file, const char *content);
	};
}
#endif