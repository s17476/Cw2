using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;

namespace Cw2
{
    class Program
    {
        // wartości domyślne
        private static readonly string inputDataFinal = "data.csv";
        private static readonly string outputDataFinal = "result.xml";
        private static readonly string outputFormatFinal = "xml";
        private static string inputData = inputDataFinal;
        private static string outputData = outputDataFinal;
        private static string outfutFormat = outputFormatFinal;
        private static IFileWriter writer = new MyXmlWriter();
        private static StreamReader inputFile;
        private static Hashtable outputTable = new Hashtable();
        private static StreamWriter logFile = new StreamWriter("log.txt");
        static void Main(string[] args)
        {
            // rozpoznanie argumentów i ewentualna korekta wartości domyślnych
            try
            {
                getArgs(args);
                
            }
            catch( ArgumentException ae)
            {
                handleException(ae);
            }
            try
            {
                inputFile = new StreamReader(inputData, System.Text.Encoding.UTF8);
            }
            catch(IOException e) 
            {
                handleException(new FileNotFoundException("Nie znaleziono pliku: " + inputData));
                try
                {
                    inputFile = new StreamReader(inputDataFinal, System.Text.Encoding.UTF8);
                }
                catch(FileNotFoundException exc)
                {
                    string msg = "Nie znaleziono pliku domyślnego: " + inputDataFinal +
                                        "\nDZIAŁANIE PROGRAMU PRZERWANE";
                    Console.WriteLine(msg);
                    logFile.WriteLine(msg);
                    logFile.Close();
                    return;
                }
                
            }
            
            // wczytywanie danych
            outputTable = ReadFile();
            inputFile.Close();
            Uczelnia uczelnia = new Uczelnia(new Studenci(outputTable));

            // konwersja i zapis do pliku
            try
            {
                writer.write(uczelnia, outputData);
            }
            catch (DirectoryNotFoundException e)
            {
                handleException(new DirectoryNotFoundException("Katalog nie istnieje: " + outputData));
                writer = new MyXmlWriter();
                writer.write(uczelnia, outputDataFinal);
            }
            catch (UnauthorizedAccessException e)
            {
                handleException(new UnauthorizedAccessException("Nie masz dostępu do tego katalogu: " + outputData));
                writer = new MyXmlWriter();
                writer.write(uczelnia, outputDataFinal);
            }
            logFile.Close();
            Console.WriteLine("Program wykonał się poprawnie");
        }

        /**
         * wyświetlanie danych błędu i raportowanie do log.txt
         */
        private static void handleException(Exception e)
        {
            logFile.WriteLine(e.Message);
            Console.WriteLine(e.Message);
            Console.WriteLine("Zostanie użyty parametr domyślny\n");
        }

        /**
         * rozpoznanie argumentów i ewentualna korekta wartości domyślnych, lub rzucenie wyjątku
         */
        private static void getArgs(string[] args) {
            string errorMsg = "Niepoprawny argument(y): ";
            Boolean argExc = false;
                foreach (string arg in args)
                {
                    string[] tmp = arg.Split(".");
                    if ((tmp.Length > 1) && (tmp[tmp.Length - 1].Equals("csv")))
                    {
                        Console.WriteLine("Wejście " + arg);
                        inputData = arg;
                    }
                    else if ((tmp.Length > 1) && (tmp[tmp.Length - 1].Equals("xml")))
                    {
                        Console.WriteLine("Wyjście " + arg);
                        outputData = arg;
                    }
                    else if ((tmp.Length > 1) && (tmp[tmp.Length - 1].Equals("json")))
                    {
                        Console.WriteLine("Wyjście " + arg);
                        outputData = arg;
                    }
                    else if (tmp[0].Equals("xml"))
                    {
                        Console.WriteLine("Format " + arg);
                        outfutFormat = arg;
                        writer = new MyXmlWriter();
                    }
                    else if (tmp[0].Equals("json"))
                    {
                        Console.WriteLine("Format " + arg);
                        outfutFormat = arg;
                        writer = new MyJsonWriter();
                        if (outputData.Equals("result.xml"))
                            outputData = "result.json";
                    }
                    else
                    {
                        argExc = true;
                        errorMsg += "[" + arg + "] ";
                    }
                }
            if (argExc) throw new ArgumentException(errorMsg);
        }

        /**
         * wczytywanie danych z pliku i dodanie do mapy 
         * key:nr indeksu, value:tablica z danymi studenta
         */
        private static Hashtable ReadFile()
        {
            string line;
            var j = 1;
            Boolean isOK;
            while ((line = inputFile.ReadLine()) != null)
            {
                isOK = true;
                var arr = line.Split(",");
                if (arr.Length == 9)
                    foreach (string i in arr)
                    {
                        if (string.IsNullOrEmpty(i))
                        {
                            // zapis do log.txt w przypadku brakujących danych lub pustych wartości
                            isOK = false;
                            logFile.WriteLine("Niepełne dane w linii nr " + j + ": " + line);
                        }
                    }
                else
                {
                    // zapis do log.txt w przypadku niepoprawnej liczby kolumn
                    isOK = false;
                    logFile.WriteLine("Niepoprawna liczba kolumn w linii nr " + j + ": " + line);
                }
                //jeżeli wpis nie istnieje w mapie i jest poprawny zostaje dodany do danych wyjściowych
                if (isOK && !outputTable.ContainsKey(arr[4]))
                {
                    var student = new Student
                    {
                        indexNumber = "s" + arr[4],
                        fname = arr[0],
                        lname = arr[1],
                        birthdate = arr[5],
                        email = arr[6],
                        mothersName = arr[7],
                        fathersName = arr[8],
                        studies = new Studies
                        {
                            name = arr[2],
                            mode = arr[3]
                        }
                    };
                    outputTable.Add(arr[4], student);
                }
                //jest raportowany do pliku log.txt w przeciwnym przypadku
                else
                    logFile.WriteLine("Redundancja elementów w linii nr " + j + ": " + line);
                j++;
            }
            return outputTable;
        }
    }
}
