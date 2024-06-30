using System;
using System.Linq;
using System.Windows;
using LiteDB;

namespace NoSqlDatabaseWPF
{
    public partial class MainWindow : Window
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<Document> _documentsCollection;

        public MainWindow()
        {
            InitializeComponent();
            _database = new LiteDatabase(@"MyData.db");
            _documentsCollection = _database.GetCollection<Document>("documents");
        }

        private void AddDocument_Click(object sender, RoutedEventArgs e)
        {
            var document = new Document
            {
                Id = Guid.NewGuid(),
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text
            };
            _documentsCollection.Insert(document);
            MessageBox.Show("Документ успішно доданий!");
            NameTextBox.Clear();
            DescriptionTextBox.Clear();
        }

        private void ViewDocuments_Click(object sender, RoutedEventArgs e)
        {
            var documents = _documentsCollection.FindAll().ToList();
            DocumentsListBox.Items.Clear();
            foreach (var doc in documents)
            {
                DocumentsListBox.Items.Add($"{doc.Name}: {doc.Description}");
            }
        }
    }
}
