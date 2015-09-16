using System;
using System.Web.UI;

namespace TestRewardPuan
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            var url = string.Format("https://www.yammer.com/oauth2/authorize?client_id={0}&response_type=code&redirect_uri={1}",
                Constants.ClientID, Constants.RedirectUrl);
            Response.Redirect(url);
        }
    }
}