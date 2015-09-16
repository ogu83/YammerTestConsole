using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestRewardPuan
{
    public partial class PointsTable : Page
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
            _svrAddress = ConfigurationManager.AppSettings.Get("MongoServer");
            _dbName = ConfigurationManager.AppSettings.Get("MongoDatabase");
            _port = int.Parse(ConfigurationManager.AppSettings.Get("MongoPort"));
            _user = ConfigurationManager.AppSettings.Get("MongoUser");
            _password = ConfigurationManager.AppSettings.Get("MongoPassword");

            var db = _mongoServer.GetDatabase(_dbName);
            var coll = db.GetCollection("UserPoints");

            var mytable = "<table>";
            mytable += "<tr>";
            mytable += "<th>UserId</th>";
            mytable += "<th>UserName</th>";
            mytable += "<th>Point</th>";
            mytable += "<th>Description</th>";
            mytable += "</tr>";
            var points = coll.FindAllAs<UserPointModel>();
            foreach (var p in points)
            {
                mytable += "<tr>";
                mytable += "<td>" + p.userId + "</td>";
                mytable += "<td>" + p.userName + "</td>";
                mytable += "<td>" + p.point + "</td>";
                mytable += "<td>" + p.desc + "</td>";
                mytable += "</tr>";
            }
            mytable += "</table>";
            div1.InnerHtml = mytable;
        }
    }
}