using System.Net;

namespace Ecommerce.Core.src.Common
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public AppException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
        }

        // Bad Request
        public static AppException BadRequest(string message = "Bad Request")
        {
            return new AppException(HttpStatusCode.BadRequest, message);
        }

        // Unauthorized
        public static AppException Unauthorized(string message = "Unauthorized Access")
        {
            return new AppException(HttpStatusCode.Unauthorized, message);
        }

        // Forbidden
        public static AppException Forbidden(string message = "Forbidden Access")
        {
            return new AppException(HttpStatusCode.Forbidden, message);
        }

        // Not Found
        public static AppException NotFound(string message = "Resource Not Found")
        {
            return new AppException(HttpStatusCode.NotFound, message);
        }

        // Conflict (e.g., duplicate data)
        public static AppException Conflict(string message = "Conflict")
        {
            return new AppException(HttpStatusCode.Conflict, message);
        }

        // Unprocessable Entity (e.g., validation errors)
        public static AppException UnprocessableEntity(string message = "Unprocessable Entity")
        {
            return new AppException(HttpStatusCode.UnprocessableEntity, message);
        }

        // Internal Server Error
        public static AppException InternalServerError(string message = "Internal Server Error")
        {
            return new AppException(HttpStatusCode.InternalServerError, message);
        }

        // Custom Error with specific status code
        public static AppException CustomError(HttpStatusCode statusCode, string message)
        {
            return new AppException(statusCode, message);
        }

        //****custom authntication exception group****

        // Unauthorized Access with specific message
        public static AppException InvalidCredential(string message = "Invalid credentials: email or password is incorrect")
        {
            return new AppException(HttpStatusCode.Unauthorized, message);
        }

        //****custom user service exception group****
        public static AppException UserNotFound(string message = "User not found")
        {
            return new AppException(HttpStatusCode.NotFound, message);
        }
        public static AppException UserCredentialErrorEmailAlreadyExist(string email)
        {
            string message = $"A user with the email {email} already exists.";
            return new AppException(HttpStatusCode.NotFound, message);
        }

        //****custom product service exception group****
        public static AppException CategoryNotFound(Guid? id)
        {
            string message = $"A category with the ID {id} not found.";
            return new AppException(HttpStatusCode.NotFound, message);
        }

        public static AppException ProductPriceLessThan0()
        {
            return new AppException(HttpStatusCode.BadRequest, "Price must be greater than 0.");
        }

        public static AppException ProductInventoryLessThan0()
        {
            return new AppException(HttpStatusCode.BadRequest, "Inventory must be greater than 0");
        }

        public static AppException CreateProductFail()
        {
            return new AppException(HttpStatusCode.InternalServerError, "create new product failed.");
        }

        public static AppException ProductNotFound(Guid id)
        {
            string message = $"A product with the ID {id} not found.";
            return new AppException(HttpStatusCode.NotFound, message);
        }

        //****custom category exception group****

        public static AppException CategoryAlreadyExixst(string name)
        {
            string message = $"A category with the name {name} already exists.";
            return new AppException(HttpStatusCode.BadRequest, message);
        }
    }
}
