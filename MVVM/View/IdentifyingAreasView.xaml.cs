using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryDeweyDecimalApp.MVVM.View
{
    public partial class IdentifyingAreasView : UserControl
    {
        private Dictionary<string, string> callNumbersAndDescriptions;
        private List<string> selectedCallNumbers;
        private List<string> shuffledDescriptions;
        private int currentQuestionIndex;

        public IdentifyingAreasView()
        {
            InitializeComponent();

            callNumbersAndDescriptions = new Dictionary<string, string>();
            selectedCallNumbers = new List<string>();
            shuffledDescriptions = new List<string>();

            InitializeData();
            LoadQuestion(0);
        }

        private void InitializeData()
        {
            // Define call numbers and their descriptions
            callNumbersAndDescriptions = new Dictionary<string, string>
            {
                { "000-099", "Generalities" },
                { "100-199", "Philosophy" },
                { "200-299", "Religion" },
                { "300-399", "Social Sciences" },
                { "400-499", "Language" },
                { "500-599", "Natural Sciences" },
                { "600-699", "Technology" },
                // Add more categories
            };

            // Create a shuffled list of descriptions
            shuffledDescriptions = callNumbersAndDescriptions.Values.OrderBy(x => Guid.NewGuid()).ToList();

            // Create a list of call numbers for the current question
            selectedCallNumbers = callNumbersAndDescriptions.Keys.Take(4).ToList();

            // Initialize current question index
            currentQuestionIndex = 0;
        }

        private void LoadQuestion(int questionIndex)
        {
            if (questionIndex >= 0 && questionIndex < callNumbersAndDescriptions.Count)
            {
                currentQuestionIndex = questionIndex;

                // Display call numbers for the current question
                CallNumbersList.ItemsSource = selectedCallNumbers;

                // Display shuffled descriptions for the current question
                DescriptionsList.ItemsSource = shuffledDescriptions.Skip(questionIndex * 7).Take(7);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if the user's answer is correct
            var selectedDescription = DescriptionsList.SelectedItem as string;
            if (selectedDescription != null)
            {
                var correctCallNumber = callNumbersAndDescriptions.FirstOrDefault(x => x.Value == selectedDescription).Key;
                if (selectedCallNumbers.Contains(correctCallNumber))
                {
                    // Correct answer
                    MessageBox.Show("Correct!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Incorrect answer
                    MessageBox.Show("Incorrect!", "Result", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            // Load the next question
            currentQuestionIndex++;
            LoadQuestion(currentQuestionIndex);
        }
    }
}
