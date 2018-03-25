﻿using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace api_dating_app.Helpers
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public static class ExtensionsHelper
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(
            this HttpResponse httpResponse,
            int currentPage,
            int itemsPerPage,
            int totalItems,
            int totalPages
        )
        {
            var paginationHeader = new PaginationHeaderHelper(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter =
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            httpResponse.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;

            return date.AddYears(age) > DateTime.Today ? --age : age;
        }
    }
}