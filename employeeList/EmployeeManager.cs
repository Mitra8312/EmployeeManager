using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace employeeList
{
    /// <summary>
    /// Класс для осуществления манипуляций с данными
    /// </summary>
    public class EmployeeManager
    {
        /// <summary>
        /// Путь к файлу со списком сотрудников
        /// </summary>
        private static string FilePath = "employees.txt";

        /// <summary>
        /// Метод загрузки пользоватлеей из файла
        /// </summary>
        /// <returns>Массив сотрудников из файла</returns>
        public static List<EmployeeModel> LoadEmployees()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return [];
                }

                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<List<EmployeeModel>>(json);
            }
            catch
            {
                Console.WriteLine("Ошибка при загрузке данных из файла");
                return [];
            }
        }

        /// <summary>
        /// Метод сохранения списка пользователей в файл
        /// </summary>
        /// <param name="employees">Список пользователей</param>
        private static void SaveEmployees(List<EmployeeModel> employees)
        {
            try
            {
                var json = JsonConvert.SerializeObject(employees);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Метод добавления пользоватлей в список
        /// </summary>
        /// <param name="arguments">Аргументы из командной строки</param>
        /// <returns>Строка с сообщением о результате операции</returns>
        public static string AddEmployee(List<string> arguments)
        {
            try
            {
                List<EmployeeModel> employees = [];

                try
                {
                    employees = LoadEmployees();
                }
                catch
                {
                    return "Ошибка при загрузке сотрудников";
                }

                int newId = employees.Count == 0 ? 1 : employees[^1].Id + 1;

                var newEmployee = new EmployeeModel
                {
                    Id = newId
                };

                foreach (var argument in arguments)
                {
                    try
                    {
                        SetPropertyValue(newEmployee, argument);
                    }
                    catch { return "Ошибка при чтении введенных данных"; }
                }

                if (newEmployee.HasMistakesInProperties())
                    return "Ошибка введенных данных";
                employees.Add(newEmployee);

                SaveEmployees(employees);

                return "Сотрудник добавлен успешно";
            }
            catch
            {
                return "Произошла ошибка при добавлении сотрудника";
            }
        }

        /// <summary>
        /// Метод обновления пользователя
        /// </summary>
        /// <param name="arguments">Аргументы из командной строки</param>
        /// <returns>Строка с сообщением о результате операции</returns>
        internal static string UpdateEmployee(List<string> arguments)
        {
            var employees = LoadEmployees();

            int id = GetIdFromArgs(arguments);

            if (id == 0) return "Ошибка при чтении введенных данных";

            var employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                foreach (var argument in arguments)
                {
                    try
                    {
                        SetPropertyValue(employee, argument);
                    }
                    catch { return "Ошибка при чтении введенных данных"; }
                }
            }
            else return $"Сотрудник с id = {id} не найден";

            SaveEmployees(employees);
            return "Данные успешно обновлены";
        }

        /// <summary>
        /// Метод удаления пользователя из списка
        /// </summary>
        /// <param name="arguments">Аргументы из командной строки</param>
        /// <returns>Строка с сообщением о результате операции</returns>
        internal static string DeleteEmployee(List<string> arguments)
        {
            var employees = LoadEmployees();

            int id = GetIdFromArgs(arguments);

            if (id == 0) return "Ошибка при чтении введенных данных";

            var employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                employees.Remove(employee);
            }
            else return $"Сотрудник с id = {id} не найден";

            SaveEmployees(employees);
            return "Данные успешно обновлены";
        }

        /// <summary>
        /// Метод получения пользователя по Id
        /// </summary>
        /// <param name="arguments">Аргументы из командной строки</param>
        /// <returns>Строка с сообщением о результате операции</returns>
        internal static string GetEmployee(List<string> arguments)
        {
            var employees = LoadEmployees();

            int id = GetIdFromArgs(arguments);

            if (id == 0) return "Ошибка при чтении введенных данных";

            var employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                return $"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}";
            }
            else return $"Сотрудник с id = {id} не найден";

        }

        /// <summary>
        /// Метод вывода списка сотрудников в консоль
        /// </summary>
        /// <returns>Строка с сообщением о результате операции</returns>
        internal static string GetEmployees()
        {
            string employeeListString = "";
            var employees = LoadEmployees();

            if (employees.Count == 0) return "Список сотрудников пуст";

            foreach (var employee in employees)
            {
                employeeListString += $"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}\n";
            }

            return employeeListString;
        }

        /// <summary>
        /// Метод обновления свойств сотрудника
        /// </summary>
        /// <param name="employee">Экземпляр класса сотрудника для его изменения</param>
        /// <param name="argument">Аргументы из командной строки</param>
        private static void SetPropertyValue(EmployeeModel? employee, string argument)
        {
            var property = GetPropertyByDescription(GetArgumentName(argument));

            if (property != null)
            {
                var convertedValue = Convert.ChangeType(GetArgumentValue(argument), property.PropertyType);
                property.SetValue(employee, convertedValue);
            }
        }

        /// <summary>
        /// Метод получения свойства сотрудника по его описания
        /// </summary>
        /// <param name="description">Описание свойства</param>
        /// <returns>Свойство класса сотрудника</returns>
        private static PropertyInfo GetPropertyByDescription(string description)
        {
            return typeof(EmployeeModel).GetProperties()
                      .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(DescriptionAttribute)) &&
                                              ((DescriptionAttribute)Attribute.GetCustomAttribute(prop, typeof(DescriptionAttribute))).Description == description);
        }

        /// <summary>
        /// Получение Id из аргументов командной строки
        /// </summary>
        /// <param name="arguments">Аргументы из командной строки</param>
        /// <returns>Id пользователя, если он существует, иначе - 0</returns>
        private static int GetIdFromArgs(List<string> arguments)
        {
            try
            {
                return Convert.ToInt32(GetArgumentValue(arguments.FirstOrDefault(arg => GetArgumentName(arg) == "Id")));
            }
            catch { return 0; }
        }

        /// <summary>
        /// Получение значения аргумента из командной строки
        /// </summary>
        /// <param name="argument">Аргумент из командной строки</param>
        /// <returns>Значение аргумента</returns>
        private static string GetArgumentValue(string argument)
        {
            return argument.Split(":")[1];
        }

        /// <summary>
        /// Получение названия аргумента из командной строки
        /// </summary>
        /// <param name="argument">Аргумент из командной строки</param>
        /// <returns>Название аргумента</returns>
        private static string GetArgumentName(string argument)
        {
            return argument.Split(":")[0];
        }
    }
}