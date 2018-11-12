using System.Collections.Generic;
using AdaptiveCards;
using BotBuilder.Samples.AdaptiveCards.Infra;
using Newtonsoft.Json;
using System.Threading;

namespace BotBuilder.Samples.AdaptiveCards
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System;
    using System.Threading.Tasks;

    [Serializable]
    public class TestDialog : IDialog<string>
    {
        private AppointmentDetails Testing { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context,
                SpecialtyChoiceReceivedAsync,
                (IEnumerable<AppointmentDetails>)Enum.GetValues(typeof(AppointmentDetails)),
                "Select Option",
                "Selected choice is not available . Please try again.",
                promptStyle: PromptStyle.Auto);
        }

        protected virtual async Task SpecialtyChoiceReceivedAsync(IDialogContext context, IAwaitable<AppointmentDetails> appointmentDetails)
        {
            Testing = await appointmentDetails;
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message != null && message.Value != null)
            {
                DateTimeInp input = JsonConvert.DeserializeObject<DateTimeInp>(message.Value.ToString());
                var toDateTime = input.ToDateTime();
                if (toDateTime != null)
                {
                    context.Done(toDateTime.ToString());
                }
            }
            else
            {
                var json = await Utility.GetCardText("DateTimePicker");
                var results = AdaptiveCard.FromJson(json);
                var card = results.Card;

                var attachment = new Attachment()
                {
                    Content = card,
                    ContentType = AdaptiveCard.ContentType,
                    Name = "Requested Date Adaptive Card"
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(attachment);

                await context.PostAsync(reply, CancellationToken.None);

                context.Wait(MessageReceivedAsync);
            }
        }
    }
}
