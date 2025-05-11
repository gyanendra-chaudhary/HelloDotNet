using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleGame
{
    public record BattleLog(string Attacker, string Defender, int Damage, int RemainingHealth);
}
