using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using LIB_Log;
namespace SettingPara
{
    public enum  DBExeResult
    {
        Successed=0,
        Failded=1,
        NoRecord=2
    }
    public class DBAccess_MySql
    {
        public WriteErrorLog Wlog = Program.Log_Agent_DB;
        public MySql.Data.MySqlClient.MySqlConnection connMysql = null;
        private MySqlTransaction myTranOne = null;
        public MySqlDataReader rec = null;
        public DBAccess_MySql(string ConnConfig)
        {
            try
            {
                var ConnStr = ConfigurationManager.ConnectionStrings[ConnConfig].ConnectionString;
                connMysql = new MySqlConnection(ConnStr);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("{0},建立连接失败.请检查配置文件是否存在 connectionStrings：{2}", ex.Message, ConnConfig));
            }
        }

        public DBAccess_MySql()
        {

        }

        public DBAccess_MySql(MySqlConnection mc, MySqlDataReader md)
        {
            this.connMysql = mc;
            this.rec = md;
        }

        public DBExeResult QuerySQL_ToTable(string Sql, out DataTable rst_dt, out string OutString)
        {
            OutString = "";
            rst_dt = new DataTable();
            try
            {
                if (myTranOne == null)
                    connMysql.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(Sql, connMysql);
                int rnt = adapter.Fill(rst_dt);
                if (myTranOne == null)
                    connMysql.Close();

                if (rst_dt.Rows.Count > 0)
                {
                    WriteLog(Enum_LogGrade.LogGrade_Nin, "QuerySQL_ToTable", Sql, string.Format("Result:{0}|Record:{1}", DBExeResult.Successed.ToString(), rst_dt.Rows.Count));
                    return DBExeResult.Successed;
                }
                else
                {
                    WriteLog(Enum_LogGrade.LogGrade_Nin, "QuerySQL_ToTable", Sql, string.Format("Result:{0}", DBExeResult.NoRecord.ToString()));
                    return DBExeResult.NoRecord;
                }
            }
            catch (Exception ex)
            {
                rst_dt = null;
                connMysql.Close();
                OutString = string.Format("{0},执行查询失败;SQL：{1}", ex.Message, Sql);
                WriteLog(Enum_LogGrade.LogGrade_Nin, "QuerySQL_ToTable", Sql, string.Format("Result:{0}|{1}", DBExeResult.Failded.ToString(), ex.Message));
                return DBExeResult.Failded;
            }
        }

        public DBExeResult ExecSql(string Sql, out string OutString)
        {
            OutString = "";
            try
            {
                if (myTranOne == null)
                    connMysql.Open();
                var mySqlCommand = new MySqlCommand(Sql, connMysql);
                var rltrow = mySqlCommand.ExecuteNonQuery();
                if (myTranOne == null)
                    connMysql.Close();
                if (rltrow >= 0)
                {
                    WriteLog(Enum_LogGrade.LogGrade_Nin, "ExecSql", Sql, string.Format("Result:{0}|Record:{1}", DBExeResult.Successed.ToString(), rltrow));
                    return DBExeResult.Successed;
                }
                else
                {
                    WriteLog(Enum_LogGrade.LogGrade_Nin, "ExecSql", Sql, string.Format("Result:{0}", DBExeResult.NoRecord.ToString()));
                    return DBExeResult.NoRecord;
                }

            }
            catch (Exception ex)
            {
                connMysql.Close();
                OutString = string.Format("{0},执行SQL失败;SQL：{2}", ex.Message, Sql);
                WriteLog(Enum_LogGrade.LogGrade_Nin, "ExecSql", Sql, string.Format("Result:{0}|{1}", DBExeResult.Failded.ToString(), ex.Message));
                return DBExeResult.Failded;
            }
        }

        public object QuerySQL_GetValue(string Sql, out string OutString)
        {
            OutString = "";
            object obj = null;
            try
            {
                if (myTranOne == null)
                    connMysql.Open();
                var mySqlCommand = new MySqlCommand(Sql, connMysql);
                obj = mySqlCommand.ExecuteScalar();
                if (myTranOne == null)
                    connMysql.Close();

                WriteLog(Enum_LogGrade.LogGrade_Nin, "ExecSql", Sql, string.Format("Result:{0}|{1}", DBExeResult.Successed.ToString(), obj));
            }
            catch (Exception ex)
            {
                connMysql.Close();
                OutString = string.Format("{0},执行SQL失败;SQL：{2}", ex.Message, Sql);
                WriteLog(Enum_LogGrade.LogGrade_Nin, "ExecSql", Sql, string.Format("Result:{0}|{1}", DBExeResult.Failded.ToString(), ex.Message));
            }
            return obj;
        }

        public DBAccess_MySql ReturnSQL_String(string Sql, out string OutString)
        {
            OutString = "";
            object obj = null;
            
            try
            {

            connMysql.Open();
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = connMysql;
            sqlCmd.CommandText = Sql;
            rec = sqlCmd.ExecuteReader();

            }catch(Exception ex)
            {
                connMysql.Close();
            }
            return new DBAccess_MySql(connMysql, rec);
        }

        public void StartTran()
        {
            if (myTranOne == null)
            {
                connMysql.Open();
                myTranOne = connMysql.BeginTransaction();

            }
        }

        public void CommitTran()
        {
            if (myTranOne != null && connMysql.State != ConnectionState.Closed)
            {
                myTranOne.Commit();
                myTranOne.Dispose();
                myTranOne = null;
                connMysql.Close();
            }
        }

        public void RollBack()
        {
            if (myTranOne != null && connMysql.State!=ConnectionState.Closed)
            {
                myTranOne.Rollback();
                myTranOne.Dispose();
                myTranOne = null;
                connMysql.Close();
            }
        }

        public void WriteLog(Enum_LogGrade logGrade, string funName,string Sql,string Info)
        {
            object obj = ConfigurationManager.AppSettings["PrintSQLLOG"];

            if (Wlog != null && obj != null &&  bool.Parse(obj.ToString()))
            {
                Wlog.WriteLog(Enum_LogType.LogType_Debug, logGrade, Enum_LogMessageType.LogMsgType_Event, funName, Sql, Info);
            }
        }

        public DataTable QueryDataTable(string sql, out string outString)
        {
            outString = "";
            DataTable dt = new DataTable();
            connMysql.Open();
            MySqlDataAdapter msd = new MySqlDataAdapter(sql, connMysql);
            int rnt = msd.Fill(dt);
            connMysql.Close();
            return dt;
        }

    }
}
