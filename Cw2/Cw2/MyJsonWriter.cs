using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Cw2
{
    [Serializable]
    class MyJsonWriter : IFileWriter
    {
        [JsonPropertyName("uczelnia")]
        public Uczelnia uczelnia { get; set; }
        public void write(Uczelnia uczelnia, string outputData)
        {
            this.uczelnia = uczelnia;

            //konfiguracja serializacji
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            //serializacja
            string jsonString = JsonSerializer.Serialize(this, options);

            /*
             * usunięcie problemu niepoprawnej strony kodowej - brak polskich znaków
             */
            List<string> plList = new List<string>();
            plList.Add("ą");
            plList.Add("ć");
            plList.Add("ę");
            plList.Add("ł");
            plList.Add("ń");
            plList.Add("ó");
            plList.Add("ś");
            plList.Add("ź");
            plList.Add("ż");
            plList.Add("Ą");
            plList.Add("Ć");
            plList.Add("Ę");
            plList.Add("Ł");
            plList.Add("Ń");
            plList.Add("Ó");
            plList.Add("Ś");
            plList.Add("Ź");
            plList.Add("Ż");

            string pl = JsonSerializer.Serialize(plList);
            var plTab = pl.Substring(1, pl.Length-1).Split(",");
            
            string[] tmp = jsonString.Split(Environment.NewLine);
            StringBuilder result = new StringBuilder();
            string stmp = "";
            for (int j = 0; j < tmp.Length; j++)
            {
                stmp = tmp[j];
                for (int i = 0; i < plTab.Length; i++)
                {
                    stmp = stmp.Replace(plTab[i].Substring(1,plTab[i].Length-2), plList.ElementAt(i));
                }
                result.Append(stmp);
                result.AppendLine();
            }

            //zapis do pliku
            File.WriteAllText(outputData, result.ToString(), Encoding.UTF8);
           
        }
    }
}
