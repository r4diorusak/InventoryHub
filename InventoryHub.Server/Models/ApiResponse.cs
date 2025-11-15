using System;
using System.Collections.Generic;

namespace InventoryHub.Server.Models
{
    /// <summary>
    /// Generic API response wrapper for consistent JSON structure across all endpoints.
    /// This ensures standardized response handling on the client side.
    /// Related to: 5 pts - JSON Structures (Consistent API responses)
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the request was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// HTTP status code of the response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message describing the result of the operation
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The actual data returned by the API
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Validation errors (if any)
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; }

        /// <summary>
        /// Timestamp of when the response was generated
        /// </summary>
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
            Errors = new Dictionary<string, List<string>>();
        }

        public static ApiResponse<T> CreateSuccess(T data, string message = "Request completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = data,
                Errors = new Dictionary<string, List<string>>()
            };
        }

        public static ApiResponse<T> CreateError(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Data = default(T),
                Errors = new Dictionary<string, List<string>>()
            };
        }
    }
}
