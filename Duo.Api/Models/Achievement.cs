using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Api.Models
{
    public class Achievement
    {
        // Achievement rarity constants
        public const string RARITY_COMMON = "Common";
        public const string RARITY_UNCOMMON = "Uncommon";
        public const string RARITY_RARE = "Rare";
        public const string RARITY_EPIC = "Epic";
        public const string RARITY_LEGENDARY = "Legendary";
        public const string RARITY_MYTHIC = "Mythic";

        // Date format constant
        private const string DATE_FORMAT = "MM/dd/yyyy";

        // Achievement properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RarityLevel { get; set; } = string.Empty;
        public DateTime AchievementUnlockDate { get; set; }

        public string FormattedUnlockDate => AchievementUnlockDate.ToString(DATE_FORMAT);
    }
}
