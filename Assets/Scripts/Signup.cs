using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System.Security.Cryptography;
using System.Text;

public class Signup : MonoBehaviour
{
    public GameObject PrefError;
    public GameObject PreferencesPage, SignUpPage, LoginPage;
    public List<GameObject> PreferencesToggles = new List<GameObject>();
    string username;
    string password;
    string passwordconfirmed;
    string email;
    string firstname;
    string lastname;
    string birthdate;
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField passwordconfirmedInput;
    public InputField emailInput;
    public InputField firstnameInput;
    public InputField lastnameInput;
    public TMP_InputField birthdateInput;
    public GameObject userDot, passwordDot, passwordconfirmedDot, emailDot, firstnameDot, lastnameDot, birthdateDot;
    public GameObject emptyfieldsError, passwordmatchError, passwordformatError, emailformatError, usernameformatError, usernameexistsError;
    bool emptyfieldserror, passwordmatcherror, passwordformaterror, emailformaterror, usernameformaterror, usernameexistserror;
    public Queries queries;
    public List<List<string>> usernameList = new List<List<string>>();
    public List<string> usernamelist = new List<string>();
    public List<string> useridlist = new List<string>();
    string GeneratedID;
    string newUserID;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (usernameInput.text == "")
        {
            userDot.SetActive(true);
        }
        if (usernameInput.text != "")
        {
            userDot.SetActive(false);
        }

        if (passwordInput.text == "")
        {
            passwordDot.SetActive(true);
        }
        if (passwordInput.text != "")
        {
            passwordDot.SetActive(false);
        }

        if (passwordconfirmedInput.text == "")
        {
            passwordconfirmedDot.SetActive(true);
        }
        if (passwordconfirmedInput.text != "")
        {
            passwordconfirmedDot.SetActive(false);
        }


        if (firstnameInput.text == "")
        {
            firstnameDot.SetActive(true);
        }
        if (firstnameInput.text != "")
        {
            firstnameDot.SetActive(false);
        }

        if (lastnameInput.text == "")
        {
            lastnameDot.SetActive(true);
        }
        if (lastnameInput.text != "")
        {
            lastnameDot.SetActive(false);
        }

        if (birthdateInput.text == "")
        {
            birthdateDot.SetActive(true);
        }
        if (birthdateInput.text != "")
        {
            birthdateDot.SetActive(false);
        }

        if (emailInput.text == "")
        {
            emailDot.SetActive(true);
        }
        if (emailInput.text != "")
        {
            emailDot.SetActive(false);
        }

        if (usernameInput.text == "" || passwordInput.text == "" || passwordconfirmedInput.text == "" || firstnameInput.text == "" || lastnameInput.text == "" || emailInput.text == "" || birthdateInput.text == "")
        {
            emptyfieldserror = true;
        }
        else
        {
            emptyfieldserror = false;
        }

    }

    public void CloseSignUp()
    {
        this.gameObject.SetActive(false);
    }
    public void CreateAccount()
    {
        ResetAll();
        username = usernameInput.text;
        password = passwordInput.text;
        passwordconfirmed = passwordconfirmedInput.text;
        email = emailInput.text;
        firstname = firstnameInput.text;
        lastname = lastnameInput.text;
        birthdate = birthdateInput.text;
        VerifyInput();
    }
    void ResetAll()
    {
        emptyfieldsError.SetActive(false);
        passwordmatchError.SetActive(false);
        passwordformatError.SetActive(false);
        emailformatError.SetActive(false);
        usernameformatError.SetActive(false);
        usernameexistsError.SetActive(false);
        emptyfieldserror = false;
        passwordmatcherror = false;
        passwordformaterror = false;
        emailformaterror = false;
        usernameformaterror = false;
        usernameexistserror = false;
        usernamelist.Clear();
        useridlist.Clear();
    }
    void VerifyInput()
    {

        if (password != passwordconfirmed)
        {
            passwordmatcherror = true;
        }
        if (IsValidEmail(email) == false)
        {
            emailformaterror = true;
        }
        if (IsValidPassword(password) == false)
        {
            passwordformaterror = true;
        }
        if (IsValidUsername(username) == false)
        {
            usernameformaterror = true;
        }
        CallGetQueryResult(queries.WriteQuery_GETALLUSERNAMES());

    }

    void SignUpResult()
    {
        if (passwordmatcherror)
        {
            passwordmatchError.SetActive(true);
        }
        if (emailformaterror)
        {
            emailformatError.SetActive(true);
        }
        if (passwordformaterror)
        {
            passwordformatError.SetActive(true);
        }
        if (usernameformaterror)
        {
            usernameformatError.SetActive(true);
        }
        if (usernameexistserror)
        {
            usernameexistsError.SetActive(true);
        }

        if (emptyfieldserror)
        {
            emptyfieldsError.SetActive(true);
        }
        if (emptyfieldserror == false && passwordmatcherror == false && emailformaterror == false && passwordformaterror == false && usernameformaterror == false && usernameexistserror == false)
        {
            CreationSuccessful();
        }
    }

    void CreationSuccessful()
    {
        CreateAccountFinal();
        //GetUserIDs(queries.WriteQuery_GETALLUSERIDS());

    }


    void CreateAccountFinal()
    {
        CreateAccountQuery(queries.WriteQuery_ADDUSER(username, firstname, lastname, birthdate, email, HashPassword(password)));
    }

    public static bool IsValidEmail(string email)
    {


        // Regular expression pattern to validate email address format
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(email);
    }

    public static bool IsValidPassword(string password)
    {

        // Check if password is between 8 and 15 characters long and has no spaces
        if (password.Length < 8 || password.Length > 15 || password.Contains(" "))
            return false;

        // Check if password contains at least one capital letter, one normal letter, one number, and one special character
        string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(password);
    }

    public static bool IsValidUsername(string username)
    {

        // Check if username is between 8 and 15 characters long and has no spaces
        if (username.Length < 8 || username.Length > 15 || username.Contains(" "))
            return false;
        else
            return true;
    }

    public void CallGetQueryResult(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            CompareUsernames(queries.ExtractInfo(result));
            SignUpResult();
        }));
    }

    public void AddPref(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
        
        }));
    }

    public void CreateAccountQuery(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            GetUserIDs(queries.WriteQuery_GETUSERINFO(username));
            OpenPreferences();
        }));
    }

    public void GetUserIDs(string query)
    {
        Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
        {
            newUserID = queries.ExtractInfo(result)[0][0];
            
           

        }));
    }

    void OpenPreferences()
    {
        PreferencesPage.SetActive(true);
    }

    public void ConfirmPreferences()
    {
        //A check that 3 toglges in PreferencesToggle list are on. If yes proceed.
        int counter = 0;

        foreach (GameObject prefToggle in PreferencesToggles)
        {
            if (prefToggle.GetComponent<Toggle>().isOn)
            {
                counter++;
            }

        }
        if (counter >= 3)
        {
            foreach (GameObject prefToggle in PreferencesToggles)
            {
                if (prefToggle.GetComponent<Toggle>().isOn)
                {

                    AddPref(queries.WriteQuery_ADDPREFERENCE(newUserID, prefToggle.GetComponent<PreferenceToggleLabel>().PreferenceLabel, "0"));
                }
            }
            OpenLoginPage();
        }
        else
        {
            PrefError.SetActive(true);
        }

        
    }

    void OpenLoginPage()
    {
        PreferencesPage.SetActive(false);
        SignUpPage.SetActive(false);
        LoginPage.SetActive(true);
    }
    void GenerateUserIDs(List<List<string>> userIDList)
    {
        for (int i = 0; i < userIDList.Count; i++)
        {
            useridlist.Add(userIDList[i][0].Trim());
        }
        GeneratedID = GenerateUniqueID(useridlist);
        //CreateAccountFinal(GeneratedID);
    }

    void CompareUsernames(List<List<string>> usernameList)
    {
        
        for (int i = 0; i < usernameList.Count; i++)
        {
            usernamelist.Add(usernameList[i][0].Trim());
        }
        if (usernamelist.Contains(username))
        {
            usernameexistserror = true;
        }

    }

   
    public static string GenerateUniqueID(List<string> existingIDs)
    {
        int userID;

        do
        {
            userID = Mathf.RoundToInt(Random.Range(1, 99999999999));
        }
        while (existingIDs.Contains(userID.ToString()));

        return userID.ToString();
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
