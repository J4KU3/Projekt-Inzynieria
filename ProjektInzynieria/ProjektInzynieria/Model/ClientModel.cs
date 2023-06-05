using ProjektInzynieria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektInzynieria.Model
{
  public class ClientModel: Clients
    {
        public ClientModel(Clients Copyclients)
        {
            ClientID = Copyclients.ClientID;
            CFName = Copyclients.CFName;
            CLName = Copyclients.CLName;
            Phone = Copyclients.Phone;
            Zipcode = Copyclients.Zipcode;
            City = Copyclients.City;
            Street = Copyclients.Street;
            ApartmentNumber = Copyclients.ApartmentNumber;

        }
    }
}
