using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using Senparc.WeixinTests;
using System;
using System.IO;
using System.Text;

namespace Senparc.Weixin.TenPayV3.Test
{
    [TestClass]
    public class BaseTenPayTest
    {
        protected static IServiceProvider _serviceProvider;

        protected static SenparcSetting _senparcSetting;
        protected static SenparcWeixinSetting _senparcWeixinSetting;

        public BaseTenPayTest()
        {
            RegisterStart();
        }

        /// <summary>
        /// ����Ĭ��ע������
        /// </summary>
        protected void RegisterStart()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//֧�� GB2312

            //ע�Ὺʼ
            RegisterService register;

            //ע�� CON2ET ȫ��
            var senparcSetting = new SenparcSetting() { IsDebug = true };

            var mockEnv = new Mock<IWebHostEnvironment>();

            mockEnv.Setup(z => z.ContentRootPath).Returns(() => UnitTestHelper.RootPath);
            register = Senparc.CO2NET.AspNet.RegisterServices.RegisterService.Start(mockEnv.Object, senparcSetting);

            RegisterServiceCollection();

            register.UseSenparcGlobal(false);

            //ע��΢��
            //var senparcWeixinSetting = new SenparcWeixinSetting(true);
            register.UseSenparcWeixin(_senparcWeixinSetting, senparcSetting);
            register.ChangeDefaultCacheNamespace("Senparc.Weixin Test Cache");
        }

#if NETSTANDARD2_0 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_1 || NET6_0
        /// <summary>
        /// ע�� IServiceCollection �� MemoryCache
        /// </summary>
        public static void RegisterServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            var configBuilder = new ConfigurationBuilder();

            var appSettingsFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UnitTestHelper.RootPath, "appsettings.json"));
            var appSettingsExisted = File.Exists(appSettingsFilePath);

            if (appSettingsExisted)
            {
                configBuilder.AddJsonFile("appsettings.json", false, false);
            }

            var config = configBuilder.Build();

            if (appSettingsExisted)
            {
                _senparcSetting = new SenparcSetting() { IsDebug = true };
                _senparcWeixinSetting = new SenparcWeixinSetting() { IsDebug = true };
                config.GetSection("SenparcSetting").Bind(_senparcSetting);
                config.GetSection("SenparcWeixinSetting").Bind(_senparcWeixinSetting);
            }

            serviceCollection.AddMemoryCache();//ʹ���ڴ滺��

            //�Ѿ����� AddSenparcGlobalServices()��ע�⣺����������ע����ɺ�ִ��
            serviceCollection.AddSenparcWeixinServices(config);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
#endif

        /// <summary>
        /// ��ȡ������Ŀ��Ŀ¼�����·��
        /// </summary>
        /// <returns></returns>
        protected string GetParentRootRelativePath()
        {
            return @"..\..\..\";
        }
    }
}
