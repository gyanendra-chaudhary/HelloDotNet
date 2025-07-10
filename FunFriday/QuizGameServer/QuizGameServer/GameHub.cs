using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace QuizGameServer
{
    public class GameHub : Hub
    {
        private static ConcurrentDictionary<string, GameRoom> _rooms = new();

        public async Task CreateRoom(string username, string gameType, int gameTime)
        {
            var roomId = GenerateRoomId();
            var room = new GameRoom
            {
                Id = roomId,
                Host = Context.ConnectionId,
                GameType = gameType,
                GameTime = gameTime,
                Players = new List<Player>
                {
                    new Player
                    {
                        Id = Context.ConnectionId,
                        Name = username,
                        Score = 0,
                        CurrentQuestionIndex = 0
                    }
                },
                Questions = new List<Question>(),
                GameStarted = false,
                GameEnded = false
            };

            _rooms.TryAdd(roomId, room);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Caller.SendAsync("RoomCreated", room);
        }

        public async Task JoinRoom(string roomId, string username)
        {
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                await Clients.Caller.SendAsync("Error", "Room not found");
                return;
            }

            if (room.GameStarted)
            {
                await Clients.Caller.SendAsync("Error", "Game already started");
                return;
            }

            if (room.Players.Any(p => p.Name.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                await Clients.Caller.SendAsync("Error", "Username already taken");
                return;
            }

            var player = new Player
            {
                Id = Context.ConnectionId,
                Name = username,
                Score = 0,
                CurrentQuestionIndex = 0
            };

            room.Players.Add(player);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId).SendAsync("PlayerJoined", player);
            await Clients.Caller.SendAsync("RoomJoined", room);
        }

        public async Task StartGame(string roomId, List<Question> questions)
        {
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                await Clients.Caller.SendAsync("Error", "Room not found");
                return;
            }

            if (room.Host != Context.ConnectionId)
            {
                await Clients.Caller.SendAsync("Error", "Only host can start the game");
                return;
            }

            if (room.Players.Count < 1)
            {
                await Clients.Caller.SendAsync("Error", "Need at least 1 player to start");
                return;
            }

            room.GameStarted = true;
            room.Questions = questions;
            room.StartTime = DateTime.UtcNow;

            await Clients.Group(roomId).SendAsync("GameStarted", questions);
        }

        public async Task SubmitAnswer(string roomId, int questionIndex, string answer)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return;
            if (!room.GameStarted || room.GameEnded) return;

            var player = room.Players.FirstOrDefault(p => p.Id == Context.ConnectionId);
            if (player == null) return;

            if (questionIndex != player.CurrentQuestionIndex || questionIndex >= room.Questions.Count) return;

            if (player.Answers?.Any(a => a.QuestionIndex == questionIndex) == true) return;

            var question = room.Questions[questionIndex];
            var isCorrect = IsAnswerCorrect(question, answer);

            if (isCorrect) player.Score += 10;

            player.Answers ??= new List<PlayerAnswer>();
            player.Answers.Add(new PlayerAnswer
            {
                QuestionIndex = questionIndex,
                Answer = answer,
                IsCorrect = isCorrect,
                SubmittedAt = DateTime.UtcNow
            });

            player.CurrentQuestionIndex++;

            await Clients.Caller.SendAsync("AnswerSubmitted", player.Id, questionIndex, answer, isCorrect);
            await Clients.Caller.SendAsync("ScoreUpdated", player.Id, player.Score);
            await Clients.Caller.SendAsync("NextQuestion", player.CurrentQuestionIndex);

            if (player.CurrentQuestionIndex >= room.Questions.Count)
            {
                await Clients.Caller.SendAsync("QuizCompleted", player.Id);
            }
        }

        public async Task SkipQuestion(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return;

            var player = room.Players.FirstOrDefault(p => p.Id == Context.ConnectionId);
            if (player == null) return;

            int currentIndex = player.CurrentQuestionIndex;

            if (currentIndex >= room.Questions.Count) return;

            player.Answers ??= new List<PlayerAnswer>();
            player.Answers.Add(new PlayerAnswer
            {
                QuestionIndex = currentIndex,
                Answer = "Skipped",
                IsCorrect = false,
                SubmittedAt = DateTime.UtcNow
            });

            player.CurrentQuestionIndex++;

            await Clients.Caller.SendAsync("QuestionSkipped", currentIndex);
            await Clients.Caller.SendAsync("NextQuestion", player.CurrentQuestionIndex);

            if (player.CurrentQuestionIndex >= room.Questions.Count)
            {
                await Clients.Caller.SendAsync("QuizCompleted", player.Id);
            }
        }

        private async Task EndGame(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var room)) return;
            if (room.GameEnded) return;

            room.GameEnded = true;

            var results = room.Players
                .OrderByDescending(p => p.Score)
                .ThenBy(p => p.Name)
                .Select((p, index) => new PlayerResult
                {
                    Id = p.Id,
                    Name = p.Name,
                    Score = p.Score,
                    Rank = index + 1,
                    Answers = p.Answers ?? new List<PlayerAnswer>()
                }).ToList();

            await Clients.Group(roomId).SendAsync("GameEnded", results);

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(5));
                _rooms.TryRemove(roomId, out _);
            });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var room in _rooms.Values)
            {
                var player = room.Players.FirstOrDefault(p => p.Id == Context.ConnectionId);
                if (player != null)
                {
                    room.Players.Remove(player);
                    await Clients.Group(room.Id).SendAsync("PlayerLeft", Context.ConnectionId);

                    if (room.Host == Context.ConnectionId && !room.GameStarted && room.Players.Any())
                    {
                        room.Host = room.Players.First().Id;
                        await Clients.Group(room.Id).SendAsync("HostChanged", room.Host);
                    }

                    if (!room.Players.Any())
                    {
                        _rooms.TryRemove(room.Id, out _);
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        private static string GenerateRoomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool IsAnswerCorrect(Question question, string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer) || string.IsNullOrWhiteSpace(question.CorrectAnswer))
                return false;

            return question.CorrectAnswer.Trim().Equals(userAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public class GameRoom
    {
        public string Id { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string GameType { get; set; } = string.Empty;
        public int GameTime { get; set; }
        public bool GameStarted { get; set; }
        public bool GameEnded { get; set; }
        public List<Player> Players { get; set; } = new();
        public List<Question> Questions { get; set; } = new();
        public DateTime StartTime { get; set; }
    }

    public class Player
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public int CurrentQuestionIndex { get; set; } = 0;
        public List<PlayerAnswer>? Answers { get; set; }
    }

    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string>? Options { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class PlayerAnswer
    {
        public int QuestionIndex { get; set; }
        public string Answer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class PlayerResult
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Rank { get; set; }
        public List<PlayerAnswer> Answers { get; set; } = new();
    }
}
