using System;

namespace SparkAuto.Models.ViewModel{

    public class ReportsViewModel{

        public enum GrapType { line,pie,radar,bubble,scatter,area,doughnut,bar  }

        public string mospopularservicename {get;set;}
        public int mostpopularservicecount {get;set;}

        public string mostvalueclientname {get;set;}
        public double mostvalueclientmoney {get;set;}
        public double totaltoday {get;set;}
        public double totalmonth {get;set;}

        
        public string graphlabels {get;set;}
        public string graphvalues {get;set;}
        public string axislabel {get;set;}
        
        public GrapType type {get;set;}

    }

}