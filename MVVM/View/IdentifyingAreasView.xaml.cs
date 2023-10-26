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
        private bool swap;
        private int points;

        public IdentifyingAreasView()
        {
            InitializeComponent();

            callNumbersAndDescriptions = new Dictionary<string, string>();
            selectedCallNumbers = new List<string>();
            shuffledDescriptions = new List<string>();
            swap = false;
            points = 0;

            InitializeData();
            LoadQuestion(0);
            NextQuestionButton.IsEnabled = false;
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

                // Shuffle the keys (call numbers) and values (descriptions)
                List<string> shuffledCallNumbers = callNumbersAndDescriptions.Keys.OrderBy(x => Guid.NewGuid()).ToList();
                List<string> shuffledDescriptions = callNumbersAndDescriptions.Values.OrderBy(x => Guid.NewGuid()).ToList();

                List<string> selectedCallNumbers;
                List<string> selectedDescriptions;

                if (swap)
                {
                    // Create a list of random call numbers and descriptions for the current question
                    selectedCallNumbers = shuffledCallNumbers.Take(7).ToList();
                    selectedDescriptions = shuffledDescriptions.Take(4).ToList();
                }
                else
                {
                    // Create a list of random call numbers and descriptions for the current question
                    selectedCallNumbers = shuffledCallNumbers.Take(4).ToList();
                    selectedDescriptions = shuffledDescriptions.Take(7).ToList();
                }


                // Display call numbers for the current question
                CallNumbersList.ItemsSource = selectedCallNumbers;

                // Display descriptions for the current question
                DescriptionsList.ItemsSource = selectedDescriptions;
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
                    points += 10;
                    PointsLabel.Content = points.ToString();

                    // Remove the correct answer from the lists
                    selectedCallNumbers.Remove(correctCallNumber);
                    shuffledDescriptions.Remove(selectedDescription);

                    if (selectedCallNumbers.Count == 0 || shuffledDescriptions.Count == 0)
                    {
                        // All questions are answered or one of the lists is empty, prompt the user to move to the next question
                        MessageBox.Show("Congratulations! You've completed the questions. Click Next Question to continue.", "Question Completed", MessageBoxButton.OK, MessageBoxImage.Information);
                        NextQuestionButton.IsEnabled = true;
                    }

                    // Update the ListBox contents after removal
            UpdateListBoxContents();
                }
                else
                {
                    // Incorrect answer
                    MessageBox.Show("Incorrect!", "Result", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateListBoxContents()
        {
            // Clear the current contents of both ListBoxes
            CallNumbersList.ItemsSource = null;
            DescriptionsList.ItemsSource = null;

            // Display call numbers for the current question
            CallNumbersList.ItemsSource = selectedCallNumbers;

            // Display descriptions for the current question
            DescriptionsList.ItemsSource = shuffledDescriptions;
        }


        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            // Load the next question
            swap = !swap;
            currentQuestionIndex++;

            // Check if there are more questions to display
            if (currentQuestionIndex < callNumbersAndDescriptions.Count)
            {
                LoadQuestion(currentQuestionIndex);
            }
            else
            {
                MessageBox.Show("All questions have been completed. Congratulations!", "All Questions Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
