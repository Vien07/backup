using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using DTO;
using CMS.Repository;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Net;
using CMS.Services.CommonServices;
using CMS.Services.TranslateServices;

namespace CMS.Services
{
    #region Thanh Toán momo
    public class PaymentServices : IPaymentServices
    {

        private readonly IContact_Repository _rep;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private readonly DBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly string _lang = "";


        public string EmailAdmin = "";
        public string RootDomain = "";
        public string Logo = "";
        public string EmailSMTPServer = "";
        public string EmailPort = "";
        public string EmailSMTPUser = "";
        public string MomoPartnerCode = "";
        public string MomoSecretKey = "";
        public string MomoAccessKey = "";
        public string MomoEndPoint = "";
        public PaymentServices(IContact_Repository rep, ICommonServices common, ITranslateServices translate, IHttpContextAccessor httpContextAccessor, DBContext dbContext)
        {

            _dbContext = dbContext;
            _rep = rep;
            _common = common;
            _translate = translate;
            MomoPartnerCode = _common.GetConfigValue(ConstantStrings.KeyMomoPartnerCode);
            MomoSecretKey = _common.GetConfigValue(ConstantStrings.KeyMomoSecrectKey);
            MomoAccessKey = _common.GetConfigValue(ConstantStrings.KeyMomoAccessKey);
            RootDomain = _common.GetConfigValue(ConstantStrings.KeyRootDomain);
            MomoEndPoint = _common.GetConfigValue(ConstantStrings.KeyMomoApi);

        }
        public dynamic Payment(dynamic obj)
        {
            //PaymentType : Thanh toán momo
            if (obj.PaymentType == 2)
            {
                //request params need to reques t to MoMo system
                //string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string endpoint = MomoEndPoint;
                string partnerCode = MomoPartnerCode;// "MOMOH6GI20210503";// "MOMO5RGX20191128";
                string accessKey = MomoAccessKey;// "GSfLhZuvchMZCgJq" ;//"M8brj9K6E22vXoDB";
                string serectkey = MomoSecretKey;// "TZvNJy38baBdcIKHNuFzVn622MtqbqZB";// "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
                string orderInfo = obj.CustomerName + "- " + obj.CustomerPhone + "- " + obj.CustomerAddress;
                //string returnUrl = RootDomain+ "/Product/ReturnPayMent" /*RootDomain +__("url.product")*/;
                string returnUrl = RootDomain + "/Cart/CheckOutSuccess" /*RootDomain +__("url.product")*/;
                string notifyurl = RootDomain + _translate.GetUrl("url.product");

                string amount = obj.Total.ToString();
                string orderid = obj.Code;
                string requestId = obj.RequestPaymentId;
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                //log.Debug("rawHash = " + rawHash);

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);
                //log.Debug("Signature = " + signature);

                //build body json request
                JObject message = new JObject{
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
                //log.Debug("Json request to MoMo: " + message.ToString());

                try
                {
                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                    JObject jresponseFromMomo = JObject.Parse(responseFromMomo);
                    if (jresponseFromMomo.GetValue("errorCode").ToString() == "0")
                    {
                        JObject jmessage = JObject.Parse(responseFromMomo);
                        return jmessage.GetValue("payUrl").ToString();


                    }
                    else
                    {
                        return "";

                    }
                }
                catch (Exception ex)
                {
                    return "";

                }
            }
            else
            {
                return _translate.GetUrl("url.order-success");
            }


        }
    }
    public class MoMoSecurity
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MoMoSecurity()
        {
            //encrypt and decrypt password using secure
        }
        public string getHash(string partnerCode, string merchantRefId,
            string amount, string paymentCode, string storeId, string storeName, string publicKeyXML)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"amount\":" +
                amount + ",\"paymentCode\":\"" +
                paymentCode + "\",\"storeId\":\"" +
                storeId + "\",\"storeName\":\"" +
                storeName + "\"}";
            log.Debug("Raw hash: " + json);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(4096)) //KeySize
            {
                try
                {
                    // MoMo's public key has format PEM.
                    // You must convert it to XML format. Recommend tool: https://superdry.apphb.com/tools/online-rsa-key-converter
                    rsa.FromXmlString(publicKeyXML);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }
        public string buildQueryHash(string partnerCode, string merchantRefId,
            string requestid, string publicKey)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"requestId\":\"" +
                requestid + "\"}";
            log.Debug("Raw hash: " + json);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }

        public string buildRefundHash(string partnerCode, string merchantRefId,
            string momoTranId, long amount, string description, string publicKey)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"momoTransId\":\"" +
                momoTranId + "\",\"amount\":" +
                amount + ",\"description\":\"" +
                description + "\"}";
            log.Debug("Raw hash: " + json);
            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }
        public string signSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;

            }
        }
    }
    public class PaymentRequest
    {
        public PaymentRequest()
        {
        }
        public static string sendPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }


                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
    #endregion
    #region VN Pay

    #endregion
}
