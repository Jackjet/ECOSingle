using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Threading.Tasks;
namespace MySql.Data.MySqlClient
{
	[Designer("MySql.Data.MySqlClient.Design.MySqlDataAdapterDesigner,MySqlClient.Design"), DesignerCategory("Code"), ToolboxBitmap(typeof(MySqlDataAdapter), "MySqlClient.resources.dataadapter.bmp")]
	public sealed class MySqlDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
	{
		private bool loadingDefaults;
		private int updateBatchSize;
		private List<IDbCommand> commandBatch;
		public event MySqlRowUpdatingEventHandler RowUpdating;
		public event MySqlRowUpdatedEventHandler RowUpdated;
		[Description("Used during Update for deleted rows in Dataset.")]
		public new MySqlCommand DeleteCommand
		{
			get
			{
				return (MySqlCommand)base.DeleteCommand;
			}
			set
			{
				base.DeleteCommand = value;
			}
		}
		[Description("Used during Update for new rows in Dataset.")]
		public new MySqlCommand InsertCommand
		{
			get
			{
				return (MySqlCommand)base.InsertCommand;
			}
			set
			{
				base.InsertCommand = value;
			}
		}
		[Category("Fill"), Description("Used during Fill/FillSchema")]
		public new MySqlCommand SelectCommand
		{
			get
			{
				return (MySqlCommand)base.SelectCommand;
			}
			set
			{
				base.SelectCommand = value;
			}
		}
		[Description("Used during Update for modified rows in Dataset.")]
		public new MySqlCommand UpdateCommand
		{
			get
			{
				return (MySqlCommand)base.UpdateCommand;
			}
			set
			{
				base.UpdateCommand = value;
			}
		}
		internal bool LoadDefaults
		{
			get
			{
				return this.loadingDefaults;
			}
			set
			{
				this.loadingDefaults = value;
			}
		}
		public override int UpdateBatchSize
		{
			get
			{
				return this.updateBatchSize;
			}
			set
			{
				this.updateBatchSize = value;
			}
		}
		public MySqlDataAdapter()
		{
			this.loadingDefaults = true;
			this.updateBatchSize = 1;
		}
		public MySqlDataAdapter(MySqlCommand selectCommand) : this()
		{
			this.SelectCommand = selectCommand;
		}
		public MySqlDataAdapter(string selectCommandText, MySqlConnection connection) : this()
		{
			this.SelectCommand = new MySqlCommand(selectCommandText, connection);
		}
		public MySqlDataAdapter(string selectCommandText, string selectConnString) : this()
		{
			this.SelectCommand = new MySqlCommand(selectCommandText, new MySqlConnection(selectConnString));
		}
		private void OpenConnectionIfClosed(DataRowState state, List<MySqlConnection> openedConnections)
		{
			MySqlCommand mySqlCommand;
			if (state != DataRowState.Added)
			{
				if (state != DataRowState.Deleted)
				{
					if (state != DataRowState.Modified)
					{
						return;
					}
					mySqlCommand = this.UpdateCommand;
				}
				else
				{
					mySqlCommand = this.DeleteCommand;
				}
			}
			else
			{
				mySqlCommand = this.InsertCommand;
			}
			if (mySqlCommand != null && mySqlCommand.Connection != null && mySqlCommand.Connection.connectionState == ConnectionState.Closed)
			{
				mySqlCommand.Connection.Open();
				openedConnections.Add(mySqlCommand.Connection);
			}
		}
		protected override int Update(DataRow[] dataRows, DataTableMapping tableMapping)
		{
			List<MySqlConnection> list = new List<MySqlConnection>();
			int result;
			try
			{
				for (int i = 0; i < dataRows.Length; i++)
				{
					DataRow dataRow = dataRows[i];
					this.OpenConnectionIfClosed(dataRow.RowState, list);
				}
				int num = base.Update(dataRows, tableMapping);
				result = num;
			}
			finally
			{
				foreach (MySqlConnection current in list)
				{
					current.Close();
				}
			}
			return result;
		}
		protected override void InitializeBatching()
		{
			this.commandBatch = new List<IDbCommand>();
		}
		protected override int AddToBatch(IDbCommand command)
		{
			MySqlCommand mySqlCommand = (MySqlCommand)command;
			if (mySqlCommand.BatchableCommandText == null)
			{
				mySqlCommand.GetCommandTextForBatching();
			}
			IDbCommand item = (IDbCommand)((ICloneable)command).Clone();
			this.commandBatch.Add(item);
			return this.commandBatch.Count - 1;
		}
		protected override int ExecuteBatch()
		{
			int num = 0;
			int i = 0;
			while (i < this.commandBatch.Count)
			{
				MySqlCommand mySqlCommand = (MySqlCommand)this.commandBatch[i++];
				int j = i;
				while (j < this.commandBatch.Count)
				{
					MySqlCommand mySqlCommand2 = (MySqlCommand)this.commandBatch[j];
					if (mySqlCommand2.BatchableCommandText == null || mySqlCommand2.CommandText != mySqlCommand.CommandText)
					{
						break;
					}
					mySqlCommand.AddToBatch(mySqlCommand2);
					j++;
					i++;
				}
				num += mySqlCommand.ExecuteNonQuery();
			}
			return num;
		}
		protected override void ClearBatch()
		{
			if (this.commandBatch.Count > 0)
			{
				MySqlCommand mySqlCommand = (MySqlCommand)this.commandBatch[0];
				if (mySqlCommand.Batch != null)
				{
					mySqlCommand.Batch.Clear();
				}
			}
			this.commandBatch.Clear();
		}
		protected override void TerminateBatching()
		{
			this.ClearBatch();
			this.commandBatch = null;
		}
		protected override IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
		{
			return (IDataParameter)this.commandBatch[commandIdentifier].Parameters[parameterIndex];
		}
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new MySqlRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new MySqlRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			if (this.RowUpdating != null)
			{
				this.RowUpdating(this, value as MySqlRowUpdatingEventArgs);
			}
		}
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			if (this.RowUpdated != null)
			{
				this.RowUpdated(this, value as MySqlRowUpdatedEventArgs);
			}
		}
		public Task<int> FillAsync(DataSet dataSet)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod3(dataSet));
		}
		public Task<int> FillAsync(DataTable dataTable)
		{
			return Task.Factory.StartNew<int>(() => this.Fill(dataTable));
		}
		public Task<int> FillAsync(DataSet dataSet, string srcTable)
		{
			return Task.Factory.StartNew<int>(() => this.Fill(dataSet, srcTable));
		}
		public Task<int> FillAsync(DataTable dataTable, IDataReader dataReader)
		{
			return Task.Factory.StartNew<int>(() => this.n__FabricatedMethodd(dataTable, dataReader));
		}

        private void n__FabricatedMethodd(DataTable dataTable, IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
		public Task<int> FillAsync(DataTable dataTable, IDbCommand command, CommandBehavior behavior)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod11(dataTable, command, behavior));
		}
		public Task<int> FillAsync(int startRecord, int maxRecords, params DataTable[] dataTables)
		{
			return Task.Factory.StartNew<int>(() => this.Fill(startRecord, maxRecords, dataTables));
		}
		public Task<int> FillAsync(DataSet dataSet, int startRecord, int maxRecords, string srcTable)
		{
			return Task.Factory.StartNew<int>(() => this.Fill(dataSet, startRecord, maxRecords, srcTable));
		}
		public Task<int> FillAsync(DataSet dataSet, string srcTable, IDataReader dataReader, int startRecord, int maxRecords)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod1b(dataSet, srcTable, dataReader, startRecord, maxRecords));
		}
		public Task<int> FillAsync(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command, CommandBehavior behavior)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod1f(dataTables, startRecord, maxRecords, command, behavior));
		}
		public Task<int> FillAsync(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod23(dataSet, startRecord, maxRecords, srcTable, command, behavior));
		}
		public Task<DataTable[]> FillSchemaAsync(DataSet dataSet, SchemaType schemaType)
		{
			return Task.Factory.StartNew<DataTable[]>(() => this.<>n__FabricatedMethod27(dataSet, schemaType));
		}
		public Task<DataTable[]> FillSchemaAsync(DataSet dataSet, SchemaType schemaType, string srcTable)
		{
			return Task.Factory.StartNew<DataTable[]>(() => this.FillSchema(dataSet, schemaType, srcTable));
		}
		public Task<DataTable[]> FillSchemaAsync(DataSet dataSet, SchemaType schemaType, string srcTable, IDataReader dataReader)
		{
			return Task.Factory.StartNew<DataTable[]>(() => this.<>n__FabricatedMethod2e(dataSet, schemaType, srcTable, dataReader));
		}
		public Task<DataTable[]> FillSchemaAsync(DataSet dataSet, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior)
		{
			return Task.Factory.StartNew<DataTable[]>(() => this.<>n__FabricatedMethod32(dataSet, schemaType, command, srcTable, behavior));
		}
		public Task<DataTable> FillSchemaAsync(DataTable dataTable, SchemaType schemaType)
		{
			return Task.Factory.StartNew<DataTable>(() => this.FillSchema(dataTable, schemaType));
		}
		public Task<DataTable> FillSchemaAsync(DataTable dataTable, SchemaType schemaType, IDataReader dataReader)
		{
			return Task.Factory.StartNew<DataTable>(() => this.<>n__FabricatedMethod39(dataTable, schemaType, dataReader));
		}
		public Task<DataTable> FillSchemaAsync(DataTable dataTable, SchemaType schemaType, IDbCommand command, CommandBehavior behavior)
		{
			return Task.Factory.StartNew<DataTable>(() => this.<>n__FabricatedMethod3d(dataTable, schemaType, command, behavior));
		}
		public Task<int> UpdateAsync(DataRow[] dataRows)
		{
			return Task.Factory.StartNew<int>(() => this.Update(dataRows));
		}
		public Task<int> UpdateAsync(DataSet dataSet)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod44(dataSet));
		}
		public Task<int> UpdateAsync(DataTable dataTable)
		{
			return Task.Factory.StartNew<int>(() => this.Update(dataTable));
		}
		public Task<int> UpdateAsync(DataRow[] dataRows, DataTableMapping tableMapping)
		{
			return Task.Factory.StartNew<int>(() => this.<>n__FabricatedMethod4b(dataRows, tableMapping));
		}
		public Task<int> UpdateAsync(DataSet dataSet, string srcTable)
		{
			return Task.Factory.StartNew<int>(() => this.Update(dataSet, srcTable));
		}
	}
}
