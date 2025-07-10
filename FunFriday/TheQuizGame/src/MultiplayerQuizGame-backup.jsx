import React, { useState, useEffect, useRef } from "react";
import {
  Users,
  Clock,
  Trophy,
  Play,
  Upload,
  Link,
  Download,
  Star,
  Zap,
  Plus,
  Hash,
} from "lucide-react";

// Sample question data embedded in code
const questionData = {
  normal: [
    { question: "What is the capital of France?", answer: "Paris" },
    { question: "What is 2 + 2?", answer: "4" },
    {
      question: "What is the largest planet in our solar system?",
      answer: "Jupiter",
    },
    { question: "Who wrote Romeo and Juliet?", answer: "Shakespeare" },
    { question: "What is the chemical symbol for gold?", answer: "Au" },
    { question: "In which year did World War II end?", answer: "1945" },
    {
      question: "What is the smallest country in the world?",
      answer: "Vatican City",
    },
    { question: "What is the fastest land animal?", answer: "Cheetah" },
    { question: "How many continents are there?", answer: "7" },
    { question: "What is the hardest natural substance?", answer: "Diamond" },
  ],
  multipleChoice: [
    {
      question: "What is the capital of France?",
      options: ["London", "Berlin", "Paris", "Madrid"],
      answer: "Paris",
    },
    {
      question: "What is 2 + 2?",
      options: ["3", "4", "5", "6"],
      answer: "4",
    },
    {
      question: "What is the largest planet in our solar system?",
      options: ["Earth", "Mars", "Jupiter", "Saturn"],
      answer: "Jupiter",
    },
    {
      question: "Who wrote Romeo and Juliet?",
      options: ["Charles Dickens", "Shakespeare", "Mark Twain", "Jane Austen"],
      answer: "Shakespeare",
    },
    {
      question: "What is the chemical symbol for gold?",
      options: ["Go", "Gd", "Au", "Ag"],
      answer: "Au",
    },
    {
      question: "In which year did World War II end?",
      options: ["1944", "1945", "1946", "1947"],
      answer: "1945",
    },
    {
      question: "What is the smallest country in the world?",
      options: ["Monaco", "San Marino", "Vatican City", "Liechtenstein"],
      answer: "Vatican City",
    },
    {
      question: "What is the fastest land animal?",
      options: ["Lion", "Cheetah", "Leopard", "Tiger"],
      answer: "Cheetah",
    },
    {
      question: "How many continents are there?",
      options: ["5", "6", "7", "8"],
      answer: "7",
    },
    {
      question: "What is the hardest natural substance?",
      options: ["Gold", "Iron", "Diamond", "Silver"],
      answer: "Diamond",
    },
  ],
};

const MultiplayerQuizGame = () => {
  const [gameState, setGameState] = useState("home"); // home, setup, lobby, playing, finished
  const [gameType, setGameType] = useState("normal"); // normal, multipleChoice
  const [questions, setQuestions] = useState([]);
  const [gameTime, setGameTime] = useState(5);
  const [rooms, setRooms] = useState({});
  const [currentRoom, setCurrentRoom] = useState("");
  const [currentUser, setCurrentUser] = useState("");
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [timeLeft, setTimeLeft] = useState(0);
  const [userAnswer, setUserAnswer] = useState("");
  const [selectedOption, setSelectedOption] = useState("");
  const [showResults, setShowResults] = useState(false);
  const [gameResults, setGameResults] = useState([]);
  const [joinRoomId, setJoinRoomId] = useState("");

  const timerRef = useRef(null);
  const gameTimerRef = useRef(null);

  // Get current room data
  const getCurrentRoom = () =>
    rooms[currentRoom] || { players: [], host: "", gameStarted: false };

  // Generate random room ID
  const generateRoomId = () => {
    return Math.random().toString(36).substring(2, 8).toUpperCase();
  };

  // Create new room
  const createRoom = () => {
    const roomId = generateRoomId();
    const newRoom = {
      id: roomId,
      host: currentUser,
      gameType: gameType,
      questions: questionData[gameType],
      gameTime: gameTime,
      players: [],
      gameStarted: false,
      currentQuestion: 0,
      timeLeft: 0,
    };

    setRooms((prev) => ({ ...prev, [roomId]: newRoom }));
    setCurrentRoom(roomId);
    setQuestions(questionData[gameType]);
    setGameState("lobby");
  };

  // Join existing room
  const joinRoom = (roomId, username) => {
    if (!roomId.trim() || !username.trim()) {
      alert("Please enter both Room ID and Username");
      return;
    }

    const room = rooms[roomId];
    if (!room) {
      alert("Room not found! Please check the Room ID.");
      return;
    }

    if (
      room.players.some((p) => p.name.toLowerCase() === username.toLowerCase())
    ) {
      alert(
        "Username already exists in this room! Please choose a different name."
      );
      return;
    }

    if (room.gameStarted) {
      alert("Game has already started in this room!");
      return;
    }

    const newPlayer = {
      id: Date.now(),
      name: username,
      score: 0,
      answers: [],
      joinedAt: new Date(),
    };

    setRooms((prev) => ({
      ...prev,
      [roomId]: {
        ...prev[roomId],
        players: [...prev[roomId].players, newPlayer],
      },
    }));

    setCurrentRoom(roomId);
    setCurrentUser(username);
    setQuestions(room.questions);
    setGameType(room.gameType);
    setGameState("lobby");
  };

  // Start game (only host can start)
  const startGame = () => {
    const room = getCurrentRoom();
    if (currentUser !== room.host) {
      alert("Only the host can start the game!");
      return;
    }

    if (room.players.length < 1) {
      alert("At least one player must join!");
      return;
    }

    setRooms((prev) => ({
      ...prev,
      [currentRoom]: {
        ...prev[currentRoom],
        gameStarted: true,
        timeLeft: gameTime * 60,
      },
    }));

    setGameState("playing");
    setTimeLeft(gameTime * 60);
    setCurrentQuestion(0);

    // Start game timer
    gameTimerRef.current = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev <= 1) {
          endGame();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
  };

  // Submit answer
  const submitAnswer = () => {
    const answerToSubmit =
      gameType === "multipleChoice" ? selectedOption : userAnswer;
    if (!answerToSubmit.trim()) return;

    const question = questions[currentQuestion];
    const isCorrect =
      answerToSubmit.toLowerCase().trim() ===
      question.answer.toLowerCase().trim();
    const points = isCorrect ? 10 : 0;

    // Update room data
    setRooms((prev) => ({
      ...prev,
      [currentRoom]: {
        ...prev[currentRoom],
        players: prev[currentRoom].players.map((player) =>
          player.name === currentUser
            ? {
                ...player,
                score: player.score + points,
                answers: [
                  ...player.answers,
                  {
                    question: currentQuestion,
                    answer: answerToSubmit,
                    correct: isCorrect,
                  },
                ],
              }
            : player
        ),
      },
    }));

    setUserAnswer("");
    setSelectedOption("");

    // Move to next question
    if (currentQuestion < questions.length - 1) {
      setCurrentQuestion((prev) => prev + 1);
    } else {
      setCurrentQuestion(0);
    }
  };

  // End game
  const endGame = () => {
    setGameState("finished");
    clearInterval(gameTimerRef.current);

    const room = getCurrentRoom();
    const results = room.players
      .sort((a, b) => b.score - a.score)
      .map((player, index) => ({
        ...player,
        rank: index + 1,
      }));

    setGameResults(results);
    setShowResults(true);
  };

  // Download results
  const downloadResults = () => {
    const dataStr = JSON.stringify(
      {
        roomId: currentRoom,
        gameType: gameType,
        results: gameResults,
        timestamp: new Date().toISOString(),
      },
      null,
      2
    );
    const dataUri =
      "data:application/json;charset=utf-8," + encodeURIComponent(dataStr);

    const exportFileDefaultName = `quiz_results_${currentRoom}_${
      new Date().toISOString().split("T")[0]
    }.json`;

    const linkElement = document.createElement("a");
    linkElement.setAttribute("href", dataUri);
    linkElement.setAttribute("download", exportFileDefaultName);
    linkElement.click();
  };

  // Format time
  const formatTime = (seconds) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins}:${secs.toString().padStart(2, "0")}`;
  };

  // Reset game
  const resetGame = () => {
    setGameState("home");
    setCurrentRoom("");
    setCurrentUser("");
    setCurrentQuestion(0);
    setTimeLeft(0);
    setUserAnswer("");
    setSelectedOption("");
    setShowResults(false);
    setGameResults([]);
    setJoinRoomId("");
    clearInterval(gameTimerRef.current);
  };

  // Home screen
  if (gameState === "home") {
    return (
      <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 p-4">
        <div className="max-w-4xl mx-auto">
          <div className="text-center mb-8">
            <div className="flex justify-center items-center gap-3 mb-4">
              <Zap className="w-10 h-10 text-yellow-400" />
              <h1 className="text-5xl font-bold text-white">QuizMaster Pro</h1>
              <Zap className="w-10 h-10 text-yellow-400" />
            </div>
            <p className="text-blue-200 text-xl">
              Multiplayer Quiz Game Experience
            </p>
          </div>

          <div className="grid md:grid-cols-2 gap-6">
            {/* Create Room */}
            <div className="bg-white/10 backdrop-blur-md rounded-2xl p-8 border border-white/20">
              <h2 className="text-2xl font-bold text-white mb-6 flex items-center gap-2">
                <Plus className="w-6 h-6" />
                Create New Room
              </h2>

              <div className="space-y-4">
                <div>
                  <label className="block text-white font-medium mb-2">
                    Your Username
                  </label>
                  <input
                    type="text"
                    value={currentUser}
                    onChange={(e) => setCurrentUser(e.target.value)}
                    placeholder="Enter your username"
                    className="w-full px-4 py-3 bg-white/20 border border-white/30 rounded-lg text-white placeholder-white/70 focus:outline-none focus:ring-2 focus:ring-blue-400"
                  />
                </div>

                <div>
                  <label className="block text-white font-medium mb-2">
                    Game Type
                  </label>
                  <div className="grid grid-cols-2 gap-2">
                    <button
                      onClick={() => setGameType("normal")}
                      className={`p-3 rounded-lg border-2 transition-colors ${
                        gameType === "normal"
                          ? "bg-blue-600 border-blue-400 text-white"
                          : "bg-white/10 border-white/30 text-white hover:bg-white/20"
                      }`}
                    >
                      Text Answer
                    </button>
                    <button
                      onClick={() => setGameType("multipleChoice")}
                      className={`p-3 rounded-lg border-2 transition-colors ${
                        gameType === "multipleChoice"
                          ? "bg-blue-600 border-blue-400 text-white"
                          : "bg-white/10 border-white/30 text-white hover:bg-white/20"
                      }`}
                    >
                      Multiple Choice
                    </button>
                  </div>
                </div>

                <div>
                  <label className="block text-white font-medium mb-2">
                    Game Duration (minutes)
                  </label>
                  <input
                    type="number"
                    min="1"
                    max="60"
                    value={gameTime}
                    onChange={(e) => setGameTime(parseInt(e.target.value) || 5)}
                    className="w-full px-4 py-3 bg-white/20 border border-white/30 rounded-lg text-white focus:outline-none focus:ring-2 focus:ring-blue-400"
                  />
                </div>

                <button
                  onClick={createRoom}
                  disabled={!currentUser.trim()}
                  className="w-full bg-green-600 hover:bg-green-700 disabled:bg-gray-500 text-white font-bold py-3 px-6 rounded-lg transition-colors flex items-center justify-center gap-2"
                >
                  <Plus className="w-5 h-5" />
                  Create Room
                </button>
              </div>
            </div>

            {/* Join Room */}
            <div className="bg-white/10 backdrop-blur-md rounded-2xl p-8 border border-white/20">
              <h2 className="text-2xl font-bold text-white mb-6 flex items-center gap-2">
                <Hash className="w-6 h-6" />
                Join Existing Room
              </h2>

              <div className="space-y-4">
                <div>
                  <label className="block text-white font-medium mb-2">
                    Room ID
                  </label>
                  <input
                    type="text"
                    value={joinRoomId}
                    onChange={(e) =>
                      setJoinRoomId(e.target.value.toUpperCase())
                    }
                    placeholder="Enter Room ID"
                    className="w-full px-4 py-3 bg-white/20 border border-white/30 rounded-lg text-white placeholder-white/70 focus:outline-none focus:ring-2 focus:ring-blue-400"
                  />
                </div>

                <div>
                  <label className="block text-white font-medium mb-2">
                    Your Username
                  </label>
                  <input
                    type="text"
                    value={currentUser}
                    onChange={(e) => setCurrentUser(e.target.value)}
                    placeholder="Enter your username"
                    className="w-full px-4 py-3 bg-white/20 border border-white/30 rounded-lg text-white placeholder-white/70 focus:outline-none focus:ring-2 focus:ring-blue-400"
                  />
                </div>

                <button
                  onClick={() => joinRoom(joinRoomId, currentUser)}
                  disabled={!joinRoomId.trim() || !currentUser.trim()}
                  className="w-full bg-blue-600 hover:bg-blue-700 disabled:bg-gray-500 text-white font-bold py-3 px-6 rounded-lg transition-colors flex items-center justify-center gap-2"
                >
                  <Users className="w-5 h-5" />
                  Join Room
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Lobby screen
  if (gameState === "lobby") {
    const room = getCurrentRoom();

    return (
      <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 p-4">
        <div className="max-w-4xl mx-auto">
          <div className="text-center mb-8">
            <h1 className="text-4xl font-bold text-white mb-2">Game Lobby</h1>
            <div className="flex items-center justify-center gap-4 text-blue-200">
              <div className="flex items-center gap-2">
                <Link className="w-5 h-5" />
                <span className="font-mono text-xl">
                  Room ID: {currentRoom}
                </span>
              </div>
              <div className="flex items-center gap-2">
                <Star className="w-5 h-5" />
                <span className="capitalize">{gameType} Mode</span>
              </div>
            </div>
          </div>

          <div className="grid md:grid-cols-2 gap-6">
            <div className="bg-white/10 backdrop-blur-md rounded-2xl p-6 border border-white/20">
              <h2 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
                <Users className="w-5 h-5" />
                Players ({room.players.length})
              </h2>

              <div className="space-y-3 max-h-60 overflow-y-auto">
                {room.players.map((player, index) => (
                  <div
                    key={player.id}
                    className="flex items-center gap-3 bg-white/10 rounded-lg p-3"
                  >
                    <div className="w-8 h-8 bg-gradient-to-r from-blue-500 to-purple-500 rounded-full flex items-center justify-center text-white font-bold">
                      {index + 1}
                    </div>
                    <span className="text-white font-medium">
                      {player.name}
                    </span>
                    {player.name === room.host && (
                      <span className="bg-yellow-500 text-black px-2 py-1 rounded text-xs font-bold">
                        HOST
                      </span>
                    )}
                  </div>
                ))}
              </div>
            </div>

            <div className="bg-white/10 backdrop-blur-md rounded-2xl p-6 border border-white/20">
              <h2 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
                <Trophy className="w-5 h-5" />
                Game Settings
              </h2>

              <div className="space-y-4 text-white">
                <div className="flex justify-between">
                  <span>Game Type:</span>
                  <span className="capitalize font-semibold">{gameType}</span>
                </div>
                <div className="flex justify-between">
                  <span>Duration:</span>
                  <span className="font-semibold">{gameTime} minutes</span>
                </div>
                <div className="flex justify-between">
                  <span>Questions:</span>
                  <span className="font-semibold">{questions.length}</span>
                </div>
                <div className="flex justify-between">
                  <span>Host:</span>
                  <span className="font-semibold">{room.host}</span>
                </div>
              </div>

              {currentUser === room.host ? (
                <button
                  onClick={startGame}
                  className="w-full mt-6 bg-green-600 hover:bg-green-700 text-white font-bold py-3 px-6 rounded-lg transition-colors flex items-center justify-center gap-2"
                >
                  <Play className="w-5 h-5" />
                  Start Game
                </button>
              ) : (
                <div className="mt-6 bg-yellow-500/20 border border-yellow-500/50 rounded-lg p-4">
                  <p className="text-yellow-200 text-center">
                    Waiting for host to start the game...
                  </p>
                </div>
              )}

              <button
                onClick={resetGame}
                className="w-full mt-3 bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg transition-colors"
              >
                Leave Room
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Playing screen
  if (gameState === "playing") {
    const question = questions[currentQuestion];
    const room = getCurrentRoom();

    return (
      <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 p-4">
        <div className="max-w-6xl mx-auto">
          <div className="flex justify-between items-center mb-6">
            <div>
              <h1 className="text-3xl font-bold text-white">QuizMaster Pro</h1>
              <p className="text-blue-200">Room: {currentRoom}</p>
            </div>
            <div className="flex items-center gap-4 text-white">
              <div className="flex items-center gap-2 bg-white/20 rounded-lg px-4 py-2">
                <Clock className="w-5 h-5" />
                <span className="font-mono text-xl">
                  {formatTime(timeLeft)}
                </span>
              </div>
            </div>
          </div>

          <div className="grid lg:grid-cols-3 gap-6">
            <div className="lg:col-span-2">
              <div className="bg-white/10 backdrop-blur-md rounded-2xl p-6 border border-white/20 mb-6">
                <div className="flex justify-between items-center mb-4">
                  <h2 className="text-xl font-bold text-white">
                    Question {currentQuestion + 1}
                  </h2>
                  <span className="text-blue-200">of {questions.length}</span>
                </div>

                <div className="bg-white/20 rounded-lg p-6 mb-4">
                  <p className="text-white text-lg font-medium">
                    {question?.question}
                  </p>
                </div>

                {gameType === "multipleChoice" ? (
                  <div className="space-y-3">
                    {question?.options?.map((option, index) => (
                      <button
                        key={index}
                        onClick={() => setSelectedOption(option)}
                        className={`w-full p-4 rounded-lg border-2 text-left transition-colors ${
                          selectedOption === option
                            ? "bg-blue-600 border-blue-400 text-white"
                            : "bg-white/10 border-white/30 text-white hover:bg-white/20"
                        }`}
                      >
                        <span className="font-bold mr-3">
                          {String.fromCharCode(65 + index)}.
                        </span>
                        {option}
                      </button>
                    ))}
                    <button
                      onClick={submitAnswer}
                      disabled={!selectedOption}
                      className="w-full bg-green-600 hover:bg-green-700 disabled:bg-gray-500 text-white font-bold py-3 px-6 rounded-lg transition-colors"
                    >
                      Submit Answer
                    </button>
                  </div>
                ) : (
                  <div className="flex gap-4">
                    <input
                      type="text"
                      value={userAnswer}
                      onChange={(e) => setUserAnswer(e.target.value)}
                      placeholder="Type your answer..."
                      className="flex-1 px-4 py-3 bg-white/20 border border-white/30 rounded-lg text-white placeholder-white/70 focus:outline-none focus:ring-2 focus:ring-blue-400"
                      onKeyPress={(e) => e.key === "Enter" && submitAnswer()}
                    />
                    <button
                      onClick={submitAnswer}
                      className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg transition-colors"
                    >
                      Submit
                    </button>
                  </div>
                )}
              </div>
            </div>

            <div className="bg-white/10 backdrop-blur-md rounded-2xl p-6 border border-white/20">
              <h2 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
                <Trophy className="w-5 h-5" />
                Live Scoreboard
              </h2>

              <div className="space-y-3">
                {room.players
                  .sort((a, b) => b.score - a.score)
                  .map((player, index) => (
                    <div
                      key={player.id}
                      className={`flex items-center justify-between p-3 rounded-lg ${
                        player.name === currentUser
                          ? "bg-blue-500/30 border border-blue-400"
                          : "bg-white/10"
                      }`}
                    >
                      <div className="flex items-center gap-3">
                        <div
                          className={`w-8 h-8 rounded-full flex items-center justify-center text-white font-bold ${
                            index === 0
                              ? "bg-yellow-500"
                              : index === 1
                              ? "bg-gray-400"
                              : index === 2
                              ? "bg-amber-600"
                              : "bg-gray-600"
                          }`}
                        >
                          {index + 1}
                        </div>
                        <span className="text-white font-medium">
                          {player.name}
                        </span>
                      </div>
                      <span className="text-white font-bold">
                        {player.score}
                      </span>
                    </div>
                  ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Results screen
  if (gameState === "finished") {
    return (
      <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 p-4">
        <div className="max-w-4xl mx-auto">
          <div className="text-center mb-8">
            <h1 className="text-4xl font-bold text-white mb-2">Game Over!</h1>
            <p className="text-blue-200">Room: {currentRoom} | Final Results</p>
          </div>

          <div className="bg-white/10 backdrop-blur-md rounded-2xl p-8 border border-white/20">
            <h2 className="text-2xl font-bold text-white mb-6 flex items-center gap-2">
              <Trophy className="w-6 h-6" />
              Final Scoreboard
            </h2>

            <div className="space-y-4 mb-6">
              {gameResults.map((player, index) => (
                <div
                  key={player.id}
                  className={`flex items-center justify-between p-4 rounded-lg ${
                    index === 0
                      ? "bg-yellow-500/20 border border-yellow-400"
                      : index === 1
                      ? "bg-gray-400/20 border border-gray-400"
                      : index === 2
                      ? "bg-amber-600/20 border border-amber-600"
                      : "bg-white/10"
                  }`}
                >
                  <div className="flex items-center gap-4">
                    <div
                      className={`w-12 h-12 rounded-full flex items-center justify-center text-white font-bold text-lg ${
                        index === 0
                          ? "bg-yellow-500"
                          : index === 1
                          ? "bg-gray-400"
                          : index === 2
                          ? "bg-amber-600"
                          : "bg-gray-600"
                      }`}
                    >
                      {index + 1}
                    </div>
                    <div>
                      <div className="text-white font-bold text-lg">
                        {player.name}
                      </div>
                      <div className="text-blue-200">
                        {player.answers.filter((a) => a.correct).length} correct
                        answers
                      </div>
                    </div>
                  </div>
                  <div className="text-white font-bold text-2xl">
                    {player.score}
                  </div>
                </div>
              ))}
            </div>

            <div className="flex gap-4">
              <button
                onClick={downloadResults}
                className="flex-1 bg-green-600 hover:bg-green-700 text-white font-bold py-3 px-6 rounded-lg transition-colors flex items-center justify-center gap-2"
              >
                <Download className="w-5 h-5" />
                Download Results
              </button>

              <button
                onClick={resetGame}
                className="flex-1 bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg transition-colors flex items-center justify-center gap-2"
              >
                <Play className="w-5 h-5" />
                New Game
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }
};

export default MultiplayerQuizGame;
