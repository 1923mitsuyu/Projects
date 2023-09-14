using System.Text.Json.Serialization;
namespace Game;
public class GamesModel
{
    [JsonPropertyName("Games")]
    public GameModel[] Games { get; set; }
    public class BoardModel
    {
        [JsonPropertyName("Row")]
        public int Rows { get; set; }
        [JsonPropertyName("Columns")]
        public int Columns;
        [JsonPropertyName("Field")]
        public char[][] Field;
    }
    public class GameModel
    {
        [JsonPropertyName("Board")]
        public BoardModel GameBoard { get; set; }
        [JsonPropertyName("GameId")]
        public int GameId { get; set; }  = 1;
        [JsonPropertyName("GameName")]
        public string GameName { get; set; }  = "SOS";
        [JsonPropertyName("GameMode")]
        public ModeModel GameMode { get; set; }
        [JsonPropertyName("Players")]
        public PlayerModel[] Players { get; set; }
    }
    public class ModeModel
    {
        [JsonPropertyName("GameType")]
        public int GameType { get; set; }
        [JsonPropertyName("GameMode")]
        public int GameMode { get; set; }
    }
    public class PlayerModel
    {
        [JsonPropertyName("PlayerName")]
        public string PlayerName { get; set; }
        [JsonPropertyName("Turn")]
        public bool Turn { get; set; }
        [JsonPropertyName("PlayerId")]
        public int PlayerId { get; set; }
        [JsonPropertyName("GameId")]
        public int GameId { get; set; }
        [JsonPropertyName("PlayerMoves")]
        public PlayerMovesModel[] PlayerMoves { get; set; }
        [JsonPropertyName("IsHuman")]
        public bool IsHuman { get; set; }
    }
    public class PlayerMovesModel
    {
        [JsonPropertyName("MoveType")]
        public int MoveType { get; set; }
        [JsonPropertyName("Row")]
        public int Row { get; set; }
        [JsonPropertyName("Column")]
        public int Column { get; set; }
    }
}