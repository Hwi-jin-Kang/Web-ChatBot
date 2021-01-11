using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class
using System.Net.Http;                  //Add for internet
using GreatWall.Helpers;                //Add for CardHelper
using System.Drawing;
using System.Web.UI.WebControls;

namespace GreatWall
{
    [Serializable]
    public class Info : IDialog<string>
    {
        string strMessage;
        string strServerUrl = "http://localhost:3984/Images/";

        public async Task StartAsync(IDialogContext context)   
        {
            strMessage = null;

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
                    context.Call(new PageHome(), MessageReceivedAsync);
                }
                else
                {
                    string strSelected = activity.Text.Trim();
                    
                    if(strSelected == "서울")
                    {
                        System.Diagnostics.Process.Start("https://www.seoul.go.kr/coronaV/coronaStatus.do");
                        
                    }
                    else if (strSelected == "인천")
                    {
                        System.Diagnostics.Process.Start("https://www.incheon.go.kr/health/HE020409");
                        
                    }
                    else if (strSelected == "경기도")
                    {
                        System.Diagnostics.Process.Start("https://www.gg.go.kr/contents/contents.do?ciIdx=1150&menuId=2909");
                        
                    }
                    else if (strSelected == "대전")
                    {
                        System.Diagnostics.Process.Start("https://www.daejeon.go.kr/corona19/index.do");
                        
                    }
                    else if (strSelected == "대구")
                    {
                        System.Diagnostics.Process.Start("http://www.daegu.go.kr/dgcontent/index.do");
                        
                    }
                    else if (strSelected == "울산")
                    {
                        System.Diagnostics.Process.Start("http://www.ulsan.go.kr/corona.jsp");
                        
                    }
                    else if (strSelected == "광주")
                    {
                        System.Diagnostics.Process.Start("https://www.gwangju.go.kr/c19/");
                        
                    }
                    else if (strSelected == "부산")
                    {
                        System.Diagnostics.Process.Start("https://www.busan.go.kr/health/corona3");
                        
                    }
                    else
                    {
                        System.Diagnostics.Process.Start("http://www.jejusi.go.kr/information/intro/coronaStatus.do");
                       
                    }
                    result = null;
                    context.Call(new Info(), MessageReceivedAsync);
                }
            }
            else
            {
                strMessage = "지역별 코로나 현황 및 확진자 동선에 대해 알려드립니다. 지역을 선택해주세요.";
                await context.PostAsync(strMessage);    //return our reply to the user
            
                //Menu
                var message = context.MakeMessage();                 //Create message      

                //Hero Card-01~04 attachment 

                message.Attachments.Add(CardHelper.GetHeroCard("서울", this.strServerUrl + "Seoul.png", "정보 보기", "서울"));
                message.Attachments.Add(CardHelper.GetHeroCard("인천", this.strServerUrl + "Incheon.png", "정보 보기", "인천"));
                message.Attachments.Add(CardHelper.GetHeroCard("경기도", this.strServerUrl + "GyeongGi-Do.png", "정보 보기", "경기도"));
                message.Attachments.Add(CardHelper.GetHeroCard("대전", this.strServerUrl + "Daejeon.png", "정보 보기", "대전"));
                message.Attachments.Add(CardHelper.GetHeroCard("대구", this.strServerUrl + "Daegu.png", "정보 보기", "대구"));
                message.Attachments.Add(CardHelper.GetHeroCard("울산", this.strServerUrl + "Ulsan.jpg", "정보 보기", "울산"));
                message.Attachments.Add(CardHelper.GetHeroCard("광주", this.strServerUrl + "Gwangju.jpg", "정보 보기", "광주"));
                message.Attachments.Add(CardHelper.GetHeroCard("부산", this.strServerUrl + "Busan.png", "정보 보기", "부산"));
                message.Attachments.Add(CardHelper.GetHeroCard("제주도", this.strServerUrl + "Jeju.jpg", "정보 보기", "제주도"));

                message.Attachments.Add(CardHelper.GetHeroCard(null, null, "이전 화면으로", "Exit"));

                message.AttachmentLayout = "carousel";              //Setting Menu Layout Format
               
                await context.PostAsync(message);                   //Output message 

                context.Wait(this.MessageReceivedAsync);
            }
        }
        
    }

}