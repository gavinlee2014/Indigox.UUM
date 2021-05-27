//using DingTalk.Api;
//using DingTalk.Api.Request;
//using DingTalk.Api.Response;
//using Indigox.Common.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;

//namespace Indigox.UUM.Application.Web
//{
//    public class DingTalkCallbackHandler : IHttpHandler
//    {
//        private string appID = "dingoamzxikzrn35lruakt";
//        private string appSecret = "jeug7Fsdy58uEisgBCMyhznJE0HPZ9UUCLUAwingbjFMfAxt5mhjUCUEDs674Nju";
//        bool IHttpHandler.IsReusable => false;

//        void IHttpHandler.ProcessRequest(HttpContext context)
//        {
//            Log.Debug("ding callback " + context.Request.QueryString);
//            string code = context.Request.Params["code"];
//            Log.Debug("ding code " + code);

//            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/getuserinfo_bycode");
//            OapiSnsGetuserinfoBycodeRequest req = new OapiSnsGetuserinfoBycodeRequest();
//            //OapiUserGetuserinfoRequest req = new OapiUserGetuserinfoRequest();
//            req.SetHttpMethod("POST");
//            req.TmpAuthCode = code;
//            //OapiUserGetuserinfoResponse res = client.Execute(req, code);
//            OapiSnsGetuserinfoBycodeResponse res = client.Execute(req, appID, appSecret);
//            //res.UserInfo.Unionid
//            Log.Debug("ding getuserinfo_bycode response:" + res.Body);

//            string accessToken = GetAccessToken();

//            client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/user/getbyunionid");
//            OapiUserGetbyunionidRequest reqUnion = new OapiUserGetbyunionidRequest();
//            reqUnion.Unionid = res.UserInfo.Unionid;
//            reqUnion.SetHttpMethod("POST");
//            OapiUserGetbyunionidResponse resUnion = client.Execute(reqUnion, accessToken);
//            Log.Debug("ding getbyunionid response:" + resUnion.Body);


//            client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/user/get");
//            OapiV2UserGetRequest reqGetRequest = new OapiV2UserGetRequest();
//            reqGetRequest.Userid= resUnion.Result.Userid;
//            reqGetRequest.Language="zh_CN";
//            OapiV2UserGetResponse rspGetResponse = client.Execute(reqGetRequest, accessToken);
//            Log.Debug("ding topapi/v2/user/get response:" + rspGetResponse.Body);

//        }

//        private string GetAccessToken()
//        {
//            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
//            OapiGettokenRequest request = new OapiGettokenRequest();
//            request.Appkey = "dingmcvwnowg85fzhlap";
//            request.Appsecret = "tEDBmM1pGFJZFwyhiINJonauRuekae4UAE5xmusSYYxH4xGVZoRmUji3KMVFlBMV";
//            request.SetHttpMethod("GET");
//            OapiGettokenResponse response = client.Execute(request);
//            Log.Debug("ding gettoken response:" + response.Body);
//            return response.AccessToken;
//        }
//    }
//}
