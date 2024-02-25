using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Challenge.Entities;
using DevExpress.Xpo.Metadata;

namespace Challenge.DatabaseLogicLayer
{
    public class DLL
    {

        IDataLayer dataLayer;
        Session session;

        public DLL() 
        {
            string connectionString = 
                MySqlConnectionProvider.GetConnectionString("localhost", "root", "1234", "winform");
            dataLayer = 
                XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);
            session = new Session(dataLayer);
        }

        public bool CheckLicence(string key)
        {
            bool isKeyExists = session.Query<Licence>().Any(x => x.Key == key);
            if (isKeyExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckLicenceDate(string key)
        {
            if (!CheckLicence(key))
            {
                return false;
            }
            DateTime? expiryDate = session.Query<Licence>()
                            .Where(x => x.Key == key)
                            .Select(x => x.Expiry)
                            .FirstOrDefault();

            if (expiryDate < DateTime.Today)
            {
                return false;
            }

            return true;
        }

        public bool AddNewLicence(Licence newLicence)
        {
            if (CheckLicence(newLicence.Key))
            {
                return false;
            }

            // Licence tablosunu oluştur
            XPClassInfo classInfo = session.GetClassInfo(typeof(Licence));
            if (!classInfo.IsPersistent)
            {
                session.CreateObjectTypeRecords(classInfo);
            }

            // İşlemi başlat
            session.BeginTransaction();

            Licence licence = new Licence(session);
            licence.Key = newLicence.Key;
            licence.Create = newLicence.Create;
            licence.Expiry = newLicence.Expiry;
            session.Save(licence);

            // İşlemi sona erdir
            session.CommitTransaction();

            return true;
            
        }

        // Lisansları listeleme
        public List<Licence> GetAllLicences()
        {
            List<Licence> licences = new List<Licence>();


            XPQuery<Licence> licencesQuery = session.Query<Licence>();
            foreach (Licence licence in licencesQuery)
            {
                licences.Add(licence);
            }
            

            return licences;
        }

        // Lisans güncelleme
        public bool UpdateLicence(int licenceId, Licence updatedLicence)
        {
            Licence licence = session.GetObjectByKey<Licence>(licenceId);
            if (licence != null)
            {
                licence.Key = updatedLicence.Key;
                licence.Expiry = updatedLicence.Expiry;
                session.Save(licence);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Lisans silme
        public bool DeleteLicence(int licenceId)
        {

            Licence licence = session.GetObjectByKey<Licence>(licenceId);
            if (licence != null)
            {
                session.Delete(licence);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool AddNewDocument(Document newDocument)
        {
            XPClassInfo classInfo = session.GetClassInfo(typeof(Document));
            if (!classInfo.IsPersistent)
            {
                session.CreateObjectTypeRecords(classInfo);
            }

            session.BeginTransaction();

            Document document = new Document(session);
            document.name = newDocument.name;
            document.path = newDocument.path;
            session.Save(document);

            session.CommitTransaction();

            return true;
        }

        public bool DeleteDocumentByPath(string path)
        {
            Document document = session.Query<Document>().FirstOrDefault(d => d.path == path);

            if (document != null)
            {
                session.BeginTransaction();
                session.Delete(document);
                session.CommitTransaction();
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
