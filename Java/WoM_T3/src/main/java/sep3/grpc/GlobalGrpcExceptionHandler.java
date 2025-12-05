package sep3.grpc;
import io.grpc.Status;
import net.devh.boot.grpc.server.advice.GrpcAdvice;
import net.devh.boot.grpc.server.advice.GrpcExceptionHandler;

// Import the specific exceptions we want to catch
import org.springframework.dao.DataIntegrityViolationException;
import jakarta.persistence.EntityNotFoundException;

//general error handler for grpc, so we get a hint on what went wrong
//instead of the error unknown that grpc normally gives
//also instead of putting everything in try-catch blocks
@GrpcAdvice public class GlobalGrpcExceptionHandler
{

  //This handler catches ALL data integrity violations.
  //the message must now be inspected to determine if it's a unique constraint violation.
  @GrpcExceptionHandler(DataIntegrityViolationException.class) public Status handleDataIntegrityViolation(
      DataIntegrityViolationException e)
  {
    // Get the most specific, root cause message
    String message = e.getMostSpecificCause().getMessage();

    // This string check might be different for MySQL, H2, etc.
    // For PostgreSQL, it often contains "unique constraint" or "duplicate key".
    if (message.contains("unique") || message.contains("duplicate key"))
    {
      // It's the unique constraint! Send ALREADY_EXISTS.
      // We can't easily get the exact value duplicated here, so we send a more generic message.
      return Status.ALREADY_EXISTS.withDescription(
          "A record with that value already exists.");
    }

    // It was some OTHER integrity violation (e.g., a "not-null" constraint)
    return Status.INVALID_ARGUMENT.withDescription("Invalid data: " + message);
  }

  //handles all "Not Found" errors from findById().orElseThrow().
  @GrpcExceptionHandler(EntityNotFoundException.class) public Status handleNotFound(
      EntityNotFoundException e)
  {
    return Status.NOT_FOUND.withDescription(e.getMessage()); // Gets the "Cow not found with id: 123" message
  }

  // NEW: Catch Enum errors or bad arguments
  @GrpcExceptionHandler(IllegalArgumentException.class)
  public Status handleIllegalArgument(IllegalArgumentException e) {
    return Status.INVALID_ARGUMENT.withDescription(e.getMessage());
  }

  // A final catch-all for any other server error.
  @GrpcExceptionHandler(Exception.class) public Status handleGenericException(
      Exception e)
  {
    return Status.INTERNAL.withDescription(
        "An unexpected internal server error occurred.");
  }
}
