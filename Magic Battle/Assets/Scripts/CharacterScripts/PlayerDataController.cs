using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PlayerDataController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthPText;
    [SerializeField]
    private TMP_Text buffPText;
    [SerializeField]
    private TMP_Text explossivePText;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private Transform playerSpawnPoint;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private string getScoreLevelURL = "http://magicbattle.ru/getScoreScale.php";
    [SerializeField]
    private string spentItemsURL = "http://magicbattle.ru/spentPotions.php";
    [SerializeField]
    private string getRewardURL = "http://magicbattle.ru/getAward.php";

    private Object player;

    //Buffering values;
    private int initHealQuant;
    private int initBuffQuant;
    private int initExplQuant;

    //Potions
    public int healthPotionsQuant;
    public int buffPotionsQuant;
    public int explosivePotionsQuant;
    public int level;
    public int score;
    public string userName;
    public bool isDeath;
    public Slider scoreBar;
    public string character;
    public bool _isClosed;
    private void Awake()
    {
        Cursor.visible = false;

        character = PlayerDataHolder.choosenCharacter;
        level = PlayerDataHolder.level_id;
        score = PlayerDataHolder.score;
        userName = PlayerDataHolder.user_name;
        healthPotionsQuant = PlayerDataHolder.healingP;
        buffPotionsQuant = PlayerDataHolder.buffP;
        explosivePotionsQuant = PlayerDataHolder.explossiveP;

        player = Resources.Load(character);
        GameObject playerGO = (GameObject)Instantiate(player, playerSpawnPoint.position, playerSpawnPoint.rotation);

        initExplQuant = PlayerDataHolder.explossiveP;
        initBuffQuant = PlayerDataHolder.buffP;
        initHealQuant = PlayerDataHolder.healingP;
        if (level == 3)
        {
            score = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetScoreScale();
        healthPText.text = healthPotionsQuant.ToString();
        buffPText.text = buffPotionsQuant.ToString();
        explossivePText.text = explosivePotionsQuant.ToString();
        isDeath = false;
        _isClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthPText.text = healthPotionsQuant.ToString();
        buffPText.text = buffPotionsQuant.ToString();
        explossivePText.text = explosivePotionsQuant.ToString();
        if (score < scoreBar.maxValue)
        {
            scoreBar.value = score;
        }
        if (score >= scoreBar.maxValue)
        {
            if (level > 2)
            {
                Debug.Log(level);
                level = 2;
            }
            level++;
            score = 0;
            GetScoreScale();
        }
        scoreText.text = score.ToString() + "/" + maxValue.ToString();
        levelText.text = level.ToString();
    }
    private void GetScoreScale()
    {
        WWWForm form = new WWWForm();
        form.AddField("level_id", level.ToString());
        WWW www = new WWW(getScoreLevelURL, form);
        StartCoroutine(GetScoreScaleFunc(www));
    }
    IEnumerator GetScoreScaleFunc(WWW www)
    {
        yield return www;
        maxValue = Int32.Parse(www.text);
        scoreBar.maxValue = maxValue;
        scoreText.text = score.ToString() + "/" + maxValue.ToString();
        www.Dispose();
    }
    public void SpentPotions()
    {
        int explDiff = initExplQuant - explosivePotionsQuant;
        int healDiff = initHealQuant - healthPotionsQuant;
        int buffDiff = initBuffQuant - buffPotionsQuant;
        WWWForm form = new WWWForm();
        form.AddField("user_name", userName);
        form.AddField("expl_spent", explDiff.ToString());
        form.AddField("heal_spent", healDiff.ToString());
        form.AddField("buff_spent", buffDiff.ToString());
        WWW www = new WWW(spentItemsURL, form);
        StartCoroutine(SpentPotionsFunc(www));
    }
    IEnumerator SpentPotionsFunc(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            if (isDeath == false)
            {
                GetReward();
            }
            else if (isDeath == true || _isClosed == true)
            {
                SceneManager.LoadScene("Menu");
            }
        }
        else
        {
            Debug.Log(www.error.ToString());
        }
        www.Dispose();
    }
    public void GetReward()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", userName);
        form.AddField("level_id", level.ToString());
        form.AddField("score", score.ToString());
        WWW www = new WWW(getRewardURL, form);
        StartCoroutine(GetRewardFunc(www));
    }

    IEnumerator GetRewardFunc(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            Debug.Log(www.error.ToString());
        }
        www.Dispose();
    }
}
