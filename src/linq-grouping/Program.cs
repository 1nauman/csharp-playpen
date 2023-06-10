// See https://aka.ms/new-console-template for more information

using linq_grouping;
using static System.Console;

var employees = new Employee[]
{
    new(1, "John Doe", new DateOnly(1990, 6, 1), new DateOnly(2010, 6, 1)),
    new(2, "Jane Smith", new DateOnly(1992, 7, 2), new DateOnly(2012, 7, 2)),
    new(3, "Robert Johnson", new DateOnly(1989, 5, 3), new DateOnly(2009, 5, 3)),
    new(4, "James Brown", new DateOnly(1991, 8, 4), new DateOnly(2011, 8, 4)),
    new(5, "Patricia Davis", new DateOnly(1993, 9, 5), new DateOnly(2013, 9, 5)),
    new(6, "Michael Wilson", new DateOnly(1990, 6, 1), new DateOnly(2010, 6, 1)),
    new(7, "Linda Jones", new DateOnly(1992, 7, 2), new DateOnly(2012, 7, 2)),
    new(8, "William Thomas", new DateOnly(1989, 5, 3), new DateOnly(2009, 5, 3)),
    new(9, "Elizabeth Taylor", new DateOnly(1991, 8, 4), new DateOnly(2011, 8, 4)),
    new(10, "David Moore", new DateOnly(1993, 9, 5), new DateOnly(2013, 9, 5)),
    new(11, "Sanjay Dutt", new DateOnly(1965, 12, 31), new DateOnly(1981, 5, 7))
};

var groupByBirthDateThenByJoiningDate = employees.GroupBy(o => new {o.BirthDate, o.JoiningDate}).ToArray();

foreach (var group in groupByBirthDateThenByJoiningDate)
{
    WriteLine($"Group: {group.Key.BirthDate:yyyy-MM-dd} {group.Key.JoiningDate:yyyy-MM-dd}");
    foreach (var employee in group)
    {
        WriteLine(employee);
    }
}

/*
 
 Sample output:
 
Group: 1990-06-01 2010-06-01
1, John Doe, 1990-06-01, 2010-06-01
6, Michael Wilson, 1990-06-01, 2010-06-01
Group: 1992-07-02 2012-07-02
2, Jane Smith, 1992-07-02, 2012-07-02
7, Linda Jones, 1992-07-02, 2012-07-02
Group: 1989-05-03 2009-05-03
3, Robert Johnson, 1989-05-03, 2009-05-03
8, William Thomas, 1989-05-03, 2009-05-03
Group: 1991-08-04 2011-08-04
4, James Brown, 1991-08-04, 2011-08-04
9, Elizabeth Taylor, 1991-08-04, 2011-08-04
Group: 1993-09-05 2013-09-05
5, Patricia Davis, 1993-09-05, 2013-09-05
10, David Moore, 1993-09-05, 2013-09-05
Group: 1965-12-31 1981-05-07
11, Sanjay Dutt, 1965-12-31, 1981-05-07

*/