using System;
using LiteDB;

namespace NoSqlDatabaseProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Путь к базе данных
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                // Получаем коллекцию документов
                var col = db.GetCollection<Document>("documents");

                // Создание документа
                var doc = new Document
                {
                    Id = Guid.NewGuid(),
                    Name = "Зразок документа",
                    Description = "Це зразок документа."
                };
                col.Insert(doc);

                // Чтение документа
                var readDoc = col.FindById(doc.Id);
                Console.WriteLine($"Читання документа: {readDoc.Name}, {readDoc.Description}");

                // Обновление документа
                readDoc.Description = "Оновлено опис.";
                col.Update(readDoc);

                // Удаление документа
                col.Delete(readDoc.Id);

                // Просмотр всех документов
                var allDocs = col.FindAll();
                foreach (var d in allDocs)
                {
                    Console.WriteLine($"Document: {d.Id}, {d.Name}, {d.Description}");
                }
            }
        }
    }
}