namespace C_.Topics
{
    record class Person(string Name, int Age);
    class Man(string name, int age)
    {
        public string Name { get; set; } = name;
        public int Age { get; set; } = age;
    }
    interface A
    {
        string GetName(string name);
    }
    interface B
    {
        string GetName(string name);
    }
    class Likes : A, B
    {
        public string GetName(string name)
        {
            return name;
        }
    };
}
