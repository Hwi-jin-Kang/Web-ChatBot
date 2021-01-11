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
    public class QnA : IDialog<string>
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
                    context.Call(new Page2(), MessageReceivedAsync);
                }
                else
                {
                    strMessage = string.Format("{0}.", activity.Text);
                    if(strMessage == "답변1.")
                    {
                        strMessage = "본 지침 상의 사례정의에 따라 의사환자 및 조사대상 유증상자로 분류되는 경우에 검사를 받을 수 있습니다. 막연한 불안감으로 검사를 받으실 필요는 없으므로, 의사선생님의 전문적인 판단을 신뢰하여 주시기 바랍니다. \r\n [ 의사환자 ] \r\n 확진환자와 접촉한 후 14일 이내에 발열 또는 호흡기증상(기침, 호흡곤란 등)이 나타난 자 \r\n [ 조사대상 유증상자 ] \r\n ① 의사의 소견에 따라 원인미상폐렴 등 코로나19가 의심되는 자 \r\n ② 국외 방문력이 있으며 귀국 후 14일 이내에 발열(37.5℃ 이상) 또는 호흡기증상(기침, 호흡곤란 등)이 나타난 자 \r\n ③ 코로나바이러스감염증-19 국내 집단발생과 역학적 연관성이 있으며, 14일 이내 발열(37.5℃ 이상) 또는 호흡기증상(기침, 호흡곤란 등)이 나타난 자";

                    }
                    else if (strMessage == "답변2.")
                    {
                        strMessage = "[ 코로나19 예방을 위한 마스크 사용 권고사항 ] \r\n\r\n▶ 적용대상 \r\n지역사회 일상생활을 영위하는 개인(단, 의료기관 종사자 등 감염우려가 있는 업무 종사자, 감염자는 제외)\r\n\r\n▶ 마스크 착용이 필요한 경우와 사용법\r\n① 보건용 마스크(KF80 이상) 착용이 필요한 경우\r\n   • 기침, 재채기, 가래, 콧물, 목아픔 등 호흡기 증상이 있는 경우\r\n   •  건강한 사람이 신종코로나 바이러스 감염 의심자를 돌보는 경우\r\n   • 의료기관을 방문하는 경우\r\n   • 많은 사람을 접촉하여야 하는, 감염과 전파 위험이 높은 직업군에 종사하는 사람\r\n    예) 대중교통 운전기사, 판매원, 역무원, 우체국 집배원, 택배기사, 대형건물 관리원 및 고객을 직접 응대하여야 하는 직업종사자 등\r\n ② 마스크 착용이 필요하지 않은 경우\r\n   • 혼잡하지 않은 야외, 개별 공간\r\n ③ 마스크 사용법\r\n   • 마스크를 착용하기 전에 손을 비누와 물로 씻거나 알코올 손소독제로 닦을 것 \r\n   • 입과 코를 완전히 가리도록 마스크를 착용한 후, 얼굴과 마스크 사이에 틈이 없는 지 확인할 것\r\n   • 마스크를 사용하는 동안 마스크를 만지지 말 것. 마스크를 만졌다면 손을 비누와 물로 씻거나 알코올 손소독제로 닦을 것";

                    }
                    else if (strMessage == "답변3.")
                    {
                        strMessage = "코로나19의 주된 전파 방법은 기침을 하는 환자가 배출한 비말을 흡입하거나 접촉하는 통하는 것입니다. 무증상자에게서 코로나19가 감염될 위험은 매우 낮습니다.";

                    }
                    else if (strMessage == "답변4.")
                    {
                        strMessage = "확진환자와 최종으로 접촉한 날로 부터 14일 동안 격리(자가, 시설, 병원)를 실시합니다. \r\n\r\n시·도지사 또는 시장·군수·구청장은 접촉자에게 격리통지서를 발부하고, 생활수칙을 안내하며, 1:1로 담당자를 지정하여 격리 해제 시까지 매일 2회 유선 연락하여 발열 또는 호흡기 증상여부를 확인합니다.";

                    }
                    else
                    {
                        strMessage = "코로나바이러스감염증-19의 잠복기는 확실하지 않습니다. 일반적으로 코로나바이러스에 준해 2~14일 추정, 최대 14일로 보고 있습니다.";

                    }
                    await context.PostAsync(strMessage);    //return our reply to the user
                    result = null;
                    context.Call(new QnA(), MessageReceivedAsync);
                }
            }
            else
            {
                strMessage = "";
                await context.PostAsync(strMessage);    //return our reply to the user
            
                //Menu
                var message = context.MakeMessage();                 //Create message      

                //Hero Card-01~04 attachment 

                message.Attachments.Add(CardHelper.GetHeroCard("Q. 누가 코로나바이러스감염증-19 검사를  받을 수 있나요? ",
                                        this.strServerUrl + "Q1.PNG", "답변보기", "답변1"));

                message.Attachments.Add(CardHelper.GetHeroCard("Q. 감염 예방을 위해 마스크는 어떤 것을  써야 하나요? ", 
                                        this.strServerUrl + "Q2.PNG", "답변보기", "답변2"));

                message.Attachments.Add(CardHelper.GetHeroCard("Q. 코로나바이러스감염증-19는 무증상에도  전파되나요? ",
                                        this.strServerUrl + "Q3.PNG", "답변보기", "답변3"));
                message.Attachments.Add(CardHelper.GetHeroCard("Q. 확진환자의 접촉자가 되면 어떤 조치가  이루어지나요? ",
                                        this.strServerUrl + "Q4.PNG", "답변보기", "답변4"));
                message.Attachments.Add(CardHelper.GetHeroCard("Q. 코로나19의 잠복기는 얼마나 되나요? ",
                                        this.strServerUrl + "Q5.PNG", "답변보기", "답변5"));

                message.Attachments.Add(CardHelper.GetHeroCard(null, null, "이전 화면으로", "Exit"));

                message.AttachmentLayout = "carousel";              //Setting Menu Layout Format
               
                await context.PostAsync(message);                   //Output message 

                context.Wait(this.MessageReceivedAsync);
            }
        }
        
    }

}