using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Text.Json.Serialization;

namespace Cw2
{
    [Serializable()]
    
    public class Uczelnia
    {
        private Hashtable StudiesCount = new Hashtable();


        [XmlAttribute]
        public string createdAt { get; set; } = DateTime.Today.ToString("dd.MM.yyyy");
        [XmlAttribute]
        public string author { get; set; } = "Grzegorz Frączek";

        [JsonIgnore]
        [XmlElement("studenci", Order = 1)]
        public Studenci Studenci { get; set; }

        [JsonPropertyName("studenci")]
        [XmlIgnore]
        public List<Student> Students { get; set; }

        [XmlArray("activeStudies", Order = 2)]
        public List<ActiveStudies> activeStudies { get; set; } = new List<ActiveStudies>();

        public Uczelnia() { }

        public Uczelnia(Studenci studenci)
        {
            this.Studenci = studenci;
            this.Students = studenci.students;
            foreach(Student student in Studenci.students)
            {
                if (StudiesCount.ContainsKey(student.studies.name))
                    StudiesCount[student.studies.name] = ((int)StudiesCount[student.studies.name]) + 1;
                else
                    StudiesCount.Add(student.studies.name, 1);
            }
            foreach (DictionaryEntry entry in StudiesCount)
            {
                activeStudies.Add(new ActiveStudies{
                    name = (string)entry.Key,
                    numberOfStudents = (int)entry.Value 
                });
            }
        }  
    }
}
