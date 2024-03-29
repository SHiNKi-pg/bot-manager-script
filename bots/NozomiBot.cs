﻿using BotManager.Common.Scripting.Attributes;
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
using BotManager.Common.Messaging;
using System.Reactive.Linq;
using BotManager.Database;

namespace bot_manager_script.bots
{
    [Bot]
    public class NozomiBot : IBot, IServiceBot<IDiscordServiceClient>, IServiceBot<ITwitterServiceClient>, IServiceBot<IMisskeyServiceClient>,
        ISendableBot, IReceivableMessageBot<IReplyableMessage>
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
        public NozomiBot()
        {
            // プロフィール設定
            var botSetting = AppSettings.GetBotDictionary()["nozomi"];
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
            using (var db = BotDatabase.Connect())
            {
                // Botがデータベースに登録されていなければ登録する
                if (!db.Bots.Any(b => b.Id == Id))
                {
                    await db.Bots.AddAsync(new()
                    {
                        Id = Id,
                        Name = Name,
                    });
                    await db.SaveChangesAsync();
                }
            }
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

        public async Task Send(string content)
        {
            List<Task> taskList = new();
            await Task.WhenAll(misskeyClient.Send(content), twitterClient.Send(content));
        }

        public IObservable<IReplyableMessage> CreateMessageNotifier()
        {
            IObservable<IReplyableMessage> discord = discordClient.MessagingReceived;
            return discord.Merge(misskeyClient.MessagingReceived);
        }
    }
}
