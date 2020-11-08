using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Cw2
{
    [Serializable()]
    public class Studenci
    {
        [JsonPropertyName("students")]
        [XmlElement("student")]
        public List<Student> students { get; set; } = new List<Student>();

        public Studenci(Hashtable studentsList)
        {
            foreach (Student tmp in studentsList.Values)
            {
                students.Add(tmp);
            }
        }
        public Studenci() { }
    }
}
