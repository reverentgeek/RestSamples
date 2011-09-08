using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace Rest.Server.Mvc.Resources
{
    // TODO: Use Visual Studio refactoring to rename "Contact" to your desired type.
    //       Then you can move the model to its own file.
    public class Contact
    {
        public int Id { get; set; }
		public string Name { get; set; }
    }

    [ServiceContract]
    // TODO: Use Visual Studio refactoring to rename "ContactResource" to desired name.
    public class ContactResource
    {
        // TODO: replace with your own "real" Repository implementation
        private static readonly Dictionary<int, Contact> repository = new Dictionary<int, Contact>();

        [WebGet(UriTemplate = "{id}")]
        public HttpResponseMessage<Contact> Get(int id)
        {
            Contact item;
            if (!repository.TryGetValue(id, out item))
            {
                var notFoundResponse = new HttpResponseMessage();
                notFoundResponse.StatusCode = HttpStatusCode.NotFound;
                notFoundResponse.Content = new StringContent("Item not found");
                throw new HttpResponseException(notFoundResponse);
            }
            var response = new HttpResponseMessage<Contact>(item);

            // set it to expire in 5 minutes
            response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(30));
            return response;
        }

        [WebInvoke(UriTemplate = "", Method = "POST")]
        public HttpResponseMessage<Contact> Post(Contact item)
        {
            item.Id = (repository.Keys.Count == 0 ? 1 : repository.Keys.Max() + 1);
            repository.Add(item.Id, item);

            var response = new HttpResponseMessage<Contact>(item);
            response.StatusCode = HttpStatusCode.Created;
            response.Headers.Location = new Uri("/api/contacts/" + item.Id, UriKind.Relative);
            return response;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public Contact Put(int id, Contact item)
        {
            repository[id] = item;
            return item;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public Contact Delete(int id)
        {
            var deleted = repository[id];
            repository.Remove(id);
            return deleted;
        }
    }
}