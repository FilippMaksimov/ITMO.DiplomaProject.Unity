using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthReg : MonoBehaviour
{
    [SerializeField] 
    public InputField userName;
    [SerializeField] 
    private InputField userPass;
    [SerializeField]
    private InputField newUserName;
    [SerializeField]
    private InputField newUserPass;
    [SerializeField]
    private Text messageText;
    [SerializeField]
    private Text messageText2;
    [SerializeField]
    private string loginURL = "http://magicbattle.ru/login.php";
    [SerializeField]
    private string registerURL = "http://magicbattle.ru/register.php";
    [SerializeField]
    private GameObject loginForm;

    private void Awake()
    {
        Cursor.visible = true;
    }
    bool IsValid(string value, int min, int max, string field) // валидация имени и пароля
    {
        if (value.Length < min)
        {
            Message("В поле [" + field + "] недостаточно символов, нужно минимум [" + min + "]");
            return false;
        }
        else if (value.Length > max)
        {
            Message("В поле [" + field + "] допустимый максимум символов, не более [" + max + "]");
            return false;
        }
        else if (Regex.IsMatch(value, @"[^\w\.@-]"))
        {
            Message("В поле [" + field + "] содержаться недопустимые символы.");
            return false;
        }

        return true;
    }

    void Message(string text)
    {
        if (loginForm.gameObject.activeSelf == true)
        {
            messageText.text = text;
        }
        else
        {
            messageText2.text = text;
        }
        Debug.Log(this + " --> " + text);
    }

    [System.Obsolete]
    public void Login()
    {
        if (!IsValid(userName.text, 3, 25, "User Name") || !IsValid(userPass.text, 6, 20, "Пароль")) return;

        WWWForm form = new WWWForm();
        form.AddField("user_name", userName.text);
        form.AddField("password", userPass.text);
        WWW www = new WWW(loginURL, form);
        StartCoroutine(LoginFunc(www));
    }

    [System.Obsolete]
    public void Register()
    {

        if (!IsValid(newUserName.text, 3, 25, "User Name") || !IsValid(newUserPass.text, 6, 20, "Пароль")) return;

        WWWForm form = new WWWForm();
        form.AddField("name", newUserName.text);
        form.AddField("password", newUserPass.text);
        WWW www = new WWW(registerURL, form);
        StartCoroutine(RegisterFunc(www));
    }

    [System.Obsolete]
    IEnumerator LoginFunc(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            if (string.Compare(www.text, "Success!") == 0) // получаем в ответе слово-ключ из файла login.php
            {
                Message("Успешный вход!");
                PlayerDataHolder.user_name = userName.text;
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Message(www.text);
            }
        }
        else
        {
            Message("Error: " + www.error);
        }
        www.Dispose();
    }

    [System.Obsolete]
    IEnumerator RegisterFunc(WWW www)
    {
        yield return www;

        if (string.Compare(www.text, "Succesfully Created User!") == 0)
        {
            Message("Пользователь успешно добавлен в базу.");
        }
        else if (string.Compare(www.text, "User allready exists!") == 0)
        {
            Message(www.text);
        }
        else
        {
            Message("Error: " + www.error);
        }
        www.Dispose();
    }
    public void Exit()
    {
        Application.Quit();
    }
}
