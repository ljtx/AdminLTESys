using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdminLTESys.Common
{
    public class JsonConfigurationHelper
    {
        public static T GetAppSetting<T>(string key,string path="appsettings.json") where T : class, new()
        {
            var curClassPath = Directory.GetCurrentDirectory();
            IConfiguration config = new  ConfigurationBuilder()
                .SetBasePath(curClassPath)
                .Add(new JsonConfigurationSource { Path = path, Optional = false, ReloadOnChange = true })
                .Build();
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }
    }
}