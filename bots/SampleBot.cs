using BotManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotManager.Common.Scripting.Attributes;
using BotManager.Service.Discord;
using BotManager.Service.Twitter;
using BotManager.Service.Misskey;

namespace bot_manager_script.bots
{
    [Bot]
    [Ignore]
    public class SampleBot : IBot, IServiceBot<IDiscordServiceClient>, IServiceBot<ITwitterServiceClient>, IServiceBot<IMisskeyServiceClient>
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
        public SampleBot()
        {
            // プロフィール設定
            // TODO: AppSettingsから設定する
            Id = "sample";
            Name = "サンプル";
            NamePattern = "sample";

            // サービスのセットアップ
            discordClient = DiscordService.Create("discord-access-token");
            twitterClient = TwitterService.Create("consumer-key", "consumer-secret", "access-token", "access-token-secret");
            misskeyClient = MisskeyService.Create("misskey-host-name", "misskey-access-token");
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
