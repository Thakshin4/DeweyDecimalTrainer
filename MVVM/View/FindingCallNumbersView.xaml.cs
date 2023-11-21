using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static LibraryDeweyDecimalApp.DeweyDecimalLibrary;

namespace LibraryDeweyDecimalApp.MVVM.View
{
    public partial class FindingCallNumbersView : UserControl
    {
        private DeweyDecimalLibrary deweyLibrary;
        private Dictionary<string, string> answerPair = new Dictionary<string, string>();

        public FindingCallNumbersView()
        {
            InitializeComponent();
            LoadDeweyData();
            InitializeQuizOptions();
        }

        private void LoadDeweyData()
        {
            try
            {
                // Load Dewey Decimal data
                DeweyDecimalLibrary libraryReader = new DeweyDecimalLibrary();
                deweyLibrary = libraryReader.ReadDeweyData("Data/DeweyData.json");

                if (deweyLibrary == null)
                {
                    // Handle the case where deweyLibrary is null
                    throw new Exception("Failed to load Dewey Decimal data.");
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception
                MessageBox.Show($"Error loading Dewey Decimal data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayRandomDescription(DeweyDecimalCategory topLevelCategory)
        {
            // Get random 3rd level description within the selected top-level category
            var randomDescription = GetRandomThirdLevelDescription(topLevelCategory);
            var topLevelKey = $"{topLevelCategory.Number} - {topLevelCategory.Description}";

            // Set corect answer as key value pair
            answerPair.Clear();
            answerPair.Add(topLevelKey, randomDescription);            

            // Display the description
            RandomDescriptionLabel.Text = randomDescription;
        }

        private string GetRandomThirdLevelDescription(DeweyDecimalCategory topLevelCategory)
        {
            // Flatten the 3rd level descriptions of the selected top-level category
            var thirdLevelDescriptions = FlattenThirdLevel(topLevelCategory);

            // Get a random description
            Random random = new Random();
            int randomIndex = random.Next(thirdLevelDescriptions.Count);
            return thirdLevelDescriptions[randomIndex];
        }


        private List<string> FlattenThirdLevel(DeweyDecimalCategory topLevelCategory)
        {
            var thirdLevelDescriptions = new List<string>();

            foreach (var subcategory in topLevelCategory.Children)
            {
                foreach (var subsubcategory in subcategory.Children)
                {
                    thirdLevelDescriptions.Add(subsubcategory.Description);
                }
            }

            return thirdLevelDescriptions;
        }


        private void InitializeQuizOptions()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 4); // Generates random number between 0 and 3 (inclusive)

            // Get top-level categories
            var topLevelCategories = deweyLibrary.Root;

            // Shuffle the categories to get random options
            var quizOptions = topLevelCategories.OrderBy(x => Guid.NewGuid()).ToList();

            // Display options to the user
            Option1.Content = $"{quizOptions[0].Number} - {quizOptions[0].Description}";
            Option2.Content = $"{quizOptions[1].Number} - {quizOptions[1].Description}";
            Option3.Content = $"{quizOptions[2].Number} - {quizOptions[2].Description}";
            Option4.Content = $"{quizOptions[3].Number} - {quizOptions[3].Description}";
            
            DisplayRandomDescription(quizOptions[randomNumber]);
        }

        private void CheckAnswer(object sender, RoutedEventArgs e)
        {
            var selectedOption = ((Button)sender).Content.ToString();
            var randomDescription = RandomDescriptionLabel.Text.ToString();            

            if (AreStringsAPair(answerPair, selectedOption, randomDescription))
            {
                // User selected the correct answer
                MessageBox.Show("Correct! Moving to the next level.");

                // Display a new random description and update quiz options
                InitializeQuizOptions();
            }
            else
            {
                // User selected the wrong answer
                var answer = $"{answerPair.FirstOrDefault()}";
                MessageBox.Show($"Incorrect! Answer: {answer}");

                // Display the same random description but with new quiz options
                InitializeQuizOptions();
            }
        }

        static bool AreStringsAPair(Dictionary<string, string> pairs, string first, string second)
    {
        // Check if the pair exists in the dictionary
        return pairs.TryGetValue(first, out var value) && value == second
            || pairs.TryGetValue(second, out var reverseValue) && reverseValue == first;
    }
    }
}
