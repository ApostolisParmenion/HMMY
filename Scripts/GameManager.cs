using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string PLAYER_ID_PREFIX = "Player ";
    public static GameObject[] playersObjects;
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    public static void RegisterPlayer(string _netId,Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netId;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        playersObjects = GameObject.FindGameObjectsWithTag("Player");
    }
    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
        playersObjects = GameObject.FindGameObjectsWithTag("Player");
    }

    public static Player GetPlayer(string _PlayerID)
    {
        return players[_PlayerID];
    }
}
