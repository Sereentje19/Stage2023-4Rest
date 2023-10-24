using Back_end.Models;
using Back_end.Models.DTOs;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDocumentService _documentService;

        /// <summary>
        /// Initializes a new instance of the CustomerService class with the provided CustomerRepository.
        /// </summary>
        /// <param name="cr">The CustomerRepository used for customer-related operations.</param>
        public CustomerService(ICustomerRepository customerRepository, IDocumentService documentService)
        {
            _customerRepository = customerRepository;
            _documentService = documentService;
        }

        public (IEnumerable<object>, Pager) GetAll(int page, int pageSize)
        {
            var customers = _customerRepository.GetAll();

            int skipCount = Math.Max(0, (page - 1) * pageSize);
            var pager = new Pager(customers.Count(), page, pageSize);

            var pagedCustomers = customers
                .Skip(skipCount)
                .Take(pageSize)
                .Select(cus => new
                {
                    cus.Name,
                    cus.Email,
                    cus.CustomerId
                })
                .ToList();

            return (pagedCustomers.Cast<object>(), pager);
        }


        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by.</param>
        /// <returns>A collection of customers matching the search criteria or an empty list if the searchfield is null or whitespace.</returns>
        public IEnumerable<Customer> GetFilteredCustomers(string searchfield)
        {
            return _customerRepository.GetFilteredCustomers(searchfield);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public CustomerDTO GetById(int id)
        {
            Customer cus = _customerRepository.GetById(id);
            return new CustomerDTO
            {
                Email = cus.Email,
                Name = cus.Name
            };
        }

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer entity to be added.</param>
        /// <returns>The unique identifier (ID) of the added customer.</returns>
        public int Post(Customer customer)
        {
            return _customerRepository.Add(customer);
        }

        public void Put(CustomerDocumentDTO customerDocumentDTO)
        {
            if (string.IsNullOrWhiteSpace(customerDocumentDTO.Name) || string.IsNullOrEmpty(customerDocumentDTO.Email))
            {
                throw new Exception("Naam of email is leeg.");
            }

            Customer existingCustomer = _customerRepository.GetById(customerDocumentDTO.CustomerId);

            //check if the customer data changed 
            if (existingCustomer.Email != customerDocumentDTO.Email || existingCustomer.Name != customerDocumentDTO.Name)
            {
                IEnumerable<Document> documents = _documentService.GetByCustomerId(customerDocumentDTO.CustomerId);

                if (documents.Count() > 1)
                {
                    AddNewCustomer(customerDocumentDTO);
                }
                else
                {
                    UpdateCustomer(customerDocumentDTO, existingCustomer);
                }
            }
        }

        private void AddNewCustomer(CustomerDocumentDTO customerDocumentDTO)
        {
            Customer cus = new Customer
            {
                Email = customerDocumentDTO.Email,
                Name = customerDocumentDTO.Name
            };

            int customerId = _customerRepository.Add(cus);
            _documentService.UpdateCustomerId(customerId, customerDocumentDTO.DocumentId);
        }

        private void UpdateCustomer(CustomerDocumentDTO customerDocumentDTO, Customer oldCustomer)
        {
            List<Customer> allCustomers = _customerRepository.GetAll();

            var matchingCustomer = allCustomers.FirstOrDefault(c =>
                                c.Email == customerDocumentDTO.Email &&
                                c.Name == customerDocumentDTO.Name);

            if (matchingCustomer != null)
            {
                customerDocumentDTO.CustomerId = matchingCustomer.CustomerId;
                _documentService.UpdateCustomerId(matchingCustomer.CustomerId, customerDocumentDTO.DocumentId);
                _customerRepository.Delete(oldCustomer);
            }
            else
            {
                oldCustomer.Email = customerDocumentDTO.Email;
                oldCustomer.Name = customerDocumentDTO.Name;
                _customerRepository.Update(oldCustomer);
            }
        }

    }
}