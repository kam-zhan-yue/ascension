using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    HighscoresJSON highscores = new HighscoresJSON { };

    private class HighscoresJSON
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        //Quick Create Highscores or Refresh
        if (PlayerPrefs.HasKey("highscoreTable") == false)
            CreateNewHighscores();

        //Load the Table
        LoadTable();

        //Sort the Table
        SortTable();

        //Print the Table
        PrintTable();
    }

    //===============PROCEDURE===============//
    public static void AddHighscoreEntry(int score, string name)
    //Purpose:          Adds a new highscore entry to the JSON file
    //int waves:        Stores the number of waves that were completed
    //string name:     Stores the name of the last player
    {
        //Create a Highscore Entry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        //Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighscoresJSON highscores = JsonUtility.FromJson<HighscoresJSON>(jsonString);

        //Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //Save the updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    //===============PROCEDURE===============//
    public void CreateNewHighscores()
    //Purpose:          Creates a new HighscoresJSON file and some placeholder highscores
    {
        highscoreEntryList = new List<HighscoreEntry>()
        {
            //new HighscoreEntry{ name = "Aaron", score = 5 },
            //new HighscoreEntry{ name = "Phillip", score = 10},
            //new HighscoreEntry{ name = "Molly", score = 4 },
            //new HighscoreEntry{ name = "Ben", score = 6 },
            //new HighscoreEntry{ name = "Kenny", score = 2 },
        };

        HighscoresJSON highscores = new HighscoresJSON { highscoreEntryList = highscoreEntryList };
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
    }

    //===============PROCEDURE===============//
    private void LoadTable()
    //Purpose:          Loads the highscore JSON file
    {
        //Loads table from PlayerPrefs
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        highscores = JsonUtility.FromJson<HighscoresJSON>(jsonString);
    }

    //===============PROCEDURE===============//
    private void SortTable()
    //Purpose:          Sorts the highscores according to score
    {
        HighscoreEntry temp;
        bool swaps = true;
        do
        {
            swaps = false;
            for (int i = 0; i < highscores.highscoreEntryList.Count - 1; i++)
            {
                if (highscores.highscoreEntryList[i].score < highscores.highscoreEntryList[i + 1].score)
                {
                    temp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[i + 1];
                    highscores.highscoreEntryList[i + 1] = temp;
                    swaps = true;
                }
            }
        }
        while (swaps);
    }

    //===============PROCEDURE===============//
    private void PrintTable()
    //Purpose:          Creates a table comprised of game objects on screen
    {
        int i = 0;
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            i++;
            if (i <= 10)
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList, 0);
        }
    }

    //===============PROCEDURE===============//
    private void CreateHighscoreEntryTransform(HighscoreEntry entry, Transform container, List<Transform> transformList, int overrideRank)
    //Purpose:          Creates a game object according to coordinates, and transform
    //HighscoreEntry entry: Holds the values of score and player name
    //List<Transform> transformList: Determines where and what game objects are created
    //int overrideRank: Only to be used when searching as it shows true rank
    {
        float templateHeight = 50;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, 200+-templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1:
                rankString = "1ST";
                entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = new Color32(255, 215, 0, 255);
                entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = new Color32(255, 215, 0, 255);
                entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = new Color32(255, 215, 0, 255);
                Debug.Log("Make " + entry.name + " gold");
                break;
            case 2:
                rankString = "2ND";
                entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = new Color32(193, 193, 193, 255);
                entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = new Color32(193, 193, 193, 255);
                entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = new Color32(193, 193, 193, 255);
                break;
            case 3:
                rankString = "3RD";
                entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50,255);
                entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
                entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = new Color32(205, 127, 50, 255);
                break;
            default:
                rankString = rank + "TH";
                break;
        }
        int score = entry.score;
        string name = entry.name;
        if (overrideRank == 0)
            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;
        else
            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = overrideRank.ToString();
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        transformList.Add(entryTransform);
    }
}
