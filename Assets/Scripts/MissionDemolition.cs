using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro; // a
public enum GameMode
{ // b
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // скрытый объект-одиночка
    [Header("Set in Inspector")]
    public TextMeshProUGUI uitLevel;
    public TextMeshProUGUI uitShots;
    public TextMeshProUGUI uitButton;
    //в UIButton_View
    public Vector3 castlePos;
    public GameObject[] castles;
    // Ссылка на объект UIText_Level
    // Ссылка на объект UIText_Shots
    // Ссылка на дочерний объект Text
    // Местоположение замка
    // Массив замков
    [Header("Set Dynamically")]
    public int level; // Текущий уровень
    public int levelMax; // Количество уровней
    public int shotsTaken;
    public GameObject castle; // Текущий замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // Режим FollowCam


    void Start()
    {
        S = this; // Определить объект-одиночку
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        // Уничтожить прежний замок, если он существует
        if (castle != null)
        {
            Destroy(castle);
        }
        // Уничтожить прежние снаряды, если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        // Создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        // Переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();
        // Сбросить цель
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // Показать данные в элементах ПИ
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        // Проверить завершение уровня
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // Изменить режим; чтобы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            // Уменьшить масштаб
            SwitchView("Show Both");
            // Начать новый уровень через 2 секунды
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Sligshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }
    // Статический метод, позволяющий из любого кода увеличить shotsTaken
    public static void ShotFired()
    { // d
        S.shotsTaken++;
    }
}

