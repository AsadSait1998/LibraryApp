﻿namespace LibraryApp.Services;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
