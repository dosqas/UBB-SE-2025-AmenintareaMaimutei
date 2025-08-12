﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoClassLibrary.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public int NumberOfCompletedSections { get; set; }
    public int NumberOfCompletedQuizzesInSection { get; set; }
    public string? Email { get; set; }

    public User(int id, string username, int numberOfCompletedSections = 0, int numberOfCompletedQuizzesInSection = 0)
    {
        UserId = id;
        Username = username;
        NumberOfCompletedSections = numberOfCompletedSections;
        NumberOfCompletedQuizzesInSection = numberOfCompletedQuizzesInSection;
    }

    public User(string username)
    {
        Username = username;
    }

    public User()
    {
    }
}