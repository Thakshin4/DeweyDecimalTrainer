using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using static LibraryDeweyDecimalApp.DeweyDecimalLibrary;

namespace LibraryDeweyDecimalApp.MVVM.View
{
    public partial class FindingCallNumbersView : UserControl
    {
        // Define the timer
        private DispatcherTimer timer;
        private TimeSpan elapsedTime;

        private int quizCount = 0;
        private int level = 1;

        private DeweyDecimalLibrary deweyLibrary;
        private Dictionary<string, string> answerPair = new Dictionary<string, string>();

        public FindingCallNumbersView()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RandomDescriptionLabel.Visibility = Visibility.Visible;
            Option1.Visibility = Visibility.Visible;
            Option2.Visibility = Visibility.Visible;
            Option3.Visibility = Visibility.Visible;
            Option4.Visibility = Visibility.Visible;

            LoadDeweyData();
            InitializeQuizOptions(level);

            // Initialize the timer
            InitializeTimer();
            timer.Start();
        }

        private void InitializeTimer()
        {
            // Initialize the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            elapsedTime = TimeSpan.Zero;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromMilliseconds(100)); // Update elapsed time
            TimerLabel.Content = $"{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}.{elapsedTime.Milliseconds:D3}";
        }

        private void SaveTime()
        {
            // Write the time to a text file
            string elapsedTimeString = TimerLabel.Content.ToString();
            string filePath = "FindingCallNumbersTimes.txt"; // Adjust the file path as needed
            File.AppendAllLines(filePath, new[] { elapsedTimeString });
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

            if (topLevelCategory.Children != null && topLevelCategory.Children.Any())
            {
                foreach (var subcategory in topLevelCategory.Children)
                {
                    // Check if the subcategory has children
                    if (subcategory.Children != null && subcategory.Children.Any())
                    {
                        foreach (var subsubcategory in subcategory.Children)
                        {
                            thirdLevelDescriptions.Add(subsubcategory.Description);
                        }
                    }
                    else
                    {
                        // If no children, add the subcategory's description directly
                        thirdLevelDescriptions.Add(subcategory.Description);
                    }
                }
            }
            else
            {
                // If no children, add the subcategory's description directly
                thirdLevelDescriptions.Add(topLevelCategory.Description);
            }

            return thirdLevelDescriptions;
        }

        private void InitializeQuizOptions(int level)
        {
            Level.Content = level;
            Quiz.Content = quizCount;

            Random random = new Random();
            int randomNumber = random.Next(0, 4); // Generates random number between 0 and 3 (inclusive)

            List<DeweyDecimalCategory> quizOptions;

            // Select categories based on the specified level
            switch (level)
            {
                case 1:
                    quizOptions = deweyLibrary.Root.OrderBy(x => Guid.NewGuid()).ToList();
                    break;

                case 2:
                    // Select second-level categories
                    var secondLevelCategories = deweyLibrary.Root
                        .SelectMany(category => category.Children)
                        .OrderBy(x => Guid.NewGuid())
                        .ToList();
                    quizOptions = secondLevelCategories;
                    break;

                case 3:
                    // Select third-level categories
                    var thirdLevelCategories = deweyLibrary.Root
                        .SelectMany(category => category.Children)
                        .SelectMany(subcategory => subcategory.Children)
                        .OrderBy(x => Guid.NewGuid())
                        .ToList();
                    quizOptions = thirdLevelCategories;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), "Invalid level specified");
            }

            // Display options to the user
            Option1.Content = $"{quizOptions[0].Number} - {quizOptions[0].Description}";
            Option2.Content = $"{quizOptions[1].Number} - {quizOptions[1].Description}";
            Option3.Content = $"{quizOptions[2].Number} - {quizOptions[2].Description}";
            Option4.Content = $"{quizOptions[3].Number} - {quizOptions[3].Description}";

            // Display random description based on the selected category
            DisplayRandomDescription(quizOptions[randomNumber]);
        }


        private void CheckAnswer(object sender, RoutedEventArgs e)
        {
            var selectedOption = ((Button)sender).Content.ToString();
            var randomDescription = RandomDescriptionLabel.Text.ToString();            

            if (AreStringsAPair(answerPair, selectedOption, randomDescription))
            {
                // User selected the correct answer
                string? message;
                if (level < 3)
                {
                    level++;
                    message = ("Correct! Moving to the next level.");
                }
                else
                {
                    level = 1;
                    quizCount++;
                    message = ("Correct! You completed the quiz.");
                    
                }

                if (quizCount == 10)
                {
                    timer.Stop();
                    SaveTime();
                    MessageBox.Show("Congratulations! You completed 10 quizzes.");
                }

                MessageBox.Show(message);
                InitializeQuizOptions(level);
            }
            else
            {
                level = 1;

                // User selected the wrong answer
                var answer = $"{answerPair.FirstOrDefault()}";
                MessageBox.Show($"Incorrect! Answer: {answer}");

                // Display the same random description but with new quiz options
                InitializeQuizOptions(level);
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
