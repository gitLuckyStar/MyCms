using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;


namespace ServiceLayer
{
    public class reCaptchaValidation
    {

        public static bool GreCaptcha(IFormCollection form)
        {            
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "xxxxxx"; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }
            // receive the response now
            string gresult = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    gresult = reader.ReadToEnd();
                }
            }
            // validate the response from Google reCaptcha

            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(gresult);

            if (!captChaesponse.Success)
            {
                return false;
            }
            return true;
        }
    }
}
