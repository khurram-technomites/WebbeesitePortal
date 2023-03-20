using System;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class ClientContentViewModel
    {
        public List<ClientContentMediaViewModel> ClientContentMediaViewModels { get; set; }
        public List<ClientDomainSuggestionsViewModel> ClientDomainSuggestionsViewModels { get; set; }
        public List<ClientEmailsViewModel> clientEmailsViewModels { get; set; }

        public long ClientId { get; set; }

        public bool IsDomainRequired { get; set; }
        public string DomainProvider { get; set; }

        public string Website { get; set; }
        public string DomainUserId { get; set; }
        public string DomainPassword { get; set; }
    }
}
