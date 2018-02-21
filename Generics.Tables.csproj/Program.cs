using System.Collections.Generic;
using System.Linq;
using NUnitLite;
using Generics.Tables;
using System;

class Program
{
	static void Main(string[] args)
	{

        Table<int, string, double> table = new Table<int, string, double>();
        table.AddRow(1);
        table.AddColumn("2");

        table.Open[1, "2"] = 2;
        Console.WriteLine(table.Open[1, "2"]);
        Console.WriteLine(table.Rows.Count());
        Console.WriteLine(table.Columns.Count());

        //new AutoRun().Execute(args);
    }
}
