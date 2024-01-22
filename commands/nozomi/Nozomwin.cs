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
    internal sealed class Nozomwin : CommandBase<IReplyableMessage>
    {
        public Nozomwin() : base(10, true, false)
        {

        }

        public override bool Conditions(IReplyableMessage value)
        {
            return value.Content.Like("(ん|ン)ヌ(ぉ|ォ)+ズォ(む|ム)ウィ(い|イ)+ん+！+");
        }

        public override async ValueTask OnNextAsync(IReplyableMessage value, CancellationToken cancellationToken)
        {
            await value.Reply("……？");
        }
    }
}
