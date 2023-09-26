using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class CharacterAndLevelChoose : MonoBehaviour
{
    [SerializeField]
    private GameObject noCharacterMessage;
    [SerializeField]
    private GameObject parentGO;
    [SerializeField]
    private string getCharactersURL = "http://magicbattle.ru/getCharacters.php";
    [SerializeField]
    private string getChrarcterPlayerURL = "http://magicbattle.ru/getCharacterPlayerData.php";
    [SerializeField]
    private string getPotionsURL = "http://magicbattle.ru/userGetItems.php";
    [SerializeField]
    private GameObject choosenLevel;
    [SerializeField]
    private GameObject noLevelSelectedMessage;
    [SerializeField]
    private GameObject noCharacterSelectedMessage;
    [SerializeField]
    private GameObject loadingPanel;

    private string _hp;
    private string _power;
    private string _speed;
    private string _score;
    private string _explossiveP;
    private string _healingP;
    private string _buffP;
    private string _level_id;
    public GameObject _choosenCharacter;

    public void GetCharacters()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        WWW www = new WWW(getCharactersURL, form);
        StartCoroutine(GetCharactersFunc(www));
    }
    IEnumerator GetCharactersFunc(WWW www)
    {
        yield return www;
        string[] characters = www.text.Split(';');
        List<Object> characherObjects = new List<Object>();
        foreach (string character in characters)
        {
            if (character != "")
            {
                characherObjects.Add(Resources.Load(character));
            }
        }
        if (characherObjects.Count > 0)
        {
            noCharacterMessage.SetActive(false);
            foreach (Object obj in characherObjects)
            {
                GameObject go = (GameObject)Instantiate(obj, parentGO.transform);
            }
        }
        else
        {
            noCharacterMessage.SetActive(true);
        }
        www.Dispose();
    }
    public void StartGame()
    {
        if (choosenLevel.name != "ChoosenLev")
        {
            if (_choosenCharacter.name != "ChoosenChar")
            {
                PlayerDataHolder.choosenCharacter = _choosenCharacter.name;
                loadingPanel.SetActive(true);
                GetCharacterPlayerData();
            }
            else
            {
                noCharacterSelectedMessage.SetActive(true);
            }
        }
        else
        {
            noLevelSelectedMessage.SetActive(true);
        }
    }
    private void GetCharacterPlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        form.AddField("character_name", _choosenCharacter.name + "Character");
        WWW www = new WWW(getChrarcterPlayerURL, form);
        StartCoroutine(GetPlayerDataFunc(www));
    }
    IEnumerator GetPlayerDataFunc(WWW www) 
    {
        yield return www;
        string[] data = www.text.Split(';');
        _hp = data[0];
        _power = data[1];
        _speed = data[2];
        _level_id = data[3];
        _score = data[4];
        GetPotions();
        www.Dispose();
    }
    private void GetPotions()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        WWW items = new WWW(getPotionsURL, form);
        StartCoroutine(GetPotionsFunc(items));
    }
    IEnumerator GetPotionsFunc(WWW www)
    {
        yield return www;
        string[] potions = www.text.Split(";"); 
        _explossiveP = potions[0];
        _healingP = potions[1];
        _buffP = potions[2];
        PlayerDataHolder.hp = Int32.Parse(_hp);
        PlayerDataHolder.power = Int32.Parse(_power);
        PlayerDataHolder.speed = float.Parse(_speed, CultureInfo.InvariantCulture.NumberFormat);
        PlayerDataHolder.level_id = Int32.Parse(_level_id);
        PlayerDataHolder.score = Int32.Parse(_score);
        PlayerDataHolder.explossiveP = Int32.Parse(_explossiveP);
        PlayerDataHolder.healingP = Int32.Parse(_healingP);
        PlayerDataHolder.buffP = Int32.Parse(_buffP);

        if (choosenLevel.name == "Survival")
        {
            SceneManager.LoadScene("Playground");
        }
        else if (choosenLevel.name == "Storm")
        {
            SceneManager.LoadScene("Playground2");
        }

        www.Dispose();
    }
    public void Back()
    {
        GameObject[] charEl = GameObject.FindGameObjectsWithTag("ChTemplate");
        foreach (GameObject ch in charEl)
        {
            Destroy(ch);
        }
        _choosenCharacter.name = "ChoosenChar";
    }
}
