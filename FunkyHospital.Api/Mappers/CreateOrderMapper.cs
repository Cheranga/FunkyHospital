using System.IO;
using AutoMapper;
using FunkyHospital.Api.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FunkyHospital.Api.Mappers
{
    public class CreateOrderMapper : ITypeConverter<HttpRequest, CreateOrderDto>
    {
        public CreateOrderDto Convert(HttpRequest request, CreateOrderDto dto, ResolutionContext context)
        {
            using (var reader = new StreamReader(request.Body))
            {
                var requestBody = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(requestBody))
                {
                    return null;
                }

                var model = JsonConvert.DeserializeObject<CreateOrderDto>(requestBody);
                return model;
            }
        }
    }
}