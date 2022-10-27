using PSW.ITMS.Service.Command;
using PSW.Lib.Logs;
using System.Net;
using System.Text.Json;
using System.Linq;
using FluentValidation;

namespace PSW.ITMS.Service.Strategies
{
    /// <summary>
    /// Generic API strategy written specifically for an API call
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class ApiStrategy<T1, T2> : Strategy
    {
        /// <summary>
        /// API request object
        /// </summary>
        public T1 RequestDTO { get; set; }

        /// <summary>
        /// API response object
        /// </summary>
        public T2 ResponseDTO { get; set; }

        /// <summary>
        /// Strategy name
        /// </summary>
        public string StrategyName { get; set; }
        public string MethodID { get; set; }
        public AbstractValidator<T1> Validator { get; set; }

        public override bool Validate()
        {
            Log.Information("|{StrategyName}|{MethodID}| Request DTO: {@RequestDTO}", StrategyName, MethodID, RequestDTO);

            if (Validator != null)
            {
                string message = "validated";
                var results = Validator.Validate(RequestDTO);
                this.IsValidated = results.IsValid;

                if (!results.IsValid)
                {
                    var errors = results.Errors.Select(x => x.ErrorMessage).ToList();
                    message = errors.Aggregate((i, j) => i + "\n" + j);
                }

                Log.Information("|{StrategyName}|{MethodID}| Validation Messages: {message}, Is Validated: {IsValidated}", StrategyName, MethodID, message, IsValidated);
                this.ValidationMessage = message;

                return this.IsValidated;
            }

            IsValidated = true;
            return this.IsValidated;
        }

        public ApiStrategy(CommandRequest request) : base(request)
        {
            Command = request;
            IsValidated = false;

            StrategyName = GetType().Name;
            MethodID = request.methodId;

            // Get Json Data From Command
            var jsonString = Command.data.GetRawText();
            // Deserialize Json to DTO
            RequestDTO = JsonSerializer.Deserialize<T1>(jsonString);
        }

        public CommandReply OKReply(string message = "success")
        {
            Reply.code = "200"; // OK
            Reply.message = message;
            Reply.data = JsonDocument.Parse(JsonSerializer.Serialize(ResponseDTO)).RootElement;
            return Reply;
        }

        public CommandReply BadRequestReply(string message, System.Exception exception, string shortDescription, string validationMessage)
        {
            Log.Error("|{0}|{1}| Exception Occurred {2}", StrategyName, MethodID, " message: " + message + " exception: " + exception);

            Reply.code = "400"; // Bad Request | Error
            Reply.message = message;
            Reply.shortDescription = shortDescription;
            Reply.fullDescription = validationMessage;
            Reply.exception = null;
            Reply.data = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(new { })).RootElement;
            return Reply;
        }

        public CommandReply BadRequestReply(string message)
        {
            Log.Error("|{0}|{1}| Exception Occurred {2}", StrategyName, MethodID, "message : " + message);

            Reply.code = "400"; // Bad Request | Error
            Reply.message = message;
            Reply.exception = null;
            Reply.data = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(new { })).RootElement;
            return Reply;
        }

        public CommandReply NotFoundReply(string message = "Record Not Found")
        {
            Reply.code = "404";
            Reply.message = message;
            return Reply;
        }

        public CommandReply InternalServerErrorReply(System.Exception exception, string message = "Internal Server Error")
        {
            Reply.code = "500"; // Internal Server Error | Error
            Reply.message = message;
            Reply.exception = exception.ToString();
            Reply.data = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(new { })).RootElement;
            return Reply;
        }

        public CommandReply InternalServerErrorReply(string message)
        {
            Reply.code = ((int)HttpStatusCode.InternalServerError).ToString();  //"500";
            Reply.message = message;
            Reply.data = JsonDocument.Parse(JsonSerializer.Serialize(ResponseDTO)).RootElement;

            Log.Debug("|{0}|{1}| Command reply: {@Reply}", StrategyName, MethodID, Reply);
            Log.Error("|{0}|{1}| API response: {2}", StrategyName, MethodID, Reply.message);
            return Reply;
        }
    }
}
