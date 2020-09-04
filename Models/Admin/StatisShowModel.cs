using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Models
{
    
    public class StatisShowModel
    {
        public PharmaciesStatis pharmacies { get; set; }
        public StocksStatis stocks { get; set; }
        public RequestsStatis requests { get; set; }
        public LzDrugsStatis lzDrugs { get; set; }
    }
    public class PharmaciesStatis
    {
        public int total { get; set; }
        public int accepted { get; set; }
        public int pending { get; set; }
        public int disabled { get; set; }
        public int rejected { get; set; }
    }
    public class StocksStatis
    {
        public int accepted { get; set; }
        public int pending { get; set; }
        public int disabled { get; set; }
        public int total { get; set; }
        public int rejected { get; set; }
    }
    public class RequestsStatis
    {
        public int total { get; set; }
        public int done { get; set; }
        public int pending { get; set; }
        public int cancel { get; set; }
    }
    public class LzDrugsStatis
    {
        public int total { get; set; }
    }
}
