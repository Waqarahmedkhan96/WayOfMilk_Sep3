namespace WoM_WebApi.GlobalExceptionHandler;

// Simple 404 exception
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// Simple 400 exception
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
