namespace RepositoryContracts.ExceptionHandling
{
    // BASIC EXCEPTIONS USED BY REPOSITORIES AND WEB API

    // Thrown when something is not found (e.g. user, post, comment).
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    // Thrown when input data is invalid (e.g. empty username or password).
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    // Thrown when trying to create or update a username that already exists.
    public class DuplicateUserNameException : Exception
    {
        public DuplicateUserNameException(string message) : base(message) { }
    }

    // DOMAIN-SPECIFIC EXCEPTIONS (ADVANCED USE)

    // Base class for business rule (domain) errors.
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
    }

    // Thrown when an entity cannot be found in the domain layer.
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string message) : base(message) { }
    }

    // Thrown when an entity already exists in the domain layer.
    public class DuplicateEntityException : DomainException
    {
        public DuplicateEntityException(string message) : base(message) { }
    }

    // Thrown when validation fails in the domain layer.
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message) : base(message) { }
    }
}
