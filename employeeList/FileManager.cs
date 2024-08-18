using Newtonsoft.Json;

namespace employeeList
{
    public class FileManager
    {
        /// <summary>
        /// Путь к файлу со списком сотрудников
        /// </summary>
        private static string FilePath = "employees.txt";

        /// <summary>
        /// Метод загрузки пользоватлеей из файла
        /// </summary>
        /// <returns>Массив сотрудников из файла</returns>
        public List<EmployeeModel> LoadEmployees()
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
        internal void SaveEmployees(List<EmployeeModel> employees)
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
    }
}
