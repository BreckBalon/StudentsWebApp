namespace WebApp.ViewModels.Student;

public class SearchStudent
{
    public string Criteria { get; set; }

    public List<StudentDetail> Students { get; set; }
}

public class StudentDetail
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address { get; set; }

    public string Score { get; set; }
}
