using LiteDB;
using System.Windows;
using System.Windows.Controls;

namespace NoSqlDatabaseWPF
{
    public partial class MainWindow : Window
    {
        public LiteDatabase Database { get; set; }
        public ILiteCollection<BsonDocument> DocumentsCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Database = new LiteDatabase(@"MyData.db");
            DocumentsCollection = Database.GetCollection<BsonDocument>("documents");
            LoadDocuments();
        }

        public void LoadDocuments()
        {
            var documents = DocumentsCollection.FindAll();
            DocumentsListBox.Items.Clear();
            foreach (var doc in documents)
            {
                var name = doc["Name"].AsString;
                var description = doc["Description"].AsString;
                DocumentsListBox.Items.Add($"{name} - {description}");
            }
        }

        public void AddDocument_Click(object sender, RoutedEventArgs e)
        {
            var doc = new BsonDocument
            {
                ["Name"] = NameTextBox.Text,
                ["Description"] = DescriptionTextBox.Text
            };
            DocumentsCollection.Insert(doc);
            LoadDocuments();
        }

        public void DeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentsListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a document to delete.");
                return;
            }

            var selectedText = DocumentsListBox.SelectedItem.ToString();
            var selectedName = selectedText.Split('-')[0].Trim();

            var docToDelete = DocumentsCollection.FindOne(d => d["Name"] == selectedName);
            if (docToDelete != null)
            {
                DocumentsCollection.Delete(docToDelete["_id"]);
                LoadDocuments();
            }
            else
            {
                MessageBox.Show("Document not found.");
            }
        }

        public void EditDocument_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentsListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a document to edit.");
                return;
            }

            var selectedText = DocumentsListBox.SelectedItem.ToString();
            var selectedName = selectedText.Split('-')[0].Trim();

            var docToEdit = DocumentsCollection.FindOne(d => d["Name"] == selectedName);
            if (docToEdit != null)
            {
                docToEdit["Name"] = NameTextBox.Text;
                docToEdit["Description"] = DescriptionTextBox.Text;
                DocumentsCollection.Update(docToEdit);
                LoadDocuments();
            }
            else
            {
                MessageBox.Show("Document not found.");
            }
        }
    }
}