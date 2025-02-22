using CustomerManagerWeb.Models;
using CustomerManagerWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomerManagerWeb.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<HomeController> _logger;
        public CustomerController(ICustomerService customerService, ILogger<HomeController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        #region :: Customer ::

        public async Task<ActionResult> Index(string pFilter, bool btnFilterClick = false)
        {
            var response = new MessageResponse<List<Customer>>();

            if (string.IsNullOrEmpty(pFilter) && !btnFilterClick)
            {
                response = await _customerService.GetAllAsync();
                return View(response);
            }
            else if (string.IsNullOrEmpty(pFilter) && btnFilterClick)
            {
                response = await _customerService.GetAllAsync();
                return PartialView("List", response.Data);
            }
            else
            {
                response = await _customerService.GetAllAsync();
                response.Data = response.Data.Where(x =>
                                                        x.Name.ToLower().Contains(pFilter.ToLower()) ||
                                                        x.Email.ToLower().Contains(pFilter.ToLower())).ToList();
                return PartialView("List", response.Data);
            }
        }

        public ActionResult Details(int id)
        {

            var response = _customerService.GetById(id);

            if (response.Success)
                return View(response.Data);
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Create()
        {
            return View(new Customer());
        }

        [HttpPost]
        public ActionResult Create([Bind] Customer customer, IFormFile ImageFile)
        {
            try
            {
                ModelState.Remove(nameof(customer.ImageFile));
                if (string.IsNullOrEmpty(customer.CompanyLogo))
                    customer.CompanyLogo = string.Empty;

                if (ModelState.IsValid)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                        customer.ImageFile = ImageFile;

                    var response = _customerService.Create(customer);

                    if (response.Success)
                    {
                        TempData["SuccessMessage"] = response.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = response.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar cliente: {ex.Message}");

                TempData["ErrorMessage"] = "Ocorreu um erro ao criar o cliente. Por favor, tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(int id)
        {
            var response = _customerService.GetById(id);

            if (response.Success)
                return View(response.Data);
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public ActionResult Edit(Customer customer, IFormFile ImageFile)
        {
            try
            {
                ModelState.Remove(nameof(customer.ImageFile));
                if (string.IsNullOrEmpty(customer.CompanyLogo))
                    customer.CompanyLogo = string.Empty;

                if (ModelState.IsValid)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                        customer.ImageFile = ImageFile;

                    var response = _customerService.Update(customer);

                    if (response.Success)
                    {
                        TempData["SuccessMessage"] = response.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = response.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                    return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar o cliente: {ex.Message}");

                TempData["ErrorMessage"] = "Ocorreu um erro ao editar o cliente. Por favor, tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var response = _customerService.Delete(id);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao remover o cliente: {ex.Message}");

                TempData["ErrorMessage"] = "Ocorreu um erro ao remover o cliente. Por favor, tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region :: Address ::
        public ActionResult CreateAddress(int Id)
        {
            var address = new Address();
            address.CustomerId = Id;

            return PartialView(address);
        }

        [HttpPost]
        public ActionResult CreateAddress([Bind] Address address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _customerService.CreateAddress(address);

                    if (response.Success)
                    {
                        TempData["SuccessMessage"] = response.Message;
                        return RedirectToAction("Details", new { id = response.Data.CustomerId });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = response.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(address);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar o endereço: {ex.Message}");

                TempData["ErrorMessage"] = "Ocorreu um erro ao criar o endereço. Por favor, tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult DetailsAddress(int Id, int CustomerId)
        {

            var response = _customerService.GetAddressById(Id);

            if (response.Success)
                return View(response.Data);
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Details", new { id = CustomerId });
            }
        }

        public ActionResult EditAddress(int Id, int CustomerId)
        {
            var response = _customerService.GetAddressById(Id);

            if (response.Success)
                return View(response.Data);
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Details", new { id = CustomerId });
            }
        }

        [HttpPost]
        public ActionResult EditAddress(Address address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _customerService.UpdateAddress(address);

                    if (response.Success)
                    {
                        TempData["SuccessMessage"] = response.Message;
                        return RedirectToAction("Details", new { id = address.CustomerId });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = response.Message;
                        return RedirectToAction("Details", new { id = address.CustomerId });
                    }
                }
                else
                    return View(address);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar o endereço: {ex.Message}");
                TempData["ErrorMessage"] = "Ocorreu um erro ao editar o endereço. Por favor, tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult DeleteAddress(int Id, int CustomerId)
        {
            try
            {
                var response = _customerService.DeleteAddress(Id);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return RedirectToAction("Details", new { id = CustomerId });
                }
                else
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction("Details", new { id = CustomerId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao remover o endereço: {ex.Message}");

                TempData["ErrorMessage"] = "Ocorreu um erro ao remover o endereço. Por favor, tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
    }
}
