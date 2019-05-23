using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HW.Bot.Interfaces;
using HW.Bot.Model;
using HW.Bot.Resources;
using Microsoft.Bot.Builder.Dialogs;

namespace HW.Bot.Dialogs.Steps
{
    public class DecideIfNewOrExistUser
    {
        public static WaterfallStep Step(IDbContext dbContext, string NEW_USER_DIALOG, string EXIST_USER_MENU)
        {
            return async (stepContext, cancellationToken) =>
            {
                var channelId = stepContext.Context.Activity.ChannelId;
                var userId = stepContext.Context.Activity.From.Id;

                var personalInfo = dbContext.GetPersonalDetails(channelId, userId);

                if (personalInfo == null)
                {
                    await stepContext.Context.SendActivityAsync(
                        string.Format(BotStrings.There_is_No_info_about_user, userId, channelId) +
                        BotStrings.We_need_some_information,
                        cancellationToken: cancellationToken);

                    return await stepContext.BeginDialogAsync(NEW_USER_DIALOG, new PersonalData(),
                        cancellationToken: cancellationToken);
                }

                return await stepContext.BeginDialogAsync(EXIST_USER_MENU, personalInfo, cancellationToken: cancellationToken);
            };
        }
    }
}
