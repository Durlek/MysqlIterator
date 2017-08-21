using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MysqlIterator
{
	internal class Searcher
	{
		private string db;
		private string connString;
		private MySqlConnection conn;
		
		private List<string> tables;

		public Searcher(string db)
		{
			this.db = db;
			this.connString  = "server=localhost; user id=test; password=test; database=" + db +"; pooling=false; convert zero datetime=True;";
			this.conn = new MySqlConnection(connString);
		}

		internal void search(string target) {
			MySqlCommand cmd = new MySqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='" + db + "'", conn);
			tables = new List<string>();
			if(cmd.Connection.State != System.Data.ConnectionState.Open) {
				cmd.Connection.Open();
			}

			MySqlDataReader R = cmd.ExecuteReader();

			while(R.Read())
				tables.Add(R.GetString(0));

			R.Close();

			foreach(string table in tables)
			{
				List<string> columns = new List<string>();
				//print("TABLE: " + table);
				cmd.CommandText = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
									WHERE TABLE_NAME='" + table + "' AND TABLE_SCHEMA = '" + db + "'";

				if(cmd.Connection.State != System.Data.ConnectionState.Open) {
					cmd.Connection.Open();
				}

				R = cmd.ExecuteReader();

				while(R.Read())
					columns.Add(R.GetString(0));

				R.Close();

				foreach(string column in columns)
				{
					try {
						cmd.CommandText =	"SELECT `" + columns[0] + "` FROM `" + table + @"`
											WHERE `" + column + "`='" + target + "' LIMIT 1";

						if(cmd.Connection.State != System.Data.ConnectionState.Open) {
							cmd.Connection.Open();
						}

						R = cmd.ExecuteReader();

						while(R.Read())
							print(table + "." + column + ":   " + columns[0] + ":" + R.GetString(0));

						R.Close();
					}
					catch(MySqlException e)
					{
						print(e.ToString());
						print(cmd.CommandText);
						break;
					}
				}
			}
		}
		private static void print(string s) {
			Console.WriteLine(s);
		}
	}
}