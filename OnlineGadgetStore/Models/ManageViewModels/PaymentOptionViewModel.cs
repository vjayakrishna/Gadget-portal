using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGadgetStore.Models.ManageViewModels
{
    public class PaymentOptionViewModel
    {
        [CreditCard]
        public CreditCardAttribute PaymentOptionList { get; set; }
    }
}
