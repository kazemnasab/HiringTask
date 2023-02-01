// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using System.Collections;

var fileStream = File.OpenRead("cr.txt");
StreamReader reader = new StreamReader(fileStream);
List<Tuple<string, string, double>> tuples = new List<Tuple<string, string, double>>();
String line;
string[] temp;
double rate;

while ((line = reader.ReadLine()) != null)
{
    temp = line.Split(new char[] { ' ' });
    rate = double.Parse(temp[2]);
    tuples.Add(Tuple.Create(temp[0], temp[1], rate));
}

CurrencyConverter currencyConverter = new CurrencyConverter();
currencyConverter.UpdateConfiguration(tuples);

double cmp = currencyConverter.Convert("Z", "H", 100);
if(double.IsNaN(cmp))
    Console.WriteLine("Relation not exist");
    else
Console.WriteLine("Convert: {0}", cmp);

Console.ReadLine();
