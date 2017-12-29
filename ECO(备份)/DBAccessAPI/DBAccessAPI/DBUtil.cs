using ADODB;
using ADOX;
using CommonAPI;
using CommonAPI.WMI;
using JRO;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public static class DBUtil
	{
		public delegate void DelegateSTOPService(int errCode);
		public const string DBERROR = "DBERROR";
		public const string DBNONE = "DBNONE";
		public const string VALUEISNULL = "VNULL";
		public const string TABLEISNULL = "TNULL";
		public const string MANAGE_DB = "history.mdb";
		public static byte[] b_mark = new byte[]
		{
			140,
			142,
			103,
			53,
			44,
			80,
			120,
			121,
			54,
			208,
			254,
			100,
			246,
			2,
			219,
			41,
			252,
			188,
			189,
			169,
			243,
			47,
			111,
			94,
			159,
			157,
			200,
			163,
			216,
			32,
			234,
			212,
			234,
			111,
			22,
			52,
			232,
			101,
			5,
			164,
			5,
			160,
			28,
			239,
			33,
			5,
			223,
			174,
			20,
			32,
			243,
			3,
			234,
			122,
			246,
			186,
			34,
			70,
			211,
			19,
			81,
			248,
			117,
			228,
			204,
			25,
			63,
			35,
			104,
			76,
			107,
			16,
			224,
			117,
			34,
			83,
			218,
			154,
			82,
			117,
			80,
			88,
			103,
			238,
			160,
			37,
			99,
			65,
			95,
			223,
			201,
			129,
			227,
			100,
			184,
			113,
			184,
			91,
			66,
			217,
			68,
			89,
			33,
			109,
			110,
			196,
			164,
			136,
			130,
			193,
			67,
			37,
			141,
			22,
			207,
			57,
			138,
			41,
			247,
			193,
			94,
			144,
			70,
			80,
			222,
			114,
			182,
			112,
			112,
			180,
			197,
			117,
			126,
			5,
			145,
			185,
			240,
			56,
			126,
			14,
			191,
			11,
			188,
			253,
			108,
			116,
			214,
			81,
			98,
			199,
			18,
			214,
			105,
			37,
			59,
			111,
			84,
			229,
			176,
			176,
			139,
			32,
			64,
			125,
			15,
			224,
			171,
			209,
			245,
			146,
			203,
			237,
			197,
			0,
			142,
			43,
			213,
			222,
			233,
			221,
			32,
			39,
			235,
			175,
			142,
			87,
			31,
			124,
			140,
			30,
			120,
			41,
			153,
			199,
			236,
			201,
			209,
			202,
			114,
			209,
			126,
			24,
			124,
			230,
			148,
			16,
			163,
			55,
			73,
			13,
			122,
			141,
			20,
			198,
			111,
			37,
			241,
			148,
			68,
			98,
			69,
			233,
			72,
			207,
			53,
			86,
			248,
			3,
			190,
			135,
			4,
			211,
			249,
			66,
			44,
			113,
			70,
			133,
			216,
			239,
			252,
			240,
			27,
			117,
			35,
			79,
			230,
			189,
			146,
			192,
			243,
			204,
			177,
			245,
			217,
			243,
			223,
			127,
			205,
			83,
			35,
			145,
			237,
			190,
			140,
			170,
			133,
			187,
			143,
			51,
			238,
			242,
			90,
			239,
			88,
			197,
			125,
			180,
			33,
			103,
			203,
			6,
			89,
			1,
			106,
			11,
			202,
			201,
			100,
			113,
			13,
			81,
			80,
			136,
			175,
			30,
			12,
			93,
			47,
			12,
			134,
			250,
			113,
			172,
			154,
			215,
			32,
			243,
			214,
			191,
			149,
			242,
			212,
			82,
			32,
			19,
			206,
			142,
			70,
			58,
			230,
			123,
			248,
			203,
			64,
			88,
			69,
			69,
			192,
			106,
			234,
			155,
			69,
			234,
			84,
			7,
			164,
			134,
			220,
			23,
			224,
			94,
			109,
			102,
			188,
			215,
			128,
			109,
			37,
			95,
			48,
			157,
			248,
			76,
			48,
			192,
			135,
			49,
			141,
			122,
			154,
			193,
			205,
			219,
			76,
			68,
			80,
			110,
			134,
			189,
			194,
			174,
			221,
			137,
			190,
			74,
			160,
			197,
			215,
			49,
			6,
			66,
			209,
			94,
			253,
			62,
			229,
			189,
			177,
			199,
			96,
			109,
			42,
			54,
			27,
			184,
			51,
			128,
			252,
			16,
			144,
			221,
			210,
			122,
			115,
			19,
			67,
			144,
			22,
			219,
			220,
			90,
			231,
			126,
			220,
			51,
			52,
			171,
			136,
			250,
			195,
			230,
			81,
			132,
			231,
			181,
			78,
			211,
			29,
			217,
			50,
			224,
			41,
			175,
			133,
			48,
			191,
			74,
			201,
			40,
			127,
			65,
			122,
			7,
			246,
			86,
			176,
			143,
			32,
			183,
			31,
			0,
			20,
			243,
			87,
			79,
			61,
			216,
			158,
			211,
			34,
			132,
			196,
			78,
			252,
			89,
			229,
			31,
			91,
			22,
			52,
			99,
			191,
			91,
			57,
			92,
			61,
			44,
			94,
			81,
			115,
			11,
			75,
			162,
			46,
			232,
			159,
			240,
			58,
			2,
			35,
			186,
			8,
			207,
			228,
			33,
			184,
			178,
			168,
			133,
			19,
			109,
			22,
			184,
			110,
			215,
			202,
			91,
			75,
			151,
			241,
			34,
			72,
			136,
			242,
			253,
			38,
			245,
			3,
			187,
			229,
			190,
			152,
			25,
			103,
			251,
			188,
			115,
			32,
			79,
			188,
			105,
			240,
			146,
			245,
			133,
			173,
			218,
			107,
			80,
			194,
			57,
			173,
			111,
			196,
			155,
			101,
			162,
			38,
			126,
			165,
			113,
			109,
			19,
			135,
			93,
			190,
			196,
			230,
			120,
			63,
			138,
			30,
			88,
			170,
			252,
			117,
			136,
			8,
			38,
			192,
			213,
			221,
			91,
			14,
			15,
			132,
			184,
			146,
			241,
			134,
			182,
			107,
			94,
			215,
			85,
			158,
			48,
			135,
			44,
			30,
			200,
			253,
			39,
			187,
			93,
			68,
			102,
			64,
			191,
			211,
			1,
			161,
			150,
			132,
			33,
			131,
			30,
			98,
			176,
			68,
			254,
			86,
			118,
			73,
			113,
			155,
			74,
			164,
			154,
			155,
			136,
			191,
			22,
			213,
			53,
			120,
			215,
			196,
			253,
			241,
			167,
			206,
			135,
			133,
			65,
			147,
			153,
			174,
			92,
			131,
			31,
			249,
			25,
			142,
			144,
			108,
			68,
			29,
			121,
			229,
			145,
			170,
			215,
			16,
			162,
			46,
			237,
			140,
			102,
			173,
			71,
			189,
			1,
			167,
			241,
			249,
			48,
			159,
			229,
			224,
			135,
			2,
			50,
			37,
			84,
			224,
			90,
			129,
			243,
			57,
			154,
			170,
			156,
			200,
			109,
			41,
			198,
			169,
			85,
			162,
			99,
			170,
			179,
			49,
			96,
			68,
			152,
			180,
			200,
			47,
			4,
			69,
			142,
			107,
			179,
			189,
			126,
			44,
			250,
			116,
			106,
			191,
			153,
			203,
			106,
			116,
			115,
			26,
			25,
			101,
			203,
			240,
			202,
			88,
			1,
			241,
			235,
			192,
			13,
			220,
			175,
			35,
			12,
			45,
			45,
			158,
			214,
			102,
			232,
			33,
			107,
			73,
			26,
			137,
			92,
			154,
			38,
			237,
			212,
			115,
			85,
			137,
			123,
			81,
			112,
			131,
			189,
			247,
			46,
			176,
			110,
			101,
			246,
			251,
			146,
			169,
			204,
			171,
			209,
			192,
			74,
			156,
			5,
			100,
			128,
			145,
			139,
			111,
			104,
			21,
			244,
			111,
			66,
			122,
			42,
			97,
			199,
			59,
			226,
			88,
			247,
			81,
			29,
			209,
			115,
			173,
			230,
			223,
			1,
			102,
			117,
			141,
			112,
			149,
			134,
			215,
			212,
			199,
			118,
			74,
			138,
			139,
			234,
			237,
			5,
			84,
			156,
			59,
			73,
			185,
			191,
			227,
			176,
			82,
			234,
			9,
			178,
			10,
			16,
			240,
			3,
			144,
			222,
			144,
			126,
			172,
			203,
			230,
			6,
			155,
			108,
			200,
			235,
			27,
			0,
			180,
			93,
			142,
			34,
			81,
			4,
			113,
			150,
			59,
			74,
			160,
			30,
			175,
			140,
			158,
			127,
			242,
			183,
			16,
			52,
			50,
			250,
			29,
			137,
			250,
			49,
			37,
			104,
			120,
			1,
			228,
			223,
			73,
			13,
			221,
			53,
			78,
			74,
			30,
			113,
			125,
			4,
			236,
			197,
			212,
			97,
			36,
			205,
			208,
			35,
			59,
			138,
			147,
			188,
			85,
			229,
			79,
			2,
			72,
			117,
			141,
			210,
			84,
			105,
			227,
			198,
			198,
			174,
			171,
			96,
			144,
			50,
			69,
			12,
			243,
			228,
			232,
			235,
			246,
			230,
			202,
			164,
			246,
			46,
			241,
			145,
			112,
			230,
			104,
			15,
			7,
			43,
			102,
			213,
			159,
			70,
			152,
			210,
			141,
			82,
			75,
			123,
			132,
			111,
			192,
			142,
			196,
			97,
			155,
			251,
			231,
			172,
			133,
			119,
			126,
			171,
			214,
			35,
			59,
			32,
			74,
			107,
			140,
			73,
			14,
			105,
			89,
			40,
			17,
			232,
			246,
			168,
			62,
			142,
			21,
			156,
			238,
			153,
			156,
			88,
			170,
			155,
			145,
			72,
			46,
			217,
			213,
			247,
			158,
			86,
			205,
			230,
			231,
			73,
			96,
			89,
			51,
			127,
			23,
			173,
			7,
			16,
			66,
			111,
			29,
			214,
			214,
			90,
			160,
			185,
			40,
			238,
			202,
			249,
			150,
			156,
			23,
			165
		};
		public static byte[] b_mark_newversion = new byte[]
		{
			32,
			183,
			31,
			0,
			20,
			243,
			87,
			79,
			61,
			216,
			158,
			211,
			34,
			132,
			196,
			78,
			252,
			89,
			229,
			31,
			91,
			22,
			52,
			99,
			191,
			91,
			57,
			92,
			61,
			44,
			94,
			81,
			115,
			11,
			75,
			162,
			46,
			232,
			159,
			240,
			58,
			2,
			35,
			186,
			8,
			207,
			228,
			33,
			184,
			178,
			168,
			133,
			19,
			109,
			22,
			184,
			110,
			215,
			202,
			91,
			75,
			151,
			241,
			34,
			72,
			136,
			242,
			253,
			38,
			245,
			3,
			187,
			229,
			190,
			152,
			25,
			103,
			251,
			188,
			115,
			32,
			79,
			188,
			105,
			240,
			146,
			245,
			133,
			173,
			218,
			107,
			80,
			194,
			57,
			173,
			111,
			196,
			155,
			101,
			162,
			38,
			126,
			165,
			113,
			109,
			19,
			135,
			93,
			190,
			196,
			230,
			120,
			63,
			138,
			30,
			88,
			170,
			252,
			117,
			136,
			8,
			38,
			192,
			213,
			221,
			91,
			14,
			15,
			132,
			184,
			146,
			241,
			134,
			182,
			107,
			94,
			215,
			85,
			158,
			48,
			135,
			44,
			30,
			200,
			253,
			39,
			187,
			93,
			68,
			102,
			64,
			191,
			211,
			1,
			161,
			150,
			132,
			33,
			131,
			30,
			98,
			176,
			68,
			254,
			86,
			118,
			73,
			113,
			155,
			74,
			164,
			154,
			155,
			136,
			191,
			22,
			213,
			53,
			120,
			215,
			196,
			253,
			241,
			167,
			206,
			135,
			133,
			65,
			147,
			153,
			174,
			92,
			131,
			31,
			249,
			25,
			142,
			144,
			108,
			68,
			29,
			121,
			229,
			145,
			170,
			215,
			16,
			162,
			46,
			237,
			140,
			102,
			173,
			71,
			189,
			1,
			167,
			241,
			249,
			48,
			159,
			229,
			224,
			135,
			2,
			50,
			37,
			84,
			224,
			90,
			129,
			243,
			57,
			154,
			170,
			156,
			200,
			109,
			41,
			198,
			169,
			85,
			162,
			99,
			170,
			179,
			49,
			96,
			68,
			152,
			180,
			200,
			47,
			4,
			69,
			142,
			107,
			179,
			189,
			126,
			44,
			250,
			116,
			106,
			191,
			153,
			203,
			106,
			116,
			115,
			26,
			25,
			101,
			203,
			240,
			202,
			88,
			1,
			241,
			235,
			192,
			13,
			220,
			175,
			35,
			12,
			45,
			45,
			158,
			214,
			102,
			232,
			33,
			107,
			73,
			26,
			137,
			92,
			154,
			38,
			237,
			212,
			115,
			85,
			137,
			123,
			81,
			112,
			131,
			189,
			247,
			46,
			176,
			110,
			101,
			246,
			251,
			146,
			169,
			204,
			171,
			209,
			192,
			74,
			156,
			5,
			100,
			128,
			145,
			139,
			111,
			104,
			21,
			244,
			111,
			66,
			122,
			42,
			97,
			199,
			59,
			226,
			88,
			247,
			81,
			29,
			209,
			115,
			173,
			230,
			223,
			1,
			102,
			117,
			141,
			112,
			149,
			134,
			215,
			212,
			199,
			118,
			74,
			138,
			139,
			234,
			237,
			5,
			84,
			156,
			59,
			73,
			185,
			191,
			227,
			176,
			82,
			234,
			9,
			178,
			10,
			16,
			240,
			3,
			144,
			222,
			144,
			126,
			172,
			203,
			230,
			6,
			155,
			108,
			200,
			235,
			27,
			0,
			180,
			93,
			142,
			34,
			81,
			4,
			113,
			150,
			59,
			74,
			160,
			30,
			175,
			140,
			158,
			127,
			242,
			183,
			16,
			52,
			50,
			250,
			29,
			137,
			250,
			49,
			37,
			104,
			120,
			1,
			228,
			223,
			73,
			13,
			221,
			53,
			78,
			74,
			30,
			113,
			125,
			4,
			236,
			197,
			212,
			97,
			36,
			205,
			208,
			35,
			59,
			138,
			147,
			188,
			85,
			229,
			79,
			2,
			72,
			117,
			141,
			210,
			84,
			105,
			227,
			198,
			198,
			174,
			171,
			96,
			144,
			50,
			69,
			12,
			243,
			228,
			232,
			235,
			246,
			230,
			202,
			164,
			246,
			46,
			241,
			145,
			112,
			230,
			104,
			15,
			7,
			43,
			102,
			213,
			159,
			70,
			152,
			210,
			141,
			82,
			75,
			123,
			132,
			111,
			192,
			142,
			196,
			97,
			155,
			251,
			231,
			172,
			133,
			119,
			126,
			171,
			214,
			35,
			59,
			32,
			74,
			107,
			140,
			73,
			14,
			105,
			89,
			40,
			17,
			232,
			246,
			168,
			62,
			142,
			21,
			156,
			238,
			153,
			156,
			88,
			170,
			155,
			145,
			72,
			46,
			217,
			213,
			247,
			158,
			86,
			205,
			230,
			231,
			73,
			96,
			89,
			51,
			127,
			23,
			173,
			7,
			16,
			66,
			111,
			29,
			214,
			214,
			90,
			160,
			185,
			40,
			238,
			202,
			249,
			150,
			156,
			23,
			165,
			140,
			142,
			103,
			53,
			44,
			80,
			120,
			121,
			54,
			208,
			254,
			100,
			246,
			2,
			219,
			41,
			252,
			188,
			189,
			169,
			243,
			47,
			111,
			94,
			159,
			157,
			200,
			163,
			216,
			32,
			234,
			212,
			234,
			111,
			22,
			52,
			232,
			101,
			5,
			164,
			5,
			160,
			28,
			239,
			33,
			5,
			223,
			174,
			20,
			32,
			243,
			3,
			234,
			122,
			246,
			186,
			34,
			70,
			211,
			19,
			81,
			248,
			117,
			228,
			204,
			25,
			63,
			35,
			104,
			76,
			107,
			16,
			224,
			117,
			34,
			83,
			218,
			154,
			82,
			117,
			80,
			88,
			103,
			238,
			160,
			37,
			99,
			65,
			95,
			223,
			201,
			129,
			227,
			100,
			184,
			113,
			184,
			91,
			66,
			217,
			68,
			89,
			33,
			109,
			110,
			196,
			164,
			136,
			130,
			193,
			67,
			37,
			141,
			22,
			207,
			57,
			138,
			41,
			247,
			193,
			94,
			144,
			70,
			80,
			222,
			114,
			182,
			112,
			112,
			180,
			197,
			117,
			126,
			5,
			145,
			185,
			240,
			56,
			126,
			14,
			191,
			11,
			188,
			253,
			108,
			116,
			214,
			81,
			98,
			199,
			18,
			214,
			105,
			37,
			59,
			111,
			84,
			229,
			176,
			176,
			139,
			32,
			64,
			125,
			15,
			224,
			171,
			209,
			245,
			146,
			203,
			237,
			197,
			0,
			142,
			43,
			213,
			222,
			233,
			221,
			32,
			39,
			235,
			175,
			142,
			87,
			31,
			124,
			140,
			30,
			120,
			41,
			153,
			199,
			236,
			201,
			209,
			202,
			114,
			209,
			126,
			24,
			124,
			230,
			148,
			16,
			163,
			55,
			73,
			13,
			122,
			141,
			20,
			198,
			111,
			37,
			241,
			148,
			68,
			98,
			69,
			233,
			72,
			207,
			53,
			86,
			248,
			3,
			190,
			135,
			4,
			211,
			249,
			66,
			44,
			113,
			70,
			133,
			216,
			239,
			252,
			240,
			27,
			117,
			35,
			79,
			230,
			189,
			146,
			192,
			243,
			204,
			177,
			245,
			217,
			243,
			223,
			127,
			205,
			83,
			35,
			145,
			237,
			190,
			140,
			170,
			133,
			187,
			143,
			51,
			238,
			242,
			90,
			239,
			88,
			197,
			125,
			180,
			33,
			103,
			203,
			6,
			89,
			1,
			106,
			11,
			202,
			201,
			100,
			113,
			13,
			81,
			80,
			136,
			175,
			30,
			12,
			93,
			47,
			12,
			134,
			250,
			113,
			172,
			154,
			215,
			32,
			243,
			214,
			191,
			149,
			242,
			212,
			82,
			32,
			19,
			206,
			142,
			70,
			58,
			230,
			123,
			248,
			203,
			64,
			88,
			69,
			69,
			192,
			106,
			234,
			155,
			69,
			234,
			84,
			7,
			164,
			134,
			220,
			23,
			224,
			94,
			109,
			102,
			188,
			215,
			128,
			109,
			37,
			95,
			48,
			157,
			248,
			76,
			48,
			192,
			135,
			49,
			141,
			122,
			154,
			193,
			205,
			219,
			76,
			68,
			80,
			110,
			134,
			189,
			194,
			174,
			221,
			137,
			190,
			74,
			160,
			197,
			215,
			49,
			6,
			66,
			209,
			94,
			253,
			62,
			229,
			189,
			177,
			199,
			96,
			109,
			42,
			54,
			27,
			184,
			51,
			128,
			252,
			16,
			144,
			221,
			210,
			122,
			115,
			19,
			67,
			144,
			22,
			219,
			220,
			90,
			231,
			126,
			220,
			51,
			52,
			171,
			136,
			250,
			195,
			230,
			81,
			132,
			231,
			181,
			78,
			211,
			29,
			217,
			50,
			224,
			41,
			175,
			133,
			48,
			191,
			74,
			201,
			40,
			127,
			65,
			122,
			7,
			246,
			86,
			176,
			143
		};
		public static DBUtil.DelegateSTOPService DSS = null;
		public static object ServiceHandle = null;
		private static string _alphaDigits = "0123456789abcdefghijklmnopqrstuvwxyz";
		public static string genSrvID()
		{
			TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
			double totalMilliseconds = timeSpan.TotalMilliseconds;
			long num = (long)totalMilliseconds;
			num /= 1000L;
			string text = DBUtil.LongToString(num, 36);
			text = text.ToUpper();
			int length = text.Length;
			if (length > 8)
			{
				text = text.Substring(0, 7);
			}
			else
			{
				if (length < 8)
				{
					switch (length)
					{
					case 1:
						text += "0123456";
						break;
					case 2:
						text += "012345";
						break;
					case 3:
						text += "01234";
						break;
					case 4:
						text += "0123";
						break;
					case 5:
						text += "012";
						break;
					case 6:
						text += "01";
						break;
					case 7:
						text += "0";
						break;
					}
				}
			}
			return text;
		}
		private static string LongToString(long value, int toBase)
		{
			string text = "";
			do
			{
				long num = value % (long)toBase;
				text += DBUtil._alphaDigits[(int)num];
				value /= (long)toBase;
			}
			while (value != 0L);
			return DBUtil.ReverseString(text);
		}
		private static string ReverseString(string s)
		{
			string text = "";
			for (int i = 0; i < s.Length; i++)
			{
				text = s.Substring(i, 1) + text;
			}
			return text;
		}
		public static int createdb()
		{
			Connection connection = (Connection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000514-0000-0010-8000-00AA006D2EA4")));
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!File.Exists(text + "history.mdb"))
				{
					Catalog catalog = (Catalog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000602-0000-0010-8000-00AA006D2EA4")));
					catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "history.mdb;Jet OLEDB:Database Password=root");
					connection = (Connection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000514-0000-0010-8000-00AA006D2EA4")));
					if (File.Exists(text + "history.mdb"))
					{
						connection.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "history.mdb;Jet OLEDB:Database Password=root", null, null, -1);
						catalog.ActiveConnection = connection;
						Table table = (Table)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000609-0000-0010-8000-00AA006D2EA4")));
						table.ParentCatalog = catalog;
						table.Name = "compactdb";
						Column column = (Column)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0000061B-0000-0010-8000-00AA006D2EA4")));
						column.ParentCatalog = catalog;
						column.Name = "dbname";
						column.Type = DataTypeEnum.adVarWChar;
						table.Columns.Append(column, DataTypeEnum.adVarWChar, 128);
						table.Keys.Append("dbnamePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
						table.Columns.Append("dbsize", DataTypeEnum.adVarWChar, 128);
						catalog.Tables.Append(table);
						Table table2 = (Table)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000609-0000-0010-8000-00AA006D2EA4")));
						table2.ParentCatalog = catalog;
						table2.Name = "mysqldb";
						Column column2 = (Column)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0000061B-0000-0010-8000-00AA006D2EA4")));
						column2.ParentCatalog = catalog;
						column2.Name = "dbname";
						column2.Type = DataTypeEnum.adVarWChar;
						table2.Columns.Append(column2, DataTypeEnum.adVarWChar, 128);
						table2.Keys.Append("dbnamePrimaryKey", KeyTypeEnum.adKeyPrimary, column2, null, null);
						catalog.Tables.Append(table2);
						connection.Close();
						return 1;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Create history db error : " + ex.Message);
				try
				{
					connection.Close();
				}
				catch
				{
				}
			}
			return -1;
		}
		public static Hashtable GetMySQLInfo()
		{
			Hashtable hashtable = new Hashtable();
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			OleDbDataAdapter oleDbDataAdapter = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string text2 = text + "history.mdb";
				if (!File.Exists(text2) && DBUtil.createdb() < 0)
				{
					Hashtable result = hashtable;
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					bool flag = false;
					try
					{
						flag = !DBUtil.DetermineTableExist(oleDbConnection, "mysqldb");
					}
					catch
					{
						flag = true;
					}
					oleDbCommand = oleDbConnection.CreateCommand();
					if (flag)
					{
						oleDbCommand.CommandText = "create table mysqldb (dbname varchar(128),PRIMARY KEY(dbname))";
						oleDbCommand.ExecuteNonQuery();
						Hashtable result = hashtable;
						return result;
					}
					DataTable dataTable = new DataTable();
					oleDbDataAdapter = new OleDbDataAdapter();
					oleDbCommand.CommandText = "select dbname from mysqldb ";
					oleDbDataAdapter.SelectCommand = oleDbCommand;
					oleDbDataAdapter.Fill(dataTable);
					oleDbDataAdapter.Dispose();
					if (dataTable != null)
					{
						hashtable = new Hashtable();
						foreach (DataRow dataRow in dataTable.Rows)
						{
							string text3 = Convert.ToString(dataRow[0]);
							if (hashtable.ContainsKey(text3))
							{
								hashtable[text3] = text3;
							}
							else
							{
								hashtable.Add(text3, text3);
							}
						}
					}
					dataTable = new DataTable();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Get MySQL database table status error : " + ex.Message);
			}
			finally
			{
				if (oleDbDataAdapter != null)
				{
					try
					{
						oleDbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return hashtable;
		}
		public static string GetTableName(string str_sql)
		{
			string result = "";
			try
			{
				if (str_sql.ToLower().IndexOf("device_data_daily") >= 0)
				{
					int num = str_sql.ToLower().IndexOf("device_data_daily");
					string result2;
					if (str_sql.Length <= num + 18)
					{
						result2 = "device_data_daily";
						return result2;
					}
					if (str_sql.Substring(num + 17, 1).ToLower().Equals(" "))
					{
						result2 = "device_data_daily";
						return result2;
					}
					result2 = str_sql.Substring(num, 25);
					return result2;
				}
				else
				{
					if (str_sql.ToLower().IndexOf("device_data_hourly") >= 0)
					{
						int num2 = str_sql.ToLower().IndexOf("device_data_hourly");
						string result2;
						if (str_sql.Length <= num2 + 19)
						{
							result2 = "device_data_hourly";
							return result2;
						}
						if (str_sql.Substring(num2 + 18, 1).ToLower().Equals(" "))
						{
							result2 = "device_data_hourly";
							return result2;
						}
						result2 = str_sql.Substring(num2, 26);
						return result2;
					}
					else
					{
						if (str_sql.ToLower().IndexOf("bank_data_daily") >= 0)
						{
							int num3 = str_sql.ToLower().IndexOf("bank_data_daily");
							string result2;
							if (str_sql.Length <= num3 + 16)
							{
								result2 = "bank_data_daily";
								return result2;
							}
							if (str_sql.Substring(num3 + 15, 1).ToLower().Equals(" "))
							{
								result2 = "bank_data_daily";
								return result2;
							}
							result2 = str_sql.Substring(num3, 23);
							return result2;
						}
						else
						{
							if (str_sql.ToLower().IndexOf("bank_data_hourly") >= 0)
							{
								int num4 = str_sql.ToLower().IndexOf("bank_data_hourly");
								string result2;
								if (str_sql.Length <= num4 + 17)
								{
									result2 = "bank_data_hourly";
									return result2;
								}
								if (str_sql.Substring(num4 + 16, 1).ToLower().Equals(" "))
								{
									result2 = "bank_data_hourly";
									return result2;
								}
								result2 = str_sql.Substring(num4, 24);
								return result2;
							}
							else
							{
								if (str_sql.ToLower().IndexOf("port_data_daily") >= 0)
								{
									int num5 = str_sql.ToLower().IndexOf("port_data_daily");
									string result2;
									if (str_sql.Length <= num5 + 16)
									{
										result2 = "port_data_daily";
										return result2;
									}
									if (str_sql.Substring(num5 + 15, 1).ToLower().Equals(" "))
									{
										result2 = "port_data_daily";
										return result2;
									}
									result2 = str_sql.Substring(num5, 23);
									return result2;
								}
								else
								{
									if (str_sql.ToLower().IndexOf("port_data_hourly") >= 0)
									{
										int num6 = str_sql.ToLower().IndexOf("port_data_hourly");
										string result2;
										if (str_sql.Length <= num6 + 17)
										{
											result2 = "port_data_hourly";
											return result2;
										}
										if (str_sql.Substring(num6 + 16, 1).ToLower().Equals(" "))
										{
											result2 = "port_data_hourly";
											return result2;
										}
										result2 = str_sql.Substring(num6, 24);
										return result2;
									}
									else
									{
										if (str_sql.ToLower().IndexOf("rack_effect") >= 0)
										{
											string result2 = "rack_effect";
											return result2;
										}
										if (str_sql.ToLower().IndexOf("rackthermal_daily") >= 0)
										{
											string result2 = "rackthermal_daily";
											return result2;
										}
										if (str_sql.ToLower().IndexOf("rackthermal_hourly") >= 0)
										{
											string result2 = "rackthermal_hourly";
											return result2;
										}
										if (str_sql.ToLower().IndexOf("rci_daily") >= 0)
										{
											string result2 = "rci_daily";
											return result2;
										}
										if (str_sql.ToLower().IndexOf("rci_hourly") >= 0)
										{
											string result2 = "rci_hourly";
											return result2;
										}
										if (str_sql.ToLower().IndexOf("device_auto_info") >= 0)
										{
											int num7 = str_sql.ToLower().IndexOf("device_auto_info");
											if (str_sql.Length > num7 + 24)
											{
												string result2 = str_sql.Substring(num7, 24);
												return result2;
											}
										}
										if (str_sql.ToLower().IndexOf("bank_auto_info") >= 0)
										{
											int num8 = str_sql.ToLower().IndexOf("bank_auto_info");
											if (str_sql.Length > num8 + 24)
											{
												string result2 = str_sql.Substring(num8, 22);
												return result2;
											}
										}
										if (str_sql.ToLower().IndexOf("port_auto_info") >= 0)
										{
											int num9 = str_sql.ToLower().IndexOf("port_auto_info");
											if (str_sql.Length > num9 + 24)
											{
												string result2 = str_sql.Substring(num9, 22);
												return result2;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			return result;
		}
		private static void ThrowException()
		{
			MySQLDatabaseException ex = new MySQLDatabaseException("MySQL Fatal Error");
			throw ex;
		}
		public static void StopService()
		{
			if (DBUtil.DSS != null)
			{
				DBUtil.DSS(256);
			}
		}
		public static void testmethod()
		{
			DBUtil.GetTableName("updaadfad sdfadsfasd fsdsd bank_auto_info20160135   ");
			DBUtil.GetTableName("updaadfad sdfadsfasd fsdsd bank_auto_info2016013");
			DBUtil.GetTableName("updaadfad sdfadsfasd fsdsd device_auto_info201601345 asdfasdersd");
			DBUtil.GetTableName("updaadfad sdfadsfasd fsdsd port_auto_info201601345 asdfasdersd");
			if (DBUtil.ServiceHandle != null)
			{
				try
				{
					ServiceBase serviceBase = (ServiceBase)DBUtil.ServiceHandle;
					serviceBase.Stop();
				}
				catch (Exception)
				{
				}
			}
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection("Database=eco;Data Source=127.0.0.1;Port=3306;User Id=root;Password=password;Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;");
				mySqlConnection.Open();
				if (mySqlConnection != null && mySqlConnection.State == ConnectionState.Open)
				{
					mySqlCommand = mySqlConnection.CreateCommand();
					DataTable dataTable = new DataTable();
					MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
					mySqlCommand.CommandText = "check table bank_data_daily";
					mySqlDataAdapter.SelectCommand = mySqlCommand;
					mySqlDataAdapter.Fill(dataTable);
					mySqlDataAdapter.Dispose();
					if (dataTable != null)
					{
						foreach (DataRow dataRow in dataTable.Rows)
						{
							string text = Convert.ToString(dataRow[3]);
							if (text != null)
							{
								text.IndexOf("OK");
							}
						}
					}
					mySqlCommand.CommandText = "insert into bank_auto_info (bank_id,power,insert_time) values (9874321,2424242,'2014-09-08 11:22:33')";
					mySqlCommand.ExecuteNonQuery();
					try
					{
						mySqlCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						mySqlConnection.Close();
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				string arg_13E_0 = ex.Message;
				string arg_146_0 = ex.StackTrace;
				try
				{
					mySqlCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					mySqlConnection.Close();
				}
				catch
				{
				}
			}
		}
		public static int RepairMySQLDataBase(string str_host, int i_port, string str_usr, string str_pwd, string str_dbname, Hashtable ht_table)
		{
			int result = -1;
			if (ht_table == null)
			{
				return -1;
			}
			if (ht_table.Count == 1 && ht_table.ContainsKey("ALL"))
			{
				return 1;
			}
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			bool flag = false;
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			if (ht_table.Count == 0)
			{
				flag = true;
			}
			try
			{
				mySqlConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=",
					str_dbname,
					";Data Source=",
					str_host,
					";Port=",
					i_port,
					";User Id=",
					str_usr,
					";Password=",
					str_pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				mySqlConnection.Open();
				if (mySqlConnection != null && mySqlConnection.State == ConnectionState.Open)
				{
					mySqlCommand = mySqlConnection.CreateCommand();
					int result2;
					if (flag)
					{
						DataTable dataTable = new DataTable();
						MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
						mySqlCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where table_schema = '" + str_dbname + "' ";
						mySqlDataAdapter.SelectCommand = mySqlCommand;
						mySqlDataAdapter.Fill(dataTable);
						mySqlDataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = Convert.ToString(dataRow[0]);
								if (!hashtable2.ContainsKey(text))
								{
									hashtable2.Add(text, text);
								}
							}
						}
						dataTable = new DataTable();
						ICollection keys = hashtable2.Keys;
						foreach (string text2 in keys)
						{
							dataTable = new DataTable();
							mySqlDataAdapter = new MySqlDataAdapter();
							mySqlCommand.CommandText = "check table " + text2;
							mySqlDataAdapter.SelectCommand = mySqlCommand;
							mySqlDataAdapter.Fill(dataTable);
							mySqlDataAdapter.Dispose();
							bool flag2 = false;
							if (dataTable != null)
							{
								foreach (DataRow dataRow2 in dataTable.Rows)
								{
									string text3 = Convert.ToString(dataRow2[3]);
									if (text3 != null && text3.IndexOf("OK") < 0)
									{
										flag2 = true;
									}
								}
							}
							if (flag2 && !hashtable.ContainsKey(text2))
							{
								hashtable.Add(text2, text2);
							}
						}
						ICollection keys2 = hashtable.Keys;
						foreach (string str in keys2)
						{
							try
							{
								dataTable = new DataTable();
								mySqlDataAdapter = new MySqlDataAdapter();
								mySqlCommand.CommandText = "repair table " + str;
								mySqlDataAdapter.SelectCommand = mySqlCommand;
								mySqlDataAdapter.Fill(dataTable);
								mySqlDataAdapter.Dispose();
								if (dataTable != null && dataTable.Rows.Count > 1)
								{
									DataTable dataTable2 = new DataTable();
									mySqlDataAdapter = new MySqlDataAdapter();
									mySqlCommand.CommandText = "check table " + str;
									mySqlDataAdapter.SelectCommand = mySqlCommand;
									mySqlDataAdapter.Fill(dataTable2);
									mySqlDataAdapter.Dispose();
									if (dataTable2 != null)
									{
										foreach (DataRow dataRow3 in dataTable2.Rows)
										{
											string text4 = Convert.ToString(dataRow3[3]);
											if (text4 != null && text4.IndexOf("OK") < 0)
											{
												result2 = -1;
												return result2;
											}
										}
									}
								}
								dataTable = new DataTable();
							}
							catch
							{
							}
						}
						DBUtil.SetMySQLInfo("ALL");
					}
					else
					{
						ICollection keys3 = ht_table.Keys;
						foreach (string text5 in keys3)
						{
							if (text5.IndexOf("ALL") <= -1)
							{
								try
								{
									DataTable dataTable3 = new DataTable();
									MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
									mySqlCommand.CommandText = "repair table " + text5;
									mySqlDataAdapter.SelectCommand = mySqlCommand;
									mySqlDataAdapter.Fill(dataTable3);
									mySqlDataAdapter.Dispose();
									if (dataTable3 != null && dataTable3.Rows.Count > 1)
									{
										DataTable dataTable4 = new DataTable();
										mySqlDataAdapter = new MySqlDataAdapter();
										mySqlCommand.CommandText = "check table " + text5;
										mySqlDataAdapter.SelectCommand = mySqlCommand;
										mySqlDataAdapter.Fill(dataTable4);
										mySqlDataAdapter.Dispose();
										if (dataTable4 != null)
										{
											foreach (DataRow dataRow4 in dataTable4.Rows)
											{
												string text6 = Convert.ToString(dataRow4[3]);
												if (text6 != null && text6.IndexOf("OK") < 0)
												{
													result2 = -1;
													return result2;
												}
											}
										}
									}
									dataTable3 = new DataTable();
								}
								catch
								{
								}
							}
						}
						DBUtil.SetMySQLInfo("ALL");
					}
					try
					{
						mySqlCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						mySqlConnection.Close();
					}
					catch
					{
					}
					result2 = 1;
					return result2;
				}
			}
			catch (Exception)
			{
				result = -1;
			}
			finally
			{
				if (mySqlCommand != null)
				{
					try
					{
						mySqlCommand.Dispose();
					}
					catch
					{
					}
				}
				if (mySqlConnection != null)
				{
					try
					{
						mySqlConnection.Close();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int CheckSysDB()
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = -1;
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					oleDbConnection.Close();
					int result = 1;
					return result;
				}
			}
			catch
			{
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static int SetMySQLInfo(string str_tname)
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			bool flag = false;
			if (str_tname.Equals("ALL"))
			{
				flag = true;
			}
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string text2 = text + "history.mdb";
				if (!File.Exists(text2))
				{
					DBUtil.createdb();
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					try
					{
						oleDbCommand = oleDbConnection.CreateCommand();
						if (flag)
						{
							oleDbCommand.CommandText = "delete from mysqldb";
							oleDbCommand.ExecuteNonQuery();
							oleDbCommand.CommandText = "insert into mysqldb ( dbname ) values ('ALL') ";
							oleDbCommand.ExecuteNonQuery();
						}
						else
						{
							oleDbCommand.CommandText = "insert into mysqldb ( dbname ) values ('" + str_tname + "') ";
							oleDbCommand.ExecuteNonQuery();
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
						{
							"SQL error : ",
							oleDbCommand.CommandText,
							"\r\n",
							ex.Message,
							"\r\n",
							ex.StackTrace
						}));
					}
					if (oleDbCommand != null)
					{
						try
						{
							oleDbCommand.Dispose();
						}
						catch
						{
						}
					}
					if (oleDbConnection != null)
					{
						try
						{
							oleDbConnection.Close();
						}
						catch
						{
						}
					}
					return 1;
				}
			}
			catch
			{
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static void CompactAccessDB(string connectionString, string mdwfilename)
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));
				JetEngine arg_2B_0 = (JetEngine)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("DE88C160-FF2C-11D1-BB6F-00C04FAE22DA")));
				string text = mdwfilename.Substring(0, mdwfilename.LastIndexOf("\\"));
				text += "\\tempdb.mdb";
				string text2 = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5", text);
				object[] args = new object[]
				{
					connectionString,
					text2
				};
				obj.GetType().InvokeMember("CompactDatabase", BindingFlags.InvokeMethod, null, obj, args);
				File.Delete(mdwfilename);
				File.Move(text, mdwfilename);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				throw;
			}
			finally
			{
				if (obj != null)
				{
					Marshal.ReleaseComObject(obj);
					obj = null;
				}
			}
		}
		public static int GenerateRackPD(Hashtable ht_device)
		{
			int result = -1;
			if (ht_device == null)
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				text + "history.mdb";
			}
			return result;
		}
		public static int CheckAllDatabase()
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string path = text + "sysdb.mdb";
				int result;
				if (!File.Exists(path))
				{
					result = DebugCenter.ST_SysdbNotExist;
					return result;
				}
				path = text + "logdb.mdb";
				if (!File.Exists(path))
				{
					result = DebugCenter.ST_LogdbNotExist;
					return result;
				}
				text += "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				path = text + "datadb.org";
				if (!File.Exists(path))
				{
					result = DebugCenter.ST_DatadbNotExist;
					return result;
				}
				int num = DBUtil.CheckSysDBSerial();
				if (num == -2)
				{
					result = DebugCenter.ST_SysdbNotMatch;
					return result;
				}
				if (num < 0)
				{
					result = DebugCenter.ST_Unknown;
					return result;
				}
				if (num == 1)
				{
					DBUtil.SetSysDBSerial();
				}
				try
				{
					DBUrl.initconfig();
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("Check SysDB Error : " + ex.Message + "\r\n" + ex.StackTrace);
					result = DebugCenter.ST_Unknown;
					return result;
				}
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					try
					{
						int num2 = 1;
						string text2;
						while (true)
						{
							text2 = "";
							try
							{
								dbConnection = new MySqlConnection(string.Concat(new string[]
								{
									"Database=",
									DBUrl.DB_CURRENT_NAME,
									";Data Source=",
									DBUrl.CURRENT_HOST_PATH,
									";Port=",
									Convert.ToString(DBUrl.CURRENT_PORT),
									";User Id=",
									DBUrl.CURRENT_USER_NAME,
									";Password=",
									DBUrl.CURRENT_PWD,
									";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
								}));
								dbConnection.Open();
							}
							catch (Exception ex2)
							{
								DateTime lastBootUpTime = Query.getLastBootUpTime();
								text2 = ex2.Message;
								if (num2 > 0)
								{
									num2--;
									Thread.Sleep(2000);
									continue;
								}
								if ((DateTime.Now - lastBootUpTime).TotalSeconds <= 90.0)
								{
									Thread.Sleep(5000);
									continue;
								}
							}
							break;
						}
						if (text2.Length > 0)
						{
							if (text2.IndexOf("Unable to connect to any of the specified MySQL hosts") > -1)
							{
								result = DebugCenter.ST_MYSQLCONNECT_ERROR;
								return result;
							}
							if (text2.IndexOf("Authentication to host") > -1 && text2.IndexOf("Access denied for user") > -1)
							{
								result = DebugCenter.ST_MYSQLAUTH_ERROR;
								return result;
							}
							if (text2.IndexOf("Authentication to host") > -1 && text2.IndexOf("Unknown database") > -1)
							{
								result = DebugCenter.ST_MYSQLNotExist;
								return result;
							}
							result = DebugCenter.ST_MYSQLCONNECT_ERROR;
							return result;
						}
						else
						{
							if (dbConnection != null && dbConnection.State == ConnectionState.Open)
							{
								if (!DBUrl.IsServer)
								{
									result = DebugCenter.ST_Success;
									return result;
								}
								string serverID4MySQL = DBUtil.GetServerID4MySQL(dbConnection);
								if (serverID4MySQL.Equals("DBERROR") || serverID4MySQL.Equals("DBNONE"))
								{
									if (DBUrl.DB_CURRENT_NAME.Equals("eco"))
									{
										string serverID = DBUtil.GetServerID();
										if (serverID.Equals("DBERROR") || serverID.Equals("DBNONE"))
										{
											result = DebugCenter.ST_Success;
											return result;
										}
									}
									result = DebugCenter.ST_Unknown;
									return result;
								}
								string serverID2 = DBUtil.GetServerID();
								if (serverID2.Equals("DBERROR") || serverID2.Equals("DBNONE"))
								{
									result = DebugCenter.ST_Unknown;
									return result;
								}
								if (!serverID2.Equals(serverID4MySQL))
								{
									result = DebugCenter.ST_MYSQLSIDNotMatch;
									return result;
								}
							}
						}
					}
					catch (Exception ex3)
					{
						string message = ex3.Message;
						if (message.IndexOf("Unable to connect to any of the specified MySQL hosts") > -1)
						{
							result = DebugCenter.ST_MYSQLCONNECT_ERROR;
							return result;
						}
						if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Access denied for user") > -1)
						{
							result = DebugCenter.ST_MYSQLAUTH_ERROR;
							return result;
						}
						if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Unknown database") > -1)
						{
							result = DebugCenter.ST_MYSQLNotExist;
							return result;
						}
						result = DebugCenter.ST_MYSQLCONNECT_ERROR;
						return result;
					}
					finally
					{
						if (dbCommand != null)
						{
							try
							{
								dbCommand.Dispose();
							}
							catch
							{
							}
						}
						if (dbConnection != null)
						{
							try
							{
								dbConnection.Close();
							}
							catch
							{
							}
						}
					}
				}
				result = DebugCenter.ST_Success;
				return result;
			}
			catch (Exception ex4)
			{
				DebugCenter.GetInstance().appendToFile("CheckDatabase error : " + ex4.Message + "\r\n" + ex4.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return DebugCenter.ST_Unknown;
		}
		public static int CheckSysDBSerial()
		{
			if (!DBUrl.IsServer)
			{
				return 2;
			}
			int result = -1;
			OleDbConnection oleDbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result2 = -2;
					return result2;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					int result2;
					if (!DBUtil.DetermineTableExist(oleDbConnection, "dbextendinfo"))
					{
						result2 = 1;
						return result2;
					}
					dbCommand = oleDbConnection.CreateCommand();
					dbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = dbCommand.ExecuteScalar();
					if (obj == null || obj == DBNull.Value)
					{
						result2 = -1;
						return result2;
					}
					int num = Convert.ToInt32(obj);
					if (num == 0)
					{
						result2 = 1;
						return result2;
					}
					if (num < 1)
					{
						result2 = -1;
						return result2;
					}
					dbCommand.CommandText = "select sysdbserial from dbextendinfo ";
					object obj2 = dbCommand.ExecuteScalar();
					if (obj2 == null || obj2 == DBNull.Value)
					{
						result2 = 1;
						return result2;
					}
					string text3 = Convert.ToString(obj2);
					if (text3 == null || text3.ToLower().IndexOf("null") >= 0)
					{
						result2 = -1;
						return result2;
					}
					if (text3.Trim().Equals(Query.getSystemID(text).Trim()))
					{
						result2 = 2;
						return result2;
					}
					result2 = -2;
					return result2;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("CheckSysDBSerial error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int SetSysDBSerial(DbConnection con)
		{
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				text + "sysdb.mdb";
				if (con != null && con.State == ConnectionState.Open)
				{
					if (!DBUtil.DetermineTableExist(con, "dbextendinfo"))
					{
						dbCommand = con.CreateCommand();
						dbCommand.CommandText = "create table dbextendinfo (sysdbserial varchar(255),serverid varchar(255),createtime datetime )";
						dbCommand.ExecuteNonQuery();
						dbCommand.CommandText = "insert into dbextendinfo (sysdbserial ) values ('" + Query.getSystemID(text) + "' )";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = dbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = Convert.ToInt32(obj);
						int result;
						if (num == 0)
						{
							dbCommand.CommandText = "insert into dbextendinfo (sysdbserial ) values ('" + Query.getSystemID(text) + "' )";
							dbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						if (num >= 1)
						{
							dbCommand.CommandText = "update dbextendinfo set sysdbserial = '" + Query.getSystemID(text) + "'";
							dbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						result = -3;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetSysDBSerial error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static int SetSysDBSerial()
		{
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = -2;
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					if (!DBUtil.DetermineTableExist(oleDbConnection, "dbextendinfo"))
					{
						oleDbCommand = oleDbConnection.CreateCommand();
						oleDbCommand.CommandText = "create table dbextendinfo (sysdbserial varchar(255),serverid varchar(255),createtime datetime )";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "insert into dbextendinfo (sysdbserial ) values ('" + Query.getSystemID(text) + "' )";
						oleDbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = oleDbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = Convert.ToInt32(obj);
						int result;
						if (num == 0)
						{
							oleDbCommand.CommandText = "insert into dbextendinfo (sysdbserial ) values ('" + Query.getSystemID(text) + "' )";
							oleDbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						if (num >= 1)
						{
							oleDbCommand.CommandText = "update dbextendinfo set sysdbserial = '" + Query.getSystemID(text) + "'";
							oleDbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						result = -3;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetSysDBSerial error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static int SetServerID4MySQL(DbConnection con, string str_sid)
		{
			DbCommand dbCommand = null;
			try
			{
				if (con != null && con.State == ConnectionState.Open)
				{
					if (!DBUtil.DetermineTableExist(con, "dbextendinfo"))
					{
						dbCommand = con.CreateCommand();
						dbCommand.CommandText = "create table dbextendinfo (sysdbserial varchar(255),serverid varchar(255),createtime datetime )";
						dbCommand.ExecuteNonQuery();
						dbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = dbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = Convert.ToInt32(obj);
						int result;
						if (num == 0)
						{
							dbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
							dbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						if (num >= 1)
						{
							dbCommand.CommandText = "update dbextendinfo set serverid = '" + str_sid + "'";
							dbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						result = -3;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetServerID error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static int SetServerID(DbConnection con, string str_sid)
		{
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string path = text + "sysdb.mdb";
				if (!File.Exists(path))
				{
					int result = -2;
					return result;
				}
				if (con != null && con.State == ConnectionState.Open)
				{
					if (!DBUtil.DetermineTableExist(con, "dbextendinfo"))
					{
						dbCommand = con.CreateCommand();
						dbCommand.CommandText = "CREATE TABLE `dbextendinfo` (`serverid` VARCHAR(255) NULL,`serverip` VARCHAR(255) NULL,`servername` VARCHAR(255) NULL,`servermac` VARCHAR(255) NULL,`createtime` DATETIME NULL,INDEX `ind1` (`serverid` ASC),INDEX `ind2` (`createtime` ASC),INDEX `ind3` (`servermac` ASC),INDEX `ind4` (`servername` ASC)) ENGINE = MyISAM;";
						dbCommand.ExecuteNonQuery();
						dbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = dbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = Convert.ToInt32(obj);
						int result;
						if (num == 0)
						{
							dbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
							dbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						if (num >= 1)
						{
							dbCommand.CommandText = "update dbextendinfo set serverid = '" + str_sid + "'";
							dbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						result = -3;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetServerID error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static int SetServerID(string srcdbpath, string str_sid)
		{
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			try
			{
				string text = srcdbpath;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = -2;
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = "update dbsource set db_name='eco_" + str_sid + "' where active_flag = 2 ";
					oleDbCommand.ExecuteNonQuery();
					if (!DBUtil.DetermineTableExist(oleDbConnection, "dbextendinfo"))
					{
						oleDbCommand.CommandText = "create table dbextendinfo (sysdbserial varchar(255),serverid varchar(255),createtime datetime )";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
						oleDbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					oleDbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = oleDbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = Convert.ToInt32(obj);
						int result;
						if (num == 0)
						{
							oleDbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
							oleDbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						if (num >= 1)
						{
							oleDbCommand.CommandText = "update dbextendinfo set serverid = '" + str_sid + "'";
							oleDbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						result = -3;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetServerID error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static int SetServerID(string str_sid)
		{
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = -2;
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					if (!DBUtil.DetermineTableExist(oleDbConnection, "dbextendinfo"))
					{
						oleDbCommand = oleDbConnection.CreateCommand();
						oleDbCommand.CommandText = "create table dbextendinfo (sysdbserial varchar(255),serverid varchar(255),createtime datetime )";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
						oleDbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = oleDbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						int num = Convert.ToInt32(obj);
						int result;
						if (num == 0)
						{
							oleDbCommand.CommandText = "insert into dbextendinfo (serverid ) values ('" + str_sid + "' )";
							oleDbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						if (num >= 1)
						{
							oleDbCommand.CommandText = "update dbextendinfo set serverid = '" + str_sid + "'";
							oleDbCommand.ExecuteNonQuery();
							result = 1;
							return result;
						}
						result = -3;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetServerID error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static string GetServerID(DbConnection con)
		{
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string path = text + "sysdb.mdb";
				if (!File.Exists(path))
				{
					string result = "DBERROR";
					return result;
				}
				if (con != null && con.State == ConnectionState.Open)
				{
					string result;
					if (!DBUtil.DetermineTableExist(con, "dbextendinfo"))
					{
						result = "DBNONE";
						return result;
					}
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = dbCommand.ExecuteScalar();
					if (obj == null || obj == DBNull.Value)
					{
						result = "DBNONE";
						return result;
					}
					int num = Convert.ToInt32(obj);
					if (num == 0)
					{
						result = "DBNONE";
						return result;
					}
					if (num < 1)
					{
						result = "DBNONE";
						return result;
					}
					dbCommand.CommandText = "select serverid from dbextendinfo ";
					object obj2 = dbCommand.ExecuteScalar();
					if (obj2 == null || obj2 == DBNull.Value)
					{
						result = "DBNONE";
						return result;
					}
					string text2 = Convert.ToString(obj2);
					if (text2 != null && text2.ToLower().IndexOf("null") < 0)
					{
						result = text2;
						return result;
					}
					result = "DBNONE";
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetServerID error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return "DBERROR";
		}
		public static string GetServerID4MySQL(DbConnection con)
		{
			DbCommand dbCommand = null;
			try
			{
				if (con != null && con.State == ConnectionState.Open)
				{
					string result;
					if (!DBUtil.DetermineTableExist(con, "dbextendinfo"))
					{
						result = "DBNONE";
						return result;
					}
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = dbCommand.ExecuteScalar();
					if (obj == null || obj == DBNull.Value)
					{
						result = "DBNONE";
						return result;
					}
					int num = Convert.ToInt32(obj);
					if (num == 0)
					{
						result = "DBNONE";
						return result;
					}
					if (num < 1)
					{
						result = "DBNONE";
						return result;
					}
					dbCommand.CommandText = "select serverid from dbextendinfo ";
					object obj2 = dbCommand.ExecuteScalar();
					if (obj2 == null || obj2 == DBNull.Value)
					{
						result = "DBNONE";
						return result;
					}
					string text = Convert.ToString(obj2);
					if (text != null && text.ToLower().IndexOf("null") < 0)
					{
						result = text;
						return result;
					}
					result = "DBNONE";
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("GetServerID4MySQL error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return "DBERROR";
		}
		public static string GetServerID()
		{
			OleDbCommand oleDbCommand = null;
			OleDbConnection oleDbConnection = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					string result = "DBERROR";
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					string result;
					if (!DBUtil.DetermineTableExist(oleDbConnection, "dbextendinfo"))
					{
						result = "DBNONE";
						return result;
					}
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = "select count(*) from dbextendinfo ";
					object obj = oleDbCommand.ExecuteScalar();
					if (obj == null || obj == DBNull.Value)
					{
						result = "DBNONE";
						return result;
					}
					int num = Convert.ToInt32(obj);
					if (num == 0)
					{
						result = "DBNONE";
						return result;
					}
					if (num < 1)
					{
						result = "DBNONE";
						return result;
					}
					oleDbCommand.CommandText = "select serverid from dbextendinfo ";
					object obj2 = oleDbCommand.ExecuteScalar();
					if (obj2 == null || obj2 == DBNull.Value)
					{
						result = "DBNONE";
						return result;
					}
					string text3 = Convert.ToString(obj2);
					if (text3 != null && text3.ToLower().IndexOf("null") < 0)
					{
						result = text3;
						return result;
					}
					result = "DBNONE";
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("SetServerID error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return "DBERROR";
		}
		public static Hashtable GetTables(DbConnection con)
		{
			Hashtable hashtable = new Hashtable();
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			DataTable dataTable = new DataTable();
			try
			{
				if (con != null && con.State == ConnectionState.Open)
				{
					if (con is MySqlConnection)
					{
						dbCommand = con.CreateCommand();
						dbDataAdapter = new MySqlDataAdapter();
						dbCommand.CommandText = "show tables ";
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = Convert.ToString(dataRow[0]);
								if (text != null && text.Length > 0 && !hashtable.ContainsKey(text))
								{
									hashtable.Add(text, text);
								}
							}
						}
						dbCommand.Dispose();
					}
					else
					{
						string[] restrictionValues = new string[]
						{
							null,
							null,
							null,
							"TABLE"
						};
						DataTable schema = con.GetSchema("Tables", restrictionValues);
						foreach (DataRow dataRow2 in schema.Rows)
						{
							string text2 = Convert.ToString(dataRow2.ItemArray[2]);
							if (text2 != null && text2.Length > 0 && !hashtable.ContainsKey(text2))
							{
								hashtable.Add(text2, text2);
							}
						}
					}
				}
			}
			catch
			{
				if (dbDataAdapter != null)
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			return hashtable;
		}
		public static bool CheckTable(OleDbConnection conn, string tableName)
		{
			DataTable schema = conn.GetSchema("Tables");
			DataRow[] array = schema.Select(string.Format("TABLE_TYPE='TABLE' and TABLE_NAME='{0}'", tableName));
			return array.Length > 0;
		}
		public static List<string> GetTableName(OleDbConnection conn)
		{
			List<string> result;
			try
			{
				conn.Open();
				DataTable oleDbSchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
				{
					null,
					null,
					null,
					"TABLE"
				});
				conn.Close();
				int count = oleDbSchemaTable.Rows.Count;
				int index = oleDbSchemaTable.Columns.IndexOf("TABLE_NAME");
				List<string> list = new List<string>();
				for (int i = 0; i < count; i++)
				{
					DataRow dataRow = oleDbSchemaTable.Rows[i];
					list.Add(dataRow.ItemArray.GetValue(index).ToString());
				}
				result = list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
			}
			return result;
		}
		public static List<string> GetTableColumn(string tableName, OleDbConnection conn)
		{
			List<string> list = new List<string>();
			DataTable dataTable = new DataTable();
			List<string> result;
			try
			{
				conn.Open();
				Guid arg_27_1 = OleDbSchemaGuid.Columns;
				object[] array = new object[4];
				array[2] = tableName;
				dataTable = conn.GetOleDbSchemaTable(arg_27_1, array);
				conn.Close();
				int count = dataTable.Rows.Count;
				int index = dataTable.Columns.IndexOf("ORDINAL_POSITION");
				int index2 = dataTable.Columns.IndexOf("COLUMN_NAME");
				for (int i = 0; i < count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					list.Add(dataRow.ItemArray.GetValue(index2).ToString());
					if (dataRow.ItemArray.GetValue(index).ToString() == "1")
					{
						list.Remove(dataRow.ItemArray.GetValue(index2).ToString());
					}
				}
				result = list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
			}
			return result;
		}
		public static string[] PrimaryKeyName(string tablename)
		{
			string text = "";
			if (tablename.ToLower() == "DList".ToLower())
			{
				text = "WorkerId";
			}
			if (text != "")
			{
				return text.Split(new char[]
				{
					','
				});
			}
			return null;
		}
		public static string GetTableColumnIndex(string tableName, OleDbConnection conn)
		{
			DataTable dataTable = new DataTable();
			string text = "";
			string result;
			try
			{
				conn.Open();
				Guid arg_27_1 = OleDbSchemaGuid.Columns;
				object[] array = new object[4];
				array[2] = tableName;
				dataTable = conn.GetOleDbSchemaTable(arg_27_1, array);
				conn.Close();
				int count = dataTable.Rows.Count;
				int index = dataTable.Columns.IndexOf("ORDINAL_POSITION");
				int index2 = dataTable.Columns.IndexOf("COLUMN_NAME");
				for (int i = 0; i < count; i++)
				{
					DataRow dataRow = dataTable.Rows[i];
					if (dataRow.ItemArray.GetValue(index).ToString() == "1")
					{
						text = dataRow.ItemArray.GetValue(index2).ToString();
					}
				}
				result = text;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
			}
			return result;
		}
		public static bool DetermineTableExist(DbConnection con, string str_tname)
		{
			bool result;
			try
			{
				if (con == null || con.State != ConnectionState.Open)
				{
					throw new Exception("DbConnection is null");
				}
				Hashtable tables = DBUtil.GetTables(con);
				if (tables == null || tables.Count <= 0)
				{
					throw new Exception("Can not get database table list");
				}
				if (tables.ContainsKey(str_tname))
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public static int ChangeDBSetting2Access()
		{
			int num = -1;
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = num;
					return result;
				}
				dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					dbCommand = dbConnection.CreateCommand();
					dbCommand.CommandText = "update dbsource set db_type='ACCESS',db_name='datadb',host_path='datadb.mdb',port= 0,user_name = 'root',pwd='root' where active_flag = 2 ";
					int num2 = dbCommand.ExecuteNonQuery();
					int result;
					if (num2 < 0)
					{
						result = -5;
						return result;
					}
					DBUrl.initconfig();
					result = 1;
					return result;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return num;
		}
		public static int ChangeDBSetting2MySQL(string str_host, int i_port, string str_usr, string str_pwd, bool b_create = false)
		{
			int num = -1;
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = num;
					return result;
				}
				dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					dbCommand = dbConnection.CreateCommand();
					dbCommand.CommandText = string.Concat(new object[]
					{
						"update dbsource set db_type='MYSQL',db_name='",
						DBUrl.DB_CURRENT_NAME,
						"',host_path='",
						str_host,
						"',port= ",
						i_port,
						",user_name = '",
						str_usr,
						"',pwd='",
						str_pwd,
						"' where active_flag = 2 "
					});
					int num2 = dbCommand.ExecuteNonQuery();
					int result;
					if (num2 < 0)
					{
						result = -5;
						return result;
					}
					DBUrl.initconfig();
					result = DebugCenter.ST_Success;
					return result;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return num;
		}
		public static int ChangeDBSetting2MySQL(string dbname, string str_host, int i_port, string str_usr, string str_pwd)
		{
			int num = -1;
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					int result = num;
					return result;
				}
				dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=^tenec0Sensor");
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					dbCommand = dbConnection.CreateCommand();
					string text3 = str_host;
					try
					{
						if (DBMaintain.IsLocalIP(str_host))
						{
							text3 = "127.0.0.1";
						}
					}
					catch
					{
					}
					dbCommand.CommandText = string.Concat(new object[]
					{
						"update dbsource set db_type='MYSQL',db_name='",
						dbname,
						"',host_path='",
						text3,
						"',port= ",
						i_port,
						",user_name = '",
						str_usr,
						"',pwd='",
						str_pwd,
						"' where active_flag = 2 "
					});
					int num2 = dbCommand.ExecuteNonQuery();
					int result;
					if (num2 < 0)
					{
						result = -5;
						return result;
					}
					DBUrl.initconfig();
					result = 1;
					return result;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return num;
		}
		public static int CheckMySQLParameter(string str_host, int i_port, string str_usr, string str_pwd, bool b_create = false)
		{
			int sT_Unknown = DebugCenter.ST_Unknown;
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				DBUrl.initconfig();
				dbConnection = new MySqlConnection(string.Concat(new string[]
				{
					"Database=",
					DBUrl.DB_CURRENT_NAME,
					";Data Source=",
					str_host,
					";Port=",
					Convert.ToString(i_port),
					";User Id=",
					str_usr,
					";Password=",
					str_pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					int result;
					if (DBUrl.IsServer)
					{
						string serverID4MySQL = DBUtil.GetServerID4MySQL(dbConnection);
						if (serverID4MySQL.Equals("DBERROR") || serverID4MySQL.Equals("DBNONE"))
						{
							result = DebugCenter.ST_Unknown;
							return result;
						}
						string serverID = DBUtil.GetServerID();
						if (serverID.Equals("DBERROR") || serverID.Equals("DBNONE"))
						{
							result = DebugCenter.ST_Unknown;
							return result;
						}
						if (!serverID.Equals(serverID4MySQL))
						{
							result = DebugCenter.ST_MYSQLSIDNotMatch;
							return result;
						}
					}
					result = DebugCenter.ST_Success;
					return result;
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				DebugCenter.GetInstance().appendToFile(ex.Message ?? "");
				if (message.IndexOf("Unable to connect to any of the specified MySQL hosts") > -1)
				{
					int result = DebugCenter.ST_MYSQLCONNECT_ERROR;
					return result;
				}
				if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Access denied for user") > -1)
				{
					int result = DebugCenter.ST_MYSQLAUTH_ERROR;
					return result;
				}
				if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Unknown database") > -1)
				{
					int result = DebugCenter.ST_MYSQLNotExist;
					return result;
				}
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return sT_Unknown;
		}
		public static int CheckMySQLParameter_NOName(string str_host, int i_port, string str_usr, string str_pwd)
		{
			int sT_Unknown = DebugCenter.ST_Unknown;
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				DBUrl.initconfig();
				dbConnection = new MySqlConnection(string.Concat(new string[]
				{
					"Database=;Data Source=",
					str_host,
					";Port=",
					Convert.ToString(i_port),
					";User Id=",
					str_usr,
					";Password=",
					str_pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				if (dbConnection != null && dbConnection.State == ConnectionState.Open)
				{
					int result = DebugCenter.ST_Success;
					return result;
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				if (message.IndexOf("Unable to connect to any of the specified MySQL hosts") > -1)
				{
					int result = DebugCenter.ST_MYSQLCONNECT_ERROR;
					return result;
				}
				if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Access denied for user") > -1)
				{
					int result = DebugCenter.ST_MYSQLAUTH_ERROR;
					return result;
				}
				if (message.IndexOf("Authentication to host") > -1 && message.IndexOf("Unknown database") > -1)
				{
					int result = DebugCenter.ST_MYSQLNotExist;
					return result;
				}
			}
			finally
			{
				if (dbCommand != null)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (dbConnection != null)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return sT_Unknown;
		}
		public static string CheckMySQLVersion(string sHost, int iPort, string sUsr, string sPwd, string fileversion)
		{
			string text = DBTools.GetMySQLVersion(sHost, string.Concat(iPort), sUsr, sPwd);
			text = DBMaintain.EditVersionString(text);
			string text2 = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			if (fileversion.Length > 0)
			{
				string text3 = DBMaintain.EditVersionString(fileversion);
				if (!DBMaintain.CompareMySQLVersion(text, text3))
				{
					return text3;
				}
				return "";
			}
			else
			{
				if (text == null || text.Length < 1 || text.CompareTo("5.6") < 0)
				{
					return "5.6";
				}
				return "";
			}
		}
		public static int AddHeader4File(string strfile)
		{
			string text = "";
			string text2 = AppDomain.CurrentDomain.BaseDirectory;
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			string path = text2 + "testheader.bak";
			byte[] array = new byte[1024];
			for (int i = 0; i < 1024; i++)
			{
				array[i] = Convert.ToByte(DBUtil.rnd());
			}
			if (File.Exists(path))
			{
				FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
				array = new byte[1024];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
				int num = 0;
				int num2 = 0;
				byte[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					byte b = array2[j];
					int num3 = num2 / 32;
					num2++;
					text = text + ",0x" + b.ToString("X2");
					if (num3 > num)
					{
						num = num3;
						text += "\r\n";
					}
				}
				DebugCenter.GetInstance().appendToFile(text);
			}
			else
			{
				FileStream fileStream2 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream2.Write(array, 0, array.Length);
				fileStream2.Close();
			}
			return -1;
		}
		public static int GetRandomSeed()
		{
			byte[] array = new byte[4];
			RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			rNGCryptoServiceProvider.GetBytes(array);
			return BitConverter.ToInt32(array, 0);
		}
		public static int rnd()
		{
			Random random = new Random(DBUtil.GetRandomSeed());
			return random.Next(0, 255);
		}
		public static byte[] GenerateHead(Hashtable ht_content, string str_ver_type)
		{
			string text = "";
			List<string> list = new List<string>();
			bool flag = false;
			if (str_ver_type.Equals("MASTER"))
			{
				text = "ecoSensorsBackup4Master";
				flag = true;
			}
			if (str_ver_type.Equals("SINGLE"))
			{
				text = "ecoSensorsBackup4Single";
			}
			ICollection keys = ht_content.Keys;
			foreach (string text2 in keys)
			{
				string text3 = (string)ht_content[text2];
				if (text.Length + text2.Length + text3.Length + 2 > 1010)
				{
					text += ";continue=1024";
					list.Add(text);
					if (flag)
					{
						text = "ecoSensorsBackup4Master";
					}
					else
					{
						text = "ecoSensorsBackup4Single";
					}
				}
				text = string.Concat(new string[]
				{
					text,
					";",
					text2,
					"=",
					text3
				});
			}
			if (text.Length > 23)
			{
				list.Add(text);
			}
			byte[] array = new byte[list.Count * 1024];
			for (int i = 0; i < list.Count; i++)
			{
				string s = list[i];
				byte[] bytes = Encoding.ASCII.GetBytes(s);
				for (int j = 0; j < 1024; j++)
				{
					int num = i * 1024 + j;
					if (j < bytes.Length)
					{
						array[num] = (bytes[j] ^ DBUtil.b_mark_newversion[j]);
					}
					else
					{
						if (j == bytes.Length || j == bytes.Length + 1)
						{
							array[num] = (35 ^ DBUtil.b_mark_newversion[j]);
						}
						else
						{
							array[num] = DBUtil.b_mark_newversion[j];
						}
					}
				}
			}
			if (array != null)
			{
				return array;
			}
			return null;
		}
		public static Hashtable GetDBFileInfo_newversion(string str_filepath)
		{
			Hashtable hashtable = new Hashtable();
			try
			{
				Hashtable result;
				if (!File.Exists(str_filepath))
				{
					result = hashtable;
					return result;
				}
				FileInfo fileInfo = new FileInfo(str_filepath);
				if (fileInfo.Length < 1024L)
				{
					result = hashtable;
					return result;
				}
				FileStream fileStream = new FileStream(str_filepath, FileMode.Open, FileAccess.Read);
				byte[] array = new byte[1024];
				int num = 0;
				while (true)
				{
					bool flag = false;
					int num2 = fileStream.Read(array, 0, 1024);
					num++;
					byte[] array2 = new byte[1024];
					for (int i = 0; i < 1024; i++)
					{
						array2[i] = (array[i] ^ DBUtil.b_mark_newversion[i]);
					}
					string @string = Encoding.ASCII.GetString(array2);
					if (@string.StartsWith("ecoSensorsBackup4Master"))
					{
						hashtable.Add("ECOTYPE", "MASTER");
					}
					else
					{
						if (!@string.StartsWith("ecoSensorsBackup4Single"))
						{
							break;
						}
						hashtable.Add("ECOTYPE", "SINGLE");
					}
					if (hashtable.ContainsKey("INFOLENGTH"))
					{
						hashtable["INFOLENGTH"] = string.Concat(num);
					}
					else
					{
						hashtable.Add("INFOLENGTH", string.Concat(num));
					}
					string text;
					if (@string.IndexOf("##") > 0)
					{
						text = @string.Substring(23);
						int length = text.LastIndexOf("##");
						text = text.Substring(0, length);
					}
					else
					{
						if (@string.EndsWith("#"))
						{
							text = @string.Substring(23);
							int length2 = text.LastIndexOf("#");
							text = text.Substring(0, length2);
						}
						else
						{
							text = @string.Substring(23);
						}
					}
					string[] separator = new string[]
					{
						";"
					};
					string[] array3 = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					string[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						string text2 = array4[j];
						int num3 = text2.IndexOf("=");
						string text3 = text2.Substring(0, num3);
						string value = text2.Substring(num3 + 1);
						if (text3.Equals("continue"))
						{
							flag = true;
						}
						if (hashtable.ContainsKey(text3))
						{
							hashtable[text3] = value;
						}
						else
						{
							hashtable.Add(text3, value);
						}
					}
					if (num2 == 0 || !flag)
					{
						goto IL_24A;
					}
				}
				result = hashtable;
				return result;
				IL_24A:
				fileStream.Close();
			}
			catch (Exception)
			{
			}
			return hashtable;
		}
		public static Hashtable GetDBFileInfo(string str_filepath)
		{
			Hashtable hashtable = new Hashtable();
			try
			{
				Hashtable result;
				if (!File.Exists(str_filepath))
				{
					result = hashtable;
					return result;
				}
				FileInfo fileInfo = new FileInfo(str_filepath);
				if (fileInfo.Length < 1024L)
				{
					result = hashtable;
					return result;
				}
				FileStream fileStream = new FileStream(str_filepath, FileMode.Open, FileAccess.Read);
				byte[] array = new byte[1024];
				int num = 0;
				while (true)
				{
					bool flag = false;
					int num2 = fileStream.Read(array, 0, 1024);
					num++;
					byte[] array2 = new byte[1024];
					for (int i = 0; i < 1024; i++)
					{
						array2[i] = (array[i] ^ DBUtil.b_mark[i]);
					}
					string @string = Encoding.ASCII.GetString(array2);
					if (@string.StartsWith("ecoSensorsBackup4Master"))
					{
						hashtable.Add("ECOTYPE", "MASTER");
					}
					else
					{
						if (!@string.StartsWith("ecoSensorsBackup4Single"))
						{
							break;
						}
						hashtable.Add("ECOTYPE", "SINGLE");
					}
					if (hashtable.ContainsKey("INFOLENGTH"))
					{
						hashtable["INFOLENGTH"] = string.Concat(num);
					}
					else
					{
						hashtable.Add("INFOLENGTH", string.Concat(num));
					}
					string text;
					if (@string.IndexOf("##") > 0)
					{
						text = @string.Substring(23);
						int length = text.LastIndexOf("##");
						text = text.Substring(0, length);
					}
					else
					{
						if (@string.EndsWith("#"))
						{
							text = @string.Substring(23);
							int length2 = text.LastIndexOf("#");
							text = text.Substring(0, length2);
						}
						else
						{
							text = @string.Substring(23);
						}
					}
					string[] separator = new string[]
					{
						";"
					};
					string[] array3 = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					string[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						string text2 = array4[j];
						int num3 = text2.IndexOf("=");
						string text3 = text2.Substring(0, num3);
						string value = text2.Substring(num3 + 1);
						if (text3.Equals("continue"))
						{
							flag = true;
						}
						if (hashtable.ContainsKey(text3))
						{
							hashtable[text3] = value;
						}
						else
						{
							hashtable.Add(text3, value);
						}
					}
					if (num2 == 0 || !flag)
					{
						goto IL_24A;
					}
				}
				result = hashtable;
				return result;
				IL_24A:
				fileStream.Close();
			}
			catch (Exception)
			{
			}
			return hashtable;
		}
	}
}
