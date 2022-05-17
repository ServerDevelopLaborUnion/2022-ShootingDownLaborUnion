# 2022 서버 개발 노동 조합 산출물 프로젝트

**멀티플레이 가능한 Top-Down 슈팅 게임이다**



게임 조작법 등은 추후 추가 예정



이하는 개발에 사용된 기술들에 대한 설명입니다.

## Javascript Websocket Server

자바스크립트 `Websocket` 서버에 관한 설명

### 기술 스택

> [Google Protobuffer]([Protocol Buffers &nbsp;|&nbsp; Google Developers](https://developers.google.com/protocol-buffers)) - *npm install google-protobuf@3.20.1-rc.1*

기존의 `json` 기반의 통신은 **문자열** 기반이기 때문에 낭비되는 자원이 많다고 생각하여 그 대신 모든 데이터를 `byte array` 로 직렬화 하기 위해 사용하였다.

하나의 스키마를 이용하여 여러가지 언어에서 사용할 수 있는 코드를 자동생성하여 쓸 수 있기 때문에 `node` 기반 서버와 `C#` 기반 클라이언트 모두가 사용하기 쉬웠다.



> [Socket.IO](https://socket.io/) - *npm install socket.io@4.4.1*

`byte`로 되어있는 `Google Protobuffer` 의 데이터를 송수신하기 위하여 기존에 사용하던 `ws` 라이브러리 대신 고급 기능들을 사용할 수 있는 `socket.io` 라이브러리를 이용하여 제작하였다.

`http` 라이브러리를 이용하여 서버를 만들고 `listen(port)` 를 해준 후 `websocket` 으로 연결을 업그래이드하는 방식을 사용하는 라이브러리이다.



> [Json Web Token](https://jwt.io/) - *npm install jsonwebtoken@8.5.1*

줄여서 `jwt` 라고도 불리는 전자 서명이다. 서버에서 채용한 암호화 알고리즘은 `SHA-256` 이다.

사용자의 **로그인 정보를 저장**하고 다음 로그인을 시도할 때 **자동로그인**이 가능하도록 만들기 위해 **사용자 정보 인증 용도**로 사용하였다.



> MongoDB or MariaDB

사용자의 **데이터를 저장하는 용도**로 사용한 **데이터베이스**이다.

**로그인 시** 데이터를 메모리에 불러오고 **연결이 끊기거나 로그아웃 될 때** 데이터를 저장하게 하는 방식으로 데이터를 관리하였다.


