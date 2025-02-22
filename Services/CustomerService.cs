using CustomerManagerWeb.Controllers;
using CustomerManagerWeb.Models;
using CustomerManagerWeb.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json.Serialization;

namespace CustomerManagerWeb.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _BaseUrl;
        private readonly ISessionService _sessionService;
        public CustomerService(IOptions<ServiceSettings> BaseUrl, ILogger<HomeController> logger, ISessionService sessionService)
        {
            _logger = logger;
            _BaseUrl = $"{BaseUrl.Value.BaseUrl}/api";
            _sessionService = sessionService;
        }
        #region :: Private Methods ::
        private bool CustomerValidate(Customer customer)
        {
            if (customer == null ||
                string.IsNullOrEmpty(customer.Name) ||
                string.IsNullOrEmpty(customer.Email))
                return false;

            return true;
        }

        private bool AddressValidate(Address address)
        {
            if (address == null ||
                string.IsNullOrEmpty(address.Name) ||
                address.CustomerId == 0)
                return false;

            return true;
        }

        #endregion

        #region :: Customer ::

        /// <summary>
        /// Criação de novo cliente
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public MessageResponse<Customer> Create(Customer customer)
        {
            if (!CustomerValidate(customer))
                return new MessageResponse<Customer> { Success = false, Message = "Cliente inválido. Por favor, revisar o cadastro!" };

            var entity = new MessageResponse<Customer>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest("/customer", Method.Post);
                request.AlwaysMultipartFormData = true;
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                // Adiciona o JSON serializado como um campo de formulário
                request.AddParameter("customerRequest", JsonConvert.SerializeObject(customer), ParameterType.RequestBody);

                // Adiciona o arquivo (ImageFile) do customerRequest
                if (customer.ImageFile != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        customer.ImageFile.CopyTo(stream);
                        var fileBytes = stream.ToArray(); 
                        request.AddFile("companyLogo", fileBytes, customer.ImageFile.FileName, customer.ImageFile.ContentType);
                    }
                }

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<Customer>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(Create)} ao tentar cadastrar o cliente {JsonConvert.SerializeObject(customer)} exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        /// <summary>
        /// Listagem de todos os clientes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageResponse<List<Customer>>> GetAllAsync()
        {
            var entity = new MessageResponse<List<Customer>>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest("/Customer", Method.Get);

                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                // Chama a requisição de forma assíncrona
                var response = await client.ExecuteAsync(request);

                if (response == null || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<List<Customer>>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(GetAllAsync)} ao tentar buscar os clientes exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        /// <summary>
        /// Consulta de um cliente pelo ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MessageResponse<Customer> GetById(int id)
        {
            var entity = new MessageResponse<Customer>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest($"/Customer/{id}", Method.Get);
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<Customer>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(GetById)} ao tentar buscar o cliente exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        /// <summary>
        /// Atualizar informações um cliente
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MessageResponse<Customer> Update(Customer customer)
        {
            if (!CustomerValidate(customer))
                return new MessageResponse<Customer> { Success = false, Message = "Cliente inválido. Por favor, revisar o cadastro!" };

            var entity = new MessageResponse<Customer>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest($"/Customer/{customer.Id}", Method.Put);
                request.AlwaysMultipartFormData = true;
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                request.AddParameter("customerRequest", JsonConvert.SerializeObject(customer), ParameterType.RequestBody);

                if (customer.ImageFile != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        customer.ImageFile.CopyTo(stream);
                        var fileBytes = stream.ToArray();
                        request.AddFile("companyLogo", fileBytes, customer.ImageFile.FileName, customer.ImageFile.ContentType);
                    }
                }

                var response = client.Execute(request);
                
                if (response == null || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<Customer>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(Update)} ao tentar atualizar o cliente {JsonConvert.SerializeObject(customer)} exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        /// <summary>
        /// Remover um cliente.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MessageResponse<object> Delete(int id)
        {
            if (id == 0)
                return new MessageResponse<object> { Success = false, Message = "Cliente não encontrado para exclusão!" };

            var entity = new MessageResponse<object>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest($"/Customer/{id}", Method.Delete);
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<object>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(Delete)} ao tentar remover o cliente id {id} exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        /// <summary>
        /// Listagem de TOP 3 Clientes com mais logradouros.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MessageResponse<List<Customer>> GetTopCustomers()
        {
            var entity = new MessageResponse<List<Customer>>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest("/Customer/GetTopCustomers", Method.Get);
                var token = _sessionService.GetToken();
                request.AddHeader("Authorization", $"Bearer {token}");

                var response = client.Execute(request);

                if (response == null || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<List<Customer>>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch {nameof(GetTopCustomers)} ao tentar buscar o top 3 clientes exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        #endregion

        #region :: Address ::

        public MessageResponse<Address> GetAddressById(int id)
        {
            var entity = new MessageResponse<Address>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest($"/Customer/Address/{id}", Method.Get);
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<Address>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(GetAddressById)} ao tentar buscar o endereço exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        public MessageResponse<Address> CreateAddress(Address address)
        {
            if (!AddressValidate(address))
                return new MessageResponse<Address> { Success = false, Message = "Endereço inválido. Por favor, revisar o cadastro!" };

            var entity = new MessageResponse<Address>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest("/Customer/Address", Method.Post);
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(address), ParameterType.RequestBody);

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<Address>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(CreateAddress)} ao tentar cadastrar um novo endereço address {JsonConvert.SerializeObject(address)} exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        public MessageResponse<Address> UpdateAddress(Address address)
        {
            if (!AddressValidate(address))
                return new MessageResponse<Address> { Success = false, Message = "Endereço inválido. Por favor, revisar o cadastro!" };

            var entity = new MessageResponse<Address>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest($"/Customer/Address/{address.Id}", Method.Put);
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(address), ParameterType.RequestBody);

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<Address>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(UpdateAddress)} ao tentar atualizar um endereço  address {JsonConvert.SerializeObject(address)} exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        public MessageResponse<object> DeleteAddress(int id)
        {
            if (id == 0)
                return new MessageResponse<object> { Success = false, Message = "Endereço não encontrado para exclusão!" };

            var entity = new MessageResponse<object>();

            try
            {
                var client = new RestClient(_BaseUrl);
                var request = new RestRequest($"/Customer/Address/{id}", Method.Delete);
                var token = _sessionService.GetToken();
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", $"Bearer {token}");

                var response = client.Execute(request);

                if (response == null)
                {
                    entity.Success = false;
                    entity.Message = $"Falha ao tentar se comunicar com o serviço!";
                }
                else
                    entity = JsonConvert.DeserializeObject<MessageResponse<object>>(response.Content);
            }
            catch (Exception ex)
            {
                entity.Success = false;
                entity.Message = $"Falha ao tentar se comunicar com o serviço! Entre em contato com o suporte!";
                _logger.LogError($"Erro catch Customer {nameof(DeleteAddress)} ao tentar remover o endreço de id {id} exception {JsonConvert.SerializeObject(ex)}");
            }

            return entity;
        }

        #endregion
    }
}
