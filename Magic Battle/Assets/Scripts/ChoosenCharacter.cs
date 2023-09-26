using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ChoosenCharacter : MonoBehaviour
{
    private GameObject choosenGO;
    private GameObject[] selectButtons;
    private GameObject[] selTexts;
    // Start is called before the first frame update
    void Start()
    {
        choosenGO = GameObject.FindGameObjectWithTag("ChoosenCharacter");
        selectButtons = GameObject.FindGameObjectsWithTag("ChooseBtn");
        selTexts = GameObject.FindGameObjectsWithTag("ChooseBtnText");
    }
    public void SetArissaChoosenCharacter()
    {
        choosenGO.name = "Arissa";
        foreach (GameObject go in selectButtons)
        {
            if (go.name == "ArissaChooseBtn")
            {
                continue;
            }
            else
            {
                go.GetComponent<Button>().interactable = true;
            }
        }
        foreach (GameObject t in selTexts)
        {
            if (t.name == "ArissaText (TMP)")
            {
                continue;
            }
            else
            {
                t.GetComponent<TextMeshProUGUI>().text = "Choose";
            }
        }
    }
    public void SetHenryChoosenCharacter()
    {
        choosenGO.name = "Henry";
        foreach (GameObject go in selectButtons)
        {
            if (go.name == "HenryChooseBtn")
            {
                continue;
            }
            else
            {
                go.GetComponent<Button>().interactable = true;
            }
        }
        foreach (GameObject t in selTexts)
        {
            if (t.name == "HenryText (TMP)")
            {
                continue;
            }
            else
            {
                t.GetComponent<TextMeshProUGUI>().text = "Choose";
            }
        }
    }
}
