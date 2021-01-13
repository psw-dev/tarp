using PSW.ITMS.Service.Command;
using System.Text.Json;

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

        public ApiStrategy(CommandRequest request) : base(request)
        {
            this.Command = request;
            this.IsValidated = false;

            // Get Json Data From Command
            var jsonString = this.Command.data.GetRawText();
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
    }
}
