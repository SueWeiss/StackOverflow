using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;


public class QuestionViewModel
{
    public Question Question { get; set; }
    public IEnumerable<Answer> Answers { get; set; }
    public bool LoggedIn { get; set; }
}

