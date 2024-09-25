using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiplayerConsole.Multiplayer;
using MultiplayerConsole.Multiplayer.Messages;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _otherPlayerPrefab;
    private MultiplayerConnection _connection;
    private List<PlayerEntity> _players = new List<PlayerEntity>();
    private GameObject _localPlayer;

    private async void Start()
    {
        _localPlayer = Instantiate(_playerPrefab);
        _connection = new MultiplayerConnection();
        await _connection.Connect("127.0.0.1", 8080);

        _connection.OnWorldStateReceived += HandleWorldState;
        _connection.OnChatMessageReceived += HandleChatMessage;
        _connection.OnPlayerEventReceived += HandlePlayerEvent;

        StartCoroutine(SendPositionPeriodically());
    }

    void Update()
    {
    }

    private void OnDisable()
    {
        _connection?.Dispose();
    }

    private async void HandleWorldState(SyncWorldState worldState)
    {
        var syncPlayerIds = worldState.Players.Select(player => player.Id);
        var playerEntityIds = _players.Select(player => player.Id);

        var newPlayerIds = syncPlayerIds.Except(playerEntityIds);

        foreach (var newPlayerId in newPlayerIds)
        {
            var syncPlayer = worldState.Players.First(player => player.Id == newPlayerId);
            await MainThreadDispatcher.RunOnMainThreadAsync(() =>
            {
                var createdGameObject = Instantiate(_otherPlayerPrefab);
                _players.Add(new PlayerEntity(createdGameObject, newPlayerId, new UnityEngine.Vector3 { x = syncPlayer.Position.X, y = syncPlayer.Position.Y, z = syncPlayer.Position.Z }));
                Debug.Log($"Добавлен игрок {newPlayerId}");
            });
        }

        var deletedPlayerIds = playerEntityIds.Except(syncPlayerIds);

        foreach (var deletedPlayerId in deletedPlayerIds)
        {
            var player = _players.First(player => player.Id == deletedPlayerId);
            MainThreadDispatcher.RunOnMainThread(() =>
            {
                Destroy(player.GameObject);
            });
            _players.Remove(player);
            Debug.Log($"Удалён игрок {player.Id}");
        }

        var existingPlayerIds = syncPlayerIds.Intersect(playerEntityIds);

        foreach (var existingPlayerId in existingPlayerIds)
        {
            var syncPlayer = worldState.Players.First(player => player.Id == existingPlayerId);
            var player = _players.First(player => player.Id == existingPlayerId);
            player.Position = new UnityEngine.Vector3
            {
                x = syncPlayer.Position.X,
                y = syncPlayer.Position.Y,
                z = syncPlayer.Position.Z
            };
            MainThreadDispatcher.RunOnMainThread(() =>
            {
                player.Player.SetPosition(player.Position.x, player.Position.y, player.Position.z);
            });
        }
    }

    private void HandleChatMessage(ChatMessage chatMessage)
    {
        Console.WriteLine($"Chat Message from {chatMessage.Id}: {chatMessage.Message}");
    }

    private void HandlePlayerEvent(PlayerEvent playerEvent)
    {
        Console.WriteLine($"Player {playerEvent.Id} has {playerEvent.Event}");
    }

    private IEnumerator SendPositionPeriodically()
    {
        while (true)
        {
            yield return StartCoroutine(WaitForTask(_connection.SendPositionAsync(new UpdatedPlayerState
            {
                Position = new MultiplayerConsole.Multiplayer.Vector3
                {
                    X = _localPlayer.transform.position.x,
                    Y = _localPlayer.transform.position.y,
                    Z = _localPlayer.transform.position.z
                }
            })));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private static IEnumerator WaitForTask(Task task)
    {
        // Ожидаем завершение задачи
        while (!task.IsCompleted)
        {
            yield return null;  // Ждем кадр
        }
        // Пропагируем исключения, если они произошли
        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }
}
