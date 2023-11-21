using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace LibraryDeweyDecimalApp
{
    public class DeweyDecimalCategory
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public List<DeweyDecimalCategory> Children { get; set; }
    }

    public class DeweyDecimalLibrary
    {
        public List<DeweyDecimalCategory> Root { get; set; }

        public DeweyDecimalLibrary()
        {
            Root = new List<DeweyDecimalCategory>();
        }

        public void BuildTree(DeweyDecimalCategory parent, TreeViewItem parentNode)
        {
            foreach (var child in parent.Children)
            {
                var childNode = new TreeViewItem { Header = $"{child.Number} - {child.Description}" };
                parentNode.Items.Add(childNode);
                BuildTree(child, childNode);
            }
        }

        public DeweyDecimalLibrary ReadDeweyData(string jsonFilePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                return JsonConvert.DeserializeObject<DeweyDecimalLibrary>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return null;
            }
        }
    }
}
