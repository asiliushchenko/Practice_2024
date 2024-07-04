using System;
using System.Linq;
using System.Windows;
using LiteDB;
using Newtonsoft.Json;

namespace NoSqlDatabaseWPF
{
    public partial class MainWindow : Window
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<BsonDocument> _documentsCollection;

        public MainWindow()
        {
            InitializeComponent();
            _database = new LiteDatabase(@"MyData.db");
            _documentsCollection = _database.GetCollection<BsonDocument>("documents");
        }

        private void AddDocument_Click(object sender, RoutedEventArgs e)
        {
            var document = new Document
            {
                Id = Guid.NewGuid(),
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text
            };

            var json = JsonConvert.SerializeObject(document);
            var bsonDoc = LiteDB.JsonSerializer.Deserialize(json).AsDocument;

            _documentsCollection.Insert(bsonDoc);
            MessageBox.Show($"Документ успішно доданий та має такий ID: {document.Id}");
            NameTextBox.Clear();
            DescriptionTextBox.Clear();

            // Логування
            Console.WriteLine($"Доданий документ: {json}");
        }

        private void ViewDocuments_Click(object sender, RoutedEventArgs e)
        {
            var documents = _documentsCollection.FindAll().ToList();
            DocumentsListBox.Items.Clear();
            foreach (var bsonDoc in documents)
            {
                var json = LiteDB.JsonSerializer.Serialize(bsonDoc);
                var doc = JsonConvert.DeserializeObject<Document>(json);
                DocumentsListBox.Items.Add($"{doc.Id} - {doc.Name}: {doc.Description}");
            }
        }

        private void DeleteDocument_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentsListBox.SelectedItem == null)
            {
                MessageBox.Show("Будь-ласка оберіть запис для видалення.");
                return;
            }

            var selectedText = DocumentsListBox.SelectedItem.ToString();
            Console.WriteLine($"Текст обраного документа: {selectedText}");

            // Витягненння ID з обраного елемента
            var idPart = selectedText.Split(' ')[0];
            Console.WriteLine($"Витягнута частина ID: {idPart}");

            // Перевірка коректності перетворення в Guid
            if (Guid.TryParse(idPart, out var selectedId))
            {
                Console.WriteLine($"Розібраний ID: {selectedId}");

                var deleted = _documentsCollection.Delete(new BsonValue(selectedId));
                if (deleted)
                {
                    MessageBox.Show("Докумет успішно видалений!");
                    Console.WriteLine($"Видалений документ з ID: {selectedId}");
                }
                else
                {
                    MessageBox.Show("Не вдалося видалити запис.");
                    Console.WriteLine($"Помилка видалення документа з ID: {selectedId}");
                }
                ViewDocuments_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Помилка витягнення ID документа.");
                Console.WriteLine("Не вдалося розібрати ID документа.");
            }
        }

        private void EditDocument_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentsListBox.SelectedItem == null)
            {
                MessageBox.Show("Будь-ласка оберіть запис для редагування.");
                return;
            }

            var selectedText = DocumentsListBox.SelectedItem.ToString();
            var selectedId = Guid.Parse(selectedText.Split(' ')[0]);

            var document = _documentsCollection.FindById(new BsonValue(selectedId));
            if (document != null)
            {
                var json = LiteDB.JsonSerializer.Serialize(document);
                var doc = JsonConvert.DeserializeObject<Document>(json);

                NameTextBox.Text = doc.Name;
                DescriptionTextBox.Text = doc.Description;

                var newDocument = new Document
                {
                    Id = doc.Id,
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text
                };

                var bsonDoc = LiteDB.JsonSerializer.Deserialize(JsonConvert.SerializeObject(newDocument)).AsDocument;
                _documentsCollection.Update(bsonDoc);

                MessageBox.Show("Документ успішно відредагован!");
                Console.WriteLine($"Відредагований документ: {JsonConvert.SerializeObject(newDocument)}");
                ViewDocuments_Click(sender, e);
            }
        }
    }
}