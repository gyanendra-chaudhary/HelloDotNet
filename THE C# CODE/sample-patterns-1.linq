<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>




for(int i =1; i<=5; i++)
{
	for(int j=1; j<=5;j++)
	{
		Console.Write("*     ");
	}
	Console.WriteLine();
}		

"-----------------------".Dump();

for(int i =1; i<=5; i++)
{
	for(int j =1;j<=5;j++)
	{
		Console.Write(i + "    ");
	}
	Console.WriteLine();
}
"-----------------------".Dump();

for(int i =1;i<=5;i++)
{
	for(int j=1;j<=5;j++)
	{
		Console.Write(j +"    ");
	}
	Console.WriteLine();
}
"-------------------------".Dump();

for(int i =1; i<=5;i++)
{
	for(int j =5; j>=1; j--)
	{
		Console.Write(j + "     ");
	}
	Console.WriteLine();
}

"-------------------------".Dump();

for(int i =5; i>=1;i--)
{
	for(int j =1; j<=5; j++)
	{
		Console.Write(i + "     ");
	}
	Console.WriteLine();
}


