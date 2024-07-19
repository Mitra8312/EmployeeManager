using employeeList;
using System.Reflection;

namespace EmployeeManagerTests
{
    [TestFixture]
    public class EmployeeManagerTests
    {
        private const string TestFilePath = "test_employees.txt";
        [SetUp]
        public void Setup()
        {
            // Установим тестовый путь к файлу через рефлексию
            var field = typeof(EmployeeManager).GetField("FilePath", BindingFlags.NonPublic | BindingFlags.Static);
            field?.SetValue(null, TestFilePath);
        }

        [Test]
        public void AddEmployee_ValidArguments_AddsEmployeeSuccessfully()
        {
            var arguments = new List<string>
            {
                "FirstName:John",
                "LastName:Doe",
                "Salary:30"
            };

            var result = EmployeeManager.AddEmployee(arguments);

            Assert.That(result, Is.EqualTo("Сотрудник добавлен успешно"));

            var employees = EmployeeManager.LoadEmployees();
            Assert.IsNotNull(employees);
            Assert.That(employees.Count, Is.EqualTo(1));
            Assert.That(employees[0].FirstName, Is.EqualTo("John"));
            Assert.That(employees[0].LastName, Is.EqualTo("Doe"));
            Assert.That(employees[0].SalaryPerHour, Is.EqualTo(30));
        }

        [Test]
        public void AddEmployee_InvalidArguments_ReturnsErrorMessage()
        {
            var arguments = new List<string>
            {
                "FirstName:John",
                "LastName:Doe",
                "Salary:abc"
            };

            var result = EmployeeManager.AddEmployee(arguments);

            Assert.That(result, Is.EqualTo("Ошибка при чтении введенных данных"));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
    }
}
