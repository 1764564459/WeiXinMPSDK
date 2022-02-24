using Senparc.NeuChar.MessageHandlers;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.Sample.MP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//ʹ�ñ��ػ���������
builder.Services.AddMemoryCache();

//Senparc.Weixin ע�ᣨ���룩
builder.Services.AddSenparcWeixinServices(builder.Configuration);

var app = builder.Build();


var senparcWeixinSetting = app.Services.GetService<Microsoft.Extensions.Options.IOptions<Senparc.Weixin.Entities.SenparcWeixinSetting>>()!.Value;

//����΢�����ã����룩
var registerService = app.UseSenparcWeixin(app.Environment, null, null, register => { }, (register, weixinSetting) =>
{
    //ע�ṫ�ں���Ϣ������ִ�ж��ע�������ںţ�
    register.RegisterMpAccount(weixinSetting, "��ʢ������С���֡����ں�");
});

#region ʹ�� MessageHadler �м��������ȡ������������ Controller
//MessageHandler �м�����ܣ�https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html

//ʹ�ù��ںŵ� MessageHandler �м����������Ҫ���� Controller��                       --DPBMARK MP
app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
{
    /* ˵����
     * 1���˴��������ʾ�˽�Ϊȫ��Ĺ��ܵ㣬�򻯵�ʹ�ÿ��Բο�����С�������ҵ΢��
     * 2��ʹ���м��Ҳ֧�ֶ��˺ţ�����ʹ�� URL ��Ӳ����ķ�ʽ���磺/Weixin?id=1����
     *    ��options.AccountSettingFunc = context => {...} �У��� context.Request �л�ȡ [id] ֵ��
     *    ��������Ӧ�� senparcWeixinSetting ��Ϣ
     */

    #region ���� SenparcWeixinSetting ���������Զ��ṩ Token��EncodingAESKey �Ȳ���

    //�˴�Ϊί�У����Ը���������̬�ж��������������룩
    options.AccountSettingFunc = context =>
        //����һ��ʹ��Ĭ������
        Senparc.Weixin.Config.SenparcWeixinSetting;

    //��������ʹ��ָ�����ã�
    //Config.SenparcWeixinSetting["<Your SenparcWeixinSetting's name filled with Token, AppId and EncodingAESKey>"]; 

    //����������� context ������̬�жϷ���Settingֵ

    #endregion

    //�� MessageHandler ���첽����δ�ṩ��дʱ������ͬ�����������裩
    options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;

    options.EnableRequestLog = true;//Ĭ�Ͼ�Ϊ true������ر���־��������Ϊ false
    options.EnbleResponseLog = true;//Ĭ�Ͼ�Ϊ true������ر���־��������Ϊ false

    //�Է����쳣���д�����ѡ��
    options.AggregateExceptionCatch = ex =>
    {
        //�߼�����...
        return false;//ϵͳ�����׳��쳣
    };
});
#endregion


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
