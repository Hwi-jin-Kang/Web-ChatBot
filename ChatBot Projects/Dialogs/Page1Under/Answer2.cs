using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class
using System.Net.Http;                  //Add for internet
using GreatWall.Helpers;                //Add for CardHelper


namespace GreatWall
{
    [Serializable]
    public class Answer2 : IDialog<string>
    {
        string strMessage;
        string strSymX;
       

        public async Task StartAsync(IDialogContext context)   
        {
            strMessage = null;
            strSymX = "[그외 증상] \n";

            //Called MessageReceivedAsync() without user input message
            await this.MessageReceivedAsync(context, null);  
        }

        private async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            if (result != null)
            {
                Activity activity = await result as Activity;

                if (activity.Text.Trim() == "Exit")
                {
                    await context.PostAsync(strSymX);    //return our reply to the user
                    strSymX = null;
                    context.Done("이용해 주셔서 감사합니다.");
                }
                else
                {
                    strMessage = string.Format("You Answer {0}.", activity.Text);
                    strSymX += activity.Text + "\n";
                    await context.PostAsync(strMessage);    //return our reply to the user

                    context.Wait(this.MessageReceivedAsync);
                }
            }
            else
            {
                strMessage = "코로나 의심증상에 포함되어 있지는 않지만 정확한 진단을 위하여 위치 기능에서 가까운 선별 진료소를 확인 후 방문하거나 자가 격리 후 다시 진단을 받아주세요. ";
                await context.PostAsync(strMessage);    //return our reply to the user
            
                //Menu
                var message = context.MakeMessage();                 //Create message      
                var actions = new List<CardAction>();       //Create List

                actions.Add(new CardAction() { Title = "1. 지역별 코로나 정보", Value = "1", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "2. 위치 기능", Value = "2", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "3. 다른 증상 보기", Value = "3", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "4. 정보 기능", Value = "4", Type = ActionTypes.ImBack });


                message.Attachments.Add(                    //Create Hero Card & attachment
                new HeroCard { Title = "원하시는 탭을 선택하세요 ", Buttons = actions }.ToAttachment()
            );

                message.AttachmentLayout = "carousel";              //Setting Menu Layout Format
               
                await context.PostAsync(message);                   //Output message 

                context.Wait(this.SendWelcomeMessageAsync);
            }
        }
        public async Task SendWelcomeMessageAsync(IDialogContext context,
                                             IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "1")
            {
                context.Call(new Info(), DialogResumeAfter);
            }
            else if (strSelected == "2")
            {
                context.Call(new Page3(), DialogResumeAfter);
            }
            else if (strSelected == "3")
            {
                context.Call(new Page1(), DialogResumeAfter);
            }
            else if (strSelected == "4")
            {
                context.Call(new Page2(), DialogResumeAfter);
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
    }
}