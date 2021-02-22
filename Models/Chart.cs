using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SparkAuto.Models
{

    public enum ChartType { line,pie,radar,bubble,scatter,area,doughnut,bar,horizontalBar }

    public class Chart{

        public List<string> labels {get;set;}
        public List<string> values {get;set;}
        public string axislabel {get;set;}
        public ChartType type {get;set;}

        public string getLabels(){
            string formatlabel = "";
            foreach(string x in this.labels){
                formatlabel += x + ",";
            }
            return formatlabel.Substring(0,formatlabel.Length - 1);
        }

        public string getValues(){
            string formatvalues = "";
            foreach(string x in this.values){
                formatvalues += x + ",";
            }
            return formatvalues.Substring(0,formatvalues.Length - 1);
        }

        public string getType(){
            return type.ToString();
        }


    }
}