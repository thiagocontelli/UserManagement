namespace UserManagement.Models;

public class AddUserInput(string name, string lastName, int age, string address)
{
    public string Name { get; private set; } = name;
    public string LastName { get; private set; } = lastName;
    public int Age { get; private set; } = age;
    public string Address { get; private set; } = address;
}
