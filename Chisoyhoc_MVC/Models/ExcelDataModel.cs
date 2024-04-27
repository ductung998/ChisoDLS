using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chisoyhoc_MVC
{
    public class Observation
    {
        public int Id { get; set; }
        public string machiso { get; set; }
        public List<string> DStenbien { get; set; }
        public List<List<string>> data { get; set; }
        public string[][] data1 { get; set; }
        // Add more properties as needed

        // Method to get property value by name
        public string GetPropertyValue(string propertyName)
        {
            switch (propertyName)
            {
                case "Id":
                    return Id.ToString();
                case "Name":
                    return machiso;
                case "Price":
                    return DStenbien.ToString();
                default:
                    return "";
            }
        }
        public void setdata()
        {
            data = new List<List<string>>();

            // Iterate through each string array in data1
            foreach (var array in data1)
            {
                // Convert string array to list of strings and add to data
                data.Add(new List<string>(array));
            }
        }
    }

}