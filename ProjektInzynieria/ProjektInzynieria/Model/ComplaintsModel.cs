﻿using ProjektInzynieria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektInzynieria.Model
{
    public class ComplaintsModel: Complaints
    {
        public ComplaintsModel(Complaints Copycomplaints)
        {
            IDComplaint = Copycomplaints.IDComplaint;
            OrderID = Copycomplaints.OrderID;
            Reason = Copycomplaints.Reason;
        }
    }
}
