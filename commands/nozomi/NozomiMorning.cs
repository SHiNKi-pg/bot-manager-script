using BotManager.Common.Messaging;
using BotManager.Common.Utility;
using BotManager.External.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_manager_script.commands.nozomi
{
    internal sealed class NozomiMorning : CommandBase<IReplyableMessage>
    {
        public NozomiMorning() : base(100, true, false)
        {

        }

        public override bool Conditions(IReplyableMessage value)
        {
            return value.ReceivedTime.Hour is (>= 5 and < 10)
                && value.Content.Like("おはよう|おはのぞみん|おはノゾミ");
        }

        public override async ValueTask OnNextAsync(IReplyableMessage value, CancellationToken cancellationToken)
        {
            await value.Reply($"{value.AuthorName}くん、おはよう♪\n" + Random.Shared.Choice(
                "今日も一日、一緒に頑張ろうね♪",
                "キミと一緒なら、今日も頑張れるよ！",
                "今日ね、私のライブがあるんだ♪絶対来てね！"
            ));
        }
    }
}
