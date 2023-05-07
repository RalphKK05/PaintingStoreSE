using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;

public class LoginPage : MonoBehaviour
{

    public ToggleManage toggleManager;
    public PageManager pageManager;
    public GameObject adminPage;
    public GameObject adminbar;
    public GameObject loginPage, feedPage;
    public InputField usernameInput;
    public InputField passwordInput;
    public GameObject usernameexistsError, passwordmatchError;
    string username;
    string password;
    public Queries queries;
    public List<List<string>> usernameList = new List<List<string>>();
    public List<string> usernamelist = new List<string>();
    public UserInfo userinfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (usernameInput.text == "")
        //{
        //    userDot.SetActive(true);
        //}
       // if (usernameInput.text != "")
       // {
       //     userDot.SetActive(false);
        //}

        //if (passwordInput.text == "")
       // {
       //     passwordDot.SetActive(true);
       // }
       // if (passwordInput.text != "")
       // {
       //    passwordDot.SetActive(false);
       // }
    }

    void ConfirmLogin()
    {
        print("LOGIN SUCCESSFUL");

        loginPage.SetActive(false);
        pageManager.OpenFEED_PAGE();
        toggleManager.Feed_Clicked();

      
    }
    void ConfirmLoginAdmin()
    {
        print("LOGIN SUCCESSFUL");

        loginPage.SetActive(false);
        adminPage.SetActive(true);
        adminbar.SetActive(true); 


    }

    public void VerifyInput()
    {
        ResetAll();
        username = usernameInput.text;
        password = passwordInput.text;
        CallGetQueryResult(queries.WriteQuery_GETALLUSERNAMES());
    }
    void ResetAll()
    {
        usernameexistsError.SetActive(false);
        passwordmatchError.SetActive(false);
        usernameList.Clear();
        usernamelist.Clear();
    }
    public void CallGetQueryResult(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            usernameList = queries.ExtractInfo(result);
            CheckUsername();
        }));
    }
    public void GetUserInfo(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            //Change so we get all userinfo we might need and put it in userinfo.
            userinfo.userID = queries.ExtractInfo(result)[0][0];
            userinfo.username = queries.ExtractInfo(result)[0][1];
            userinfo.fName = queries.ExtractInfo(result)[0][2];
            userinfo.lName = queries.ExtractInfo(result)[0][3];
            userinfo.birthDate = queries.ExtractInfo(result)[0][4];
            userinfo.email = queries.ExtractInfo(result)[0][5];
            userinfo.password = queries.ExtractInfo(result)[0][6];
            if (queries.ExtractInfo(result)[0][7] == "0")
            {
                ConfirmLogin();
            }
            if (queries.ExtractInfo(result)[0][7] == "1")
            {
                ConfirmLoginAdmin();
            }
            
        }));
    }
    public void CheckPassword(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
           
            if (queries.ExtractInfo(result)[0][0] == HashPassword(password))
            {
                
                GetUserInfo(queries.WriteQuery_GETUSERINFO(username));
            }
            else
            {
                WrongPassword();
            }
        }));
    }

    void CheckUsername()
    {
        for (int i = 0; i < usernameList.Count; i++)
        {
            usernamelist.Add(usernameList[i][0].Trim());
        }
        if (usernamelist.Contains(username))
        {
            CheckPassword(queries.WriteQuery_GETUSERPASSWORD(username));
        }
        else
        {
            WrongUsername();
        }
    }

    void WrongUsername()
    {
        usernameexistsError.SetActive(true);
    }
    void WrongPassword()
    {
        passwordmatchError.SetActive(true);
    }



    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
