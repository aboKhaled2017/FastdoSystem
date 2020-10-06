using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class DeleteStockClassForPharmaModel
    {
        private string _DeletedClassId = null;
        private string _ReplaceClassId =null;
        [Required]
        public string DeletedClassId 
        {
            get { return _DeletedClassId; }
            set { _DeletedClassId = value; }
        }
        public string ReplaceClassId
        {
            set { _ReplaceClassId = value; }
        }
    
        public Guid getDeletedClassId 
        { 
            get { return !string.IsNullOrEmpty(_DeletedClassId) ? Guid.Parse(_DeletedClassId) : Guid.Empty; }
        }
        public Guid getReplaceClassId
        {
            get { return !string.IsNullOrEmpty(_ReplaceClassId) ? Guid.Parse(_ReplaceClassId) : Guid.Empty; }
        }
    }
}
