//Assembly: Inventory Entry Form
//Author: Joseph Castrejon
//Description: Windows form application that allows the entry of items for a music store. 
//The entered item has the following fields: Item Type, Manufacturer, Model, Serial Number, Condition, Price and Photos

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Web;
using System.Web.UI;


namespace SoundCoreMenus{
	
	//Define enumerations for combobox controls
	public enum ItemCategory {Guitar, Amplifier, Keyboard, Lighting, Microphone, PA, Percussion, Speaker}
	public	enum GuitarManufacturers {Austin, Cordoba, Dean, Epiphone, ESP, Fender, LTD, Martin, Schecter, Sigma, WashBurn, Yamaha}
	public 	enum KeyboardManufacturers {Behringer,Casio,Roland}
	public	enum ItemCondition {BrandNew, Excellent, Good, Poor, Factory_Second}
	
	public class App
	{
		//Pathes for saving xml and pictures for entered items. 
		private static string DatabasePath = Application.StartupPath + @"/scdb/";
		private static string PicturesPath = Application.StartupPath + @"/Pictures/";
		
		[STAThread]
		static public void Main()
		{		
			EntryForm StartupForm = new EntryForm();
			StartupForm.ShowDialog();
		}
		
		
		public class EntryForm : Form
		{
				//Common Item types, brands, and manufacturers
				string[] ItemStrings = new string[9]{"Amplifier", "Effects Pedal","Guitar", "Keyboard", "Lighting", "Microphone", "PA Equipment", "Percussion", "Speaker"};
				string[] Guitar_Brands = new string[13]{"Alvarez", "Austin", "Cordoba", "Dean", "Epiphone", "ESP", "Fender", "LTD","Martin", "Schecter", "Sigma", "Washburn", "Yamaha"};
				string[] Amp_Brands = new string[9]{"Boss", "Lee Jackson", "Egnater", "Fender", "Marshall", "Ampeg", "Crate", "Peavey", "Hartke"};
				string[] Keyboard_Brands = new string[3]{"Casio", "Roland", "Yamaha"};
				string[] Lighting_Brands = new string[1]{"American DJ"};
				string[] Microphone_Brands = new string[4]{"CAD","Shure", "Samson", "Peavey"};
				string[] PA_Brands = new string[9]{"ALESIS","Behringer", "CAD", "MACKIE", "Soundcraft", "Shure", "Samson","Peavey", "Presonus"};
				string[] Percussion_Brands = new string[11]{"DDRUM","CB Percussion","Gretsch","Gibraltar","LP","Mapex","Sabian","Pearl","Pro-Mark","Vic Firth", "Zildjian"};
				string[] Speaker_Brands = new string[2] {"Celestion","JBL"};
				string[] Item_Conditions = new string[5] {"Brand New", "Excellent", "Good", "Poor","Factory Second"};
				string[] Effects_Pedals = new string[7] {"Boss", "Digitech", "Electro-Harmonix", "Outlaw Effects", "MXR", "Vox", "Dunlop"};
				private List<string> PhotoFilesList = new List<string>();
				
				//Create the components of the form
				Label FormTitle = new Label();
				Label ItemLabel = new Label();
				Label ManuLabel = new Label();
				Label PriceLabel = new Label();
				Label PhotoLabel = new Label();
				Label SerialNumberLabel = new Label();
				Label DescLabel = new Label();
				Label ConditionLabel = new Label();
				Label ModelLabel = new Label();
				Label ShippingLabel	= new Label();
				Label UPCLabel = new Label();
				ComboBox ItemType = new ComboBox();
				ComboBox Manufacturer = new ComboBox(); 
				ComboBox ConditionInput = new ComboBox();
				TextBox PriceInput = new TextBox();
				TextBox ModelNumber = new TextBox();
				TextBox SerialNumberInput = new TextBox();
				TextBox Description = new TextBox();
				TextBox ShippingInput = new TextBox();
				TextBox UPCInput = new TextBox();
				ListBox PhotoList = new ListBox();
				ButtonBase EnterItem = new Button();
				ButtonBase PhotoSelect = new Button();
				ButtonBase PhotoRemove = new Button();
				PictureBox ItemPreview = new PictureBox();
				
				public EntryForm()
				{
					InitializeComponent();
				}
				
				private void InitializeComponent()
				{
					//Initialize the Components properties
					
					//Default sizes for comboboxes, combobox labels, and buttons 
					Size DefaultButtonSize = new Size(90,40);
					Size DefaultComboBox = new Size(150,20);
					
					//Assign Sizes
					ItemLabel.Size = DefaultComboBox;
					ItemType.Size = DefaultComboBox;
					ManuLabel.Size = DefaultComboBox;
					Manufacturer.Size = DefaultComboBox;
					PriceLabel.Size = DefaultComboBox;
					PriceInput.Size = DefaultComboBox;
					SerialNumberLabel.Size = DefaultComboBox;
					SerialNumberInput.Size = DefaultComboBox;
					ConditionInput.Size = DefaultComboBox;
					ConditionLabel.Size = DefaultComboBox;
					ModelNumber.Size = DefaultComboBox;
					ModelLabel.Size = DefaultComboBox;
					EnterItem.Size = DefaultButtonSize;
					PhotoSelect.Size = DefaultButtonSize;
					PhotoRemove.Size = DefaultButtonSize;	
					FormTitle.Size = new Size(400,40);
					PhotoLabel.Size = new Size(250,15);
					DescLabel.Size = new Size(260,20);
					Description.Size = new Size(310,70);
					PhotoList.Size = new Size(250,95);
					ItemPreview.Size = new Size(250,250);
					ShippingInput.Size = DefaultComboBox;
					ShippingLabel.Size = DefaultComboBox;
					UPCInput.Size = DefaultComboBox;
					UPCLabel.Size = DefaultComboBox;
					//Text formatting
					PhotoSelect.Text = "Select\nPhotos";
					EnterItem.Text = "\tAdd\n\tItem";
					PhotoRemove.Text = "Remove\nPhotos";
					PhotoLabel.Text = "Current Photo";
					ItemLabel.Text = "Item Type:";
					ManuLabel.Text = "Manufacturer:";
					FormTitle.Text = "Database Item Entry";
					PriceLabel.Text = "Item Price:";
					SerialNumberLabel.Text = "Serial Number:";
					DescLabel.Text = "Enter a short description:";
					ModelLabel.Text = "Model:";
					ConditionLabel.Text = "Item Condition:";
					ShippingLabel.Text = "Shipping Cost:";
					UPCLabel.Text = "UPC";
					Font DefaultInputFont = new Font("Arial", 10);
					Font DefaultLabelFont = new Font("Arial", 12, FontStyle.Bold);
					
					//Font Formatting
					SerialNumberLabel.Font = new Font("Arial", 12,FontStyle.Bold);
					FormTitle.Font = new Font("Arial",24,FontStyle.Bold);
					PhotoLabel.Font = new Font("Arial", 14, FontStyle.Bold);
					EnterItem.Font = new Font("Arial", 11);
					PhotoSelect.Font = new Font("Arial", 10);
					PhotoRemove.Font = new Font("Arial", 10);
					ItemLabel.Font = DefaultLabelFont;
					ManuLabel.Font = DefaultLabelFont;
					PriceLabel.Font = DefaultLabelFont;
					ModelLabel.Font = DefaultLabelFont;
					DescLabel.Font = DefaultLabelFont;
					ConditionLabel.Font = DefaultLabelFont;
					ItemType.Font = DefaultInputFont;
					Manufacturer.Font = DefaultInputFont;
					ConditionInput.Font = DefaultInputFont;
					PriceInput.Font = DefaultInputFont;
					ModelNumber.Font = DefaultInputFont;
					SerialNumberInput.Font = DefaultInputFont;
					ShippingLabel.Font = DefaultLabelFont;
					UPCLabel.Font = DefaultLabelFont;
					UPCInput.Font = DefaultInputFont;
					
					//Text Alignments
					PhotoLabel.TextAlign = ContentAlignment.MiddleCenter;
					EnterItem.TextAlign = ContentAlignment.MiddleLeft;
					PhotoSelect.TextAlign = ContentAlignment.MiddleLeft;
					PhotoRemove.TextAlign = ContentAlignment.MiddleLeft;
					
					//Images Controls
					EnterItem.Image = Image.FromFile("./assets/Plus-Sign.png");
					EnterItem.ImageAlign = ContentAlignment.MiddleRight;
					PhotoSelect.Image = Image.FromFile("./assets/Camera.png");
					PhotoSelect.ImageAlign = ContentAlignment.MiddleRight;
					PhotoRemove.Image = Image.FromFile("./assets/Remove-Camera.png");
					PhotoRemove.ImageAlign = ContentAlignment.MiddleRight;
					ItemPreview.BorderStyle = BorderStyle.FixedSingle;
					PhotoList.ScrollAlwaysVisible = true;
				
					//Locations
					EnterItem.Location = new Point(10,400);
					PhotoSelect.Location = new Point(120,400);
					PhotoRemove.Location = new Point(230,400);
					FormTitle.Location = new Point (150,10);
					ItemLabel.Location = new Point(10, 70);
					ItemType.Location = new Point(10, 90);
					ManuLabel.Location = new Point(170,70);
					Manufacturer.Location = new Point(170, 90);
					ModelLabel.Location = new Point(10, 130);
					ModelNumber.Location = new Point(10, 150);
					SerialNumberLabel.Location = new Point(170,130);
					SerialNumberInput.Location = new Point(170,150);
					ConditionLabel.Location = new Point(10, 190);
					ConditionInput.Location = new Point(10, 210);						
					PriceLabel.Location = new Point(10,250);
					PriceInput.Location = new Point(10,270);
					PhotoLabel.Location = new Point(330,70);
					PhotoList.Location = new Point(330,350);	
					ItemPreview.Location = new Point(330, 90);
					DescLabel.Location = new Point(10,300);
					Description.Location = new Point(10,320);
					ShippingLabel.Location = new Point(170, 250);
					ShippingInput.Location = new Point(170, 270);
					UPCLabel.Location = new Point(170,190);
					UPCInput.Location = new Point(170,210);
					
					
					ItemPreview.Image = Image.FromFile("./assets/test-image.jpg");

					//Remove user input from combobox
					ItemType.DropDownStyle = ComboBoxStyle.DropDownList;
					Manufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
					ConditionInput.DropDownStyle = ComboBoxStyle.DropDownList;
					
					//Set the max length for description input
					Description.MaxLength = 1000;
					
					foreach(string Item in ItemStrings)
						ItemType.Items.Add(Item);
			
					foreach(string Condition in Item_Conditions)
					{
						ConditionInput.Items.Add(Condition);
					}
			
					//Finally set control options and add to the form object.
					this.Text = "Sound Core - Database Entry";
					this.StartPosition = FormStartPosition.CenterScreen;
					//this.MaximizeBox = false;
					//this.MinimizeBox = false;
					this.FormBorderStyle = FormBorderStyle.FixedSingle;
					this.MaximumSize = new Size(600,480);
					this.Size = new Size(600,480);
					
					this.Icon = new Icon("./assets/Database.ico", 64,64);	
					this.Controls.Add(SerialNumberLabel);
					this.Controls.Add(FormTitle);
					this.Controls.Add(ItemLabel);
					this.Controls.Add(ItemType);
					this.Controls.Add(Manufacturer);
					this.Controls.Add(ModelNumber);
					this.Controls.Add(SerialNumberInput);
					this.Controls.Add(ConditionInput);
					this.Controls.Add(UPCInput);
					this.Controls.Add(PriceInput);
					this.Controls.Add(ShippingInput);
					this.Controls.Add(Description);
					this.Controls.Add(ShippingLabel);
					this.Controls.Add(ModelLabel);
					this.Controls.Add(ManuLabel);
					this.Controls.Add(ConditionLabel);
					this.Controls.Add(PriceLabel);
					this.Controls.Add(UPCLabel);
					this.Controls.Add(PhotoLabel);
					this.Controls.Add(PhotoSelect);
					this.Controls.Add(PhotoRemove);
					this.Controls.Add(PhotoList);
					this.Controls.Add(EnterItem);
					this.Controls.Add(DescLabel);
					this.Controls.Add(ItemPreview);
					Description.Multiline = true;
					ConditionInput.Enabled = false;
					Manufacturer.Enabled = false;
					ModelNumber.Enabled = false;
					PriceInput.Enabled = false;
					SerialNumberInput.Enabled = false;
					Description.Enabled = false;
					ShippingInput.Enabled = false;
					UPCInput.Enabled = false;
					
				
					//DELEGATE METHODS AND EVENT HANDLERS
					ItemType.SelectedValueChanged += new EventHandler(ItemType_SelectedValueChanged);
					Manufacturer.SelectedValueChanged += new EventHandler(Manufacturer_SelectedValueChanged);
					EnterItem.Click += new EventHandler(EnterItem_Click);		
					PhotoSelect.Click += new EventHandler(PhotoSelect_Click);	
					PhotoRemove.Click += new EventHandler(PhotoRemove_Click);
					PhotoList.DoubleClick += new EventHandler(PhotoList_DoubleClick);
				}
				
				//Event Handlers 
				private void PhotoRemove_Click(object sender, EventArgs e)
				{
					
					try{
						if(PhotoList.GetItemText(PhotoList.SelectedItem) != null)
						{
							int ItemIndex = PhotoList.SelectedIndex;
							this.PhotoList.Items.Remove(PhotoList.SelectedItem);
							//MessageBox.Show(PhotoList.SelectedItem.ToString());
							PhotoFilesList.RemoveAt(ItemIndex);
						}	
					}catch(ArgumentOutOfRangeException RE)
					{
						MessageBox.Show(RE.Message, "ERROR: No Photo to remove", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					
				}
				
				private void PhotoSelect_Click(object sender, EventArgs e)
				{
					//Clear the list of files
					OpenFileDialog SelectFiles = new OpenFileDialog();
					SelectFiles.Filter = "Images (*.bmp;*.jpg;*.gif)|*.BMP;*.JPG;*.GIF|" + "All files (*.*)|*.*";
					SelectFiles.Title = "Select photos of entered item.";
					SelectFiles.Multiselect = true;
					if(SelectFiles.ShowDialog() == DialogResult.OK)
					{
						foreach(string PhotoNames in SelectFiles.FileNames)
						{
							PhotoFilesList.Add(PhotoNames);
							PhotoList.Items.Add(Path.GetFileName(PhotoNames));
						}	
					}
					
				}
				
				private void ItemType_Click(object sender, EventArgs e)
				{
					
					
				}
				
				
				private void ItemType_SelectedValueChanged(object sender, EventArgs e)
				{
					try{
						if(ItemType.SelectedItem != null)
						{
							
							Manufacturer.Enabled = true;
							Manufacturer.Items.Clear();		
							switch(ItemType.SelectedItem.ToString()){
									case "Guitar":
										foreach(string Item in Guitar_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Amplifier":
										foreach(string Item in Amp_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Keyboard":
										foreach(string Item in Keyboard_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Lighting":
										foreach(string Item in Lighting_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Microphone":
										foreach(string Item in Microphone_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "PA Equipment":
										foreach(string Item in PA_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Percussion":
										foreach(string Item in Percussion_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Speaker":
										foreach(string Item in Speaker_Brands)
											Manufacturer.Items.Add(Item);
									break;
									case "Effects Pedal":
										foreach(string Item in Effects_Pedals)
											Manufacturer.Items.Add(Item);
									break;
									default:
										MessageBox.Show("Could not add manufacturers to drop down list!");
									break;
							};
						}
					}catch(Exception Ex){
						MessageBox.Show(Ex.ToString(),"EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
						
				}
				
				///<summary>
				///Enable other input fields whenever the value of the Manufacturer input changes
				///</summary>
				private void Manufacturer_SelectedValueChanged(object sender, EventArgs e)
				{
					if(Manufacturer.SelectedItem != null)
					{
						//Allow input for the rest of the item controls 	 
						ConditionInput.Enabled = true;
						ModelNumber.Enabled = true;
						PriceInput.Enabled = true;
						SerialNumberInput.Enabled = true;
						Description.Enabled = true;
						UPCInput.Enabled = true;
						ShippingInput.Enabled = true;

					}
				}
				
				///<summary>
				///Method that is called when the user chooses to enter an item to the database.
				///</summary>
				private void EnterItem_Click(object sender, EventArgs e)
				{
					
					bool CorrectInfo = CheckFormFields();
					string ItemInput = ItemType.GetItemText(ItemType.SelectedItem);
				
					if(CorrectInfo)
					{
						try{
							
							string FileName;
						
							switch(ItemInput)
							{
								case "Guitar":
									FileName = "Guitars.xml";
									AddItem(FileName);
								break;
								case "Amplifier":
									FileName = "Amplifiers.xml";
									AddItem(FileName);
								break;
								case "Keyboard":
									FileName = "Keyboards.xml";
									AddItem(FileName);
								break;
								case "Lighting":
									FileName = "Lighting.xml";
									AddItem(FileName);
								break;
								case "Microphone":
									FileName = "Microphones.xml";
									AddItem(FileName);
								break;
								case "PA":	
									FileName = "PA_Equipment.xml";
									AddItem(FileName);
								break;
								case "Percussion":
									FileName = "Percussion.xml";
									AddItem(FileName);
								break;
								case "Speaker":
									FileName = "Speakers.xml";
									AddItem(FileName);
								break;
								case "Effects Pedal":
									FileName = "Effects_Pedals.xml";
									AddItem(FileName);
								break;
								default:
									throw new Exception("Item could not be added to database!");
							}
						}catch(Exception Ex)
						{
							MessageBox.Show(Ex.ToString(),"EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						
						ClearFormFields();
					}
				}
		
	
				///<summary>
				///Loads a user specifed XML document, appends an item to the end, and saves the document.
				///</summary>
				private void AddItem(string FileName)
				{
				    bool FilesCopied = false;
					string XmlPath = @"./scdb/";
					string ItemCategory = ItemType.GetItemText(ItemType.SelectedItem);
				
					
					//Create an empty XML Document
					XDocument XMLDatabaseFile = new XDocument();
					
					//Get the Item type from the form, in order to create the create XML object
					string[] ItemInfo = new string[] {
						Manufacturer.GetItemText(Manufacturer.SelectedItem),
						ModelNumber.Text,
						SerialNumberInput.Text, 
						Description.Text,
						ConditionInput.GetItemText(ConditionInput.Text),
						ShippingInput.Text,
						UPCInput.Text
					};
					
					//Item price check. Checks for negative prices and characters in the price.
					float price = 0;						
					bool CorrectPrice = float.TryParse(PriceInput.Text, out price);
				
				
					//Create the base Item XML
					XElement ItemXML = new XElement("Item");
					
					try{

							//Load the previous xml file.
							XMLDatabaseFile = XDocument.Load(XmlPath + FileName);
							//MessageBox.Show("Adding to " + XMLDatabaseFile.Root.Name.ToString() + " Database.");
							ItemXML.Add(new XElement("Manufacturer",ItemInfo[0]));
							ItemXML.Add(new XElement("Model",ItemInfo[1]));
							ItemXML.Add(new XElement("SerialNumber",ItemInfo[2]));
							ItemXML.Add(new XElement("UPC",ItemInfo[6]));
							ItemXML.Add(new XElement("Condition",ItemInfo[4]));
							ItemXML.Add(new XElement("Price", price));
							ItemXML.Add(new XElement("ShippingCost",ItemInfo[5]));
							ItemXML.Add(new XElement("Description",ItemInfo[3]));
							ItemXML.Add(new XElement("DateAdded", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));
							
							string ItemPage = string.Join("-", new string[]{ItemInfo[0].Replace(' ','-'),ItemInfo[1].Replace(' ','-')});
						
							ItemXML.Add(new XElement("ItemPage", "./Item/" + ItemPage + ".html"));
							
							switch(ItemCategory){
								case "PA Equipment":
									ItemXML.Add(new XElement("MainPhoto", "./Pictures/PA/" + ItemInfo[0] + "/" + ItemInfo[1] + "/" + ItemInfo[0] + " " + ItemInfo[1] + " (Main).jpg" ));
									break;
								case "Lighting":
								case "Percussion":
									ItemXML.Add(new XElement("MainPhoto", "./Pictures/" + ItemCategory + "/" + ItemInfo[0] + "/" + ItemInfo[1] + "/" + ItemInfo[0] + " " + ItemInfo[1] + " (Main).jpg" ));
									break;
								default:
									ItemXML.Add(new XElement("MainPhoto", "./Pictures/" + ItemCategory + "s/" + ItemInfo[0] + "/" + ItemInfo[1] + "/" + ItemInfo[0] + " " + ItemInfo[1] + " (Main).jpg"));
									break;
							}
							
							/*
							XElement Pictures = new XElement("ItemPics");
							foreach(var FilePath in PhotoFilesList)
							{
								Pictures.Add(new XElement("Picture", FilePath));
							}
							ItemXML.Add(Pictures);
							*/
							
							XNode LastItem = XMLDatabaseFile.Root.LastNode;
							
							
							if(LastItem == null)
							{
								XMLDatabaseFile.Root.Add(ItemXML);	
								
							}else if(LastItem != null)
							{
								LastItem.AddAfterSelf(ItemXML);
							}
							
							XMLDatabaseFile.Save(XmlPath+FileName);
							//File.Copy( XmlPath + FileName, @"O:\Inventory Entry\Xml + FileName,true);
					}
					catch(Exception e)
					{
						MessageBox.Show(e.Message);
					}
					
					try{
						FilesCopied = CopyPhotos(PhotoFilesList);			
						GenerateItemPage(ItemXML);
					}
					catch (Exception e)
					{
						MessageBox.Show(e.Message);
					}
					finally{
						//Flag for verifying copied images.	
						if(FilesCopied){
						
							//Check which item category is being added and display which item type is created.
							switch(FileName)
							{
								case "Amplifiers.xml":
									MessageBox.Show("Amplifier Added.");
								break;
								case "Guitars.xml":
									MessageBox.Show("Guitar Added.");
								break;
								case "Keyboards.xml":
									MessageBox.Show("Keyboard Added.");
								break;
								case "Lighting.xml":
									MessageBox.Show("Lighting Item Added.");
								break;
								case "Microphones.xml":
									MessageBox.Show("Microphone Added.");
								break;
								case "PA-Equipment.xml":
									MessageBox.Show("PA Item Added.");
								break;
								case "Percussion.xml":
									MessageBox.Show("Percussion Item Added.");
								break;
								case "Speakers.xml":
									MessageBox.Show("Speaker Added.");
								break;
							}
						}else{
							MessageBox.Show("Item pictures could not be uploaded.","ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
		
		
				///<summary>
				///Clears the controls of the form so that they are ready for entering a new item.
				///</summary>
				private void ClearFormFields()
				{
					foreach (System.Windows.Forms.Control field in this.Controls)
					{
						if (field is TextBox){
							((TextBox)field).Clear();
							((TextBox)field).ForeColor = Color.Black;
							((TextBox)field).Enabled = false;
						}
						else if (field is ComboBox){
							((ComboBox)field).SelectedIndex=-1;
							((ComboBox)field).Enabled = false;
						}
						else if(field is ListBox)
						{
							((ListBox)field).Items.Clear();
						}
						else if(field is PictureBox)
						{
							((PictureBox)field).Image = Image.FromFile("./assets/test-image.jpg");
							//Remember to remove all file names from the photo files list or the wrong photos will be associated with the created item.
							this.PhotoFilesList.Clear();
						}
					}
					
					this.ItemType.Enabled = true;	
				}

				///<summary>
				///Checks the controls of the form for missing information or information that is entered incorrectly.
				///</summary>
				private bool CheckFormFields()
				{
					bool CorrectPrice = false;
					float price = 0;
					//Item price check. Checks for negative prices and characters in the price.	
					CorrectPrice = float.TryParse(PriceInput.Text, out price);
					
					if (CorrectPrice != true){
						MessageBox.Show("Input price is not a number.","ERROR: Incorrect Price Format",MessageBoxButtons.OK,MessageBoxIcon.Error);
						PriceInput.ForeColor = Color.Red;
						return false;
					}
					
					if (price < 0){
						MessageBox.Show("Price can not be less than zero", "ERROR: Negative Price", MessageBoxButtons.OK, MessageBoxIcon.Error);
						PriceInput.ForeColor = Color.Red;
						return false;
					}
					//The price input input is correct, so change the color back to black.
					PriceInput.ForeColor = Color.Black;
			
					//Shipping Cost Check
					bool ShippingCorrect = false;
					float ShippingCost = 0;
					ShippingCorrect = float.TryParse(ShippingInput.Text,out ShippingCost);
					if(ShippingCorrect != true)
					{
						MessageBox.Show("Shipping cost is not a number.", "ERROR: Invalid Shipping Cost", MessageBoxButtons.OK, MessageBoxIcon.Error);
						ShippingInput.ForeColor = Color.Red;
						return false;				
					}else if(ShippingCost < 0)
					{
						MessageBox.Show("Shipping cost can't be less than zero.", "ERROR: Invalid Shipping Cost", MessageBoxButtons.OK, MessageBoxIcon.Error);
						ShippingInput.ForeColor = Color.Red;
						return false;
					}
					ShippingInput.ForeColor = Color.Black;
					
					//Get the description of the item, make sure it is less than 1000 characters.
					//Using less than 1000 characters saves on file sizes.
					string DescriptionBuffer;
					DescriptionBuffer = this.Description.Text;
					char[] ItemDescription = new char[1000];

					//Check for long descriptions.
					if(DescriptionBuffer.Length > 1000)
					{
						Description.ForeColor = Color.Red;
						MessageBox.Show("The current description is too long. Please make sure the description is less than 500 characters", "ERROR: Description Length too long", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					Description.ForeColor = Color.Black;
					
					//Check for empty text boxes
					if(ModelNumber.Text == ""){
						MessageBox.Show("Please enter a model number", "ERROR: Model Number Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
						ModelNumber.ForeColor = Color.Red;
						return false;						
					}
					ModelNumber.ForeColor = Color.Black;	
						
						
					//Check the Serial Number	
					if(SerialNumberInput.Text == ""){
						
						DialogResult SerialNumberCheck = MessageBox.Show("Entered item is missing a serial number. Continue adding item?", "WARNING: Missing Serial Number",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation); 
	
						if (SerialNumberCheck == DialogResult.Cancel)
						{
							//Do something here if OK was clicked.
							SerialNumberInput.ForeColor = Color.Red;
							return false;
						}
					}	
					SerialNumberInput.ForeColor = Color.Black;
					
					//Check the Condition 
					if(ConditionInput.Text == ""){
						MessageBox.Show("Please select the item's condition", "ERROR: Item Condition Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
						ConditionInput.ForeColor = Color.Red;
						return false;
					}
					ConditionInput.ForeColor = Color.Black;
					
					//Check the UPC
					if(UPCInput.Text == ""){
						MessageBox.Show("Please enter the item's UPC.", "ERROR: UPC Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
						UPCInput.ForeColor = Color.Red;
						return false;
					}
					UPCInput.ForeColor = Color.Black;
					
					//Check for no photo added.
					if(PhotoFilesList.Count == 0){
						MessageBox.Show("Please add at least one photo for the item that you are currently entering", "ERROR: No Photo", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					
					//Check Image dimensions. 
					foreach(var PhotoFile in PhotoFilesList)
					{
						
						using(Image UploadedImage = Image.FromFile(PhotoFile))
						{
							float AspectRatio = UploadedImage.Width / UploadedImage.Height;
							//MessageBox.Show("Aspect Ratio of Image : \" " + PhotoFile + "\" = " + Math.Round((float)UploadedImage.Width / UploadedImage.Height,2));
							
							//Uploaded photos must have 1:1 or 4:3 aspect ratios
							if( UploadedImage.Width == UploadedImage.Height ) 
								continue;
							else if( Math.Round((float)UploadedImage.Width / UploadedImage.Height,2) == 1.33)
								continue;
							else{
								MessageBox.Show("Image \"" + PhotoFile.ToString() + "\" has incompatible dimensions. Please use photos with an aspect ratio of 1:1 or 4:3","ERROR: Unusable Photo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);			
								return false;
							}
						}
						
					}
				
						
					return true;
				}
	
				///<summary>
				///Displays a photo in the form's picturebox when the user double clicks on a file name within files list.
				///</summary>
				private void PhotoList_DoubleClick(object sender, EventArgs e)
				{
					if(PhotoList.SelectedItem != null)
					{
						var PhotoIndex = PhotoList.SelectedIndex;
						Image ItemImage = Image.FromFile(PhotoFilesList[PhotoIndex]);
						this.ItemPreview.SizeMode = PictureBoxSizeMode.StretchImage;
						this.ItemPreview.Image = ItemImage;
					}
				}

				///<summary>
				///Copies the user selected photos to the database. 
				///</summary>
				private bool CopyPhotos(List<string> PhotoFileNames)
				{
					try{
						int ListLength = PhotoFileNames.Count;
						string EnteredCategory = ItemType.GetItemText(ItemType.SelectedItem);
						string EnteredManufacturer = Manufacturer.GetItemText(Manufacturer.SelectedItem);
						string EnteredModel = ModelNumber.Text;
						string NewPhotoPath;
						
						if(EnteredCategory == "PA Equipment")
							NewPhotoPath = PicturesPath + "/PA/" + EnteredManufacturer + "/" + EnteredModel + "/";
						else if(EnteredCategory == "Percussion" || EnteredCategory == "Lighting")
							NewPhotoPath = PicturesPath + "/" + EnteredCategory + "/" + EnteredManufacturer + "/" + EnteredModel + "/";
						else
							NewPhotoPath = PicturesPath + "/" + EnteredCategory + "s/" + EnteredManufacturer + "/" + EnteredModel + "/";
						
						Directory.CreateDirectory(NewPhotoPath);
										
						for(int x = 0; x < ListLength; x++)
						{
							//Copy the original image
							using(Bitmap OriginalImage = new Bitmap(Image.FromFile(PhotoFileNames[x])))
							{
								if(OriginalImage.Size.Height < 400 || OriginalImage.Size.Width < 400)
									throw new Exception("Image size too small.");
								
								OriginalImage.Save(NewPhotoPath+"IMG"+x.ToString()+".jpg", ImageFormat.Jpeg);	
							}
							
							//Create main photo on the first iteration.
							if(x == 0)
							{
								using(Bitmap MainImage = new Bitmap(Image.FromFile(PhotoFileNames[x]),400,400))
								{
									string FileName = EnteredManufacturer + " " + EnteredModel + " (Main)";
									MainImage.Save(NewPhotoPath+FileName+".jpg",ImageFormat.Jpeg);
								} 
							}
							
							//Save a thumbnail icon
							using(Bitmap NewImage = new Bitmap(Image.FromFile(PhotoFileNames[x]),64,64))
							{
								string ThumbnailName = Path.GetFileNameWithoutExtension(PhotoFileNames[x]) + " (thumbnail)";
								NewImage.Save(NewPhotoPath+ThumbnailName+".jpg",ImageFormat.Jpeg);
							}
						}
					}catch(Exception e)
					{
						MessageBox.Show(e.Message+"\n ","ERROR: COULD NOT COPY PHOTOS");
						return false;	
					}
					
					MessageBox.Show("Item photos uploaded.");	
					return true;
				}
			
				///<summary>
				///Creates an HTML page to be uploaded to www.soundcoremusic.com
				///</summary>
				private void GenerateItemPage(XElement ItemXML)
				{
					const string HTMLPagePath = "./Item/";
					string ItemMFG = ItemXML.Element("Manufacturer").Value.ToString().Replace(' ','-');
					string ItemModel = ItemXML.Element("Model").Value.ToString().Replace(' ','-');
					string FullItemName = ItemMFG + " " + ItemModel;
					string FileName = HTMLPagePath + ItemMFG + "-" + ItemModel + ".html";
					
					StringWriter SW = new StringWriter();
				
					using(HtmlTextWriter PageWriter = new HtmlTextWriter(SW))
					{
							PageWriter.AddAttribute("lang", "en");
							PageWriter.RenderBeginTag(HtmlTextWriterTag.Html);
							CreateHTMLHead(PageWriter, ItemXML);
							PageWriter.WriteLine(); 
							PageWriter.RenderBeginTag(HtmlTextWriterTag.Body); // Begin rendering the body tag	
							PageWriter.AddAttribute("id","wrap");
							PageWriter.RenderBeginTag(HtmlTextWriterTag.Div);  //Begin rendering the wrap div	
							
							//Create the main sections of the document.
							CreatePageHeader(PageWriter);
							CreatePageNav(PageWriter);
							CreateItemSection(PageWriter, ItemXML);
							CreatePageFooter(PageWriter);
							PageWriter.AddAttribute("src","../../js/navigation.js");
							PageWriter.RenderBeginTag(HtmlTextWriterTag.Script);
							PageWriter.RenderEndTag(); //End Script Tag
							PageWriter.AddAttribute("src", "../../js/ItemZoomViewer.js");
							PageWriter.RenderBeginTag(HtmlTextWriterTag.Script);
							PageWriter.RenderEndTag(); // End Rendering the wrap div
							PageWriter.RenderEndTag(); // End Rendering the body tag
							PageWriter.Indent++;
							PageWriter.Indent--;
							PageWriter.RenderEndTag();
					}
					
					File.WriteAllText(FileName,SW.ToString());
					SW.Dispose();
				}
		
				///<summary>
				///Create the head markup for the HTML Page
				///</summary>
				private void CreateHTMLHead(HtmlTextWriter PageMaker, XElement ItemInfo)
				{
					const string TitleSuffix = "| Sound Core Store";
					string ItemMFG = ItemInfo.Element("Manufacturer").Value.ToString();
					string ItemModel = ItemInfo.Element("Model").Value.ToString();
					string FullItemName = ItemMFG + " " + ItemModel;
					string StoreCategory = ItemType.GetItemText(ItemType.SelectedItem);
					
					//Create the html elements for the head tag.
					PageMaker.RenderBeginTag("head"); //Render Begin - head
					PageMaker.RenderBeginTag("title"); //Render Begin - title
					PageMaker.WriteLine(ItemMFG + " " + ItemModel + " " + TitleSuffix);
					PageMaker.RenderEndTag(); //Render End - title
					PageMaker.WriteLine();
					PageMaker.AddAttribute("charset","UTF-8"); 
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Meta); //Render Begin - meta
					PageMaker.RenderEndTag();// Render End - meta 
					PageMaker.WriteLine();
					PageMaker.AddAttribute("name","description");
					PageMaker.AddAttribute("content", ItemMFG + " " + ItemModel + "  from Sound Core\'s online store."); // Render Begin - meta 2
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Meta); //Render Begin - Meta 2
					PageMaker.RenderEndTag(); // Rend End - meta 2
					PageMaker.WriteLine();
					PageMaker.AddAttribute("name","keywords");
					PageMaker.AddAttribute("content", "Sound Core Music, Sound Core, Music Store Carbondale,Sound Core " + StoreCategory + "," + FullItemName + "," + ItemModel );		
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Meta); // Render begin - meta 3
					PageMaker.RenderEndTag(); // Render End - meta 3
					PageMaker.WriteLine();
					
					/*-----------------------
					STYLE SHEETS ADDED BELOW
					------------------------*/
					//Adding Styles assumes that the page will be added to the Item subdirectory
					PageMaker.AddAttribute("rel","stylesheet");
					PageMaker.AddAttribute("type","text/css");
					PageMaker.AddAttribute("href","../../css/Items.css");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Link); 
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					PageMaker.AddAttribute("rel","stylesheet");
					PageMaker.AddAttribute("type","text/css");
					PageMaker.AddAttribute("href","../../css/SoundCoreTheme.css");		
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Link);
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					
					/*------------------------------
					JAVASCRIPT LIBRARIES ADDED BELOW
					-------------------------------*/
					PageMaker.AddAttribute("src","../../js/jquery.js");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Script);
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					PageMaker.AddAttribute("src","../../js/bootstrap.js");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Script);
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					//Load Favicon
					PageMaker.AddAttribute("rel","icon");
					PageMaker.AddAttribute("href","../../favicon.ico");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Link);
					PageMaker.RenderEndTag();
					PageMaker.RenderEndTag(); //Render End - head
					
				}
		
				///<summary>
				///Creates the HTML markup for Sound Core's Header
				///</summary>
				private void CreatePageHeader(HtmlTextWriter PageMaker)
				{	
					//DON'T CHANGE THESE 
					string ScLogoPath = "../../images/scLogo.png";
					string TextLogoPath = "../../images/logo.png";
					string[] PhoneAddress = new string[]{"Retail", "Studio", "24 Hr"};
					string[] PhoneNumbers = new string[]{"(618) 457-0280", "(618) 519-9326","(618) 457-5641"};
					
					Dictionary<string,string> StoreHours = new Dictionary<string,string>();
					StoreHours.Add("Mon - Fri", "10am - 7pm");
					StoreHours.Add("Saturday","10am - 6pm");
					StoreHours.Add("Sunday", "10am - 6pm");
					
				
					//Add the Main Icon
					PageMaker.RenderBeginTag("header");
					PageMaker.AddAttribute("id","scLogo");
					PageMaker.AddAttribute("src",ScLogoPath);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Img);
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					
					//Add the Text Logo 
					PageMaker.AddAttribute("id","Logo");
					PageMaker.AddAttribute("src", TextLogoPath);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Img);
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					
					//Create the Hours Table
					PageMaker.AddAttribute("id","phone");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
					PageMaker.AddAttribute("id","phoneNum");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Table);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Tbody);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Tr);
					PageMaker.AddAttribute("colspan","2");
					PageMaker.AddAttribute("text-align","center");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Td);
					PageMaker.Write("PHONE");
					PageMaker.RenderEndTag(); //End Render for td
					PageMaker.RenderEndTag(); //End Render for tr
					
					for(int x = 0; x < 3; x++)
					{
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Tr);
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Td);
						PageMaker.Write(PhoneAddress[x]);
						PageMaker.RenderEndTag();
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Td);
						PageMaker.Write(PhoneNumbers[x]);
						PageMaker.RenderEndTag();
						PageMaker.RenderEndTag();
					}
					
					PageMaker.RenderEndTag(); // End Render for tbody
					PageMaker.RenderEndTag(); //End Render for Table
					PageMaker.RenderEndTag(); // End Render for the div element containing the table
					PageMaker.WriteLine();
					
					//Create the Phone Number Table
					PageMaker.AddAttribute("id","storeHours");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);   // Begin Div Tag
					PageMaker.AddAttribute("id","Hours");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Table); // Begin Table Tag
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Tbody); // Begin Tbody Tag
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Tr);    // Begin TR tag
					PageMaker.AddAttribute("colspan", "2");
					PageMaker.AddAttribute("height","30px");
					PageMaker.AddAttribute("border-radius","15px");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Td);   //Begin Td tag
					PageMaker.Write("HOURS");
					PageMaker.RenderEndTag(); // End Render for td
					PageMaker.RenderEndTag(); // End Render for tr
					
					foreach(KeyValuePair<string,string> HoursPair in StoreHours)
					{
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Tr);
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Td);
						PageMaker.Write(HoursPair.Key);
						PageMaker.RenderEndTag();
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Td);
						PageMaker.Write(HoursPair.Value);
						PageMaker.RenderEndTag();
						PageMaker.RenderEndTag();
						PageMaker.WriteLine();
					}
					
					PageMaker.RenderEndTag(); // End Render for tbody
					PageMaker.RenderEndTag(); // End Render for Table			
					PageMaker.RenderEndTag(); //End Render for containing div
					PageMaker.RenderEndTag(); //End Render for header tag
					PageMaker.WriteLine();
				}
		
				///<summary>
				///Creates the HTML markup for Sound Core's Navigation Bar
				///</summary>
				private void CreatePageNav(HtmlTextWriter PageMaker)
				{
					string HomePage = "../../Index.php";
					Dictionary<string,string> ServicePages = new Dictionary<string,string>();
					Dictionary<string,string> RentalPages = new Dictionary<string,string>();
					Dictionary<string,string> ProdPages = new Dictionary<string,string>();
					Dictionary<string,string> StorePages = new Dictionary<string,string>();
					PageMaker.Indent++;
					PageMaker.RenderBeginTag("nav");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Ul);
					//The process needs to made into a seperate assembly for the future. 
					for(int x = 0; x < 5; x++)
					{
						PageMaker.WriteLine();
						
						if(x == 4){
							PageMaker.AddAttribute("id","active");
							PageMaker.AddAttribute("class","active");
						}
					
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Li); //Begin the LI tag
						
						switch(x)
						{
							case 0:
								PageMaker.AddAttribute("href",HomePage);
								PageMaker.WriteLine();
								PageMaker.Indent++;
								PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
								PageMaker.Write("Home");
								PageMaker.Indent--;
								PageMaker.RenderEndTag();
								PageMaker.WriteLine();
								break;
							case 1:
								PageMaker.AddAttribute("href","../../Services/Services.php");
								PageMaker.WriteLine();
								PageMaker.Indent++;
								PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
								PageMaker.Write("Services");
								PageMaker.RenderEndTag();
								PageMaker.WriteLine();
								PageMaker.AddAttribute("class","dropdown");
								PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
								ServicePages.Add("Audio & Video Transfers","../../Services/Audio-Video-Transfers.php");
								ServicePages.Add("Repairs & Set Up","../../Services/Repairs.php");
								ServicePages.Add("Audio & Video Installs","../../Services/Audio-Video-Installations.php");
								ServicePages.Add("DJ Services","../../Services/DJ_Services.php");

								foreach(KeyValuePair<string,string> Link in ServicePages)
								{
									PageMaker.AddAttribute("href",Link.Value);		
									PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
									PageMaker.Write(Link.Key);
									PageMaker.RenderEndTag();
									PageMaker.WriteLine();
								}
								
								PageMaker.RenderEndTag(); // Render End tag
								PageMaker.Indent--;
								PageMaker.WriteLine();	
								break;
							case 2:
								PageMaker.AddAttribute("href","../../Rentals/Rentals.php");
								PageMaker.WriteLine();
								PageMaker.Indent++;
								PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
								PageMaker.Write("Rentals");
								PageMaker.RenderEndTag();
								PageMaker.WriteLine();
								PageMaker.AddAttribute("class","dropdown");
								PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
								RentalPages.Add("PA & Live Sound","../../Rentals/PA-System-Rentals.php");
								RentalPages.Add("Video Projects","../../Rentals/Video-Projector-Rentals.php");
								RentalPages.Add("Lights","../../Rentals/Light-Rentals.php");
								RentalPages.Add("Staging & Trussing","../../Rentals/Stage-Truss-Rentals.php");
								
								foreach(KeyValuePair<string,string> Link in RentalPages)
								{
									PageMaker.AddAttribute("href",Link.Value);		
									PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
									PageMaker.Write(Link.Key);
									PageMaker.RenderEndTag();
									PageMaker.WriteLine();
								}
								
								PageMaker.RenderEndTag();
								PageMaker.Indent--;
								PageMaker.WriteLine();
								break;
							case 3:
								PageMaker.AddAttribute("href","../../Productions/Productions.php");
								PageMaker.WriteLine();
								PageMaker.Indent++;
								PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
								PageMaker.Write("Productions");
								PageMaker.RenderEndTag();
								PageMaker.WriteLine();
								PageMaker.AddAttribute("class","dropdown");
								PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
								ProdPages.Add("Video & Film","../../Productions/Film-Video-Production.php");
								ProdPages.Add("Audio","../../Productions/Studio-Production.php");
								
								foreach(KeyValuePair<string,string> Link in ProdPages)
								{
									PageMaker.AddAttribute("href",Link.Value);		
									PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
									PageMaker.Write(Link.Key);
									PageMaker.RenderEndTag();
									PageMaker.WriteLine();
								}
								PageMaker.RenderEndTag();
								PageMaker.Indent--;
								PageMaker.WriteLine();
								
								break;
							case 4:
								PageMaker.AddAttribute("href","../Store.php");
								PageMaker.WriteLine();
								PageMaker.Indent++;
								PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
								PageMaker.Write("Store");
								PageMaker.RenderEndTag();
								PageMaker.Indent--;
								PageMaker.WriteLine();
								break;
						}
						PageMaker.RenderEndTag(); //End the LI tag
					}
					PageMaker.RenderEndTag(); //End the UL Tag
 					PageMaker.RenderEndTag(); //End the Nav Tag
					PageMaker.WriteLine();
				}
		
				///<summary>
				///Creates the HTML markup for Sound Core footer section.
				///</summary>
				private void CreatePageFooter(HtmlTextWriter PageMaker)
				{
					PageMaker.WriteLine();
					PageMaker.RenderBeginTag("footer");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Ul);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Li); // Begin Li tag
					PageMaker.AddAttribute("class","socialLink");
					PageMaker.AddAttribute("href","https://www.facebook.com/SoundCoure/");
					PageMaker.AddAttribute("id","facebook");
					PageMaker.WriteLine();
					PageMaker.RenderBeginTag(HtmlTextWriterTag.A); // Begin A Tag
					PageMaker.AddAttribute("class","social");
					PageMaker.AddAttribute("src","../../images/Facebook.png");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Img); // Begin Img Tag
					PageMaker.RenderEndTag(); // End Img Tag
					PageMaker.WriteLine("Like our page on facebook.");
					PageMaker.RenderEndTag(); // End A Tag
					PageMaker.RenderEndTag(); // End Li tag
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Li); //Begin New Li Tag
					PageMaker.AddAttribute("class","socialLink");	
					PageMaker.AddAttribute("href","https://www.reverb.com/shop/sound-core-music-and-video");
					PageMaker.AddAttribute("id","reverb");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.A); //Begin A Tag
					PageMaker.AddAttribute("class","social");
					PageMaker.AddAttribute("src","../../images/reverb.png");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Img); //Begin Img Tag
					PageMaker.RenderEndTag(); // End Img Tag
					PageMaker.Write("Check out our store at Reverb!");
					PageMaker.RenderEndTag(); // End A Tag
					PageMaker.RenderEndTag(); // End Li Tag
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Li); //Begin Li Tag
					PageMaker.AddAttribute("class","socialLink");	
					PageMaker.AddAttribute("href","https://www.twitter.com/SoundCoreSI");	
					PageMaker.AddAttribute("id","twitter");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.A);
					PageMaker.AddAttribute("class","social");
					PageMaker.AddAttribute("src","../../images/Twitter.png");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Img);
					PageMaker.WriteLine("Follow us on twitter!");
					PageMaker.RenderEndTag(); // End Img Tag
					PageMaker.RenderEndTag(); // End A Tag
					PageMaker.RenderEndTag(); // End Li Tag
					PageMaker.RenderEndTag(); // End UL Tag
					PageMaker.RenderEndTag(); // End footer 	
				}
				
				///<summary>
				///Creates the HTML Item Section
				///<summary>
				private void CreateItemSection(HtmlTextWriter PageMaker, XElement ItemInfo)
				{
					string ItemName = ItemInfo.Element("Manufacturer").Value.ToString() + " " + ItemInfo.Element("Model").Value.ToString();
					string ItemMDL = ItemInfo.Element("Model").Value.ToString();
					string ItemMFG = ItemInfo.Element("Manufacturer").Value.ToString();
					string ItemCategory =  ItemType.GetItemText(ItemType.SelectedItem);
					string MainPhotoPath = "../Pictures";
					float TotalPrice = float.Parse(ItemInfo.Element("Price").Value) + float.Parse(ItemInfo.Element("ShippingCost").Value);
					float ShippingPrice = float.Parse(ItemInfo.Element("ShippingCost").Value);
					
					//string UploadedFileName = Path.GetFileName(ItemInfo.Element("ItemPics").FirstNode.ToString());
					
					//Get the relative path of item photos
					switch(ItemCategory){
						case "PA Equipment":
							MainPhotoPath = MainPhotoPath + "/PA Equipment/" + ItemMFG + "/" + ItemMDL + "/";
							break;
						case "Lighting":
							MainPhotoPath = MainPhotoPath + "/Lighting/" + ItemMFG + "/" + ItemMDL + "/"; 
							break;
						case "Percussion":
							MainPhotoPath = MainPhotoPath + "/Percussion/" + ItemMFG + "/" + ItemMDL + "/";
							break;
						default:
							MainPhotoPath = MainPhotoPath + "/" + ItemCategory + "s/" + ItemMFG + "/" + ItemMDL + "/";
							break;
					}
					
					
					
					PageMaker.WriteLine();
					PageMaker.RenderBeginTag("main"); //Begin main tag
					PageMaker.AddAttribute("id","ItemPictures");
					
					//Item Pictures Section
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div); // Begin Pictures Div
					
					PageMaker.AddAttribute("id","ZoomContainer");
					PageMaker.AddAttribute("onmousemove","zoom(event)");
					PageMaker.AddStyleAttribute("background-image",MainPhotoPath+"IMG0.jpg");
					PageMaker.AddStyleAttribute("background-size","200% 200%");
					PageMaker.RenderBeginTag("figure");
					
					PageMaker.AddAttribute("id","MainPhoto");
					PageMaker.AddAttribute("alt", ItemName);
					PageMaker.AddAttribute("src",MainPhotoPath+ItemName+" (Main).jpg");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Img);
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					PageMaker.RenderEndTag();
					PageMaker.AddAttribute("id","ThumbnailPics");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
					
					for(int x = 0; x < PhotoFilesList.Count; x++)
					{
						string IconName = Path.GetFileNameWithoutExtension(PhotoFilesList[x]) + " (thumbnail).jpg";
						
						PageMaker.AddAttribute("class","ThumbnailPic");
						PageMaker.AddAttribute("src",MainPhotoPath+IconName);
						PageMaker.AddAttribute("onclick", "SwitchPictures(" + x.ToString() + ")");
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Img);
						PageMaker.RenderEndTag();
						PageMaker.WriteLine();
					}
					
					PageMaker.RenderEndTag();
					PageMaker.Indent--;
					PageMaker.RenderEndTag(); // End Pictures Div

					//Item Information Section
					PageMaker.AddAttribute("id","ItemInfo");  //Begin ItemInfo Div
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
					PageMaker.AddAttribute("class","ItemName");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.H1);
					PageMaker.Write(ItemName);
					PageMaker.RenderEndTag();
					PageMaker.AddAttribute("class","ItemDesc");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.H3);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.U);
					PageMaker.Write("DESCRIPTION");
					PageMaker.RenderEndTag();
					PageMaker.RenderEndTag();
					PageMaker.RenderBeginTag(HtmlTextWriterTag.P);
					PageMaker.Write(ItemInfo.Element("Description").Value.ToString());
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					PageMaker.AddAttribute("id","costs");
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Div);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Span);
					PageMaker.Write("Retail Price........." + ItemInfo.Element("Price").Value.ToString());
					PageMaker.RenderEndTag();
					
					if(ShippingPrice == 0.00 || ShippingPrice == 0)
					{
						PageMaker.AddAttribute("class", "FreeShipping");
						PageMaker.RenderBeginTag(HtmlTextWriterTag.Span);
						PageMaker.Write("FREE SHIPPING");
					}
					else{
						PageMaker.Write("Shipping.........." + ItemInfo.Element("ShippingCost").Value.ToString());
					}
					
					PageMaker.RenderEndTag();
					PageMaker.RenderBeginTag(HtmlTextWriterTag.Span);
					PageMaker.RenderBeginTag(HtmlTextWriterTag.B);
					PageMaker.Write("Total Price.........." + TotalPrice.ToString());
					PageMaker.RenderEndTag();
					PageMaker.RenderEndTag();
					PageMaker.WriteLine();
					PageMaker.Write(HtmlTextWriter.TagLeftChar + "!--PAYPAL BUTTON CODE BELOW--" + HtmlTextWriter.TagRightChar);
					PageMaker.RenderEndTag();
					PageMaker.RenderEndTag();
					PageMaker.Indent--;
					PageMaker.RenderEndTag(); //End ItemInfo Div
					PageMaker.RenderEndTag(); //End main tag
				}
				

				
		
		}
		
		
		
		
		
	}

	

}