using Back_end.Models;
using Back_end.Models.DTOs;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDocumentRepository _documentService;

        /// <summary>
        /// Initializes a new instance of the CustomerService class with the provided CustomerRepository.
        /// </summary>
        /// <param name="cr">The CustomerRepository used for customer-related operations.</param>
        public CustomerService(ICustomerRepository cr, IDocumentRepository ds)
        {
            _customerRepository = cr;
            _documentService = ds;
        }


        /// <summary>
        /// Retrieves a customer by their unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID if found; otherwise, returns null.</returns>
        public CustomerDTO GetById(int id)
        {
            Customer cus = _customerRepository.GetById(id);
            var customerDTO = new CustomerDTO
            {
                Email = cus.Email,
                Name = cus.Name
            };

            return customerDTO;
        }


        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
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


        /// <summary>
        /// Filters and retrieves a collection of customers based on a search field.
        /// </summary>
        /// <param name="searchfield">The search field to filter customers by.</param>
        /// <returns>A collection of customers matching the search criteria or an empty list if the searchfield is null or whitespace.</returns>
        public IEnumerable<Customer> FilterAll(string searchfield)
        {
            if (string.IsNullOrWhiteSpace(searchfield))
            {
                return new List<Customer>();
            }
            return _customerRepository.FilterAll(searchfield);
        }


        public void Put(CustomerDocumentDTO customerDocumentDTO)
        {
            Customer oldCustomer = _customerRepository.GetById(customerDocumentDTO.CustomerId);
            IEnumerable<Document> documents = _documentService.GetByCustomerId(customerDocumentDTO.CustomerId);
            List<Customer> allCustomers = _customerRepository.GetAll();

            if (documents.Count() > 1)
            {
                AddNewCustomer(customerDocumentDTO);
            }
            else
            {
                UpdateCustomer(allCustomers, customerDocumentDTO, oldCustomer);
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

        private void UpdateCustomer(List<Customer> allCustomers, CustomerDocumentDTO customerDocumentDTO, Customer oldCustomer)
        {
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