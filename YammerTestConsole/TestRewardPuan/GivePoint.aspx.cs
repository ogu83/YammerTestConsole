using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Configuration;
using System.Web.UI;

namespace TestRewardPuan
{
    public partial class GivePoint : Page
    {
        private string _svrAddress;
        private string _dbName;
        private int _port;
        private string _user;
        private string _password;

        private static MongoServer __mongoServer;
        protected MongoServer _mongoServer
        {
            get
            {
                if (__mongoServer == null)
                {
                    var crediantial = MongoCredential.CreateMongoCRCredential("admin", _user, _password);
                    __mongoServer = new MongoServer(new MongoServerSettings
                    {
                        Server = new MongoServerAddress(_svrAddress, _port),
                        Credentials = new MongoCredential[] { crediantial }
                    });
                }
                return __mongoServer;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var userName = Request.QueryString["userName"];
            var userId = int.Parse(Request.QueryString["userId"]);
            var point = double.Parse(Request.QueryString["point"]);
            var desc = Request.QueryString["desc"];

            var newUserPoint = new UserPointModel
            {
                desc = desc,
                point = point,
                userId = userId,
                userName = userName
            };

            _svrAddress = ConfigurationManager.AppSettings.Get("MongoServer");
            _dbName = ConfigurationManager.AppSettings.Get("MongoDatabase");
            _port = int.Parse(ConfigurationManager.AppSettings.Get("MongoPort"));
            _user = ConfigurationManager.AppSettings.Get("MongoUser");
            _password = ConfigurationManager.AppSettings.Get("MongoPassword");

            var db = _mongoServer.GetDatabase(_dbName);
            var coll = db.GetCollection("UserPoints");
            var existingUserPoint = coll.FindOneAs<UserPointModel>(Query.EQ("userId", newUserPoint.userId));
            if (existingUserPoint != null)
            {
                existingUserPoint.point += newUserPoint.point;
                coll.Save<UserPointModel>(existingUserPoint);
            }
            else
            {
                coll.Insert<UserPointModel>(newUserPoint);
            }

        }
    }
}