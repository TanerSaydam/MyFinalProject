using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {        
        IDataResult<IList<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GetById(int productId);
        IDataResult<IList<Product>> GetAll();
        IDataResult<IList<Product>> GetListByCategory(int categoryId);
        IResult Add(Product product);
        IResult Update(Product product);
        IResult Delete(Product product);
        IResult TranscaptionalOperation(Product product);
    }
}
