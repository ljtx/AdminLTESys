using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ambiel.RedisClient
{
    public class RedisClient
    {
        private int dbNum { get; set; }
        private readonly ConnectionMultiplexer _conn;
        public string customKey;
        private IOptions<RedisConfigOptions> _config;
        public RedisClient(IOptions<RedisConfigOptions> config)
        {
            _config = config;
            _conn = RedisConnectionSingleton.GetConnectionMultiplexerInstance(config);
            customKey = string.IsNullOrWhiteSpace(config.Value.RedisKey) ? "ambiel" : config.Value.RedisKey;
            dbNum = 1;
        }
        #region 辅助方法
        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = customKey;
            return prefixKey + oldKey;
        }
        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _conn.GetDatabase(dbNum);
            return func(database);
        }
        private string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        private T ConvertObj<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        private List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }
        #endregion 辅助方法

        #region 同步方法
         /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringSet(key, value, expiry));
        }
        

        #endregion

    }
}