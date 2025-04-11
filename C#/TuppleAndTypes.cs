using System;
public class TuppleAndTypes
{
    public void IntializeAndPrintSimpleTupple()
    {
        var tpl = (Name: "Ram", Age: 28);
        Console.WriteLine($"{tpl.Name} is {tpl.Age} Years Old");
        tpl.Age = 30;
        Console.WriteLine($"Now {tpl.Name} is {tpl.Age} Years Old");

        var tpl1 = (Name: "", Age: 0);
        tpl1 = tpl;
        Console.WriteLine(tpl1);

        (int, string) tpl2 = (1, "Paro");

        Console.WriteLine($"The first person is {tpl2.Item2} with id {tpl2.Item1}");
    }

    public record Point(int X, int Y);

    public record Point1(int X, int Y)
    {
        public double Slope() => (double)Y / (double)X;
    }

    public void HelloRecord()
    {
        System.Console.WriteLine("👉 Here we are exploring about record.");
        Point pt = new Point(1, 2);
        var pt1 = pt with { Y = 4 };
        Console.WriteLine($"The two points are {pt} and {pt1}");

        System.Console.WriteLine("👉 Lets explore record with behavior here.");
        var pt2 = new Point1(2, 10);
        double slope = pt2.Slope();
        Console.WriteLine(slope);



    }
}