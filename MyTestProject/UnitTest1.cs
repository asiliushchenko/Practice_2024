using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoSqlDatabaseWPF;
using System;
using System.Windows.Controls;
using System.Threading;

namespace MyTestProject
{
    [TestClass]
    public class MainWindowTests
    {
        private MainWindow _mainWindow;

        [TestInitialize]
        public void Setup()
        {
            Thread thread = new Thread(() =>
            {
                _mainWindow = new MainWindow();
                System.Windows.Threading.Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [TestMethod]
        public void Test_AddDocument()
        {
            _mainWindow.NameTextBox = new TextBox { Text = "Test Name" };
            _mainWindow.DescriptionTextBox = new TextBox { Text = "Test Description" };

            _mainWindow.AddDocument_Click(null, null);

            Assert.IsTrue(_mainWindow.DocumentsListBox.Items.Contains("Test Name - Test Description"));
        }

        [TestMethod]
        public void Test_DeleteDocument()
        {
            _mainWindow.NameTextBox = new TextBox { Text = "Test Name" };
            _mainWindow.DescriptionTextBox = new TextBox { Text = "Test Description" };
            _mainWindow.AddDocument_Click(null, null);

            _mainWindow.DocumentsListBox.SelectedItem = "Test Name - Test Description";

            _mainWindow.DeleteDocument_Click(null, null);

            Assert.IsFalse(_mainWindow.DocumentsListBox.Items.Contains("Test Name - Test Description"));
        }

        [TestMethod]
        public void Test_EditDocument()
        {
            _mainWindow.NameTextBox = new TextBox { Text = "Test Name" };
            _mainWindow.DescriptionTextBox = new TextBox { Text = "Test Description" };
            _mainWindow.AddDocument_Click(null, null);

            _mainWindow.DocumentsListBox.SelectedItem = "Test Name - Test Description";
            _mainWindow.NameTextBox.Text = "Updated Name";
            _mainWindow.DescriptionTextBox.Text = "Updated Description";

            _mainWindow.EditDocument_Click(null, null);

            Assert.IsFalse(_mainWindow.DocumentsListBox.Items.Contains("Test Name - Test Description"));
            Assert.IsTrue(_mainWindow.DocumentsListBox.Items.Contains("Updated Name - Updated Description"));
        }
    }
}