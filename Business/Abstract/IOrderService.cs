﻿using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IList<Order> GetAll();
        Order Get(int id);
        void Add(Order order);
        void Update(Order order);
        void Delete(Order order);
    }
}