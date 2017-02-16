using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation
{
    /// <summary>
    /// Various methods for validating xml
    /// </summary>
    public class ValidateXml
    {

        #region General Methods
        /// <summary>
        /// Gets a <see cref="ValidationResult"/> with information a specified xml file is valid
        /// </summary>
        /// <param name="xmlFile">The path to the xml file to validate</param>
        /// <returns>
        /// Returns <see cref="ValidationResult"/> instance. If any errors are prsent thne <see cref="ValidationResult.HasErrors"/> will be set to true.
        /// Alos if there are errors <see cref="ValidationResult.Errors"/> will contain information about the errors.
        /// </returns>
        /// <remarks>
        /// This method will automatically determine if the xml file is a snippit install, Plugin or Profile and validate depending on type and version.
        /// </remarks>
        public static ValidationResult ValidateXmlFile(string xmlFile)
        {
            ValidationResult result = new ValidationResult();
            if (!File.Exists(xmlFile))
            {
                result.Errors.Add(Properties.Resources.ErrorFileNotExist);
                return result;
            }
            try
            {
                ValidationResult SubResult = null;
                XDocument custDoc = XDocument.Load(xmlFile);
                XmlKindEnum XmlKind = ValidateXml.GetXmlType(custDoc.Root);
                switch (XmlKind)
                {
                    case XmlKindEnum.SnippitInstl:
                        SubResult = ValidateXml.ValidateSnipitInstalDoc(custDoc);
                        break;
                    case XmlKindEnum.Plugin:
                        SubResult = ValidateXml.ValidatePluginDoc(custDoc);
                        break;
                    case XmlKindEnum.Profile:
                        SubResult = ValidateXml.ValidateProfileDoc(custDoc);
                        break;
                    default:
                        SubResult = new ValidationResult();
                        break;
                }
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {

                result.Errors.Add(err.Message);
            }
            return result;
        }

        /// <summary>
        /// Gets a <see cref="ValidationResult"/> with information a specified xml string is valid
        /// </summary>
        /// <param name="xmlData">The xml string to validate</param>
        /// <returns>
        /// Returns <see cref="ValidationResult"/> instance. If any errors are prsent thne <see cref="ValidationResult.HasErrors"/> will be set to true.
        /// Alos if there are errors <see cref="ValidationResult.Errors"/> will contain information about the errors.
        /// </returns>
        /// <remarks>
        /// This method will automatically determine if the xml file is a snippit install, Plugin or Profile and validate depending on type and version.
        /// </remarks>
        public static ValidationResult ValidateXmlString(string xmlData)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                result.Errors.Add(Properties.Resources.ErrorEmptyXmlString);
                return result;
            }
            try
            {
                ValidationResult SubResult = null;
                XDocument custDoc = XDocument.Parse(xmlData);
                XmlKindEnum XmlKind = ValidateXml.GetXmlType(custDoc.Root);
                switch (XmlKind)
                {
                    case XmlKindEnum.SnippitInstl:
                        SubResult = ValidateXml.ValidateSnipitInstalDoc(custDoc);
                        break;
                    case XmlKindEnum.Plugin:
                        SubResult = ValidateXml.ValidatePluginDoc(custDoc);
                        break;
                    case XmlKindEnum.Profile:
                        SubResult = ValidateXml.ValidateProfileDoc(custDoc);
                        break;
                    default:
                        SubResult = new ValidationResult();
                        break;
                }
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {

                result.Errors.Add(err.Message);
            }
            return result;
        }
        #endregion

        #region Snipit Instal
        /// <summary>
        /// Validates SnipitInstal Xml file
        /// </summary>
        /// <param name="xmlFile">The full path to the file to validate</param>
        /// <returns>
        /// <see cref="ValidationResult"/> with <see cref="ValidationResult.HasErrors"/> True if there are errros with the xml;
        /// Othewise <see cref="ValidationResult.HasErrors"/> will be False. Any error messages will be contained in <see cref="ValidationResult.Errors"/>
        /// </returns>
        /// <remarks>Validation occrus against internal XSD file for plugin</remarks>
        public static ValidationResult ValidateSnipitInstalFile(string xmlFile)
        {
            ValidationResult result = new ValidationResult();
            if (!File.Exists(xmlFile))
            {
                result.Errors.Add(Properties.Resources.ErrorFileNotExist);
                return result;
            }
            try
            {
                XDocument custDoc = XDocument.Load(xmlFile);
                ValidationResult SubResult = ValidateXml.ValidateSnipitInstalDoc(custDoc);
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;
        }

       

        /// <summary>
        /// Validates SnipitInstal Xml file
        /// </summary>
        /// <param name="xmlData">String comtaining the xml data for the SnipitInstal</param>
        /// <returns>
        /// <see cref="ValidationResult"/> with <see cref="ValidationResult.HasErrors"/> True if there are errros with the xml;
        /// Othewise <see cref="ValidationResult.HasErrors"/> will be False. Any error messages will be contained in <see cref="ValidationResult.Errors"/>
        /// </returns>
        /// <remarks>Validation occrus against internal XSD file for plugin</remarks>
        public static ValidationResult ValidateSnipitInstalXml(string xmlData)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                result.Errors.Add(Properties.Resources.ErrorEmptyXmlString);
                return result;
            }
            try
            {
                XDocument custDoc = XDocument.Parse(xmlData);
                ValidationResult SubResult = ValidateXml.ValidateSnipitInstalDoc(custDoc);
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {

                result.Errors.Add(err.Message);
            }


            return result;
        }

        private static ValidationResult ValidateSnipitInstalDoc(XDocument custDoc)
        {
            ValidationResult result = new ValidationResult();
            try
            {
                decimal scVer = ValidateXml.GetSchemaVersion(custDoc.Root);
                ValidateXml.ValidateVersionRange(scVer);

                string xsdMarkup = AppCommon.Instance.SchemaSnippitInstal1_1;
                //future versions when there is more then on version of schema
                //if (scVer == 1.1M)
                //{
                //    xsdMarkup = AppCommon.Instance.SchemaSnippitInstal1_1;
                //}
                //else if (scVer == 1.2M)
                //{
                //    xsdMarkup = AppCommon.Instance.SchemaSnippitInstal1_2;
                //}

                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

                custDoc.Validate(schemas, (o, e) =>
                {
                    result.Errors.Add(e.Message);
                });

            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;

        }
        #endregion

        #region Plugin
        /// <summary>
        /// Validates Plugin Xml file
        /// </summary>
        /// <param name="xmlFile">The full path to the file to validate</param>
        /// <returns>
        /// <see cref="ValidationResult"/> with <see cref="ValidationResult.HasErrors"/> True if there are errros with the xml;
        /// Othewise <see cref="ValidationResult.HasErrors"/> will be False. Any error messages will be contained in <see cref="ValidationResult.Errors"/>
        /// </returns>
        /// <remarks>Validation occrus against internal XSD file for plugin</remarks>
        public static ValidationResult ValidatePluginFile(string xmlFile)
        {
            ValidationResult result = new ValidationResult();
            if (!File.Exists(xmlFile))
            {
                result.Errors.Add(Properties.Resources.ErrorFileNotExist);
                return result;
            }
            try
            {
                XDocument custDoc = XDocument.Load(xmlFile);
                ValidationResult SubResult = ValidateXml.ValidatePluginDoc(custDoc);
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {

                result.Errors.Add(err.Message);
            }
                    

            return result;
        }

        /// <summary>
        /// Validates Plugin Xml file
        /// </summary>
        /// <param name="xmlData">String comtaining the xml data for the plugin</param>
        /// <returns>
        /// <see cref="ValidationResult"/> with <see cref="ValidationResult.HasErrors"/> True if there are errros with the xml;
        /// Othewise <see cref="ValidationResult.HasErrors"/> will be False. Any error messages will be contained in <see cref="ValidationResult.Errors"/>
        /// </returns>
        /// <remarks>Validation occrus against internal XSD file for plugin</remarks>
        public static ValidationResult ValidatePluginXml(string xmlData)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                result.Errors.Add(Properties.Resources.ErrorEmptyXmlString);
                return result;
            }
            try
            {
                XDocument custDoc = XDocument.Parse(xmlData);
                ValidationResult SubResult = ValidateXml.ValidatePluginDoc(custDoc);
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;
        }

        private static ValidationResult ValidatePluginDoc(XDocument custDoc)
        {
            ValidationResult result = new ValidationResult();
            try
            {
                decimal scVer = ValidateXml.GetSchemaVersion(custDoc.Root);
                ValidateXml.ValidateVersionRange(scVer);

                string xsdMarkup = AppCommon.Instance.SchemaPlugin1_1;
                //future versions when there is more then on version of schema
                //if (scVer == 1.1M)
                //{
                //    xsdMarkup = AppCommon.Instance.SchemaPlugin1_1;
                //}
                //else if (scVer == 1.2M)
                //{
                //    xsdMarkup = AppCommon.Instance.SchemaPlugin1_2;
                //}

                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

                custDoc.Validate(schemas, (o, e) =>
                {
                    result.Errors.Add(e.Message);
                });

            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;
        }

        #endregion

        #region Profile
            /// <summary>
            /// Validates Profile Xml file
            /// </summary>
            /// <param name="xmlFile">The full path to the file to validate</param>
            /// <returns>
            /// <see cref="ValidationResult"/> with <see cref="ValidationResult.HasErrors"/> True if there are errros with the xml;
            /// Othewise <see cref="ValidationResult.HasErrors"/> will be False. Any error messages will be contained in <see cref="ValidationResult.Errors"/>
            /// </returns>
            /// <remarks>Validation occrus against internal XSD file for plugin</remarks>
        public static ValidationResult ValidateProfileFile(string xmlFile)
        {
            ValidationResult result = new ValidationResult();
            if (!File.Exists(xmlFile))
            {
                result.Errors.Add(Properties.Resources.ErrorFileNotExist);
                return result;
            }
            try
            {
                XDocument custDoc = XDocument.Load(xmlFile);
                ValidationResult SubResult = ValidateXml.ValidateProfileDoc(custDoc);
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;
        }

        /// <summary>
        /// Validates Profile Xml file
        /// </summary>
        /// <param name="xmlData">String comtaining the xml data for the plugin</param>
        /// <returns>
        /// <see cref="ValidationResult"/> with <see cref="ValidationResult.HasErrors"/> True if there are errros with the xml;
        /// Othewise <see cref="ValidationResult.HasErrors"/> will be False. Any error messages will be contained in <see cref="ValidationResult.Errors"/>
        /// </returns>
        /// <remarks>Validation occrus against internal XSD file for plugin</remarks>
        public static ValidationResult ValidateProfileXml(string xmlData)
        {
            ValidationResult result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                result.Errors.Add(Properties.Resources.ErrorEmptyXmlString);
                return result;
            }
            try
            {
                XDocument custDoc = XDocument.Parse(xmlData);
                ValidationResult SubResult = ValidateXml.ValidateProfileDoc(custDoc);
                if (SubResult.HasErrors)
                {
                    result.Errors.AddRange(SubResult.Errors);
                }
            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;
        }

        private static ValidationResult ValidateProfileDoc(XDocument custDoc)
        {
            ValidationResult result = new ValidationResult();
            try
            {
                decimal scVer = ValidateXml.GetSchemaVersion(custDoc.Root);
                ValidateXml.ValidateVersionRange(scVer);

                string xsdMarkup = AppCommon.Instance.SchemaProfile1_1;
                //future versions when there is more then on version of schema
                //if (scVer == 1.1M)
                //{
                //    xsdMarkup = AppCommon.Instance.SchemaProfile1_1;
                //}
                //else if (scVer == 1.2M)
                //{
                //    xsdMarkup = AppCommon.Instance.SchemaProfile1_2;
                //}
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

                custDoc.Validate(schemas, (o, e) =>
                {
                    result.Errors.Add(e.Message);
                });

            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
            }
            return result;
        }
        #endregion

        #region Helper Methods
        private static void ValidateVersionRange(decimal ver)
        {
            if (ver < AppCommon.Instance.MinAllowSchemaVersion || ver > AppCommon.Instance.MAxAllowSchemaVersion)
            {
                string msg = string.Format(Properties.Resources.ErrorSchemVersion,
                    AppCommon.Instance.MinAllowSchemaVersion,
                    AppCommon.Instance.MAxAllowSchemaVersion);
                throw new NotSupportedException(msg);
            }
        }

        private static decimal GetSchemaVersion(XElement rootElement)
        {
            XAttribute att = rootElement.Attribute("schemaVersion");
            if (att == null)
            {
                throw new XmlException(Properties.Resources.ErrorMissingSchemaVersion);

            }
            decimal result = 0.0M;
            if (decimal.TryParse(att.Value, out result))
            {
                return result;
            }
            return 0.0M;
        }

        private static XmlKindEnum GetXmlType(XElement rootElement)
        {
            if (IsSilement(rootElement))
            {
                return XmlKindEnum.SnippitInstl;
            }
            if (IsProfileElement(rootElement))
            {
                return XmlKindEnum.Profile;
            }
            if (IsPluginElement(rootElement))
            {
                return XmlKindEnum.Plugin;
            }
            return XmlKindEnum.None;
        }

        private static bool IsSilement(XElement rootElement)
        {
            if (rootElement.Name == @"SnippitInstal")
            {
                return true;
            }
            return false;
        }
        private static bool IsProfileElement(XElement rootElement)
        {
            if (rootElement.Name == @"profile")
            {
                return true;
            }
            return false;
        }
        private static bool IsPluginElement(XElement rootElement)
        {
            if (rootElement.Name == @"plugin")
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
