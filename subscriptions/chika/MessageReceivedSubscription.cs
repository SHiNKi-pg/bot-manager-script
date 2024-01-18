using BotManager.Common;
using BotManager.Common.Extensions;
using BotManager.Common.Messaging;
using BotManager.Common.Scripting;
using BotManager.Common.Scripting.Attributes;
using BotManager.External;
using BotManager.External.ScriptSupport;
using BotManager.Reactive.Distribution;
using BotManager.Service.Discord;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace bot_manager_script.subscriptions.chika
{
    [Action]
    public sealed class MessageReceivedSubscription : SubscriptionScriptBase
    {
        public MessageReceivedSubscription() : base("chika_timeline", "チカタイムライン")
        {

        }

        protected override void RegistSubscription(SubscriptionArguments args, CompositeDisposable subscriptions)
        {
            if(args.BotManager.TryGetBot<IReceivableMessageBot<IReplyableMessage>>("chika", out var chika, _ => true))
            {
                var distributor = chika.CreateMessageNotifier().ToAsyncDistributor();

                subscriptions.Add(distributor);
            }
        }
    }
}
