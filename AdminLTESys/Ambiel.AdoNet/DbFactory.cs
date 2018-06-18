using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Ambiel.AdoNet
{
    public class DbFactory
    {
        public static List<DbFactory> dbFactorylist { get; set; }

        private DbFactory()
        {
            
        }

        /// <summary>
        /// 根据数据库连接字符串以及数据库类型创建一个数据库工厂对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static DbFactory NewInstance(string connectionString, DataBaseType.DatabaseType dbType)
        {
            if(dbFactorylist==null)
            dbFactorylist = new List<DbFactory>();
            if (dbFactorylist.Count == 0)
            {
                DbFactory factory = new DbFactory();
                factory.connectionString = connectionString;
                factory.dbType = dbType;
                factory.DbParmChar = factory.CreateDbParamCharacter();
                dbFactorylist.Add(factory);
            }
            else
            {
                if (dbFactorylist[0].connectionString != connectionString)
                {
                    dbFactorylist[0].connectionString = connectionString;
                    dbFactorylist[0].dbType = dbType;
                    dbFactorylist[0].DbParmChar = dbFactorylist[0].CreateDbParamCharacter();
                }
            }
            return dbFactorylist[0];
        }

        private string connectionString;
        private DataBaseType.DatabaseType dbType;
        private string dbParmChar;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType.DatabaseType DbType
        {
            get { return dbType; }
            set { value = dbType; }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { value = connectionString; }
        }

        /// <summary>
        /// 参数的前缀字符
        /// </summary>
        public string DbParmChar
        {
            get { return dbParmChar; }
            set { dbParmChar = value; }
        }

         /// <summary>
         /// 创建数据库sqlparameter符号
         /// </summary>
         /// <returns></returns>
        public  string CreateDbParamCharacter()
         {
             string character = string.Empty;
             switch (dbType)
             {
                 case  DataBaseType.DatabaseType.SQLSERVER:
                     character = "@";
                     break;
                 case  DataBaseType.DatabaseType.MYSQL:
                     character = "?";
                     break;
                 default:
                     throw new Exception("未找到对应的数据库配置");
             }

             return character;
         }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型和传入的
        /// 数据库链接字符串来创建相应数据库连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateDbConnection()
        {
            IDbConnection conn = null;
            switch (dbType)
            {
                case DataBaseType.DatabaseType.SQLSERVER:
                    conn = new SqlConnection(connectionString);
                    break;              
                case DataBaseType.DatabaseType.MYSQL:
                    conn = new MySqlConnection(connectionString);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }

            return conn;
        }
        /// <summary>
        /// 创建DbCommand
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public  IDbCommand CreateDbCommand()
        {
            IDbCommand cmd = null;
            switch (dbType)
            {
                case DataBaseType.DatabaseType.SQLSERVER:
                    cmd = new SqlCommand();
                    break;
                case DataBaseType.DatabaseType.MYSQL:
                    cmd = new MySqlCommand();
                    break;
                default:
                    throw new Exception("");
            }
            return cmd;
        }
        /// <summary>
        /// 创建dataadapter
        /// </summary>
        /// <returns></returns>
        public  IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            IDbDataAdapter adapter = null;
            switch (dbType)
            {
                case DataBaseType.DatabaseType.SQLSERVER:
                    adapter = new SqlDataAdapter((SqlCommand)cmd);
                    break;               
                case DataBaseType.DatabaseType.MYSQL:
                    adapter = new MySqlDataAdapter((MySqlCommand)cmd);
                    break;
                default: throw new Exception("未找到数据配置");
            }
            return adapter;
        }
        /// <summary>
        /// 创建DbParameter，只创建不赋值
        /// </summary>
        /// <returns></returns>
        public  IDbDataParameter CreateDbParameter()
        {
            IDbDataParameter param = null;
            switch (dbType)
            {
                case DataBaseType.DatabaseType.SQLSERVER:
                    param = new SqlParameter();
                    break;
              
                case DataBaseType.DatabaseType.MYSQL:
                    param = new MySqlParameter();
                    break;
                default:
                    throw new Exception("未找到对应的数据库配置");
            }

            return param;
        }
        /// <summary>
        /// 创建DbParameter,赋值
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value)
        {
            IDbDataParameter param = CreateDbParameter();
            param.ParameterName = paramName;
            param.Value = value;

            return param;
        }
        /// <summary>
        /// 创建DbParameter,赋值 
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value, DbType _dataType)
        {
            IDbDataParameter param = CreateDbParameter();
            param.DbType = _dataType;
            param.ParameterName = paramName;
            param.Value = value;

            return param;
        }
        /// <summary>
        /// 创建DbParameter,赋值 
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value, ParameterDirection direction)
        {           
            IDbDataParameter param = CreateDbParameter();
            param.Direction = direction;
            param.ParameterName = paramName;
            param.Value = value;

            return param;
        }
        /// <summary>
        /// 创建DbParameter,赋值
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value, int size, ParameterDirection direction)
        {
            IDbDataParameter param = CreateDbParameter();
            param.Direction = direction;
            param.ParameterName = paramName;
            param.Value = value;
            param.Size = size;

            return param;
        }
        /// <summary>
        /// 创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateDbParameter(string paramName, object value, DbType _dataType, ParameterDirection direction)
        {
            IDbDataParameter param = CreateDbParameter();
            param.Direction = direction;
            param.DbType = _dataType;
            param.ParameterName = paramName;
            param.Value = value;

            return param;
        }

        /// <summary>
        /// 创建相应数据库的参数数组对象
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter[] CreateDbParameters(int size)
        {
            int i = 0;
            IDbDataParameter[] param = null;
            switch (dbType)
            {
                case DataBaseType.DatabaseType.SQLSERVER:
                    param = new SqlParameter[size];
                    while (i < size) { param[i] = new SqlParameter(); i++; }
                    break;
               
                case DataBaseType.DatabaseType.MYSQL:
                    param = new MySqlParameter[size];
                    while (i < size) { param[i] = new MySqlParameter(); i++; }
                    break;              
                default:
                    throw new Exception("数据库类型目前不支持！");
            }

            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的事物对象
        /// </summary>
        /// <returns></returns>
        public IDbTransaction CreateDbTransaction()
        {
            IDbConnection conn = CreateDbConnection();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn.BeginTransaction();
        }


        /// <summary>
        /// 创建数据库的事物对象
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public IDbTransaction CreateDbTransaction(System.Data.IsolationLevel level)
        {
            IDbConnection conn = CreateDbConnection();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn.BeginTransaction(level);
        } 

    }
}