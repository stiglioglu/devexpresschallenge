using Challenge.DatabaseLogicLayer;
using Challenge.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Challenge.BusinessLogicLayer
{
    public class BLL
    {

        DLL dll;

        public BLL() 
        {
            dll = new DLL();
        }

        public bool CheckLicenceKey()
        {
            string value = "";
            string jsonFilePath = "licence.json";
            string jsonString = File.ReadAllText(jsonFilePath);
            JObject jsonObject = JObject.Parse(jsonString);
            
            foreach (var property in jsonObject)
            {
                value = property.Value.ToString();
                if (value == "demo")
                {
                    return true;
                }
            }

            if (dll.CheckLicenceDate(value))
            {
                return true;
            }
            return false;
        }

        public bool AddNewLicence(string key, string newDate)
        {
            if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(newDate))
            {
                DateTime expiry = DateTime.Parse(newDate);
                if (expiry < DateTime.Today)
                {
                    return false;
                }
                Licence licence = new Licence();
                licence.Key = key;
                licence.Create = DateTime.Now;
                licence.Expiry = expiry;
                return dll.AddNewLicence(licence);
            }
            else
            {
                return false;
            }
            
        }

        public List<Licence> GetAllLicences()
        {
            return dll.GetAllLicences();
        }

        public bool UpdateLicence(string id, string key, string newDate)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(newDate))
            {
                int licenceId = int.Parse(id);
                DateTime expiry = DateTime.Parse(newDate);
                if (expiry < DateTime.Today)
                {
                    return false;
                }
                Licence licence = new Licence();
                licence.Key = key;
                licence.Expiry = expiry;
                return dll.UpdateLicence(licenceId, licence);
            }
            else
            {
                return false;
            }
        }

        public bool DeleteLicence(int licenceId)
        { 
            return dll.DeleteLicence(licenceId);
        }

        public bool AddNewDocument(string name, string path)
        { 
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path))
            {
                return false;
            }
            Document document = new Document();
            document.name = name;
            document.path = path;
            return dll.AddNewDocument(document);
        }

        public bool DeleteDocumentByPath(string path)
        {
            return dll.DeleteDocumentByPath(path);
        }

    }
}
