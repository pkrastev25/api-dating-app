using System;
using Microsoft.AspNetCore.Http;

namespace api_dating_app.Helpers
{
    /// <summary>
    /// Helper class used to manage expensions within the application.
    /// </summary>
    public static class ExtensionsHelper
    {
        /// <summary>
        /// Adds an 'Application-error' to the header of the response together with the
        /// error message itself.
        /// </summary>
        /// 
        /// <param name="response">The response which will contain the application error</param>
        /// <param name="message">The message of the application error</param>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Calculates the age based on a starting date.
        /// </summary>
        /// 
        /// <param name="date">Starting date used to calculate the age</param>
        /// <returns>The age of the user</returns>
        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;

            return date.AddYears(age) > DateTime.Today ? --age : age;
        }
    }
}