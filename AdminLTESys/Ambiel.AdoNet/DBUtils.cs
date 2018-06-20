using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Ambiel.AdoNet
{
    public static class DBUtils
    {
        private static readonly string strConn = "server=localhost;database=TestDb2;user=root;password=888888;";
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connectionString = string.Empty;
        /// <summary>
        /// 支持数据库字符串
        /// </summary>
        private static string provider = string.Empty;

        /// <summary>
        /// 数据库事物对象
        /// </summary>
        private static IDbTransaction m_Transaction = null;
        /// <summary>
        /// 数据库类型
        /// </summary>
        private static DataBaseType.DatabaseType dataBaseType = DataBaseType.DatabaseType.MYSQL;

        /// <summary>
        /// 数据库工厂对象,生成相应的数据库操作对象
        /// </summary>
        private static DbFactory dbFactory { get; set; }

      
        /// <summary>
        /// 获取当前的持久层对象
        /// </summary>
        /// <returns></returns>
         static DBUtils ()
        {
            if (!string.IsNullOrEmpty(strConn))
            {
                ConnectDB(strConn);
            }             
        }

        /// <summary>
        /// 根据连接类型名进行连接配置
        /// </summary>
        /// <param name="connName"></param>
        public static void ConnectDB(string connName)
        {
            if (connName == null) return;

            //ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["Default"];

            provider = "MySqlClient";
            connectionString = strConn;

            if (provider.Contains("MySqlClient"))
            {
                dataBaseType = DataBaseType.DatabaseType.MYSQL;
            }
            else if (provider.Contains("SqlClient"))
            {
                dataBaseType = DataBaseType.DatabaseType.SQLSERVER;
            }            

            dbFactory = DbFactory.NewInstance(connectionString, dataBaseType);
        }

        /// <summary>
        /// 创建合适的sqlparameter
        /// </summary>
        /// <param name="commandParameter"></param>
        public static List<IDbDataParameter> CreateDataparameters(SqlParam commandParameter)
        {
            IDbDataParameter trueCommandParameter = null;
            string character = dbFactory.CreateDbParamCharacter();
            List<IDbDataParameter> trueCommandParameters = new  List<IDbDataParameter>();
            switch (dataBaseType)
            {
                case DataBaseType.DatabaseType.MYSQL:
                    string param = commandParameter._param.Replace("@", character);
                    trueCommandParameter =
                        new MySqlParameter(param, commandParameter._value);
                    trueCommandParameters.Add(trueCommandParameter);
                    break;
                 default:
                     trueCommandParameter =
                         new SqlParameter(commandParameter._param, commandParameter._value);
                     trueCommandParameters.Add(trueCommandParameter);
                     break;
            }

           return trueCommandParameters;
        }
        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <param name="dbFactory">数据库配置对象</param>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            int val = 0;
            using (IDbConnection connection = dbFactory.CreateDbConnection())
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    PrepareCommand(dbFactory, cmd, connection, null, cmdType, cmdText, null);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            return val;
        }
        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters"></param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery( CommandType cmdType, string cmdText, params SqlParam[] commandParameters)
        {
            List<IDbDataParameter> trueCommandParameters = null;
            int val = 0;
            using (IDbConnection connection = dbFactory.CreateDbConnection())
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    foreach (var item in commandParameters)
                    {
                        trueCommandParameters = CreateDataparameters(item);
                    }
                    PrepareCommand(dbFactory, cmd, connection,null, cmdType, cmdText,  trueCommandParameters);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            return val;
        }
        /// <summary>
        ///通过提供的参数，执行无结果集返回的数据库操作命令
        ///并返回执行数据库操作所影响的行数。
        /// </summary>
        /// <param name="trans">sql事务对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回通过执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(IDbTransaction trans, CommandType cmdType, string cmdText, params SqlParam[] commandParameters)
        {
            int val = 0;
            List<IDbDataParameter> trueCommandParameters = null;
            using (IDbConnection conn = dbFactory.CreateDbConnection())
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    if (trans == null || trans.Connection == null)
                    {
                        foreach (var item in commandParameters)
                        {
                            trueCommandParameters = CreateDataparameters(item);
                        }

                        PrepareCommand(dbFactory, cmd, conn, trans, cmdType, cmdText, trueCommandParameters);
                        val = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        foreach (var item in commandParameters)
                        {
                            trueCommandParameters = CreateDataparameters(item);
                        }

                        PrepareCommand(dbFactory, cmd, trans.Connection, trans, cmdType, cmdText,
                            trueCommandParameters);
                        val = cmd.ExecuteNonQuery();
                    }
                    cmd.Parameters.Clear();
                }
            }
            return val;
        }
        /// <summary>
        /// 使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(DbFactory dbFactory, CommandType cmdType, string cmdText, params SqlParam[] commandParameters)
        {
            
            IDbConnection conn = dbFactory.CreateDbConnection();
            //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
            //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
            //将不会执行。
            try
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    List<IDbDataParameter> trueCommandParameters = null;
                    foreach (var item in commandParameters)
                    {
                        trueCommandParameters = CreateDataparameters(item);
                    }

                    PrepareCommand(dbFactory, cmd, conn, null, cmdType, cmdText, trueCommandParameters);
                    IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return rdr;
                }
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        ///使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader( CommandType cmdType, string cmdText)
        {
           
            IDbConnection conn = dbFactory.CreateDbConnection();

            //我们在这里使用一个 try/catch,因为如果PrepareCommand方法抛出一个异常，我们想在捕获代码里面关闭
            //connection连接对象，因为异常发生datareader将不会存在，所以commandBehaviour.CloseConnection
            //将不会执行。
            try
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    PrepareCommand(dbFactory, cmd, conn, null, cmdType, cmdText, null);
                    IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return rdr;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }
        /// <summary>
        ///使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(IDbTransaction trans, CommandType cmdType, string cmdText)
        {
            IDbConnection conn = trans.Connection;
            try
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    PrepareCommand(dbFactory, cmd, conn,trans, cmdType, cmdText, null);
                    IDataReader rdr = cmd.ExecuteReader();
                    cmd.Parameters.Clear();
                    return rdr;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }
        /// <summary>
        ///使用提供的参数，执行有结果集返回的数据库操作命令
        /// 并返回SqlDataReader对象
        /// </summary>
        /// <param name="closeConnection">读取完关闭Reader是否同时也关闭数据库连接</param>
        /// <param name="connection">数据库链接</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader( bool closeConnection, IDbConnection connection, CommandType cmdType, string cmdText)
        {
           
            IDbConnection conn = connection;

            try
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    PrepareCommand(dbFactory, cmd, conn, null, cmdType, cmdText, null);
                    IDataReader rdr = closeConnection
                        ? cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        : cmd.ExecuteReader();
                    cmd.Parameters.Clear();
                    return rdr;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet dataSet( CommandType cmdType, string cmdText)
        {
            DataSet ds = new DataSet();
            IDbCommand cmd = dbFactory.CreateDbCommand();
            IDbConnection conn = dbFactory.CreateDbConnection();
            try
            {
                PrepareCommand(dbFactory, cmd, conn, null, cmdType, cmdText, null);
                IDbDataAdapter sda = dbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch
            {
                conn.Close();
                cmd.Dispose();
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
        /// <summary>
        ///依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="dbFactory">数据库配置对象</param>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar( CommandType cmdType, string cmdText)
        {
           
            using (IDbConnection connection = dbFactory.CreateDbConnection())
            {
                using (IDbCommand cmd = dbFactory.CreateDbCommand())
                {
                    PrepareCommand(dbFactory, cmd, connection, null, cmdType, cmdText, null);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }
       
        /// <summary>
        ///依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="trans">数据库事物对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="commandParameters">执行命令所需的参数数组</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        /*public static object ExecuteScalar( IDbTransaction trans, CommandType cmdType, string cmdText, params SqlParam[] commandParameters)
        {
            IDbCommand cmd = dbFactory.CreateDbCommand();
            List<IDbDataParameter> trueCommandParameters = null;
            foreach (var item in commandParameters)
            {
                trueCommandParameters=  CreateDataparameters(item);
            }
            PrepareCommand(dbFactory, cmd, trans.Connection, trans, cmdType, cmdText, trueCommandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }*/

        /// <summary>
        /// 执行命令前的准备工作
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(DbFactory dbFactory, IDbCommand cmd, IDbConnection conn,
            IDbTransaction trans, CommandType cmdType, string cmdText, List<IDbDataParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (IDbDataParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}