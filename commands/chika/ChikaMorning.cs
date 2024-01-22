using BotManager.Common.Messaging;
using BotManager.Common.Utility;
using BotManager.External.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_manager_script.commands.chika
{
    internal sealed class ChikaMorning : CommandBase<IReplyableMessage>
    {
        public ChikaMorning() : base(100, true, false)
        {

        }

        public override bool Conditions(IReplyableMessage value)
        {
            return value.ReceivedTime.Hour is (>= 5 and < 10)
                && value.Content.Like("おはよう|おはチカ|おはちーちゃん|おはチーちゃん");
        }

        public override async ValueTask OnNextAsync(IReplyableMessage value, CancellationToken cancellationToken)
        {
            await value.Reply($"{value.AuthorName}、おはようございます！\n" + Random.Shared.Choice(
                "今日もあなたのことを応援していますよ！"
            ));
        }
    }
}
