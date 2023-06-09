using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    private GameObject titleScreen, homeScreen, instructionsScreen, optionsScreen, onScreen;
    private Button play, instructions, options, quit, backFromInstructions, backFromOptions;
    private Slider levelSlider;
    private Toggle music, sfx;
    private Text numLevelsText;
    private bool activeTitle;
    private static int numLevels = 5;
    private static bool musicMute = false, sfxMute = false;


    // Start is called before the first frame update
    void Start()
    {
        titleScreen = GameObject.Find("Title Screen");
        homeScreen = GameObject.Find("Home Screen");
        instructionsScreen = GameObject.Find("Instructions Screen");
        optionsScreen = GameObject.Find("Options Screen");
        onScreen = homeScreen;

        play = GameObject.Find("Play").GetComponent<Button>();
        instructions = GameObject.Find("Instructions").GetComponent<Button>();
        options = GameObject.Find("Options").GetComponent<Button>();
        quit = GameObject.Find("Quit").GetComponent<Button>();
        backFromInstructions = instructionsScreen.transform.GetChild(2).GetComponent<Button>();
        backFromOptions = optionsScreen.transform.GetChild(2).GetComponent<Button>();

        play.onClick.AddListener(delegate { OnClick(play.gameObject); });
        instructions.onClick.AddListener(delegate { OnClick(instructions.gameObject); });
        options.onClick.AddListener(delegate { OnClick(options.gameObject); });
        quit.onClick.AddListener(delegate { OnClick(quit.gameObject); });
        backFromInstructions.onClick.AddListener(delegate { OnClick(backFromInstructions.gameObject); });
        backFromOptions.onClick.AddListener(delegate { OnClick(backFromOptions.gameObject); });

        levelSlider = GameObject.Find("Slider").GetComponent<Slider>();
        levelSlider.value = numLevels;
        levelSlider.onValueChanged.AddListener(delegate { OnSlide(); });
        GetComponent<Instantiate>().SetNumLevels((int)levelSlider.value);

        

        music = GameObject.Find("Music").GetComponent<Toggle>();
        music.isOn = !musicMute;
        music.onValueChanged.AddListener(delegate { OnToggle(music.gameObject); });
        sfx = GameObject.Find("SFX").GetComponent<Toggle>();
        sfx.isOn = !sfxMute;
        sfx.onValueChanged.AddListener(delegate { OnToggle(sfx.gameObject); });

        numLevelsText = GameObject.Find("Value").GetComponent<Text>();
        numLevelsText.text = levelSlider.value.ToString();

        activeTitle = true;


        titleScreen.SetActive(true);
        homeScreen.SetActive(true);
        instructionsScreen.SetActive(false);
        optionsScreen.SetActive(false);
    }


    public void OnClick(GameObject button)
    {
        GameObject.Find("EventSystem").GetComponent<AudioSource>().Play();

        switch (button.name)
        {
            case "Play":
                {
                    titleScreen.SetActive(false);

                    activeTitle = false;
                    onScreen = null;
                    break;
                }
            case "Instructions":
                {
                    homeScreen.SetActive(false);
                    optionsScreen.SetActive(false);

                    instructionsScreen.SetActive(true);
                    onScreen = instructionsScreen;
                    break;
                }
            case "Options":
                {
                    homeScreen.SetActive(false);
                    instructionsScreen.SetActive(false);

                    optionsScreen.SetActive(true);
                    onScreen = optionsScreen;
                    break;
                }
            case "Quit":
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
                    break;
                }
            case "Back":
                {
                    if(onScreen == instructionsScreen)
                    {
                        instructionsScreen.SetActive(false);
                        homeScreen.SetActive(true);
                    }
                    else
                    {
                        optionsScreen.SetActive(false);
                        homeScreen.SetActive(true);
                    }
                    
                    break;
                }
            default: break;
        }
    }


    public void OnSlide()
    {
        GameObject.Find("EventSystem").GetComponent<AudioSource>().Play();
        numLevelsText.text = levelSlider.value.ToString();
        GetComponent<Instantiate>().SetNumLevels((int)levelSlider.value);
        numLevels = (int)levelSlider.value;
    }

    public void OnToggle(GameObject toggle)
    {
        if(sfx.isOn)
            GameObject.Find("EventSystem").GetComponent<AudioSource>().Play();

        switch(toggle.name)
        {
            case "Music":
                {
                    if (music.isOn)
                        musicMute = false;
                    else
                        musicMute = true;
                    break;
                }
            case "SFX":
                {
                    if (sfx.isOn)
                        sfxMute = false;
                    else
                        sfxMute = true;
                    break;
                }
        }
    }


    public bool isTitleScreenActive()
    { return activeTitle; }

    public bool isMusicMute()
    { return musicMute; }

    public bool isSFXMute()
    { return sfxMute; }
}
