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
    public class  Page1 : IDialog<string>
    {
        string strMessage;
        string strSym;


        public async Task StartAsync(IDialogContext context)   
        {
            //strMessage = null;
            //strSym = "[자주 묻는 질문] \n";
            //
            ////Called MessageReceivedAsync() without user input message
            //await this.MessageReceivedAsync(context, null); 
            context.Wait(MessageReceivedAsync);
        }
    
        private async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            if (result == null)
            {
                Activity activity = await result as Activity;

                if (activity.Text.Trim() == "Exit")
                {
                    await context.PostAsync(strSym);    //return our reply to the user
                    strSym = null;
                    context.Done("증상을 확인하였습니다.");
                }
                else
                {
                    strMessage = string.Format("현재 나의 증상은? {0}.", activity.Text);
                    strSym += activity.Text + "\n";
                    await context.PostAsync(strMessage);    //return our reply to the user

                    context.Wait(this.MessageReceivedAsync);
                  
                }
            }
            else
            {
                strMessage = "현재 사용자의 증상을 아래에서 선택하세요. ";
                await context.PostAsync(strMessage);    //return our reply to the user
            
                //Menu
                var message = context.MakeMessage();
                var actions = new List<CardAction>(); 
                actions.Add(new CardAction() { Title = "발열", Value = "1", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "호흡곤란", Value = "2", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "두통", Value = "3", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "권태감", Value = "4", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "가래생김", Value = "5", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "설사", Value = "6", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "기침", Value = "7", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "인후통", Value = "8", Type = ActionTypes.ImBack });
                actions.Add(new CardAction() { Title = "그외 증상이 나타남", Value = "9", Type = ActionTypes.ImBack });


                message.Attachments.Add(                    //Create Hero Card & attachment
                new HeroCard { Title = "코로나 의심증상 ", Buttons = actions }.ToAttachment()
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
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "2")
            {
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "3")
            {
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "4")
            {
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "5")
            {
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "6")
            {
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "7")
            {
                context.Call(new Answer1(), DialogResumeAfter); ;
            }
            else if (strSelected == "8")
            {
                context.Call(new Answer1(), DialogResumeAfter);
            }
            else if (strSelected == "9")
            {
                context.Call(new Answer2(), DialogResumeAfter);
            }
            else
            {
                strMessage = "You have made a mistake. Please select again...";
                await context.PostAsync(strMessage);
                context.Wait(SendWelcomeMessageAsync);
            }
        }
        public async Task DialogResumeAfter(IDialogContext context1, IAwaitable<string> result1)
        {
            try
            {
                strMessage = await result1;

                //await context.PostAsync(WelcomeMessage); ;
                await this.MessageReceivedAsync(context1, result1);
            }
            catch (TooManyAttemptsException)
            {
                await context1.PostAsync("Error occurred....");
            }
        }
    }
}