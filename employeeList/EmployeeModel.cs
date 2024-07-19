using System.ComponentModel;

namespace employeeList
{
    /// <summary>
    /// Модель сотрудника для хранения данных
    /// </summary>
    public class EmployeeModel
    {
        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [Description("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [Description("LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Зарплата сотрудника
        /// </summary>
        [Description("Salary")]
        public decimal SalaryPerHour { get; set; }

        /// <summary>
        /// Метод проверки введенных 
        /// </summary>
        /// <returns></returns>
        public bool HasMistakesInProperties()
        {
            if (FirstName == null || LastName == null) return true;
            else if (SalaryPerHour <= 0) return true;
            else return false;
        }
    }
}
