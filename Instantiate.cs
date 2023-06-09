using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Instantiate : MonoBehaviour
{
    private GameObject[] prefabs, zombos;
    private List<GameObject> zomboPrefabs;
    private GameObject redTower, blueTower, greenTower, selectedTower, selectedButton, zombo,
                        choices, inventory, sellStore, mCamera, center, lastSelectedButton, gameScreens;
    private List<Button> towers;
    private Button red, blue, green, iRed, iBlue, iGreen, buildDone, sellButton, storeButton, upgradeButton,
        iRedL2, iRedL3, iRedL4, iBlueL2, iBlueL3, iBlueL4, iGreenL2, iGreenL3, iGreenL4;
    private bool setup, build, clicked, breakAgain, youWon, onTitle, gameOver, musicMute, sfxMute;
    private int health, i, redOnField, blueOnField, greenOnField, sellPrice, numLevels,
        rofL2, rofL3, rofL4, bofL2, bofL3, bofL4, gofL2, gofL3, gofL4, tLevel, upgradePrice;
    private static int level = 1, money = 1000, numRed = 0, numBlue = 0, numGreen = 0, num = 0, numZombos = 10,
        numRedL2 = 0, numRedL3 = 0, numRedL4 = 0, numBlueL2 = 0, numBlueL3 = 0, numBlueL4 = 0, numGreenL2 = 0,
        numGreenL3 = 0, numGreenL4 = 0;
    private static float zomboSpeed = 2f, wfs = 5f;
    private Text levelText, healthText, moneyText, numRedText, numBlueText, numGreenText, centerText,
        numRedL2Text, numRedL3Text, numRedL4Text, numBlueL2Text, numBlueL3Text, numBlueL4Text,
        numGreenL2Text, numGreenL3Text, numGreenL4Text, enemiesText;
    private Color32 selectedRed, selectedBlue, selectedGreen, originalColor;
    private TowerController tc;
    private TLController tl;
    private Material rL1, rL2, rL3, rL4, bL1, bL2, bL3, bL4, gL1, gL2, gL3, gL4, material;
    private static GameObject titleScreen;
    private static bool triedToReset = false;



    public void Awake()
    {
        gameScreens = GameObject.Find("Game Screens");
        if (!titleScreen)
            titleScreen = GameObject.Find("Title Screen");

        if (!triedToReset)
        {
            if (level > 1)
            {
                GetComponent<TitleController>().enabled = false;
                gameScreens.SetActive(true);
                titleScreen.SetActive(false);

                gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Pre-Level Build Mode");
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            level = 1;  money = 1000; numRed = 0; numBlue = 0; numGreen = 0; num = 0; numZombos = 10;
            numRedL2 = 0; numRedL3 = 0; numRedL4 = 0; numBlueL2 = 0; numBlueL3 = 0; numBlueL4 = 0; numGreenL2 = 0;
            numGreenL3 = 0; numGreenL4 = 0;

            zomboSpeed = 2f; wfs = 5f;

            triedToReset = false;
        }

        
    }

    // Start is called before the first frame update
    public void Start()
    {
        prefabs = Resources.LoadAll<GameObject>("Prefabs");
        towers = new List<Button>();
        zomboPrefabs = new List<GameObject>();

        foreach (GameObject prefab in prefabs)
        {
            if (prefab.tag == "Tower")
            {
                switch (prefab.name)
                {
                    case "Red Tower":
                        redTower = prefab; redTower.GetComponent<TLController>().SetUp();
                        break;
                    case "Blue Tower": blueTower = prefab; blueTower.GetComponent<TLController>().SetUp(); break;
                    case "Green Tower": greenTower = prefab; greenTower.GetComponent<TLController>().SetUp(); break;
                }
            }

            if (prefab.tag == "Enemy")
                zomboPrefabs.Add(prefab);
        }

        choices = GameObject.Find("Choices");
        zombo = Resources.Load<GameObject>("Prefabs/zombie");
        sellStore = GameObject.Find("Sell/Store");
        mCamera = Camera.main.gameObject;
        center = GameObject.Find("Center Text Background");

        choices.SetActive(true);
        sellStore.SetActive(false);
        center.SetActive(false);

        red = choices.transform.GetChild(0).GetComponent<Button>();
        blue = choices.transform.GetChild(1).GetComponent<Button>();
        green = choices.transform.GetChild(2).GetComponent<Button>();
        inventory = choices.transform.GetChild(3).gameObject;
        iRed = inventory.transform.GetChild(0).GetComponent<Button>();
        iBlue = inventory.transform.GetChild(1).GetComponent<Button>();
        iGreen = inventory.transform.GetChild(2).GetComponent<Button>();
        buildDone = GameObject.Find("Build/Done").GetComponent<Button>();
        sellButton = sellStore.transform.GetChild(0).GetComponent<Button>();
        storeButton = sellStore.transform.GetChild(1).GetComponent<Button>();
        upgradeButton = sellStore.transform.GetChild(2).GetComponent<Button>();
        iRedL2 = inventory.transform.GetChild(3).GetComponent<Button>();
        iRedL3 = inventory.transform.GetChild(4).GetComponent<Button>();
        iRedL4 = inventory.transform.GetChild(5).GetComponent<Button>();
        iBlueL2 = inventory.transform.GetChild(6).GetComponent<Button>();
        iBlueL3 = inventory.transform.GetChild(7).GetComponent<Button>();
        iBlueL4 = inventory.transform.GetChild(8).GetComponent<Button>();
        iGreenL2 = inventory.transform.GetChild(9).GetComponent<Button>();
        iGreenL3 = inventory.transform.GetChild(10).GetComponent<Button>();
        iGreenL4 = inventory.transform.GetChild(11).GetComponent<Button>();

        red.onClick.AddListener(delegate { OnClick(red.gameObject); });
        blue.onClick.AddListener(delegate { OnClick(blue.gameObject); });
        green.onClick.AddListener(delegate { OnClick(green.gameObject); });
        iRed.onClick.AddListener(delegate { OnClick(iRed.gameObject); });
        iBlue.onClick.AddListener(delegate { OnClick(iBlue.gameObject); });
        iGreen.onClick.AddListener(delegate { OnClick(iGreen.gameObject); });
        buildDone.onClick.AddListener(delegate { OnClick(buildDone.gameObject); });
        sellButton.onClick.AddListener(delegate { OnClick(sellButton.gameObject); });
        storeButton.onClick.AddListener(delegate { OnClick(storeButton.gameObject); });
        upgradeButton.onClick.AddListener(delegate { OnClick(upgradeButton.gameObject); });
        iRedL2.onClick.AddListener(delegate { OnClick(iRedL2.gameObject); });
        iRedL3.onClick.AddListener(delegate { OnClick(iRedL3.gameObject); });
        iRedL4.onClick.AddListener(delegate { OnClick(iRedL4.gameObject); });
        iBlueL2.onClick.AddListener(delegate { OnClick(iBlueL2.gameObject); });
        iBlueL3.onClick.AddListener(delegate { OnClick(iBlueL3.gameObject); });
        iBlueL4.onClick.AddListener(delegate { OnClick(iBlueL4.gameObject); });
        iGreenL2.onClick.AddListener(delegate { OnClick(iGreenL2.gameObject); });
        iGreenL3.onClick.AddListener(delegate { OnClick(iGreenL3.gameObject); });
        iGreenL4.onClick.AddListener(delegate { OnClick(iGreenL4.gameObject); });

        selectedRed = new Color32(150, 4, 20, 255);
        selectedBlue = new Color32(13, 15, 150, 255);
        selectedGreen = new Color32(0, 150, 20, 255);


        setup = false; build = true; clicked = false; onTitle = false;
        breakAgain = true; youWon = false; gameOver = false;
        musicMute = false; sfxMute = false;
        health = 100; i = 0; redOnField = 0; blueOnField = 0;
        greenOnField = 0; sellPrice = 0; numLevels = 5;
        rofL2 = 0; rofL3 = 0; rofL4 = 0; bofL2 = 0; bofL3 = 0;
        bofL4 = 0; gofL2 = 0; gofL3 = 0; gofL4 = 0; tLevel = 0;

        levelText = GameObject.Find("Level").GetComponent<Text>();
        healthText = GameObject.Find("Health").GetComponent<Text>();
        moneyText = GameObject.Find("Money").GetComponent<Text>();
        enemiesText = GameObject.Find("Enemies").GetComponent<Text>();
        numRedText = iRed.gameObject.transform.GetChild(0).GetComponent<Text>();
        numBlueText = iBlue.gameObject.transform.GetChild(0).GetComponent<Text>();
        numGreenText = iGreen.gameObject.transform.GetChild(0).GetComponent<Text>();
        centerText = center.transform.GetChild(0).GetComponent<Text>();
        numRedL2Text = iRedL2.gameObject.transform.GetChild(0).GetComponent<Text>();
        numRedL3Text = iRedL3.gameObject.transform.GetChild(0).GetComponent<Text>();
        numRedL4Text = iRedL4.gameObject.transform.GetChild(0).GetComponent<Text>();
        numBlueL2Text = iBlueL2.gameObject.transform.GetChild(0).GetComponent<Text>();
        numBlueL3Text = iBlueL3.gameObject.transform.GetChild(0).GetComponent<Text>();
        numBlueL4Text = iBlueL4.gameObject.transform.GetChild(0).GetComponent<Text>();
        numGreenL2Text = iGreenL2.gameObject.transform.GetChild(0).GetComponent<Text>();
        numGreenL3Text = iGreenL3.gameObject.transform.GetChild(0).GetComponent<Text>();
        numGreenL4Text = iGreenL4.gameObject.transform.GetChild(0).GetComponent<Text>();

        levelText.text = "Level: " + level;
        healthText.text = "Health: " + health;
        moneyText.text = "Money: $" + money;
        enemiesText.text = "Enemies: " + (numZombos - i);
        centerText.text = "Level Clear!";
        numRedText.text = numRed.ToString();
        numBlueText.text = numBlue.ToString();
        numGreenText.text = numGreen.ToString();
        numRedL2Text.text = numRedL2.ToString();
        numRedL3Text.text = numRedL3.ToString();
        numRedL4Text.text = numRedL4.ToString();
        numBlueL2Text.text = numBlueL2.ToString();
        numBlueL3Text.text = numBlueL3.ToString();
        numBlueL4Text.text = numBlueL4.ToString();
        numGreenL2Text.text = numGreenL2.ToString();
        numGreenL3Text.text = numGreenL3.ToString();
        numGreenL4Text.text = numGreenL4.ToString();
        

        rL1 = Resources.Load<Material>("Materials/TowerLevels/RedL1");
        rL2 = Resources.Load<Material>("Materials/TowerLevels/RedL2");
        rL3 = Resources.Load<Material>("Materials/TowerLevels/RedL3");
        rL4 = Resources.Load<Material>("Materials/TowerLevels/RedL4");
        bL1 = Resources.Load<Material>("Materials/TowerLevels/BlueL1");
        bL2 = Resources.Load<Material>("Materials/TowerLevels/BlueL2");
        bL3 = Resources.Load<Material>("Materials/TowerLevels/BlueL3");
        bL4 = Resources.Load<Material>("Materials/TowerLevels/BlueL4");
        gL1 = Resources.Load<Material>("Materials/TowerLevels/GreenL1");
        gL2 = Resources.Load<Material>("Materials/TowerLevels/GreenL2");
        gL3 = Resources.Load<Material>("Materials/TowerLevels/GreenL3");
        gL4 = Resources.Load<Material>("Materials/TowerLevels/GreenL4");


        int j = Random.Range(1, 4);
        if (j == 1 && level > 1) StartCoroutine("Break");
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<TitleController>().isTitleScreenActive() && !onTitle)
        {
            onTitle = true;
            gameScreens.SetActive(false);
        }
        if(!GetComponent<TitleController>().isTitleScreenActive() && onTitle)
        {
            onTitle = false;
            gameScreens.SetActive(true);
            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Pre-Level Build Mode");
            gameObject.GetComponent<AudioSource>().Play();
        }

        if(GetComponent<TitleController>().isMusicMute() && !musicMute)
        {
            musicMute = true;
            titleScreen.GetComponent<AudioSource>().mute = true;
            GetComponent<AudioSource>().mute = true;
        }
        if (!GetComponent<TitleController>().isMusicMute() && musicMute)
        {
            musicMute = false;
            titleScreen.GetComponent<AudioSource>().mute = false;
            GetComponent<AudioSource>().mute = false;
        }


        if (GetComponent<TitleController>().isSFXMute() && !sfxMute)
        {
            sfxMute = true;
            foreach(GameObject prefab in prefabs)
            {
                if (prefab.GetComponent<AudioSource>())
                    prefab.GetComponent<AudioSource>().mute = true;
            }

            GameObject.Find("EventSystem").GetComponent<AudioSource>().mute = true;
        }
        if (!GetComponent<TitleController>().isSFXMute() && sfxMute)
        {
            sfxMute = false;
            foreach (GameObject prefab in prefabs)
            {
                if (prefab.GetComponent<AudioSource>())
                    prefab.GetComponent<AudioSource>().mute = false;
            }

            GameObject.Find("EventSystem").GetComponent<AudioSource>().mute = false;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            triedToReset = true;
            SceneManager.LoadScene(0);
        }
            

        zombos = GameObject.FindGameObjectsWithTag("Enemy");

        if (!setup && GetComponent<PathGenerator>().GetFinished())
        {
            foreach (GameObject tower in GetComponent<PathGenerator>().GetTowers())
                towers.Add(tower.GetComponent<Button>());

            foreach (Button button in towers)
                button.onClick.AddListener(delegate { OnClick(button.gameObject); });

            setup = true;
        }

        if (health <= 0 && !gameOver)
        {
            gameOver = true;
            StopAllCoroutines();
            StartCoroutine("GameOver");
        }
        else if (i == numZombos && zombos.Length == 0 && health > 0 && !youWon)
        { 
            youWon = true;
            StopAllCoroutines();
            StartCoroutine("YouWon");
        }
            
        if (selectedButton)
        {
            if (!lastSelectedButton)
            {
                lastSelectedButton = selectedButton;
                originalColor = selectedButton.GetComponent<Image>().color;
            }

            lastSelectedButton.GetComponent<Image>().color = originalColor;
            originalColor = selectedButton.GetComponent<Image>().color;


            switch (selectedButton.name)
            {
                case "RedCount":
                    selectedButton.GetComponent<Image>().color = selectedRed;
                    break;

                case "BlueCount":
                    selectedButton.GetComponent<Image>().color = selectedBlue;
                    break;
                case "GreenCount":
                    selectedButton.GetComponent<Image>().color = selectedGreen;
                    break;
                case "Red L2":
                    selectedButton.GetComponent<Image>().color = selectedRed;
                    break;
                case "Red L3":
                    selectedButton.GetComponent<Image>().color = selectedRed;
                    break;
                case "Red L4":
                    selectedButton.GetComponent<Image>().color = selectedRed;
                    break;
                case "Blue L2":
                    selectedButton.GetComponent<Image>().color = selectedBlue;
                    break;
                case "Blue L3":
                    selectedButton.GetComponent<Image>().color = selectedBlue;
                    break;
                case "Blue L4":
                    selectedButton.GetComponent<Image>().color = selectedBlue;
                    break;
                case "Green L2":
                    selectedButton.GetComponent<Image>().color = selectedGreen;
                    break;
                case "Green L3":
                    selectedButton.GetComponent<Image>().color = selectedGreen;
                    break;
                case "Green L4":
                    selectedButton.GetComponent<Image>().color = selectedGreen;
                    break;
                default: break;
            }

            lastSelectedButton = selectedButton;
        }
        else if (lastSelectedButton)
            lastSelectedButton.GetComponent<Image>().color = originalColor;
            

    }

    public void OnClick(GameObject button)
    {
        if (clicked)
            return;
            
        clicked = true;

        GameObject.Find("EventSystem").GetComponent<AudioSource>().Play();

        switch (button.name)
        {
            case "Red":
                selectedButton = null;
                if (money >= 300)
                {
                    money -= 300;
                    moneyText.text = "Money: $" + money;
                    numRed++;
                    numRedText.text = numRed.ToString();
                }
                break;
            case "Blue":
                selectedButton = null;
                if (money >= 200)
                {
                    money -= 200;
                    moneyText.text = "Money: $" + money;
                    numBlue++;
                    numBlueText.text = numBlue.ToString();
                }
                break;
            case "Green":
                selectedButton = null;
                if (money >= 100)
                {
                    money -= 100;
                    moneyText.text = "Money: $" + money;
                    numGreen++;
                    numGreenText.text = numGreen.ToString();
                }
                break;

            case "RedCount":

                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 1;
                selectedButton = button;
                selectedTower = redTower;
                num = numRed;
                tc = selectedTower.GetComponent<TowerController>();
                tc.SetLevel(tLevel);
                tl = selectedTower.GetComponent<TLController>();
                tl.SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numRed > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;
            case "BlueCount":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 1;
                selectedButton = button;
                selectedTower = blueTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                num = numBlue;
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numBlue > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;
            case "GreenCount":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 1;
                selectedButton = button;
                selectedTower = greenTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                num = numGreen;
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numGreen > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);

                break;


            case "Red L2":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 2;
                num = numRedL2;
                selectedButton = button;
                selectedTower = redTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numRedL2 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);

                break;

            case "Red L3":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 3;
                num = numRedL3;
                selectedButton = button;
                selectedTower = redTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numRedL3 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Red L4":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 4;
                num = numRedL4;
                selectedButton = button;
                selectedTower = redTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numRedL4 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;


            case "Blue L2":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 2;
                num = numBlueL2;
                selectedButton = button;
                selectedTower = blueTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numBlueL2 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Blue L3":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 3;
                num = numBlueL3;
                selectedButton = button;
                selectedTower = blueTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numBlueL3 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Blue L4":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 4;
                num = numBlueL4;
                selectedButton = button;
                selectedTower = blueTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numBlueL4 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Green L2":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 2;
                num = numGreenL2;
                selectedButton = button;
                selectedTower = greenTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numGreenL2 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Green L3":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 3;
                num = numGreenL3;
                selectedButton = button;
                selectedTower = greenTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numGreenL3 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Green L4":
                storeButton.gameObject.SetActive(false);
                upgradeButton.gameObject.SetActive(false);
                tLevel = 4;
                num = numGreenL4;
                selectedButton = button;
                selectedTower = greenTower;
                selectedTower.GetComponent<TowerController>().SetLevel(tLevel);
                selectedTower.GetComponent<TLController>().SetPrices();
                sellPrice = selectedTower.GetComponent<TLController>().GetPrice();
                sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + tLevel + ": $" + sellPrice;
                if (numGreenL4 > 0)
                    sellStore.SetActive(true);
                else
                    sellStore.SetActive(false);
                break;

            case "Build/Done":
                mCamera.GetComponent<CameraController>().Clear();
                if (build)
                {
                    if (button.transform.GetChild(0).GetComponent<Text>().text == "Start")
                    {
                        int a = level; if (level > 5) a = level - 5;
                        string clip = "Audio/Battle " + a;
                        gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>
                            (clip);
                        gameObject.GetComponent<AudioSource>().Play();
                        StartCoroutine("SpawnZombos");
                    }
                        

                    build = false;
                    selectedButton = null;
                    selectedTower = null;
                    num = 0;
                    button.transform.GetChild(0).GetComponent<Text>().text = "Build";
                    choices.SetActive(false);
                    sellStore.SetActive(false);
                    DisableTowerButtons();
                }
                else
                {
                    build = true;
                    choices.SetActive(true);
                    button.transform.GetChild(0).GetComponent<Text>().text = "Done";
                    EnableTowerButtons();
                }
                break;

            case "Sell":
                selectedButton = null;
                int ignore = 0;
                bool onField = false;

                if(selectedTower.GetComponent<TLController>().GetHasTower())
                {
                    onField = true;
                    tl = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TLController>();
                    tc = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TowerController>();
                }
                else
                {
                    tl = selectedTower.GetComponent<TLController>();
                    tc = selectedTower.GetComponent<TowerController>();
                }

                switch (tl.GetColorNType())
                {
                    case "redInventory":
                        switch (tl.GetComponent<TowerController>().GetLevel())
                        {
                            case 1:
                                numRed--; numRedText.text = numRed.ToString();
                                if (numRed == 0) ignore++; break;
                            case 2:
                                numRedL2--; numRedL2Text.text = numRedL2.ToString();
                                if (numRedL2 == 0) ignore++; break;
                            case 3:
                                numRedL3--; numRedL3Text.text = numRedL3.ToString();
                                if (numRedL3 == 0) ignore++; break;
                            case 4:
                                numRedL4--; numRedL4Text.text = numRedL4.ToString();
                                if (numRedL4 == 0) ignore++; break;
                            default: break;

                        }
                        break;


                    case "blueInventory":
                        switch (tl.GetComponent<TowerController>().GetLevel())
                        {
                            case 1:
                                numBlue--; numBlueText.text = numBlue.ToString();
                                if (numBlue == 0) ignore++; break;
                            case 2:
                                numBlueL2--; numBlueL2Text.text = numBlueL2.ToString();
                                if (numBlueL2 == 0) ignore++; break;
                            case 3:
                                numBlueL3--; numBlueL3Text.text = numBlueL3.ToString();
                                if (numBlueL3 == 0) ignore++; break;
                            case 4:
                                numBlueL4--; numBlueL4Text.text = numBlueL4.ToString();
                                if (numBlueL4 == 0) ignore++; break;
                            default: break;

                        }
                        break;
                    case "greenInventory":
                        switch (tl.GetComponent<TowerController>().GetLevel())
                        {
                            case 1:
                                numGreen--; numGreenText.text = numGreen.ToString();
                                if (numGreen == 0) ignore++; break;
                            case 2:
                                numGreenL2--; numGreenL2Text.text = numGreenL2.ToString();
                                if (numGreenL2 == 0) ignore++; break;
                            case 3:
                                numGreenL3--; numGreenL3Text.text = numGreenL3.ToString();
                                if (numGreenL3 == 0) ignore++; break;
                            case 4:
                                numGreenL4--; numGreenL4Text.text = numGreenL4.ToString();
                                if (numGreenL4 == 0) ignore++; break;
                            default: break;

                        }
                        break;
                    case "redField":
                        switch (tc.GetLevel())
                        {
                            case 1: redOnField--; if (redOnField == 0) ignore++; break;
                            case 2: rofL2--; if (rofL2 == 0) ignore++; break;
                            case 3: rofL3--; if (rofL3 == 0) ignore++; break;
                            case 4: rofL4--; if (rofL4 == 0) ignore++; break;
                            default: break;
                        }
                        break;

                    case "blueField":
                        switch (tc.GetLevel())
                        {
                            case 1: blueOnField--; if (blueOnField == 0) ignore++; break;
                            case 2: bofL2--; if (bofL2 == 0) ignore++; break;
                            case 3: bofL3--; if (bofL3 == 0) ignore++; break;
                            case 4: bofL4--; if (bofL4 == 0) ignore++; break;
                            default: break;
                        }
                        break;
                    case "greenField":
                        switch (tc.GetLevel())
                        {
                            case 1: greenOnField--; if (greenOnField == 0) ignore++; break;
                            case 2: gofL2--; if (gofL2 == 0) ignore++; break;
                            case 3: gofL3--; if (gofL3 == 0) ignore++; break;
                            case 4: gofL4--; if (gofL4 == 0) ignore++; break;
                            default: break;
                        }
                        break;
                    default: Debug.Log(tl.GetColorNType()); break;
                }

                selectedTower.GetComponent<TLController>().Clear();

                money += sellPrice;
                moneyText.text = "Money: $" + money;

                if (ignore > 0 || onField)
                {
                    selectedTower = null;
                    sellStore.SetActive(false);
                }

                break;

            case "Store":
                selectedButton = null;
                ignore = 0;

                tc = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TowerController>();

                switch (selectedTower.GetComponent<TLController>().GetColorNType())
                {
                    case "redField":
                        switch (tc.GetLevel())
                        {
                            case 1:
                                redOnField--; numRed++; numRedText.text = numRed.ToString();
                                if (redOnField == 0) ignore++; break;
                            case 2:
                                rofL2--; numRedL2++; numRedL2Text.text = numRedL2.ToString();
                                if (rofL2 == 0) ignore++; break;
                            case 3:
                                rofL3--; numRedL3++; numRedL3Text.text = numRedL3.ToString();
                                if (rofL3 == 0) ignore++; break;
                            case 4:
                                rofL4--; numRedL4++; numRedL4Text.text = numRedL4.ToString();
                                if (rofL4 == 0) ignore++; break;
                        }
                        break;

                    case "blueField":
                        switch (tc.GetLevel())
                        {
                            case 1:
                                blueOnField--; numBlue++; numBlueText.text = numBlue.ToString();
                                if (blueOnField == 0) ignore++; break;
                            case 2:
                                bofL2--; numBlueL2++; numBlueL2Text.text = numBlueL2.ToString();
                                if (bofL2 == 0) ignore++; break;
                            case 3:
                                bofL3--; numBlueL3++; numBlueL3Text.text = numBlueL3.ToString();
                                if (bofL3 == 0) ignore++; break;
                            case 4:
                                bofL4--; numBlueL4++; numBlueL4Text.text = numBlueL4.ToString();
                                if (bofL4 == 0) ignore++; break;
                        }
                        break;
                    case "greenField":
                        switch (tc.GetLevel())
                        {
                            case 1:
                                greenOnField--; numGreen++; numGreenText.text = numGreen.ToString();
                                if (greenOnField == 0) ignore++; break;
                            case 2:
                                gofL2--; numGreenL2++; numGreenL2Text.text = numGreenL2.ToString();
                                if (gofL2 == 0) ignore++; break;
                            case 3:
                                gofL3--; numGreenL3++; numGreenL3Text.text = numGreenL3.ToString();
                                if (gofL3 == 0) ignore++; break;
                            case 4:
                                gofL4--; numGreenL4++; numGreenL4Text.text = numGreenL4.ToString();
                                if (gofL4 == 0) ignore++; break;
                        }
                        break;
                    default: break;
                }

                selectedTower.GetComponent<TLController>().Clear();

                selectedTower = null;
                sellStore.SetActive(false);


                break;


            case "Upgrade":
                tl = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TLController>();
                tc = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TowerController>();

                if (money >= upgradePrice)
                {
                    money -= upgradePrice;
                    moneyText.text = "Money: $" + money;

                    switch (tl.GetColorNType())
                    {
                        case "redField":
                            switch (tc.GetLevel())
                            {
                                case 1:
                                    redOnField--; rofL2++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = rL2; break;
                                case 2:
                                    rofL2--; rofL3++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = rL3; break;
                                case 3:
                                    rofL3--; rofL4++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = rL4; break;
                                default: break;
                            }
                            break;
                        case "blueField":
                            switch (tc.GetLevel())
                            {
                                case 1:
                                    blueOnField--; bofL2++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = bL2; break;
                                case 2:
                                    bofL2--; bofL3++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = bL3; break;
                                case 3:
                                    bofL3--; bofL4++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = bL4; break;
                                default: break;
                            }
                            break;
                        case "greenField":
                            switch (tc.GetLevel())
                            {
                                case 1:
                                    greenOnField--; gofL2++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = gL2; break;
                                case 2:
                                    gofL2--; gofL3++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = gL3; break;
                                case 3:
                                    gofL3--; gofL4++;
                                    tl.gameObject.GetComponent<MeshRenderer>().material = gL4; break;
                                default: break;
                            }
                            break;
                        default: break;
                    }

                    tc.SetLevel(tc.GetLevel() + 1);
                    tl.SetPrices();

                    upgradePrice = tl.GetUpgradePrice();
                    sellPrice = tl.GetPrice();

                    upgradeButton.transform.GetChild(0).GetComponent<Text>().text =
                        "Upgrade\nL" + (tc.GetLevel() + 1) + ": $" + upgradePrice;
                    sellButton.transform.GetChild(0).GetComponent<Text>().text =
                        "Sell\nL" + tc.GetLevel() + ": $" + sellPrice;
                }

                if (tc.GetLevel() == 4)
                    upgradeButton.gameObject.SetActive(false);

                break;

            default:
                bool hasTower = button.GetComponent<TLController>().GetHasTower();
                if (!hasTower && selectedTower && num > 0)
                {
                    switch (selectedTower.name)
                    {
                        case "Red Tower":
                            switch (selectedTower.GetComponent<TowerController>().GetLevel())
                            {
                                case 1:
                                    numRed--; numRedText.text = numRed.ToString();
                                    redOnField++; material = rL1; break;
                                case 2:
                                    numRedL2--; numRedL2Text.text = numRedL2.ToString();
                                    rofL2++; material = rL2; break;
                                case 3:
                                    numRedL3--; numRedL3Text.text = numRedL3.ToString();
                                    rofL3++; material = rL3; break;
                                case 4:
                                    numRedL4--; numRedL4Text.text = numRedL4.ToString();
                                    rofL4++; material = rL4; break;
                            }
                            break;

                        case "Blue Tower":
                            switch (selectedTower.GetComponent<TowerController>().GetLevel())
                            {
                                case 1:
                                    numBlue--; numBlueText.text = numBlue.ToString();
                                    blueOnField++; material = bL1; break;
                                case 2:
                                    numBlueL2--; numBlueL2Text.text = numBlueL2.ToString();
                                    bofL2++; material = bL2; break;
                                case 3:
                                    numBlueL3--; numBlueL3Text.text = numBlueL3.ToString();
                                    bofL3++; material = bL3; break;
                                case 4:
                                    numBlueL4--; numBlueL4Text.text = numBlueL4.ToString();
                                    bofL4++; material = bL4; break;
                            }
                            break;
                        case "Green Tower":
                            switch (selectedTower.GetComponent<TowerController>().GetLevel())
                            {
                                case 1:
                                    numGreen--; numGreenText.text = numGreen.ToString();
                                    greenOnField++; material = gL1; break;
                                case 2:
                                    numGreenL2--; numGreenL2Text.text = numGreenL2.ToString();
                                    gofL2++; material = gL2; break;
                                case 3:
                                    numGreenL3--; numGreenL3Text.text = numGreenL3.ToString();
                                    gofL3++; material = gL3; break;
                                case 4:
                                    numGreenL4--; numGreenL4Text.text = numGreenL4.ToString();
                                    gofL4++; material = gL4; break;
                            }
                            break;
                        default: break;
                    }

                    selectedTower.GetComponent<MeshRenderer>().material = material;

                    GameObject tClone =
                        Instantiate(selectedTower, button.transform.position, selectedTower.transform.rotation);

                    tClone.GetComponent<TLController>().SetUp();
                    tClone.GetComponent<TLController>().SetPrice(selectedTower.GetComponent<TLController>().GetPrice());
                    tClone.GetComponent<TLController>().SetUpgradePrice(selectedTower.GetComponent<TLController>().GetUpgradePrice());
                    tClone.GetComponent<TowerController>().SetLevel(tLevel);
                    button.GetComponent<TLController>().SetHasTower(true, tClone);

                    num--;
                }
                else if (hasTower)
                {
                    selectedButton = null;
                    selectedTower = button;
                    int l = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TowerController>().GetLevel();
                    sellPrice = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TLController>().GetPrice();
                    upgradePrice = selectedTower.GetComponent<TLController>().GetTower().GetComponent<TLController>().GetUpgradePrice();
                    upgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Upgrade\nL" + (l + 1) + ": $" + upgradePrice;
                    sellButton.transform.GetChild(0).GetComponent<Text>().text = "Sell\nL" + l + ": $" + sellPrice;
                    storeButton.gameObject.SetActive(true);

                    if (l < 4)
                        upgradeButton.gameObject.SetActive(true);
                    else
                        upgradeButton.gameObject.SetActive(false);

                    sellStore.SetActive(true);
                    num = -1;
                }
                else
                    num = 0;

                if (num == 0)
                {
                    selectedButton = null;
                    sellStore.SetActive(false);
                }

                break;
        }

        StartCoroutine("StopSecondClick");
    }


    IEnumerator SpawnZombos()
    {
        while (i < numZombos)
        {
            int j = Random.Range(0, 3);
            zombo = zomboPrefabs[j];

            GameObject zClone = Instantiate(zombo);
            zClone.GetComponent<ZomboController>().SetSpeed(zomboSpeed);
            enemiesText.text = "Enemies: " + (numZombos - (i + 1));

            yield return new WaitForSeconds(wfs);

            i++;
            
        }
    }

    IEnumerator GameOver()
    {
        centerText.text = "Game Over!";
        center.SetActive(true);

        gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>
                            ("Audio/Game Over");
        gameObject.GetComponent<AudioSource>().Play();

        if (GetComponent<AudioSource>().mute)
            yield return new WaitForSeconds(2);
        else
            yield return new WaitForSeconds(8);

        triedToReset = true;
        SceneManager.LoadScene(0);
    }

    IEnumerator YouWon()
    {
        if (level < numLevels)
        {
            centerText.text = "Level Clear!";
            center.SetActive(true);

            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>
                            ("Audio/Level Clear");
            gameObject.GetComponent<AudioSource>().Play();

            if (GetComponent<AudioSource>().mute)
                yield return new WaitForSeconds(2);
            else
                yield return new WaitForSeconds(4);
            level++;
            numZombos += 10;

            numRed += redOnField;
            numBlue += blueOnField;
            numGreen += greenOnField;
            numRedL2 += rofL2;
            numRedL3 += rofL3;
            numRedL4 += rofL4;
            numBlueL2 += bofL2;
            numBlueL3 += bofL3;
            numBlueL4 += bofL4;
            numGreenL2 += gofL2;
            numGreenL3 += gofL3;
            numGreenL4 += gofL4;

            zomboSpeed += 0.5f;
            if (wfs > 1) wfs -= 0.5f;
            SceneManager.LoadScene(0);
        }

        else
        {
            centerText.text = "You Won!";
            center.SetActive(true);

            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>
                            ("Audio/You Won");
            gameObject.GetComponent<AudioSource>().Play();

            if (GetComponent<AudioSource>().mute)
                yield return new WaitForSeconds(2);
            else
                yield return new WaitForSeconds(10);

            triedToReset = true;
            SceneManager.LoadScene(0);
        }
    }

    public void DisableTowerButtons()
    {
        foreach (Button tower in towers)
            tower.enabled = false;
    }

    public void EnableTowerButtons()
    {
        foreach (Button tower in towers)
            tower.enabled = true;
    }

    IEnumerator Break()
    {
        while(breakAgain)
        {
            breakAgain = false;
            int i = Random.Range(1, 4);
            int j = Random.Range(1, 5);

            int ignore = 0;

            switch (i)
            {
                case 1:
                    switch (j)
                    {
                        case 1:
                            if (numRed == 0) { ignore++; break; }
                            numRed--; numRedText.text = numRed.ToString();
                            centerText.text = "Bomb Tower (L1) Broke!"; break;
                        case 2:
                            if (numRedL2 == 0) { ignore++; break; }
                            numRedL2--; numRedL2Text.text = numRedL2.ToString();
                            centerText.text = "Bomb Tower (L2) Broke!"; break;
                        case 3:
                            if (numRedL3 == 0) { ignore++; break; }
                            numRedL3--; numRedL3Text.text = numRedL3.ToString();
                            centerText.text = "Bomb Tower (L3) Broke!"; break;
                        case 4:
                            if (numRedL4 == 0) { ignore++; break; }
                            numRedL4--; numRedL4Text.text = numRedL4.ToString();
                            centerText.text = "Bomb Tower (L4) Broke!"; break;
                        default: break;
                    }
                    break;

                case 2:
                    switch(j)
                    {
                        case 1:
                            if (numBlue == 0) { ignore++; break; }
                            numBlue--; numBlueText.text = numBlue.ToString(); 
                            centerText.text = "Spike Tower (L1) Broke!"; break;
                        case 2:
                            if (numBlueL2 == 0) { ignore++; break; }
                            numBlueL2--; numBlueL2Text.text = numBlueL2.ToString();
                            centerText.text = "Spike Tower (L2) Broke!"; break;
                        case 3:
                            if (numBlueL3 == 0) { ignore++; break; }
                            numBlueL3--; numBlueL3Text.text = numBlueL3.ToString();
                            centerText.text = "Spike Tower (L3) Broke!"; break;
                        case 4:
                            if (numBlueL4 == 0) { ignore++; break; }
                            numBlueL4--; numBlueL4Text.text = numBlueL4.ToString();
                            centerText.text = "Spike Tower (L4) Broke!"; break;
                        default: break;
                    }
                    break;
                    
                case 3:
                    switch(j)
                    {
                        case 1:
                            if (numGreen == 0) { ignore++; break; }
                            numGreen--; numGreenText.text = numGreen.ToString();
                            centerText.text = "Turret Tower (L1) Broke!"; break;
                        case 2:
                            if (numGreenL2 == 0) { ignore++; break; }
                            numGreenL2--; numGreenL2Text.text = numGreen.ToString();
                            centerText.text = "Turret Tower (L2) Broke!"; break;
                        case 3:
                            if (numGreenL3 == 0) { ignore++; break; }
                            numGreenL3--; numGreenL3Text.text = numGreen.ToString();
                            centerText.text = "Turret Tower (L3) Broke!"; break;
                        case 4:
                            if (numGreenL4 == 0) { ignore++; break; }
                            numGreenL4--; numGreenL4Text.text = numGreen.ToString();
                            centerText.text = "Turret Tower (L4) Broke!"; break;
                        default: break;
                    }
                    break;

                default: break;
            }

            if (ignore == 0)
            {
                center.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 50);
                center.SetActive(true);
                yield return new WaitForSeconds(2);
                center.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50);
                center.SetActive(false);
            }
            else
                breakAgain = true;
        }
        
    }

    IEnumerator StopSecondClick()
    {
        yield return new WaitForSeconds(0.1f);
        clicked = false;
    }

    public void Ouch(string name)
    {
        switch (name)
        {
            case "zombie(Clone)": health -= 10; break;
            case "zombiegirl_w_kurniawan(Clone)": health -= 5; break;
            case "maynard(Clone)": health -= 20; break;
            default: break;
        }

        if (health < 0) health = 0; 
        healthText.text = "Health: " + health; 
    }

    public int GetHealth()
    { return health; }

    public void AddMoney(int amount)
    { money += amount; moneyText.text = "Money: $" + money; }

    public bool GetBuildMode()
    { return build; }

    public void SetNumLevels(int num)
    { numLevels = num; }
}