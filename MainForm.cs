using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Leadtools;
using Leadtools.Codecs;
using Leadtools.Dicom;

namespace DicomEncapsulatedPdf
{
   public partial class MainForm : Form
   {
      private RasterCodecs _codecs;
      //Specify the paths for our PDF and our output DICOM File
      private string _PDFOutFile = @"C:\Users\Public\Documents\LEADTOOLS Images\LeadtoolsPdfDcm.pdf";
      private string _EncapsulatedPDFDicomFile = @"EncapsulatedPDF.dcm";

      public MainForm()
      {
         InitializeComponent();
      }

      private void MainForm_Load(object sender, EventArgs e)
      {
         //Set our LEADTOOLS License and Developer Key
         RasterSupport.SetLicense(@"C:\LEADTOOLS 19\Common\License\LEADTOOLS.LIC", System.IO.File.ReadAllText(@"C:\LEADTOOLS 19\Common\License\LEADTOOLS.LIC.KEY"));

         //Create a RasterCodecs Instance
         _codecs = new RasterCodecs();

         //Startup our DICOM Engine
         DicomEngine.Startup();
      }

      private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         //Shutdown our DICOM Engine
         DicomEngine.Shutdown();
      }

      private void btnCreate_Click(object sender, EventArgs e)
      {
         try
         {
            //Create a new DicomDataSet
            using (DicomDataSet ds = new DicomDataSet())
            {
               //Initialize the data set with the DicomClass EncapsulatedPdfStorage
               ds.Initialize(DicomClassType.EncapsulatedPdfStorage, DicomDataSetInitializeFlags.ExplicitVR | DicomDataSetInitializeFlags.LittleEndian | DicomDataSetInitializeFlags.AddMandatoryElementsOnly | DicomDataSetInitializeFlags.AddMandatoryModulesOnly);

               //Get the EncapsulatedDocument DICOM Element (0042:0011)
               DicomElement element = ds.FindFirstElement(null, DicomTag.EncapsulatedDocument, true);
               if (element != null)
               {
                  OpenFileDialog openFileDialog = new OpenFileDialog();
                  openFileDialog.Filter = @"Pdf (*.pdf)|*.pdf";

                  if (openFileDialog.ShowDialog() == DialogResult.OK)
                  {
                     //Set the document into the element
                     DicomDataSet_SetEncapsulatedDocumentExample(element, false, ds, openFileDialog.FileName);

                     //Save out the new DICOM Data Set
                     ds.Save(_EncapsulatedPDFDicomFile, DicomDataSetSaveFlags.None);
                     MessageBox.Show(string.Format("Encapsulated PDF Storage Dicom Dataset file \"{0}\" successfully created.", _PDFOutFile));
                  }
               }
               else
               {
                  MessageBox.Show("Couldn't find Encapsulated Document element (0042,0011)");
                  return;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }

      void DicomDataSet_SetEncapsulatedDocumentExample(DicomElement element, bool child, DicomDataSet ds, string sFileDocumentIn)
      {
         try
         {
            //Create a new DICOM Encapsulated Document
            DicomEncapsulatedDocument encapsulatedDocument = new DicomEncapsulatedDocument();

            //Set the attributes of the encapsulated document
            encapsulatedDocument.Type = DicomEncapsulatedDocumentType.Pdf; //Type == Pdf
            encapsulatedDocument.InstanceNumber = 123;
            encapsulatedDocument.ContentDate = new DicomDateValue(2015, 1, 1);

            encapsulatedDocument.ContentTime = new DicomTimeValue(12, 30, 00, 1);

            encapsulatedDocument.AcquisitionDateTime = new DicomDateTimeValue(2015, 1, 1, 12, 30, 00, 01, -3);

            encapsulatedDocument.BurnedInAnnotation = "YES";
            encapsulatedDocument.DocumentTitle = sFileDocumentIn;
            encapsulatedDocument.VerificationFlag = "UNVERIFIED";
            encapsulatedDocument.HL7InstanceIdentifier = string.Empty;

            //Mime type of the document (ignored)
            encapsulatedDocument.MimeTypeOfEncapsulatedDocument = "***** This is ignored when calling SetEncapsulatedDocument *****";

            //Mime types of the document
            string[] sListOfMimeTypes = new string[] { "image/jpeg", "application/pdf" };
            encapsulatedDocument.SetListOfMimeTypes(sListOfMimeTypes);

            DicomCodeSequenceItem conceptNameCodeSequence = new DicomCodeSequenceItem();
            conceptNameCodeSequence.CodingSchemeDesignator = "LN";
            conceptNameCodeSequence.CodeValue = "12345";
            conceptNameCodeSequence.CodeMeaning = "Sample Code Meaning";

            //Set the document into the data set
            ds.SetEncapsulatedDocument(element, child, sFileDocumentIn, encapsulatedDocument, conceptNameCodeSequence);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }

      void DumpEncapsulatedDocumentTags(string sMsgIn, DicomEncapsulatedDocument encapsulatedDocument, DicomCodeSequenceItem conceptNameCodeSequence)
      {
         try
         {
            //Print out all of the attributes for the encapsulated document
            string sMimeTypes = string.Empty;
            string[] sListOfMimeTypes = encapsulatedDocument.GetListOfMimeTypes();
            foreach (string s in sListOfMimeTypes)
               sMimeTypes = sMimeTypes + s + ",";
            string sMsg1 = string.Format("{0}\n\nuType: {1}\nInstanceNumber: {2}\nContentDate: {3}\nContentTime: {4}\nAcquisitionDateTime: {5}\nBurnedInAnnotation: {6}\nDocumentTitle: {7}\nVerificationFlag: {8}\nHL7InstanceIdentifier: {9}\nMIMETypeOfEncapsulatedDocument: {10}\nListOfMIMETypes: {11}\n\n",
               sMsgIn,
               encapsulatedDocument.Type.ToString(),
               encapsulatedDocument.InstanceNumber,
               encapsulatedDocument.ContentDate.ToString(),
               encapsulatedDocument.ContentTime.ToString(),
               encapsulatedDocument.AcquisitionDateTime.ToString(),
               encapsulatedDocument.BurnedInAnnotation,
               encapsulatedDocument.DocumentTitle,
               encapsulatedDocument.VerificationFlag,
               encapsulatedDocument.HL7InstanceIdentifier,
               encapsulatedDocument.MimeTypeOfEncapsulatedDocument,
               sMimeTypes
               );
            // ConceptNameCodeSequence 
            string sMsg2 = string.Format("CodeValue: {0}\nCodingSchemeDesignator: {1}\nCodingSchemeVersion: {2}\nCodeMeaning: {3}\nContextIdentifier: {4}\nMappingResource: {5}\nContextGroupVersion: {6}\nContextGroupLocalVersion: {7}\nContextGroupExtensionCreatorUID: {8}",
               conceptNameCodeSequence.CodeValue,
               conceptNameCodeSequence.CodingSchemeDesignator,
               conceptNameCodeSequence.CodingSchemeVersion,
               conceptNameCodeSequence.CodeMeaning,
               conceptNameCodeSequence.ContextIdentifier,
               conceptNameCodeSequence.MappingResource,
               conceptNameCodeSequence.ContextGroupVersion.ToString(),
               conceptNameCodeSequence.ContextGroupLocalVersion.ToString(),
               conceptNameCodeSequence.ContextGroupExtensionCreatorUID
               );

            MessageBox.Show(sMsg1 + sMsg2);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }

      void DicomDataSet_GetEncapsulatedDocumentExample(DicomElement element, bool child, DicomDataSet ds, string sFileDocumentOut)
      {
         try
         {
            DicomEncapsulatedDocument encapsulatedDocument = new DicomEncapsulatedDocument(); //New encapsulated document
            DicomCodeSequenceItem conceptNameCodeSequence = new DicomCodeSequenceItem(); //New code sequence item

            ds.GetEncapsulatedDocument(element, child, sFileDocumentOut, encapsulatedDocument, conceptNameCodeSequence); //Load the encapsulated document from the data set
            string sMsg;
            sMsg = string.Format("Encapsulated Document Extracted: {0}", sFileDocumentOut);
            DumpEncapsulatedDocumentTags(sMsg, encapsulatedDocument, conceptNameCodeSequence); //Print out all the attributes
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }

      private void btnLoad_Click(object sender, EventArgs e)
      {
         try
         {
            using (DicomDataSet ds = new DicomDataSet()) //New data set
            {
               if (!File.Exists(_EncapsulatedPDFDicomFile)) //Check to see if the DICOM file has already been created
               {
                  MessageBox.Show(string.Format("File \"{0}\" doesn't exist, try clicking the \"{1}\" button first", _EncapsulatedPDFDicomFile, btnCreate.Text));
                  return;
               }

               ds.Load(_EncapsulatedPDFDicomFile, DicomDataSetLoadFlags.None); //Load the DICOM File containing the encapsulsated document
               DicomElement element = ds.FindFirstElement(null, DicomTag.EncapsulatedDocument, true); //Find the EncapsulatedDocument element (0042:0011)
               if (element != null)
               {
                  DicomDataSet_GetEncapsulatedDocumentExample(element, false, ds, _PDFOutFile); //Extract and print the attributes of the encapsulated document

                  _codecs.Options.Load.XResolution = 300;
                  _codecs.Options.Load.YResolution = 300;
                  rasterImageViewer1.Image = _codecs.Load(_PDFOutFile); //Load the encapsulated document into the image viewer
               }
               else
               {
                  MessageBox.Show("Couldn't find Encapsulated Document element (0042,0011)");
                  return;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }
   }
}
