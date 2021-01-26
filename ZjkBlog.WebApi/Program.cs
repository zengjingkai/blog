using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ZjkBlog.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                    Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(p =>
                    {
                        var path = System.IO.Directory.GetCurrentDirectory();
                        p.AddLog4Net($"{path}/config/log4net.config");//�����ļ�
                        p.AddFilter("System", LogLevel.Warning);//���������ռ�
                        p.AddFilter("Microsoft",LogLevel.Warning);//���˵�ϵͳĬ�ϵ�һЩ��־
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
