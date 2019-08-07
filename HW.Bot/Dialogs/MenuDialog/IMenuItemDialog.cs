using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HW.Bot.Dialogs.MenuDialog
{
    internal interface IMenuItemDialog
    {
        string Id { get; }
        string MenuItemOptionText { get; }
        Func<ITurnContext, object, CancellationToken, Task> HandleResult { get; }
        Dialog GetDialog();
    }
}
