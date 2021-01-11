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
    public class Prevent : IDialog<string>
    {
        string strMessage;
        string strPre;
        string strServerUrl = "http://localhost:3984/Images/";

        public async Task StartAsync(IDialogContext context)   
        {
            strMessage = null;
            strPre = "";

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
                    await context.PostAsync(strPre);    //return our reply to the user
                    strPre = null;
                    context.Done("답을 찾았습니다.");
                }
                else
                {
                    strMessage = string.Format("자주 묻는 질문 {0}.", activity.Text);
                    strPre += activity.Text + "\n";
                    await context.PostAsync(strMessage);    //return our reply to the user

                    context.Wait(this.MessageReceivedAsync);
                }
            }
            else
            {
                strMessage = "";
                await context.PostAsync(strMessage);    //return our reply to the user
            
                //Menu
                var message = context.MakeMessage();                 //Create message      

                //Hero Card-01~04 attachment 

                message.Attachments.Add(CardHelper.Imagecard(this.strServerUrl + "P1.png"));
                message.Attachments.Add(CardHelper.Imagecard(this.strServerUrl + "P2.png"));
                message.Attachments.Add(CardHelper.Imagecard(this.strServerUrl + "P3_1.png"));
                message.Attachments.Add(CardHelper.Imagecard(this.strServerUrl + "P4.png"));
                message.Attachments.Add(CardHelper.Imagecard(this.strServerUrl + "P5.png"));
                message.Attachments.Add(CardHelper.Imagecard(this.strServerUrl + "P6.png"));
                message.Attachments.Add(CardHelper.GetHeroCard(null, null, "이전 화면으로", "Exit"));

                message.AttachmentLayout = "carousel";              //Setting Menu Layout Format
               
                await context.PostAsync(message);                   //Output message 

                context.Wait(this.MessageReceivedAsync);
            }
        }
         
    }
}