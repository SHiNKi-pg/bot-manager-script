using BotManager.Common.Messaging;
using BotManager.Common.Utility;
using BotManager.External.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_manager_script.commands.tsumugi
{
    internal sealed class TsumugiMorning : CommandBase<IReplyableMessage>
    {
        public TsumugiMorning() : base(100, true, false)
        {

        }

        public override bool Conditions(IReplyableMessage value)
        {
            return value.ReceivedTime.Hour is (>= 5 and < 10)
                && value.Content.Like("おはよう|おはツム|おはつむ");
        }

        public override async ValueTask OnNextAsync(IReplyableMessage value, CancellationToken cancellationToken)
        {
            await value.Reply($"騎士さん、おはようございます！\n" + Random.Shared.Choice(
                "ちゃんと起きてるか見張ってますからねっ！",
                "ほら、今日も1日頑張ってください！",
                "ほら、早く出発しますよ！準備してください！"
            ));
        }
    }
}
