<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestRewardPuan.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Yammer Test Reward Points</title>
    <script type="text/javascript" src="Scripts/jquery-2.1.4.js"></script>
    <script type="text/javascript" data-app-id="1q4fJuqSy6XYdiQfiGsQ"
        src="https://c64.assets-yammer.com/assets/platform_js_sdk.js">
    </script>
    <script type="text/javascript">
        var user;
        var accessToken;
        var authOK = false;

        yam.getLoginStatus(
          function (response) {
              if (response.authResponse) {
                  //alert("logged in");
                  //console.dir(response); //print user information to the console

                  authOK = true;

                  user = response.user;
                  accessToken = response.access_token;

                  document.getElementById('yammer-login').innerHTML = 'Welcome to Yammer Test Reward Points!';
                  document.getElementById('yammer-logout').style.visibility = 'visible';

                  document.getElementById('yammer-full_name').innerHTML = 'Full Name : <b>' + user.full_name + '</b>';
                  document.getElementById('yammer-id').innerHTML = 'User Id : <b>' + user.id + '</b>';
                  document.getElementById('yammer-email').innerHTML = 'E-mail : <b>' + user.email + '</b>';

                  document.getElementById('divGivePoints').style.visibility = 'visible';
                  document.getElementById('yammer-points').style.visibility = 'visible';
                  getUsers();
              }
              else {
                  //authResponse = false if the user is not logged in, or is logged in but hasn't authorized your app yet
                  //alert("logged out");
              }
          }
        );

        function logout() {
            yam.getLoginStatus(
               function (response) {
                   if (response.authResponse) {
                       yam.platform.logout(function (response) {
                           document.getElementById('yammer-login').innerHTML = '';
                           document.getElementById('yammer-logout').style.visibility = 'collapse';
                           document.getElementById('divGivePoints').style.visibility = 'collapse';
                           document.getElementById('yammer-points').style.visibility = 'collapse';
                           
                           location.reload();
                           //alert("user was logged out");
                       })
                   }
               }
             );
        }

        function getUsers() {
            yam.platform.request({
                url: "users.json",     //this is one of many REST endpoints that are available
                method: "GET",
                data: {    //use the data object literal to specify parameters, as documented in the REST API section of this developer site
                    //"letter": "a",
                    //"page": "2",
                },
                success: function (data) { //print message response information to the console
                    var markup = "<option value='-1'>Select User</option>";
                    for (var x = 0; x < data.length; x++) {
                        markup += "<option value=" + data[x].id + ">" + data[x].full_name + "</option>";
                    }
                    $("#selectUser").html(markup).show();
                },
                error: function (user) {
                    alert("There was an error with the request.");
                }
            });
        }

        function givePoints() {
            var point = document.getElementById('txtPoints').value;
            var desc = document.getElementById('txtDesc').value
            var userId = document.getElementById('selectUser').value;

            var msg = { "body": desc + ". Points: " + point };

            if (userId > 0) {
                yam.platform.request({
                    url: "messages.json",
                    method: "POST",
                    data: msg,
                    success: function (data) {
                        alert("Point Assigment Success");
                        var url = "http://" + document.location.host + "/TestRewardPuan/" + "GivePoint.aspx?userName=" + user.full_name + "&userId=" + userId + "&point=" + point + "&desc=" + desc;
                        $.get(url)
                    },
                    error: function (user) {
                        alert("There was an error with the request.");
                    }
                });
            }
        }
    </script>
</head>

<body style="font-family: 'Segoe UI'">
    <div>
        <h1>Yammer Test Reward Points</h1>
        <hr />
        <h2>User Details</h2>
        <div id="divUserDetails">
            <div id="yammer-login"></div>
            <div id="yammer-full_name"></div>
            <div id="yammer-id"></div>
            <div id="yammer-email"></div>
        </div>
        <div>
            <a id="yammer-points" href="PointsTable.aspx">Points Table</a>
            <input type="button" id="yammer-logout" style="visibility: collapse" onclick="logout();" value="Logout" />
        </div>
        <div id="divGivePoints" style="visibility: collapse">
            <hr />
            <h2>Give Points To Employees</h2>
            <h3>User</h3>
            <select id="selectUser">
                <option value="-1">Select User</option>
            </select>
            <h3>Points (Miles, BiggPionts, etc...)</h3>
            <input id="txtPoints" type="text" value="10" />
            <h3>Description</h3>
            <textarea id="txtDesc">Congrats you have gained 10 Points</textarea>
            <br />
            <input type="button" id="btnGivePoints" onclick="givePoints();" value="Submit" />
            <hr />
        </div>
        <script type="text/javascript">
            yam.connect.loginButton('#yammer-login', function (resp) {
                if (resp.authResponse) {
                    if (!authOK)
                        location.reload();
                }
            });
        </script>        
    </div>
</body>
</html>
