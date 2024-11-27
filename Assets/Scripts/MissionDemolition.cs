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
    static private MissionDemolition S; // ������� ������-��������
    [Header("Set in Inspector")]
    public TextMeshProUGUI uitLevel;
    public TextMeshProUGUI uitShots;
    public TextMeshProUGUI uitButton;
    //� UIButton_View
    public Vector3 castlePos;
    public GameObject[] castles;
    // ������ �� ������ UIText_Level
    // ������ �� ������ UIText_Shots
    // ������ �� �������� ������ Text
    // �������������� �����
    // ������ ������
    [Header("Set Dynamically")]
    public int level; // ������� �������
    public int levelMax; // ���������� �������
    public int shotsTaken;
    public GameObject castle; // ������� �����
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // ����� FollowCam


    void Start()
    {
        S = this; // ���������� ������-��������
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        // ���������� ������� �����, ���� �� ����������
        if (castle != null)
        {
            Destroy(castle);
        }
        // ���������� ������� �������, ���� ��� ����������
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        // ������� ����� �����
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        // �������������� ������ � ��������� �������
        SwitchView("Show Both");
        ProjectileLine.S.Clear();
        // �������� ����
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // �������� ������ � ��������� ��
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        // ��������� ���������� ������
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // �������� �����; ����� ���������� �������� ���������� ������
            mode = GameMode.levelEnd;
            // ��������� �������
            SwitchView("Show Both");
            // ������ ����� ������� ����� 2 �������
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
    // ����������� �����, ����������� �� ������ ���� ��������� shotsTaken
    public static void ShotFired()
    { // d
        S.shotsTaken++;
    }
}

