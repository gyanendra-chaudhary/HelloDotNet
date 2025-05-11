using System.Runtime.InteropServices;
using C_.Topics;


// #region Tupple & Types
// TuppleAndTypes tuppleAndTypes = new TuppleAndTypes();
// tuppleAndTypes.IntializeAndPrintSimpleTupple();
// tuppleAndTypes.HelloRecord();
// #endregion


// # region Daily Challenges
//     StringWordReverse stringWordReverse =  new StringWordReverse();

//     string result = stringWordReverse.ReverseTheStringWords("Hello World");
//     Console.WriteLine(result);


// #endregion


// ZigzagConversion zigzagConversion = new ZigzagConversion();
// var convertedData = zigzagConversion.Convert("PAYPALISHIRING",3);
// System.Console.WriteLine(convertedData);




//StringToInteger stringToInteger = new StringToInteger();

//var result = stringToInteger.MyAtoi("42");
//System.Console.WriteLine($"Result: {result}");


//var sandip = new Person("Sandip", 25);
//var gandu = new Person("Sandip", 25);
////gandu.Age = 26;
//gandu = gandu with { Age = 26 };
//Console.WriteLine(sandip.ToString() == gandu.ToString());


//var sandipAgain = new Man("Sandip", 25);
//var hero = new Man("Sandip", 25);

//Console.WriteLine(sandipAgain == hero);



//Likes likes = new Likes();

//Console.WriteLine(likes.GetName("Hello World"));


//var sandipGandu = new
//{
//    Name = "Sandip",
//    Gender = "G"
//};
//var sarkarGandu = sandipGandu with { Name = "G Sandip again" };
//Console.WriteLine(sarkarGandu.Name);


namespace Hello;
class Apple(decimal Price, string Name)
{

    private decimal price;
    public decimal Price { get { return price; } set { price = price + 1000; } }
    public string Name { get; set; }
};

class Hello
{
    public static void Main(string[] args)
    {

        var iphone16 = new Apple(400, "Iphone16");
        Console.WriteLine($"Introducing new {iphone16.Name} with price {iphone16.Price}");
    }

}



