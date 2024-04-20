using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WpfApp4
{
    public partial class MainWindow : Window
    {
        private List<Expense> expenses;

        public MainWindow()
        {
            InitializeComponent();
            expenses = new List<Expense>();
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = datePicker.SelectedDate ?? DateTime.Now;
            decimal amount = 0;
            if (decimal.TryParse(amountTextBox.Text, out amount))
            {
                string note = noteTextBox.Text;
                expenses.Add(new Expense { Date = date, Amount = amount, Note = note });
                expenseListBox.ItemsSource = null;
                expenseListBox.ItemsSource = expenses;
            }
            else
            {
                MessageBox.Show("Некорректная сумма. Пожалуйста, введите корректное число.");
            }
        }

        private void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            if (expenseListBox.SelectedItem != null)
            {
                expenses.Remove((Expense)expenseListBox.SelectedItem);
                expenseListBox.ItemsSource = null;
                expenseListBox.ItemsSource = expenses;
            }
            else
            {
                MessageBox.Show("Выберите расход для удаления.");
            }
        }

        private void SaveExpenses_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string json = JsonConvert.SerializeObject(expenses);
                File.WriteAllText(filePath, json);
            }
        }

        private void LoadExpenses_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    expenses = JsonConvert.DeserializeObject<List<Expense>>(json);
                    expenseListBox.ItemsSource = expenses;
                }
                else
                {
                    MessageBox.Show("Файл не найден.");
                }
            }
        }
    }

    public class Expense
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}