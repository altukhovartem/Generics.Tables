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


        new AutoRun().Execute(args);
    }
}
