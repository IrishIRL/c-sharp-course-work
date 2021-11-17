using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameConfig
    {

        [Key] public int ConfigId { get; set; } = default!;
        public string ConfigName { get; set; } = default!;
        public string ConfigJson { get; set; } = default!;
        public DateTime ConfigBuildDate { get; set; }
    }
}