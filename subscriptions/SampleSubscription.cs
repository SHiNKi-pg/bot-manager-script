using BotManager.Common.Scripting;
using BotManager.Common.Scripting.Attributes;
using BotManager.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace bot_manager_script.subscriptions
{
    [Action]
    [Ignore]
    public class SampleSubscription : ISubscription<SubscriptionArguments>
    {
        public string Id => "sample";

        public string Name => "サンプルサブスクリプション";

        public IDisposable SubscribeFrom(SubscriptionArguments args)
        {
            CompositeDisposable disposables = new();

            // TODO: イベントの購読

            return disposables;
        }
    }
}
