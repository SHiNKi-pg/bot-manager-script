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

namespace bot_manager_script.subscriptions.nozomi
{
    [Action]
    public sealed class MessageReceivedSubscription : SubscriptionScriptBase
    {
        public MessageReceivedSubscription() : base("nozomi_timeline", "ノゾミタイムライン")
        {

        }

        protected override void RegistSubscription(SubscriptionArguments args, CompositeDisposable subscriptions)
        {
            if(args.BotManager.TryGetBot<IReceivableMessageBot<IReplyableMessage>>("nozomi", out var nozomi, _ => true))
            {
                var distributor = nozomi.CreateMessageNotifier().ToAsyncDistributor();

                distributor.Add(new commands.nozomi.NozomiMorning());
                distributor.Add(new commands.common.Dice());

                subscriptions.Add(distributor);
            }
        }
    }
}
