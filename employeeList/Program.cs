using employeeList;

internal class Program
{
    private static void Main(string[] args)
    {
        switch (args[0])
        {
            case "-add":
                Console.WriteLine(EmployeeManager.AddEmployee([.. args]));
                break;

            case "-update":
                Console.WriteLine(EmployeeManager.UpdateEmployee([.. args]));
                break;

            case "-delete":
                Console.WriteLine(EmployeeManager.DeleteEmployee([.. args]));
                break;

            case "-get":
                Console.WriteLine(EmployeeManager.GetEmployee([.. args]));
                break;

            case "-getall":
                Console.WriteLine(EmployeeManager.GetEmployees());
                break;

            default:
                Console.WriteLine("Неизвестная команда");
                break;

        }
    }
}