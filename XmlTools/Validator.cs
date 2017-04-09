using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace XmlTools
{
    public class Validator
    {
        private string _targetNamespace = "http://library.by/catalog";
        private string _schemaPath;

        public Validator(string schemaPath)
        {
            _schemaPath = schemaPath;
        }

        public Validator(string targetNamespace, string schemaPath)
        {
            _targetNamespace = targetNamespace;
            _schemaPath = schemaPath;
        }

        public List<string> DetectErrors(string filePath)
        {
            var errors = new List<string>();
            XmlReader reader = XmlReader.Create(filePath, GetSettings(errors));
            while (reader.Read()) ;

            return errors;
        }

        private XmlReaderSettings GetSettings(List<string> errors)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(_targetNamespace, _schemaPath);
            settings.ValidationEventHandler += delegate (object sender, ValidationEventArgs e)
                {
                    errors.Add(string.Format("[Line:{0}, Position:{1}] {2}", e.Exception.LineNumber, e.Exception.LinePosition, e.Message));
                };
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationType = ValidationType.Schema;

            return settings;
        }
    }
}
