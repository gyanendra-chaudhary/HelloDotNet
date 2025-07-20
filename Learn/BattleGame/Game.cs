namespace BattleGame
{
    public class Game
    {
        private Character _player1;
        private Character _player2;
        private List<BattleLog> _battleLogList;
        public Game()
        {
            _battleLogList = new List<BattleLog>();
        }

        public void Start()
        {
            var warriorTemplate = new CharacterTemplate("Warrior", 100, 15);
            var mageTemplate = new CharacterTemplate("Mage", 80, 20);

            _player1 = new Character(warriorTemplate, new Point(0, 0));
            _player2 = new Character(mageTemplate, new Point(0, 0));

            Console.WriteLine("Battle Begins!");
            RunBattle();
        }
        private void RunBattle()
        {
            int turn = 0;
            while (_player1.IsAlive() && _player2.IsAlive())
            {
                var attacker = turn %2 ==0? _player1 : _player2;
                var defender = turn %2 ==0 ? _player2 : _player1;

                Console.WriteLine($"{attacker.Name} attacks {defender.Name}");

                defender.TakeDamage(attacker.AttackPower);

                _battleLogList.Add(new BattleLog(attacker.Name,
                defender.Name,
                attacker.AttackPower,
                defender.CurrentHealth));

                turn++;
            }

            var winner = _player1.IsAlive()?_player1.Name: _player2.Name;
            Console.WriteLine($"\n🎉 {winner} wins!");
            Console.WriteLine("\nBattle Log:");

            foreach(var log in _battleLogList)
            {
                Console.WriteLine($"{log.Attacker} hit {log.Defender} for {log.Damage} damage. {log.Defender} has {log.RemainingHealth} HP left.");
            }
        }
    }


}
