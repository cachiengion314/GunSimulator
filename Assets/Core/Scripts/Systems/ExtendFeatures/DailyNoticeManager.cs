using UnityEngine;
using HoangNam;

public class DailyNoticeManager : MonoBehaviour
{
    public static DailyNoticeManager Instance { get; private set; }
    public bool _isDayLySginIn = true;
    public bool _isDayLyTask = false;
    [SerializeField] GameObject _objNoticeDailySignIn;
    [SerializeField] GameObject _objNoticeDailyTask;
    [Header("---DailyTask---")]
    [SerializeField] GameObject _objDailyTask;
    [SerializeField] GameObject _objDailySign;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_LEVELINDEX, 0) >= 8)
        {
            _objDailyTask.SetActive(true);
        }
        else
        {
            _objDailyTask.SetActive(false);
        }

        if (PlayerPrefs.GetInt(KeyStr.KEY_CURRENT_LEVELINDEX, 0) >= 3)
        {
            _objDailySign.SetActive(true);
        }
        else
        {
            _objDailySign.SetActive(false);
        }
        _objNoticeDailySignIn.gameObject.SetActive(false);
        _objNoticeDailyTask.gameObject.SetActive(false);

    }
   
    void Update()
    {
        if (_isDayLySginIn == true)
        {
            _objNoticeDailySignIn.gameObject.SetActive(true);
        }
        else
        {
            _objNoticeDailySignIn.gameObject.SetActive(false);
        }
        if (_isDayLyTask == true)
        {
            _objNoticeDailyTask.gameObject.SetActive(true);
        }
        else
        {
            _objNoticeDailyTask.gameObject.SetActive(false);
        }
    }


}
