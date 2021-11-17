using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameSave
    {
        [Key] public int SavedGameId { get; set; } = default!;
        public string SavedGame { get; set; } = default!;
        public DateTime GameSaveDate { get; set; }
    }
}