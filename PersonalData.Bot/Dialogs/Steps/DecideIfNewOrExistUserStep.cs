using HW.Bot.Interfaces;
using HW.Bot.Resources;
using Microsoft.Bot.Builder.Dialogs;
using Models;

namespace HW.Bot.Dialogs.Steps
{
    public class DecideIfNewOrExistUser
    {
        public static WaterfallStep Step(IDbContext dbContext, string newUserDialogId, string existUserMenuId = null)
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

                    return await stepContext.BeginDialogAsync(newUserDialogId, new Person(),
                        cancellationToken: cancellationToken);
                }

                if (existUserMenuId != null)
                {
                    return await stepContext.BeginDialogAsync(existUserMenuId, personalInfo,
                        cancellationToken: cancellationToken);
                }

                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            };
        }
    }
}
