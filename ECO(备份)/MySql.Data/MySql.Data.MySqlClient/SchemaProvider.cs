using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace MySql.Data.MySqlClient
{
	internal class SchemaProvider
	{
		protected MySqlConnection connection;
		public static string MetaCollection = "MetaDataCollections";
		public SchemaProvider(MySqlConnection connectionToUse)
		{
			this.connection = connectionToUse;
		}
		public virtual MySqlSchemaCollection GetSchema(string collection, string[] restrictions)
		{
			if (this.connection.State != ConnectionState.Open)
			{
				throw new MySqlException("GetSchema can only be called on an open connection.");
			}
			collection = StringUtility.ToUpperInvariant(collection);
			MySqlSchemaCollection schemaInternal = this.GetSchemaInternal(collection, restrictions);
			if (schemaInternal == null)
			{
				throw new ArgumentException("Invalid collection name");
			}
			return schemaInternal;
		}
		public virtual MySqlSchemaCollection GetDatabases(string[] restrictions)
		{
			Regex regex = null;
			int num = int.Parse(this.connection.driver.Property("lower_case_table_names"));
			string text = "SHOW DATABASES";
			if (num == 0 && restrictions != null && restrictions.Length >= 1)
			{
				text = text + " LIKE '" + restrictions[0] + "'";
			}
			MySqlSchemaCollection mySqlSchemaCollection = this.QueryCollection("Databases", text);
			if (num != 0 && restrictions != null && restrictions.Length >= 1 && restrictions[0] != null)
			{
				regex = new Regex(restrictions[0], RegexOptions.IgnoreCase);
			}
			MySqlSchemaCollection mySqlSchemaCollection2 = new MySqlSchemaCollection("Databases");
			mySqlSchemaCollection2.AddColumn("CATALOG_NAME", typeof(string));
			mySqlSchemaCollection2.AddColumn("SCHEMA_NAME", typeof(string));
			foreach (MySqlSchemaRow current in mySqlSchemaCollection.Rows)
			{
				if (regex == null || regex.Match(current[0].ToString()).Success)
				{
					MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection2.AddRow();
					mySqlSchemaRow[1] = current[0];
				}
			}
			return mySqlSchemaCollection2;
		}
		public virtual MySqlSchemaCollection GetTables(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Tables");
			mySqlSchemaCollection.AddColumn("TABLE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_TYPE", typeof(string));
			mySqlSchemaCollection.AddColumn("ENGINE", typeof(string));
			mySqlSchemaCollection.AddColumn("VERSION", typeof(ulong));
			mySqlSchemaCollection.AddColumn("ROW_FORMAT", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_ROWS", typeof(ulong));
			mySqlSchemaCollection.AddColumn("AVG_ROW_LENGTH", typeof(ulong));
			mySqlSchemaCollection.AddColumn("DATA_LENGTH", typeof(ulong));
			mySqlSchemaCollection.AddColumn("MAX_DATA_LENGTH", typeof(ulong));
			mySqlSchemaCollection.AddColumn("INDEX_LENGTH", typeof(ulong));
			mySqlSchemaCollection.AddColumn("DATA_FREE", typeof(ulong));
			mySqlSchemaCollection.AddColumn("AUTO_INCREMENT", typeof(ulong));
			mySqlSchemaCollection.AddColumn("CREATE_TIME", typeof(DateTime));
			mySqlSchemaCollection.AddColumn("UPDATE_TIME", typeof(DateTime));
			mySqlSchemaCollection.AddColumn("CHECK_TIME", typeof(DateTime));
			mySqlSchemaCollection.AddColumn("TABLE_COLLATION", typeof(string));
			mySqlSchemaCollection.AddColumn("CHECKSUM", typeof(ulong));
			mySqlSchemaCollection.AddColumn("CREATE_OPTIONS", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_COMMENT", typeof(string));
			string[] array = new string[4];
			if (restrictions != null && restrictions.Length >= 2)
			{
				array[0] = restrictions[1];
			}
			MySqlSchemaCollection databases = this.GetDatabases(array);
			if (restrictions != null)
			{
				Array.Copy(restrictions, array, Math.Min(array.Length, restrictions.Length));
			}
			foreach (MySqlSchemaRow current in databases.Rows)
			{
				array[1] = current["SCHEMA_NAME"].ToString();
				this.FindTables(mySqlSchemaCollection, array);
			}
			return mySqlSchemaCollection;
		}
		protected void QuoteDefaultValues(MySqlSchemaCollection schemaCollection)
		{
			if (schemaCollection == null)
			{
				return;
			}
			if (!schemaCollection.ContainsColumn("COLUMN_DEFAULT"))
			{
				return;
			}
			foreach (MySqlSchemaRow current in schemaCollection.Rows)
			{
				object arg = current["COLUMN_DEFAULT"];
				if (MetaData.IsTextType(current["DATA_TYPE"].ToString()))
				{
					current["COLUMN_DEFAULT"] = string.Format("{0}", arg);
				}
			}
		}
		public virtual MySqlSchemaCollection GetColumns(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Columns");
			mySqlSchemaCollection.AddColumn("TABLE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("COLUMN_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("ORDINAL_POSITION", typeof(ulong));
			mySqlSchemaCollection.AddColumn("COLUMN_DEFAULT", typeof(string));
			mySqlSchemaCollection.AddColumn("IS_NULLABLE", typeof(string));
			mySqlSchemaCollection.AddColumn("DATA_TYPE", typeof(string));
			mySqlSchemaCollection.AddColumn("CHARACTER_MAXIMUM_LENGTH", typeof(ulong));
			mySqlSchemaCollection.AddColumn("CHARACTER_OCTET_LENGTH", typeof(ulong));
			mySqlSchemaCollection.AddColumn("NUMERIC_PRECISION", typeof(ulong));
			mySqlSchemaCollection.AddColumn("NUMERIC_SCALE", typeof(ulong));
			mySqlSchemaCollection.AddColumn("CHARACTER_SET_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("COLLATION_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("COLUMN_TYPE", typeof(string));
			mySqlSchemaCollection.AddColumn("COLUMN_KEY", typeof(string));
			mySqlSchemaCollection.AddColumn("EXTRA", typeof(string));
			mySqlSchemaCollection.AddColumn("PRIVILEGES", typeof(string));
			mySqlSchemaCollection.AddColumn("COLUMN_COMMENT", typeof(string));
			string columnRestriction = null;
			if (restrictions != null && restrictions.Length == 4)
			{
				columnRestriction = restrictions[3];
				restrictions[3] = null;
			}
			MySqlSchemaCollection tables = this.GetTables(restrictions);
			foreach (MySqlSchemaRow current in tables.Rows)
			{
				this.LoadTableColumns(mySqlSchemaCollection, current["TABLE_SCHEMA"].ToString(), current["TABLE_NAME"].ToString(), columnRestriction);
			}
			this.QuoteDefaultValues(mySqlSchemaCollection);
			return mySqlSchemaCollection;
		}
		private void LoadTableColumns(MySqlSchemaCollection schemaCollection, string schema, string tableName, string columnRestriction)
		{
			string cmdText = string.Format("SHOW FULL COLUMNS FROM `{0}`.`{1}`", schema, tableName);
			MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
			int num = 1;
			using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
			{
				while (mySqlDataReader.Read())
				{
					string @string = mySqlDataReader.GetString(0);
					if (columnRestriction == null || !(@string != columnRestriction))
					{
						MySqlSchemaRow mySqlSchemaRow = schemaCollection.AddRow();
						mySqlSchemaRow["TABLE_CATALOG"] = DBNull.Value;
						mySqlSchemaRow["TABLE_SCHEMA"] = schema;
						mySqlSchemaRow["TABLE_NAME"] = tableName;
						mySqlSchemaRow["COLUMN_NAME"] = @string;
						mySqlSchemaRow["ORDINAL_POSITION"] = num++;
						mySqlSchemaRow["COLUMN_DEFAULT"] = mySqlDataReader.GetValue(5);
						mySqlSchemaRow["IS_NULLABLE"] = mySqlDataReader.GetString(3);
						mySqlSchemaRow["DATA_TYPE"] = mySqlDataReader.GetString(1);
						mySqlSchemaRow["CHARACTER_MAXIMUM_LENGTH"] = DBNull.Value;
						mySqlSchemaRow["CHARACTER_OCTET_LENGTH"] = DBNull.Value;
						mySqlSchemaRow["NUMERIC_PRECISION"] = DBNull.Value;
						mySqlSchemaRow["NUMERIC_SCALE"] = DBNull.Value;
						mySqlSchemaRow["CHARACTER_SET_NAME"] = mySqlDataReader.GetValue(2);
						mySqlSchemaRow["COLLATION_NAME"] = mySqlSchemaRow["CHARACTER_SET_NAME"];
						mySqlSchemaRow["COLUMN_TYPE"] = mySqlDataReader.GetString(1);
						mySqlSchemaRow["COLUMN_KEY"] = mySqlDataReader.GetString(4);
						mySqlSchemaRow["EXTRA"] = mySqlDataReader.GetString(6);
						mySqlSchemaRow["PRIVILEGES"] = mySqlDataReader.GetString(7);
						mySqlSchemaRow["COLUMN_COMMENT"] = mySqlDataReader.GetString(8);
						SchemaProvider.ParseColumnRow(mySqlSchemaRow);
					}
				}
			}
		}
		private static void ParseColumnRow(MySqlSchemaRow row)
		{
			string text = row["CHARACTER_SET_NAME"].ToString();
			int num = text.IndexOf('_');
			if (num != -1)
			{
				row["CHARACTER_SET_NAME"] = text.Substring(0, num);
			}
			string text2 = row["DATA_TYPE"].ToString();
			num = text2.IndexOf('(');
			if (num == -1)
			{
				return;
			}
			row["DATA_TYPE"] = text2.Substring(0, num);
			int num2 = text2.IndexOf(')', num);
			string text3 = text2.Substring(num + 1, num2 - (num + 1));
			string a = row["DATA_TYPE"].ToString().ToLower();
			if (a == "char" || a == "varchar")
			{
				row["CHARACTER_MAXIMUM_LENGTH"] = text3;
				return;
			}
			if (a == "real" || a == "decimal")
			{
				string[] array = text3.Split(new char[]
				{
					','
				});
				row["NUMERIC_PRECISION"] = array[0];
				if (array.Length == 2)
				{
					row["NUMERIC_SCALE"] = array[1];
				}
			}
		}
		public virtual MySqlSchemaCollection GetIndexes(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Indexes");
			mySqlSchemaCollection.AddColumn("INDEX_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("INDEX_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("INDEX_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("UNIQUE", typeof(bool));
			mySqlSchemaCollection.AddColumn("PRIMARY", typeof(bool));
			mySqlSchemaCollection.AddColumn("TYPE", typeof(string));
			mySqlSchemaCollection.AddColumn("COMMENT", typeof(string));
			int val = (restrictions == null) ? 4 : restrictions.Length;
			string[] array = new string[Math.Max(val, 4)];
			if (restrictions != null)
			{
				restrictions.CopyTo(array, 0);
			}
			array[3] = "BASE TABLE";
			MySqlSchemaCollection tables = this.GetTables(array);
			foreach (MySqlSchemaRow current in tables.Rows)
			{
				string sql = string.Format("SHOW INDEX FROM `{0}`.`{1}`", MySqlHelper.DoubleQuoteString((string)current["TABLE_SCHEMA"]), MySqlHelper.DoubleQuoteString((string)current["TABLE_NAME"]));
				MySqlSchemaCollection mySqlSchemaCollection2 = this.QueryCollection("indexes", sql);
				foreach (MySqlSchemaRow current2 in mySqlSchemaCollection2.Rows)
				{
					long num = (long)current2["SEQ_IN_INDEX"];
					if (num == 1L && (restrictions == null || restrictions.Length != 4 || restrictions[3] == null || current2["KEY_NAME"].Equals(restrictions[3])))
					{
						MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
						mySqlSchemaRow["INDEX_CATALOG"] = null;
						mySqlSchemaRow["INDEX_SCHEMA"] = current["TABLE_SCHEMA"];
						mySqlSchemaRow["INDEX_NAME"] = current2["KEY_NAME"];
						mySqlSchemaRow["TABLE_NAME"] = current2["TABLE"];
						mySqlSchemaRow["UNIQUE"] = ((long)current2["NON_UNIQUE"] == 0L);
						mySqlSchemaRow["PRIMARY"] = current2["KEY_NAME"].Equals("PRIMARY");
						mySqlSchemaRow["TYPE"] = current2["INDEX_TYPE"];
						mySqlSchemaRow["COMMENT"] = current2["COMMENT"];
					}
				}
			}
			return mySqlSchemaCollection;
		}
		public virtual MySqlSchemaCollection GetIndexColumns(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("IndexColumns");
			mySqlSchemaCollection.AddColumn("INDEX_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("INDEX_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("INDEX_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("COLUMN_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("ORDINAL_POSITION", typeof(int));
			mySqlSchemaCollection.AddColumn("SORT_ORDER", typeof(string));
			int val = (restrictions == null) ? 4 : restrictions.Length;
			string[] array = new string[Math.Max(val, 4)];
			if (restrictions != null)
			{
				restrictions.CopyTo(array, 0);
			}
			array[3] = "BASE TABLE";
			MySqlSchemaCollection tables = this.GetTables(array);
			foreach (MySqlSchemaRow current in tables.Rows)
			{
				string cmdText = string.Format("SHOW INDEX FROM `{0}`.`{1}`", current["TABLE_SCHEMA"], current["TABLE_NAME"]);
				MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
				using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
				{
					while (mySqlDataReader.Read())
					{
						string @string = SchemaProvider.GetString(mySqlDataReader, mySqlDataReader.GetOrdinal("KEY_NAME"));
						string string2 = SchemaProvider.GetString(mySqlDataReader, mySqlDataReader.GetOrdinal("COLUMN_NAME"));
						if (restrictions == null || ((restrictions.Length < 4 || restrictions[3] == null || !(@string != restrictions[3])) && (restrictions.Length < 5 || restrictions[4] == null || !(string2 != restrictions[4]))))
						{
							MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
							mySqlSchemaRow["INDEX_CATALOG"] = null;
							mySqlSchemaRow["INDEX_SCHEMA"] = current["TABLE_SCHEMA"];
							mySqlSchemaRow["INDEX_NAME"] = @string;
							mySqlSchemaRow["TABLE_NAME"] = SchemaProvider.GetString(mySqlDataReader, mySqlDataReader.GetOrdinal("TABLE"));
							mySqlSchemaRow["COLUMN_NAME"] = string2;
							mySqlSchemaRow["ORDINAL_POSITION"] = mySqlDataReader.GetValue(mySqlDataReader.GetOrdinal("SEQ_IN_INDEX"));
							mySqlSchemaRow["SORT_ORDER"] = mySqlDataReader.GetString("COLLATION");
						}
					}
				}
			}
			return mySqlSchemaCollection;
		}
		public virtual MySqlSchemaCollection GetForeignKeys(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Foreign Keys");
			mySqlSchemaCollection.AddColumn("CONSTRAINT_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("CONSTRAINT_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("CONSTRAINT_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("MATCH_OPTION", typeof(string));
			mySqlSchemaCollection.AddColumn("UPDATE_RULE", typeof(string));
			mySqlSchemaCollection.AddColumn("DELETE_RULE", typeof(string));
			mySqlSchemaCollection.AddColumn("REFERENCED_TABLE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("REFERENCED_TABLE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("REFERENCED_TABLE_NAME", typeof(string));
			string filterName = null;
			if (restrictions != null && restrictions.Length >= 4)
			{
				filterName = restrictions[3];
				restrictions[3] = null;
			}
			MySqlSchemaCollection tables = this.GetTables(restrictions);
			foreach (MySqlSchemaRow current in tables.Rows)
			{
				this.GetForeignKeysOnTable(mySqlSchemaCollection, current, filterName, false);
			}
			return mySqlSchemaCollection;
		}
		public virtual MySqlSchemaCollection GetForeignKeyColumns(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Foreign Keys");
			mySqlSchemaCollection.AddColumn("CONSTRAINT_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("CONSTRAINT_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("CONSTRAINT_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("COLUMN_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("ORDINAL_POSITION", typeof(int));
			mySqlSchemaCollection.AddColumn("REFERENCED_TABLE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("REFERENCED_TABLE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("REFERENCED_TABLE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("REFERENCED_COLUMN_NAME", typeof(string));
			string filterName = null;
			if (restrictions != null && restrictions.Length >= 4)
			{
				filterName = restrictions[3];
				restrictions[3] = null;
			}
			MySqlSchemaCollection tables = this.GetTables(restrictions);
			foreach (MySqlSchemaRow current in tables.Rows)
			{
				this.GetForeignKeysOnTable(mySqlSchemaCollection, current, filterName, true);
			}
			return mySqlSchemaCollection;
		}
		private string GetSqlMode()
		{
			MySqlCommand mySqlCommand = new MySqlCommand("SELECT @@SQL_MODE", this.connection);
			return mySqlCommand.ExecuteScalar().ToString();
		}
		private void GetForeignKeysOnTable(MySqlSchemaCollection fkTable, MySqlSchemaRow tableToParse, string filterName, bool includeColumns)
		{
			string sqlMode = this.GetSqlMode();
			if (filterName != null)
			{
				filterName = StringUtility.ToLowerInvariant(filterName);
			}
			string cmdText = string.Format("SHOW CREATE TABLE `{0}`.`{1}`", tableToParse["TABLE_SCHEMA"], tableToParse["TABLE_NAME"]);
			string input = null;
			MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
			using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
			{
				mySqlDataReader.Read();
				string @string = mySqlDataReader.GetString(1);
				input = StringUtility.ToLowerInvariant(@string);
			}
			MySqlTokenizer mySqlTokenizer = new MySqlTokenizer(input);
			mySqlTokenizer.AnsiQuotes = (sqlMode.IndexOf("ANSI_QUOTES") != -1);
			mySqlTokenizer.BackslashEscapes = (sqlMode.IndexOf("NO_BACKSLASH_ESCAPES") != -1);
			while (true)
			{
				string text = mySqlTokenizer.NextToken();
				while (text != null && (text != "constraint" || mySqlTokenizer.Quoted))
				{
					text = mySqlTokenizer.NextToken();
				}
				if (text == null)
				{
					break;
				}
				SchemaProvider.ParseConstraint(fkTable, tableToParse, mySqlTokenizer, includeColumns);
			}
		}
		private static void ParseConstraint(MySqlSchemaCollection fkTable, MySqlSchemaRow table, MySqlTokenizer tokenizer, bool includeColumns)
		{
			string text = tokenizer.NextToken();
			MySqlSchemaRow mySqlSchemaRow = fkTable.AddRow();
			string a = tokenizer.NextToken();
			if (a != "foreign" || tokenizer.Quoted)
			{
				return;
			}
			tokenizer.NextToken();
			tokenizer.NextToken();
			mySqlSchemaRow["CONSTRAINT_CATALOG"] = table["TABLE_CATALOG"];
			mySqlSchemaRow["CONSTRAINT_SCHEMA"] = table["TABLE_SCHEMA"];
			mySqlSchemaRow["TABLE_CATALOG"] = table["TABLE_CATALOG"];
			mySqlSchemaRow["TABLE_SCHEMA"] = table["TABLE_SCHEMA"];
			mySqlSchemaRow["TABLE_NAME"] = table["TABLE_NAME"];
			mySqlSchemaRow["REFERENCED_TABLE_CATALOG"] = null;
			mySqlSchemaRow["CONSTRAINT_NAME"] = text.Trim(new char[]
			{
				'\'',
				'`'
			});
			List<string> srcColumns = includeColumns ? SchemaProvider.ParseColumns(tokenizer) : null;
			while (a != "references" || tokenizer.Quoted)
			{
				a = tokenizer.NextToken();
			}
			string text2 = tokenizer.NextToken();
			string text3 = tokenizer.NextToken();
			if (text3.StartsWith(".", StringComparison.Ordinal))
			{
				mySqlSchemaRow["REFERENCED_TABLE_SCHEMA"] = text2;
				mySqlSchemaRow["REFERENCED_TABLE_NAME"] = text3.Substring(1).Trim(new char[]
				{
					'\'',
					'`'
				});
				tokenizer.NextToken();
			}
			else
			{
				mySqlSchemaRow["REFERENCED_TABLE_SCHEMA"] = table["TABLE_SCHEMA"];
				mySqlSchemaRow["REFERENCED_TABLE_NAME"] = text2.Substring(1).Trim(new char[]
				{
					'\'',
					'`'
				});
			}
			List<string> targetColumns = includeColumns ? SchemaProvider.ParseColumns(tokenizer) : null;
			if (includeColumns)
			{
				SchemaProvider.ProcessColumns(fkTable, mySqlSchemaRow, srcColumns, targetColumns);
				return;
			}
			fkTable.Rows.Add(mySqlSchemaRow);
		}
		private static List<string> ParseColumns(MySqlTokenizer tokenizer)
		{
			List<string> list = new List<string>();
			string text = tokenizer.NextToken();
			while (text != ")")
			{
				if (text != ",")
				{
					list.Add(text);
				}
				text = tokenizer.NextToken();
			}
			return list;
		}
		private static void ProcessColumns(MySqlSchemaCollection fkTable, MySqlSchemaRow row, List<string> srcColumns, List<string> targetColumns)
		{
			for (int i = 0; i < srcColumns.Count; i++)
			{
				MySqlSchemaRow mySqlSchemaRow = fkTable.AddRow();
				row.CopyRow(mySqlSchemaRow);
				mySqlSchemaRow["COLUMN_NAME"] = srcColumns[i];
				mySqlSchemaRow["ORDINAL_POSITION"] = i;
				mySqlSchemaRow["REFERENCED_COLUMN_NAME"] = targetColumns[i];
				fkTable.Rows.Add(mySqlSchemaRow);
			}
		}
		public virtual MySqlSchemaCollection GetUsers(string[] restrictions)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT Host, User FROM mysql.user");
			if (restrictions != null && restrictions.Length > 0)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " WHERE User LIKE '{0}'", new object[]
				{
					restrictions[0]
				});
			}
			MySqlSchemaCollection mySqlSchemaCollection = this.QueryCollection("Users", stringBuilder.ToString());
			mySqlSchemaCollection.Columns[0].Name = "HOST";
			mySqlSchemaCollection.Columns[1].Name = "USERNAME";
			return mySqlSchemaCollection;
		}
		public virtual MySqlSchemaCollection GetProcedures(string[] restrictions)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Procedures");
			mySqlSchemaCollection.AddColumn("SPECIFIC_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_CATALOG", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_SCHEMA", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_TYPE", typeof(string));
			mySqlSchemaCollection.AddColumn("DTD_IDENTIFIER", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_BODY", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_DEFINITION", typeof(string));
			mySqlSchemaCollection.AddColumn("EXTERNAL_NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("EXTERNAL_LANGUAGE", typeof(string));
			mySqlSchemaCollection.AddColumn("PARAMETER_STYLE", typeof(string));
			mySqlSchemaCollection.AddColumn("IS_DETERMINISTIC", typeof(string));
			mySqlSchemaCollection.AddColumn("SQL_DATA_ACCESS", typeof(string));
			mySqlSchemaCollection.AddColumn("SQL_PATH", typeof(string));
			mySqlSchemaCollection.AddColumn("SECURITY_TYPE", typeof(string));
			mySqlSchemaCollection.AddColumn("CREATED", typeof(DateTime));
			mySqlSchemaCollection.AddColumn("LAST_ALTERED", typeof(DateTime));
			mySqlSchemaCollection.AddColumn("SQL_MODE", typeof(string));
			mySqlSchemaCollection.AddColumn("ROUTINE_COMMENT", typeof(string));
			mySqlSchemaCollection.AddColumn("DEFINER", typeof(string));
			StringBuilder stringBuilder = new StringBuilder("SELECT * FROM mysql.proc WHERE 1=1");
			if (restrictions != null)
			{
				if (restrictions.Length >= 2 && restrictions[1] != null)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " AND db LIKE '{0}'", new object[]
					{
						restrictions[1]
					});
				}
				if (restrictions.Length >= 3 && restrictions[2] != null)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " AND name LIKE '{0}'", new object[]
					{
						restrictions[2]
					});
				}
				if (restrictions.Length >= 4 && restrictions[3] != null)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " AND type LIKE '{0}'", new object[]
					{
						restrictions[3]
					});
				}
			}
			MySqlCommand mySqlCommand = new MySqlCommand(stringBuilder.ToString(), this.connection);
			using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
			{
				while (mySqlDataReader.Read())
				{
					MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
					mySqlSchemaRow["SPECIFIC_NAME"] = mySqlDataReader.GetString("specific_name");
					mySqlSchemaRow["ROUTINE_CATALOG"] = DBNull.Value;
					mySqlSchemaRow["ROUTINE_SCHEMA"] = mySqlDataReader.GetString("db");
					mySqlSchemaRow["ROUTINE_NAME"] = mySqlDataReader.GetString("name");
					string @string = mySqlDataReader.GetString("type");
					mySqlSchemaRow["ROUTINE_TYPE"] = @string;
					mySqlSchemaRow["DTD_IDENTIFIER"] = ((StringUtility.ToLowerInvariant(@string) == "function") ? mySqlDataReader.GetString("returns") : DBNull.Value);
					mySqlSchemaRow["ROUTINE_BODY"] = "SQL";
					mySqlSchemaRow["ROUTINE_DEFINITION"] = mySqlDataReader.GetString("body");
					mySqlSchemaRow["EXTERNAL_NAME"] = DBNull.Value;
					mySqlSchemaRow["EXTERNAL_LANGUAGE"] = DBNull.Value;
					mySqlSchemaRow["PARAMETER_STYLE"] = "SQL";
					mySqlSchemaRow["IS_DETERMINISTIC"] = mySqlDataReader.GetString("is_deterministic");
					mySqlSchemaRow["SQL_DATA_ACCESS"] = mySqlDataReader.GetString("sql_data_access");
					mySqlSchemaRow["SQL_PATH"] = DBNull.Value;
					mySqlSchemaRow["SECURITY_TYPE"] = mySqlDataReader.GetString("security_type");
					mySqlSchemaRow["CREATED"] = mySqlDataReader.GetDateTime("created");
					mySqlSchemaRow["LAST_ALTERED"] = mySqlDataReader.GetDateTime("modified");
					mySqlSchemaRow["SQL_MODE"] = mySqlDataReader.GetString("sql_mode");
					mySqlSchemaRow["ROUTINE_COMMENT"] = mySqlDataReader.GetString("comment");
					mySqlSchemaRow["DEFINER"] = mySqlDataReader.GetString("definer");
				}
			}
			return mySqlSchemaCollection;
		}
		protected virtual MySqlSchemaCollection GetCollections()
		{
			object[][] data = new object[][]
			{
				new object[]
				{
					"MetaDataCollections",
					0,
					0
				},
				new object[]
				{
					"DataSourceInformation",
					0,
					0
				},
				new object[]
				{
					"DataTypes",
					0,
					0
				},
				new object[]
				{
					"Restrictions",
					0,
					0
				},
				new object[]
				{
					"ReservedWords",
					0,
					0
				},
				new object[]
				{
					"Databases",
					1,
					1
				},
				new object[]
				{
					"Tables",
					4,
					2
				},
				new object[]
				{
					"Columns",
					4,
					4
				},
				new object[]
				{
					"Users",
					1,
					1
				},
				new object[]
				{
					"Foreign Keys",
					4,
					3
				},
				new object[]
				{
					"IndexColumns",
					5,
					4
				},
				new object[]
				{
					"Indexes",
					4,
					3
				},
				new object[]
				{
					"Foreign Key Columns",
					4,
					3
				},
				new object[]
				{
					"UDF",
					1,
					1
				}
			};
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("MetaDataCollections");
			mySqlSchemaCollection.AddColumn("CollectionName", typeof(string));
			mySqlSchemaCollection.AddColumn("NumberOfRestrictions", typeof(int));
			mySqlSchemaCollection.AddColumn("NumberOfIdentifierParts", typeof(int));
			SchemaProvider.FillTable(mySqlSchemaCollection, data);
			return mySqlSchemaCollection;
		}
		private MySqlSchemaCollection GetDataSourceInformation()
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("DataSourceInformation");
			mySqlSchemaCollection.AddColumn("CompositeIdentifierSeparatorPattern", typeof(string));
			mySqlSchemaCollection.AddColumn("DataSourceProductName", typeof(string));
			mySqlSchemaCollection.AddColumn("DataSourceProductVersion", typeof(string));
			mySqlSchemaCollection.AddColumn("DataSourceProductVersionNormalized", typeof(string));
			mySqlSchemaCollection.AddColumn("GroupByBehavior", typeof(GroupByBehavior));
			mySqlSchemaCollection.AddColumn("IdentifierPattern", typeof(string));
			mySqlSchemaCollection.AddColumn("IdentifierCase", typeof(IdentifierCase));
			mySqlSchemaCollection.AddColumn("OrderByColumnsInSelect", typeof(bool));
			mySqlSchemaCollection.AddColumn("ParameterMarkerFormat", typeof(string));
			mySqlSchemaCollection.AddColumn("ParameterMarkerPattern", typeof(string));
			mySqlSchemaCollection.AddColumn("ParameterNameMaxLength", typeof(int));
			mySqlSchemaCollection.AddColumn("ParameterNamePattern", typeof(string));
			mySqlSchemaCollection.AddColumn("QuotedIdentifierPattern", typeof(string));
			mySqlSchemaCollection.AddColumn("QuotedIdentifierCase", typeof(IdentifierCase));
			mySqlSchemaCollection.AddColumn("StatementSeparatorPattern", typeof(string));
			mySqlSchemaCollection.AddColumn("StringLiteralPattern", typeof(string));
			mySqlSchemaCollection.AddColumn("SupportedJoinOperators", typeof(SupportedJoinOperators));
			DBVersion version = this.connection.driver.Version;
			string value = string.Format("{0:0}.{1:0}.{2:0}", version.Major, version.Minor, version.Build);
			MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
			mySqlSchemaRow["CompositeIdentifierSeparatorPattern"] = "\\.";
			mySqlSchemaRow["DataSourceProductName"] = "MySQL";
			mySqlSchemaRow["DataSourceProductVersion"] = this.connection.ServerVersion;
			mySqlSchemaRow["DataSourceProductVersionNormalized"] = value;
			mySqlSchemaRow["GroupByBehavior"] = GroupByBehavior.Unrelated;
			mySqlSchemaRow["IdentifierPattern"] = "(^\\`\\p{Lo}\\p{Lu}\\p{Ll}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Nd}@$#_]*$)|(^\\`[^\\`\\0]|\\`\\`+\\`$)|(^\\\" + [^\\\"\\0]|\\\"\\\"+\\\"$)";
			mySqlSchemaRow["IdentifierCase"] = IdentifierCase.Insensitive;
			mySqlSchemaRow["OrderByColumnsInSelect"] = false;
			mySqlSchemaRow["ParameterMarkerFormat"] = "{0}";
			mySqlSchemaRow["ParameterMarkerPattern"] = "(@[A-Za-z0-9_$#]*)";
			mySqlSchemaRow["ParameterNameMaxLength"] = 128;
			mySqlSchemaRow["ParameterNamePattern"] = "^[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)";
			mySqlSchemaRow["QuotedIdentifierPattern"] = "(([^\\`]|\\`\\`)*)";
			mySqlSchemaRow["QuotedIdentifierCase"] = IdentifierCase.Sensitive;
			mySqlSchemaRow["StatementSeparatorPattern"] = ";";
			mySqlSchemaRow["StringLiteralPattern"] = "'(([^']|'')*)'";
			mySqlSchemaRow["SupportedJoinOperators"] = 15;
			mySqlSchemaCollection.Rows.Add(mySqlSchemaRow);
			return mySqlSchemaCollection;
		}
		private static MySqlSchemaCollection GetDataTypes()
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("DataTypes");
			mySqlSchemaCollection.AddColumn("TypeName", typeof(string));
			mySqlSchemaCollection.AddColumn("ProviderDbType", typeof(int));
			mySqlSchemaCollection.AddColumn("ColumnSize", typeof(long));
			mySqlSchemaCollection.AddColumn("CreateFormat", typeof(string));
			mySqlSchemaCollection.AddColumn("CreateParameters", typeof(string));
			mySqlSchemaCollection.AddColumn("DataType", typeof(string));
			mySqlSchemaCollection.AddColumn("IsAutoincrementable", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsBestMatch", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsCaseSensitive", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsFixedLength", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsFixedPrecisionScale", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsLong", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsNullable", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsSearchable", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsSearchableWithLike", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsUnsigned", typeof(bool));
			mySqlSchemaCollection.AddColumn("MaximumScale", typeof(short));
			mySqlSchemaCollection.AddColumn("MinimumScale", typeof(short));
			mySqlSchemaCollection.AddColumn("IsConcurrencyType", typeof(bool));
			mySqlSchemaCollection.AddColumn("IsLiteralSupported", typeof(bool));
			mySqlSchemaCollection.AddColumn("LiteralPrefix", typeof(string));
			mySqlSchemaCollection.AddColumn("LiteralSuffix", typeof(string));
			mySqlSchemaCollection.AddColumn("NativeDataType", typeof(string));
			MySqlBit.SetDSInfo(mySqlSchemaCollection);
			MySqlBinary.SetDSInfo(mySqlSchemaCollection);
			MySqlDateTime.SetDSInfo(mySqlSchemaCollection);
			MySqlTimeSpan.SetDSInfo(mySqlSchemaCollection);
			MySqlString.SetDSInfo(mySqlSchemaCollection);
			MySqlDouble.SetDSInfo(mySqlSchemaCollection);
			MySqlSingle.SetDSInfo(mySqlSchemaCollection);
			MySqlByte.SetDSInfo(mySqlSchemaCollection);
			MySqlInt16.SetDSInfo(mySqlSchemaCollection);
			MySqlInt32.SetDSInfo(mySqlSchemaCollection);
			MySqlInt64.SetDSInfo(mySqlSchemaCollection);
			MySqlDecimal.SetDSInfo(mySqlSchemaCollection);
			MySqlUByte.SetDSInfo(mySqlSchemaCollection);
			MySqlUInt16.SetDSInfo(mySqlSchemaCollection);
			MySqlUInt32.SetDSInfo(mySqlSchemaCollection);
			MySqlUInt64.SetDSInfo(mySqlSchemaCollection);
			return mySqlSchemaCollection;
		}
		protected virtual MySqlSchemaCollection GetRestrictions()
		{
			object[][] data = new object[][]
			{
				new object[]
				{
					"Users",
					"Name",
					"",
					0
				},
				new object[]
				{
					"Databases",
					"Name",
					"",
					0
				},
				new object[]
				{
					"Tables",
					"Database",
					"",
					0
				},
				new object[]
				{
					"Tables",
					"Schema",
					"",
					1
				},
				new object[]
				{
					"Tables",
					"Table",
					"",
					2
				},
				new object[]
				{
					"Tables",
					"TableType",
					"",
					3
				},
				new object[]
				{
					"Columns",
					"Database",
					"",
					0
				},
				new object[]
				{
					"Columns",
					"Schema",
					"",
					1
				},
				new object[]
				{
					"Columns",
					"Table",
					"",
					2
				},
				new object[]
				{
					"Columns",
					"Column",
					"",
					3
				},
				new object[]
				{
					"Indexes",
					"Database",
					"",
					0
				},
				new object[]
				{
					"Indexes",
					"Schema",
					"",
					1
				},
				new object[]
				{
					"Indexes",
					"Table",
					"",
					2
				},
				new object[]
				{
					"Indexes",
					"Name",
					"",
					3
				},
				new object[]
				{
					"IndexColumns",
					"Database",
					"",
					0
				},
				new object[]
				{
					"IndexColumns",
					"Schema",
					"",
					1
				},
				new object[]
				{
					"IndexColumns",
					"Table",
					"",
					2
				},
				new object[]
				{
					"IndexColumns",
					"ConstraintName",
					"",
					3
				},
				new object[]
				{
					"IndexColumns",
					"Column",
					"",
					4
				},
				new object[]
				{
					"Foreign Keys",
					"Database",
					"",
					0
				},
				new object[]
				{
					"Foreign Keys",
					"Schema",
					"",
					1
				},
				new object[]
				{
					"Foreign Keys",
					"Table",
					"",
					2
				},
				new object[]
				{
					"Foreign Keys",
					"Constraint Name",
					"",
					3
				},
				new object[]
				{
					"Foreign Key Columns",
					"Catalog",
					"",
					0
				},
				new object[]
				{
					"Foreign Key Columns",
					"Schema",
					"",
					1
				},
				new object[]
				{
					"Foreign Key Columns",
					"Table",
					"",
					2
				},
				new object[]
				{
					"Foreign Key Columns",
					"Constraint Name",
					"",
					3
				},
				new object[]
				{
					"UDF",
					"Name",
					"",
					0
				}
			};
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("Restrictions");
			mySqlSchemaCollection.AddColumn("CollectionName", typeof(string));
			mySqlSchemaCollection.AddColumn("RestrictionName", typeof(string));
			mySqlSchemaCollection.AddColumn("RestrictionDefault", typeof(string));
			mySqlSchemaCollection.AddColumn("RestrictionNumber", typeof(int));
			SchemaProvider.FillTable(mySqlSchemaCollection, data);
			return mySqlSchemaCollection;
		}
		private static MySqlSchemaCollection GetReservedWords()
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("ReservedWords");
			mySqlSchemaCollection.AddColumn(DbMetaDataColumnNames.ReservedWord, typeof(string));
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MySql.Data.MySqlClient.Properties.ReservedWords.txt");
			StreamReader streamReader = new StreamReader(manifestResourceStream);
			for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
			{
				string[] array = text.Split(new char[]
				{
					' '
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string value = array2[i];
					if (!string.IsNullOrEmpty(value))
					{
						MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
						mySqlSchemaRow[0] = value;
					}
				}
			}
			streamReader.Dispose();
			manifestResourceStream.Close();
			return mySqlSchemaCollection;
		}
		protected static void FillTable(MySqlSchemaCollection dt, object[][] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				object[] array = data[i];
				MySqlSchemaRow mySqlSchemaRow = dt.AddRow();
				for (int j = 0; j < array.Length; j++)
				{
					mySqlSchemaRow[j] = array[j];
				}
			}
		}
		private void FindTables(MySqlSchemaCollection schema, string[] restrictions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "SHOW TABLE STATUS FROM `{0}`", new object[]
			{
				restrictions[1]
			});
			if (restrictions != null && restrictions.Length >= 3 && restrictions[2] != null)
			{
				stringBuilder2.AppendFormat(CultureInfo.InvariantCulture, " LIKE '{0}'", new object[]
				{
					restrictions[2]
				});
			}
			stringBuilder.Append(stringBuilder2.ToString());
			string value = (restrictions[1].ToLower() == "information_schema") ? "SYSTEM VIEW" : "BASE TABLE";
			MySqlCommand mySqlCommand = new MySqlCommand(stringBuilder.ToString(), this.connection);
			using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
			{
				while (mySqlDataReader.Read())
				{
					MySqlSchemaRow mySqlSchemaRow = schema.AddRow();
					mySqlSchemaRow["TABLE_CATALOG"] = null;
					mySqlSchemaRow["TABLE_SCHEMA"] = restrictions[1];
					mySqlSchemaRow["TABLE_NAME"] = mySqlDataReader.GetString(0);
					mySqlSchemaRow["TABLE_TYPE"] = value;
					mySqlSchemaRow["ENGINE"] = SchemaProvider.GetString(mySqlDataReader, 1);
					mySqlSchemaRow["VERSION"] = mySqlDataReader.GetValue(2);
					mySqlSchemaRow["ROW_FORMAT"] = SchemaProvider.GetString(mySqlDataReader, 3);
					mySqlSchemaRow["TABLE_ROWS"] = mySqlDataReader.GetValue(4);
					mySqlSchemaRow["AVG_ROW_LENGTH"] = mySqlDataReader.GetValue(5);
					mySqlSchemaRow["DATA_LENGTH"] = mySqlDataReader.GetValue(6);
					mySqlSchemaRow["MAX_DATA_LENGTH"] = mySqlDataReader.GetValue(7);
					mySqlSchemaRow["INDEX_LENGTH"] = mySqlDataReader.GetValue(8);
					mySqlSchemaRow["DATA_FREE"] = mySqlDataReader.GetValue(9);
					mySqlSchemaRow["AUTO_INCREMENT"] = mySqlDataReader.GetValue(10);
					mySqlSchemaRow["CREATE_TIME"] = mySqlDataReader.GetValue(11);
					mySqlSchemaRow["UPDATE_TIME"] = mySqlDataReader.GetValue(12);
					mySqlSchemaRow["CHECK_TIME"] = mySqlDataReader.GetValue(13);
					mySqlSchemaRow["TABLE_COLLATION"] = SchemaProvider.GetString(mySqlDataReader, 14);
					mySqlSchemaRow["CHECKSUM"] = mySqlDataReader.GetValue(15);
					mySqlSchemaRow["CREATE_OPTIONS"] = SchemaProvider.GetString(mySqlDataReader, 16);
					mySqlSchemaRow["TABLE_COMMENT"] = SchemaProvider.GetString(mySqlDataReader, 17);
				}
			}
		}
		private static string GetString(MySqlDataReader reader, int index)
		{
			if (reader.IsDBNull(index))
			{
				return null;
			}
			return reader.GetString(index);
		}
		public virtual MySqlSchemaCollection GetUDF(string[] restrictions)
		{
			string text = "SELECT name,ret,dl FROM mysql.func";
			if (restrictions != null && restrictions.Length >= 1 && !string.IsNullOrEmpty(restrictions[0]))
			{
				text += string.Format(" WHERE name LIKE '{0}'", restrictions[0]);
			}
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection("User-defined Functions");
			mySqlSchemaCollection.AddColumn("NAME", typeof(string));
			mySqlSchemaCollection.AddColumn("RETURN_TYPE", typeof(int));
			mySqlSchemaCollection.AddColumn("LIBRARY_NAME", typeof(string));
			MySqlCommand mySqlCommand = new MySqlCommand(text, this.connection);
			try
			{
				using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
				{
					while (mySqlDataReader.Read())
					{
						MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
						mySqlSchemaRow[0] = mySqlDataReader.GetString(0);
						mySqlSchemaRow[1] = mySqlDataReader.GetInt32(1);
						mySqlSchemaRow[2] = mySqlDataReader.GetString(2);
					}
				}
			}
			catch (MySqlException ex)
			{
				if (ex.Number != 1142)
				{
					throw;
				}
				throw new MySqlException(Resources.UnableToEnumerateUDF, ex);
			}
			return mySqlSchemaCollection;
		}
		protected virtual MySqlSchemaCollection GetSchemaInternal(string collection, string[] restrictions)
		{
			switch (collection)
			{
			case "METADATACOLLECTIONS":
				return this.GetCollections();
			case "DATASOURCEINFORMATION":
				return this.GetDataSourceInformation();
			case "DATATYPES":
				return SchemaProvider.GetDataTypes();
			case "RESTRICTIONS":
				return this.GetRestrictions();
			case "RESERVEDWORDS":
				return SchemaProvider.GetReservedWords();
			case "USERS":
				return this.GetUsers(restrictions);
			case "DATABASES":
				return this.GetDatabases(restrictions);
			case "UDF":
				return this.GetUDF(restrictions);
			}
			if (restrictions == null)
			{
				restrictions = new string[2];
			}
			if (this.connection != null && this.connection.Database != null && this.connection.Database.Length > 0 && restrictions.Length > 1 && restrictions[1] == null)
			{
				restrictions[1] = this.connection.Database;
			}
			if (collection != null)
			{
				if (collection == "TABLES")
				{
					return this.GetTables(restrictions);
				}
				if (collection == "COLUMNS")
				{
					return this.GetColumns(restrictions);
				}
				if (collection == "INDEXES")
				{
					return this.GetIndexes(restrictions);
				}
				if (collection == "INDEXCOLUMNS")
				{
					return this.GetIndexColumns(restrictions);
				}
				if (collection == "FOREIGN KEYS")
				{
					return this.GetForeignKeys(restrictions);
				}
				if (collection == "FOREIGN KEY COLUMNS")
				{
					return this.GetForeignKeyColumns(restrictions);
				}
			}
			return null;
		}
		internal string[] CleanRestrictions(string[] restrictionValues)
		{
			string[] array = null;
			if (restrictionValues != null)
			{
				array = (string[])restrictionValues.Clone();
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text != null)
					{
						array[i] = text.Trim(new char[]
						{
							'`'
						});
					}
				}
			}
			return array;
		}
		protected MySqlSchemaCollection QueryCollection(string name, string sql)
		{
			MySqlSchemaCollection mySqlSchemaCollection = new MySqlSchemaCollection(name);
			MySqlCommand mySqlCommand = new MySqlCommand(sql, this.connection);
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
			for (int i = 0; i < mySqlDataReader.FieldCount; i++)
			{
				mySqlSchemaCollection.AddColumn(mySqlDataReader.GetName(i), mySqlDataReader.GetFieldType(i));
			}
			using (mySqlDataReader)
			{
				while (mySqlDataReader.Read())
				{
					MySqlSchemaRow mySqlSchemaRow = mySqlSchemaCollection.AddRow();
					for (int j = 0; j < mySqlDataReader.FieldCount; j++)
					{
						mySqlSchemaRow[j] = mySqlDataReader.GetValue(j);
					}
				}
			}
			return mySqlSchemaCollection;
		}
	}
}
