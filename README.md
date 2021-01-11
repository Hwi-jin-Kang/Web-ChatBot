# Web-ChatBot 2020 project

## 코로나 알리미

### 한눈에 살펴보기
![이미지](https://user-images.githubusercontent.com/71058308/104145324-78018c80-5401-11eb-93ca-a59b9822ee2f.jpg)

 * 지역별 코로나 정보
 
   확진자 동선api를 얻을 수 없어 각 대표시의 홈페이지에서 제공하는 코로나 현황으로 대체
   
 * 위치 기능
 
   근처 보건소, 약국에 대한 정보를 네이버 지도에 띄워줌
 
 * 진단 기능
 
   코로나의 초기 증상에 대해 보여주며 제시된 선택지를 고를 경우 진료를 받으라는 문구를 전송 후 직전의 화면으로 넘어감
   
 * 정보 기능

   자주 묻는 질문, 예방 수칙에 대해 이미지 카드로 정보를 제공함
   
#### 주요 코드 위치
    ./ChatBot Projects/Helpers/CardHelper.cs -> 이미지 카드를 입력하게끔 해주는 코드 
    ./ChatBot Projects/Dialogs/*             -> 한마디로 쉽게 말하자면 UI 구현하는 코드
    ./ChatBot Projects/Controllers/MessagesController.cs -> 메세지를 보내는 형식을 지정해주는 코드

#### 유튜브 영상
  
 [동작 영상](https://www.youtube.com/watch?v=hI5tiQf7YHE)
