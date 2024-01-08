using BotManager.Common.Scripting.Attributes;
using BotManager.Common;
using BotManager.Service.Discord;
using BotManager.Service.Misskey;
using BotManager.Service.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotManager.Engine;

namespace bot_manager_script.bots
{
    [Bot]
    public class ChikaBot : IBot, IServiceBot<IDiscordServiceClient>, IServiceBot<ITwitterServiceClient>, IServiceBot<IMisskeyServiceClient>
    {
        #region フィールド
        private readonly IDiscordServiceClient discordClient;
        private readonly ITwitterServiceClient twitterClient;
        private readonly IMisskeyServiceClient misskeyClient;
        #endregion
        #region プロパティ
        public string Id { get; init; }

        public string Name { get; init; }

        public string NamePattern { get; init; }

        IDiscordServiceClient IServiceBot<IDiscordServiceClient>.Client => discordClient;

        ITwitterServiceClient IServiceBot<ITwitterServiceClient>.Client => twitterClient;

        IMisskeyServiceClient IServiceBot<IMisskeyServiceClient>.Client => misskeyClient;

        #endregion

        #region コンストラクタ
        public ChikaBot()
        {
            // プロフィール設定
            var botSetting = AppSettings.GetBotDictionary()["chika"];
            Id = botSetting.Id;
            Name = botSetting.Name;
            NamePattern = botSetting.NamePattern;

            // サービスのセットアップ
            var discordCertification = botSetting.DiscordSetting!.Certificate;
            discordClient = DiscordService.Create(discordCertification.Token);

            var twitterCertification = botSetting.TwitterSetting!.Certificate;
            twitterClient = TwitterService.Create(twitterCertification.ConsumerKey, twitterCertification.ConsumerSecret, twitterCertification.AccessToken, twitterCertification.AccessTokenSecret);

            var misskeyCertification = botSetting.MisskeySetting!.Certificate;
            misskeyClient = MisskeyService.Create(misskeyCertification.Host, misskeyCertification.AccessToken);
        }
        #endregion
        public async Task StartAsync()
        {
            // Botの起動
            await Task.WhenAll(discordClient.StartAsync(), twitterClient.StartAsync(), misskeyClient.StartAsync());
        }

        public async Task EndAsync()
        {
            // Botの停止
            await Task.WhenAll(discordClient.EndAsync(), twitterClient.EndAsync(), misskeyClient.EndAsync());
        }

        public void Dispose()
        {
            // 後始末
            discordClient.Dispose();
            twitterClient.Dispose();
            misskeyClient.Dispose();
        }
    }
}
