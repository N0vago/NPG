using Dan.Main;
using Dan.Models;
using UnityEngine;
using System;

public class SaveManager
{
    //TODO: Paste your key here: 
    private static string publicKey = "5d73f8fe179c18e73a425c22e27c531f71b9e85a0b13992e688b8486f2a2c4df";
    
    //Toggle to see logs: 
    private static bool logResults = true;

    public static void SaveLocally(string _userName, int _userScore)
    {
        PlayerPrefs.SetInt(_userName, _userScore);

        if (logResults)
        {
            Debug.Log($"Score {_userScore} SAVED locally for user {_userName}");
        }
    }

    public static void UploadSaveToCloud(string _userName, int _userScore)
    {
        if (logResults)
        {
            LeaderboardCreator.UploadNewEntry(publicKey, _userName, _userScore, _logSaveUploaded);
        }
        else
        {
            LeaderboardCreator.UploadNewEntry(publicKey, _userName, _userScore);
        }

        void _logSaveUploaded(bool _) => Debug.Log($"Score {_userScore} SAVED in cloud for user {_userName}");
    }

    public static int GetUserScore(string _userName)
    {
        int _userScore = PlayerPrefs.GetInt(_userName, 0);

        if (logResults)
        {
            Debug.Log($"Score {_userScore} was loaded from local save for user {_userName}");
        }

        return _userScore;
    }

    public static void GetUserScoreFromServer(string _userName, Action<int> _callback, Action<string> _callbackFail)
    {
        if (_userName == null) return;

        LeaderboardCreator.GetLeaderboard(publicKey, _handleGameSave, _callbackFail);

        void _handleGameSave(Entry[] _entries)
        {
            int _globalSavedScore = 0;

            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i].Username.Equals(_userName))
                {
                    _globalSavedScore = _entries[i].Score;

                    if (logResults)
                    {
                        Debug.Log(
                            $"Score {_globalSavedScore} was loaded from global save for user {_userName}");
                    }

                    _callback(_globalSavedScore);
                    return;
                }
            }

            _callbackFail?.Invoke("");
        }
    }

    public static void RemoveGlobalSave(string _userName, Action<bool> _callback)
    {
        LeaderboardCreator.DeleteEntry(publicKey, _callback);
    }
}