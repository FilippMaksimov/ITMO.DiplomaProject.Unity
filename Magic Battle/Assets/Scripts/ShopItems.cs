using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ShopItems : MonoBehaviour
{
    [SerializeField]
    private string getGoldURL = "http://magicbattle.ru/userSelect.php";
    [SerializeField]
    private string getUserItems = "http://magicbattle.ru/userGetItems.php";
    [SerializeField]
    private string buyCharacterURL = "http://magicbattle.ru/buyCharacter.php";
    [SerializeField]
    private string charStatURL = "http://magicbattle.ru/getCharacterStoreData.php";
    [SerializeField]
    private string buyItemsURL = "http://magicbattle.ru/buyItems.php";
    [SerializeField]
    private TMP_Text gold;
    [SerializeField]
    private TMP_Text healingPotions;
    [SerializeField]
    private TMP_Text explossivePotions;
    [SerializeField]
    private TMP_Text buffingPotions;
    [SerializeField]
    private GameObject chooseGO;
    [SerializeField]
    private GameObject choosePotion;
    [SerializeField]
    private GameObject messageBox;
    [SerializeField]
    private GameObject messageBox1;
    [SerializeField]
    private GameObject messageBox2;
    [SerializeField]
    private GameObject messageBox3;
    [SerializeField]
    private GameObject btn1;
    [SerializeField]
    private GameObject btn2;
    [SerializeField]
    private TMP_Text btn1Text;
    [SerializeField]
    private TMP_Text btn2Text;


    private string[] userI;

    // Start is called before the first frame update
    public void UpdateUserGold()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        WWW userGold = new WWW(getGoldURL, form);
        StartCoroutine(UpdateGoldFunc(userGold));
    }
    public void UpdateUserItems()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        WWW items = new WWW(getUserItems, form);
        StartCoroutine(UpdateItemsFunc(items));

    }
    public void CharacterScrollStates()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        form.AddField("character_id", "1");
        WWW char1 = new WWW(charStatURL, form);

        WWWForm form2 = new WWWForm();
        form2.AddField("user_name", GetComponent<MainMenuScript>().user_name);
        form2.AddField("character_id", "2");
        WWW char2 = new WWW(charStatURL, form2);

        StartCoroutine(CharacterUpdateStatus(char1, 1));
        StartCoroutine(CharacterUpdateStatus(char2, 2));
    }
    public void BuyCharacter()
    {
        int money = Int32.Parse(gold.text);
        int price = 0;
        int characterId = 0;
        if (chooseGO.name == "Arissa")
        {
            price = 100;
            characterId = 1;
        }
        else if (chooseGO.name == "Henry")
        {
            price = 5000;
            characterId = 2;
        }
        if (money - price >= 0)
        {
            money -= price;
            WWWForm form = new WWWForm();
            form.AddField("user_name", GetComponent<MainMenuScript>().user_name);
            form.AddField("character_id",characterId.ToString());
            form.AddField("money", money.ToString());
            WWW www = new WWW(buyCharacterURL, form);
            StartCoroutine(BuyCharacterFunc(www));
        }
        else
        {
            messageBox.SetActive(true);
        }

        Debug.Log(chooseGO.name);
    }
    public void BuyPotion()
    {
        int money = Int32.Parse(gold.text);
        int price = 50;
        int itemId = 0;
        if (choosePotion.name == "Expl")
        {
            itemId = 1;
        }
        else if (choosePotion.name == "Heal")
        {
            itemId = 2;
        }
        else if (choosePotion.name == "Buff")
        {
            itemId = 3;
        }
        if (money - price >= 0)
        {
            money -= price;
            WWWForm form = new WWWForm();
            form.AddField("user_name", PlayerDataHolder.user_name);
            form.AddField("item_id", itemId.ToString());
            form.AddField("money", money.ToString());
            WWW www = new WWW(buyItemsURL, form);
            StartCoroutine(BuyItemsFunc(www));
        }
        else
        {
            messageBox.SetActive(true);
        }
    }

    IEnumerator UpdateGoldFunc(WWW www)
    {
        yield return www;
        gold.text = www.text;
        www.Dispose();
    }
    IEnumerator UpdateItemsFunc(WWW www)
    {
        yield return www;
        userI = www.text.Split(';');
        explossivePotions.text = userI[0];
        healingPotions.text = userI[1];
        buffingPotions.text = userI[2]; 
        www.Dispose();
    }
    IEnumerator BuyCharacterFunc(WWW www)
    {
        yield return www;
        if (string.Compare(www.text, "Succesfully purchased!") == 0) 
        { 
            messageBox1.SetActive(true);
        }
        else if (string.Compare(www.text, "You have not reach level 2 for for upgrading") == 0)
        {
            messageBox2.SetActive(true);
        }
        else if (string.Compare(www.text, "You have not reach level 3 for for upgrading") == 0)
        {
            messageBox3.SetActive(true);
        }
        else
        {
            Debug.Log(www.text.ToString());
        }
        UpdateUserGold();
        CharacterScrollStates();
        www.Dispose();
    }
    IEnumerator BuyItemsFunc(WWW www)
    {
        yield return www;
        if (string.Compare(www.text, "Succesfully purchased!") == 0)
        {
            messageBox1.SetActive(true);
        }
        else Debug.Log(www.text.ToString());
        UpdateUserGold();
        UpdateUserItems();
        www.Dispose();
    }

    IEnumerator CharacterUpdateStatus(WWW www, int char_id)
    {
        yield return www;
        if (char_id == 1)
        {
            UIScrollValue(www.text, btn1, btn1Text);
        }
        else if (char_id == 2)
        {
            UIScrollValue(www.text, btn2, btn2Text);
        }
        www.Dispose();
    }
    private void UIScrollValue(string status, GameObject item, TMP_Text textUI)
    {
        if (string.Compare(status, "Buy") == 0)
        {
            textUI.text = "Buy";
        }
        else if (string.Compare(status, "Update") == 0)
        {
            textUI.text = "Upgrade";
        }
        else if (string.Compare(status, "Max") == 0)
        {
            textUI.text = "Max";
            item.GetComponent<Button>().interactable = false;
        }
    }
}
