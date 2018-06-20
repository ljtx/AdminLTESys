using System.Data;

namespace Ambiel.AdoNet
{
    public class SqlParam
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string _param { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public DbType _dbtype { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object _value { get; set; }

        
        public SqlParam(string param, DbType dbtype, object value)
        {
            this._param = param;
            this._dbtype = dbtype;
            this._value = value; 
        }
        public SqlParam(string param,  object value)
        {
            this._param = param;
            this._value = value; 
        }
    }
}