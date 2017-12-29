using CommonAPI;
using CommonAPI.InterProcess;
using CommonAPI.Timers;
using CommonAPI.Tools;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace DBAccessAPI
{
	public class DBTools
	{
		public delegate int DelegateWriteLog(string logKey, params string[] logPrar);
		public const int PROGRAMBARTYPE_DECRYPT = 0;
		public const int PROGRAMBARTYPE_ENCRYPT = 1;
		public const int PROGRAMBARTYPE_ZIP = 2;
		public const int PROGRAMBARTYPE_UNZIP = 3;
		public const int PROGRAMBARTYPE_COPYFILE = 4;
		public const int PROGRAMBARTYPE_MOVEFILE = 5;
		public const string DBEXCHANGEFOLDER = "tmpdbexchangefolder";
		public const double CONVERT_RATIO = 10000.0;
		public const string COMPATIBLE_DBVERSION = "1.0.0.0";
		public const string LASTDB_VERSION = "1.4.0.7";
		public const string HEAD_VERSION = "1.0.0";
		public const int INTERVAL_BASE_MYSQL = 45;
		public const int INTERVAL_BASE_ACCESS = 100;
		private static List<int> _listMySQLFatalErrorCodes;
		public static DBTools.DelegateWriteLog DWL;
		private static Hashtable hm_tablesql;
		public static int ProgramBar_Percent;
		public static int ProgramBar_Type;
		public static long timeTick;
		public static Hashtable ht_tablename;
		public static string CURRENT_VERSION;
		public static DateTime MUDate;
		public static int DATASTORE_INTERVAL;
		public static DateTime DT_FIRST;
		public static DateTime DT_TODAYTASK;
		public static bool b_up;
		static DBTools()
		{
			DBTools.DWL = null;
			DBTools.hm_tablesql = new Hashtable();
			DBTools.ProgramBar_Percent = 1;
			DBTools.ProgramBar_Type = 0;
			DBTools.timeTick = 0L;
			DBTools.ht_tablename = new Hashtable();
			DBTools.CURRENT_VERSION = "1.4.0.7";
			DBTools.DATASTORE_INTERVAL = 120;
			DBTools.DT_FIRST = new DateTime(1970, 1, 1, 0, 0, 0);
			DBTools.DT_TODAYTASK = new DateTime(1970, 1, 1, 0, 0, 0);
			DBTools.b_up = false;
			DBTools._listMySQLFatalErrorCodes = new List<int>();
			DBTools._listMySQLFatalErrorCodes.Add(1021);
			DBTools._listMySQLFatalErrorCodes.Add(1044);
			DBTools._listMySQLFatalErrorCodes.Add(1045);
			DBTools._listMySQLFatalErrorCodes.Add(1047);
			DBTools._listMySQLFatalErrorCodes.Add(1049);
			DBTools._listMySQLFatalErrorCodes.Add(1051);
			DBTools._listMySQLFatalErrorCodes.Add(1053);
			DBTools._listMySQLFatalErrorCodes.Add(1077);
			DBTools._listMySQLFatalErrorCodes.Add(1105);
			DBTools._listMySQLFatalErrorCodes.Add(1129);
			DBTools._listMySQLFatalErrorCodes.Add(1130);
			DBTools._listMySQLFatalErrorCodes.Add(1194);
			DBTools._listMySQLFatalErrorCodes.Add(1195);
			DBTools.hm_tablesql.Add("devicedefine", "CREATE TABLE `devicedefine` (`model_nm` varchar(64) NOT NULL,`dev_ver` varchar(64) NOT NULL,`first_data` text,`second_data` text,`compatible` int(11) DEFAULT NULL,`reserve` varchar(255) DEFAULT NULL,`insert_time` datetime DEFAULT NULL,PRIMARY KEY (`model_nm`,`dev_ver`)) ENGINE=MyISAM");
			DBTools.hm_tablesql.Add("cleanupsetting", "CREATE TABLE `cleanupsetting` (`bytimeflag` int(11) NOT NULL,`byrecordflag` int(11) NOT NULL,`keepcount` int(11) DEFAULT NULL) ENGINE=MyISAM ");
			DBTools.hm_tablesql.Add("smtpsetting", "CREATE TABLE `smtpsetting` (`EnableSMTP` int(11) DEFAULT '0',`ServerAddress` varchar(128) DEFAULT NULL,`PortId` bigint(20) DEFAULT NULL,`EmailFrom` varchar(255) DEFAULT NULL,`EmailTo` varchar(1024) DEFAULT NULL,`SendEvent` varchar(1024) DEFAULT NULL,`EnableAuth` int(11) DEFAULT '0',`Account` varchar(255) DEFAULT NULL,`UserPwd` varchar(255) DEFAULT NULL,KEY `ipid` (`PortId`)) ENGINE=MyISAM ");
			DBTools.hm_tablesql.Add("snmpsetting", "CREATE TABLE `snmpsetting` (`snmpusername` varchar(64) DEFAULT NULL,`snmpport` int(11) DEFAULT NULL,`snmpver` int(11) DEFAULT NULL,`snmppprl` varchar(16) DEFAULT NULL,`snmpppwd` varchar(64) DEFAULT NULL,`snmpaprl` varchar(16) DEFAULT NULL,`snmpapwd` varchar(64) DEFAULT NULL,`snmptimeout` int(11) DEFAULT NULL,`snmpretry` int(11) DEFAULT NULL,`trapusername` varchar(64) DEFAULT NULL,`trapport` int(11) DEFAULT NULL,`trapver` int(11) DEFAULT NULL,`trappprl` varchar(16) DEFAULT NULL,`trapppwd` varchar(64) DEFAULT NULL,`trapaprl` varchar(16) DEFAULT NULL,`trapapwd` varchar(64) DEFAULT NULL) ENGINE=MyISAM ");
			DBTools.hm_tablesql.Add("systemparameter", "CREATE TABLE `systemparameter` (`ServiceDelay` int(11) NOT NULL,`Layout` int(11) DEFAULT NULL,`EnergyType` int(11) DEFAULT NULL,`EnergyValue` float DEFAULT NULL,`ReferenceDevice` int(11) DEFAULT NULL,`CO2KG` float DEFAULT NULL,`CO2COST` float DEFAULT NULL,`Electricitycost` float DEFAULT NULL,`TemperatureUnit` int(11) DEFAULT NULL,`Currency` varchar(64) DEFAULT NULL,`RackfullnameFlag` int(11) DEFAULT NULL ) ENGINE=MyISAM ");
			DBTools.hm_tablesql.Add("bank_data_daily", "CREATE TABLE `bank_data_daily` (`bank_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` date NOT NULL,KEY `bddind1` (`bank_id`),KEY `bddind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("bank_data_hourly", "CREATE TABLE `bank_data_hourly` (`bank_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `bdhind1` (`bank_id`),KEY `bdhind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("device_data_daily", "CREATE TABLE `device_data_daily` (`device_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` date NOT NULL,KEY `dddind1` (`device_id`),KEY `dddind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("device_data_hourly", "CREATE TABLE `device_data_hourly` (`device_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `ddhind1` (`device_id`),KEY `ddhind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("port_data_daily", "CREATE TABLE `port_data_daily` (`port_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` date NOT NULL,KEY `pddind1` (`port_id`),KEY `pddind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("port_data_hourly", "CREATE TABLE `port_data_hourly` (`port_id` int(11) NOT NULL,`power_consumption` bigint(20) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `pdhind1` (`port_id`),KEY `pdhind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("rack_effect", "CREATE TABLE `rack_effect` (`id` int(11) NOT NULL AUTO_INCREMENT,`RCI_High` float(16,4) DEFAULT NULL,`RCI_Low` float(16,4) DEFAULT NULL,`RHI_High` float(16,4) DEFAULT NULL,`RHI_Low` float(16,4) DEFAULT NULL,`RPI_High` float(16,4) DEFAULT NULL,`RPI_Low` float(16,4) DEFAULT NULL,`RAI_High` float(16,4) DEFAULT NULL,`RAI_Low` float(16,4) DEFAULT NULL,`RTI` float(16,4) DEFAULT NULL,`Fan_Saving` float(16,4) DEFAULT '0.0000',`Chiller_Saving` float(16,4) DEFAULT '0.0000',`Aggressive_Saving` float(16,4) DEFAULT '0.0000',`Insert_time` datetime DEFAULT NULL,PRIMARY KEY (`id`),KEY `reind1` (`Insert_time`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("rackthermal_daily", "CREATE TABLE `rackthermal_daily` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("rackthermal_hourly", "CREATE TABLE `rackthermal_hourly` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("rci_daily", "CREATE TABLE `rci_daily` (`rci_high` float(16,4) DEFAULT NULL,`rci_lo` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `index_rcid` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("rci_hourly", "CREATE TABLE `rci_hourly` (`rci_high` float(16,4) DEFAULT NULL,`rci_lo` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `index_rcih` (`insert_time`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("tmpid", "CREATE TABLE `tmpid` (`id` int(11) NOT NULL,PRIMARY KEY (`id`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("backuptask", "CREATE TABLE `backuptask` (`id` int(11) NOT NULL AUTO_INCREMENT,`taskname` varchar(128) DEFAULT NULL,`tasktype` int(11) DEFAULT NULL,`storetype` int(11) DEFAULT NULL,`username` varchar(255) DEFAULT NULL,`pwd` varchar(255) DEFAULT NULL,`host` varchar(255) DEFAULT NULL,`port` bigint(20) DEFAULT NULL,`filepath` varchar(1024) DEFAULT NULL,PRIMARY KEY (`id`),KEY `btind1` (`tasktype`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("bank_info", "CREATE TABLE `bank_info` (`id` int(11) NOT NULL AUTO_INCREMENT,`device_id` int(11) NOT NULL,`port_nums` varchar(255) DEFAULT NULL,`bank_nm` varchar(20) NOT NULL,`voltage` bigint(20) DEFAULT NULL,`max_voltage` float DEFAULT '-300',`min_voltage` float DEFAULT '-300',`max_power_diss` float DEFAULT '-300',`min_power_diss` float DEFAULT '0',`max_power` float DEFAULT '-300',`min_power` float DEFAULT '-300',`max_current` float DEFAULT '-300',`min_current` float DEFAULT '-300',PRIMARY KEY (`id`),KEY `did` (`device_id`),KEY `bnm` (`bank_nm`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("currentpue", "CREATE TABLE `currentpue` (`curhour` datetime DEFAULT NULL,`curday` datetime DEFAULT NULL,`curweek` datetime DEFAULT NULL,`ithourpue` double DEFAULT NULL,`nonithourpue` double DEFAULT NULL,`itdaypue` double DEFAULT NULL,`nonitdaypue` double DEFAULT NULL,`itweekpue` double DEFAULT NULL,`nonitweekpue` double DEFAULT NULL,`lasttime` datetime DEFAULT NULL,KEY `cpind1` (`curhour`),KEY `cpind2` (`curday`),KEY `cpind3` (`curweek`),KEY `cpind4` (`lasttime`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("data_group", "CREATE TABLE `data_group` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `groupname` varchar(255) DEFAULT NULL,  `grouptype` varchar(20) DEFAULT NULL,  `linecolor` varchar(50) DEFAULT NULL,  `isselect` int(11) DEFAULT NULL,  `thermalflag` int(11) DEFAULT NULL,  `billflag` int(11) DEFAULT NULL,  PRIMARY KEY (`id`),KEY `gnm` (`groupname`),KEY `dgind1` (`grouptype`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("device_addr_info", "CREATE TABLE `device_addr_info` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `rack_nm` varchar(32) DEFAULT NULL,  `sx` int(11) DEFAULT NULL,  `sy` int(11) DEFAULT NULL,  `ex` int(11) DEFAULT NULL,  `ey` int(11) DEFAULT NULL,  `reserve` varchar(100) DEFAULT NULL,  PRIMARY KEY (`id`),  KEY `rnm` (`rack_nm`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("device_base_info", "CREATE TABLE `device_base_info` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `device_ip` varchar(64) DEFAULT NULL,  `device_nm` varchar(255) DEFAULT NULL,  `mac` varchar(20) DEFAULT NULL,  `privacypw` varchar(20) DEFAULT NULL,  `authenpw` varchar(20) DEFAULT NULL,  `privacy` varchar(20) DEFAULT NULL,  `authen` varchar(20) DEFAULT NULL,  `timeout` bigint(20) DEFAULT '10',  `retry` bigint(20) DEFAULT '10',  `user_name` varchar(64) DEFAULT NULL,  `portid` int(11) DEFAULT '0',  `snmpVersion` int(11) DEFAULT '3',  `model_nm` varchar(255) DEFAULT NULL,  `max_voltage` float DEFAULT '-300',  `min_voltage` float DEFAULT '-300',  `max_power_diss` float DEFAULT '-300',  `min_power_diss` float DEFAULT '0',  `max_power` float DEFAULT '-300',  `min_power` float DEFAULT '-300',  `max_current` float DEFAULT '-300',  `min_current` float DEFAULT '-300',  `rack_id` int(11) DEFAULT NULL,  `fw_version` varchar(255) DEFAULT NULL,  `pop_flag` int(11) DEFAULT '0',  `pop_threshold` float DEFAULT '-1',  `door` smallint(6) DEFAULT '0',  `device_capacity` float DEFAULT '0',`restoreflag` int(11) DEFAULT '0',  PRIMARY KEY (`id`),  KEY `imac` (`mac`),  KEY `irack` (`rack_id`),  KEY `iport` (`portid`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("device_sensor_info", "CREATE TABLE `device_sensor_info` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `device_id` int(11) DEFAULT NULL,  `sensor_nm` varchar(255) DEFAULT NULL,  `max_humidity` float DEFAULT '-300',  `min_humidity` float DEFAULT '-300',  `max_temperature` float DEFAULT '-300',  `min_temperature` float DEFAULT '-300',  `max_press` float DEFAULT '-300',  `min_press` float DEFAULT '-300',  `sensor_type` smallint(6) DEFAULT '0',  `location_type` smallint(6) DEFAULT NULL,  PRIMARY KEY (`id`),  KEY `did` (`device_id`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("device_voltage", "CREATE TABLE `device_voltage` (  `id` bigint(20) NOT NULL,  `voltage` float DEFAULT NULL,  PRIMARY KEY (`id`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("deviceflag", "CREATE TABLE `deviceflag` (`dev_fresh_flag` int(11) NOT NULL DEFAULT '1',PRIMARY KEY (`dev_fresh_flag`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("event_info", "CREATE TABLE `event_info` (  `eventid` varchar(10) NOT NULL,  `logflag` int(11) DEFAULT NULL,  `mailflag` int(11) DEFAULT NULL,  `reserve` bigint(20) DEFAULT NULL,  PRIMARY KEY (`eventid`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("gatewaytable", "CREATE  TABLE IF NOT EXISTS  `gatewaytable` (  `gid` VARCHAR(32) NOT NULL ,  `bid` VARCHAR(32) NOT NULL ,  `sid` VARCHAR(32) NOT NULL ,  `slevel` INT NULL ,  `disname` VARCHAR(255) NULL ,  `capacity` FLOAT NULL ,  `eleflag` INT NULL ,  `distype` VARCHAR(64) NULL ,  `location` VARCHAR(255) NULL ,  `ip` VARCHAR(255) NULL ,  PRIMARY KEY (`gid`, `bid`, `sid`) ,  INDEX `ilev` (`slevel` ASC) )ENGINE = MyISAM");
			DBTools.hm_tablesql.Add("gatewaylastpd", "CREATE TABLE `gatewaylastpd` (  `bid` varchar(128) NOT NULL,  `sid` varchar(128) NOT NULL,  `eleflag` int(11) DEFAULT NULL,  `pd` double DEFAULT NULL,  `timemark` datetime DEFAULT NULL,  PRIMARY KEY (`bid`,`sid`),  KEY `itime` (`timemark`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("group_detail", "CREATE TABLE `group_detail` (  `group_id` int(11) NOT NULL,  `grouptype` varchar(20) DEFAULT NULL,  `dest_id` int(11) DEFAULT NULL,  KEY `idid` (`dest_id`),  KEY `igid` (`group_id`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("groupcontroltask", "CREATE TABLE `groupcontroltask` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `taskname` varchar(255) DEFAULT NULL,  `groupname` varchar(255) DEFAULT NULL,  `groupid` int(11) DEFAULT NULL,  `tasktype` int(11) DEFAULT NULL,  `status` int(11) DEFAULT NULL,  `reserve` varchar(255) DEFAULT NULL,  PRIMARY KEY (`id`),  KEY `igid` (`groupid`),  KEY `itnm` (`taskname`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("logrecords", "CREATE TABLE `logrecords` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `ticks` datetime DEFAULT NULL,  `eventid` varchar(10) DEFAULT NULL,  `logpara` text,  PRIMARY KEY (`id`),  KEY `it` (`ticks`),  KEY `ie` (`eventid`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("port_info", "CREATE TABLE `port_info` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `device_id` int(11) DEFAULT NULL,  `port_num` int(11) DEFAULT NULL,  `port_nm` varchar(20) DEFAULT NULL,  `port_confirmation` float DEFAULT '1',  `port_ondelay_time` bigint(20) DEFAULT '0',  `port_offdelay_time` bigint(20) DEFAULT '0',  `max_voltage` float DEFAULT '-300',  `min_voltage` float DEFAULT '-300',  `max_power_diss` float DEFAULT '-300',  `min_power_diss` float DEFAULT '0',  `max_power` float DEFAULT '-300',  `min_power` float DEFAULT '-300',  `max_current` float DEFAULT '-300',  `min_current` float DEFAULT '-300',  `shutdown_method` int(11) DEFAULT '1',  `mac` varchar(255) DEFAULT NULL,  PRIMARY KEY (`id`),  KEY `did` (`device_id`),  KEY `pnum` (`port_num`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("portflag", "CREATE TABLE `portflag` (  `port_fresh_flag` int(11) NOT NULL DEFAULT '1',  PRIMARY KEY (`port_fresh_flag`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("reportbill", "CREATE TABLE `reportbill` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `title` varchar(255) DEFAULT NULL,  `writer` varchar(50) DEFAULT NULL,  `reporttime` datetime DEFAULT NULL,  `reportpath` varchar(1024) DEFAULT NULL,  PRIMARY KEY (`id`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("reportinfo", "CREATE TABLE `reportinfo` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `title` varchar(255) DEFAULT NULL,  `writer` varchar(50) DEFAULT NULL,  `reporttime` datetime DEFAULT NULL,  `reportpath` varchar(1024) DEFAULT NULL,  PRIMARY KEY (`id`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("reportthermal", "CREATE TABLE `reportthermal` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `title` varchar(255) DEFAULT NULL,  `writer` varchar(50) DEFAULT NULL,  `reporttime` datetime DEFAULT NULL,  `reportpath` varchar(1024) DEFAULT NULL,  PRIMARY KEY (`id`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("sys_para", "CREATE TABLE `sys_para` (  `para_name` varchar(255) NOT NULL,  `para_type` varchar(255) DEFAULT NULL,  `para_value` varchar(255) DEFAULT NULL,  PRIMARY KEY (`para_name`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("sys_users", "CREATE TABLE `sys_users` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `user_name` varchar(50) DEFAULT NULL,  `userpwd` varchar(50) DEFAULT NULL,  `enabled` int(11) DEFAULT '0',  `role_type` int(11) DEFAULT '0',  `user_right` int(11) DEFAULT NULL,  `port_nm` varchar(200) DEFAULT NULL,  `devices` varchar(1024) DEFAULT NULL,  PRIMARY KEY (`id`),  KEY `iunm` (`user_name`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("taskschedule", "CREATE TABLE `taskschedule` (  `groupid` int(11) NOT NULL,  `dayofweek` int(11) DEFAULT NULL,  `optype` bigint(20) DEFAULT NULL,  `scheduletime` time DEFAULT NULL,  `status` int(11) DEFAULT NULL,  `reserve` varchar(255) DEFAULT NULL,  KEY `igid` (`groupid`),  KEY `iopt` (`optype`),  KEY `isc` (`scheduletime`)) ENGINE=MYISAM ");
			DBTools.hm_tablesql.Add("zone_info", "CREATE TABLE `zone_info` (  `id` int(11) NOT NULL AUTO_INCREMENT,  `zone_nm` varchar(50) DEFAULT NULL,  `racks` varchar(20480) DEFAULT NULL,  `sx` int(11) DEFAULT NULL,  `sy` int(11) DEFAULT NULL,  `ex` int(11) DEFAULT NULL,  `ey` int(11) DEFAULT NULL,  `color` varchar(50) DEFAULT NULL,  `reserve` bigint(20) DEFAULT NULL,  PRIMARY KEY (`id`),  KEY `icol` (`color`),  KEY `inm` (`zone_nm`)) ENGINE=MYISAM");
			DBTools.hm_tablesql.Add("ugp", "CREATE TABLE `ugp` (`uid` INT NOT NULL,`gid` INT NOT NULL,`status` INT NULL,PRIMARY KEY (`uid`, `gid`),INDEX `uid` (`uid` ASC),INDEX `gid` (`gid` ASC)) ENGINE = MyISAM");
		}
		public static bool IsMySQLFatalError(Exception errDB)
		{
			if (!DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return false;
			}
			if (errDB.GetType().FullName.Equals("MySql.Data.MySqlClient.MySqlException"))
			{
				MySqlException ex = (MySqlException)errDB;
				if (DBTools._listMySQLFatalErrorCodes.Contains(ex.Number))
				{
					return true;
				}
			}
			return false;
		}
		public static int preparetable(DbConnection con, DateTime dt_insert)
		{
			string str = "device_auto_info" + dt_insert.ToString("yyyyMMdd");
			string str2 = "bank_auto_info" + dt_insert.ToString("yyyyMMdd");
			string str3 = "port_auto_info" + dt_insert.ToString("yyyyMMdd");
			string text = dt_insert.ToString("yyyyMMdd");
			string text2 = "CREATE TABLE `bank_data_daily{0}` (";
			text2 += "`bank_id` int(11) NOT NULL,";
			text2 += "`power_consumption` bigint(20) DEFAULT NULL,";
			text2 += "`insert_time` date NOT NULL,";
			text2 += "KEY `index_bankpdd` (`bank_id`,`insert_time`),  KEY `bdd_idx1` (`bank_id`),  KEY `bdd_idx2` (`insert_time`)";
			text2 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text3 = "CREATE TABLE `bank_data_hourly{0}` (";
			text3 += "`bank_id` int(11) NOT NULL,";
			text3 += "`power_consumption` bigint(20) DEFAULT NULL,";
			text3 += "`insert_time` datetime NOT NULL,";
			text3 += "KEY `index_bankpdh` (`bank_id`,`insert_time`),  KEY `bdh_idx1` (`bank_id`),  KEY `bdh_idx2` (`insert_time`)";
			text3 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text4 = "CREATE TABLE `device_data_daily{0}` (";
			text4 += "`device_id` int(11) NOT NULL,";
			text4 += "`power_consumption` bigint(20) DEFAULT NULL,";
			text4 += "`insert_time` date NOT NULL,";
			text4 += "KEY `index_dev_daily` (`device_id`,`insert_time`),  KEY `ddd_idx1` (`device_id`),  KEY `ddd_idx2` (`insert_time`)";
			text4 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text5 = "CREATE TABLE `device_data_hourly{0}` (";
			text5 += "`device_id` int(11) NOT NULL,";
			text5 += "`power_consumption` bigint(20) DEFAULT NULL,";
			text5 += "`insert_time` datetime NOT NULL,";
			text5 += "KEY `index_devicepdh` (`device_id`,`insert_time`),  KEY `ddh_idx1` (`device_id`),  KEY `ddh_idx2` (`insert_time`)";
			text5 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text6 = "CREATE TABLE `port_data_daily{0}` (";
			text6 += "`port_id` int(11) NOT NULL,";
			text6 += "`power_consumption` bigint(20) DEFAULT NULL,";
			text6 += "`insert_time` date NOT NULL,";
			text6 += "KEY `index_port_daily` (`port_id`,`insert_time`),  KEY `pdd_idx1` (`port_id`),  KEY `pdd_idx2` (`insert_time`)";
			text6 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text7 = "CREATE TABLE `port_data_hourly{0}` (";
			text7 += "`port_id` int(11) NOT NULL,";
			text7 += "`power_consumption` bigint(20) DEFAULT NULL,";
			text7 += "`insert_time` datetime NOT NULL,";
			text7 += "KEY `index_portpdh` (`port_id`,`insert_time`), KEY `pdh_idx1` (`port_id`),  KEY `pdh_idx2` (`insert_time`)";
			text7 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			DbCommand dbCommand = null;
			try
			{
				if (con != null && con.State == ConnectionState.Open)
				{
					dbCommand = con.CreateCommand();
					dbCommand.CommandText = "CREATE TABLE `" + str + "` (`device_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `daiind1` (`device_id`),KEY `daiind2` (`insert_time`)) ENGINE=MYISAM ";
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = "CREATE TABLE `" + str2 + "` (`bank_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `baiind1` (`bank_id`),KEY `baiind2` (`insert_time`)) ENGINE=MYISAM ";
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = "CREATE TABLE `" + str3 + "` (`port_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `paiind1` (`port_id`),KEY `paiind2` (`insert_time`)) ENGINE=MYISAM ";
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = string.Format(text2, text);
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = string.Format(text3, text);
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = string.Format(text4, text);
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = string.Format(text5, text);
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = string.Format(text6, text);
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = string.Format(text7, text);
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = "CREATE TABLE `rackthermal_hourly" + text + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					dbCommand.CommandText = "CREATE TABLE `rackthermal_daily" + text + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
					try
					{
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
			}
			catch (Exception)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
			}
			return 0;
		}
		public static int MySQL_PrepareNewTables(string dbConnectString, string strTableSuffix, bool bAllwaysDrop = false)
		{
			string text = "CREATE TABLE IF NOT EXISTS `dev_min_pw_pd_{0}` (";
			text += "`device_id`         smallint NOT NULL,";
			text += "`power`             int DEFAULT NULL,";
			text += "`power_consumption` int DEFAULT NULL,";
			text += "`insert_time`       smallint NOT NULL";
			text += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text2 = "CREATE TABLE IF NOT EXISTS `dev_hour_pd_{0}` (";
			text2 += "`device_id`         smallint NOT NULL,";
			text2 += "`power_consumption` int DEFAULT NULL,";
			text2 += "`power_max`         int DEFAULT NULL,";
			text2 += "`insert_time`       tinyint NOT NULL";
			text2 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text3 = "CREATE TABLE IF NOT EXISTS `dev_day_pd_pm_{0}` (";
			text3 += "`device_id`         smallint NOT NULL,";
			text3 += "`power_consumption` bigint DEFAULT NULL,";
			text3 += "`power_max`         int DEFAULT NULL";
			text3 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text4 = "CREATE TABLE IF NOT EXISTS `port_min_pw_pd_{0}` (";
			text4 += "`port_id`            mediumint NOT NULL,";
			text4 += "`power`              int DEFAULT NULL,";
			text4 += "`power_consumption`  int DEFAULT NULL,";
			text4 += "`insert_time`        smallint NOT NULL";
			text4 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text5 = "CREATE TABLE IF NOT EXISTS `port_hour_pd_{0}` (";
			text5 += "`port_id`           mediumint NOT NULL,";
			text5 += "`power_consumption` int DEFAULT NULL,";
			text5 += "`power_max`         int DEFAULT NULL,";
			text5 += "`insert_time`       tinyint NOT NULL";
			text5 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text6 = "CREATE TABLE IF NOT EXISTS `port_day_pd_pm_{0}` (";
			text6 += "`port_id`           mediumint NOT NULL,";
			text6 += "`power_consumption` int DEFAULT NULL,";
			text6 += "`power_max`         int NOT NULL";
			text6 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text7 = "CREATE TABLE IF NOT EXISTS `rack_hour_thermal_{0}` (";
			text7 += "`rack_id`         smallint NOT NULL,";
			text7 += "`intakepeak`      float DEFAULT NULL,";
			text7 += "`exhaustpeak`     float DEFAULT NULL,";
			text7 += "`differencepeak`  float DEFAULT NULL,";
			text7 += "`insert_time`     tinyint NOT NULL";
			text7 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			string text8 = "CREATE TABLE IF NOT EXISTS `rack_day_thermal_{0}` (";
			text8 += "`rack_id`          smallint NOT NULL,";
			text8 += "`intakepeak`       float DEFAULT NULL,";
			text8 += "`exhaustpeak`      float DEFAULT NULL,";
			text8 += "`differencepeak`   float DEFAULT NULL";
			text8 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection(dbConnectString);
				mySqlConnection.Open();
				mySqlCommand = mySqlConnection.CreateCommand();
				if (bAllwaysDrop)
				{
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `dev_min_pw_pd_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `dev_hour_pd_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `dev_day_pd_pm_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `port_min_pw_pd_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `port_hour_pd_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `port_day_pd_pm_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `rack_hour_thermal_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					mySqlCommand.CommandText = "DROP TABLE IF EXISTS `rack_day_thermal_" + strTableSuffix + "`";
					try
					{
						mySqlCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				mySqlCommand.CommandText = string.Format(text, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text2, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text3, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text4, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text5, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text6, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text7, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				mySqlCommand.CommandText = string.Format(text8, strTableSuffix);
				try
				{
					mySqlCommand.ExecuteNonQuery();
				}
				catch
				{
				}
				try
				{
					mySqlCommand.Dispose();
				}
				catch
				{
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					if (mySqlCommand != null)
					{
						mySqlCommand.Dispose();
					}
					if (mySqlConnection != null)
					{
						mySqlConnection.Close();
						mySqlConnection.Dispose();
					}
				}
				catch
				{
				}
			}
			return 0;
		}
		public static int preparetable(DateTime dt_insert)
		{
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				string text = "device_auto_info" + dt_insert.ToString("yyyyMMdd");
				string text2 = "bank_auto_info" + dt_insert.ToString("yyyyMMdd");
				string text3 = "port_auto_info" + dt_insert.ToString("yyyyMMdd");
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				if (!DBTools.ht_tablename.ContainsKey(text))
				{
					flag = true;
				}
				if (!DBTools.ht_tablename.ContainsKey(text2))
				{
					flag2 = true;
				}
				if (!DBTools.ht_tablename.ContainsKey(text3))
				{
					flag3 = true;
				}
				string text4 = dt_insert.ToString("yyyyMMdd");
				string text5 = "CREATE TABLE `bank_data_daily{0}` (";
				text5 += "`bank_id` int(11) NOT NULL,";
				text5 += "`power_consumption` bigint(20) DEFAULT NULL,";
				text5 += "`insert_time` date NOT NULL,";
				text5 += "KEY `index_bankpdd` (`bank_id`,`insert_time`), KEY `bdd_idx1` (`bank_id`),  KEY `bdd_idx2` (`insert_time`)";
				text5 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
				string text6 = "CREATE TABLE `bank_data_hourly{0}` (";
				text6 += "`bank_id` int(11) NOT NULL,";
				text6 += "`power_consumption` bigint(20) DEFAULT NULL,";
				text6 += "`insert_time` datetime NOT NULL,";
				text6 += "KEY `index_bankpdh` (`bank_id`,`insert_time`), KEY `bdh_idx1` (`bank_id`),  KEY `bdh_idx2` (`insert_time`)";
				text6 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
				string text7 = "CREATE TABLE `device_data_daily{0}` (";
				text7 += "`device_id` int(11) NOT NULL,";
				text7 += "`power_consumption` bigint(20) DEFAULT NULL,";
				text7 += "`insert_time` date NOT NULL,";
				text7 += "KEY `index_dev_daily` (`device_id`,`insert_time`), KEY `ddd_idx1` (`device_id`),  KEY `ddd_idx2` (`insert_time`)";
				text7 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
				string text8 = "CREATE TABLE `device_data_hourly{0}` (";
				text8 += "`device_id` int(11) NOT NULL,";
				text8 += "`power_consumption` bigint(20) DEFAULT NULL,";
				text8 += "`insert_time` datetime NOT NULL,";
				text8 += "KEY `index_devicepdh` (`device_id`,`insert_time`), KEY `ddh_idx1` (`device_id`),  KEY `ddh_idx2` (`insert_time`)";
				text8 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
				string text9 = "CREATE TABLE `port_data_daily{0}` (";
				text9 += "`port_id` int(11) NOT NULL,";
				text9 += "`power_consumption` bigint(20) DEFAULT NULL,";
				text9 += "`insert_time` date NOT NULL,";
				text9 += "KEY `index_port_daily` (`port_id`,`insert_time`), KEY `pdd_idx1` (`port_id`),  KEY `pdd_idx2` (`insert_time`)";
				text9 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
				string text10 = "CREATE TABLE `port_data_hourly{0}` (";
				text10 += "`port_id` int(11) NOT NULL,";
				text10 += "`power_consumption` bigint(20) DEFAULT NULL,";
				text10 += "`insert_time` datetime NOT NULL,";
				text10 += "KEY `index_portpdh` (`port_id`,`insert_time`), KEY `pdh_idx1` (`port_id`),  KEY `pdh_idx2` (`insert_time`)";
				text10 += ") ENGINE=MyISAM DEFAULT CHARSET=utf8;";
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						if (flag)
						{
							dbCommand.CommandText = "CREATE TABLE `" + text + "` (`device_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `daiind1` (`device_id`),KEY `daiind2` (`insert_time`)) ENGINE=MYISAM ";
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add(text, text);
							}
							catch
							{
							}
						}
						if (flag2)
						{
							dbCommand.CommandText = "CREATE TABLE `" + text2 + "` (`bank_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `baiind1` (`bank_id`),KEY `baiind2` (`insert_time`)) ENGINE=MYISAM ";
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add(text2, text2);
							}
							catch
							{
							}
						}
						if (flag3)
						{
							dbCommand.CommandText = "CREATE TABLE `" + text3 + "` (`port_id` int(11) NOT NULL,`power` bigint(20) NOT NULL DEFAULT '0',`insert_time` datetime NOT NULL,KEY `paiind1` (`port_id`),KEY `paiind2` (`insert_time`)) ENGINE=MYISAM ";
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add(text3, text3);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("bank_data_daily" + text4))
						{
							dbCommand.CommandText = string.Format(text5, text4);
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("bank_data_daily" + text4, "bank_data_daily" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("bank_data_hourly" + text4))
						{
							dbCommand.CommandText = string.Format(text6, text4);
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("bank_data_hourly" + text4, "bank_data_hourly" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("device_data_daily" + text4))
						{
							dbCommand.CommandText = string.Format(text7, text4);
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("device_data_daily" + text4, "device_data_daily" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("device_data_hourly" + text4))
						{
							dbCommand.CommandText = string.Format(text8, text4);
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("device_data_hourly" + text4, "device_data_hourly" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("port_data_daily" + text4))
						{
							dbCommand.CommandText = string.Format(text9, text4);
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("port_data_daily" + text4, "port_data_daily" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("port_data_hourly" + text4))
						{
							dbCommand.CommandText = string.Format(text10, text4);
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("port_data_hourly" + text4, "port_data_hourly" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("rackthermal_hourly" + text4))
						{
							dbCommand.CommandText = "CREATE TABLE `rackthermal_hourly" + text4 + "` ( `rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` datetime NOT NULL,KEY `rhind1` (`rack_id`),KEY `rhind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("rackthermal_hourly" + text4, "rackthermal_hourly" + text4);
							}
							catch
							{
							}
						}
						if (!DBTools.ht_tablename.ContainsKey("rackthermal_daily" + text4))
						{
							dbCommand.CommandText = "CREATE TABLE `rackthermal_daily" + text4 + "` (`rack_id` int(11) NOT NULL,`intakepeak` float(16,4) DEFAULT NULL,`exhaustpeak` float(16,4) DEFAULT NULL,`differencepeak` float(16,4) DEFAULT NULL,`insert_time` date NOT NULL,KEY `rdind1` (`rack_id`),KEY `rdind2` (`insert_time`) ) ENGINE=MyISAM DEFAULT CHARSET=utf8";
							try
							{
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
							try
							{
								DBTools.ht_tablename.Add("rackthermal_daily" + text4, "rackthermal_daily" + text4);
							}
							catch
							{
							}
						}
						dbCommand.Dispose();
						dBConn.Close();
					}
				}
				catch (Exception)
				{
				}
			}
			return 0;
		}
		public static int DropMySQLDatabase(string dbname, string host, int port, string user, string pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			string text = "";
			int result;
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=;Data Source=",
					host,
					";Port=",
					port,
					";User Id=",
					user,
					";Password=",
					pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbCommand = dbConnection.CreateCommand();
				dbCommand.CommandText = "show variables where variable_name = 'datadir'";
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					text = Convert.ToString(dbDataReader.GetValue(1));
				}
				dbDataReader.Close();
				if (text != null && text.Length > 0)
				{
					string text2 = text;
					if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
					{
						text2 += Path.DirectorySeparatorChar;
					}
					DirectorySecurity directorySecurity = new DirectorySecurity();
					directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
					Directory.SetAccessControl(text2, directorySecurity);
				}
				try
				{
					dbCommand.CommandText = "DROP DATABASE `" + dbname + "`";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
					if (Directory.Exists(text + dbname))
					{
						int i = 1;
						while (i < 5)
						{
							if (!Directory.Exists(text + dbname))
							{
								break;
							}
							i++;
							if (DBTools.DeleteDir(text + dbname) > 0)
							{
								break;
							}
						}
					}
					try
					{
						dbCommand.CommandText = "DROP DATABASE `" + dbname + "`";
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
				result = 1;
			}
			catch (Exception)
			{
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
				result = -1;
			}
			return result;
		}
		public static int CheckMySQLParae(string host, int port, string user, string pwd)
		{
			DbConnection dbConnection = null;
			int result;
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=;Data Source=",
					host,
					";Port=",
					port,
					";User Id=",
					user,
					";Password=",
					pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbConnection.Close();
				result = 1;
			}
			catch (Exception)
			{
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
				result = -1;
			}
			return result;
		}
		public static int InitMySQLDatabase4Import(string host, int port, string user, string pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = "";
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=;Data Source=",
					host,
					";Port=",
					port,
					";User Id=",
					user,
					";Password=",
					pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbCommand = dbConnection.CreateCommand();
				dbCommand.CommandText = string.Concat(new string[]
				{
					"grant ALL on *.* to '",
					user,
					"'@'%' identified by \"",
					pwd,
					"\""
				});
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "set global max_connections=5000 ";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.CommandText = "show variables where variable_name = 'datadir'";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.Read())
					{
						text = Convert.ToString(dbDataReader.GetValue(1));
					}
					dbDataReader.Close();
					if (text != null && text.Length > 0)
					{
						string text2 = text;
						if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
						{
							text2 += Path.DirectorySeparatorChar;
						}
						DirectorySecurity directorySecurity = new DirectorySecurity();
						directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
						Directory.SetAccessControl(text2, directorySecurity);
					}
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "DROP DATABASE `eco`";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
					DBTools.DeleteDir(text + "eco");
					try
					{
						dbCommand.CommandText = "DROP DATABASE `eco`";
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				dbCommand.CommandText = "CREATE SCHEMA  eco";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "USE eco";
				dbCommand.ExecuteNonQuery();
				dbCommand.Dispose();
				dbConnection.Close();
				return 1;
			}
			catch (Exception)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
			}
			return -1;
		}
		public static int InitMySQLDatabase(string host, int port, string user, string pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			try
			{
				string text = "";
				dbConnection = new MySqlConnection(string.Concat(new object[]
				{
					"Database=;Data Source=",
					host,
					";Port=",
					port,
					";User Id=",
					user,
					";Password=",
					pwd,
					";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbCommand = dbConnection.CreateCommand();
				dbCommand.CommandText = string.Concat(new string[]
				{
					"grant ALL on *.* to '",
					user,
					"'@'%' identified by \"",
					pwd,
					"\""
				});
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "set global max_connections=5000 ";
				dbCommand.ExecuteNonQuery();
				try
				{
					dbCommand.CommandText = "show variables where variable_name = 'datadir'";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					if (dbDataReader.Read())
					{
						text = Convert.ToString(dbDataReader.GetValue(1));
					}
					dbDataReader.Close();
					if (text != null && text.Length > 0)
					{
						string text2 = text;
						if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
						{
							text2 += Path.DirectorySeparatorChar;
						}
						DirectorySecurity directorySecurity = new DirectorySecurity();
						directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
						Directory.SetAccessControl(text2, directorySecurity);
					}
				}
				catch
				{
				}
				try
				{
					dbCommand.CommandText = "DROP DATABASE `eco`";
					dbCommand.ExecuteNonQuery();
				}
				catch
				{
					DBTools.DeleteDir(text + "eco");
					try
					{
						dbCommand.CommandText = "DROP DATABASE `eco`";
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
				}
				dbCommand.CommandText = "CREATE SCHEMA  eco";
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "USE eco";
				dbCommand.ExecuteNonQuery();
				ICollection keys = DBTools.hm_tablesql.Keys;
				foreach (string text3 in keys)
				{
					bool flag = false;
					string key;
					switch (key = text3)
					{
					case "bank_auto_info":
						flag = true;
						break;
					case "bank_data_daily":
						flag = true;
						break;
					case "bank_data_hourly":
						flag = true;
						break;
					case "device_auto_info":
						flag = true;
						break;
					case "device_data_daily":
						flag = true;
						break;
					case "device_data_hourly":
						flag = true;
						break;
					case "port_auto_info":
						flag = true;
						break;
					case "port_data_daily":
						flag = true;
						break;
					case "port_data_hourly":
						flag = true;
						break;
					case "rack_effect":
						flag = true;
						break;
					case "rackthermal_daily":
						flag = true;
						break;
					case "rackthermal_hourly":
						flag = true;
						break;
					case "rci_daily":
						flag = true;
						break;
					case "rci_hourly":
						flag = true;
						break;
					case "tmpid":
						flag = true;
						break;
					}
					if (flag)
					{
						dbCommand.CommandText = Convert.ToString(DBTools.hm_tablesql[text3]);
						try
						{
							dbCommand.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
							{
								"DBERROR : INIT MYSQL DB ERROR ||| SQL is :",
								text3,
								"\r\n Exception is $$$",
								ex.Message,
								"\r\n",
								ex.StackTrace
							}));
						}
					}
				}
				dbCommand.Dispose();
				dbConnection.Close();
				return 1;
			}
			catch (Exception)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dbConnection.Close();
				}
				catch
				{
				}
			}
			return -1;
		}
		public static bool CheckSQL4DataDB(string str_sql)
		{
			return DBUrl.SERVERMODE || str_sql.IndexOf("port_auto_info") > 0 || str_sql.IndexOf("port_data_daily") > 0 || str_sql.IndexOf("device_auto_info") > 0 || str_sql.IndexOf("device_data_daily") > 0 || str_sql.IndexOf("rack_effect") > 0 || str_sql.IndexOf("bank_auto_info") > 0 || str_sql.IndexOf("bank_data_daily") > 0 || str_sql.IndexOf("device_data_hourly") > 0 || str_sql.IndexOf("port_data_hourly") > 0 || str_sql.IndexOf("bank_data_hourly") > 0 || str_sql.IndexOf("rackthermal_hourly") > 0 || str_sql.IndexOf("rackthermal_daily") > 0 || str_sql.IndexOf("rci_hourly") > 0 || str_sql.IndexOf("rci_daily") > 0;
		}
		public static int executeSql(string sql)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (DBTools.CheckSQL4DataDB(sql))
				{
					dBConn = DBConnPool.getDynaConnection();
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
					{
						sql = sql.Replace("#", "'");
					}
				}
				else
				{
					dBConn = DBConnPool.getConnection();
				}
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						sql = sql.Replace("#", "'");
					}
					dbCommand.CommandText = sql;
					result = dbCommand.ExecuteNonQuery();
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return result;
		}
		public static DataTable CreateDataTable(string str_sql)
		{
			DataTable dataTable = new DataTable();
			DBConn dBConn = null;
			DbDataAdapter dbDataAdapter = new OleDbDataAdapter();
			DbCommand dbCommand = new OleDbCommand();
			bool flag = false;
			if (!DBUrl.SERVERMODE && DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("ACCESS") && str_sql.IndexOf("rack_effect") > 0)
			{
				flag = true;
			}
			try
			{
				if (flag)
				{
					dBConn = DBConnPool.getThermalConnection();
				}
				else
				{
					if (DBTools.CheckSQL4DataDB(str_sql))
					{
						dBConn = DBConnPool.getDynaConnection();
						if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
						{
							str_sql = str_sql.Replace("#", "'");
						}
					}
					else
					{
						dBConn = DBConnPool.getConnection();
					}
				}
				if (dBConn.con != null)
				{
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						str_sql = str_sql.Replace("#", "'");
					}
					dbCommand.CommandText = str_sql;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbDataAdapter.Dispose();
				}
				catch
				{
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return dataTable;
		}
		public static DataTable CreateDataTable4ThermalDB(string str_sql)
		{
			DataTable dataTable = new DataTable();
			DBConn dBConn = null;
			DbDataAdapter dbDataAdapter = new OleDbDataAdapter();
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					str_sql = str_sql.Replace("#", "'");
				}
				dBConn = DBConnPool.getThermalConnection();
				if (dBConn.con != null)
				{
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						str_sql = str_sql.Replace("#", "'");
					}
					dbCommand.CommandText = str_sql;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbDataAdapter.Dispose();
				}
				catch
				{
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return dataTable;
		}
		public static DataTable CreateDataTable4SysDB(string str_sql)
		{
			DataTable dataTable = new DataTable();
			DBConn dBConn = null;
			DbDataAdapter dbDataAdapter = new OleDbDataAdapter();
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				if (DBUrl.SERVERMODE)
				{
					str_sql = str_sql.Replace("#", "'");
				}
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					if (DBUrl.SERVERMODE)
					{
						str_sql = str_sql.Replace("#", "'");
					}
					dbCommand.CommandText = str_sql;
					dbDataAdapter.SelectCommand = dbCommand;
					dbDataAdapter.Fill(dataTable);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbDataAdapter.Dispose();
				}
				catch
				{
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return dataTable;
		}
		private static ArrayList CreateDataTable_Array(DBConn conn, string str_sqlformat, string opIDs)
		{
			ArrayList arrayList = new ArrayList();
			string text = opIDs;
			while (text.Length > 0)
			{
				string text2;
				if (text.Length > 30000)
				{
					text2 = text.Substring(0, 30000);
					int num = text2.LastIndexOf(',');
					text2 = text.Substring(0, num);
					text = text.Substring(num + 1);
				}
				else
				{
					text2 = text;
					text = "";
				}
				string text3 = string.Format(str_sqlformat, text2);
				DataTable dataTable = new DataTable();
				DbDataAdapter dbDataAdapter = new OleDbDataAdapter();
				DbCommand dbCommand = new OleDbCommand();
				try
				{
					if (conn.con != null)
					{
						dbDataAdapter = DBConn.GetDataAdapter(conn.con);
						dbCommand = DBConn.GetCommandObject(conn.con);
						dbCommand.CommandType = CommandType.Text;
						if (DBUrl.SERVERMODE)
						{
							text3 = text3.Replace("#", "'");
						}
						dbCommand.CommandText = text3;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				}
				finally
				{
					try
					{
						dbDataAdapter.Dispose();
					}
					catch
					{
					}
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
				}
				arrayList.Add(dataTable);
			}
			return arrayList;
		}
		private static ArrayList CreateDataTable_Array(string str_sqlformat, string opIDs)
		{
			ArrayList arrayList = new ArrayList();
			string text = opIDs;
			while (text.Length > 0)
			{
				string text2;
				if (text.Length > 30000)
				{
					text2 = text.Substring(0, 30000);
					int num = text2.LastIndexOf(',');
					text2 = text.Substring(0, num);
					text = text.Substring(num + 1);
				}
				else
				{
					text2 = text;
					text = "";
				}
				string str_sql = string.Format(str_sqlformat, text2);
				DataTable value = DBTools.CreateDataTable(str_sql);
				arrayList.Add(value);
			}
			return arrayList;
		}
		private static DataTable MergeDataTable(ArrayList listDataTable, string PrimaryKey)
		{
			DataTable dataTable = null;
			if (listDataTable.Count == 1)
			{
				dataTable = (DataTable)listDataTable[0];
			}
			else
			{
				if (listDataTable.Count > 1)
				{
					dataTable = (DataTable)listDataTable[0];
					dataTable.PrimaryKey = new DataColumn[]
					{
						dataTable.Columns[PrimaryKey]
					};
					for (int i = 1; i < listDataTable.Count; i++)
					{
						DataTable dataTable2 = (DataTable)listDataTable[i];
						dataTable2.PrimaryKey = new DataColumn[]
						{
							dataTable2.Columns[PrimaryKey]
						};
						dataTable.Merge(dataTable2);
					}
				}
			}
			return dataTable;
		}
		public static Hashtable GetRackPDSum(int i_period_type, ArrayList allRacks)
		{
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			Hashtable hashtable = new Hashtable();
			DateTime dateTime = DateTime.Now;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DataTable dataTable = new DataTable();
			DbDataAdapter dbDataAdapter = null;
			bool flag = false;
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				flag = true;
				int queryThreads = MultiThreadQuery.getQueryThreads();
				if (queryThreads > 0)
				{
					Hashtable rackPDSum = MultiThreadQuery.GetRackPDSum(i_period_type);
					MultiThreadQuery.WriteLog("GetRackPDSum(multi-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
					return rackPDSum;
				}
			}
			DateTime dt_inserttime = DateTime.Now;
			DateTime dateTime5;
			switch (i_period_type)
			{
			case 0:
			{
				dateTime = DateTime.Now;
				DateTime dateTime2 = dateTime;
				DateTime dateTime3 = dateTime2.AddHours(1.0);
				string.Concat(new string[]
				{
					" and insert_time between #",
					dateTime2.ToString("yyyy-MM-dd HH:00:00"),
					"# and #",
					dateTime3.ToString("yyyy-MM-dd HH:00:00"),
					"#"
				});
				try
				{
					dBConn = DBConnPool.getDynaConnection(dateTime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache = DBCache.GetDeviceCache();
						if (deviceCache != null && deviceCache.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = string.Concat(new object[]
							{
								"select sum(power_consumption)/",
								10000.0,
								" as power_con,device_id from device_data_hourly where insert_time between #",
								dateTime2.ToString("yyyy-MM-dd HH:00:00"),
								"# and #",
								dateTime3.ToString("yyyy-MM-dd HH:00:00"),
								"# group by device_id"
							});
							if (flag)
							{
								text = text.Replace("#", "'");
							}
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow in dataTable.Rows)
								{
									int num = Convert.ToInt32(dataRow[1]);
									if (deviceCache.ContainsKey(num))
									{
										DeviceInfo deviceInfo = (DeviceInfo)deviceCache[num];
										long rackID = deviceInfo.RackID;
										double num2 = Convert.ToDouble(dataRow[0]);
										if (hashtable.ContainsKey(rackID))
										{
											double num3 = (double)hashtable[rackID];
											hashtable[rackID] = num2 + num3;
										}
										else
										{
											hashtable.Add(rackID, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
					goto IL_1873;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1873;
				}
				break;
			}
			case 1:
				break;
			case 2:
				goto IL_5C8;
			case 3:
			{
				dateTime = DateTime.Now.Date;
				DateTime dateTime4 = dateTime.AddDays((double)(1 - dateTime.Day));
				int num4 = 0;
				if (!flag)
				{
					goto IL_EF0;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache2 = DBCache.GetDeviceCache();
						if (deviceCache2 != null && deviceCache2.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = string.Concat(new object[]
							{
								"select sum(power_consumption)/",
								10000.0,
								" as power_con,device_id from device_data_daily where insert_time >= '",
								dateTime4.ToString("yyyy-MM-dd 00:00:00"),
								"' and insert_time <'",
								dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
								"' group by device_id"
							});
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow2 in dataTable.Rows)
								{
									int num5 = Convert.ToInt32(dataRow2[1]);
									if (deviceCache2.ContainsKey(num5))
									{
										DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache2[num5];
										long rackID2 = deviceInfo2.RackID;
										double num2 = Convert.ToDouble(dataRow2[0]);
										if (hashtable.ContainsKey(rackID2))
										{
											double num6 = (double)hashtable[rackID2];
											hashtable[rackID2] = num2 + num6;
										}
										else
										{
											hashtable.Add(rackID2, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
					goto IL_1873;
				}
				catch
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1873;
				}
				IL_C9A:
				dt_inserttime = dateTime4.AddDays((double)num4);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")) || dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(dateTime4.AddMonths(1).Date.ToString("yyyy-MM-dd")))
				{
					goto IL_1873;
				}
				num4++;
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache3 = DBCache.GetDeviceCache();
						if (deviceCache3 != null && deviceCache3.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = "select sum(power_consumption)/" + 10000.0 + " as power_con,device_id from device_data_daily group by device_id";
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow3 in dataTable.Rows)
								{
									int num7 = Convert.ToInt32(dataRow3[1]);
									if (deviceCache3.ContainsKey(num7))
									{
										DeviceInfo deviceInfo3 = (DeviceInfo)deviceCache3[num7];
										long rackID3 = deviceInfo3.RackID;
										double num2 = Convert.ToDouble(dataRow3[0]);
										if (hashtable.ContainsKey(rackID3))
										{
											double num8 = (double)hashtable[rackID3];
											hashtable[rackID3] = num2 + num8;
										}
										else
										{
											hashtable.Add(rackID3, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
				IL_EF0:
				if (num4 > 31)
				{
					goto IL_1873;
				}
				goto IL_C9A;
			}
			case 4:
			{
				dateTime = DateTime.Now.Date;
				dateTime5 = dateTime.AddMonths(-((dateTime.Month - 1) % 3)).AddDays((double)(1 - dateTime.Day));
				dateTime5.AddMonths(3);
				int num9 = 0;
				if (!flag)
				{
					goto IL_13AB;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache4 = DBCache.GetDeviceCache();
						if (deviceCache4 != null && deviceCache4.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = string.Concat(new object[]
							{
								"select sum(power_consumption)/",
								10000.0,
								" as power_con,device_id from device_data_daily where insert_time >= '",
								dateTime5.ToString("yyyy-MM-dd 00:00:00"),
								"' and insert_time <'",
								dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
								"' group by device_id"
							});
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow4 in dataTable.Rows)
								{
									int num10 = Convert.ToInt32(dataRow4[1]);
									if (deviceCache4.ContainsKey(num10))
									{
										DeviceInfo deviceInfo4 = (DeviceInfo)deviceCache4[num10];
										long rackID4 = deviceInfo4.RackID;
										double num2 = Convert.ToDouble(dataRow4[0]);
										if (hashtable.ContainsKey(rackID4))
										{
											double num11 = (double)hashtable[rackID4];
											hashtable[rackID4] = num2 + num11;
										}
										else
										{
											hashtable.Add(rackID4, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
					goto IL_1873;
				}
				catch
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1873;
				}
				IL_1155:
				dt_inserttime = dateTime5.AddDays((double)num9);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")) || dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(dateTime5.AddMonths(3).Date.ToString("yyyy-MM-dd")))
				{
					goto IL_1873;
				}
				num9++;
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache5 = DBCache.GetDeviceCache();
						if (deviceCache5 != null && deviceCache5.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = "select sum(power_consumption)/" + 10000.0 + " as power_con,device_id from device_data_daily group by device_id";
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow5 in dataTable.Rows)
								{
									int num12 = Convert.ToInt32(dataRow5[1]);
									if (deviceCache5.ContainsKey(num12))
									{
										DeviceInfo deviceInfo5 = (DeviceInfo)deviceCache5[num12];
										long rackID5 = deviceInfo5.RackID;
										double num2 = Convert.ToDouble(dataRow5[0]);
										if (hashtable.ContainsKey(rackID5))
										{
											double num13 = (double)hashtable[rackID5];
											hashtable[rackID5] = num2 + num13;
										}
										else
										{
											hashtable.Add(rackID5, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
				IL_13AB:
				if (num9 > 92)
				{
					goto IL_1873;
				}
				goto IL_1155;
			}
			case 5:
			{
				dateTime = DateTime.Now.Date;
				dateTime5 = dateTime.AddDays((double)(1 - Convert.ToInt32(dateTime.DayOfYear.ToString("d"))));
				int num14 = 0;
				if (!flag)
				{
					goto IL_1867;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache6 = DBCache.GetDeviceCache();
						if (deviceCache6 != null && deviceCache6.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = string.Concat(new object[]
							{
								"select sum(power_consumption)/",
								10000.0,
								" as power_con,device_id from device_data_daily where insert_time >= '",
								dateTime5.ToString("yyyy-MM-dd 00:00:00"),
								"' and insert_time <'",
								dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
								"' group by device_id"
							});
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow6 in dataTable.Rows)
								{
									int num15 = Convert.ToInt32(dataRow6[1]);
									if (deviceCache6.ContainsKey(num15))
									{
										DeviceInfo deviceInfo6 = (DeviceInfo)deviceCache6[num15];
										long rackID6 = deviceInfo6.RackID;
										double num2 = Convert.ToDouble(dataRow6[0]);
										if (hashtable.ContainsKey(rackID6))
										{
											double num16 = (double)hashtable[rackID6];
											hashtable[rackID6] = num2 + num16;
										}
										else
										{
											hashtable.Add(rackID6, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
					goto IL_1873;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1873;
				}
				IL_1611:
				dt_inserttime = dateTime5.AddDays((double)num14);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")) || dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(dateTime5.AddYears(1).Date.ToString("yyyy-MM-dd")))
				{
					goto IL_1873;
				}
				num14++;
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache7 = DBCache.GetDeviceCache();
						if (deviceCache7 != null && deviceCache7.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = "select sum(power_consumption)/" + 10000.0 + " as power_con,device_id from device_data_daily group by device_id";
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow7 in dataTable.Rows)
								{
									int num17 = Convert.ToInt32(dataRow7[1]);
									if (deviceCache7.ContainsKey(num17))
									{
										DeviceInfo deviceInfo7 = (DeviceInfo)deviceCache7[num17];
										long rackID7 = deviceInfo7.RackID;
										double num2 = Convert.ToDouble(dataRow7[0]);
										if (hashtable.ContainsKey(rackID7))
										{
											double num18 = (double)hashtable[rackID7];
											hashtable[rackID7] = num2 + num18;
										}
										else
										{
											hashtable.Add(rackID7, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
				IL_1867:
				if (num14 > 366)
				{
					goto IL_1873;
				}
				goto IL_1611;
			}
			default:
				goto IL_1873;
			}
			dateTime = DateTime.Now.Date;
			try
			{
				dBConn = DBConnPool.getDynaConnection(dateTime);
				if (dBConn != null && dBConn.con != null)
				{
					Hashtable deviceCache8 = DBCache.GetDeviceCache();
					if (deviceCache8 != null && deviceCache8.Count > 0)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text;
						if (flag)
						{
							text = string.Concat(new object[]
							{
								"select sum(power_consumption)/",
								10000.0,
								" as power_con,device_id from device_data_daily where insert_time between '",
								dateTime.ToString("yyyy-MM-dd 00:00:00"),
								"' and '",
								dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
								"' group by device_id"
							});
						}
						else
						{
							text = "select sum(power_consumption)/" + 10000.0 + " as power_con,device_id from device_data_daily group by device_id";
						}
						dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = text;
						dbDataAdapter.SelectCommand = dbCommand;
						dbDataAdapter.Fill(dataTable);
						dbDataAdapter.Dispose();
						dbCommand.Dispose();
						dBConn.Close();
						if (dataTable != null)
						{
							foreach (DataRow dataRow8 in dataTable.Rows)
							{
								int num19 = Convert.ToInt32(dataRow8[1]);
								if (deviceCache8.ContainsKey(num19))
								{
									DeviceInfo deviceInfo8 = (DeviceInfo)deviceCache8[num19];
									long rackID8 = deviceInfo8.RackID;
									double num2 = Convert.ToDouble(dataRow8[0]);
									if (hashtable.ContainsKey(rackID8))
									{
										double num20 = (double)hashtable[rackID8];
										hashtable[rackID8] = num2 + num20;
									}
									else
									{
										hashtable.Add(rackID8, num2);
									}
								}
							}
						}
						dataTable = new DataTable();
					}
				}
				goto IL_1873;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~RefreshRackPDCache Error : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbDataAdapter.Dispose();
				}
				catch
				{
				}
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
				goto IL_1873;
			}
			IL_5C8:
			dateTime = DateTime.Now.Date;
			dateTime5 = dateTime.AddDays((double)(1 - Convert.ToInt32(dateTime.DayOfWeek.ToString("d"))));
			if (dateTime.DayOfWeek == DayOfWeek.Sunday)
			{
				dateTime5 = dateTime5.AddDays(-7.0);
			}
			dt_inserttime = DateTime.Now;
			if (flag)
			{
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache9 = DBCache.GetDeviceCache();
						if (deviceCache9 != null && deviceCache9.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = string.Concat(new object[]
							{
								"select sum(power_consumption)/",
								10000.0,
								" as power_con,device_id from device_data_daily where insert_time >= '",
								dateTime5.ToString("yyyy-MM-dd 00:00:00"),
								"' and insert_time <'",
								dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
								"' group by device_id"
							});
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow9 in dataTable.Rows)
								{
									int num21 = Convert.ToInt32(dataRow9[1]);
									if (deviceCache9.ContainsKey(num21))
									{
										DeviceInfo deviceInfo9 = (DeviceInfo)deviceCache9[num21];
										long rackID9 = deviceInfo9.RackID;
										double num2 = Convert.ToDouble(dataRow9[0]);
										if (hashtable.ContainsKey(rackID9))
										{
											double num22 = (double)hashtable[rackID9];
											hashtable[rackID9] = num2 + num22;
										}
										else
										{
											hashtable.Add(rackID9, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
					goto IL_1873;
				}
				catch
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1873;
				}
			}
			for (int i = 0; i < 7; i++)
			{
				dt_inserttime = dateTime5.AddDays((double)i);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")))
				{
					break;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						Hashtable deviceCache10 = DBCache.GetDeviceCache();
						if (deviceCache10 != null && deviceCache10.Count > 0)
						{
							dbCommand = dBConn.con.CreateCommand();
							string text = "select sum(power_consumption)/" + 10000.0 + " as power_con,device_id from device_data_daily group by device_id";
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = text;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable);
							dbDataAdapter.Dispose();
							dbCommand.Dispose();
							dBConn.Close();
							if (dataTable != null)
							{
								foreach (DataRow dataRow10 in dataTable.Rows)
								{
									int num23 = Convert.ToInt32(dataRow10[1]);
									if (deviceCache10.ContainsKey(num23))
									{
										DeviceInfo deviceInfo10 = (DeviceInfo)deviceCache10[num23];
										long rackID10 = deviceInfo10.RackID;
										double num2 = Convert.ToDouble(dataRow10[0]);
										if (hashtable.ContainsKey(rackID10))
										{
											double num24 = (double)hashtable[rackID10];
											hashtable[rackID10] = num2 + num24;
										}
										else
										{
											hashtable.Add(rackID10, num2);
										}
									}
								}
							}
							dataTable = new DataTable();
						}
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
			IL_1873:
			MultiThreadQuery.WriteLog("GetRackPDSum(single-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			return hashtable;
		}
		public static double GetDataCenterPDSum(int i_period_type)
		{
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			double num = 0.0;
			DateTime dateTime = DateTime.Now;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DateTime dt_inserttime = DateTime.Now;
			bool flag = false;
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				flag = true;
				int queryThreads = MultiThreadQuery.getQueryThreads();
				if (queryThreads > 0)
				{
					num = MultiThreadQuery.GetDataCenterPDSum(i_period_type);
					MultiThreadQuery.WriteLog("GetDataCenterPDSum(multi-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
					return num;
				}
			}
			string text;
			DateTime dateTime5;
			switch (i_period_type)
			{
			case 0:
			{
				dateTime = DateTime.Now;
				DateTime dateTime2 = dateTime;
				DateTime dateTime3 = dateTime2.AddHours(1.0);
				text = string.Concat(new string[]
				{
					" and insert_time between #",
					dateTime2.ToString("yyyy-MM-dd HH:00:00"),
					"# and #",
					dateTime3.ToString("yyyy-MM-dd HH:00:00"),
					"#"
				});
				try
				{
					dBConn = DBConnPool.getDynaConnection(dateTime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = string.Concat(new object[]
						{
							"select sum(power_consumption)/",
							10000.0,
							" as power_con from device_data_hourly where 1=1 ",
							text
						});
						if (flag)
						{
							text2 = text2.Replace("#", "'");
						}
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj = dbCommand.ExecuteScalar();
						try
						{
							if (obj != DBNull.Value)
							{
								num = Convert.ToDouble(obj);
							}
							else
							{
								num = 0.0;
							}
						}
						catch (Exception)
						{
							num = 0.0;
						}
						dbCommand.Dispose();
						dBConn.Close();
					}
					goto IL_1046;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1046;
				}
				break;
			}
			case 1:
				break;
			case 2:
				goto IL_397;
			case 3:
			{
				dateTime = DateTime.Now.Date;
				DateTime dateTime4 = dateTime.AddDays((double)(1 - dateTime.Day));
				int num2 = 0;
				if (!flag)
				{
					goto IL_9C7;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = string.Concat(new object[]
						{
							"select sum(power_consumption)/",
							10000.0,
							" as power_con from device_data_daily where insert_time >= '",
							dateTime4.ToString("yyyy-MM-dd 00:00:00"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
							"' "
						});
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj2 = dbCommand.ExecuteScalar();
						double num3 = 0.0;
						try
						{
							if (obj2 != DBNull.Value)
							{
								num3 = Convert.ToDouble(obj2);
							}
							else
							{
								num3 = 0.0;
							}
						}
						catch (Exception)
						{
							num3 = 0.0;
						}
						num += num3;
						dbCommand.Dispose();
						dBConn.Close();
					}
					goto IL_1046;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1046;
				}
				IL_835:
				dt_inserttime = dateTime4.AddDays((double)num2);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")) || dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(dateTime4.AddMonths(1).Date.ToString("yyyy-MM-dd")))
				{
					goto IL_1046;
				}
				num2++;
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = "select sum(power_consumption)/" + 10000.0 + " as power_con from device_data_daily ";
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj3 = dbCommand.ExecuteScalar();
						double num4 = 0.0;
						try
						{
							if (obj3 != DBNull.Value)
							{
								num4 = Convert.ToDouble(obj3);
							}
							else
							{
								num4 = 0.0;
							}
						}
						catch (Exception)
						{
							num4 = 0.0;
						}
						num += num4;
						dbCommand.Dispose();
						dBConn.Close();
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
				IL_9C7:
				if (num2 > 31)
				{
					goto IL_1046;
				}
				goto IL_835;
			}
			case 4:
			{
				dateTime = DateTime.Now.Date;
				dateTime5 = dateTime.AddMonths(-((dateTime.Month - 1) % 3)).AddDays((double)(1 - dateTime.Day));
				dateTime5.AddMonths(3);
				int num5 = 0;
				if (!flag)
				{
					goto IL_D06;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = string.Concat(new object[]
						{
							"select sum(power_consumption)/",
							10000.0,
							" as power_con from device_data_daily where insert_time >= '",
							dateTime5.ToString("yyyy-MM-dd 00:00:00"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
							"' "
						});
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj4 = dbCommand.ExecuteScalar();
						double num6 = 0.0;
						try
						{
							if (obj4 != DBNull.Value)
							{
								num6 = Convert.ToDouble(obj4);
							}
							else
							{
								num6 = 0.0;
							}
						}
						catch (Exception)
						{
							num6 = 0.0;
						}
						num += num6;
						dbCommand.Dispose();
						dBConn.Close();
					}
					goto IL_1046;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1046;
				}
				IL_B74:
				dt_inserttime = dateTime5.AddDays((double)num5);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")) || dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(dateTime5.AddMonths(3).Date.ToString("yyyy-MM-dd")))
				{
					goto IL_1046;
				}
				num5++;
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = "select sum(power_consumption)/" + 10000.0 + " as power_con from device_data_daily ";
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj5 = dbCommand.ExecuteScalar();
						double num7 = 0.0;
						try
						{
							if (obj5 != DBNull.Value)
							{
								num7 = Convert.ToDouble(obj5);
							}
							else
							{
								num7 = 0.0;
							}
						}
						catch (Exception)
						{
							num7 = 0.0;
						}
						num += num7;
						dbCommand.Dispose();
						dBConn.Close();
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
				IL_D06:
				if (num5 > 92)
				{
					goto IL_1046;
				}
				goto IL_B74;
			}
			case 5:
			{
				dateTime = DateTime.Now.Date;
				dateTime5 = dateTime.AddDays((double)(1 - Convert.ToInt32(dateTime.DayOfYear.ToString("d"))));
				int num8 = 0;
				if (!flag)
				{
					goto IL_103A;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = string.Concat(new object[]
						{
							"select sum(power_consumption)/",
							10000.0,
							" as power_con from device_data_daily where insert_time >= '",
							dateTime5.ToString("yyyy-MM-dd 00:00:00"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
							"' "
						});
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj6 = dbCommand.ExecuteScalar();
						double num9 = 0.0;
						try
						{
							if (obj6 != DBNull.Value)
							{
								num9 = Convert.ToDouble(obj6);
							}
							else
							{
								num9 = 0.0;
							}
						}
						catch (Exception)
						{
							num9 = 0.0;
						}
						num += num9;
						dbCommand.Dispose();
						dBConn.Close();
					}
					goto IL_1046;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1046;
				}
				IL_EA8:
				dt_inserttime = dateTime5.AddDays((double)num8);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")) || dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(dateTime5.AddYears(1).Date.ToString("yyyy-MM-dd")))
				{
					goto IL_1046;
				}
				num8++;
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = "select sum(power_consumption)/" + 10000.0 + " as power_con from device_data_daily ";
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj7 = dbCommand.ExecuteScalar();
						double num10 = 0.0;
						try
						{
							if (obj7 != DBNull.Value)
							{
								num10 = Convert.ToDouble(obj7);
							}
							else
							{
								num10 = 0.0;
							}
						}
						catch (Exception)
						{
							num10 = 0.0;
						}
						num += num10;
						dbCommand.Dispose();
						dBConn.Close();
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
				IL_103A:
				if (num8 > 366)
				{
					goto IL_1046;
				}
				goto IL_EA8;
			}
			default:
				goto IL_1046;
			}
			dateTime = DateTime.Now.Date;
			text = " and insert_time =#" + dateTime.ToString("yyyy-MM-dd") + "#";
			try
			{
				dBConn = DBConnPool.getDynaConnection(dateTime);
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					string text2 = "select sum(power_consumption)/" + 10000.0 + " as power_con from device_data_daily ";
					if (flag)
					{
						text2 = string.Concat(new object[]
						{
							"select sum(power_consumption)/",
							10000.0,
							" as power_con from device_data_daily where insert_time >= '",
							dateTime.ToString("yyyy-MM-dd 00:00:00"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
							"' "
						});
					}
					dbCommand.CommandText = text2;
					object obj8 = dbCommand.ExecuteScalar();
					try
					{
						if (obj8 != DBNull.Value)
						{
							num = Convert.ToDouble(obj8);
						}
						else
						{
							num = 0.0;
						}
					}
					catch (Exception)
					{
						num = 0.0;
					}
					dbCommand.Dispose();
					dBConn.Close();
				}
				goto IL_1046;
			}
			catch (Exception)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
				goto IL_1046;
			}
			IL_397:
			dateTime = DateTime.Now.Date;
			dateTime5 = dateTime.AddDays((double)(1 - Convert.ToInt32(dateTime.DayOfWeek.ToString("d"))));
			if (dateTime.DayOfWeek == DayOfWeek.Sunday)
			{
				dateTime5 = dateTime5.AddDays(-7.0);
			}
			dt_inserttime = DateTime.Now;
			if (flag)
			{
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = string.Concat(new object[]
						{
							"select sum(power_consumption)/",
							10000.0,
							" as power_con from device_data_daily where insert_time >= '",
							dateTime5.ToString("yyyy-MM-dd 00:00:00"),
							"' and insert_time < '",
							dateTime.AddDays(1.0).ToString("yyyy-MM-dd 00:00:00"),
							"' "
						});
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj9 = dbCommand.ExecuteScalar();
						double num11 = 0.0;
						try
						{
							if (obj9 != DBNull.Value)
							{
								num11 = Convert.ToDouble(obj9);
							}
							else
							{
								num11 = 0.0;
							}
						}
						catch (Exception)
						{
							num11 = 0.0;
						}
						num += num11;
						dbCommand.Dispose();
						dBConn.Close();
					}
					goto IL_1046;
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
					goto IL_1046;
				}
			}
			for (int i = 0; i < 7; i++)
			{
				dt_inserttime = dateTime5.AddDays((double)i);
				if (dt_inserttime.Date.ToString("yyyy-MM-dd").Equals(DateTime.Now.AddDays(1.0).Date.ToString("yyyy-MM-dd")))
				{
					break;
				}
				try
				{
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string text2 = "select sum(power_consumption)/" + 10000.0 + " as power_con from device_data_daily ";
						if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							text2 = text2.Replace("#", "'");
						}
						dbCommand.CommandText = text2;
						object obj10 = dbCommand.ExecuteScalar();
						double num12 = 0.0;
						try
						{
							if (obj10 != DBNull.Value)
							{
								num12 = Convert.ToDouble(obj10);
							}
							else
							{
								num12 = 0.0;
							}
						}
						catch (Exception)
						{
							num12 = 0.0;
						}
						num += num12;
						dbCommand.Dispose();
						dBConn.Close();
					}
				}
				catch (Exception)
				{
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
			IL_1046:
			MultiThreadQuery.WriteLog("GetDataCenterPDSum(single-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			return num;
		}
		public static string GetCurrentDBVersion()
		{
			string result = "";
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "select para_value from sys_para where para_name = 'DBVERSION' ";
					object obj = dbCommand.ExecuteScalar();
					if (obj != null && obj != DBNull.Value)
					{
						result = Convert.ToString(obj);
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
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception)
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
				if (dBConn != null)
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
			return result;
		}
		public static int UpdateDatabase(string str_logkey)
		{
			DBTools.b_up = true;
			string currentDBVersion = DBTools.GetCurrentDBVersion();
			if (currentDBVersion == null || currentDBVersion.Length == 0)
			{
				DBTools.b_up = false;
				return -1;
			}
			if (!DBUrl.SERVERMODE)
			{
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
						string text2 = AppDomain.CurrentDomain.BaseDirectory + "datadb.org";
						if (File.Exists(text2))
						{
							File.Copy(text2, text + "datadb.org", true);
							File.Copy(text2, text + "thermaldb.mdb", true);
						}
					}
				}
				catch
				{
					DBTools.b_up = false;
					int result = -1;
					return result;
				}
			}
			DBConn dBConn = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn != null || dBConn.con != null)
				{
					int num = AccessDBUpdate.UPSYSDBSTRUCTURE();
					if (num < 0)
					{
						int result = -1;
						return result;
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.2.1.3") < 0)
					{
						int num2 = AccessDBUpdate.UP2V1213(dBConn);
						if (num2 < 0)
						{
							int result = -1;
							return result;
						}
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.2.3.0") < 0)
					{
						int num3 = AccessDBUpdate.UP2V1230(dBConn);
						if (num3 < 0)
						{
							int result = -1;
							return result;
						}
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.3.2.5") < 0)
					{
						if (!DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
						{
							int num4 = AccessDBUpdate.UPDATADB2V1325();
							DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Finish Transfer database(by files) to Multi files");
							if (num4 < 0)
							{
								int result = -1;
								return result;
							}
						}
						if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.3.2.5") < 0)
						{
							int num5 = AccessDBUpdate.UPLog2V1325(dBConn);
							if (num5 < 0)
							{
								int result = -1;
								return result;
							}
						}
						int num6 = AccessDBUpdate.UP2V1325(dBConn);
						if (num6 < 0)
						{
							int result = -1;
							return result;
						}
					}
					int num7 = AccessDBUpdate.RenewDeviceVoltage(dBConn);
					if (num7 < 0)
					{
						int result = -1;
						return result;
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.3.7.0") < 0)
					{
						if (currentDBVersion.CompareTo("1.3.6.6") == 0)
						{
							int num8 = AccessDBUpdate.V1366UP1370(dBConn);
							if (num8 < 0)
							{
								int result = -1;
								return result;
							}
						}
						else
						{
							int num9 = AccessDBUpdate.UP2V1370(dBConn);
							if (num9 < 0)
							{
								int result = -1;
								return result;
							}
						}
						DBTools.CURRENT_VERSION = "1.3.7.0";
					}
					if (currentDBVersion.CompareTo("1.3.7.0") == 0)
					{
						DBTools.CURRENT_VERSION = "1.3.7.0";
					}
					if ((currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.4.0.1") < 0) && currentDBVersion.CompareTo("1.3.9.9") == 0)
					{
						int num10 = AccessDBUpdate.V1399UP1401(dBConn);
						if (num10 < 0)
						{
							int result = -1;
							return result;
						}
						DBTools.CURRENT_VERSION = "1.4.0.1";
					}
					if (currentDBVersion.CompareTo("1.4.0.1") == 0)
					{
						DBTools.CURRENT_VERSION = "1.4.0.1";
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.4.0.3") < 0)
					{
						int num11 = AccessDBUpdate.UP2SupportPOP(dBConn);
						if (num11 < 0)
						{
							int result = -1;
							return result;
						}
					}
					if (DBTools.CURRENT_VERSION.CompareTo("1.3.7.0") == 0)
					{
						DBMaintain.SetConvertOldDataStatus(false);
						Task.Factory.StartNew<int>(() => AccessDBUpdate.V1370UP1403());
					}
					if (DBTools.CURRENT_VERSION.CompareTo("1.4.0.1") == 0)
					{
						DBMaintain.SetConvertOldDataStatus(false);
						Task.Factory.StartNew<int>(() => AccessDBUpdate.V1401UP1403());
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.4.0.5") < 0)
					{
						int num12 = AccessDBUpdate.V1403UP1405(dBConn);
						if (num12 < 0)
						{
							int result = -1;
							return result;
						}
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.4.0.6") < 0)
					{
						int num13 = AccessDBUpdate.V1405UP1406(dBConn);
						if (num13 < 0)
						{
							int result = -1;
							return result;
						}
					}
					if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.4.0.7") < 0)
					{
						int num14 = AccessDBUpdate.V1406UP1407(dBConn);
						if (num14 < 0)
						{
							int result = -1;
							return result;
						}
					}
					dBConn.Close();
				}
			}
			catch
			{
				if (dBConn != null)
				{
					dBConn.Close();
				}
				DBTools.b_up = false;
				int result = -1;
				return result;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.2.1.4") < 0)
						{
							int num15 = AccessDBUpdate.UPMYSQL2V1214(dBConn);
							if (num15 < 0)
							{
								DBTools.b_up = false;
								int result = -1;
								return result;
							}
						}
						if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.2.2.1") < 0)
						{
							int num16 = AccessDBUpdate.UPMYSQL2V1221(dBConn);
							if (num16 < 0)
							{
								DBTools.b_up = false;
								int result = -1;
								return result;
							}
						}
						if (currentDBVersion.Length < 1 || currentDBVersion.CompareTo("1.3.2.5") < 0)
						{
							int num17 = AccessDBUpdate.UPMYSQL2V1325(dBConn);
							if (num17 < 0)
							{
								DBTools.b_up = false;
								int result = -1;
								return result;
							}
						}
						if (dBConn != null)
						{
							dBConn.Close();
						}
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~UPdateDB Error : " + ex.Message + "\n" + ex.StackTrace);
					if (dBConn != null)
					{
						dBConn.Close();
					}
					DBTools.b_up = false;
					int result = -1;
					return result;
				}
			}
			DataTable dataTable = new DataTable();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn != null && dBConn.con != null)
				{
					DbCommand dbCommand = dBConn.con.CreateCommand();
					try
					{
						dbCommand.CommandText = "drop table logrecords ";
						dbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					str_logkey.Replace(";", "','");
					string[] separator = new string[]
					{
						";"
					};
					string[] array = str_logkey.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn.con);
					dbCommand.CommandText = "select eventid from event_info ";
					dataAdapter.SelectCommand = dbCommand;
					dataAdapter.Fill(dataTable);
					dataAdapter.Dispose();
					if (dataTable == null || dataTable.Rows.Count != array.Length)
					{
						dbCommand.CommandText = "delete from event_info ";
						dbCommand.ExecuteNonQuery();
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string str = array2[i];
							try
							{
								dbCommand.CommandText = "insert into event_info (eventid,logflag,mailflag ) values('" + str + "',1,0 ) ";
								dbCommand.ExecuteNonQuery();
							}
							catch
							{
							}
						}
					}
					int result = 1;
					return result;
				}
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Upgrade databae error : " + ex2.Message + "\n" + ex2.StackTrace);
			}
			finally
			{
				if (dBConn != null)
				{
					try
					{
						dBConn.close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		public static bool CheckFreeSpaceSize4ExportDB(string str_path)
		{
			long num = 0L;
			string text = AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb";
			if (!File.Exists(text))
			{
				return false;
			}
			string text2 = AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb";
			if (!File.Exists(text2))
			{
				return false;
			}
			string text3 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
			{
				text3 += Path.DirectorySeparatorChar;
			}
			string text4 = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
			if (text4[text4.Length - 1] != Path.DirectorySeparatorChar)
			{
				text4 += Path.DirectorySeparatorChar;
			}
			List<FileInfo> list = new List<FileInfo>();
			if (Directory.Exists(text4))
			{
				DBTools.DelFile(text4);
			}
			else
			{
				Directory.CreateDirectory(text4);
			}
			bool result;
			try
			{
				FileInfo item = new FileInfo(text);
				list.Add(item);
				item = new FileInfo(text2);
				list.Add(item);
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					string text5 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
					{
						text5 += Path.DirectorySeparatorChar;
					}
					if (File.Exists(text5 + "history.mdb"))
					{
						FileInfo item2 = new FileInfo(text5 + "history.mdb");
						list.Add(item2);
					}
					string mySQLDataPath = DBTools.GetMySQLDataPath(DBUrl.DB_CURRENT_NAME, "127.0.0.1", DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
					if (mySQLDataPath.Length < 1)
					{
						result = false;
						return result;
					}
					if (!Directory.Exists(mySQLDataPath))
					{
						result = false;
						return result;
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(mySQLDataPath);
					FileInfo[] files = directoryInfo.GetFiles();
					if (files.Length != 0)
					{
						FileInfo[] array = files;
						for (int i = 0; i < array.Length; i++)
						{
							FileInfo item3 = array[i];
							list.Add(item3);
						}
					}
				}
				else
				{
					string text6 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text6[text6.Length - 1] != Path.DirectorySeparatorChar)
					{
						text6 += Path.DirectorySeparatorChar;
					}
					if (Directory.Exists(text6))
					{
						DirectoryInfo directoryInfo2 = new DirectoryInfo(text6);
						FileInfo[] files2 = directoryInfo2.GetFiles();
						if (files2.Length != 0)
						{
							FileInfo[] array2 = files2;
							for (int j = 0; j < array2.Length; j++)
							{
								FileInfo fileInfo = array2[j];
								if (fileInfo.Extension.ToLower().Equals(".mdb"))
								{
									list.Add(fileInfo);
								}
							}
						}
					}
				}
				if (list != null && list.Count > 0)
				{
					new List<FileInfo>();
					foreach (FileInfo current in list)
					{
						num += current.Length;
					}
					long num2 = num * 3L;
					long l_fspace = num * 2L;
					long l_tspace = num;
					long l_space = num2;
					int workPath = DBMaintain.GetWorkPath(str_path, l_space, l_tspace, l_fspace);
					if (workPath < 0)
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public static bool ExportDatabase(string str_file)
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string value = string.Concat(version);
			string directoryName = Path.GetDirectoryName(str_file);
			Hashtable hashtable = new Hashtable();
			string text = "";
			string str_ver_type = "";
			long num = 0L;
			if (DBUrl.IsServer)
			{
				str_ver_type = "MASTER";
			}
			else
			{
				str_ver_type = "SINGLE";
			}
			string text2 = AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb";
			if (!File.Exists(text2))
			{
				return false;
			}
			string text3 = AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb";
			if (!File.Exists(text3))
			{
				return false;
			}
			string text4 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text4[text4.Length - 1] != Path.DirectorySeparatorChar)
			{
				text4 += Path.DirectorySeparatorChar;
			}
			string text5 = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
			string aimPath = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
			if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
			{
				text5 += Path.DirectorySeparatorChar;
			}
			List<FileInfo> list = new List<FileInfo>();
			if (Directory.Exists(text5))
			{
				DBTools.DelFile(text5);
			}
			else
			{
				Directory.CreateDirectory(text5);
			}
			try
			{
				if (!DBUrl.SERVERMODE)
				{
					FileInfo item = new FileInfo(text2);
					list.Add(item);
					item = new FileInfo(text3);
					list.Add(item);
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
					{
						string text6 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
						if (text6[text6.Length - 1] != Path.DirectorySeparatorChar)
						{
							text6 += Path.DirectorySeparatorChar;
						}
						if (File.Exists(text6 + "history.mdb"))
						{
							FileInfo item2 = new FileInfo(text6 + "history.mdb");
							list.Add(item2);
						}
						hashtable.Add("DATABASETYPE", "MYSQL");
						string text7 = DBTools.GetMySQLDataPath(DBUrl.DB_CURRENT_NAME, "127.0.0.1", DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
						if (text7.Length < 1)
						{
							bool result = false;
							return result;
						}
						if (!Directory.Exists(text7))
						{
							bool result = false;
							return result;
						}
						string mySQLVersion = DBTools.GetMySQLVersion("127.0.0.1", string.Concat(DBUrl.CURRENT_PORT), DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
						if (text7[text7.Length - 1] != Path.DirectorySeparatorChar)
						{
							text7 += Path.DirectorySeparatorChar;
						}
						try
						{
							text = mySQLVersion;
						}
						catch (Exception)
						{
						}
						DirectoryInfo directoryInfo = new DirectoryInfo(text7);
						FileInfo[] files = directoryInfo.GetFiles();
						if (files.Length != 0)
						{
							FileInfo[] array = files;
							for (int i = 0; i < array.Length; i++)
							{
								FileInfo item3 = array[i];
								list.Add(item3);
							}
						}
					}
					else
					{
						hashtable.Add("DATABASETYPE", "ACCESS");
						string text8 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
						if (text8[text8.Length - 1] != Path.DirectorySeparatorChar)
						{
							text8 += Path.DirectorySeparatorChar;
						}
						if (Directory.Exists(text8))
						{
							DirectoryInfo directoryInfo2 = new DirectoryInfo(text8);
							FileInfo[] files2 = directoryInfo2.GetFiles();
							if (files2.Length != 0)
							{
								FileInfo[] array2 = files2;
								for (int j = 0; j < array2.Length; j++)
								{
									FileInfo fileInfo = array2[j];
									if (fileInfo.Extension.ToLower().Equals(".mdb"))
									{
										list.Add(fileInfo);
									}
								}
							}
						}
					}
				}
				int num2 = 0;
				if (list != null && list.Count > 0)
				{
					List<FileInfo> list2 = new List<FileInfo>();
					long num3 = 0L;
					foreach (FileInfo current in list)
					{
						num += current.Length;
					}
					long num4 = num * 3L;
					long l_fspace = num * 2L;
					num3 = num;
					long l_space = num4;
					num2 = DBMaintain.GetWorkPath(directoryName, l_space, num3, l_fspace);
					if (num2 < 0)
					{
						bool result = false;
						return result;
					}
					if (num2 == 2)
					{
						text5 = directoryName;
						if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
						{
							text5 += Path.DirectorySeparatorChar;
						}
						text5 += "tmpdbexchangefolder";
						if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
						{
							text5 += Path.DirectorySeparatorChar;
						}
						if (Directory.Exists(text5))
						{
							DBTools.DelFile(text5);
						}
						else
						{
							Directory.CreateDirectory(text5);
						}
						try
						{
							DirectorySecurity directorySecurity = new DirectorySecurity();
							directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
							Directory.SetAccessControl(text5, directorySecurity);
						}
						catch
						{
							bool result = false;
							return result;
						}
					}
					DriveInfo currentDrive = DBTools.getCurrentDrive(text5);
					if (currentDrive != null)
					{
						long availableFreeSpace = currentDrive.AvailableFreeSpace;
						if (num2 == 3)
						{
							if (availableFreeSpace <= num3)
							{
								DBTools.DeleteDir(text5);
								bool result = false;
								return result;
							}
						}
						else
						{
							if (availableFreeSpace <= num4)
							{
								DBTools.DeleteDir(text5);
								bool result = false;
								return result;
							}
						}
					}
					foreach (FileInfo current2 in list)
					{
						try
						{
							File.Copy(current2.FullName, text5 + current2.Name, true);
							list2.Add(new FileInfo(text5 + current2.Name));
						}
						catch
						{
							DBTools.DeleteDir(text5);
							bool result = false;
							return result;
						}
					}
					if (num2 == 3)
					{
						text5 = directoryName;
						if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
						{
							text5 += Path.DirectorySeparatorChar;
						}
						text5 += "tmpdbexchangefolder";
						if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
						{
							text5 += Path.DirectorySeparatorChar;
						}
						if (Directory.Exists(text5))
						{
							DBTools.DelFile(text5);
						}
						else
						{
							Directory.CreateDirectory(text5);
						}
						try
						{
							DirectorySecurity directorySecurity2 = new DirectorySecurity();
							directorySecurity2.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
							Directory.SetAccessControl(text5, directorySecurity2);
						}
						catch
						{
							DBTools.DeleteDir(aimPath);
							bool result = false;
							return result;
						}
					}
					using (ZipArchive zipArchive = ZipArchive.CreateZipFile(text5 + "tmpdb.zip"))
					{
						foreach (FileInfo current3 in list2)
						{
							string name = current3.Name;
							try
							{
								using (FileStream fileStream = new FileStream(current3.FullName, FileMode.Open, FileAccess.Read))
								{
									using (Stream stream = zipArchive.AddFile(name).SetStream())
									{
										byte[] array3 = new byte[67108863];
										for (int k = fileStream.Read(array3, 0, array3.Length); k > 0; k = fileStream.Read(array3, 0, array3.Length))
										{
											stream.Write(array3, 0, k);
										}
										stream.Flush();
										stream.Close();
									}
								}
								current3.Delete();
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile("Export database error : " + current3.FullName);
								DebugCenter.GetInstance().appendToFile("Export database error : " + ex.Message + "\n" + ex.StackTrace);
							}
						}
					}
					if (File.Exists(text5 + "tmpdb.zip") && new FileInfo(text5 + "tmpdb.zip").Length > 0L)
					{
						string fileOut = str_file.Substring(0, str_file.LastIndexOf(".") + 1) + "bak";
						hashtable.Add("TOTALSIZE", string.Concat(num));
						string currentDBVersion = DBTools.GetCurrentDBVersion();
						hashtable.Add("SYSDBVERSION", currentDBVersion);
						hashtable.Add("HEADVERSION", "1.0.0");
						hashtable.Add("ECOVERSION", value);
						if (text.Length > 0)
						{
							hashtable.Add("MYSQLVERSION", text);
						}
						byte[] array4 = DBUtil.GenerateHead(hashtable, str_ver_type);
						bool result;
						if (array4 == null || array4.Length < 23)
						{
							DBTools.DeleteDir(text5);
							DBTools.DeleteDir(aimPath);
							result = false;
							return result;
						}
						if (DBUrl.IsServer)
						{
							AESEncryptionUtility.Encrypt(array4, text5 + "tmpdb.zip", fileOut, "retsaMrosneS0cenet^");
						}
						else
						{
							AESEncryptionUtility.Encrypt(array4, text5 + "tmpdb.zip", fileOut, "rosneS0cenet^");
						}
						File.Delete(text5 + "tmpdb.zip");
						DBTools.DeleteDir(text5);
						DBTools.DeleteDir(aimPath);
						result = true;
						return result;
					}
				}
			}
			catch (Exception ex2)
			{
				DebugCenter.GetInstance().appendToFile("Export database error : " + ex2.Message + "\n" + ex2.StackTrace);
				DBTools.DeleteDir(text5);
				DBTools.DeleteDir(aimPath);
				bool result = false;
				return result;
			}
			DBTools.DeleteDir(text5);
			DBTools.DeleteDir(aimPath);
			return false;
		}
		public static bool ImportDatabase(string str_file)
		{
			bool flag = true;
			DBTools.ProgramBar_Percent = 1;
			long num = 0L;
			bool flag2 = true;
			string path = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
			if (str_file.IndexOf(".mdb") > 0 && str_file.IndexOf(",") < 0)
			{
				flag2 = false;
			}
			DBTools.ProgramBar_Type = 5;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder" + Path.DirectorySeparatorChar;
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				if (files.Length != 0)
				{
					FileInfo[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						FileInfo fileInfo = array[i];
						long length = fileInfo.Length;
						num += length;
					}
				}
				bool result;
				if (flag2)
				{
					string[] separator = new string[]
					{
						","
					};
					string[] array2 = str_file.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					string text2 = "";
					string text3 = "";
					string text4 = "";
					string text5 = "";
					string text6 = "";
					int num2 = 0;
					if (array2 != null && array2.Length > 1)
					{
						text2 = array2[0];
						num2 = Convert.ToInt32(array2[1]);
						text3 = array2[2];
						text4 = array2[3];
						text5 = array2[4];
						if (array2.Length > 5)
						{
							text6 = array2[5];
						}
					}
					bool flag3 = false;
					if (DBUrl.DB_CURRENT_NAME.Equals(text5))
					{
						flag = false;
					}
					if (DBUrl.IsServer && text5.Equals("eco"))
					{
						flag3 = true;
					}
					int num3;
					if (!flag3)
					{
						num3 = DBTools.DropMySQLDatabase(text5, text2, num2, text3, text4);
						DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_ImportDatabase_ERROR, false);
						if (num3 < 0)
						{
							result = false;
							return result;
						}
						if (flag)
						{
							DBTools.DropMySQLDatabase(DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
						}
					}
					if (DBUrl.IsServer)
					{
						num3 = DBMaintain.InitMySQLDatabase4Master(ref text5, text2, num2, text3, text4);
					}
					else
					{
						num3 = DBTools.InitMySQLDatabase4Import(text2, num2, text3, text4);
					}
					if (num3 < 0)
					{
						result = false;
						return result;
					}
					string mySQLDataPath = DBTools.GetMySQLDataPath(text5, text2, num2, text3, text4);
					if (mySQLDataPath.Length < 1)
					{
						result = false;
						return result;
					}
					if (File.Exists(text + "history.mdb"))
					{
						string text7 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
						if (text7[text7.Length - 1] != Path.DirectorySeparatorChar)
						{
							text7 += Path.DirectorySeparatorChar;
						}
						try
						{
							File.Copy(text + "history.mdb", text7 + "history.mdb", true);
							File.Delete(text + "history.mdb");
						}
						catch
						{
							result = false;
							return result;
						}
					}
					if (File.Exists(text + "sysdb.mdb"))
					{
						try
						{
							File.Copy(text + "sysdb.mdb", AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb", true);
							File.Delete(text + "sysdb.mdb");
							if (text6.Length > 0)
							{
								int num4 = DBUtil.ChangeDBSetting2MySQL(text5, text2, num2, text3, text4);
								if (num4 < 0)
								{
									result = false;
									return result;
								}
							}
						}
						catch
						{
							result = false;
							return result;
						}
					}
					if (File.Exists(text + "logdb.mdb"))
					{
						try
						{
							File.Copy(text + "logdb.mdb", AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb", true);
							File.Delete(text + "logdb.mdb");
						}
						catch
						{
							result = false;
							return result;
						}
					}
					if (File.Exists(text + "datadb.mdb"))
					{
						try
						{
							File.Delete(text + "datadb.mdb");
						}
						catch
						{
							result = false;
							return result;
						}
					}
					int num5 = DBTools.MoveDir(AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder", mySQLDataPath, num);
					if (num5 < 0)
					{
						result = false;
						return result;
					}
				}
				else
				{
					string text8 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text8[text8.Length - 1] != Path.DirectorySeparatorChar)
					{
						text8 += Path.DirectorySeparatorChar;
					}
					DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_ImportDatabase_ERROR, false);
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
					{
						DBTools.DropMySQLDatabase(DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
					}
					if (Directory.Exists(text8))
					{
						DirectoryInfo directoryInfo2 = new DirectoryInfo(text8);
						FileInfo[] files2 = directoryInfo2.GetFiles();
						if (files2.Length != 0)
						{
							FileInfo[] array3 = files2;
							for (int j = 0; j < array3.Length; j++)
							{
								FileInfo fileInfo2 = array3[j];
								if (fileInfo2.Extension.ToLower().Equals(".mdb"))
								{
									try
									{
										fileInfo2.Delete();
									}
									catch (Exception ex)
									{
										DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~DeleteAllAccessDB Error : " + ex.Message + "\n" + ex.StackTrace);
									}
								}
							}
						}
					}
					else
					{
						Directory.CreateDirectory(text8);
					}
					if (Directory.Exists(text))
					{
						DirectoryInfo directoryInfo3 = new DirectoryInfo(text);
						FileInfo[] files3 = directoryInfo3.GetFiles();
						long num6 = 0L;
						if (files3.Length != 0)
						{
							FileInfo[] array4 = files3;
							for (int k = 0; k < array4.Length; k++)
							{
								FileInfo fileInfo3 = array4[k];
								if (fileInfo3.Extension.ToLower().Equals(".mdb"))
								{
									try
									{
										num6 += fileInfo3.Length;
										if (fileInfo3.Name.Equals("datadb.mdb"))
										{
											File.Copy(fileInfo3.FullName, AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb", true);
										}
										else
										{
											if (fileInfo3.Name.Equals("sysdb.mdb"))
											{
												File.Copy(fileInfo3.FullName, AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb", true);
											}
											else
											{
												if (fileInfo3.Name.Equals("logdb.mdb"))
												{
													File.Copy(fileInfo3.FullName, AppDomain.CurrentDomain.BaseDirectory + "logdb.mdb", true);
												}
												else
												{
													File.Move(fileInfo3.FullName, text8 + fileInfo3.Name);
												}
											}
										}
										DBTools.ProgramBar_Percent = AccessDBUpdate.GetPercent(num6, num);
									}
									catch (Exception ex2)
									{
										DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~ImportDB Error : " + ex2.Message + "\n" + ex2.StackTrace);
										result = false;
										return result;
									}
								}
							}
						}
					}
				}
				DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_Success, false);
				result = true;
				return result;
			}
			catch
			{
			}
			finally
			{
				try
				{
					DBTools.DelFile(path);
					if (Directory.Exists(path))
					{
						Directory.Delete(path);
					}
				}
				catch
				{
				}
			}
			return false;
		}
		public static string CheckDatabase(string str_filepath)
		{
			string text = "";
			DBTools.ProgramBar_Percent = 1;
			DBTools.ProgramBar_Type = 0;
			string text2 = "";
			string text3 = "";
			string text4 = "";
			if (!File.Exists(str_filepath))
			{
				return "";
			}
			string text5 = AppDomain.CurrentDomain.BaseDirectory + "tmpdbexchangefolder";
			string text6 = AppDomain.CurrentDomain.BaseDirectory;
			if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
			{
				text5 += Path.DirectorySeparatorChar;
			}
			if (text6[text6.Length - 1] != Path.DirectorySeparatorChar)
			{
				text6 += Path.DirectorySeparatorChar;
			}
			try
			{
				DirectorySecurity directorySecurity = new DirectorySecurity();
				directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
				Directory.SetAccessControl(text6, directorySecurity);
			}
			catch
			{
			}
			string result;
			if (Directory.Exists(text5))
			{
				try
				{
					DirectorySecurity directorySecurity2 = new DirectorySecurity();
					directorySecurity2.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
					Directory.SetAccessControl(text5, directorySecurity2);
				}
				catch
				{
					result = "";
					return result;
				}
				DBTools.DelFile(text5);
			}
			else
			{
				Directory.CreateDirectory(text5);
				try
				{
					DirectorySecurity directorySecurity3 = new DirectorySecurity();
					directorySecurity3.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
					Directory.SetAccessControl(text5, directorySecurity3);
				}
				catch
				{
					result = "";
					return result;
				}
			}
			DriveInfo currentDrive = DBTools.getCurrentDrive(text5);
			if (currentDrive != null)
			{
				long availableFreeSpace = currentDrive.AvailableFreeSpace;
				FileInfo fileInfo = new FileInfo(str_filepath);
				if (availableFreeSpace < fileInfo.Length * 2L)
				{
					return "UNZIP_ERROR";
				}
			}
			int num = 0;
			long num2 = 0L;
			int i_length = 0;
			try
			{
				Hashtable hashtable = DBUtil.GetDBFileInfo(str_filepath);
				if (hashtable == null || hashtable.Count < 1)
				{
					hashtable = DBUtil.GetDBFileInfo_newversion(str_filepath);
				}
				if (hashtable == null || hashtable.Count < 2)
				{
					num = 0;
				}
				string text7 = "";
				if (hashtable.ContainsKey("ECOTYPE"))
				{
					text7 = (string)hashtable["ECOTYPE"];
					if (text7.Equals("MASTER"))
					{
						if (!DBUrl.IsServer)
						{
							result = "DBVERSION_ERROR";
							return result;
						}
						num = 1;
					}
					else
					{
						if (DBUrl.IsServer)
						{
							num = 4;
						}
						else
						{
							num = 2;
						}
					}
				}
				if (hashtable.ContainsKey("ECOVERSION"))
				{
					text = Convert.ToString(hashtable["ECOVERSION"]);
				}
				if (hashtable.ContainsKey("TOTALSIZE"))
				{
					num2 = Convert.ToInt64(hashtable["TOTALSIZE"]);
				}
				if (hashtable.ContainsKey("INFOLENGTH"))
				{
					i_length = Convert.ToInt32(hashtable["INFOLENGTH"]);
				}
				if (hashtable.ContainsKey("SYSDBVERSION"))
				{
					text3 = Convert.ToString(hashtable["SYSDBVERSION"]);
				}
				if (hashtable.ContainsKey("HEADVERSION"))
				{
					text4 = Convert.ToString(hashtable["HEADVERSION"]);
				}
				if (hashtable.ContainsKey("MYSQLVERSION"))
				{
					text2 = Convert.ToString(hashtable["MYSQLVERSION"]);
				}
				DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
				{
					"Prepare to import database backup file, M or S : (",
					text7,
					"), ecoSensor version : (",
					text,
					"), total size : (",
					num2,
					"), sysdb version : (",
					text3,
					"), MySQL version : (",
					text2,
					") "
				}));
			}
			catch (Exception)
			{
			}
			if (text4.Length > 0 && text4.CompareTo("1.0.0") > 0)
			{
				return "DBVERSION_ERROR";
			}
			if (text3.Length > 0)
			{
				if ("1.4.0.7".CompareTo(text3) < 0)
				{
					return "DBVERSION_ERROR";
				}
				if (num == 4 && text3.CompareTo("1.4.0.3") < 0)
				{
					return "DBVERSION_ERROR";
				}
			}
			FileInfo fileInfo2 = new FileInfo(str_filepath);
			string text8 = text6 + fileInfo2.Name.Substring(0, fileInfo2.Name.LastIndexOf(".") + 1) + "zip";
			if (num > 0)
			{
				DriveInfo currentDrive2 = DBTools.getCurrentDrive(text5);
				if (currentDrive2 != null)
				{
					long availableFreeSpace2 = currentDrive2.AvailableFreeSpace;
					if (availableFreeSpace2 < num2 * 2L)
					{
						File.Delete(text8);
						return "DISKSIZELOW";
					}
				}
			}
			try
			{
				switch (num)
				{
				case 0:
					if (DBUrl.IsServer)
					{
						try
						{
							AESEncryptionUtility.Decrypt(str_filepath, text8, "retsaMrosneS0cenet^");
							DebugCenter.GetInstance().appendToFile("import backup file : non-head,Master");
							goto IL_5E2;
						}
						catch (Exception ex)
						{
							if (ex.GetType().FullName.IndexOf("CryptographicException") > -1)
							{
								try
								{
									File.Delete(text8);
								}
								catch
								{
								}
								result = "";
								return result;
							}
							try
							{
								File.Delete(text8);
							}
							catch
							{
							}
							result = "UNZIP_ERROR";
							return result;
						}
					}
					try
					{
						AESEncryptionUtility.Decrypt(str_filepath, text8, "rosneS0cenet^");
						DebugCenter.GetInstance().appendToFile("import backup file : non-head,Single");
						goto IL_5E2;
					}
					catch
					{
						try
						{
							File.Delete(text8);
						}
						catch
						{
						}
						result = "";
						return result;
					}
					break;
				case 1:
					break;
				case 2:
					goto IL_53F;
				case 3:
					goto IL_5E2;
				case 4:
					goto IL_592;
				default:
					goto IL_5E2;
				}
				try
				{
					AESEncryptionUtility.Decrypt(i_length, str_filepath, text8, "retsaMrosneS0cenet^");
					goto IL_5E2;
				}
				catch (Exception ex2)
				{
					if (ex2.GetType().FullName.IndexOf("CryptographicException") > -1)
					{
						try
						{
							File.Delete(text8);
						}
						catch
						{
						}
						result = "";
						return result;
					}
					result = "UNZIP_ERROR";
					return result;
				}
				try
				{
					IL_53F:
					AESEncryptionUtility.Decrypt(i_length, str_filepath, text8, "rosneS0cenet^");
					goto IL_5E2;
				}
				catch (Exception ex3)
				{
					if (ex3.GetType().FullName.IndexOf("CryptographicException") > -1)
					{
						try
						{
							File.Delete(text8);
						}
						catch
						{
						}
						result = "";
						return result;
					}
					result = "UNZIP_ERROR";
					return result;
				}
				try
				{
					IL_592:
					AESEncryptionUtility.Decrypt(i_length, str_filepath, text8, "rosneS0cenet^");
				}
				catch (Exception ex4)
				{
					if (ex4.GetType().FullName.IndexOf("CryptographicException") > -1)
					{
						try
						{
							File.Delete(text8);
						}
						catch
						{
						}
						result = "";
						return result;
					}
					result = "UNZIP_ERROR";
					return result;
				}
				IL_5E2:
				if (!File.Exists(text8))
				{
					result = "UNZIP_ERROR";
					return result;
				}
				try
				{
					if (num > 0)
					{
						DriveInfo currentDrive3 = DBTools.getCurrentDrive(text5);
						if (currentDrive3 != null)
						{
							long availableFreeSpace3 = currentDrive3.AvailableFreeSpace;
							if (availableFreeSpace3 < num2)
							{
								File.Delete(text8);
								result = "UNZIP_ERROR";
								return result;
							}
						}
					}
					using (ZipArchive zipArchive = ZipArchive.OpenOnFile(text8))
					{
						bool flag = false;
						long l_total = 0L;
						DBTools.ProgramBar_Type = 3;
						long num3 = 0L;
						string text9 = text5;
						if (text9[text9.Length - 1] != Path.DirectorySeparatorChar)
						{
							text9 += Path.DirectorySeparatorChar;
						}
						if (num2 <= 0L)
						{
							flag = true;
							l_total = (long)zipArchive.Files.Count<ZipArchive.ZipFileInfo>();
						}
						foreach (ZipArchive.ZipFileInfo current in zipArchive.Files)
						{
							if (!current.FolderFlag)
							{
								using (Stream stream = current.GetStream())
								{
									string text10 = text9 + Path.GetDirectoryName(current.Name) + Path.DirectorySeparatorChar;
									if (text10[text10.Length - 1] != Path.DirectorySeparatorChar)
									{
										text10 += Path.DirectorySeparatorChar;
									}
									if (!Directory.Exists(text10))
									{
										Directory.CreateDirectory(text10);
									}
									using (FileStream fileStream = new FileStream(text9 + current.Name, FileMode.Create))
									{
										byte[] array = new byte[67108863];
										for (int i = stream.Read(array, 0, array.Length); i > 0; i = stream.Read(array, 0, array.Length))
										{
											fileStream.Write(array, 0, i);
											if (!flag)
											{
												num3 += (long)i;
												int num4 = AccessDBUpdate.GetPercent(num3, num2);
												double value = 0.01 * (double)num4 * 60.0;
												num4 = 40 + Convert.ToInt32(value);
												if (num4 > 100)
												{
													num4 = 100;
												}
												if (num4 < 40)
												{
													num4 = 40;
												}
												DBTools.ProgramBar_Percent = num4;
											}
										}
										fileStream.Flush();
									}
								}
								if (flag)
								{
									num3 += 1L;
									int num5 = AccessDBUpdate.GetPercent(num3, l_total);
									double value2 = 0.01 * (double)num5 * 60.0;
									num5 = 40 + Convert.ToInt32(value2);
									if (num5 > 100)
									{
										num5 = 100;
									}
									if (num5 < 40)
									{
										num5 = 40;
									}
									DBTools.ProgramBar_Percent = num5;
								}
							}
						}
						DBTools.ProgramBar_Percent = 100;
					}
					File.Delete(text8);
				}
				catch
				{
					File.Delete(text8);
					result = "UNZIP_ERROR";
					return result;
				}
			}
			catch
			{
				File.Delete(text8);
				result = "";
				return result;
			}
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = new OleDbCommand();
			string text11 = "";
			string text12 = "";
			string text13 = "";
			string text14 = "";
			string text15 = "";
			int num6 = 0;
			try
			{
				string connectionString = string.Concat(new object[]
				{
					"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=",
					text5,
					Path.DirectorySeparatorChar,
					"sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor"
				});
				oleDbConnection = new OleDbConnection(connectionString);
				oleDbConnection.Open();
				oleDbCommand = new OleDbCommand();
				oleDbCommand.Connection = oleDbConnection;
				oleDbCommand.CommandText = "select * from dbsource where active_flag = 2 ";
				DbDataReader dbDataReader = oleDbCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					text13 = Convert.ToString(dbDataReader.GetValue(2));
					text12 = Convert.ToString(dbDataReader.GetValue(3));
					text11 = Convert.ToString(dbDataReader.GetValue(4));
					num6 = Convert.ToInt32(dbDataReader.GetValue(5));
					text14 = Convert.ToString(dbDataReader.GetValue(6));
					text15 = Convert.ToString(dbDataReader.GetValue(7));
				}
				dbDataReader.Close();
				dbDataReader.Dispose();
				try
				{
					oleDbCommand.CommandText = "update device_base_info set restoreflag = 1 ";
					oleDbCommand.ExecuteNonQuery();
				}
				catch (Exception)
				{
					try
					{
						string text16 = AppDomain.CurrentDomain.BaseDirectory;
						if (text16[text16.Length - 1] != Path.DirectorySeparatorChar)
						{
							text16 += Path.DirectorySeparatorChar;
						}
						File.AppendAllText(text16 + "restore.flag", "older version database", Encoding.ASCII);
					}
					catch
					{
					}
				}
				if (text11.Length > 1)
				{
					if (text13.ToUpper().Equals("MYSQL"))
					{
						try
						{
							int num7 = DBTools.CheckMySQLParae(text11, num6, text14, text15);
							if (num7 < 0)
							{
								if (text2.Length > 0)
								{
									result = string.Concat(new object[]
									{
										"MYSQL_CONNECT_ERROR,",
										text11,
										",",
										num6,
										",",
										text14,
										",",
										text15,
										",",
										text12,
										",",
										text2
									});
									return result;
								}
								result = string.Concat(new object[]
								{
									"MYSQL_CONNECT_ERROR,",
									text11,
									",",
									num6,
									",",
									text14,
									",",
									text15,
									",",
									text12
								});
								return result;
							}
							else
							{
								string text17 = DBTools.GetMySQLVersion(text11, string.Concat(num6), text14, text15);
								text17 = DBMaintain.EditVersionString(text17);
								if (text5[text5.Length - 1] != Path.DirectorySeparatorChar)
								{
									text5 += Path.DirectorySeparatorChar;
								}
								if (File.Exists(text5 + "mysql_version_info.msg"))
								{
									string text18 = "";
									using (StreamReader streamReader = new StreamReader(text5 + "mysql_version_info.msg"))
									{
										text18 = streamReader.ReadToEnd();
									}
									text18 = DBMaintain.EditVersionString(text18);
									if (text18 == null || text18.Length < 1 || !DBMaintain.CompareMySQLVersion(text17, text18))
									{
										if (text18 == null || text18.Length < 1)
										{
											result = "MYSQLVERSIONERROR;5.6";
											return result;
										}
										result = "MYSQLVERSIONERROR;" + text18;
										return result;
									}
									else
									{
										try
										{
											File.Delete(text5 + "mysql_version_info.msg");
											goto IL_C8D;
										}
										catch
										{
											goto IL_C8D;
										}
									}
								}
								if (text2.Length > 0)
								{
									text2 = DBMaintain.EditVersionString(text2);
									if (!DBMaintain.CompareMySQLVersion(text17, text2))
									{
										result = "MYSQLVERSIONERROR;" + text2;
										return result;
									}
								}
								else
								{
									if (text17 == null || text17.Length < 1 || text17.CompareTo("5.6") < 0)
									{
										result = "MYSQLVERSIONERROR;5.6";
										return result;
									}
								}
								IL_C8D:;
							}
						}
						catch (Exception ex5)
						{
							DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex5.Message + "\n" + ex5.StackTrace);
							result = "";
							return result;
						}
						result = string.Concat(new object[]
						{
							text11,
							",",
							num6,
							",",
							text14,
							",",
							text15,
							",",
							text12
						});
					}
					else
					{
						result = text11;
					}
				}
				else
				{
					result = "";
				}
			}
			catch (Exception)
			{
				result = "";
			}
			finally
			{
				oleDbCommand.Dispose();
				try
				{
					DBUtil.SetSysDBSerial(oleDbConnection);
				}
				catch
				{
				}
				if (oleDbConnection != null)
				{
					oleDbConnection.Close();
				}
			}
			return result;
		}
		public static List<BillReportInfo> GetBillReportInfo(string str_Start, string str_End, string device_id, string portid)
		{
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			List<BillReportInfo> list = new List<BillReportInfo>();
			int num = 0;
			int monthHour = DBTools.GetMonthHour(str_Start);
			if (monthHour < 0)
			{
				return list;
			}
			long num2 = 0L;
			long num3 = 0L;
			bool flag = false;
			if (portid != null && portid.Length > 0)
			{
				flag = true;
			}
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				int queryThreads = MultiThreadQuery.getQueryThreads();
				if (queryThreads > 0)
				{
					list = MultiThreadQuery.GetBillReportInfo(queryThreads, str_Start, str_End, device_id, portid);
					MultiThreadQuery.WriteLog("GetBillReportInfo(multi-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
					return list;
				}
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						if (Sys_Para.GetBill_ratetype() == 1)
						{
							string commandText = "";
							string text = Sys_Para.GetBill_2from1();
							text = text.Substring(0, 2);
							int bill_2duration = Sys_Para.GetBill_2duration1();
							num = DBTools.GetPeakMonthHour(str_Start, bill_2duration);
							string text2 = DBTools.gethourstring(text, bill_2duration);
							string commandText2;
							if (flag)
							{
								commandText2 = string.Concat(new string[]
								{
									"select sum(power_consumption)as kwh from port_data_hourly where port_id in (",
									portid,
									") and  insert_time > '",
									str_Start,
									"' and insert_time < '",
									str_End,
									"' and date_format(insert_time, '%H') in ",
									text2
								});
								commandText = string.Concat(new string[]
								{
									"select sum(power_consumption)as kwh from port_data_hourly where port_id in (",
									portid,
									") and  insert_time > '",
									str_Start,
									"' and insert_time < '",
									str_End,
									"' and date_format(insert_time, '%H') not in ",
									text2
								});
							}
							else
							{
								commandText2 = string.Concat(new string[]
								{
									"select sum(power_consumption)as kwh from device_data_hourly where device_id in (",
									device_id,
									") and  insert_time > '",
									str_Start,
									"' and insert_time < '",
									str_End,
									"' and  date_format(insert_time, '%H') in ",
									text2
								});
								commandText = string.Concat(new string[]
								{
									"select sum(power_consumption)as kwh from device_data_hourly where device_id in (",
									device_id,
									") and  insert_time > '",
									str_Start,
									"' and insert_time < '",
									str_End,
									"' and   date_format(insert_time, '%H')  not in ",
									text2
								});
							}
							try
							{
								dbCommand.CommandText = commandText2;
								object obj = dbCommand.ExecuteScalar();
								if (obj != null && obj != DBNull.Value)
								{
									num2 = Convert.ToInt64(obj);
								}
							}
							catch
							{
							}
							try
							{
								dbCommand.CommandText = commandText;
								object obj2 = dbCommand.ExecuteScalar();
								if (obj2 != null && obj2 != DBNull.Value)
								{
									num3 = Convert.ToInt64(obj2);
								}
							}
							catch
							{
							}
							BillReportInfo item = new BillReportInfo(num2, num, "PEAK");
							list.Add(item);
							BillReportInfo item2 = new BillReportInfo(num3, monthHour - num, "NON_PEAK");
							list.Add(item2);
						}
						else
						{
							if (flag)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"select sum(power_consumption)as kwh from port_data_hourly where port_id in (",
									portid,
									") and  insert_time > '",
									str_Start,
									"' and insert_time < '",
									str_End,
									"'  "
								});
							}
							else
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"select sum(power_consumption)as kwh from device_data_hourly where device_id in (",
									device_id,
									") and  insert_time > '",
									str_Start,
									"' and insert_time < '",
									str_End,
									"'  "
								});
							}
							try
							{
								object obj3 = dbCommand.ExecuteScalar();
								if (obj3 != null && obj3 != DBNull.Value)
								{
									num2 = Convert.ToInt64(obj3);
								}
							}
							catch
							{
							}
							BillReportInfo item3 = new BillReportInfo(num2, monthHour, "ALL");
							list.Add(item3);
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
						if (dBConn != null)
						{
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
					goto IL_806;
				}
				catch (Exception)
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
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					goto IL_806;
				}
			}
			DateTime dateTime;
			DateTime value;
			try
			{
				dateTime = Convert.ToDateTime(str_Start);
				value = Convert.ToDateTime(str_End);
			}
			catch
			{
				List<BillReportInfo> result = list;
				return result;
			}
			string commandText3 = "";
			string text3 = Sys_Para.GetBill_2from1();
			text3 = text3.Substring(0, 2);
			int bill_2duration2 = Sys_Para.GetBill_2duration1();
			num = DBTools.GetPeakMonthHour(str_Start, bill_2duration2);
			string str = DBTools.gethourstring(text3, bill_2duration2);
			int i = 0;
			while (i < 32)
			{
				DateTime dt_inserttime = dateTime.AddDays((double)i);
				i++;
				if (dt_inserttime.CompareTo(value) >= 0)
				{
					break;
				}
				DBConn dBConn2 = null;
				DbCommand dbCommand2 = null;
				try
				{
					dBConn2 = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn2 != null && dBConn2.con != null)
					{
						dbCommand2 = dBConn2.con.CreateCommand();
						if (Sys_Para.GetBill_ratetype() == 1)
						{
							if (flag)
							{
								string[] separator = new string[]
								{
									","
								};
								string[] array = portid.Split(separator, StringSplitOptions.RemoveEmptyEntries);
								if (array != null)
								{
									string[] array2 = array;
									for (int j = 0; j < array2.Length; j++)
									{
										string str2 = array2[j];
										string commandText4 = "select sum(power_consumption)as kwh from port_data_hourly where port_id = " + str2 + " and FORMAT(insert_time, 'HH') in " + str;
										commandText3 = "select sum(power_consumption)as kwh from port_data_hourly where port_id = " + str2 + " and FORMAT(insert_time, 'HH') not in " + str;
										try
										{
											dbCommand2.CommandText = commandText4;
											object obj4 = dbCommand2.ExecuteScalar();
											if (obj4 != null && obj4 != DBNull.Value)
											{
												num2 += Convert.ToInt64(obj4);
											}
										}
										catch
										{
										}
										try
										{
											dbCommand2.CommandText = commandText3;
											object obj5 = dbCommand2.ExecuteScalar();
											if (obj5 != null && obj5 != DBNull.Value)
											{
												num3 += Convert.ToInt64(obj5);
											}
										}
										catch
										{
										}
									}
								}
							}
							else
							{
								string[] separator2 = new string[]
								{
									","
								};
								string[] array3 = device_id.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
								if (array3 != null)
								{
									string[] array2 = array3;
									for (int j = 0; j < array2.Length; j++)
									{
										string str3 = array2[j];
										string commandText4 = "select sum(power_consumption)as kwh from device_data_hourly where device_id = " + str3 + " and FORMAT(insert_time, 'HH') in " + str;
										commandText3 = "select sum(power_consumption)as kwh from device_data_hourly where device_id = " + str3 + " and FORMAT(insert_time, 'HH') not in " + str;
										try
										{
											dbCommand2.CommandText = commandText4;
											object obj6 = dbCommand2.ExecuteScalar();
											if (obj6 != null && obj6 != DBNull.Value)
											{
												num2 += Convert.ToInt64(obj6);
											}
										}
										catch
										{
										}
										try
										{
											dbCommand2.CommandText = commandText3;
											object obj7 = dbCommand2.ExecuteScalar();
											if (obj7 != null && obj7 != DBNull.Value)
											{
												num3 += Convert.ToInt64(obj7);
											}
										}
										catch
										{
										}
									}
								}
							}
						}
						else
						{
							if (flag)
							{
								string[] separator3 = new string[]
								{
									","
								};
								string[] array4 = portid.Split(separator3, StringSplitOptions.RemoveEmptyEntries);
								if (array4 != null)
								{
									string[] array2 = array4;
									for (int j = 0; j < array2.Length; j++)
									{
										string str4 = array2[j];
										dbCommand2.CommandText = "select sum(power_consumption)as kwh from port_data_hourly where port_id = " + str4;
										try
										{
											object obj8 = dbCommand2.ExecuteScalar();
											if (obj8 != null && obj8 != DBNull.Value)
											{
												num2 += Convert.ToInt64(obj8);
											}
										}
										catch
										{
										}
									}
								}
							}
							else
							{
								string[] separator4 = new string[]
								{
									","
								};
								string[] array5 = device_id.Split(separator4, StringSplitOptions.RemoveEmptyEntries);
								if (array5 != null)
								{
									string[] array2 = array5;
									for (int j = 0; j < array2.Length; j++)
									{
										string str5 = array2[j];
										dbCommand2.CommandText = "select sum(power_consumption)as kwh from device_data_hourly where device_id =" + str5;
										try
										{
											object obj9 = dbCommand2.ExecuteScalar();
											if (obj9 != null && obj9 != DBNull.Value)
											{
												num2 += Convert.ToInt64(obj9);
											}
										}
										catch
										{
										}
									}
								}
							}
						}
						if (dbCommand2 != null)
						{
							try
							{
								dbCommand2.Dispose();
							}
							catch
							{
							}
						}
						if (dBConn2 != null)
						{
							try
							{
								dBConn2.Close();
							}
							catch
							{
							}
						}
					}
				}
				catch (Exception)
				{
					if (dbCommand2 != null)
					{
						try
						{
							dbCommand2.Dispose();
						}
						catch
						{
						}
					}
					if (dBConn2 != null)
					{
						try
						{
							dBConn2.Close();
						}
						catch
						{
						}
					}
				}
			}
			if (Sys_Para.GetBill_ratetype() == 1)
			{
				BillReportInfo item4 = new BillReportInfo(num2, num, "PEAK");
				list.Add(item4);
				BillReportInfo item5 = new BillReportInfo(num3, monthHour - num, "NON_PEAK");
				list.Add(item5);
				goto IL_806;
			}
			BillReportInfo item6 = new BillReportInfo(num2, monthHour, "ALL");
			list.Add(item6);
			IL_806:
			MultiThreadQuery.WriteLog("GetBillReportInfo(single-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			return list;
		}
		public static Dictionary<long, string> GetRackDeviceMapByGroupID(long l_gid, string str_type)
		{
			Dictionary<long, string> dictionary = new Dictionary<long, string>();
			bool flag = true;
			if (str_type.Equals("zone") || str_type.Equals("rack") || str_type.Equals("dev") || str_type.Equals("outlet"))
			{
				flag = false;
			}
			try
			{
				Hashtable groupDestIDMap = DBCache.GetGroupDestIDMap();
				if (!flag && (groupDestIDMap == null || groupDestIDMap.Count < 1 || !groupDestIDMap.ContainsKey(l_gid)))
				{
					Dictionary<long, string> result = dictionary;
					return result;
				}
				if (str_type.Equals("zone"))
				{
					List<long> list = (List<long>)groupDestIDMap[l_gid];
					if (list != null)
					{
						Hashtable zoneCache = DBCache.GetZoneCache();
						if (zoneCache != null && zoneCache.Count > 0)
						{
							foreach (long current in list)
							{
								if (zoneCache.ContainsKey(current))
								{
									ZoneInfo zoneInfo = (ZoneInfo)zoneCache[current];
									string rackInfo = zoneInfo.RackInfo;
									if (rackInfo != null && rackInfo.Length > 0)
									{
										Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
										string[] separator = new string[]
										{
											","
										};
										string[] array = rackInfo.Split(separator, StringSplitOptions.RemoveEmptyEntries);
										string[] array2 = array;
										for (int i = 0; i < array2.Length; i++)
										{
											string value = array2[i];
											long num = Convert.ToInt64(value);
											if (rackDeviceMap != null && rackDeviceMap.ContainsKey(Convert.ToInt32(num)))
											{
												List<int> list2 = (List<int>)rackDeviceMap[Convert.ToInt32(num)];
												string text = "";
												foreach (int current2 in list2)
												{
													if (text.Length > 0)
													{
														text = text + "," + current2;
													}
													else
													{
														text += current2;
													}
												}
												if (text.Length > 0)
												{
													dictionary.Add(num, text);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				if (str_type.Equals("rack"))
				{
					List<long> list3 = (List<long>)groupDestIDMap[l_gid];
					if (list3 != null && list3.Count > 0)
					{
						Hashtable rackDeviceMap2 = DBCache.GetRackDeviceMap();
						if (rackDeviceMap2 != null && rackDeviceMap2.Count > 0)
						{
							using (List<long>.Enumerator enumerator = list3.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									int num2 = (int)enumerator.Current;
									if (rackDeviceMap2.ContainsKey(Convert.ToInt32(num2)))
									{
										List<int> list4 = (List<int>)rackDeviceMap2[Convert.ToInt32(num2)];
										string text2 = "";
										foreach (int current3 in list4)
										{
											if (text2.Length > 0)
											{
												text2 = text2 + "," + current3;
											}
											else
											{
												text2 += current3;
											}
										}
										if (text2.Length > 0)
										{
											dictionary.Add((long)num2, text2);
										}
									}
								}
							}
						}
					}
				}
				if (str_type.Equals("dev"))
				{
					List<long> list5 = (List<long>)groupDestIDMap[l_gid];
					if (list5 != null && list5.Count > 0)
					{
						Hashtable deviceCache = DBCache.GetDeviceCache();
						if (deviceCache != null && deviceCache.Count > 0)
						{
							foreach (long current4 in list5)
							{
								if (deviceCache.ContainsKey(Convert.ToInt32(current4)))
								{
									DeviceInfo deviceInfo = (DeviceInfo)deviceCache[Convert.ToInt32(current4)];
									long num3 = (long)deviceInfo.DeviceID;
									long rackID = deviceInfo.RackID;
									if (dictionary.ContainsKey(rackID))
									{
										string text3 = dictionary[rackID];
										if (text3.Length > 0)
										{
											dictionary[rackID] = text3 + "," + num3;
										}
										else
										{
											dictionary[rackID] = text3 + num3;
										}
									}
									else
									{
										dictionary.Add(rackID, string.Concat(num3));
									}
								}
							}
						}
					}
				}
				if (str_type.Equals("outlet"))
				{
					List<long> list6 = (List<long>)groupDestIDMap[l_gid];
					if (list6 != null && list6.Count > 0)
					{
						Hashtable portCache = DBCache.GetPortCache();
						Hashtable deviceCache2 = DBCache.GetDeviceCache();
						if (portCache != null && portCache.Count > 0 && deviceCache2 != null && deviceCache2.Count > 0)
						{
							foreach (long current5 in list6)
							{
								if (portCache.ContainsKey(Convert.ToInt32(current5)))
								{
									PortInfo portInfo = (PortInfo)portCache[Convert.ToInt32(current5)];
									int deviceID = portInfo.DeviceID;
									if (deviceCache2.ContainsKey(deviceID))
									{
										DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache2[deviceID];
										long rackID2 = deviceInfo2.RackID;
										if (dictionary.ContainsKey(rackID2))
										{
											string text4 = dictionary[rackID2];
											if (text4.Length > 0)
											{
												dictionary[rackID2] = text4 + "," + current5;
											}
											else
											{
												dictionary[rackID2] = text4 + current5;
											}
										}
										else
										{
											dictionary.Add(rackID2, string.Concat(current5));
										}
									}
								}
							}
						}
					}
				}
				if (str_type.Equals("alloutlet"))
				{
					Hashtable devicePortMap = DBCache.GetDevicePortMap();
					Hashtable deviceCache3 = DBCache.GetDeviceCache();
					if (devicePortMap != null && devicePortMap.Count > 0 && deviceCache3 != null && deviceCache3.Count > 0)
					{
						ICollection keys = devicePortMap.Keys;
						foreach (int num4 in keys)
						{
							if (deviceCache3.ContainsKey(num4))
							{
								DeviceInfo deviceInfo3 = (DeviceInfo)deviceCache3[num4];
								long rackID3 = deviceInfo3.RackID;
								List<int> list7 = (List<int>)devicePortMap[num4];
								string text5 = "";
								foreach (int current6 in list7)
								{
									if (text5.Length > 0)
									{
										text5 = text5 + "," + current6;
									}
									else
									{
										text5 += current6;
									}
								}
								if (text5.Length > 0)
								{
									if (dictionary.ContainsKey(rackID3))
									{
										string text6 = dictionary[rackID3];
										if (text6.Length > 0)
										{
											dictionary[rackID3] = text6 + "," + text5;
										}
										else
										{
											dictionary[rackID3] = text6 + text5;
										}
									}
									else
									{
										dictionary.Add(rackID3, text5 ?? "");
									}
								}
							}
						}
					}
				}
				if (str_type.Equals("alldev"))
				{
					Hashtable deviceCache4 = DBCache.GetDeviceCache();
					if (deviceCache4 != null && deviceCache4.Count > 0)
					{
						ICollection values = deviceCache4.Values;
						foreach (DeviceInfo deviceInfo4 in values)
						{
							long num5 = (long)deviceInfo4.DeviceID;
							long rackID4 = deviceInfo4.RackID;
							if (dictionary.ContainsKey(rackID4))
							{
								string text7 = dictionary[rackID4];
								if (text7.Length > 0)
								{
									dictionary[rackID4] = text7 + "," + num5;
								}
								else
								{
									dictionary[rackID4] = text7 + num5;
								}
							}
							else
							{
								dictionary.Add(rackID4, string.Concat(num5));
							}
						}
					}
				}
				if (str_type.Equals("allrack"))
				{
					Hashtable rackDeviceMap3 = DBCache.GetRackDeviceMap();
					if (rackDeviceMap3 != null && rackDeviceMap3.Count > 0)
					{
						ICollection keys2 = rackDeviceMap3.Keys;
						foreach (int num6 in keys2)
						{
							List<int> list8 = (List<int>)rackDeviceMap3[num6];
							string text8 = "";
							foreach (int current7 in list8)
							{
								if (text8.Length > 0)
								{
									text8 = text8 + "," + current7;
								}
								else
								{
									text8 += current7;
								}
							}
							if (text8.Length > 0)
							{
								dictionary.Add((long)num6, text8);
							}
						}
					}
				}
				if (dictionary != null && dictionary.Count > 0)
				{
					Dictionary<long, string> result = dictionary;
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetRackDeviceMapByGroupID Error : " + ex.Message + "\n" + ex.StackTrace);
			}
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			DbDataReader dbDataReader = null;
			Dictionary<long, string> dictionary2 = new Dictionary<long, string>();
			Dictionary<long, string> dictionary3 = new Dictionary<long, string>();
			Dictionary<long, string> dictionary4 = new Dictionary<long, string>();
			Dictionary<long, string> dictionary5 = new Dictionary<long, string>();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandText = "select group_id,grouptype,dest_id from group_detail where group_id =" + l_gid + " order by dest_id asc";
					dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						long num7 = Convert.ToInt64(dbDataReader.GetValue(2));
						if (str_type != null)
						{
							if (!(str_type == "zone"))
							{
								if (!(str_type == "rack"))
								{
									if (!(str_type == "dev"))
									{
										if (str_type == "outlet")
										{
											dictionary5.Add(num7, string.Concat(num7));
										}
									}
									else
									{
										dictionary4.Add(num7, string.Concat(num7));
									}
								}
								else
								{
									dictionary3.Add(num7, string.Concat(num7));
								}
							}
							else
							{
								dictionary2.Add(num7, string.Concat(num7));
							}
						}
					}
					dbDataReader.Close();
					if (str_type != null)
					{
						if (<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x6000293-1 == null)
						{
							<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x6000293-1 = new Dictionary<string, int>(7)
							{

								{
									"alloutlet",
									0
								},

								{
									"alldev",
									1
								},

								{
									"allrack",
									2
								},

								{
									"zone",
									3
								},

								{
									"rack",
									4
								},

								{
									"dev",
									5
								},

								{
									"outlet",
									6
								}
							};
						}
						int num8;
						if (<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x6000293-1.TryGetValue(str_type, out num8))
						{
							switch (num8)
							{
							case 0:
								dbCommand.CommandText = "select p.id as portid,d.rack_id as rackid from port_info p inner join device_base_info d on p.device_id = d.id ";
								dbDataReader = dbCommand.ExecuteReader();
								while (dbDataReader.Read())
								{
									long num9 = Convert.ToInt64(dbDataReader.GetValue(0));
									long key = Convert.ToInt64(dbDataReader.GetValue(1));
									if (dictionary.ContainsKey(key))
									{
										string text9 = dictionary[key];
										if (text9.Length > 0)
										{
											dictionary[key] = text9 + "," + num9;
										}
										else
										{
											dictionary[key] = text9 + num9;
										}
									}
									else
									{
										dictionary.Add(key, string.Concat(num9));
									}
								}
								dbDataReader.Close();
								goto IL_1070;
							case 1:
								dbCommand.CommandText = "select id,rack_id from device_base_info ";
								dbDataReader = dbCommand.ExecuteReader();
								while (dbDataReader.Read())
								{
									long num10 = Convert.ToInt64(dbDataReader.GetValue(0));
									long key2 = Convert.ToInt64(dbDataReader.GetValue(1));
									if (dictionary.ContainsKey(key2))
									{
										string text10 = dictionary[key2];
										if (text10.Length > 0)
										{
											dictionary[key2] = text10 + "," + num10;
										}
										else
										{
											dictionary[key2] = text10 + num10;
										}
									}
									else
									{
										dictionary.Add(key2, string.Concat(num10));
									}
								}
								dbDataReader.Close();
								goto IL_1070;
							case 2:
								dbCommand.CommandText = "select id from device_addr_info ";
								dbDataReader = dbCommand.ExecuteReader();
								while (dbDataReader.Read())
								{
									long num11 = Convert.ToInt64(dbDataReader.GetValue(0));
									dictionary3.Add(num11, string.Concat(num11));
								}
								dbDataReader.Close();
								goto IL_1070;
							case 3:
								using (Dictionary<long, string>.Enumerator enumerator4 = dictionary2.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										KeyValuePair<long, string> current8 = enumerator4.Current;
										long key3 = current8.Key;
										dbCommand.CommandText = "select racks from zone_info where id = " + key3;
										string text11 = "";
										try
										{
											object value2 = dbCommand.ExecuteScalar();
											text11 = Convert.ToString(value2);
										}
										catch (Exception ex2)
										{
											DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
										}
										if (text11.Length > 0)
										{
											string[] separator2 = new string[]
											{
												","
											};
											string[] array3 = text11.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
											string[] array2 = array3;
											for (int i = 0; i < array2.Length; i++)
											{
												string value3 = array2[i];
												long key4 = Convert.ToInt64(value3);
												if (!dictionary3.ContainsKey(key4))
												{
													dictionary3.Add(key4, "");
												}
											}
										}
									}
									goto IL_1070;
								}
								break;
							case 4:
								goto IL_1070;
							case 5:
								break;
							case 6:
								goto IL_F79;
							default:
								goto IL_1070;
							}
							using (Dictionary<long, string>.Enumerator enumerator4 = dictionary4.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									KeyValuePair<long, string> current9 = enumerator4.Current;
									long key5 = current9.Key;
									dbCommand.CommandText = "select rack_id from device_base_info where id =" + key5;
									long key6 = 0L;
									try
									{
										object value4 = dbCommand.ExecuteScalar();
										key6 = Convert.ToInt64(value4);
									}
									catch (Exception ex3)
									{
										DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex3.Message + "\n" + ex3.StackTrace);
									}
									if (dictionary.ContainsKey(key6))
									{
										string text12 = dictionary[key6];
										if (text12.Length > 0)
										{
											dictionary[key6] = text12 + "," + key5;
										}
										else
										{
											dictionary[key6] = text12 + key5;
										}
									}
									else
									{
										dictionary.Add(key6, string.Concat(key5));
									}
								}
								goto IL_1070;
							}
							IL_F79:
							string str_sql = "select p.id as portid,d.rack_id as rackid from port_info p inner join device_base_info d on p.device_id = d.id ";
							DataTable dataTable = new DataTable();
							dataTable = DBTools.CreateDataTable4SysDB(str_sql);
							foreach (KeyValuePair<long, string> current10 in dictionary5)
							{
								long key7 = current10.Key;
								DataRow[] array4 = dataTable.Select(" portid = " + key7);
								if (array4 != null && array4.Length > 0)
								{
									long key8 = Convert.ToInt64(array4[0]["rackid"]);
									if (dictionary.ContainsKey(key8))
									{
										string text13 = dictionary[key8];
										if (text13.Length > 0)
										{
											dictionary[key8] = text13 + "," + key7;
										}
										else
										{
											dictionary[key8] = text13 + key7;
										}
									}
									else
									{
										dictionary.Add(key8, string.Concat(key7));
									}
								}
							}
						}
					}
					IL_1070:
					if (dictionary.Count < 1 && dictionary3.Count > 0)
					{
						string str_sql2 = "select id,rack_id from device_base_info ";
						DataTable dataTable2 = new DataTable();
						dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
						DataRow[] array5 = dataTable2.Select();
						DataRow[] array6 = array5;
						for (int i = 0; i < array6.Length; i++)
						{
							DataRow dataRow = array6[i];
							long key9 = Convert.ToInt64(dataRow["rack_id"]);
							long num12 = Convert.ToInt64(dataRow["id"]);
							if (dictionary3.ContainsKey(key9))
							{
								if (dictionary.ContainsKey(key9))
								{
									string text14 = dictionary[key9];
									if (text14.Length > 0)
									{
										dictionary[key9] = text14 + "," + num12;
									}
									else
									{
										dictionary[key9] = text14 + num12;
									}
								}
								else
								{
									dictionary.Add(key9, string.Concat(num12));
								}
							}
						}
					}
				}
			}
			catch (Exception ex4)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex4.Message + "\n" + ex4.StackTrace);
			}
			finally
			{
				try
				{
					if (dbDataReader != null)
					{
						dbDataReader.Close();
					}
				}
				catch
				{
				}
				try
				{
					if (dbCommand != null)
					{
						dbCommand.Dispose();
					}
				}
				catch
				{
				}
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return dictionary;
		}
		public static DataTable GetChart1Data(string str_Start, string str_End, string device_id, string portid, string groupby, string strgrouptype, string dblibnameDev, string dblibnamePort)
		{
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			if (!DBUrl.SERVERMODE && !DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				DataTable pDGroupByTimeType = DBTools.getPDGroupByTimeType(str_Start, str_End, device_id, portid, groupby, strgrouptype);
				MultiThreadQuery.WriteLog("GetChart1Data(single-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
				return pDGroupByTimeType;
			}
			int queryThreads = MultiThreadQuery.getQueryThreads();
			if (queryThreads > 0)
			{
				DataTable chart1Data = MultiThreadQuery.GetChart1Data(str_Start, str_End, device_id, portid, groupby, strgrouptype, dblibnameDev, dblibnamePort);
				MultiThreadQuery.WriteLog("GetChart1Data(multi-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
				return chart1Data;
			}
			DateTime now = DateTime.Now;
			DataTable dataTable = new DataTable();
			ArrayList arrayList = new ArrayList();
			if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
			{
				string str_sqlformat = string.Concat(new string[]
				{
					"select sum(power_consumption) as power_consumption,",
					groupby,
					" as period from ",
					dblibnameDev,
					" a where device_id in ( {0} ) and insert_time >= #",
					str_Start,
					"# and insert_time  <#",
					str_End,
					"# group by ",
					groupby,
					" order by ",
					groupby,
					" ASC"
				});
				arrayList = DBTools.CreateDataTable_Array(str_sqlformat, device_id);
			}
			else
			{
				if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
				{
					string str_sqlformat = string.Concat(new string[]
					{
						"select sum(power_consumption) as power_consumption,",
						groupby,
						" as period from ",
						dblibnamePort,
						" a where port_id in ( {0}) and insert_time >= #",
						str_Start,
						"# and insert_time  <#",
						str_End,
						"# group by ",
						groupby,
						" order by ",
						groupby,
						" ASC"
					});
					arrayList = DBTools.CreateDataTable_Array(str_sqlformat, portid);
				}
			}
			if (arrayList.Count == 1)
			{
				dataTable = (DataTable)arrayList[0];
			}
			else
			{
				if (arrayList.Count > 1)
				{
					Dictionary<string, double> dictionary = new Dictionary<string, double>();
					dataTable = (DataTable)arrayList[0];
					foreach (DataRow dataRow in dataTable.Rows)
					{
						dictionary.Add(Convert.ToString(dataRow["period"]), Convert.ToDouble(dataRow["power_consumption"]));
					}
					for (int i = 1; i < arrayList.Count; i++)
					{
						DataTable dataTable2 = (DataTable)arrayList[i];
						foreach (DataRow dataRow2 in dataTable2.Rows)
						{
							string key = Convert.ToString(dataRow2["period"]);
							double num = Convert.ToDouble(dataRow2["power_consumption"]);
							if (dictionary.ContainsKey(key))
							{
								num += dictionary[key];
								dictionary[key] = num;
							}
							else
							{
								dictionary.Add(key, num);
							}
						}
					}
					dataTable.Rows.Clear();
					foreach (string current in dictionary.Keys)
					{
						dataTable.Rows.Add(new object[]
						{
							dictionary[current],
							current
						});
					}
					dataTable = new DataView(dataTable)
					{
						Sort = "period ASC"
					}.ToTable();
				}
			}
			DateTime now2 = DateTime.Now;
			now2 - now;
			MultiThreadQuery.WriteLog("GetChart1Data(single-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			return dataTable;
		}
		private static DataTable getPDGroupByTimeType(string str_start, string str_end, string device_id, string portid, string groupby, string strgrouptype)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("power_consumption");
			dataColumn.DataType = Type.GetType("System.Double");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("period");
			dataColumn2.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn2);
			Hashtable hashtable = new Hashtable();
			if (groupby != null)
			{
				if (!(groupby == "FORMAT(insert_time, 'yyyy-MM-dd HH')"))
				{
					if (!(groupby == "FORMAT(insert_time, 'yyyy-MM-dd')"))
					{
						if (groupby == "FORMAT(insert_time, 'yyyy-MM')")
						{
							goto IL_8B5;
						}
						if (!(groupby == "FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')"))
						{
							goto IL_112F;
						}
						goto IL_CF3;
					}
				}
				else
				{
					ArrayList arrayList = new ArrayList();
					DateTime dateTime = Convert.ToDateTime(str_start);
					DateTime value = Convert.ToDateTime(str_end);
					dateTime.ToString("yyyy-MM-dd HH");
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					try
					{
						int i = 0;
						while (i < 25)
						{
							DateTime dt_inserttime = dateTime.AddHours((double)i);
							i++;
							if (dt_inserttime.CompareTo(value) >= 0)
							{
								break;
							}
							dBConn = DBConnPool.getDynaConnection(dt_inserttime);
							if (dBConn != null && dBConn.con != null)
							{
								dt_inserttime.ToString("yyyy-MM-dd HH");
								dbCommand = dBConn.con.CreateCommand();
								if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
								{
									string str_sqlformat = string.Concat(new string[]
									{
										"select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy-MM-dd HH') as period from device_data_hourly a where device_id in ( {0} ) and insert_time >= #",
										dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
										"# and insert_time  <#",
										dt_inserttime.AddHours(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
										"#  group by FORMAT(insert_time, 'yyyy-MM-dd HH') "
									});
									arrayList = DBTools.CreateDataTable_Array(dBConn, str_sqlformat, device_id);
								}
								else
								{
									if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
									{
										string str_sqlformat2 = string.Concat(new string[]
										{
											"select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy-MM-dd HH') as period from port_data_hourly a where port_id in ( {0} ) and insert_time >= #",
											dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
											"# and insert_time  <#",
											dt_inserttime.AddHours(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
											"# group by FORMAT(insert_time, 'yyyy-MM-dd HH')"
										});
										arrayList = DBTools.CreateDataTable_Array(dBConn, str_sqlformat2, portid);
									}
								}
								try
								{
									dBConn.Close();
								}
								catch
								{
								}
								DataTable dataTable2 = new DataTable();
								if (arrayList.Count == 1)
								{
									dataTable2 = (DataTable)arrayList[0];
								}
								else
								{
									if (arrayList.Count > 1)
									{
										Dictionary<string, double> dictionary = new Dictionary<string, double>();
										dataTable2 = (DataTable)arrayList[0];
										foreach (DataRow dataRow in dataTable2.Rows)
										{
											dictionary.Add(Convert.ToString(dataRow["period"]), Convert.ToDouble(dataRow["power_consumption"]));
										}
										for (int j = 1; j < arrayList.Count; j++)
										{
											DataTable dataTable3 = (DataTable)arrayList[j];
											foreach (DataRow dataRow2 in dataTable3.Rows)
											{
												string key = Convert.ToString(dataRow2["period"]);
												double num = Convert.ToDouble(dataRow2["power_consumption"]);
												if (dictionary.ContainsKey(key))
												{
													num += dictionary[key];
													dictionary[key] = num;
												}
												else
												{
													dictionary.Add(key, num);
												}
											}
										}
										dataTable2.Rows.Clear();
										foreach (string current in dictionary.Keys)
										{
											dataTable2.Rows.Add(new object[]
											{
												dictionary[current],
												current
											});
										}
										dataTable2 = new DataView(dataTable2)
										{
											Sort = "period ASC"
										}.ToTable();
									}
								}
								if (dataTable2 != null && dataTable2.Rows.Count > 0)
								{
									DataRow dataRow3 = dataTable.NewRow();
									dataRow3[0] = Convert.ToDouble(dataTable2.Rows[0][0]);
									dataRow3[1] = Convert.ToString(dataTable2.Rows[0][1]);
									dataTable.Rows.Add(dataRow3);
								}
							}
						}
						goto IL_112F;
					}
					catch (Exception)
					{
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
						goto IL_112F;
					}
				}
				ArrayList arrayList2 = new ArrayList();
				DateTime dateTime2 = Convert.ToDateTime(str_start);
				DateTime value2 = Convert.ToDateTime(str_end);
				dateTime2.ToString("yyyy-MM-dd");
				DBConn dBConn2 = null;
				DbCommand dbCommand2 = null;
				try
				{
					int k = 0;
					while (k < 32)
					{
						DateTime dt_inserttime2 = dateTime2.AddDays((double)k);
						k++;
						if (dt_inserttime2.CompareTo(value2) >= 0)
						{
							break;
						}
						dBConn2 = DBConnPool.getDynaConnection(dt_inserttime2);
						if (dBConn2 != null && dBConn2.con != null)
						{
							dt_inserttime2.ToString("yyyy-MM-dd");
							if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
							{
								string str_sqlformat3 = "select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy-MM-dd') as period from device_data_daily a where device_id in ( {0} )  group by FORMAT(insert_time, 'yyyy-MM-dd')";
								arrayList2 = DBTools.CreateDataTable_Array(dBConn2, str_sqlformat3, device_id);
							}
							else
							{
								if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
								{
									string str_sqlformat4 = "select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy-MM-dd') as period from port_data_daily a where port_id in ( {0} ) group by FORMAT(insert_time, 'yyyy-MM-dd')";
									arrayList2 = DBTools.CreateDataTable_Array(dBConn2, str_sqlformat4, portid);
								}
							}
							try
							{
								dBConn2.Close();
							}
							catch
							{
							}
							DataTable dataTable4 = new DataTable();
							if (arrayList2.Count == 1)
							{
								dataTable4 = (DataTable)arrayList2[0];
							}
							else
							{
								if (arrayList2.Count > 1)
								{
									Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
									dataTable4 = (DataTable)arrayList2[0];
									foreach (DataRow dataRow4 in dataTable4.Rows)
									{
										dictionary2.Add(Convert.ToString(dataRow4["period"]), Convert.ToDouble(dataRow4["power_consumption"]));
									}
									for (int l = 1; l < arrayList2.Count; l++)
									{
										DataTable dataTable5 = (DataTable)arrayList2[l];
										foreach (DataRow dataRow5 in dataTable5.Rows)
										{
											string key2 = Convert.ToString(dataRow5["period"]);
											double num2 = Convert.ToDouble(dataRow5["power_consumption"]);
											if (dictionary2.ContainsKey(key2))
											{
												num2 += dictionary2[key2];
												dictionary2[key2] = num2;
											}
											else
											{
												dictionary2.Add(key2, num2);
											}
										}
									}
									dataTable4.Rows.Clear();
									foreach (string current2 in dictionary2.Keys)
									{
										dataTable4.Rows.Add(new object[]
										{
											dictionary2[current2],
											current2
										});
									}
									dataTable4 = new DataView(dataTable4)
									{
										Sort = "period ASC"
									}.ToTable();
								}
							}
							if (dataTable4 != null && dataTable4.Rows.Count > 0)
							{
								DataRow dataRow6 = dataTable.NewRow();
								dataRow6[0] = Convert.ToDouble(dataTable4.Rows[0][0]);
								dataRow6[1] = Convert.ToString(dataTable4.Rows[0][1]);
								dataTable.Rows.Add(dataRow6);
							}
						}
					}
					goto IL_112F;
				}
				catch (Exception)
				{
					try
					{
						dbCommand2.Dispose();
					}
					catch
					{
					}
					try
					{
						dBConn2.Close();
					}
					catch
					{
					}
					goto IL_112F;
				}
				IL_8B5:
				ArrayList arrayList3 = new ArrayList();
				DateTime d = Convert.ToDateTime(str_start);
				DateTime dateTime3 = Convert.ToDateTime(str_end);
				int num3 = Convert.ToInt32((dateTime3 - d).TotalDays);
				string key3 = d.ToString("yyyy-MM");
				DBConn dBConn3 = null;
				try
				{
					int m = 0;
					IEnumerator enumerator;
					while (m < num3)
					{
						DateTime dt_inserttime3 = d.AddDays((double)m);
						m++;
						if (dt_inserttime3.CompareTo(dateTime3) >= 0)
						{
							break;
						}
						dBConn3 = DBConnPool.getDynaConnection(dt_inserttime3);
						if (dBConn3 != null && dBConn3.con != null)
						{
							key3 = dt_inserttime3.ToString("yyyy-MM");
							if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
							{
								string str_sqlformat5 = "select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy-MM') as period from device_data_daily a where device_id in ( {0} )  group by FORMAT(insert_time, 'yyyy-MM')";
								arrayList3 = DBTools.CreateDataTable_Array(dBConn3, str_sqlformat5, device_id);
							}
							else
							{
								if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
								{
									string str_sqlformat6 = "select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy-MM') as period from port_data_daily a where port_id in ( {0} ) group by FORMAT(insert_time, 'yyyy-MM')";
									arrayList3 = DBTools.CreateDataTable_Array(dBConn3, str_sqlformat6, portid);
								}
							}
							try
							{
								dBConn3.Close();
							}
							catch
							{
							}
							DataTable dataTable6 = new DataTable();
							if (arrayList3.Count == 1)
							{
								dataTable6 = (DataTable)arrayList3[0];
							}
							else
							{
								if (arrayList3.Count > 1)
								{
									Dictionary<string, double> dictionary3 = new Dictionary<string, double>();
									dataTable6 = (DataTable)arrayList3[0];
									enumerator = dataTable6.Rows.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DataRow dataRow7 = (DataRow)enumerator.Current;
											dictionary3.Add(Convert.ToString(dataRow7["period"]), Convert.ToDouble(dataRow7["power_consumption"]));
										}
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
									for (int n = 1; n < arrayList3.Count; n++)
									{
										DataTable dataTable7 = (DataTable)arrayList3[n];
										enumerator = dataTable7.Rows.GetEnumerator();
										try
										{
											while (enumerator.MoveNext())
											{
												DataRow dataRow8 = (DataRow)enumerator.Current;
												string key4 = Convert.ToString(dataRow8["period"]);
												double num4 = Convert.ToDouble(dataRow8["power_consumption"]);
												if (dictionary3.ContainsKey(key4))
												{
													num4 += dictionary3[key4];
													dictionary3[key4] = num4;
												}
												else
												{
													dictionary3.Add(key4, num4);
												}
											}
										}
										finally
										{
											IDisposable disposable = enumerator as IDisposable;
											if (disposable != null)
											{
												disposable.Dispose();
											}
										}
									}
									dataTable6.Rows.Clear();
									foreach (string current3 in dictionary3.Keys)
									{
										dataTable6.Rows.Add(new object[]
										{
											dictionary3[current3],
											current3
										});
									}
									dataTable6 = new DataView(dataTable6)
									{
										Sort = "period ASC"
									}.ToTable();
								}
							}
							if (dataTable6 != null && dataTable6.Rows.Count > 0)
							{
								if (hashtable.ContainsKey(key3))
								{
									long num5 = Convert.ToInt64(dataTable6.Rows[0][0]);
									hashtable[key3] = num5 + Convert.ToInt64(hashtable[key3]);
								}
								else
								{
									long num6 = Convert.ToInt64(dataTable6.Rows[0][0]);
									hashtable.Add(key3, num6);
								}
							}
						}
					}
					ICollection keys = hashtable.Keys;
					enumerator = keys.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Current;
							DataRow dataRow9 = dataTable.NewRow();
							dataRow9[0] = Convert.ToDouble(hashtable[text]);
							dataRow9[1] = text;
							dataTable.Rows.Add(dataRow9);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					dBConn3.Close();
					goto IL_112F;
				}
				catch
				{
					goto IL_112F;
				}
				IL_CF3:
				ArrayList arrayList4 = new ArrayList();
				DateTime d2 = Convert.ToDateTime(str_start);
				DateTime dateTime4 = Convert.ToDateTime(str_end);
				int num7 = Convert.ToInt32((dateTime4 - d2).TotalDays);
				DBConn dBConn4 = null;
				try
				{
					int num8 = 0;
					IEnumerator enumerator;
					while (num8 <= num7)
					{
						DateTime dt_inserttime4 = d2.AddDays((double)num8);
						num8++;
						if (dt_inserttime4.CompareTo(dateTime4) >= 0)
						{
							break;
						}
						dBConn4 = DBConnPool.getDynaConnection(dt_inserttime4);
						if (dBConn4 != null && dBConn4.con != null)
						{
							if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
							{
								string str_sqlformat7 = "select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q') as period from device_data_daily a where device_id in ( {0} )  group by FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')";
								arrayList4 = DBTools.CreateDataTable_Array(dBConn4, str_sqlformat7, device_id);
							}
							else
							{
								if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
								{
									string str_sqlformat8 = "select sum(power_consumption) as power_consumption,FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q') as period from port_data_daily a where port_id in ( {0} ) group by FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')";
									arrayList4 = DBTools.CreateDataTable_Array(dBConn4, str_sqlformat8, portid);
								}
							}
							try
							{
								dBConn4.Close();
							}
							catch
							{
							}
							DataTable dataTable8 = new DataTable();
							if (arrayList4.Count == 1)
							{
								dataTable8 = (DataTable)arrayList4[0];
							}
							else
							{
								if (arrayList4.Count > 1)
								{
									Dictionary<string, double> dictionary4 = new Dictionary<string, double>();
									dataTable8 = (DataTable)arrayList4[0];
									enumerator = dataTable8.Rows.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DataRow dataRow10 = (DataRow)enumerator.Current;
											dictionary4.Add(Convert.ToString(dataRow10["period"]), Convert.ToDouble(dataRow10["power_consumption"]));
										}
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
									for (int num9 = 1; num9 < arrayList4.Count; num9++)
									{
										DataTable dataTable9 = (DataTable)arrayList4[num9];
										enumerator = dataTable9.Rows.GetEnumerator();
										try
										{
											while (enumerator.MoveNext())
											{
												DataRow dataRow11 = (DataRow)enumerator.Current;
												string key5 = Convert.ToString(dataRow11["period"]);
												double num10 = Convert.ToDouble(dataRow11["power_consumption"]);
												if (dictionary4.ContainsKey(key5))
												{
													num10 += dictionary4[key5];
													dictionary4[key5] = num10;
												}
												else
												{
													dictionary4.Add(key5, num10);
												}
											}
										}
										finally
										{
											IDisposable disposable = enumerator as IDisposable;
											if (disposable != null)
											{
												disposable.Dispose();
											}
										}
									}
									dataTable8.Rows.Clear();
									foreach (string current4 in dictionary4.Keys)
									{
										dataTable8.Rows.Add(new object[]
										{
											dictionary4[current4],
											current4
										});
									}
									dataTable8 = new DataView(dataTable8)
									{
										Sort = "period ASC"
									}.ToTable();
								}
							}
							if (dataTable8 != null && dataTable8.Rows.Count > 0)
							{
								string key6 = Convert.ToString(dataTable8.Rows[0][1]);
								if (hashtable.ContainsKey(key6))
								{
									long num11 = Convert.ToInt64(dataTable8.Rows[0][0]);
									hashtable[key6] = num11 + Convert.ToInt64(hashtable[key6]);
								}
								else
								{
									long num12 = Convert.ToInt64(dataTable8.Rows[0][0]);
									hashtable.Add(key6, num12);
								}
							}
						}
					}
					ICollection keys2 = hashtable.Keys;
					enumerator = keys2.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text2 = (string)enumerator.Current;
							DataRow dataRow12 = dataTable.NewRow();
							dataRow12[0] = Convert.ToDouble(hashtable[text2]);
							dataRow12[1] = text2;
							dataTable.Rows.Add(dataRow12);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					dBConn4.Close();
				}
				catch
				{
				}
			}
			IL_112F:
			dataTable = new DataView(dataTable)
			{
				Sort = "period ASC"
			}.ToTable();
			return dataTable;
		}
		private static DataTable getPowerMaxByTimeType(string str_Start, string str_End, string device_id, string portid, string groupby, string strgrouptype)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("power");
			dataColumn.DataType = Type.GetType("System.Double");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("starttime");
			dataColumn2.DataType = Type.GetType("System.DateTime");
			dataTable.Columns.Add(dataColumn2);
			dataColumn2 = new DataColumn("endtime");
			dataColumn2.DataType = Type.GetType("System.DateTime");
			dataTable.Columns.Add(dataColumn2);
			DataColumn dataColumn3 = new DataColumn("period");
			dataColumn3.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn3);
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			Hashtable hashtable3 = new Hashtable();
			ArrayList arrayList = new ArrayList();
			DateTime d = Convert.ToDateTime(str_Start);
			DateTime dateTime = Convert.ToDateTime(str_End);
			string key = d.ToString("yyyy-MM-dd HH");
			if (groupby != null)
			{
				IEnumerator enumerator;
				if (!(groupby == "FORMAT(insert_time, 'yyyy-MM-dd HH')"))
				{
					if (!(groupby == "FORMAT(insert_time, 'yyyy-MM-dd')"))
					{
						if (groupby == "FORMAT(insert_time, 'yyyy-MM')")
						{
							goto IL_BF5;
						}
						if (!(groupby == "FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')"))
						{
							goto IL_1697;
						}
						goto IL_1139;
					}
				}
				else
				{
					int i = 0;
					while (i < 25)
					{
						DateTime dt_inserttime = d.AddHours((double)i);
						i++;
						if (dt_inserttime.CompareTo(dateTime) >= 0)
						{
							break;
						}
						DBConn dynaConnection = DBConnPool.getDynaConnection(dt_inserttime);
						if (dynaConnection != null && dynaConnection.con != null)
						{
							key = dt_inserttime.ToString("yyyy-MM-dd HH");
							if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
							{
								string str_sqlformat = string.Concat(new string[]
								{
									"select sum(power) as power_value, insert_time from device_auto_info where insert_time >=#",
									dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
									"# and  insert_time <#",
									dt_inserttime.AddHours(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
									"# and device_id in ( {0} )  group by insert_time order by insert_time ASC "
								});
								arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, device_id);
							}
							else
							{
								if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
								{
									string str_sqlformat = string.Concat(new string[]
									{
										"select sum(power) as power_value, insert_time from port_auto_info where insert_time >=#",
										dt_inserttime.ToString("yyyy-MM-dd HH:mm:ss"),
										"# and  insert_time <#",
										dt_inserttime.AddHours(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
										"# and port_id in ( {0} )  group by insert_time order by insert_time ASC "
									});
									arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, portid);
								}
							}
							try
							{
								dynaConnection.Close();
							}
							catch
							{
							}
							DataTable dataTable2 = new DataTable();
							if (arrayList.Count == 1)
							{
								dataTable2 = (DataTable)arrayList[0];
							}
							else
							{
								if (arrayList.Count > 1)
								{
									Dictionary<string, long> dictionary = new Dictionary<string, long>();
									dataTable2 = (DataTable)arrayList[0];
									enumerator = dataTable2.Rows.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DataRow dataRow = (DataRow)enumerator.Current;
											dictionary.Add(Convert.ToString(dataRow["insert_time"]), Convert.ToInt64(dataRow["power_value"]));
										}
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
									for (int j = 1; j < arrayList.Count; j++)
									{
										DataTable dataTable3 = (DataTable)arrayList[j];
										enumerator = dataTable3.Rows.GetEnumerator();
										try
										{
											while (enumerator.MoveNext())
											{
												DataRow dataRow2 = (DataRow)enumerator.Current;
												string key2 = Convert.ToString(dataRow2["insert_time"]);
												long num = Convert.ToInt64(dataRow2["power_value"]);
												if (dictionary.ContainsKey(key2))
												{
													num += dictionary[key2];
													dictionary[key2] = num;
												}
												else
												{
													dictionary.Add(key2, num);
												}
											}
										}
										finally
										{
											IDisposable disposable = enumerator as IDisposable;
											if (disposable != null)
											{
												disposable.Dispose();
											}
										}
									}
									dataTable2.Rows.Clear();
									foreach (string current in dictionary.Keys)
									{
										dataTable2.Rows.Add(new object[]
										{
											dictionary[current],
											current
										});
									}
									dataTable2 = new DataView(dataTable2)
									{
										Sort = "insert_time ASC"
									}.ToTable();
								}
							}
							if (dataTable2 != null && dataTable2.Rows.Count > 0)
							{
								string value = Convert.ToDateTime(dataTable2.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
								string value2 = Convert.ToDateTime(dataTable2.Rows[dataTable2.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
								enumerator = dataTable2.Rows.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										DataRow dataRow3 = (DataRow)enumerator.Current;
										key = Convert.ToDateTime(dataRow3[1]).ToString("yyyy-MM-dd HH");
										long num2 = Convert.ToInt64(dataRow3[0]);
										if (hashtable.ContainsKey(key))
										{
											long num3 = Convert.ToInt64(hashtable[key]);
											if (num2 > num3)
											{
												hashtable[key] = num2;
											}
										}
										else
										{
											hashtable.Add(key, num2);
											hashtable3.Add(key, value);
											hashtable2.Add(key, value2);
										}
									}
								}
								finally
								{
									IDisposable disposable = enumerator as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
							}
						}
					}
					if (hashtable == null || hashtable.Count <= 0)
					{
						goto IL_1697;
					}
					ICollection keys = hashtable.Keys;
					enumerator = keys.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Current;
							DataRow dataRow4 = dataTable.NewRow();
							double num4 = (double)(Convert.ToInt64(hashtable[text]) / 1000L);
							dataRow4[0] = num4;
							dataRow4[1] = Convert.ToDateTime(hashtable3[text]);
							dataRow4[2] = Convert.ToDateTime(hashtable2[text]);
							dataRow4[3] = text;
							dataTable.Rows.Add(dataRow4);
						}
						goto IL_1697;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				int k = 0;
				while (k < 32)
				{
					DateTime dt_inserttime2 = d.AddDays((double)k);
					k++;
					if (dt_inserttime2.CompareTo(dateTime) >= 0)
					{
						break;
					}
					DBConn dynaConnection = DBConnPool.getDynaConnection(dt_inserttime2);
					if (dynaConnection != null && dynaConnection.con != null)
					{
						key = dt_inserttime2.ToString("yyyy-MM-dd");
						if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
						{
							string str_sqlformat = "select sum(power) as power_value, insert_time from device_auto_info where device_id in ( {0} )  group by insert_time order by insert_time ASC ";
							arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, device_id);
						}
						else
						{
							if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
							{
								string str_sqlformat = "select sum(power) as power_value, insert_time from port_auto_info where port_id in ( {0} )  group by insert_time order by insert_time ASC ";
								arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, portid);
							}
						}
						try
						{
							dynaConnection.Close();
						}
						catch
						{
						}
						DataTable dataTable4 = new DataTable();
						if (arrayList.Count == 1)
						{
							dataTable4 = (DataTable)arrayList[0];
						}
						else
						{
							if (arrayList.Count > 1)
							{
								Dictionary<string, long> dictionary2 = new Dictionary<string, long>();
								dataTable4 = (DataTable)arrayList[0];
								enumerator = dataTable4.Rows.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										DataRow dataRow5 = (DataRow)enumerator.Current;
										dictionary2.Add(Convert.ToString(dataRow5["insert_time"]), Convert.ToInt64(dataRow5["power_value"]));
									}
								}
								finally
								{
									IDisposable disposable = enumerator as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
								for (int l = 1; l < arrayList.Count; l++)
								{
									DataTable dataTable5 = (DataTable)arrayList[l];
									enumerator = dataTable5.Rows.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DataRow dataRow6 = (DataRow)enumerator.Current;
											string key3 = Convert.ToString(dataRow6["insert_time"]);
											long num5 = Convert.ToInt64(dataRow6["power_value"]);
											if (dictionary2.ContainsKey(key3))
											{
												num5 += dictionary2[key3];
												dictionary2[key3] = num5;
											}
											else
											{
												dictionary2.Add(key3, num5);
											}
										}
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
								}
								dataTable4.Rows.Clear();
								foreach (string current2 in dictionary2.Keys)
								{
									dataTable4.Rows.Add(new object[]
									{
										dictionary2[current2],
										current2
									});
								}
								dataTable4 = new DataView(dataTable4)
								{
									Sort = "insert_time ASC"
								}.ToTable();
							}
						}
						if (dataTable4 != null && dataTable4.Rows.Count > 0)
						{
							string value3 = Convert.ToDateTime(dataTable4.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
							string value4 = Convert.ToDateTime(dataTable4.Rows[dataTable4.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
							enumerator = dataTable4.Rows.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DataRow dataRow7 = (DataRow)enumerator.Current;
									key = Convert.ToDateTime(dataRow7[1]).ToString("yyyy-MM-dd");
									long num6 = Convert.ToInt64(dataRow7[0]);
									if (hashtable.ContainsKey(key))
									{
										long num7 = Convert.ToInt64(hashtable[key]);
										if (num6 > num7)
										{
											hashtable[key] = num6;
										}
									}
									else
									{
										hashtable.Add(key, num6);
										hashtable3.Add(key, value3);
										hashtable2.Add(key, value4);
									}
								}
							}
							finally
							{
								IDisposable disposable = enumerator as IDisposable;
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
						}
					}
				}
				if (hashtable == null || hashtable.Count <= 0)
				{
					goto IL_1697;
				}
				ICollection keys2 = hashtable.Keys;
				enumerator = keys2.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string text2 = (string)enumerator.Current;
						DataRow dataRow8 = dataTable.NewRow();
						double num8 = (double)(Convert.ToInt64(hashtable[text2]) / 1000L);
						dataRow8[0] = num8;
						dataRow8[1] = Convert.ToDateTime(hashtable3[text2]);
						dataRow8[2] = Convert.ToDateTime(hashtable2[text2]);
						dataRow8[3] = text2;
						dataTable.Rows.Add(dataRow8);
					}
					goto IL_1697;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				IL_BF5:
				int m = 0;
				int num9 = Convert.ToInt32((dateTime - d).TotalDays);
				while (m <= num9)
				{
					DateTime dt_inserttime3 = d.AddDays((double)m);
					m++;
					if (dt_inserttime3.CompareTo(dateTime) >= 0)
					{
						break;
					}
					DBConn dynaConnection = DBConnPool.getDynaConnection(dt_inserttime3);
					if (dynaConnection != null && dynaConnection.con != null)
					{
						key = dt_inserttime3.ToString("yyyy-MM");
						if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
						{
							string str_sqlformat = "select sum(power) as power_value, insert_time from device_auto_info where device_id in ( {0} )  group by insert_time order by insert_time ASC ";
							arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, device_id);
						}
						else
						{
							if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
							{
								string str_sqlformat = "select sum(power) as power_value, insert_time from port_auto_info where port_id in ( {0} )  group by insert_time order by insert_time ASC ";
								arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, portid);
							}
						}
						try
						{
							dynaConnection.Close();
						}
						catch
						{
						}
						DataTable dataTable6 = new DataTable();
						if (arrayList.Count == 1)
						{
							dataTable6 = (DataTable)arrayList[0];
						}
						else
						{
							if (arrayList.Count > 1)
							{
								Dictionary<string, long> dictionary3 = new Dictionary<string, long>();
								dataTable6 = (DataTable)arrayList[0];
								enumerator = dataTable6.Rows.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										DataRow dataRow9 = (DataRow)enumerator.Current;
										dictionary3.Add(Convert.ToString(dataRow9["insert_time"]), Convert.ToInt64(dataRow9["power_value"]));
									}
								}
								finally
								{
									IDisposable disposable = enumerator as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
								for (int n = 1; n < arrayList.Count; n++)
								{
									DataTable dataTable7 = (DataTable)arrayList[n];
									enumerator = dataTable7.Rows.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DataRow dataRow10 = (DataRow)enumerator.Current;
											string key4 = Convert.ToString(dataRow10["insert_time"]);
											long num10 = Convert.ToInt64(dataRow10["power_value"]);
											if (dictionary3.ContainsKey(key4))
											{
												num10 += dictionary3[key4];
												dictionary3[key4] = num10;
											}
											else
											{
												dictionary3.Add(key4, num10);
											}
										}
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
								}
								dataTable6.Rows.Clear();
								foreach (string current3 in dictionary3.Keys)
								{
									dataTable6.Rows.Add(new object[]
									{
										dictionary3[current3],
										current3
									});
								}
								dataTable6 = new DataView(dataTable6)
								{
									Sort = "insert_time ASC"
								}.ToTable();
							}
						}
						if (dataTable6 != null && dataTable6.Rows.Count > 0)
						{
							string value5 = Convert.ToDateTime(dataTable6.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
							string value6 = Convert.ToDateTime(dataTable6.Rows[dataTable6.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
							enumerator = dataTable6.Rows.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DataRow dataRow11 = (DataRow)enumerator.Current;
									key = dt_inserttime3.ToString("yyyy-MM");
									long num11 = Convert.ToInt64(dataRow11[0]);
									if (hashtable.ContainsKey(key))
									{
										long num12 = Convert.ToInt64(hashtable[key]);
										if (num11 > num12)
										{
											hashtable[key] = num11;
										}
										try
										{
											DateTime value7 = Convert.ToDateTime(hashtable2[key]);
											if (Convert.ToDateTime(value6).CompareTo(value7) > 0)
											{
												hashtable2[key] = value6;
											}
											continue;
										}
										catch
										{
											continue;
										}
									}
									hashtable.Add(key, num11);
									hashtable3.Add(key, value5);
									hashtable2.Add(key, value6);
								}
							}
							finally
							{
								IDisposable disposable = enumerator as IDisposable;
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
						}
					}
				}
				if (hashtable == null || hashtable.Count <= 0)
				{
					goto IL_1697;
				}
				ICollection keys3 = hashtable.Keys;
				enumerator = keys3.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string text3 = (string)enumerator.Current;
						DataRow dataRow12 = dataTable.NewRow();
						double num13 = (double)(Convert.ToInt64(hashtable[text3]) / 1000L);
						dataRow12[0] = num13;
						dataRow12[1] = Convert.ToDateTime(hashtable3[text3]);
						dataRow12[2] = Convert.ToDateTime(hashtable2[text3]);
						dataRow12[3] = text3;
						dataTable.Rows.Add(dataRow12);
					}
					goto IL_1697;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				IL_1139:
				int num14 = 0;
				int num15 = Convert.ToInt32((dateTime - d).TotalDays);
				while (num14 <= num15)
				{
					DateTime dt_inserttime4 = d.AddDays((double)num14);
					num14++;
					if (dt_inserttime4.CompareTo(dateTime) >= 0)
					{
						break;
					}
					DBConn dynaConnection = DBConnPool.getDynaConnection(dt_inserttime4);
					if (dynaConnection != null && dynaConnection.con != null)
					{
						key = dt_inserttime4.ToString("yyyy-MM");
						if (strgrouptype == "zone" || strgrouptype == "rack" || strgrouptype == "allrack" || strgrouptype == "dev" || strgrouptype == "alldev")
						{
							string str_sqlformat = "select sum(power) as power_value, insert_time,FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q') as period from device_auto_info where device_id in ( {0} )  group by insert_time order by insert_time ASC ";
							arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, device_id);
						}
						else
						{
							if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
							{
								string str_sqlformat = "select sum(power) as power_value, insert_time,FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q') as period from port_auto_info where port_id in ( {0} )  group by insert_time order by insert_time ASC ";
								arrayList = DBTools.CreateDataTable_Array(dynaConnection, str_sqlformat, portid);
							}
						}
						try
						{
							dynaConnection.Close();
						}
						catch
						{
						}
						DataTable dataTable8 = new DataTable();
						if (arrayList.Count == 1)
						{
							dataTable8 = (DataTable)arrayList[0];
						}
						else
						{
							if (arrayList.Count > 1)
							{
								Dictionary<string, long> dictionary4 = new Dictionary<string, long>();
								dataTable8 = (DataTable)arrayList[0];
								string text4 = "";
								enumerator = dataTable8.Rows.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										DataRow dataRow13 = (DataRow)enumerator.Current;
										dictionary4.Add(Convert.ToString(dataRow13["insert_time"]), Convert.ToInt64(dataRow13["power_value"]));
										text4 = Convert.ToString(dataRow13[2]);
									}
								}
								finally
								{
									IDisposable disposable = enumerator as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
								for (int num16 = 1; num16 < arrayList.Count; num16++)
								{
									DataTable dataTable9 = (DataTable)arrayList[num16];
									enumerator = dataTable9.Rows.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DataRow dataRow14 = (DataRow)enumerator.Current;
											string key5 = Convert.ToString(dataRow14["insert_time"]);
											long num17 = Convert.ToInt64(dataRow14["power_value"]);
											if (dictionary4.ContainsKey(key5))
											{
												num17 += dictionary4[key5];
												dictionary4[key5] = num17;
											}
											else
											{
												dictionary4.Add(key5, num17);
											}
										}
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
								}
								dataTable8.Rows.Clear();
								foreach (string current4 in dictionary4.Keys)
								{
									dataTable8.Rows.Add(new object[]
									{
										dictionary4[current4],
										current4,
										text4
									});
								}
								dataTable8 = new DataView(dataTable8)
								{
									Sort = "insert_time ASC"
								}.ToTable();
							}
						}
						if (dataTable8 != null && dataTable8.Rows.Count > 0)
						{
							string value8 = Convert.ToDateTime(dataTable8.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
							string value9 = Convert.ToDateTime(dataTable8.Rows[dataTable8.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
							enumerator = dataTable8.Rows.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DataRow dataRow15 = (DataRow)enumerator.Current;
									key = Convert.ToString(dataRow15[2]);
									long num18 = Convert.ToInt64(dataRow15[0]);
									if (hashtable.ContainsKey(key))
									{
										long num19 = Convert.ToInt64(hashtable[key]);
										if (num18 > num19)
										{
											hashtable[key] = num18;
										}
										try
										{
											DateTime value10 = Convert.ToDateTime(hashtable2[key]);
											if (Convert.ToDateTime(value9).CompareTo(value10) > 0)
											{
												hashtable2[key] = value9;
											}
											continue;
										}
										catch
										{
											continue;
										}
									}
									hashtable.Add(key, num18);
									hashtable3.Add(key, value8);
									hashtable2.Add(key, value9);
								}
							}
							finally
							{
								IDisposable disposable = enumerator as IDisposable;
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
						}
					}
				}
				if (hashtable != null && hashtable.Count > 0)
				{
					ICollection keys4 = hashtable.Keys;
					enumerator = keys4.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string text5 = (string)enumerator.Current;
							DataRow dataRow16 = dataTable.NewRow();
							double num20 = (double)(Convert.ToInt64(hashtable[text5]) / 1000L);
							dataRow16[0] = num20;
							dataRow16[1] = Convert.ToDateTime(hashtable3[text5]);
							dataRow16[2] = Convert.ToDateTime(hashtable2[text5]);
							dataRow16[3] = text5;
							dataTable.Rows.Add(dataRow16);
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			IL_1697:
			dataTable = new DataView(dataTable)
			{
				Sort = "period ASC"
			}.ToTable();
			return dataTable;
		}
		public static DataTable GetChart2Data(string str_Start, string str_End, string device_id, string portid, string groupby, string strgrouptype)
		{
			TickTimer tickTimer = new TickTimer();
			tickTimer.Start();
			bool flag = false;
			if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
			{
				flag = true;
			}
			if (!DBUrl.SERVERMODE && !DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				DataTable powerMaxByTimeType = DBTools.getPowerMaxByTimeType(str_Start, str_End, device_id, portid, groupby, strgrouptype);
				MultiThreadQuery.WriteLog("GetChart2Data(Access): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
				return powerMaxByTimeType;
			}
			int queryThreads = MultiThreadQuery.getQueryThreads();
			if (queryThreads > 0)
			{
				DataTable chart2Data = MultiThreadQuery.GetChart2Data(queryThreads, str_Start, str_End, device_id, portid, groupby, strgrouptype);
				MultiThreadQuery.WriteLog("GetChart2Data(multi-thread): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
				return chart2Data;
			}
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("power");
			dataColumn.DataType = Type.GetType("System.Double");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("starttime");
			dataColumn2.DataType = Type.GetType("System.DateTime");
			dataTable.Columns.Add(dataColumn2);
			dataColumn2 = new DataColumn("endtime");
			dataColumn2.DataType = Type.GetType("System.DateTime");
			dataTable.Columns.Add(dataColumn2);
			DataColumn dataColumn3 = new DataColumn("period");
			dataColumn3.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn3);
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			Hashtable hashtable3 = new Hashtable();
			DateTime d = Convert.ToDateTime(str_Start);
			DateTime dateTime = Convert.ToDateTime(str_End);
			string key = d.ToString("yyyy-MM-dd HH");
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			dBConn = DBConnPool.getDynaConnection();
			if (dBConn == null || dBConn.con == null)
			{
				return dataTable;
			}
			try
			{
				dbCommand = dBConn.con.CreateCommand();
				DataTable dataTable2 = new DataTable();
				if (groupby != null)
				{
					if (!(groupby == "date_format(insert_time, '%Y-%m-%d %H')"))
					{
						if (!(groupby == "date_format(insert_time, '%Y-%m-%d')"))
						{
							if (!(groupby == "date_format(insert_time, '%Y-%m')"))
							{
								if (groupby == "concat(date_format(insert_time, '%Y'),'Q',quarter(insert_time))")
								{
									int i = 0;
									int num = Convert.ToInt32((dateTime - d).TotalDays);
									while (i <= num)
									{
										DateTime dateTime2 = d.AddDays((double)i);
										i++;
										if (dateTime2.CompareTo(dateTime) >= 0)
										{
											break;
										}
										string text = dateTime2.ToString("yyyyMMdd");
										if (!flag)
										{
											dbCommand.CommandText = string.Concat(new string[]
											{
												"select sum(power) as power_value, insert_time,concat(date_format(insert_time, '%Y'),'Q',quarter(insert_time)) as period from device_auto_info",
												text,
												" where device_id in ( ",
												device_id,
												" )  group by insert_time order by insert_time ASC "
											});
										}
										else
										{
											dbCommand.CommandText = string.Concat(new string[]
											{
												"select sum(power) as power_value, insert_time,concat(date_format(insert_time, '%Y'),'Q',quarter(insert_time)) as period from port_auto_info",
												text,
												" where port_id in ( ",
												portid,
												" )  group by insert_time order by insert_time ASC "
											});
										}
										dataTable2 = new DataTable();
										dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
										dbDataAdapter.SelectCommand = dbCommand;
										try
										{
											dbDataAdapter.Fill(dataTable2);
										}
										catch
										{
											dbDataAdapter.Dispose();
											continue;
										}
										dbDataAdapter.Dispose();
										if (dataTable2 != null && dataTable2.Rows.Count > 0)
										{
											string value = Convert.ToDateTime(dataTable2.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
											string value2 = Convert.ToDateTime(dataTable2.Rows[dataTable2.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
											foreach (DataRow dataRow in dataTable2.Rows)
											{
												key = Convert.ToString(dataRow[2]);
												long num2 = Convert.ToInt64(dataRow[0]);
												if (hashtable.ContainsKey(key))
												{
													long num3 = Convert.ToInt64(hashtable[key]);
													if (num2 > num3)
													{
														hashtable[key] = num2;
													}
													try
													{
														DateTime value3 = Convert.ToDateTime(hashtable2[key]);
														if (Convert.ToDateTime(value2).CompareTo(value3) > 0)
														{
															hashtable2[key] = value2;
														}
														continue;
													}
													catch
													{
														continue;
													}
												}
												hashtable.Add(key, num2);
												hashtable3.Add(key, value);
												hashtable2.Add(key, value2);
											}
										}
									}
								}
							}
							else
							{
								int j = 0;
								int num4 = Convert.ToInt32((dateTime - d).TotalDays);
								while (j <= num4)
								{
									DateTime dateTime3 = d.AddDays((double)j);
									j++;
									if (dateTime3.CompareTo(dateTime) >= 0)
									{
										break;
									}
									key = dateTime3.ToString("yyyy-MM");
									string text2 = dateTime3.ToString("yyyyMMdd");
									if (!flag)
									{
										dbCommand.CommandText = string.Concat(new string[]
										{
											"select sum(power) as power_value, insert_time from device_auto_info",
											text2,
											" where  device_id in ( ",
											device_id,
											" )  group by insert_time  order by insert_time ASC"
										});
									}
									else
									{
										dbCommand.CommandText = string.Concat(new string[]
										{
											"select sum(power) as power_value, insert_time from port_auto_info",
											text2,
											" where  port_id in ( ",
											portid,
											" )  group by insert_time  order by insert_time ASC"
										});
									}
									dataTable2 = new DataTable();
									dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
									dbDataAdapter.SelectCommand = dbCommand;
									try
									{
										dbDataAdapter.Fill(dataTable2);
									}
									catch
									{
										dbDataAdapter.Dispose();
										continue;
									}
									dbDataAdapter.Dispose();
									if (dataTable2 != null && dataTable2.Rows.Count > 0)
									{
										string value4 = Convert.ToDateTime(dataTable2.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
										string value5 = Convert.ToDateTime(dataTable2.Rows[dataTable2.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
										foreach (DataRow dataRow2 in dataTable2.Rows)
										{
											key = dateTime3.ToString("yyyy-MM");
											long num5 = Convert.ToInt64(dataRow2[0]);
											if (hashtable.ContainsKey(key))
											{
												long num6 = Convert.ToInt64(hashtable[key]);
												if (num5 > num6)
												{
													hashtable[key] = num5;
												}
												try
												{
													DateTime value6 = Convert.ToDateTime(hashtable2[key]);
													if (Convert.ToDateTime(value5).CompareTo(value6) > 0)
													{
														hashtable2[key] = value5;
													}
													continue;
												}
												catch
												{
													continue;
												}
											}
											hashtable.Add(key, num5);
											hashtable3.Add(key, value4);
											hashtable2.Add(key, value5);
										}
									}
								}
							}
						}
						else
						{
							int k = 0;
							while (k < 32)
							{
								DateTime dateTime4 = d.AddDays((double)k);
								k++;
								if (dateTime4.CompareTo(dateTime) >= 0)
								{
									break;
								}
								key = dateTime4.ToString("yyyy-MM-dd");
								string text3 = dateTime4.ToString("yyyyMMdd");
								if (!flag)
								{
									dbCommand.CommandText = string.Concat(new string[]
									{
										"select sum(power) as power_value, insert_time from device_auto_info",
										text3,
										" where  device_id in ( ",
										device_id,
										" )  group by insert_time  order by insert_time ASC"
									});
								}
								else
								{
									dbCommand.CommandText = string.Concat(new string[]
									{
										"select sum(power) as power_value, insert_time from port_auto_info",
										text3,
										" where  port_id in ( ",
										portid,
										" )  group by insert_time  order by insert_time ASC"
									});
								}
								dataTable2 = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
								dbDataAdapter.SelectCommand = dbCommand;
								try
								{
									dbDataAdapter.Fill(dataTable2);
								}
								catch
								{
									dbDataAdapter.Dispose();
									continue;
								}
								dbDataAdapter.Dispose();
								if (dataTable2 != null && dataTable2.Rows.Count > 0)
								{
									string value7 = Convert.ToDateTime(dataTable2.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
									string value8 = Convert.ToDateTime(dataTable2.Rows[dataTable2.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
									foreach (DataRow dataRow3 in dataTable2.Rows)
									{
										key = Convert.ToDateTime(dataRow3[1]).ToString("yyyy-MM-dd");
										long num7 = Convert.ToInt64(dataRow3[0]);
										if (hashtable.ContainsKey(key))
										{
											long num8 = Convert.ToInt64(hashtable[key]);
											if (num7 > num8)
											{
												hashtable[key] = num7;
											}
										}
										else
										{
											hashtable.Add(key, num7);
											hashtable3.Add(key, value7);
											hashtable2.Add(key, value8);
										}
									}
								}
							}
						}
					}
					else
					{
						int l = 0;
						while (l < 25)
						{
							DateTime dateTime5 = d.AddHours((double)l);
							l++;
							if (dateTime5.CompareTo(dateTime) >= 0)
							{
								break;
							}
							key = dateTime5.ToString("yyyy-MM-dd HH");
							string text4 = dateTime5.ToString("yyyyMMdd");
							if (!flag)
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"select sum(power) as power_value, insert_time from device_auto_info",
									text4,
									" where insert_time >= '",
									dateTime5.ToString("yyyy-MM-dd HH:mm:ss"),
									"' and  insert_time < '",
									dateTime5.AddHours(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
									"' and device_id in ( ",
									device_id,
									" )  group by insert_time  order by insert_time ASC"
								});
							}
							else
							{
								dbCommand.CommandText = string.Concat(new string[]
								{
									"select sum(power) as power_value, insert_time from port_auto_info",
									text4,
									" where insert_time >= '",
									dateTime5.ToString("yyyy-MM-dd HH:mm:ss"),
									"' and  insert_time < '",
									dateTime5.AddHours(1.0).ToString("yyyy-MM-dd HH:mm:ss"),
									"' and port_id in ( ",
									portid,
									" )  group by insert_time  order by insert_time ASC"
								});
							}
							dataTable2 = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(dBConn.con);
							dbDataAdapter.SelectCommand = dbCommand;
							try
							{
								dbDataAdapter.Fill(dataTable2);
							}
							catch
							{
								dbDataAdapter.Dispose();
								continue;
							}
							dbDataAdapter.Dispose();
							if (dataTable2 != null && dataTable2.Rows.Count > 0)
							{
								string value9 = Convert.ToDateTime(dataTable2.Rows[0][1]).ToString("yyyy-MM-dd HH:mm:ss");
								string value10 = Convert.ToDateTime(dataTable2.Rows[dataTable2.Rows.Count - 1][1]).ToString("yyyy-MM-dd HH:mm:ss");
								foreach (DataRow dataRow4 in dataTable2.Rows)
								{
									key = Convert.ToDateTime(dataRow4[1]).ToString("yyyy-MM-dd HH");
									long num9 = Convert.ToInt64(dataRow4[0]);
									if (hashtable.ContainsKey(key))
									{
										long num10 = Convert.ToInt64(hashtable[key]);
										if (num9 > num10)
										{
											hashtable[key] = num9;
										}
									}
									else
									{
										hashtable.Add(key, num9);
										hashtable3.Add(key, value9);
										hashtable2.Add(key, value10);
									}
								}
							}
						}
					}
				}
				if (hashtable != null && hashtable.Count > 0)
				{
					ICollection keys = hashtable.Keys;
					foreach (string text5 in keys)
					{
						DataRow dataRow5 = dataTable.NewRow();
						double num11 = (double)(Convert.ToInt64(hashtable[text5]) / 1000L);
						dataRow5[0] = num11;
						dataRow5[1] = Convert.ToDateTime(hashtable3[text5]);
						dataRow5[2] = Convert.ToDateTime(hashtable2[text5]);
						dataRow5[3] = text5;
						dataTable.Rows.Add(dataRow5);
					}
				}
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
				if (dBConn != null)
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~GetChart2Data Error : " + ex.Message + "\n" + ex.StackTrace);
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
				if (dBConn != null)
				{
					try
					{
						dBConn.Close();
					}
					catch
					{
					}
				}
			}
			dataTable = new DataView(dataTable)
			{
				Sort = "period ASC"
			}.ToTable();
			MultiThreadQuery.WriteLog("GetChart2Data(MySQL): elapsed=" + tickTimer.getElapsed().ToString(), new string[0]);
			return dataTable;
		}
		private static void GetPowerAndNameByID(ref Hashtable ht_devpw, ref Hashtable ht_portpw, DBConn conn, bool b_port, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			DbCommand dbCommand = null;
			DbDataAdapter dbDataAdapter = null;
			try
			{
				if (conn != null && conn.con != null)
				{
					string text = "";
					string format = "";
					dbCommand = conn.con.CreateCommand();
					if (!b_port)
					{
						try
						{
							if (strBegin.Length == 0 && strEnd.Length == 0)
							{
								format = "select device_id,max(power)/1000 as power from device_auto_info where device_id in ({0}) group by device_id";
							}
							else
							{
								format = string.Concat(new string[]
								{
									"select device_id,max(power)/1000 as power from device_auto_info where insert_time >= #",
									strBegin,
									"# and  insert_time < #",
									strEnd,
									"# and device_id in ({0}) group by device_id"
								});
							}
							text = invalid_device_ids;
							while (text.Length > 0)
							{
								string text2;
								if (text.Length > 30000)
								{
									text2 = text.Substring(0, 30000);
									int num = text2.LastIndexOf(',');
									text2 = text.Substring(0, num);
									text = text.Substring(num + 1);
								}
								else
								{
									text2 = text;
									text = "";
								}
								string commandText = string.Format(format, text2);
								DataTable dataTable = new DataTable();
								dbDataAdapter = DBConn.GetDataAdapter(conn.con);
								dbCommand.CommandText = commandText;
								dbDataAdapter.SelectCommand = dbCommand;
								dbDataAdapter.Fill(dataTable);
								dbDataAdapter.Dispose();
								if (dataTable != null)
								{
									foreach (DataRow dataRow in dataTable.Rows)
									{
										int num2 = Convert.ToInt32(dataRow[0]);
										double num3 = Convert.ToDouble(dataRow[1]);
										if (ht_devpw.ContainsKey(num2))
										{
											double num4 = (double)ht_devpw[num2];
											if (num3 > num4)
											{
												ht_devpw[num2] = num3;
											}
										}
										else
										{
											ht_devpw.Add(num2, num3);
										}
									}
								}
								dataTable = new DataTable();
							}
						}
						catch (Exception)
						{
						}
					}
					try
					{
						if (strBegin.Length == 0 && strEnd.Length == 0)
						{
							format = "select port_id,max(power)/1000 as power from port_auto_info where port_id in ({0}) group by port_id";
						}
						else
						{
							format = string.Concat(new string[]
							{
								"select port_id,max(power)/1000 as power from port_auto_info where insert_time >= #",
								strBegin,
								"# and  insert_time < #",
								strEnd,
								"# and port_id in ({0}) group by port_id"
							});
						}
						text = portid;
						while (text.Length > 0)
						{
							string text2;
							if (text.Length > 30000)
							{
								text2 = text.Substring(0, 30000);
								int num = text2.LastIndexOf(',');
								text2 = text.Substring(0, num);
								text = text.Substring(num + 1);
							}
							else
							{
								text2 = text;
								text = "";
							}
							string commandText2 = string.Format(format, text2);
							DataTable dataTable2 = new DataTable();
							dbDataAdapter = DBConn.GetDataAdapter(conn.con);
							dbCommand.CommandText = commandText2;
							dbDataAdapter.SelectCommand = dbCommand;
							dbDataAdapter.Fill(dataTable2);
							dbDataAdapter.Dispose();
							if (dataTable2 != null)
							{
								foreach (DataRow dataRow2 in dataTable2.Rows)
								{
									int num5 = Convert.ToInt32(dataRow2[0]);
									double num6 = Convert.ToDouble(dataRow2[1]);
									if (ht_portpw.ContainsKey(num5))
									{
										double num7 = (double)ht_portpw[num5];
										if (num6 > num7)
										{
											ht_portpw[num5] = num6;
										}
									}
									else
									{
										ht_portpw.Add(num5, num6);
									}
								}
							}
							dataTable2 = new DataTable();
						}
					}
					catch (Exception)
					{
					}
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
			}
			catch (Exception)
			{
			}
		}
		public static bool IsNumeric(string value)
		{
			string text = "0123456789";
			for (int i = 0; i < value.Length; i++)
			{
				char value2 = value[i];
				if (text.IndexOf(value2) < 0)
				{
					return false;
				}
			}
			return true;
		}
		public static List<string> GetAllTableSuffixes()
		{
			List<string> list = new List<string>();
			DBConn dBConn = null;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper() == "MYSQL")
			{
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn == null || dBConn.con == null)
					{
						return list;
					}
					DbCommand dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where table_name like '%_auto_info%' and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
						string text = Convert.ToString(dbDataReader.GetValue(0));
						text = text.Trim();
						if (text.Length > 8)
						{
							string text2 = text.Substring(text.Length - 8, 8);
							if (!list.Contains(text2) && DBTools.IsNumeric(text2))
							{
								list.Add(text2);
							}
						}
					}
					dbDataReader.Close();
					dbCommand.Dispose();
					dBConn.Close();
					return list;
				}
				catch (Exception)
				{
					if (dBConn != null)
					{
						dBConn.Close();
					}
					return list;
				}
			}
			try
			{
				string text3 = AppDomain.CurrentDomain.BaseDirectory;
				if (!text3.EndsWith("/") && !text3.EndsWith("\\"))
				{
					text3 += "\\";
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(text3 + "datadb\\");
				FileInfo[] files = directoryInfo.GetFiles("*.mdb");
				for (int i = 0; i < files.Length; i++)
				{
					FileInfo fileInfo = files[i];
					string text4 = fileInfo.FullName;
					int num = text4.LastIndexOf(".");
					if (num >= 0)
					{
						text4 = text4.Substring(0, num);
					}
					if (text4.Length >= 8)
					{
						text4 = text4.Substring(text4.Length - 8, 8);
						if (!list.Contains(text4) && DBTools.IsNumeric(text4))
						{
							list.Add(text4);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}
		public static DataTable GetOutLetPowerAndNameEx(string strgrouptype, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("power");
			dataColumn.DataType = Type.GetType("System.Double");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("server_name");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("server_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("pdu_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("device_name");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("port_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			bool b_port = false;
			if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
			{
				b_port = true;
			}
			Hashtable deviceCache = DBCache.GetDeviceCache();
			Hashtable portCache = DBCache.GetPortCache();
			List<string> list = new List<string>();
			try
			{
				DateTime dateTime = Convert.ToDateTime(strBegin);
				DateTime dateTime2 = Convert.ToDateTime(strEnd);
				List<string> allTableSuffixes = DBTools.GetAllTableSuffixes();
				DateTime t = dateTime;
				DateTime t2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 23, 59, 59);
				while (t <= t2)
				{
					string item = t.ToString("yyyyMMdd");
					if (allTableSuffixes.Contains(item) && (t.Year != dateTime2.Year || t.Month != dateTime2.Month || t.Day != dateTime2.Day || dateTime2.Hour != 0 || dateTime2.Minute != 0 || dateTime2.Second != 0))
					{
						list.Add(item);
					}
					t = t.AddDays(1.0);
				}
			}
			catch (Exception)
			{
				DataTable result = dataTable;
				return result;
			}
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return MultiThreadQuery.GetOutLetPowerAndName(strgrouptype, strBegin, strEnd, portid, invalid_device_ids);
			}
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			for (int i = 0; i < list.Count; i++)
			{
				try
				{
					string text = list[i];
					text = text.Insert(6, "-");
					text = text.Insert(4, "-");
					text += " 00:00:00";
					DateTime dt_inserttime = Convert.ToDateTime(text);
					dBConn = DBConnPool.getDynaConnection(dt_inserttime);
					if (dBConn != null && dBConn.con != null)
					{
						DBTools.GetPowerAndNameByID(ref hashtable, ref hashtable2, dBConn, b_port, strBegin, strEnd, portid, invalid_device_ids);
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
						if (dBConn != null)
						{
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
				}
				catch (Exception)
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
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
				}
			}
			if (hashtable != null && hashtable.Count > 0)
			{
				ICollection keys = hashtable.Keys;
				foreach (int num in keys)
				{
					if (deviceCache != null && deviceCache.ContainsKey(num))
					{
						DeviceInfo deviceInfo = (DeviceInfo)deviceCache[num];
						DataRow dataRow = dataTable.NewRow();
						dataRow[0] = (double)hashtable[num];
						dataRow[1] = deviceInfo.DeviceName;
						dataRow[2] = num;
						dataRow[3] = num;
						dataRow[4] = deviceInfo.DeviceName;
						dataRow[5] = 0;
						dataTable.Rows.Add(dataRow);
					}
				}
			}
			if (hashtable2 != null && hashtable2.Count > 0)
			{
				ICollection keys2 = hashtable2.Keys;
				foreach (int num2 in keys2)
				{
					if (portCache != null && portCache.ContainsKey(num2))
					{
						PortInfo portInfo = (PortInfo)portCache[num2];
						if (deviceCache != null && deviceCache.ContainsKey(portInfo.DeviceID))
						{
							DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache[portInfo.DeviceID];
							DataRow dataRow2 = dataTable.NewRow();
							dataRow2[0] = (double)hashtable2[num2];
							dataRow2[1] = deviceInfo2.DeviceName + " " + portInfo.PortName;
							dataRow2[2] = num2;
							dataRow2[3] = portInfo.DeviceID;
							dataRow2[4] = deviceInfo2.DeviceName;
							dataRow2[5] = num2;
							dataTable.Rows.Add(dataRow2);
						}
					}
				}
			}
			DataTable dataTable2 = new DataView(dataTable)
			{
				Sort = "device_name, port_id"
			}.ToTable();
			dataTable2.Columns.Remove(dataTable2.Columns["device_name"]);
			dataTable2.Columns.Remove(dataTable2.Columns["port_id"]);
			return dataTable2;
		}
		public static DataTable GetOutLetPDAndNameEx(string strgrouptype, string dblibnamePort, string dblibnameDev, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("server_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("power_consumption");
			dataColumn.DataType = Type.GetType("System.Double");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("server_name");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("pdu_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("device_name");
			dataColumn.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn);
			dataColumn = new DataColumn("port_id");
			dataColumn.DataType = Type.GetType("System.Int32");
			dataTable.Columns.Add(dataColumn);
			ArrayList listDataTable = new ArrayList();
			bool flag = false;
			if (strgrouptype == "alloutlet" || strgrouptype == "outlet")
			{
				flag = true;
			}
			Hashtable deviceCache = DBCache.GetDeviceCache();
			Hashtable portCache = DBCache.GetPortCache();
			if (deviceCache == null || deviceCache.Count == 0 || portCache == null || portCache.Count == 0)
			{
				return dataTable;
			}
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return MultiThreadQuery.GetOutLetPDAndName(strgrouptype, dblibnamePort, dblibnameDev, strBegin, strEnd, portid, invalid_device_ids);
			}
			try
			{
				List<string> list = new List<string>();
				try
				{
					List<string> allTableSuffixes = DBTools.GetAllTableSuffixes();
					DateTime dateTime = Convert.ToDateTime(strBegin);
					DateTime dateTime2 = Convert.ToDateTime(strEnd);
					DateTime t = dateTime;
					DateTime t2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 23, 59, 59);
					while (t <= t2)
					{
						string item = t.ToString("yyyyMMdd");
						if (allTableSuffixes.Contains(item) && (t.Year != dateTime2.Year || t.Month != dateTime2.Month || t.Day != dateTime2.Day || dateTime2.Hour != 0 || dateTime2.Minute != 0 || dateTime2.Second != 0))
						{
							list.Add(item);
						}
						t = t.AddDays(1.0);
					}
				}
				catch (Exception)
				{
					return dataTable;
				}
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i];
					text = text.Insert(6, "-");
					text = text.Insert(4, "-");
					text += " 00:00:00";
					DateTime dt_inserttime = Convert.ToDateTime(text);
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					try
					{
						dBConn = DBConnPool.getDynaConnection(dt_inserttime);
						string text2 = "";
						if (dBConn != null && dBConn.con != null)
						{
							string text3;
							if (dblibnamePort.IndexOf("hourly") >= 0)
							{
								text3 = "select port_id,sum(a.power_consumption) as power_consumption from " + dblibnamePort + " a";
								text2 = " group by a.port_id ";
							}
							else
							{
								text3 = "select port_id,a.power_consumption as power_consumption from " + dblibnamePort + " a";
							}
							string text4 = "";
							if (list.Count == 1)
							{
								string text5 = text4;
								text4 = string.Concat(new string[]
								{
									text5,
									" a.insert_time >= #",
									strBegin,
									"# and a.insert_time  <#",
									strEnd,
									"#"
								});
							}
							else
							{
								if (i == 0)
								{
									text4 = text4 + " a.insert_time >= #" + strBegin + "#";
								}
								else
								{
									if (i == list.Count - 1)
									{
										text4 = text4 + " a.insert_time  <#" + strEnd + "#";
									}
								}
							}
							if (!string.IsNullOrEmpty(text4))
							{
								text4 += " and ";
							}
							string text6 = text3;
							text3 = string.Concat(new string[]
							{
								text6,
								" where ",
								text4,
								" a.port_id in ({0}) ",
								text2
							});
							listDataTable = DBTools.CreateDataTable_Array(dBConn, text3, portid);
							DataTable dataTable2 = DBTools.MergeDataTable(listDataTable, "port_id");
							if (dataTable2 != null)
							{
								foreach (DataRow dataRow in dataTable2.Rows)
								{
									int num = Convert.ToInt32(dataRow[0]);
									double num2 = Convert.ToDouble(dataRow[1]);
									if (hashtable2.ContainsKey(num))
									{
										double num3 = (double)hashtable2[num];
										hashtable2[num] = num3 + num2;
									}
									else
									{
										hashtable2.Add(num, num2);
									}
								}
							}
							if (!flag)
							{
								text2 = "";
								if (dblibnameDev.IndexOf("hourly") >= 0)
								{
									text3 = "select a.device_id,sum(a.power_consumption) as power_consumption from " + dblibnameDev + " a";
									text2 = " group by a.device_id ";
								}
								else
								{
									text3 = "select a.device_id,a.power_consumption as power_consumption from " + dblibnameDev + " a";
								}
								text4 = "";
								if (list.Count == 1)
								{
									string text7 = text4;
									text4 = string.Concat(new string[]
									{
										text7,
										" a.insert_time >= #",
										strBegin,
										"# and a.insert_time  <#",
										strEnd,
										"#"
									});
								}
								else
								{
									if (i == 0)
									{
										text4 = text4 + " a.insert_time >= #" + strBegin + "#";
									}
									else
									{
										if (i == list.Count - 1)
										{
											text4 = text4 + " a.insert_time  <#" + strEnd + "#";
										}
									}
								}
								if (!string.IsNullOrEmpty(text4))
								{
									text4 += " and ";
								}
								string text8 = text3;
								text3 = string.Concat(new string[]
								{
									text8,
									" where ",
									text4,
									" a.device_id in ({0}) ",
									text2
								});
								listDataTable = DBTools.CreateDataTable_Array(dBConn, text3, invalid_device_ids);
								dataTable2 = DBTools.MergeDataTable(listDataTable, "device_id");
								if (dataTable2 != null)
								{
									foreach (DataRow dataRow2 in dataTable2.Rows)
									{
										int num4 = Convert.ToInt32(dataRow2[0]);
										double num5 = Convert.ToDouble(dataRow2[1]);
										if (hashtable.ContainsKey(num4))
										{
											double num6 = (double)hashtable[num4];
											hashtable[num4] = num6 + num5;
										}
										else
										{
											hashtable.Add(num4, num5);
										}
									}
								}
							}
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
					catch
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
						if (dBConn != null)
						{
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
				}
				ICollection keys = hashtable.Keys;
				foreach (int num7 in keys)
				{
					if (deviceCache.ContainsKey(num7))
					{
						DeviceInfo deviceInfo = (DeviceInfo)deviceCache[num7];
						DataRow dataRow3 = dataTable.NewRow();
						dataRow3[0] = num7;
						dataRow3[1] = (double)hashtable[num7];
						dataRow3[2] = deviceInfo.DeviceName;
						dataRow3[3] = num7;
						dataRow3[4] = deviceInfo.DeviceName;
						dataRow3[5] = 0;
						dataTable.Rows.Add(dataRow3);
					}
				}
				ICollection keys2 = hashtable2.Keys;
				foreach (int num8 in keys2)
				{
					if (portCache.ContainsKey(num8))
					{
						PortInfo portInfo = (PortInfo)portCache[num8];
						DataRow dataRow4 = dataTable.NewRow();
						dataRow4[0] = num8;
						dataRow4[1] = (double)hashtable2[num8];
						dataRow4[2] = portInfo.PortName;
						dataRow4[3] = portInfo.DeviceID;
						dataRow4[4] = "";
						if (deviceCache != null && deviceCache.ContainsKey(portInfo.DeviceID))
						{
							DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache[portInfo.DeviceID];
							dataRow4[4] = deviceInfo2.DeviceName;
						}
						dataRow4[5] = num8;
						dataTable.Rows.Add(dataRow4);
					}
				}
			}
			catch (Exception)
			{
			}
			DataTable dataTable3 = new DataView(dataTable)
			{
				Sort = "device_name, port_id"
			}.ToTable();
			dataTable3.Columns.Remove(dataTable3.Columns["device_name"]);
			dataTable3.Columns.Remove(dataTable3.Columns["port_id"]);
			return dataTable3;
		}
		public static DataTable GetOutLetPowerAndName(string strgrouptype, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			return DBTools.GetOutLetPowerAndNameEx(strgrouptype, strBegin, strEnd, portid, invalid_device_ids);
		}
		public static DataTable GetOutLetPDAndName(string strgrouptype, string dblibnamePort, string dblibnameDev, string strBegin, string strEnd, string portid, string invalid_device_ids)
		{
			return DBTools.GetOutLetPDAndNameEx(strgrouptype, dblibnamePort, dblibnameDev, strBegin, strEnd, portid, invalid_device_ids);
		}
		public static bool DatabaseIsReady()
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
			if (!File.Exists(text + "history.mdb"))
			{
				return true;
			}
			Hashtable historyDataStatus = DBTools.GetHistoryDataStatus();
			if (historyDataStatus != null && historyDataStatus.Count > 0)
			{
				ICollection values = historyDataStatus.Values;
				foreach (long num in values)
				{
					if (num < 1L)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}
		public static int FileCopy(string str_src, string str_dest)
		{
			int result = -1;
			try
			{
				byte[] array = new byte[33554432];
				using (Stream stream = File.Open(str_src, FileMode.Open))
				{
					using (Stream stream2 = File.Create(str_dest))
					{
						while (stream.Position < stream.Length)
						{
							int count = stream.Read(array, 0, array.Length);
							stream2.Write(array, 0, count);
						}
					}
				}
				return 1;
			}
			catch
			{
			}
			return result;
		}
		private static void CCDB()
		{
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
				Hashtable historyDataStatus = DBTools.GetHistoryDataStatus();
				if (historyDataStatus != null && historyDataStatus.Count > 0)
				{
					string text2 = AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb";
					ICollection keys = historyDataStatus.Keys;
					foreach (string text3 in keys)
					{
						long num = Convert.ToInt64(historyDataStatus[text3]);
						DateTime dateTime = DateTime.Now;
						if (text3.Length >= 15)
						{
							dateTime = Convert.ToDateTime(string.Concat(new string[]
							{
								text3.Substring(7, 4),
								"-",
								text3.Substring(11, 2),
								"-",
								text3.Substring(13, 2),
								" 00:00:00"
							}));
							if (File.Exists(text2))
							{
								string text4 = text + text3;
								if (num == -2L)
								{
									DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start copy  file " + text4);
									if (DBTools.FileCopy(text2, text4) < 0)
									{
										continue;
									}
									DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Finish copy  file " + text4);
									int num2 = DBTools.FilterData(text4, dateTime.ToString("yyyy-MM-dd"), false);
									if (num2 < 0)
									{
										continue;
									}
									num2 = DBTools.CompactAccess(text4);
									if (num2 < 0)
									{
										continue;
									}
									if (num2 > 0 && File.Exists(text4))
									{
										FileInfo fileInfo = new FileInfo(text4);
										long num3 = fileInfo.Length * 3L;
										DBTools.UpdateHistoryDataStatus(text3, string.Concat(num3));
									}
								}
								if (num == -1L)
								{
									int num4 = DBTools.FilterData(text4, dateTime.ToString("yyyy-MM-dd"), false);
									if (num4 < 0)
									{
										continue;
									}
									num4 = DBTools.CompactAccess(text4);
									if (num4 < 0)
									{
										continue;
									}
									if (num4 > 0 && File.Exists(text4))
									{
										FileInfo fileInfo2 = new FileInfo(text4);
										long num5 = fileInfo2.Length * 3L;
										DBTools.UpdateHistoryDataStatus(text3, string.Concat(num5));
									}
								}
								if (num == 0L)
								{
									int num6 = DBTools.CompactAccess(text + text3);
									if (num6 > 0 && File.Exists(text + text3))
									{
										FileInfo fileInfo3 = new FileInfo(text + text3);
										long num7 = fileInfo3.Length * 3L;
										DBTools.UpdateHistoryDataStatus(fileInfo3.Name, string.Concat(num7));
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
		}
		public static void UpgradeCheck()
		{
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return;
			}
			Thread thread = new Thread(new ThreadStart(DBTools.CCDB));
			thread.Start();
			DebugCenter.GetInstance().appendToFile("Start Check Compact file  thread");
		}
		public static int CompactAccess(string str_filename)
		{
			try
			{
				DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start compact file " + str_filename);
				Process process = new Process();
				string fileName = AppDomain.CurrentDomain.BaseDirectory + "CompactADB.exe";
				string arguments = "\"-src=" + str_filename + "\"";
				process.StartInfo = new ProcessStartInfo(fileName, arguments)
				{
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					Verb = "runas"
				};
				process.StartInfo.UseShellExecute = false;
				process.Start();
				process.WaitForExit();
				string text = process.StandardOutput.ReadToEnd();
				int result;
				if (text.IndexOf("successfully") > 0)
				{
					DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Success Finish compact file " + str_filename);
					result = 1;
					return result;
				}
				DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Failed compact file " + str_filename + "\r\n" + text);
				result = -1;
				return result;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("Compact Access is error : " + ex.Message + "\r\n" + ex.StackTrace);
			}
			return -1;
		}
		public static int FilterData(string str_datafilename, string str_datetime, bool b_thermal)
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start delete garbage data : " + str_datafilename);
			try
			{
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str_datafilename + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					oleDbCommand = oleDbConnection.CreateCommand();
					if (b_thermal)
					{
						try
						{
							oleDbCommand.CommandText = "drop table device_auto_info ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table bank_auto_info ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table port_auto_info ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table port_data_daily ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table port_data_hourly ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table bank_data_daily ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table bank_data_hourly ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table device_data_daily ";
							oleDbCommand.ExecuteNonQuery();
						}
						catch
						{
						}
						try
						{
							oleDbCommand.CommandText = "drop table device_data_hourly ";
							oleDbCommand.ExecuteNonQuery();
							goto IL_6C2;
						}
						catch
						{
							goto IL_6C2;
						}
					}
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_device_data_daily from device_data_daily where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table device_data_daily ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into device_data_daily from tmp_device_data_daily";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on device_data_daily (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on device_data_daily (device_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_device_data_daily ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_device_data_hourly from device_data_hourly where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table device_data_hourly ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into device_data_hourly from tmp_device_data_hourly";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on device_data_hourly (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on device_data_hourly (device_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_device_data_hourly ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_device_auto_info from device_auto_info where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table device_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into device_auto_info from tmp_device_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on device_auto_info (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on device_auto_info (device_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_device_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_bank_data_daily from bank_data_daily where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table bank_data_daily ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into bank_data_daily from tmp_bank_data_daily";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on bank_data_daily (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on bank_data_daily (bank_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_bank_data_daily ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_bank_data_hourly from bank_data_hourly where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table bank_data_hourly ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into bank_data_hourly from tmp_bank_data_hourly";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on bank_data_hourly (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on bank_data_hourly (bank_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_bank_data_hourly ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_bank_auto_info from bank_auto_info where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table bank_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into bank_auto_info from tmp_bank_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on bank_auto_info (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on bank_auto_info (bank_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_bank_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_port_data_daily from port_data_daily where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table port_data_daily ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into port_data_daily from tmp_port_data_daily";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on port_data_daily (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on port_data_daily (port_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_port_data_daily ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_port_data_hourly from port_data_hourly where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table port_data_hourly ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into port_data_hourly from tmp_port_data_hourly";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on port_data_hourly (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on port_data_hourly (port_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_port_data_hourly ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"select * into tmp_port_auto_info from port_auto_info where insert_time >= #",
						str_datetime,
						" 00:00:00# and insert_time <= #",
						str_datetime,
						" 23:59:59# "
					});
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "drop table port_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					oleDbCommand.CommandText = "select * into port_auto_info from tmp_port_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					try
					{
						oleDbCommand.CommandText = "create index idx1 on port_auto_info (insert_time)";
						oleDbCommand.ExecuteNonQuery();
						oleDbCommand.CommandText = "create index idx2 on port_auto_info (port_id)";
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
					}
					oleDbCommand.CommandText = "drop table tmp_port_auto_info ";
					oleDbCommand.ExecuteNonQuery();
					IL_6C2:
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
					DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Finish delete garbage data : " + str_datafilename);
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
			DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Failed delete garbage data : " + str_datafilename);
			return -1;
		}
		public static int UpdateHistoryDataStatus(string str_key, string str_value)
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
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
				string str = text + "history.mdb";
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"update compactdb set dbsize = '",
						str_value,
						"' where dbname ='",
						str_key,
						"' "
					});
					try
					{
						oleDbCommand.ExecuteNonQuery();
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
		public static int InsertHistoryDataStatus(string str_key, string str_value)
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
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
				string str = text + "history.mdb";
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = string.Concat(new string[]
					{
						"insert into compactdb (dbname,dbsize) values ('",
						str_key,
						"','",
						str_value,
						"') "
					});
					try
					{
						oleDbCommand.ExecuteNonQuery();
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
		public static Hashtable GetHistoryDataStatus()
		{
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			OleDbDataAdapter oleDbDataAdapter = null;
			Hashtable hashtable = new Hashtable();
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
					Hashtable result = hashtable;
					return result;
				}
				oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text2 + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					DataTable dataTable = new DataTable();
					oleDbDataAdapter = new OleDbDataAdapter();
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = "select dbname,dbsize from compactdb ";
					oleDbDataAdapter.SelectCommand = oleDbCommand;
					oleDbDataAdapter.Fill(dataTable);
					oleDbDataAdapter.Dispose();
					if (dataTable != null)
					{
						hashtable = new Hashtable();
						foreach (DataRow dataRow in dataTable.Rows)
						{
							string key = Convert.ToString(dataRow[0]);
							long num = Convert.ToInt64(dataRow[1]);
							if (hashtable.ContainsKey(key))
							{
								hashtable[key] = num;
							}
							else
							{
								hashtable.Add(key, num);
							}
						}
					}
					dataTable = new DataTable();
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~GetHistoryDataStatus Error : " + ex.Message + "\n" + ex.StackTrace);
				Hashtable result = hashtable;
				return result;
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
		public static long CalculateMU()
		{
			long result = -1L;
			Hashtable deviceCache = DBCache.GetDeviceCache();
			Hashtable bankCache = DBCache.GetBankCache();
			Hashtable portCache = DBCache.GetPortCache();
			if (deviceCache != null && bankCache != null && portCache != null)
			{
				result = (long)(deviceCache.Count + bankCache.Count + portCache.Count);
			}
			return result;
		}
		public static DateTime GetDataBeginDate()
		{
			DateTime result = DateTime.Now;
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				DBConn dynaConnection = DBConnPool.getDynaConnection();
				DbCommand dbCommand = new OleDbCommand();
				try
				{
					if (dynaConnection != null && dynaConnection.con != null)
					{
						DataTable dataTable = new DataTable();
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dynaConnection.con);
						dbCommand = dynaConnection.con.CreateCommand();
						dbCommand.CommandType = CommandType.Text;
						if (DBUrl.SERVERMODE)
						{
							dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where ( table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%' ) and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						}
						else
						{
							dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where ( table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%') and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						}
						dataAdapter.SelectCommand = dbCommand;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								string text = Convert.ToString(dataRow[0]);
								if (text.Length >= 22)
								{
									try
									{
										string value = text.Substring(text.Length - 8, 4);
										string value2 = text.Substring(text.Length - 4, 2);
										string value3 = text.Substring(text.Length - 2, 2);
										DateTime dateTime = new DateTime(Convert.ToInt32(value), Convert.ToInt32(value2), Convert.ToInt32(value3), 0, 0, 0);
										if (result.CompareTo(dateTime) > 0)
										{
											result = dateTime;
										}
									}
									catch
									{
									}
								}
							}
						}
						dataTable = new DataTable();
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						if (dynaConnection != null)
						{
							dynaConnection.close();
						}
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						if (dynaConnection != null)
						{
							dynaConnection.close();
						}
					}
					catch
					{
					}
				}
				return result;
			}
			List<DateTime> dataDBFileNameList = AccessDBUpdate.GetDataDBFileNameList();
			if (dataDBFileNameList != null && dataDBFileNameList.Count > 0)
			{
				foreach (DateTime current in dataDBFileNameList)
				{
					if (result.CompareTo(current) > 0)
					{
						result = current;
					}
				}
			}
			return result;
		}
		public static int CalculateInterval()
		{
			int num = -1;
			long mU = DBCache.GetMU();
			DBTools.DT_FIRST = DBTools.GetDataBeginDate();
			bool flag = false;
			if (Sys_Para.GetDBOpt_keepMMflag() == 1)
			{
				flag = true;
			}
			int num2 = 36;
			if (flag)
			{
				num2 = Sys_Para.GetDBOpt_keepMM();
			}
			int num3 = num2 * 30 - Math.Abs((DateTime.Now - DBTools.DT_FIRST).Days);
			if (num3 <= 0)
			{
				num3 = 1;
			}
			if (DBUrl.SERVERMODE)
			{
				if (!Backuptask.IsSameServer())
				{
					goto IL_372;
				}
				try
				{
					if (mU > 0L)
					{
						double num4 = (double)((long)num3 * mU * 45L * 24L);
						string dB_LOCATION = DBUrl.DB_LOCATION;
						DiskInfo diskInfo = new DiskInfo(dB_LOCATION);
						double num5 = (double)diskInfo.DiskAvailableFreeSpace();
						num4 = num5 / num4;
						double value = 3600.0 / num4;
						num = Convert.ToInt32(value);
					}
					goto IL_372;
				}
				catch
				{
					goto IL_372;
				}
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				if (DBMaintain.IsLocalIP(DBUrl.CURRENT_HOST_PATH))
				{
					try
					{
						if (mU > 0L)
						{
							double num6 = (double)((long)num3 * mU * 45L * 24L);
							string dB_LOCATION2 = DBUrl.DB_LOCATION;
							DiskInfo diskInfo2 = new DiskInfo(dB_LOCATION2);
							double num7 = (double)diskInfo2.DiskAvailableFreeSpace();
							num6 = num7 / num6;
							double value2 = 3600.0 / num6;
							num = Convert.ToInt32(value2);
						}
						goto IL_372;
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
						{
							"MU is : ",
							mU,
							" i_days : ",
							num3,
							"\r\n",
							ex.Message,
							"\r\n",
							ex.StackTrace
						}));
						goto IL_372;
					}
				}
				try
				{
					if (mU > 0L)
					{
						double num8 = (double)((long)num3 * mU * 45L * 24L);
						long[] dBSpaceSize = DBTools.GetDBSpaceSize();
						double num9;
						if (dBSpaceSize.Length > 2 && dBSpaceSize[2] > 0L)
						{
							num9 = 322122547200.0 - (double)dBSpaceSize[2];
							if (num9 < 0.0)
							{
								num9 = 107374182400.0;
							}
						}
						else
						{
							num9 = 107374182400.0;
						}
						num8 = num9 / num8;
						double value3 = 3600.0 / num8;
						num = Convert.ToInt32(value3);
					}
					goto IL_372;
				}
				catch (Exception ex2)
				{
					DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
					{
						"MU is : ",
						mU,
						" i_days : ",
						num3,
						"\r\n",
						ex2.Message,
						"\r\n",
						ex2.StackTrace
					}));
					goto IL_372;
				}
			}
			try
			{
				if (mU > 0L)
				{
					double num10 = 2147483648.0 / (double)(mU * 100L * 24L);
					double num11 = (double)((long)num3 * mU * 100L * 24L);
					string dB_LOCATION3 = DBUrl.DB_LOCATION;
					DiskInfo diskInfo3 = new DiskInfo(dB_LOCATION3);
					double num12 = (double)diskInfo3.DiskAvailableFreeSpace();
					num11 = num12 / num11;
					if (num11 > num10)
					{
						num11 = num10;
					}
					double value4 = 3600.0 / num11;
					num = Convert.ToInt32(value4);
				}
			}
			catch (Exception ex3)
			{
				DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
				{
					"MU is : ",
					mU,
					" i_days : ",
					num3,
					"\r\n",
					ex3.Message,
					"\r\n",
					ex3.StackTrace
				}));
			}
			IL_372:
			if (num < 0)
			{
				num = 900;
			}
			if (num < 60)
			{
				num = 60;
			}
			if (num > 900)
			{
				num = 900;
			}
			DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
			{
				"MU is : ",
				mU,
				" Interval : ",
				num
			}));
			return num;
		}
		public static void CleanupDatabase()
		{
			DBTools.Cleanup();
		}
		private static DateTime getBeginTime()
		{
			DateTime now = DateTime.Now;
			try
			{
				DBConn dynaConnection = DBConnPool.getDynaConnection();
				if (dynaConnection.con != null)
				{
					DbCommand commandObject = DBConn.GetCommandObject(dynaConnection.con);
					commandObject.CommandText = "select min(insert_time) from device_auto_info ";
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return now;
		}
		public static long Cleanup()
		{
			long result = 0L;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			DateTime now = DateTime.Now;
			DateTime now2 = DateTime.Now;
			DateTime dateTime = DateTime.Now;
			DateTime dateTime2 = DateTime.Now;
			try
			{
				DateTime dateTime3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 30, 0);
				if (DateTime.Now.CompareTo(dateTime3) < 0)
				{
					long result2 = 0L;
					return result2;
				}
				if (Math.Abs((DateTime.Now - dateTime3).TotalSeconds) > 60.0)
				{
					long result2 = 0L;
					return result2;
				}
				if (DBTools.DT_TODAYTASK.CompareTo(dateTime3) > 0)
				{
					long result2 = 0L;
					return result2;
				}
				DBTools.DT_TODAYTASK = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 5, 30, 0);
				try
				{
					long mU = DBTools.CalculateMU();
					DBCache.SetMU(mU);
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("Calculate MU Error : \r\n" + ex.Message + "\n" + ex.StackTrace);
				}
				try
				{
					int interval = DBTools.CalculateInterval();
					DBCache.SetInterval(interval);
				}
				catch (Exception ex2)
				{
					DebugCenter.GetInstance().appendToFile("Calculate Interval Error : \r\n" + ex2.Message + "\n" + ex2.StackTrace);
				}
				bool flag4 = false;
				bool flag5 = false;
				if (Sys_Para.GetDBOpt_keepMMflag() == 1)
				{
					flag5 = true;
				}
				if (Sys_Para.GetDBOpt_deloldflag() == 1)
				{
					flag4 = true;
				}
				long mU2 = DBCache.GetMU();
				if (flag5 || flag4)
				{
					dateTime = DBTools.GetDataBeginDate();
				}
				if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					bool b_mysql = true;
					if (flag5)
					{
						DBTools.DeleteData(Sys_Para.GetDBOpt_keepMM(), b_mysql, ref flag2, ref now);
					}
					if (flag4 && DBMaintain.IsLocalIP(DBUrl.CURRENT_HOST_PATH) && (!DBUrl.SERVERMODE || Backuptask.IsSameServer()))
					{
						string dB_LOCATION = DBUrl.DB_LOCATION;
						DiskInfo diskInfo = new DiskInfo(dB_LOCATION);
						long num = diskInfo.DiskAvailableFreeSpace();
						long num2 = 3600L * mU2 * 45L * 24L / (long)DBCache.GetInterval();
						while (num < num2)
						{
							int num3 = DBTools.DeleteDataOneDay(b_mysql, ref flag3, ref now2);
							if (num3 < 1)
							{
								break;
							}
							num = diskInfo.DiskAvailableFreeSpace();
						}
					}
				}
				else
				{
					bool b_mysql = false;
					if (flag5)
					{
						DBTools.DeleteData(Sys_Para.GetDBOpt_keepMM(), b_mysql, ref flag2, ref now);
					}
					if (flag4 && (!DBUrl.SERVERMODE || Backuptask.IsSameServer()))
					{
						string dB_LOCATION2 = DBUrl.DB_LOCATION;
						DiskInfo diskInfo2 = new DiskInfo(dB_LOCATION2);
						long num4 = diskInfo2.DiskAvailableFreeSpace();
						long num5 = 3600L * mU2 * 100L * 24L / (long)DBCache.GetInterval();
						while (num4 < num5)
						{
							int num6 = DBTools.DeleteDataOneDay(b_mysql, ref flag3, ref now2);
							if (num6 < 1)
							{
								break;
							}
							num4 = diskInfo2.DiskAvailableFreeSpace();
						}
					}
				}
				if (flag2)
				{
					dateTime2 = now;
					flag = true;
				}
				if (flag3)
				{
					dateTime2 = now2;
					flag = true;
				}
				if (flag)
				{
					string text = dateTime.ToString("yyyy-MM-dd") + " To " + dateTime2.ToString("yyyy-MM-dd");
					if (DBTools.DWL != null)
					{
						DBTools.DWL("0120063", new string[]
						{
							text
						});
					}
				}
				if (!DBUrl.SERVERMODE && DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("ACCESS"))
				{
					CompactDBThread.CheckDBFile();
					CompactDBThread @object = new CompactDBThread();
					try
					{
						Thread thread = new Thread(new ThreadStart(@object.ComPactDBFile));
						thread.Start();
					}
					catch (ThreadStateException ex3)
					{
						DebugCenter.GetInstance().appendToFile(ex3.Message + "\n" + ex3.StackTrace);
					}
				}
				DeviceOperation.CleanupGP();
			}
			catch (Exception ex4)
			{
				DebugCenter.GetInstance().appendToFile("Clean up database error : " + ex4.Message + "\n" + ex4.StackTrace);
			}
			return result;
		}
		public static int ShudownAccess()
		{
			try
			{
				DBTools.KillProcess("EcoSensors");
				InterProcessEvent.setGlobalEvent("Database_Maintaining", true);
				WorkQueue<string> instance = WorkQueue<string>.getInstance();
				WorkQueue<string> instance_pd = WorkQueue<string>.getInstance_pd();
				WorkQueue<string> instance_rackeffect = WorkQueue<string>.getInstance_rackeffect();
				TaskStatus.SetDBStatus(-1);
				int num = instance.CloseDBConnection();
				int num2 = instance_pd.CloseDBConnection();
				int num3 = instance_rackeffect.CloseDBConnection();
				int num4 = DBConnPool.CloseAllConnection();
				if (num > 0 && num2 > 0 && num3 > 0 && num4 > 0)
				{
					return 1;
				}
				InterProcessEvent.setGlobalEvent("Database_Maintaining", false);
				TaskStatus.SetDBStatus(1);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			InterProcessEvent.setGlobalEvent("Database_Maintaining", false);
			return -1;
		}
		public static int DeleteDataOneDay(bool b_mysql, ref bool b_log, ref DateTime dtime_to)
		{
			DBTools.DT_FIRST = DBTools.GetDataBeginDate();
			DateTime dT_FIRST = DBTools.DT_FIRST;
			if (Math.Abs((DateTime.Now - dT_FIRST).Days) < 1)
			{
				return 0;
			}
			DateTime dateTime = dT_FIRST.AddDays(1.0);
			DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
			int num = -1;
			if (b_mysql)
			{
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						string[] array = new string[]
						{
							"rci_daily",
							"rci_hourly"
						};
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text = array2[i];
							try
							{
								dbCommand.CommandText = "lock tables " + text + " write ";
								dbCommand.ExecuteNonQuery();
								dbCommand.CommandText = string.Concat(new string[]
								{
									"delete from ",
									text,
									" where insert_time < '",
									dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
									"' "
								});
								int num2 = dbCommand.ExecuteNonQuery();
								num += num2;
								if (num2 > 0)
								{
									b_log = true;
									dtime_to = dateTime2;
								}
								DateTime now = DateTime.Now;
								DateTime.Now - now;
								dbCommand.CommandText = "unlock tables";
								dbCommand.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								try
								{
									dbCommand.CommandText = "unlock tables";
									dbCommand.ExecuteNonQuery();
								}
								catch
								{
								}
								DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
								{
									"SQL Command is : ",
									dbCommand.CommandText,
									"\r\n DBERROR~~~~~~~~~~~DBERROR : ",
									ex.Message,
									"\n",
									ex.StackTrace
								}));
							}
						}
						try
						{
							string str = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table rackthermal_daily" + str;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str2 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table rackthermal_hourly" + str2;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str3 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table device_data_daily" + str3;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str4 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table device_data_hourly" + str4;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str5 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table bank_data_daily" + str5;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str6 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table bank_data_hourly" + str6;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str7 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table port_data_daily" + str7;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str8 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table port_data_hourly" + str8;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str9 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table device_auto_info" + str9;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str10 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table bank_auto_info" + str10;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							string str11 = dT_FIRST.ToString("yyyyMMdd");
							dbCommand.CommandText = "drop table port_auto_info" + str11;
							dbCommand.ExecuteNonQuery();
							b_log = true;
							dtime_to = dateTime2;
						}
						catch
						{
						}
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						try
						{
							if (dBConn != null)
							{
								dBConn.close();
							}
						}
						catch
						{
						}
					}
					return num;
				}
				catch (Exception ex2)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						if (dBConn != null)
						{
							dBConn.close();
						}
					}
					catch
					{
					}
					return num;
				}
			}
			string text2 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			if (Directory.Exists(text2))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text2);
				FileInfo[] files = directoryInfo.GetFiles();
				if (files.Length != 0)
				{
					FileInfo[] array3 = files;
					for (int j = 0; j < array3.Length; j++)
					{
						FileInfo fileInfo = array3[j];
						if (fileInfo.Name.IndexOf("datadb") == 0 && fileInfo.Extension.ToLower().Equals(".mdb"))
						{
							try
							{
								string name = fileInfo.Name;
								if (name.Length >= 15 && Convert.ToDateTime(string.Concat(new string[]
								{
									name.Substring(7, 4),
									"-",
									name.Substring(11, 2),
									"-",
									name.Substring(13, 2),
									" 00:00:00"
								})).CompareTo(dateTime2) < 0)
								{
									fileInfo.Delete();
									num += 10000;
									b_log = true;
									dtime_to = dateTime2;
								}
							}
							catch (Exception ex3)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~GetDataDBFileNameList Error : " + ex3.Message + "\n" + ex3.StackTrace);
							}
						}
					}
					return num;
				}
			}
			return num;
		}
		public static int DeleteData(int i_month, bool b_mysql, ref bool b_log, ref DateTime dtime_to)
		{
			DateTime dateTime = DateTime.Now.AddMonths(-i_month);
			DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
			dtime_to = dateTime2;
			int num = 0;
			if (b_mysql)
			{
				DBConn dynaConnection = DBConnPool.getDynaConnection();
				DbCommand dbCommand = new OleDbCommand();
				try
				{
					if (dynaConnection != null)
					{
						dbCommand = DBConn.GetCommandObject(dynaConnection.con);
						dbCommand.CommandType = CommandType.Text;
						DataTable dataTable = new DataTable();
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dynaConnection.con);
						dbCommand = dynaConnection.con.CreateCommand();
						dbCommand.CommandType = CommandType.Text;
						if (DBUrl.SERVERMODE)
						{
							dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where ( table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%' or table_name like 'rackthermal_hourly20%' or table_name like 'rackthermal_daily20%' ) and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						}
						else
						{
							dbCommand.CommandText = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES where ( table_name like '%_auto_info%' or table_name like '%_data_daily%' or table_name like '%_data_hourly%' or table_name like 'rackthermal_hourly20%' or table_name like 'rackthermal_daily20%' ) and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						}
						dataAdapter.SelectCommand = dbCommand;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								try
								{
									string text = Convert.ToString(dataRow[0]);
									if (text.Length > 8)
									{
										string value = text.Substring(text.Length - 8, 4);
										string value2 = text.Substring(text.Length - 4, 2);
										string value3 = text.Substring(text.Length - 2, 2);
										try
										{
											DateTime dateTime3 = new DateTime(Convert.ToInt32(value), Convert.ToInt32(value2), Convert.ToInt32(value3), 0, 0, 0);
											if (dateTime3.CompareTo(dateTime2) < 0)
											{
												dbCommand.CommandText = "DROP TABLE " + text;
												dbCommand.ExecuteNonQuery();
												b_log = true;
											}
										}
										catch
										{
										}
									}
								}
								catch
								{
								}
							}
						}
						dataTable = new DataTable();
						string[] array = new string[]
						{
							"rci_daily",
							"rci_hourly"
						};
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text2 = array2[i];
							try
							{
								dbCommand.CommandText = "lock tables " + text2 + " write ";
								try
								{
									dbCommand.ExecuteNonQuery();
								}
								catch
								{
									try
									{
										dbCommand.CommandText = "unlock tables";
										dbCommand.ExecuteNonQuery();
									}
									catch
									{
									}
								}
								dbCommand.CommandText = string.Concat(new string[]
								{
									"delete from ",
									text2,
									" where insert_time < '",
									dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
									"' "
								});
								try
								{
									int num2 = dbCommand.ExecuteNonQuery();
									num += num2;
								}
								catch
								{
									try
									{
										dbCommand.CommandText = "unlock tables";
										dbCommand.ExecuteNonQuery();
									}
									catch
									{
									}
								}
								dbCommand.CommandText = "unlock tables";
								dbCommand.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
								{
									"SQL Command is : ",
									dbCommand.CommandText,
									"\r\n DBERROR~~~~~~~~~~~DBERROR : ",
									ex.Message,
									"\n",
									ex.StackTrace
								}));
							}
						}
						try
						{
							dbCommand.Dispose();
						}
						catch
						{
						}
						try
						{
							if (dynaConnection != null)
							{
								dynaConnection.close();
							}
						}
						catch
						{
						}
						if (num > 0)
						{
							dtime_to = dateTime2;
							b_log = true;
						}
						return num;
					}
					return -1;
				}
				catch (Exception ex2)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex2.Message + "\n" + ex2.StackTrace);
					try
					{
						dbCommand.Dispose();
					}
					catch
					{
					}
					try
					{
						if (dynaConnection != null)
						{
							dynaConnection.close();
						}
					}
					catch
					{
					}
					return -1;
				}
			}
			string text3 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
			{
				text3 += Path.DirectorySeparatorChar;
			}
			if (Directory.Exists(text3))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text3);
				FileInfo[] files = directoryInfo.GetFiles();
				if (files.Length != 0)
				{
					FileInfo[] array3 = files;
					for (int j = 0; j < array3.Length; j++)
					{
						FileInfo fileInfo = array3[j];
						if (fileInfo.Name.IndexOf("datadb") == 0 && fileInfo.Extension.ToLower().Equals(".mdb"))
						{
							try
							{
								string name = fileInfo.Name;
								if (name.Length >= 15 && Convert.ToDateTime(string.Concat(new string[]
								{
									name.Substring(7, 4),
									"-",
									name.Substring(11, 2),
									"-",
									name.Substring(13, 2),
									" 00:00:00"
								})).CompareTo(dateTime2) < 0)
								{
									fileInfo.Delete();
									num += 10000;
									b_log = true;
									dtime_to = dateTime2;
								}
							}
							catch (Exception ex3)
							{
								DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~GetDataDBFileNameList Error : " + ex3.Message + "\n" + ex3.StackTrace);
							}
						}
					}
					return num;
				}
			}
			return -1;
		}
		public static void KillProcess(string str_pname)
		{
			Process[] processesByName = Process.GetProcessesByName(str_pname);
			Process[] array = processesByName;
			for (int i = 0; i < array.Length; i++)
			{
				Process process = array[i];
				try
				{
					process.Kill();
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~~~~ KillProcess error : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}
		public static string BackupConfiguration4ServerMode()
		{
			string text = "";
			string text2 = "";
			Random random = new Random((int)DateTime.Now.Ticks);
			try
			{
				random.Next(1, 60);
				string arg = DateTime.Now.ToString("yyyyMMddHHmmss");
				string str = "DBbackuptmp" + arg + random.Next(1, 60);
				text = AppDomain.CurrentDomain.BaseDirectory;
				text2 = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
					text2 += Path.DirectorySeparatorChar;
				}
				DirectorySecurity directorySecurity = new DirectorySecurity();
				directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
				Directory.SetAccessControl(text, directorySecurity);
				Directory.SetAccessControl(text2, directorySecurity);
				text += str;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				else
				{
					DBTools.DeleteDir(text);
					Directory.CreateDirectory(text);
				}
				Directory.SetAccessControl(text, directorySecurity);
			}
			catch (Exception)
			{
				DBTools.DeleteDir(text);
				string result = "";
				return result;
			}
			bool flag = false;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "lock tables backuptask read,bank_info read,cleanupsetting read,data_group read,device_addr_info read,device_base_info read,device_sensor_info read,event_info read,gatewaytable read,group_detail read,group_member read,groupcontroltask read,port_info read,smtpsetting read,snmpsetting read,sys_para read,sys_users read,ugp read,systemparameter read,taskschedule read,zone_info read ";
					dbCommand.ExecuteNonQuery();
					flag = true;
					string str2 = text.Replace("\\", "\\\\");
					dbCommand.CommandText = "select * from backuptask into outfile '" + str2 + "backtask.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from bank_info into outfile '" + str2 + "bank_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from cleanupsetting into outfile '" + str2 + "cleanupsetting.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from data_group into outfile '" + str2 + "data_group.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from device_addr_info into outfile '" + str2 + "device_addr_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from device_base_info into outfile '" + str2 + "device_base_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from device_sensor_info into outfile '" + str2 + "device_sensor_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from event_info into outfile '" + str2 + "event_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from gatewaytable into outfile '" + str2 + "gatewaytable.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from group_detail into outfile '" + str2 + "group_detail.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from group_member into outfile '" + str2 + "group_member.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from groupcontroltask into outfile '" + str2 + "groupcontroltask.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from port_info into outfile '" + str2 + "port_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from smtpsetting into outfile '" + str2 + "smtpsetting.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from snmpsetting into outfile '" + str2 + "snmpsetting.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from sys_para into outfile '" + str2 + "sys_para.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from sys_users into outfile '" + str2 + "sys_users.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from ugp into outfile '" + str2 + "ugp.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from systemparameter into outfile '" + str2 + "systemparameter.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from taskschedule into outfile '" + str2 + "taskschedule.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "select * from zone_info into outfile '" + str2 + "zone_info.csv' fields terminated by ',' ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "unlock tables";
					dbCommand.ExecuteNonQuery();
				}
				dBConn.Close();
			}
			catch (Exception)
			{
				if (dBConn != null && dBConn.con != null)
				{
					if (dbCommand != null && flag)
					{
						try
						{
							dbCommand.CommandText = "unlock tables ";
							dbCommand.ExecuteNonQuery();
						}
						catch (Exception)
						{
						}
					}
					dBConn.Close();
				}
				DBTools.DeleteDir(text);
				string result = "";
				return result;
			}
			string[] files = Directory.GetFiles(text);
			string text3 = text2 + ("tmpzip" + random.Next(1, 1000)) + "." + "zip";
			try
			{
				using (ZipArchive zipArchive = ZipArchive.CreateZipFile(text3))
				{
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string text4 = array[i];
						FileInfo fileInfo = new FileInfo(text4);
						int startIndex = Path.GetDirectoryName(text4).Length + 1;
						using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
						{
							using (Stream stream = zipArchive.AddFile(fileInfo.FullName.Substring(startIndex)).SetStream())
							{
								byte[] array2 = new byte[67108863];
								for (int j = fileStream.Read(array2, 0, array2.Length); j > 0; j = fileStream.Read(array2, 0, array2.Length))
								{
									stream.Write(array2, 0, j);
								}
								stream.Flush();
								stream.Close();
							}
						}
					}
				}
			}
			catch (Exception)
			{
				try
				{
					DBTools.DeleteDir(text);
					if (File.Exists(text3))
					{
						File.Delete(text3);
					}
				}
				catch (Exception)
				{
				}
				string result = "";
				return result;
			}
			string text5 = text2 + ("tmpdat" + random.Next(1000, 2000)) + "." + "dat";
			try
			{
				if (File.Exists(text3))
				{
					AESEncryptionUtility.Encrypt(text3, text5, DBUrl.SERVERID);
				}
			}
			catch (Exception)
			{
				try
				{
					DBTools.DeleteDir(text);
					if (File.Exists(text3))
					{
						File.Delete(text3);
					}
					if (File.Exists(text5))
					{
						File.Delete(text5);
					}
				}
				catch (Exception)
				{
				}
				string result = "";
				return result;
			}
			DBTools.DeleteDir(text);
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			return text5;
		}
		public static int RestoreConfiguration4ServerMode(string str_backupfile)
		{
			if (!File.Exists(str_backupfile))
			{
				return -1;
			}
			string text = AppDomain.CurrentDomain.BaseDirectory;
			string text2 = AppDomain.CurrentDomain.BaseDirectory;
			Random random = new Random((int)DateTime.Now.Ticks);
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
				text2 += Path.DirectorySeparatorChar;
			}
			try
			{
				random.Next(1, 60);
				string arg = DateTime.Now.ToString("yyyyMMddHHmmss");
				string str = "Uncompacttmp" + arg + random.Next(1, 60);
				DirectorySecurity directorySecurity = new DirectorySecurity();
				directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
				Directory.SetAccessControl(text2, directorySecurity);
				Directory.SetAccessControl(text, directorySecurity);
				text2 += str;
				if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
				{
					text2 += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				else
				{
					DBTools.DeleteDir(text2);
					Directory.CreateDirectory(text2);
				}
				Directory.SetAccessControl(text2, directorySecurity);
			}
			catch (Exception)
			{
				DBTools.DeleteDir(text2);
				int result = -2;
				return result;
			}
			string text3 = text + random.Next(3000, 4000) + "backuptmp.zip";
			try
			{
				AESEncryptionUtility.Decrypt(str_backupfile, text3, DBUrl.SERVERID);
			}
			catch (Exception)
			{
				DBTools.DeleteDir(text2);
				if (File.Exists(text3))
				{
					File.Delete(text3);
				}
				int result = -3;
				return result;
			}
			if (!File.Exists(text3))
			{
				DBTools.DeleteDir(text2);
				return -3;
			}
			try
			{
				using (ZipArchive zipArchive = ZipArchive.OpenOnFile(text3))
				{
					string text4 = text2;
					foreach (ZipArchive.ZipFileInfo current in zipArchive.Files)
					{
						if (!current.FolderFlag)
						{
							using (Stream stream = current.GetStream())
							{
								string path = text4 + Path.GetDirectoryName(current.Name) + Path.DirectorySeparatorChar;
								if (!Directory.Exists(path))
								{
									Directory.CreateDirectory(path);
								}
								using (FileStream fileStream = new FileStream(text4 + current.Name, FileMode.Create))
								{
									byte[] array = new byte[67108863];
									for (int i = stream.Read(array, 0, array.Length); i > 0; i = stream.Read(array, 0, array.Length))
									{
										fileStream.Write(array, 0, i);
									}
									fileStream.Flush();
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
				DBTools.DeleteDir(text2);
				File.Delete(text3);
				int result = -4;
				return result;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					string str2 = text2.Replace("\\", "\\\\");
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandText = "lock tables backuptask write,bank_info write,cleanupsetting write,data_group write,device_addr_info write,device_base_info write,device_sensor_info write,event_info write,gatewaytable write,group_detail write,group_member write,groupcontroltask write,port_info write,smtpsetting write,snmpsetting write,sys_para write,sys_users write,ugp write,systemparameter write,taskschedule write,zone_info write ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "truncate table port_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "port_info.csv") && new FileInfo(text2 + "port_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "port_info.csv' REPLACE INTO TABLE port_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table backuptask ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "backuptask.csv") && new FileInfo(text2 + "backuptask.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "backuptask.csv' REPLACE INTO TABLE backuptask fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table bank_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "bank_info.csv") && new FileInfo(text2 + "bank_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "bank_info.csv' REPLACE INTO TABLE bank_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table cleanupsetting ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "cleanupsetting.csv") && new FileInfo(text2 + "cleanupsetting.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "cleanupsetting.csv' REPLACE INTO TABLE cleanupsetting fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table data_group ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "data_group.csv") && new FileInfo(text2 + "data_group.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "data_group.csv' REPLACE INTO TABLE data_group fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table device_addr_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "device_addr_info.csv") && new FileInfo(text2 + "device_addr_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "device_addr_info.csv' REPLACE INTO TABLE device_addr_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table device_base_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "device_base_info.csv") && new FileInfo(text2 + "device_base_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "device_base_info.csv' REPLACE INTO TABLE device_base_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table device_sensor_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "device_sensor_info.csv") && new FileInfo(text2 + "device_sensor_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "device_sensor_info.csv' REPLACE INTO TABLE device_sensor_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table event_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "event_info.csv") && new FileInfo(text2 + "event_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "event_info.csv' REPLACE INTO TABLE event_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table gatewaytable ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "gatewaytable.csv") && new FileInfo(text2 + "gatewaytable.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "gatewaytable.csv' REPLACE INTO TABLE gatewaytable fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table group_detail ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "group_detail.csv") && new FileInfo(text2 + "group_detail.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "group_detail.csv' REPLACE INTO TABLE group_detail fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table group_member ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "group_member.csv") && new FileInfo(text2 + "group_member.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "group_member.csv' REPLACE INTO TABLE group_member fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table groupcontroltask ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "groupcontroltask.csv") && new FileInfo(text2 + "groupcontroltask.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "groupcontroltask.csv' REPLACE INTO TABLE groupcontroltask fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table smtpsetting ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "smtpsetting.csv") && new FileInfo(text2 + "smtpsetting.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "smtpsetting.csv' REPLACE INTO TABLE smtpsetting fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table snmpsetting ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "snmpsetting.csv") && new FileInfo(text2 + "snmpsetting.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "snmpsetting.csv' REPLACE INTO TABLE snmpsetting fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table sys_para ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "sys_para.csv") && new FileInfo(text2 + "sys_para.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "sys_para.csv' REPLACE INTO TABLE sys_para fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table sys_users ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "sys_users.csv") && new FileInfo(text2 + "sys_users.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "sys_users.csv' REPLACE INTO TABLE sys_users fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table ugp ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "ugp.csv") && new FileInfo(text2 + "ugp.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "ugp.csv' REPLACE INTO TABLE ugp fields terminated by ',' ";
					}
					dbCommand.CommandText = "truncate table systemparameter ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "systemparameter.csv") && new FileInfo(text2 + "systemparameter.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "systemparameter.csv' REPLACE INTO TABLE systemparameter fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table taskschedule ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "taskschedule.csv") && new FileInfo(text2 + "taskschedule.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "taskschedule.csv' REPLACE INTO TABLE taskschedule fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "truncate table zone_info ";
					dbCommand.ExecuteNonQuery();
					if (File.Exists(text2 + "zone_info.csv") && new FileInfo(text2 + "zone_info.csv").Length > 0L)
					{
						dbCommand.CommandText = "LOAD DATA LOCAL INFILE '" + str2 + "zone_info.csv' REPLACE INTO TABLE zone_info fields terminated by ',' ";
						dbCommand.ExecuteNonQuery();
					}
					dbCommand.CommandText = "update device_base_info set restoreflag = 1 ";
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "unlock tables";
					dbCommand.ExecuteNonQuery();
				}
				dBConn.Close();
				DBTools.DeleteDir(text2);
				File.Delete(text3);
				int result = 1;
				return result;
			}
			catch (Exception)
			{
				if (dBConn != null && dBConn.con != null)
				{
					if (dbCommand != null)
					{
						try
						{
							dbCommand.CommandText = "unlock tables ";
							dbCommand.ExecuteNonQuery();
						}
						catch (Exception)
						{
						}
					}
					dBConn.Close();
				}
			}
			DBTools.DeleteDir(text2);
			File.Delete(text3);
			return -4;
		}
		public static long EvaluateCount()
		{
			long num = 0L;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				DataTable dataTable = new DataTable();
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "SELECT table_rows FROM INFORMATION_SCHEMA.TABLES where (table_name like '%_auto_info%' or table_name like '%_daily%' or table_name like '%_hourly%' ) and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dataAdapter.SelectCommand = dbCommand;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								try
								{
									long num2 = Convert.ToInt64(dataRow[0]);
									num += num2;
								}
								catch
								{
								}
							}
						}
						dataTable = new DataTable();
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
						if (dBConn != null)
						{
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
					goto IL_1B1;
				}
				catch (Exception)
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
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					goto IL_1B1;
				}
			}
			long num3 = 1L;
			Hashtable deviceCache = DBCache.GetDeviceCache();
			Hashtable bankCache = DBCache.GetBankCache();
			Hashtable portCache = DBCache.GetPortCache();
			if (deviceCache != null && bankCache != null && portCache != null)
			{
				num3 = (long)(deviceCache.Count + bankCache.Count + portCache.Count);
			}
			List<DateTime> dataDBFileNameList = AccessDBUpdate.GetDataDBFileNameList();
			long num4 = 1L;
			if (dataDBFileNameList != null)
			{
				num4 = (long)dataDBFileNameList.Count;
			}
			num += num3 * num4 * 25L;
			long num5 = (long)DBCache.GetInterval();
			if (num5 > 0L)
			{
				num += num3 * num4 * (86400L / num5);
			}
			IL_1B1:
			if (num > 0L)
			{
				return num;
			}
			return 1L;
		}
		public static long EvaluateTime()
		{
			long num = 0L;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				DataTable dataTable = new DataTable();
				try
				{
					dBConn = DBConnPool.getDynaConnection();
					if (dBConn != null && dBConn.con != null)
					{
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "SELECT table_rows FROM INFORMATION_SCHEMA.TABLES where (table_name like '%_auto_info%' or table_name like '%_daily%' or table_name like '%_hourly%' ) and table_schema = '" + DBUrl.DB_CURRENT_NAME + "' ";
						DbDataAdapter dataAdapter = DBConn.GetDataAdapter(dBConn.con);
						dataAdapter.SelectCommand = dbCommand;
						dataAdapter.Fill(dataTable);
						dataAdapter.Dispose();
						if (dataTable != null)
						{
							foreach (DataRow dataRow in dataTable.Rows)
							{
								try
								{
									long num2 = Convert.ToInt64(dataRow[0]);
									num += num2;
								}
								catch
								{
								}
							}
						}
						dataTable = new DataTable();
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
						if (dBConn != null)
						{
							try
							{
								dBConn.Close();
							}
							catch
							{
							}
						}
					}
					goto IL_1B1;
				}
				catch (Exception)
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
					if (dBConn != null)
					{
						try
						{
							dBConn.Close();
						}
						catch
						{
						}
					}
					goto IL_1B1;
				}
			}
			long num3 = 1L;
			Hashtable deviceCache = DBCache.GetDeviceCache();
			Hashtable bankCache = DBCache.GetBankCache();
			Hashtable portCache = DBCache.GetPortCache();
			if (deviceCache != null && bankCache != null && portCache != null)
			{
				num3 = (long)(deviceCache.Count + bankCache.Count + portCache.Count);
			}
			List<DateTime> dataDBFileNameList = AccessDBUpdate.GetDataDBFileNameList();
			long num4 = 1L;
			if (dataDBFileNameList != null)
			{
				num4 = (long)dataDBFileNameList.Count;
			}
			num += num3 * num4 * 25L;
			long num5 = (long)DBCache.GetInterval();
			if (num5 > 0L)
			{
				num += num3 * num4 * (86400L / num5);
			}
			IL_1B1:
			if (num > 0L)
			{
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					num /= 240L;
				}
				else
				{
					num /= 5000L;
				}
			}
			return num;
		}
		public static void DelFile(string path)
		{
			string[] files = Directory.GetFiles(path);
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string path2 = array[i];
				File.Delete(path2);
			}
		}
		public static int CopyDir(string srcPath, string aimPath)
		{
			int result = -1;
			try
			{
				if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
				{
					aimPath += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(aimPath))
				{
					Directory.CreateDirectory(aimPath);
				}
				string[] files = Directory.GetFiles(srcPath);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (Directory.Exists(text))
					{
						DBTools.CopyDir(text, aimPath + Path.GetFileName(text));
					}
					else
					{
						File.Copy(text, aimPath + Path.GetFileName(text), true);
					}
				}
				return 100;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return result;
		}
		public static int MoveDir(string srcPath, string aimPath, long l_t)
		{
			long num = 0L;
			int num2 = -1;
			try
			{
				if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
				{
					aimPath += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(aimPath))
				{
					Directory.CreateDirectory(aimPath);
				}
				string[] files = Directory.GetFiles(srcPath);
				string[] array = files;
				int result;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (Directory.Exists(text))
					{
						DBTools.MoveDir(text, aimPath + Path.GetFileName(text), l_t);
					}
					else
					{
						FileInfo fileInfo = new FileInfo(text);
						num += fileInfo.Length;
						try
						{
							File.Move(text, aimPath + Path.GetFileName(text));
						}
						catch (IOException)
						{
							try
							{
								File.Copy(text, aimPath + Path.GetFileName(text), true);
								File.Delete(text);
							}
							catch (Exception)
							{
								result = num2;
								return result;
							}
						}
						catch (Exception)
						{
							result = num2;
							return result;
						}
						DBTools.ProgramBar_Percent = AccessDBUpdate.GetPercent(num, l_t);
					}
				}
				result = 100;
				return result;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return num2;
		}
		public static int GetPeakMonthHour(string str_Start, int i_dur)
		{
			try
			{
				DateTime dateTime = Convert.ToDateTime(str_Start);
				int num = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
				return num * i_dur;
			}
			catch
			{
			}
			return -1;
		}
		public static int GetMonthHour(string str_Start)
		{
			try
			{
				DateTime dateTime = Convert.ToDateTime(str_Start);
				int num = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
				return num * 24;
			}
			catch
			{
			}
			return -1;
		}
		public static string gethourstring(string str_from, int i_dur)
		{
			string text = "";
			try
			{
				int num = Convert.ToInt32(str_from);
				int num2 = num;
				for (int i = 0; i < i_dur; i++)
				{
					if (num2 > 23)
					{
						num2 = 0;
					}
					if (num2 < 10)
					{
						"'0" + num2 + "'";
					}
					else
					{
						"'" + num2 + "'";
					}
					if (i == i_dur - 1)
					{
						text += num2;
					}
					else
					{
						text = text + num2 + ",";
					}
					num2++;
				}
				return "( " + text + ")";
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return "";
		}
		public static int DeleteDir(string aimPath)
		{
			try
			{
				if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
				{
					aimPath += Path.DirectorySeparatorChar;
				}
				string[] fileSystemEntries = Directory.GetFileSystemEntries(aimPath);
				string[] array = fileSystemEntries;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					if (Directory.Exists(path))
					{
						DBTools.DeleteDir(aimPath + Path.GetFileName(path));
					}
					else
					{
						File.Delete(aimPath + Path.GetFileName(path));
					}
				}
				Directory.Delete(aimPath, true);
				return 1;
			}
			catch
			{
			}
			return -1;
		}
		private static DriveInfo getCurrentDrive(string path)
		{
			try
			{
				string pathRoot = Path.GetPathRoot(path);
				if (string.IsNullOrEmpty(pathRoot))
				{
					DriveInfo result = null;
					return result;
				}
				DriveInfo[] drives = DriveInfo.GetDrives();
				DriveInfo[] array = drives;
				for (int i = 0; i < array.Length; i++)
				{
					DriveInfo driveInfo = array[i];
					if (driveInfo.Name.StartsWith(pathRoot.ToUpper()) || driveInfo.Name.StartsWith(pathRoot))
					{
						DriveInfo result = driveInfo;
						return result;
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}
		public static long[] GetDBSpaceSize()
		{
			long[] array = new long[2];
			long num = -1L;
			long num2 = -1L;
			try
			{
				if (DBUrl.SERVERMODE)
				{
					DBConn dBConn = null;
					DbCommand dbCommand = null;
					DbDataReader dbDataReader = null;
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn != null && dBConn.con != null)
						{
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "SELECT sum(DATA_LENGTH)+sum(INDEX_LENGTH) FROM information_schema.TABLES where TABLE_SCHEMA = '" + DBUrl.DB_CURRENT_NAME + "' ";
							object obj = dbCommand.ExecuteScalar();
							if (obj != null && obj != DBNull.Value)
							{
								num = Convert.ToInt64(obj);
							}
							dbCommand.CommandText = "show variables where variable_name = 'datadir' ";
							string path = "";
							dbDataReader = dbCommand.ExecuteReader();
							if (dbDataReader.Read())
							{
								path = Convert.ToString(dbDataReader.GetValue(1));
							}
							dbDataReader.Close();
							dbCommand.Dispose();
							dBConn.Close();
							DriveInfo currentDrive = DBTools.getCurrentDrive(path);
							if (currentDrive != null)
							{
								num2 = currentDrive.AvailableFreeSpace;
							}
							if (num >= 0L && num2 > 0L)
							{
								array[0] = num;
								array[1] = num2;
							}
						}
						goto IL_44D;
					}
					catch (Exception)
					{
						if (dbDataReader != null)
						{
							dbDataReader.Close();
						}
						if (dbCommand != null)
						{
							dbCommand.Dispose();
						}
						if (dBConn != null)
						{
							dBConn.Close();
						}
						goto IL_44D;
					}
				}
				string text = AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				string text2 = text + "sysdb.mdb";
				if (!File.Exists(text2))
				{
					array[0] = -10L;
					array[1] = -10L;
					long[] result = array;
					return result;
				}
				FileInfo fileInfo = new FileInfo(text2);
				num = fileInfo.Length;
				fileInfo = new FileInfo(text + "logdb.mdb");
				if (File.Exists(text + "logdb.mdb"))
				{
					num += fileInfo.Length;
				}
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("ACCESS"))
				{
					string text3 = AppDomain.CurrentDomain.BaseDirectory + "datadb";
					if (text3[text3.Length - 1] != Path.DirectorySeparatorChar)
					{
						text3 += Path.DirectorySeparatorChar;
					}
					if (!Directory.Exists(text3))
					{
						array[0] = -10L;
						array[1] = -10L;
						long[] result = array;
						return result;
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(text3);
					FileInfo[] files = directoryInfo.GetFiles();
					if (files.Length != 0)
					{
						FileInfo[] array2 = files;
						for (int i = 0; i < array2.Length; i++)
						{
							FileInfo fileInfo2 = array2[i];
							if (fileInfo2.Extension.ToLower().Equals(".mdb") || fileInfo2.Extension.ToLower().Equals(".org"))
							{
								num += fileInfo2.Length;
							}
						}
					}
					DriveInfo currentDrive2 = DBTools.getCurrentDrive(text);
					if (currentDrive2 != null)
					{
						num2 = currentDrive2.AvailableFreeSpace;
					}
					if (num >= 0L && num2 >= 0L)
					{
						array[0] = num;
						array[1] = num2;
					}
				}
				else
				{
					array = new long[]
					{
						-10L,
						-10L,
						-10L,
						-10L
					};
					long num3 = num;
					DriveInfo currentDrive3 = DBTools.getCurrentDrive(text);
					if (currentDrive3 == null)
					{
						long[] result = array;
						return result;
					}
					num2 = currentDrive3.AvailableFreeSpace;
					if (num3 >= 0L && num2 >= 0L)
					{
						array[0] = num3;
						array[1] = num2;
						array[2] = -10L;
						array[3] = -10L;
					}
					num2 = -10L;
					num = -10L;
					DBConn dBConn2 = null;
					DbCommand dbCommand2 = null;
					DbDataReader dbDataReader2 = null;
					try
					{
						dBConn2 = DBConnPool.getDynaConnection();
						if (dBConn2 != null && dBConn2.con != null)
						{
							dbCommand2 = dBConn2.con.CreateCommand();
							dbCommand2.CommandText = "SELECT sum(DATA_LENGTH)+sum(INDEX_LENGTH) FROM information_schema.TABLES where TABLE_SCHEMA = '" + DBUrl.DB_CURRENT_NAME + "' ";
							object obj2 = dbCommand2.ExecuteScalar();
							if (obj2 != null && !obj2.Equals(DBNull.Value))
							{
								num = Convert.ToInt64(obj2);
							}
							if (DBMaintain.IsLocalIP(DBUrl.CURRENT_HOST_PATH))
							{
								dbCommand2.CommandText = "show variables where variable_name = 'datadir' ";
								string path2 = "";
								dbDataReader2 = dbCommand2.ExecuteReader();
								if (dbDataReader2.Read())
								{
									path2 = Convert.ToString(dbDataReader2.GetValue(1));
								}
								dbDataReader2.Close();
								dbCommand2.Dispose();
								dBConn2.Close();
								try
								{
									num2 = -10L;
									DriveInfo currentDrive4 = DBTools.getCurrentDrive(path2);
									if (currentDrive4 != null)
									{
										num2 = currentDrive4.AvailableFreeSpace;
									}
								}
								catch
								{
								}
							}
							if (num >= 0L)
							{
								array[2] = num;
							}
							if (num2 >= 0L)
							{
								array[3] = num2;
							}
						}
					}
					catch (Exception)
					{
						if (dbDataReader2 != null)
						{
							dbDataReader2.Close();
						}
						if (dbCommand2 != null)
						{
							dbCommand2.Dispose();
						}
						if (dBConn2 != null)
						{
							dBConn2.Close();
						}
					}
				}
				IL_44D:;
			}
			catch (Exception)
			{
			}
			return array;
		}
		public static string GetMySQLVersion(string strhost, string iPort, string strusr, string pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = null;
			string result = "";
			try
			{
				dbConnection = new MySqlConnection(string.Concat(new string[]
				{
					"Database=;Data Source=",
					strhost,
					";Port=",
					iPort,
					";User Id=",
					strusr,
					";Password=",
					pwd,
					";Default Command Timeout=0;charset=utf8;"
				}));
				dbConnection.Open();
				dbCommand = new MySqlCommand();
				dbCommand.Connection = dbConnection;
				dbCommand.CommandText = "select version()";
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				if (dbDataReader.Read())
				{
					result = Convert.ToString(dbDataReader.GetValue(0));
				}
				dbDataReader.Close();
				return result;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					if (dbConnection != null)
					{
						dbConnection.Close();
					}
				}
				catch
				{
				}
			}
			return "";
		}
		public static string GetMySQLDataPath(string dbname, string strhost, int iPort, string strusr, string pwd)
		{
			DbConnection dbConnection = null;
			DbCommand dbCommand = new OleDbCommand();
			string text = "";
			try
			{
				if (DBUrl.SERVERMODE)
				{
					dbConnection = new MySqlConnection(string.Concat(new object[]
					{
						"Database=;Data Source=",
						strhost,
						";Port=",
						iPort,
						";User Id=",
						strusr,
						";Password=",
						pwd,
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
				}
				else
				{
					dbConnection = new MySqlConnection(string.Concat(new object[]
					{
						"Database=;Data Source=",
						strhost,
						";Port=",
						iPort,
						";User Id=",
						strusr,
						";Password=",
						pwd,
						";Pooling=true;Min Pool Size=0;Max Pool Size=150;Default Command Timeout=0;charset=utf8;"
					}));
				}
				dbConnection.Open();
				dbCommand = new MySqlCommand();
				dbCommand.Connection = dbConnection;
				dbCommand.CommandText = "show variables where variable_name = 'datadir'";
				DbDataReader dbDataReader = dbCommand.ExecuteReader();
				while (dbDataReader.Read())
				{
					string text2 = Convert.ToString(dbDataReader.GetValue(1));
					if (DBUrl.IsServer)
					{
						text = text2 + dbname + Path.DirectorySeparatorChar;
					}
					else
					{
						text = text2 + "eco" + Path.DirectorySeparatorChar;
					}
					try
					{
						DirectorySecurity directorySecurity = new DirectorySecurity();
						directorySecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
						Directory.SetAccessControl(text2, directorySecurity);
						Directory.SetAccessControl(text, directorySecurity);
						break;
					}
					catch
					{
						break;
					}
				}
				dbDataReader.Close();
				return text;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				dbCommand.Dispose();
				if (dbConnection != null)
				{
					dbConnection.Close();
				}
			}
			return "";
		}
		public static DbParameter GetParameter(string str_key, object obj_value, DbCommand command)
		{
			DbParameter dbParameter = command.CreateParameter();
			dbParameter.ParameterName = str_key;
			dbParameter.Value = obj_value;
			return dbParameter;
		}
		public static void Write_DBERROR_Log()
		{
			try
			{
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				string text = "";
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("ACCESS"))
				{
					text = Path.GetPathRoot(baseDirectory);
				}
				else
				{
					string cURRENT_HOST_PATH = DBUrl.CURRENT_HOST_PATH;
					if (DBMaintain.IsLocalIP(cURRENT_HOST_PATH))
					{
						try
						{
							string mySQLDataPath = DBTools.GetMySQLDataPath(DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
							text = Path.GetPathRoot(mySQLDataPath);
							goto IL_75;
						}
						catch
						{
							text = "N/A";
							goto IL_75;
						}
					}
					text = "N/A";
				}
				IL_75:
				DriveInfo[] drives = DriveInfo.GetDrives();
				DriveInfo[] array = drives;
				for (int i = 0; i < array.Length; i++)
				{
					DriveInfo driveInfo = array[i];
					if (driveInfo.Name.StartsWith(text.ToUpper()) || driveInfo.Name.StartsWith(text))
					{
						long arg_B6_0 = driveInfo.AvailableFreeSpace;
					}
				}
				if (DBTools.DWL != null)
				{
					long[] dBSpaceSize = DBTools.GetDBSpaceSize();
					if (dBSpaceSize != null && dBSpaceSize.Length > 2)
					{
						long num = dBSpaceSize[3];
						long num2 = dBSpaceSize[2];
						string text2;
						if (num < 0L)
						{
							text2 = "N/A";
						}
						else
						{
							double num3 = (double)num;
							text2 = (num3 / 1048576.0).ToString();
						}
						string text3;
						if (num2 < 0L)
						{
							text3 = "N/A";
						}
						else
						{
							double num4 = (double)num2;
							text3 = (num4 / 1048576.0).ToString();
						}
						DBTools.DWL("0110071", new string[]
						{
							text,
							text2,
							text3
						});
					}
					else
					{
						long num5 = dBSpaceSize[1];
						long num6 = dBSpaceSize[0];
						string text4;
						if (num5 < 0L)
						{
							text4 = "N/A";
						}
						else
						{
							double num7 = (double)num5;
							text4 = (num7 / 1048576.0).ToString();
						}
						string text5;
						if (num6 < 0L)
						{
							text5 = "N/A";
						}
						else
						{
							double num8 = (double)num6;
							text5 = (num8 / 1048576.0).ToString();
						}
						DBTools.DWL("0110071", new string[]
						{
							text,
							text4,
							text5
						});
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Generate 0110071 Log Error : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}
}
