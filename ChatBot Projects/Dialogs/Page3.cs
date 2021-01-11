using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;       //Add for List<>

namespace GreatWall
{

    [Serializable]
    public class Page3 : IDialog<string>
    {
        protected int count = 1;
        string strMessage;
        private string strWelcomeMessage = "";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            await context.PostAsync(strWelcomeMessage);    //return our reply to the user

            var message = context.MakeMessage();        //Create message
            var actions = new List<CardAction>();       //Create List

            actions.Add(new CardAction() { Title = "1. 보건소 위치", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 약국 위치", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 이전 화면으로", Value = "3", Type = ActionTypes.ImBack });

            message.Attachments.Add(                    //Create Hero Card & attachment
                new HeroCard { Title = "정보 기능", Buttons = actions }.ToAttachment()
            );

            await context.PostAsync(message);           //return our reply to the user

            context.Wait(SendWelcomeMessageAsync);
        }

        public async Task SendWelcomeMessageAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "1")
            {
                System.Diagnostics.Process.Start("https://map.naver.com/v5/search/%EB%B3%B4%EA%B1%B4%EC%86%8C?c=14120487.5286779,4513793.6479995,13,0,0,0,dh");
                await context.PostAsync(strMessage);    //return our reply to the user
                result = null;
                context.Call(new Page3(), MessageReceivedAsync);
            }
            else if (strSelected == "2")
            {
                System.Diagnostics.Process.Start("https://map.naver.com/v5/search/%EC%95%BD%EA%B5%AD?c=14122224.6787559,4513654.8905466,16,0,0,0,dhh");
                await context.PostAsync(strMessage);    //return our reply to the user
                result = null;
                context.Call(new Page3(), MessageReceivedAsync);
            }
            else if (strSelected == "3")
            {
                context.Call(new PageHome(), DialogResumeAfter);
            }
            else
            {
                strMessage = "You have made a mistake. Please select again...";
                await context.PostAsync(strMessage);
                context.Wait(SendWelcomeMessageAsync);
            }

        }

        public async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = await result;

                //await context.PostAsync(WelcomeMessage); ;
                await this.MessageReceivedAsync(context, result);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred....");
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}

