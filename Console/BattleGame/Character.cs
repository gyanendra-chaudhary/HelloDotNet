namespace BattleGame
{
    public class Character
    {
        public string Name { get; set; }
        public int CurrentHealth { get; private set; }
        public int AttackPower { get; private set; }
        public Point Position { get; set; }
        public Character(CharacterTemplate template, Point startPos)
        {
            Name = template.Name;
            CurrentHealth = template.MaxHealth;
            AttackPower = template.AttackPower;
            Position = startPos;
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0) CurrentHealth = 0;
        }
        public bool IsAlive() => CurrentHealth > 0;
    }
}
