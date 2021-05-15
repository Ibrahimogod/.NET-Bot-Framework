using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class MyBotDialog : ComponentDialog
    {
        string user;

        public MyBotDialog(string id = null)
            : base(nameof(MyBotDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ConfirmNameAsync,
                RequestNameAsync,
                WannaSaySomthingAsync,
                FuckNameAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> WannaSaySomthingAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            user = stepContext.Context.Activity.Text;

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Say Something about Me"),
                    RetryPrompt = MessageFactory.Text("Say Something about Me")
                });
        }

        private async Task<DialogTurnResult> FuckNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string reply = "";


            await stepContext.Context.SendActivitiesAsync(
                 new Activity[] {
                new Activity { Type = ActivityTypes.Typing },
                new Activity { Type = "delay", Value= 6000 },
                MessageFactory.Text(reply,reply),
             },
             cancellationToken);

            return await stepContext.EndDialogAsync(stepContext.Result, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            user = string.Empty;
            var reply = MessageFactory.Text("Can You Enter Your Name in Arabic? \n هل يمكنك كتابة اسمك بالعربية؟");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "Yes/نعم", Type = ActionTypes.ImBack, Value = "Yes/نعم", Image = "https://via.placeholder.com/50/00FF00?text=Yes", ImageAltText = "Y" },
                    new CardAction() { Title = "No/لا", Type = ActionTypes.ImBack, Value = "No/لا", Image = "https://via.placeholder.com/50/FF0000?text=No", ImageAltText = "N" },
                },
            };
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = reply
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> RequestNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivitiesAsync(
                new Activity[]
                {
                    new Activity { Type = ActivityTypes.Typing },
                    new Activity { Type = "delay", Value= 4000 }
                }, cancellationToken);

            var promptMessage = MessageFactory.Text("برجاء ادخال اسمك بالعربية", "برجاء ادخال اسمك بالعربية", InputHints.ExpectingInput);
            var retryPrmot = MessageFactory.Text("Sorry Wrong Value", "عفوا برجاء ادخال اسمك بصورة صحيحة يا عرص", InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = promptMessage,
                    RetryPrompt = retryPrmot
                }, cancellationToken);
        }
    }
}
