using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Controllers.Stocks
{
    public class AssignAnotherClassForPharmacyModel
    {
        private string _NewClassId = null;
        private string _OldClassId = null;
        [Required]
        public string NewClassId
        {
            get { return _NewClassId; }
            set { _NewClassId = value; }
        }
        public Guid getNewClassId
        {
            get { return !string.IsNullOrEmpty(_NewClassId) ? Guid.Parse(_NewClassId) : Guid.Empty; }
        }
        public string OldClassId
        {
            get { return _OldClassId; }
            set { _OldClassId = value; }
        }
        public Guid getOldClassId
        {
            get { return !string.IsNullOrEmpty(_OldClassId) ? Guid.Parse(_OldClassId) : Guid.Empty; }
        }
        [Required]
        public string PharmaId { get; set; }

    }
}
