using BotManager.Common.Messaging;
using BotManager.Common.Utility;
using BotManager.External.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace bot_manager_script.commands.common
{
    internal sealed class Dice : CommandBase<IReplyableMessage>
    {
        public Dice() :base(10, true, true)
        {

        }

        private const string DICE_COMMAND_PATTERN = @"^!dice (?<num>\d+)d(?<max>\d+)$";

        public override bool Conditions(IReplyableMessage value)
        {
            return value.Content.Like(DICE_COMMAND_PATTERN, RegexOptions.IgnoreCase);
        }

        public override async ValueTask OnNextAsync(IReplyableMessage value, CancellationToken cancellationToken)
        {
            if(value.Content.TryGetRegexGroup(DICE_COMMAND_PATTERN, RegexOptions.IgnoreCase, out var regexGroups))
            {
                try
                {
                    int diceNum = int.Parse(regexGroups["num"].Value);
                    int diceMax = int.Parse(regexGroups["max"].Value);

                    List<int> dices = new();
                    for(int i = 0; i < diceNum; i++)
                    {
                        dices.Add(Random.Shared.Next(1, diceMax + 1));
                    }
                    string message = string.Join(", ", dices);
                    await value.Reply(message);
                }
                catch (Exception)
                {
                    // TODO: 変換できなかった場合
                }
            }
        }
    }
}
