using System.Net.Mail;
using System.Net;
using System.Text;

namespace EBookStoreAPI.Models.Infra
{
    public class EmailHelper
    {
        private string senderEmail = "yoyoann2023@gmail.com"; // 寄件者

        public void SendForgetPasswordEmail(string url, string account, string email)
        {
            var subject = "[重設密碼通知]";
            var body = $@"Hi {account},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>我要重設密碼</a>], 以進行重設密碼, 如果您沒有提出申請, 請忽略本信, 謝謝";

            var from = senderEmail;
            var to = email;

            SendFromGmail(from, to, subject, body);
        }

        public void SendConfirmRegisterEmail(string url, string account, string email)
        {
            //var url2 = "https://127.0.0.1:5173/customerMail";
            var subject = "[布可網路書店新會員確認信]";
//            var body = $@"Hi {account},
//<br />
//請點擊此連結 [<a href='{url}' target='_blank'>的確是我申請會員</a>], 如果您沒有提出申請, 請忽略本信, 謝謝";

            var body = $@"<img src=""https://ci4.googleusercontent.com/proxy/Y6y46vWSqrWFvqI5i7bwBG_X-fbPXtOIrwi8QK8SEet5WW1jbQBm8sDz_ytVNMKmbjoHc4sOL8PtkMhmhpAGsxX2ByZuug5ofx0pBlw5tmfGBvKj7e9U5jaPRrgSp2YDQXvYu7lRR7vNkIBdmb9fGdHhUaElM7MZkes9pJMLQVwk=s0-d-e1-ft#https://upload.wikimedia.org/wikipedia/commons/thumb/9/92/Open_book_nae_02.svg/2560px-Open_book_nae_02.svg.png"" width=""75"" height=""41"" style=""margin-right: 0px;""><div><b><font face=""microsoft jhenghei, sans-serif"" size=""4"">立即啟用布可網路書店帳號!</font></b><div><br></div></div><div>請點擊[<a href='{url}' target='_blank'>此連結</a>]完成會員驗證，即可馬上啟用布可網路書店帳號!</div><div><br></div><div><br></div><div><font color=""#666666"">不是您註冊的帳號嗎?</font></div><div><font color=""#666666"">可以由此<a href=""https://127.0.0.1:5173/customerMail"">聯絡客服</a></font></div>";



            var from = senderEmail;
            var to = email;

            SendFromGmail(from, to, subject, body);
        }

        public virtual void SendFromGmail(string from, string to, string subject, string body)
        {
            //from = senderEmail;
            // todo 以下是開發時,測試之用, 只是建立text file, 不真的寄出信
            //var path = HttpContext.Current.Server.MapPath("~/files/");
            //CreateTextFile(path, from, to, subject, body);
            //return;

            // 以下是實作程式, 可以視需要真的寄出信, 或者只是單純建立text file,供開發時使用
            // ref https://dotblogs.com.tw/chichiblog/2018/04/20/122816

            //var smtpAccount = senderEmail;
            var smtpAccount = from;

            // TODO 請在這裡填入密碼,或從web.config裡讀取
            var smtpPassword = "jcstqetsjfmioegz";

            var smtpServer = "smtp.gmail.com";
            var SmtpPort = 587;

            var mms = new MailMessage();
            mms.From = new MailAddress(smtpAccount, "布可網路書店");
            mms.Subject = subject;
            mms.Body = body;
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            mms.To.Add(new MailAddress(to));

            using (var client = new SmtpClient(smtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpAccount, smtpPassword);//寄信帳密 
                client.Send(mms); //寄出信件
            }
            mms.Dispose();
        }

        private void CreateTextFile(string path, string from, string to, string subject, string body)
        {
            var fileName = $"{to.Replace("@", "_")} {DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
            var fullPath = Path.Combine(path, fileName);

            var contents = $@"from:{from}
to:{to}
subject:{subject}

{body}";
            File.WriteAllText(fullPath, contents, Encoding.UTF8);
        }
    }
}
