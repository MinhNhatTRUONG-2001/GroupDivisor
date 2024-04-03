using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace TextFileExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<string> names;
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("names.txt"))
            {
                File.WriteAllText("names.txt", "");
            }
            nameList.Text = File.ReadAllText("names.txt");
            names = nameList.Text.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private void AddName(object sender, RoutedEventArgs e)
        {
            File.AppendAllText("names.txt", nameField.Text + Environment.NewLine);
            nameList.Text = File.ReadAllText("names.txt");
            names.Add(nameField.Text);
            nameField.Text = "";
        }

        private void Divide(object sender, RoutedEventArgs e)
        {
            int rows = 0, columns = 0; //Each column represent one group
            int fourMemberGroups;
            //Define group structure
            if (names.Count < 4)
            {
                rows = names.Count;
                columns = 1;
                groupStructure.Text = "1 team that has less than four members";
            }
            else
            {
                switch (names.Count % 4)
                {
                    case 0:
                        rows = 4;
                        columns = names.Count / 4;
                        groupStructure.Text = columns + " four-member team(s)";
                        break;
                    case 1:
                        rows = 5;
                        columns = names.Count / 4;
                        fourMemberGroups = names.Count / 4 - 1;
                        if (fourMemberGroups == 0)
                        {
                            groupStructure.Text = "1 five-member team";
                        }
                        else
                        {
                            groupStructure.Text = fourMemberGroups + " four-member team(s) and 1 five-member team";
                        }
                        break;
                    case 2:
                        rows = 4;
                        columns = names.Count / 4 + 1;
                        fourMemberGroups = names.Count / 4 - 1;
                        if (fourMemberGroups == 0)
                        {
                            groupStructure.Text = "2 three-member teams";
                        }
                        else
                        {
                            groupStructure.Text = fourMemberGroups + " four-member team(s) and 2 three-member teams";
                        }
                        break;
                    case 3:
                        rows = 4;
                        columns = names.Count / 4 + 1;
                        groupStructure.Text = names.Count / 4 + " four-member team(s) and 1 three-member team";
                        break;
                }
            }
            //Randomize the name list
            Random rnd = new Random();
            List<string> namesCopy = new List<string>(names);
            string[] randomizedNames = new string[namesCopy.Count];
            for (int i = 0; i < names.Count; i++)
            {
                int ncIndex = rnd.Next(namesCopy.Count);
                randomizedNames[i] = namesCopy[ncIndex];
                namesCopy.RemoveAt(ncIndex);
            }
            //Assign names to groups
            string[,] groups = new string[rows, columns];
            int rnIndex = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (rnIndex >= randomizedNames.Length)
                    {
                        groups[i, j] = "";
                    }
                    else
                    {
                        groups[i, j] = randomizedNames[rnIndex].Trim();
                    }
                    rnIndex++;
                }
            }
            //Display result on the screen
            string result = "";
            for (int j = 0; j < columns; j++)
            {
                result += "- Group " + (j + 1) + ": ";
                for (int i = 0; i < rows; i++)
                {
                    if (groups[i, j] != "")
                    {
                        result += groups[i, j] + ", ";
                    }
                }
                result += "\n";
            }
            groupResult.Text = result;
        }
    }
}