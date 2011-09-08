using System.Web.Routing;
using Microsoft.ApplicationServer.Http.Activation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Rest.Server.Mvc.App_Start.WebApi), "Start")]

namespace Rest.Server.Mvc.App_Start {
    public static class WebApi {
        public static void Start() {
            // TODO: change "MyModel" to desired route segment
            RouteTable.Routes.MapServiceRoute<Resources.ContactResource>("api/contacts");
        }
    }
}